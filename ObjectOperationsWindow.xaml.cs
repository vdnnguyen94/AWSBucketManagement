using Amazon.S3.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly ObjectOps _objectOps;

        public ObjectOperationsWindow()
        {
            InitializeComponent();
            var s3Client = Helper.s3Client;
            _bucketOps = new BucketOps(s3Client);
            _objectOps = new ObjectOps(s3Client);
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
        //List items in Bucket
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }
        //Add item or upload button
        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            // check valid
            if (BucketComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select a bucket first.", "Validation Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(FilePathTextBox.Text))
            {
                MessageBox.Show("Please browse for a file to upload first.", "Validation Error");
                return;
            }

            string selectedBucket = BucketComboBox.SelectedItem.ToString();
            string filePath = FilePathTextBox.Text;
            try
            {
               
                await _objectOps.UploadFileAsync(selectedBucket, filePath);
                MessageBox.Show($"File '{System.IO.Path.GetFileName(filePath)}' uploaded successfully to AWS CLOUD.", "Success");

                BucketComboBox_SelectionChanged(null, null);
                FilePathTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading file: {ex.Message}", "Upload Error");
            }
        }
        // delete button
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // check item selected or not
            if (ObjectDataGrid.SelectedItem is not S3Object selectedObject)
            {
                MessageBox.Show("Please select an object to delete.", "Selection Error");
                return;
            }

            // ensure bucket is valid
            if (BucketComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a bucket.", "Selection Error");
                return;
            }

            // confirm
            var confirmResult = MessageBox.Show(
                $"Are you sure you want to delete the object '{selectedObject.Key}'?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmResult == MessageBoxResult.No)
            {
                return; 
            }

            try
            {
                string bucketName = BucketComboBox.SelectedItem.ToString();

                await _objectOps.DeleteObjectAsync(bucketName, selectedObject.Key);

                MessageBox.Show($"Object '{selectedObject.Key}' was deleted successfully.", "Success");

                BucketComboBox_SelectionChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting object: {ex.Message}", "Delete Error");
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Get all buckets
                var buckets = await _bucketOps.GetBucketsWithDatesAsync();

                //  ADD THIS LINE FOR DEBUGGING
            //  MessageBox.Show($"Found {buckets.Count} buckets.");

                // 2. Populate the ComboBox with bucket names
                BucketComboBox.ItemsSource = buckets.Select(b => b.BucketName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading buckets: {ex.Message}", "Error");
            }
        }

        //download button
        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
 
            if (ObjectDataGrid.SelectedItem is not S3Object selectedObject)
            {
                MessageBox.Show("Please select an object to download.", "Selection Error");
                return;
            }


            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = selectedObject.Key;


            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string bucketName = BucketComboBox.SelectedItem.ToString();
                    string savePath = saveFileDialog.FileName;
                    await _objectOps.DownloadFileAsync(bucketName, selectedObject.Key, savePath);

                    MessageBox.Show($"File '{selectedObject.Key}' downloaded successfully to:\n{savePath}", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error downloading file: {ex.Message}", "Download Error");
                }
            }
        }
    }
}
