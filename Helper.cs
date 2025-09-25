using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon;
using DotNetEnv;

namespace _301289600Van_Lab1
{
    public static class Helper
    {
        public static readonly IAmazonS3 s3Client;

        static Helper()
        {
            Env.Load(); // load .env file
            string? accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            string? secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            string? region = Environment.GetEnvironmentVariable("AWS_REGION");

            // Validate manually if you want
            if (string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey))
                throw new Exception("Missing AWS credentials in .env");

            var regionEndpoint = RegionEndpoint.GetBySystemName(region ?? "us-east-1");

            s3Client = new AmazonS3Client(accessKey!, secretKey!, regionEndpoint);

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

