using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WindowMirror
{
    /// <summary>
    /// Interaction logic for PopOutViewer.xaml
    /// </summary>
    public partial class PopOutViewer : Window
    {
        public MainWindow mainWindow;

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

        }

        private void Window_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
