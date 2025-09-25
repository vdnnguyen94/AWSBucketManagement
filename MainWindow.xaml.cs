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
                // list buckets
                var buckets = await _bucketOps.GetBucketsWithDatesAsync();

                // display name in datagrid
                BucketDataGrid.ItemsSource = buckets;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error listing buckets: {ex.Message}");
            }
        }
        private async void CreateBucket_Click(object sender, RoutedEventArgs e)
        {
            // hget bucket name from textbox
            string newBucketName = BucketNameTextBox.Text.Trim().ToLower();

            // Basic validation
            if (string.IsNullOrWhiteSpace(newBucketName))
            {
                MessageBox.Show("Please enter a valid bucket name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var result = await _bucketOps.CreateBucketAsync(newBucketName);
                MessageBox.Show($"Bucket '{result}' created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                BucketNameTextBox.Clear(); // Clear the textbox 
                ListBuckets_Click(null, null); // Refresh the grid
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
