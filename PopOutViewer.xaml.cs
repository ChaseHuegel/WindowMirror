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
        public MainWindow mainWindow;
        public BitmapSource bitmapSource;

        public PopOutViewer(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            InitializeComponent();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            //  Close the app if the main window is hidden
            if (mainWindow != null && !mainWindow.IsVisible)
                Application.Current.Shutdown();

            this.Close();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null && !mainWindow.IsVisible)
                mainWindow.Show();

            this.Hide();
        }

        private void captureButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.windowsComboBox.SelectedIndex == -1) return;

            Process process = (Process)mainWindow.windowsComboBox.SelectedItem;

            if (process != null)
            {
                IntPtr hwnd = process.MainWindowHandle;

                viewerImage.Source = Capture.Snapshot(hwnd, 0, 0, (int)this.Width, (int)this.Height);

                //  Make sure the image size matches the window size
                viewerImage.Width = this.Width;
                viewerImage.Height = this.Height;
            }
        }

        private void Window_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
