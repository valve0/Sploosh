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
using Sploosh.ViewModels;

namespace Sploosh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;


            //for (int i = 0; i < 64; i++)
            //{
            //    Button btn = new Button();
            //    btn.Style = (Style)Resources["gridButton"];
               
            //    squidGrid.Children.Add(btn);

            //    btn.Content = $"{i}";

            //    btn.Command = mainViewModel.GridClick;
            //    btn.CommandParameter = btn.Content;

            //    //Row calculator
            //    int rowNumber = i/8;
            //    //Column Calculator
            //    int colNumber = i%8;

            //    //squidGrid.Children.Add(btn);
            //    Grid.SetRow(btn, rowNumber);

            //    Grid.SetColumn(btn, colNumber);



            //}

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void ShowSettingsWindow(object sender, RoutedEventArgs e)
        {
            
                var mainWindow = sender as Window;
                SettingsWindow settingsWindow = new SettingsWindow();
                settingsWindow.Owner = mainWindow;
                settingsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settingsWindow.ShowDialog();
        }


    }
}
