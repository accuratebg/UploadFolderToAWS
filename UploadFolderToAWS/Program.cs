using System;
using System.Configuration;

namespace UploadFolderToAWS
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Uploading start.");
                string directoryPath = ConfigurationManager.AppSettings["DirectoryPath"];
                CloudFunctions.UploadFilesParlelly(directoryPath, CloudFunctions.GetTransferUtility());
                Console.WriteLine("Uploading end.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Uploading Fail.");
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
