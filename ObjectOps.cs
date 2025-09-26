using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace _301289600Van_Lab1
{
    public class ObjectOps
    {
        private readonly IAmazonS3 s3Client;

        public ObjectOps(IAmazonS3 s3Client)
        {
            this.s3Client = s3Client;
        }

        //List Objects
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

        //Upload Obj
        public async Task UploadFileAsync(string bucketName, string filePath)
        {
            using (var transferUtility = new TransferUtility(s3Client))
            {

                string key = Path.GetFileName(filePath);
                await transferUtility.UploadAsync(filePath, bucketName, key);
            }
        }
        //detlete
        public async Task DeleteObjectAsync(string bucketName, string objectKey)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = objectKey
            };
            await s3Client.DeleteObjectAsync(deleteRequest);
        }
        //download
        public async Task DownloadFileAsync(string bucketName, string objectKey, string savePath)
        {
            var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = objectKey,
                FilePath = savePath 
            };

            using (var transferUtility = new TransferUtility(s3Client))
            {
                await transferUtility.DownloadAsync(downloadRequest);
            }
        }
    }
}
