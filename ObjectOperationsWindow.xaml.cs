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
    /// Interaction logic for ObjectOperationsWindow.xaml
    /// </summary>
    public partial class ObjectOperationsWindow : Window
    {
        private readonly BucketOps _bucketOps;

        public ObjectOperationsWindow()
        {
            InitializeComponent();
            var s3Client = Helper.s3Client;
            _bucketOps = new BucketOps(s3Client);
            this.Closed += ObjectOperationsWindow_Closed;
        }
        private void BackToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ObjectOperationsWindow_Closed(object? sender, EventArgs e)
        {
            Application.Current.MainWindow?.Show();
        }

        private async void BucketComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BucketComboBox.SelectedItem == null)
            {
                return;
            }

            try
            {
                string selectedBucket = BucketComboBox.SelectedItem.ToString();

                // 1. Get all objects for the selected bucket
                var objects = await _bucketOps.ListObjectsAsync(selectedBucket);

                // 2. Display the objects in the DataGrid
                ObjectDataGrid.ItemsSource = objects;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error listing objects: {ex.Message}", "Error");
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Get all buckets
                var buckets = await _bucketOps.GetBucketsWithDatesAsync();

                // ✨ ADD THIS LINE FOR DEBUGGING
                MessageBox.Show($"Found {buckets.Count} buckets.");

                // 2. Populate the ComboBox with bucket names
                BucketComboBox.ItemsSource = buckets.Select(b => b.BucketName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading buckets: {ex.Message}", "Error");
            }
        }
    }
}
