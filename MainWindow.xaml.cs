using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowMirror.Display;

namespace WindowMirror
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Process> windows = new ObservableCollection<Process>();
        private ObservableCollection<Monitor> displays = new ObservableCollection<Monitor>();
        private Timer errorTimer = new Timer();

        PopOutViewer viewerWindow;

        public MainWindow()
        {
            InitializeComponent();

            //  Make sure the error panel defaults to hidden
            errorPanel.Visibility = Visibility.Collapsed;

            //  Configure the error panel timer
            errorTimer.AutoReset = false;
            errorTimer.Elapsed += ErrorTimeEvent;

            //  Populate the list of windows valid for capture
            UpdateWindowsList();

            //  Populate the list of displays valid for capture
            UpdateDisplaysList();
        }

        private void popOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewerWindow != null) viewerWindow.Close();

            viewerWindow = new PopOutViewer();
            viewerWindow.Show();
        }

        private void captureButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
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
                //displaysComboBox.IsEnabled = false;
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
