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
                OutputListBox.Items.Clear();
                var buckets = await _bucketOps.GetBucketNamesAsync();
                foreach (var name in buckets)
                {
                    OutputListBox.Items.Add(name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
