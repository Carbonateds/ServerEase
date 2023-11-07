using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace ServerEase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //If both height and width in the configuration file are set to 0, consider the window as maximized.
            if (Library.GetMainWindowHeight() == 0.0D && Library.GetMainWindowWidth() == 0.0D)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                Height = Library.GetMainWindowHeight();
                Width = Library.GetMainWindowWidth();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            switch(WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
                default:
                    WindowState = WindowState.Normal;
                    break;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    BorderThickness = new Thickness(0);
                    MaximizeButton.Content = "\uE922";
                    break;
                default:
                    BorderThickness = new Thickness(8);
                    MaximizeButton.Content = "\uE923";
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //If the windows is maximized, then set both height and width to 0.
            if (WindowState == WindowState.Maximized)
                Library.SetMainWindowSize(0, 0);
            else
                Library.SetMainWindowSize(Height, Width);
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            new SettingWindow() { Owner = this }.Show();
        }
    }
}
