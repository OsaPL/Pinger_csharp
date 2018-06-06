using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConfigFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Config cfg;
       
        public MainWindow()
        {
            InitializeComponent();

            cfg = new Config();
            loadedNamesList.ItemsSource = cfg.Values;
            cfg.Defaults = new Config();
            loadedResetNamesList.ItemsSource = cfg.Defaults.Values;

            cfg.SafeAdd(new Field("NameString", "string"));
            cfg.SafeAdd(new Field("NameInt", 1));
            cfg.SafeAdd(new Field("NameDouble", 1.2));
            cfg.Defaults.SafeAdd(new Field());
        }

        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var field = ((ListViewItem)sender).Content as Field; //Casting back to the binded
        }

        private void changeViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (loadedNamesList.IsVisible)
            {
                loadedResetNamesList.Visibility = Visibility.Visible;
                loadedNamesList.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadedResetNamesList.Visibility = Visibility.Collapsed;
                loadedNamesList.Visibility = Visibility.Visible;
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //If you wanna just push it, even when it can be converted
                //cfg.Values.Add(new Field(textBox.Text));
                cfg.SafeAdd(new Field(textBox.Text));
                loadedNamesList.Items.Refresh();
            }
        }

        private void resetFileButton_Click(object sender, RoutedEventArgs e)
        {
            cfg.ResetToDefaults();
            loadedNamesList.Items.Refresh();
            loadedResetNamesList.Items.Refresh();
        }
    }
}
