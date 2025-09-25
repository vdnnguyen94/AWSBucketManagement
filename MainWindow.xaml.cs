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

        private async void DeleteBucket_Click(object sender, RoutedEventArgs e)
        {
            // get selected data from the grid
            if (BucketDataGrid.SelectedItem is not S3Bucket selectedBucket || selectedBucket.BucketName == null)
            {
                MessageBox.Show("Please select a bucket to delete.", "Selection Error");
                return;
            }

            try
            {
                // 2. Check if the bucket is empty
                var objects = await _bucketOps.ListObjectsAsync(selectedBucket.BucketName);

                if (objects.Any()) // Bucket is NOT empty
                {
                    // 3. Ask for user confirmation as required 
                    var confirmResult = MessageBox.Show(
                        $"The bucket '{selectedBucket.BucketName}' is not empty. Are you sure you want to delete all its contents and the bucket itself?",
                        "Confirm Deletion",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (confirmResult == MessageBoxResult.Yes)
                    {
                        // If confirmed, empty the bucket first, then the SDK call handles the final deletion.
                        await _bucketOps.EmptyBucketAsync(selectedBucket.BucketName);
                    }
                    else
                    {
                        // If user says no, do nothing.
                        return;
                    }
                }
                else // Bucket is empty
                {
                    // 4. If empty, delete it straight away.
                    await _bucketOps.DeleteBucketAsync(selectedBucket.BucketName);
                }

                MessageBox.Show($"Bucket '{selectedBucket.BucketName}' was deleted successfully.", "Success");

                // 5. Refresh the DataGrid to show the bucket is gone.
                ListBuckets_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting bucket: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
