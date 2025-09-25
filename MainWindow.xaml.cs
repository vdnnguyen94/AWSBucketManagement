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

namespace _301289600Van_Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //menu
        public MainWindow()
        {
            InitializeComponent(); 
        }
        //event for bucket management
        private void BucketOperationsButton_Click(object sender, RoutedEventArgs e)
        {
            // bucket operation
            var bucketWindow = new BucketOperationsWindow();
            // close menu
            this.Hide(); 
            bucketWindow.Show();
        }
        //vent for obj management
        private void ObjectOperationsButton_Click(object sender, RoutedEventArgs e)
        {
            //object operation
            MessageBox.Show("Object Level Operations will be implemented next!", "Coming Soon");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

