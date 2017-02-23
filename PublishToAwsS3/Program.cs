﻿using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System;
using System.IO;

namespace PublishToAwsS3
{
    class Program
    {
        private static string bucketName;
        private static string sourceDirectory;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter required arguments. Usage: PublishToAwsS3 <bucketName> <sourceDirectory>");
                return;
            }

            bucketName = args[0];
            sourceDirectory = args[1];

            CheckBucket();
        }

        private static void CheckBucket()
        {
            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.EUCentral1))
            {
                if (!(AmazonS3Util.DoesS3BucketExist(client, bucketName)))
                {
                    CreateBucket(client);
                }

                // Upload files.
                UploadFiles(client);
                //UploadFiles("F:\\Development\\FireDeptStopwatch\\FireDeptStopwatch\\deploy", bucketName, client);
            }
        }

        private static void CreateBucket(AmazonS3Client client)
        {
            try
            {
                PutBucketRequest putRequest1 = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };

                PutBucketResponse response1 = client.PutBucket(putRequest1);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                    Console.WriteLine("For service sign up go to http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("Error occurred. Message:'{0}' when writing an object", amazonS3Exception.Message);
                }
            }
        }

        private static void UploadFiles(IAmazonS3 client)
        {
            try
            {
                TransferUtility transferUtility = new TransferUtility(client);
                var filePaths = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);

                foreach (var filePath in filePaths)
                {
                    var lastWrite = File.GetLastWriteTime(filePath).ToUniversalTime();
                    var key = filePath.Replace(sourceDirectory, @"");
                    if (key.StartsWith(@"\"))
                        key = key.Substring(1);
                    key = key.Replace(@"\", @"/");

                    bool fileExists = false, fileNewer = false;

                    var fileInfo = new S3FileInfo(client, bucketName, key);
                    if ((fileExists = fileInfo.Exists))
                    {
                        var getObjectMetadataRequest = new GetObjectMetadataRequest
                        {
                            BucketName = bucketName,
                            Key = key
                        };

                        var getObjectMetadataResponse = client.GetObjectMetadata(getObjectMetadataRequest);

                        fileNewer = lastWrite > getObjectMetadataResponse.LastModified;
                    }

                    if (!fileExists || fileNewer)
                    {
                         var uploadRequest = new TransferUtilityUploadRequest
                        {
                            BucketName = bucketName,
                            FilePath = filePath,
                            Key = key,
                            CannedACL = S3CannedACL.PublicRead
                        };

                        uploadRequest.UploadProgressEvent +=
                            new EventHandler<UploadProgressArgs>(UploadRequest_UploadPartProgressEvent);

                        transferUtility.Upload(uploadRequest);
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                    Console.WriteLine("For service sign up go to http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("Error occurred. Message:'{0}' when writing an object", amazonS3Exception.Message);
                }
            }
        }

        private static void UploadRequest_UploadPartProgressEvent(object sender, UploadProgressArgs e)
        {
            Console.WriteLine(
                "Tranfering {0} / {1} KB/s, current file: {2}",
                e.TransferredBytes / 1024,
                e.TotalBytes / 1024,
                e.FilePath
            );
        }
    }
}
