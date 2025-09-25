using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace _301289600Van_Lab1
{
    public class BucketOps
    {
        //private static IAmazonS3 s3Client;

        //static async Task Main(string[] args)
        //{
        //    s3Client = new AmazonS3Client(GetBasicCredentials(), RegionEndpoint.USEast1);
        //    await CreateBucket(bucketName);

        //    // s3Client.DeleteBucketAsync(bucketName).Wait();
        //}
        private readonly IAmazonS3 s3Client;

        public BucketOps(IAmazonS3 client)
        {
            s3Client = client;
        }

        public async Task<List<string>> GetBucketListAsync()
        {
            var response = await s3Client.ListBucketsAsync();
            var bucketInfoList = new List<string>();

            foreach (var bucket in response.Buckets)
            {
                bucketInfoList.Add($"{bucket.BucketName} ({bucket.CreationDate.ToShortDateString()})");
            }

            return bucketInfoList;
        }

        public async Task<string> CreateBucketAsync(string bucketName)
        {
            try
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };

                var response = await s3Client.PutBucketAsync(request);

                return $"Bucket created: {bucketName} (Request ID: {response.ResponseMetadata.RequestId})";
            }
            catch (AmazonS3Exception ex)
            {
                return $"S3 Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"General Error: {ex.Message}";
            }
        }
    }
}
