using System;
using System.Windows;
using Amazon.S3;
using _301289600Van_Lab1;

namespace _301289600Van_Lab1
{
    public partial class MainWindow : Window
    {
        private readonly BucketOps _bucketOps;

        public MainWindow()
        {
            InitializeComponent();

            // Set up S3 client and operations class
            var s3Client = Helper.s3Client;
            _bucketOps = new BucketOps(s3Client);
        }

        private async void ListBuckets_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of S3Bucket objects
                var buckets = await _bucketOps.GetBucketsWithDatesAsync();

                // Set the DataGrid's data source directly
                BucketDataGrid.ItemsSource = buckets;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error listing buckets: {ex.Message}");
            }
        }
        private async void CreateBucket_Click(object sender, RoutedEventArgs e)
        {
            string newBucketName = $"s3demo-{Guid.NewGuid().ToString().Substring(0, 8)}";

            try
            {
                var result = await _bucketOps.CreateBucketAsync(newBucketName);
                MessageBox.Show($"Bucket created: {result}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh the grid after creating
                ListBuckets_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating bucket: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
