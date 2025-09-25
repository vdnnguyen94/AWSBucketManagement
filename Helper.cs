using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon;


namespace _301289600Van_Lab1
{
    public static class Helper
    {
        public static readonly IAmazonS3 s3Client;

        static Helper()
        {
            s3Client = new AmazonS3Client(); // Loads credentials from default chain
        }
    }
}

   

    //private static BasicAWSCredentials GetBasicCredentials()
    //{
    //    var builder = new ConfigurationBuilder()
    //                            .SetBasePath(Directory.GetCurrentDirectory())
    //                            .AddJsonFile("AppSettings.json");

    //    var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
    //    var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

    //    return new BasicAWSCredentials(accessKeyID, secretKey);

    //}

