using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace UploadFolderToAWS
{
    public static class CloudFunctions
    {
        private readonly static string ProfileName = "";
        private readonly static string LogPath = "";
        private readonly static string BucketName = "";

        static CloudFunctions()
        {
            ProfileName = ConfigurationManager.AppSettings["ProfileName"];
            LogPath = ConfigurationManager.AppSettings["LogPath"] + "LogFile" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
            BucketName = ConfigurationManager.AppSettings["BucketName"];
        }

        public static void UploadDirectoryWithFiles(string localDir, TransferUtility fileTransferUtility, string cleanPath = null)
        {
            localDir = @"D:\Users\hsharma\Article23As1\";
            fileTransferUtility.UploadDirectory(localDir, BucketName, "Article23A", SearchOption.AllDirectories);
            
        }
        public static void UploadFilesParlelly(string localDir, TransferUtility fileTransferUtility, string cleanPath = null)
        { 
            int count = 0;
            StringBuilder sb = new StringBuilder();
            Parallel.ForEach(Directory.GetFiles(localDir), filePath =>
            {
                string currentFile = Path.GetFileName(filePath);
                fileTransferUtility.Upload(filePath, BucketName + @"/" + "Article23As", currentFile);
                Console.WriteLine(currentFile + " uploaded.");
                count++;
                sb.Append(count + " File Name: " + currentFile + Environment.NewLine);
            });
            Console.WriteLine("Total file uploaded: "+ count.ToString());
            File.AppendAllText(LogPath, sb.ToString());
        }

        private static AmazonS3Client GetAWSClient()
        {
            var credentials = new StoredProfileAWSCredentials(ProfileName);
            
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the MINIO_REGION enviroment variable.
                ServiceURL = "https://s3.amazonaws.com",
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };
            return new AmazonS3Client(credentials,config);
        }

        public static TransferUtility GetTransferUtility()
        {
            return new TransferUtility(GetAWSClient());
        }

    }
}
