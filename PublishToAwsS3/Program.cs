using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishToAwsS3
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckBucket("ssv-stoparica");
        }

        private static void CheckBucket(string bucketName)
        {
            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.EUCentral1))
            {
                if (!(AmazonS3Util.DoesS3BucketExist(client, bucketName)))
                {
                    CreateBucket(bucketName, client);
                }

                // Upload files.
                UploadFiles("F:\\Development\\FireDeptStopwatch\\FireDeptStopwatch\\deploy", bucketName, client);
            }
        }

        private static void CreateBucket(string bucketName, AmazonS3Client client)
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

        private static void UploadFiles(string directory, string bucketname, IAmazonS3 client)
        {
            try
            {
                TransferUtility transferUtility = new TransferUtility(client);
                var filePaths = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);

                foreach (var filePath in filePaths)
                {
                    var lastWrite = File.GetLastWriteTime(filePath).ToUniversalTime();
                    var key = filePath.Replace(directory, @"");
                    if (key.StartsWith(@"\"))
                        key = key.Substring(1);
                    key = key.Replace(@"\", @"/");

                    bool fileExists = false, fileNewer = false;

                    var fileInfo = new S3FileInfo(client, bucketname, key);
                    if ((fileExists = fileInfo.Exists))
                    {
                        var getObjectMetadataRequest = new GetObjectMetadataRequest
                        {
                            BucketName = bucketname,
                            Key = key
                        };

                        var getObjectMetadataResponse = client.GetObjectMetadata(getObjectMetadataRequest);

                        fileNewer = lastWrite > getObjectMetadataResponse.LastModified;
                    }

                    if (!fileExists || fileNewer)
                    {
                         var uploadRequest = new TransferUtilityUploadRequest
                        {
                            BucketName = bucketname,
                            FilePath = filePath,
                            Key = key,
                            CannedACL = S3CannedACL.PublicRead
                        };

                        uploadRequest.UploadProgressEvent +=
                            new EventHandler<UploadProgressArgs>(UploadRequest_UploadPartProgressEvent);

                        transferUtility.Upload(uploadRequest);
                    }
                }

                //TransferUtility transferUtility = new TransferUtility(client);

                //TransferUtilityUploadDirectoryRequest uploadDirectoryRequest = new TransferUtilityUploadDirectoryRequest
                //{
                //    BucketName = bucketname,
                //    Directory = "D:\\Users\\Benjamin\\Desktop\\deploy",
                //    SearchOption = SearchOption.AllDirectories,
                //    CannedACL = S3CannedACL.PublicRead
                //};

                //uploadDirectoryRequest.UploadDirectoryProgressEvent +=
                //    new EventHandler<UploadDirectoryProgressArgs>(UploadRequest_UploadPartProgressEvent);

                //transferUtility.UploadDirectory(uploadDirectoryRequest);
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
