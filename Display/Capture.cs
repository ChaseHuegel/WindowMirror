using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using SwapChain1 = SharpDX.DXGI.SwapChain1;
using MapFlags = SharpDX.Direct3D11.MapFlags;

using Swordfish;
using Swordfish.Threading;
using System.Timers;
using static WindowMirror.Display.GDI32;

namespace WindowMirror.Display
{
    public class Capture
    {
        public event EventHandler<FrameReadyEvent> OnFrameReadyEvent;
        public class FrameReadyEvent : Event
        {
            public BitmapSource bitmap;
        }

        protected MainWindow main;
        protected ThreadWorker thread;
        protected Timer timer;

        protected BitmapSource bitmapSrc;

        public bool Capturing { get { return capturing; } }
        protected bool capturing = false;

        public int FPS = 30;    //  Target framerate

        public int x = 0;
        public int y = 0;
        protected int width = 0;
        protected int height = 0;

        protected Process process;
        protected Monitor display;

        public Capture(MainWindow main)
        {
            this.main = main;

            //thread = new ThreadWorker(CaptureShot);

            //timer = new Timer(1000 / FPS);
            //timer.Elapsed += FrameReady;
            //timer.AutoReset = true;
            //timer.Enabled = false;
        }

        public Process GetTargetWindow() { return process; }
        public Monitor GetTargetDisplay() { return display; }

        public void SetTargets(Process process, Monitor display)
        {
            this.process = process;
            this.display = display;
        }

        public void Stop() { capturing = true; thread.Stop(); }
        public void Start() 
        {
            capturing = true;
            thread.Start();
        }

        public void FrameReady(object sender, ElapsedEventArgs e)
        {
            CaptureShot();
        }

        public void CaptureShot()
        {
            if (process != null)
            {
                RECT rect;
                GetWindowRect(process.MainWindowHandle, out rect);

                //  Capture the whole window
                width = rect.Right - rect.Left;
                height = rect.Bottom - rect.Top;

                bitmapSrc = Capture.SnapshotWindow(process.MainWindowHandle, x, y, width, height);
            }
            else if (display != null)
            {
                //  Capture whole display
                width = (int)display.ScreenSize.X;
                height = (int)display.ScreenSize.Y;

                bitmapSrc = Capture.SnapshotDisplay(display, x, y, width, height);
            }

            if (bitmapSrc == null)
                return;

            //  Invoke a frame event
            FrameReadyEvent e = new FrameReadyEvent { bitmap = bitmapSrc };
            OnFrameReadyEvent?.Invoke(this, e);

            //  Throttle the thread based on target FPS
            System.Threading.Thread.Sleep( 1000 / FPS );
        }

        public static BitmapSource SnapshotDX(IntPtr hwnd, int x, int y, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // graphics card adapter
            const int numAdapter = 0;

            // output device
            const int numOutput = 0;

            // Create factory
            Factory1 factory = new Factory1();

            // Create device from Adapter
            Adapter1 adapter = factory.GetAdapter1(numAdapter);
            Device device = new Device(adapter);

            //  Create outputs
            Output output = adapter.GetOutput(numOutput);
            Output1 output1 = output.QueryInterface<Output1>();

            // Duplicate the output
            OutputDuplication duplicatedOutput = output1.DuplicateOutput(device);

            var textureDesc = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = { Count = 1, Quality = 0 },
                Usage = ResourceUsage.Staging
            };

            Texture2D screenTexture = new Texture2D(device, textureDesc);

            //  Convert bitmap to bitmapsource
            BitmapSource result = Imaging.CreateBitmapSourceFromHBitmap
            (
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            return result;
        }

        public static BitmapSource SnapshotWindow(IntPtr hwnd, int x, int y, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //  Pull an image from the target handle
            using (Graphics fromHwnd = Graphics.FromHwnd(hwnd))
            using (Graphics gfx = Graphics.FromImage(bitmap))
            {
                IntPtr hdcHwnd = fromHwnd.GetHdc();
                IntPtr hdcGfx = gfx.GetHdc();

                //  Copy pixels from our hwnd to the bitmap (gfx)
                BitBlt(hdcGfx, 0, 0, width, height, hdcHwnd, x, y, TernaryRasterOperations.SRCCOPY);

                //  Release the hdc
                gfx.ReleaseHdc();
                fromHwnd.ReleaseHdc();
            }

            BitmapSource result = Imaging.CreateBitmapSourceFromHBitmap
                (
                    bitmap.GetHbitmap(), 
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty, 
                    BitmapSizeOptions.FromEmptyOptions()
                );

            return result;
        }

        public static BitmapSource SnapshotDisplay(Monitor monitor, int x, int y, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            x += (int)monitor.WorkArea.TopLeft.X;
            y += (int)monitor.WorkArea.TopLeft.Y;

            //  Pull an image from the target handle
            using (Graphics gfx = Graphics.FromImage(bitmap))
            {
                gfx.CopyFromScreen(x, y, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            }

            BitmapSource result = Imaging.CreateBitmapSourceFromHBitmap
                (
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                );

            return result;
        }
    }
}
