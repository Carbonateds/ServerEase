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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerEase.SettingPage
{
    /// <summary>
    /// Interaction logic for GeneralPage.xaml
    /// </summary>
    public partial class GeneralPage : Page
    {
        SettingWindow parent;
        public GeneralPage(SettingWindow owner)
        {
            InitializeComponent();
            parent = owner;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            switch (parent.configuration.Lang)
            {
                case "en":
                    LanguageComboBox.SelectedIndex = 0;
                    break;
                case "zh-CN":
                    LanguageComboBox.SelectedIndex = 1;
                    break;
                case "zh-TR":
                    LanguageComboBox.SelectedIndex = 2;
                    break;
                case "de":
                    LanguageComboBox.SelectedIndex = 3;
                    break;
                case "ru":
                    LanguageComboBox.SelectedIndex = 4;
                    break;
                case "fr":
                    LanguageComboBox.SelectedIndex = 5;
                    break;
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (LanguageComboBox.SelectedIndex)
            {
                case 0:
                    parent.configuration.Lang = "en";
                    break;
                case 1:
                    parent.configuration.Lang = "zh-CN";
                    break;
                case 2:
                    parent.configuration.Lang = "zh-TR";
                    break;
                case 3:
                    parent.configuration.Lang = "de";
                    break;
                case 4:
                    parent.configuration.Lang = "ru";
                    break;
                case 5:
                    parent.configuration.Lang = "fr";
                    break;
            }
        }
    }
}
