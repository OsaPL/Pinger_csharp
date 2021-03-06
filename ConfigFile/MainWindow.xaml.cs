﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


            ///Testing values adding, if any of them wont show up in window, somethings not okay. Probably conversion.
            //Simple types
            cfg.SafeAdd(new Field("NameString", "string"));
            cfg.SafeAdd(new Field("NameInt", 1));
            cfg.SafeAdd(new Field("NameDouble", 1.2));
            //Not mscorlib types
            cfg.SafeAdd(new Field("NameColor", Color.FromArgb(10, 20, 30, 40)));
            //My own class
            cfg.SafeAdd(new Field("NameExample", new ExampleClass()));
            //TODO: More types to check?


            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += RefreshEncryptState;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            encryptButton.Background = new SolidColorBrush(Colors.Red);
            ignoreNotSecureButton.Background = new SolidColorBrush(Colors.Red);
        }

        private void RefreshEncryptState(object sender, EventArgs e)
        {
            if (cfg.Secure)
            {
                encryptButton.Background = new SolidColorBrush(Colors.Green);
            }
            else
            {
                encryptButton.Background = new SolidColorBrush(Colors.Red);
            }
        }

        void GenerateRndData()
        {
            //TODO: Do some random data generation to do some automated testing
        }

        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var field = ((ListViewItem)sender).Content as Field; //Casting back to the binded
            loadedNamesList.Items.Refresh();
            loadedResetNamesList.Items.Refresh();
            textBox.Text = field.ToString();
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
            if (e.Key == Key.Enter)
            {
                //If you wanna just push it, even when it cant be converted
                //cfg.Values.Add(new Field(textBox.Text));
                if (cfg.SafeAdd(new Field(textBox.Text)))
                    loadedNamesList.Items.Refresh();
            }
        }

        private void resetFileButton_Click(object sender, RoutedEventArgs e)
        {
            cfg.ResetToDefaults();
            loadedNamesList.ItemsSource = null;
            loadedNamesList.ItemsSource = cfg.Values;
            //loadedResetNamesList.Items.Refresh();
        }

        private void saveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                //cfg.cfgPath = saveFileDialog.FileName;
                //cfg.SaveCfg();
                //Or better yet we do something like this
                cfg.SaveCfg(saveFileDialog.FileName);
            }

        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (loadedResetNamesList.IsVisible)
            {
                loadedResetNamesList.Visibility = Visibility.Collapsed;
                loadedNamesList.Visibility = Visibility.Visible;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //cfg.cfgPath = openFileDialog.FileName;
                //cfg.LoadCfg();
                //Or better yet we do something like this
                cfg.LoadCfg(openFileDialog.FileName);
                loadedNamesList.Items.Refresh();
            }
        }
        private void openResetFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (loadedNamesList.IsVisible)
            {
                loadedResetNamesList.Visibility = Visibility.Visible;
                loadedNamesList.Visibility = Visibility.Collapsed;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //cfg.cfgPath = saveFileDialog.FileName;
                //cfg.SaveCfg();
                //Or better yet we do something like this
                cfg.Defaults.LoadCfg(openFileDialog.FileName);
                loadedResetNamesList.Items.Refresh();
            }
        }

        private void checkIfValidButton_Click(object sender, RoutedEventArgs e)
        {
            if (cfg.CheckForMissingVars(true))
            {
                checkIfValidButton.Background = new SolidColorBrush(Colors.Green);
            }
            else
            {
                checkIfValidButton.Background = new SolidColorBrush(Colors.Red);
            }
            loadedNamesList.Items.Refresh();
        }
        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (cfg.Secure)
            {
                cfg.Secure = false;
                encryptButton.Background = new SolidColorBrush(Colors.Red);
            }
            else
            {
                cfg.Secure = true;
                encryptButton.Background = new SolidColorBrush(Colors.Green);
            }

        }



        private void ignoreNotSecureButton_Click(object sender, RoutedEventArgs e)
        {
            if (cfg.IgnoreIfNonSecure)
            {
                cfg.IgnoreIfNonSecure = false;
                ignoreNotSecureButton.Background = new SolidColorBrush(Colors.Red);
            }
            else
            {
                cfg.IgnoreIfNonSecure = true;
                ignoreNotSecureButton.Background = new SolidColorBrush(Colors.Green);
            }
        }

        private void encryptButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(cfg.ExceptionStack.ToString(), "Config stack");
        }
    }

    public class ExampleClass
    {
        int i;
        IPAddress Ip;
        public ExampleClass()
        {
            i = 0;
            Ip = new IPAddress(12345);
        }

        public override string ToString()
        {
            return i + "  " + Ip;
        }
    }
}
