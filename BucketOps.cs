using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<S3Bucket>> GetBucketsWithDatesAsync()
        {
            var response = await s3Client.ListBucketsAsync();
            // This now creates a list of S3Bucket objects
            return response.Buckets
                .Select(b => new S3Bucket { BucketName = b.BucketName, CreationDate = b.CreationDate })
                .ToList();
        }

        // create bucket
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
        // listing names of buckets
        public async Task<List<S3Object>> ListObjectsAsync(string bucketName)
        {
            var objectList = new List<S3Object>();
            var request = new ListObjectsV2Request { BucketName = bucketName };
            ListObjectsV2Response response;
            bool checkIsTruncated;
            do
            {
                response = await s3Client.ListObjectsV2Async(request);
                if (response.S3Objects != null)
                {
                    objectList.AddRange(response.S3Objects.Select(o => new S3Object { Key = o.Key, Size = o.Size }));
                }
                request.ContinuationToken = response.NextContinuationToken;
                checkIsTruncated = (bool)response.IsTruncated;
            }
            while (checkIsTruncated);

            return objectList; 
        }

        // delete objs in the bucket
        public async Task EmptyBucketAsync(string bucketName)
        {
            await AmazonS3Util.DeleteS3BucketWithObjectsAsync(s3Client, bucketName);
        }

        public async Task DeleteBucketAsync(string bucketName)
        {
            await s3Client.DeleteBucketAsync(bucketName);
        }
    }
}
