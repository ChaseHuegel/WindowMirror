using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WindowMirror.Display;
using static WindowMirror.Display.Capture;
using static WindowMirror.Display.GDI32;

namespace WindowMirror
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Process> windows = new ObservableCollection<Process>();
        private ObservableCollection<Monitor> displays = new ObservableCollection<Monitor>();

        private Capture capture;
        private Timer captureTimer;

        private PopOutViewer viewerWindow;
        private Timer errorTimer;

        private BitmapSource bitmapSrc;

        public MainWindow()
        {
            InitializeComponent();

            //  Create a capture tool
            capture = new Capture(this);
            //capture.OnFrameReadyEvent += OnFrameReady;

            //  TODO: proper multithreading for capturing, GDI seems to be nonfunctional on side threads
            //  Using a timer for now
            captureTimer = new Timer(1000 / 30);    //  30 FPS
            captureTimer.Elapsed += FrameReadyTimer;
            captureTimer.AutoReset = true;
            captureTimer.Enabled = false;

            //  Make sure the error panel defaults to hidden
            errorPanel.Visibility = Visibility.Collapsed;

            //  Configure the error panel timer
            errorTimer = new Timer();
            errorTimer.AutoReset = false;
            errorTimer.Elapsed += ErrorTimeEvent;

            //  Populate the list of windows valid for capture
            UpdateWindowsList();

            //  Populate the list of displays valid for capture
            UpdateDisplaysList();
        }

        private void FrameReadyTimer(object sender, ElapsedEventArgs e)
        {
            //  form control is not thread safe, use a callback
            Action action = delegate () 
            {
                Process process = GetSelectedWindow();
                Monitor display = GetSelectedDisplay();

                if (process != null)
                {
                    RECT rect;
                    GetWindowRect(process.MainWindowHandle, out rect);

                    //  Capture the whole window
                    int width = rect.Right - rect.Left;
                    int height = rect.Bottom - rect.Top;

                    bitmapSrc = Capture.SnapshotWindow(process.MainWindowHandle, 0, 0, width, height);
                }
                else if (display != null)
                {
                    //  Capture whole display
                    int width = (int)(display.WorkArea.Right - display.WorkArea.Left);
                    int height = (int)(display.WorkArea.Bottom - display.WorkArea.Top);

                    bitmapSrc = Capture.SnapshotDisplay(display, 0, 0, width, height);
                }

                previewImage.Source = bitmapSrc;

                if (IsViewerActive())
                {
                    viewerWindow.viewerImage.Width = viewerWindow.Width;
                    viewerWindow.viewerImage.Height = viewerWindow.Height;
                    viewerWindow.viewerImage.Source = bitmapSrc;
                }
            };

            //  send the callback to the dispatcher
            this.Dispatcher?.Invoke(action);
        }

        private void OnFrameReady(Object sender, FrameReadyEvent e)
        {
            //  form control is not thread safe, use a callback
            Action action = delegate () {
                previewImage.Source = e.bitmap;

                if (IsViewerActive())
                    viewerWindow.viewerImage.Source = e.bitmap;
            };

            //  send the callback to the dispatcher
            this.Dispatcher.Invoke(action);
        }

        private void popOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewerWindow != null) viewerWindow.Close();

            viewerWindow = new PopOutViewer(this);
            viewerWindow.Show();
        }

        private void captureButton_Click(object sender, RoutedEventArgs e)
        {
            //capture.SetTargets(GetSelectedWindow(), GetSelectedDisplay());

            if (!capture.Capturing)
                captureTimer.Enabled = true;
            //capture.Start();
            else
                captureTimer.Enabled = false;
            //capture.Stop();

            //Process process = GetSelectedWindow();
            //Monitor display = GetSelectedDisplay();

            //if (process != null)
            //    previewImage.Source = Capture.SnapshotWindow(process.MainWindowHandle, 0, 0, (int)previewImage.Width, (int)previewImage.Height);
            //else if (display != null)
            //    previewImage.Source = Capture.SnapshotDisplay(display, 0, 0, (int)previewImage.Width, (int)previewImage.Height);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsViewerActive())
                this.Hide();
            else
                this.Close();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void windowsComboBox_Open(object sender, EventArgs e)
        {
            UpdateWindowsList();
        }

        private void displaysComboBox_Open(object sender, EventArgs e)
        {
            UpdateDisplaysList();
        }

        private void windowsComboBox_Select(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedItem != null)
            {
                displaysComboBox.SelectedIndex = -1;
            }
        }

        private void displaysComboBox_Select(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedItem != null)
            {
                windowsComboBox.SelectedIndex = -1;
            }
        }

        public bool IsViewerActive()
        {
            return (viewerWindow != null && viewerWindow.IsVisible);
        }

        public Process GetSelectedWindow()
        {
            Process process = null;

            if (windowsComboBox.SelectedIndex != -1)
                process = (Process)windowsComboBox.SelectedItem;

            return process;
        }

        public Monitor GetSelectedDisplay()
        {
            Monitor display = null;

            if (displaysComboBox.SelectedIndex != -1)
                display = (Monitor)displaysComboBox.SelectedItem;

            return display;
        }

        public void UpdateWindowsList()
        {
            //  Reset our list and populate it with valid windows
            windows.Clear();
            foreach (Process process in Process.GetProcesses())
            {
                //  If a window title is empty, it isn't valid for capture
                if (string.IsNullOrWhiteSpace(process.MainWindowTitle) == false)
                    windows.Add(process);
            }

            //  Fill combo box
            windowsComboBox.ItemsSource = windows;

            if (windows.Count == 0) ShowError("NO VALID WINDOWS", 3f);
        }

        public void UpdateDisplaysList()
        {
            //  Reset our list and populate it with valid displays
            displays.Clear();
            foreach (Monitor display in MonitorHelper.GetMonitors())
            {
                displays.Add(display);
            }

            //  Fill combo box
            displaysComboBox.ItemsSource = displays;

            //  Default to primary display if there are no others
            if (displays.Count == 1)
            {
                displaysComboBox.SelectedIndex = 0;
            }

            //  If you have no displays then you wouldn't see this anyways
            if (displays.Count == 0) ShowError("NO VALID DISPLAYS", 3f);
        }

        public void ShowError(string message, float duration)
        {
            errorPanel.Visibility = Visibility.Visible;

            errorTimer.Interval = duration * 1000;
            errorMessage.Content = message;

            errorTimer.Enabled = true;
        }

        public void ErrorTimeEvent(object sender, ElapsedEventArgs e)
        {
            //  form control is not thread safe, place code into a callback
            Action action = delegate () {
                if (errorTimer.Enabled == false)
                {
                    errorPanel.Visibility = Visibility.Collapsed;
                }
            };

            //  send the callback to the dispatcher on main thread
            this.Dispatcher.Invoke( action );
        }
    }
}
