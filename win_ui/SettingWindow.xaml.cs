using ServerEase.SettingPage;
using System;
using System.Windows;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace ServerEase
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        MainWindow parent;
        public Configuration configuration = new Configuration();

        public SettingWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            GeneralSettingButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0658AF"));
            MainFrame.Content = new GeneralPage(this);
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            configuration.Lang = Marshal.PtrToStringAnsi(Library.GetLanguage());
            parent = (MainWindow)Owner;
            parent.MainOverlayer.Visibility = Visibility.Visible;
            parent.MainOverlayer.IsHitTestVisible = true;
            parent.SizeChanged += Location_Change;
            parent.LocationChanged += Location_Change;
            parent.Closing += (sender1, e1) => { e1.Cancel = true; };
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            parent.MainOverlayer.Visibility = Visibility.Collapsed;
            parent.MainOverlayer.IsHitTestVisible = false;
            parent.SizeChanged -= Location_Change;
            parent.LocationChanged -= Location_Change;
            parent.Closing += (sender1, e1) => { e1.Cancel = false; };
        }

        private void Location_Change(object sender, EventArgs e)
        {
            if (parent != null)
            {
                if(parent.WindowState == WindowState.Maximized)
                {
                    Left = (SystemParameters.MaximizedPrimaryScreenWidth - Width) / 2;
                    Top = (SystemParameters.MaximizedPrimaryScreenHeight - Height) / 2;
                }
                else
                {
                    Left = parent.Left + (parent.ActualWidth - Width) / 2;
                    Top = parent.Top + (parent.ActualHeight - Height) / 2;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Library.SetLanguage(Marshal.StringToHGlobalAnsi(configuration.Lang));
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Panuon.WPF.UI;component/Control.xaml", UriKind.Absolute) });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Language\\" + Marshal.PtrToStringAnsi(Library.GetLanguage()) + ".xaml", UriKind.Relative) });

            if ((bool)FindResource("MachineTranslate"))
                MessageBox.Show(FindResource("MachineTranslateWarning").ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class Configuration
    {
        public string Lang { get; set; }
    }
}
