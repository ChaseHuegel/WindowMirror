using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowMirror.Display;

namespace WindowMirror
{
    /// <summary>
    /// Interaction logic for PopOutViewer.xaml
    /// </summary>
    public partial class PopOutViewer : Window
    {
        public MainWindow main;
        public BitmapSource bitmapSource;

        public PopOutViewer(MainWindow mainWindow)
        {
            this.main = mainWindow;

            InitializeComponent();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            //  Close the app if the main window is hidden
            if (main != null && !main.IsVisible)
                Application.Current.Shutdown();

            this.Close();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (main != null && !main.IsVisible)
                main.Show();

            this.Hide();
        }

        private void captureButton_Click(object sender, RoutedEventArgs e)
        {
            viewerImage.Width = this.Width;
            viewerImage.Height = this.Height;

            Process process = main.GetSelectedWindow();
            Monitor display = main.GetSelectedDisplay();

            if (process != null)
                viewerImage.Source = Capture.SnapshotWindow(process.MainWindowHandle, 0, 0, (int)viewerImage.Width, (int)viewerImage.Height);
            else if (display != null)
                viewerImage.Source = Capture.SnapshotDisplay(display, 0, 0, (int)viewerImage.Width, (int)viewerImage.Height);
        }

        private void Window_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
