using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Amazon.S3;
using Amazon.S3.Model;

namespace SFTPConnector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false);

            IConfiguration config = builder.Build();

            var awsS3AssetConfiguration = config.GetSection("AWSS3Asset").Get<AWSS3Asset>();
            var remoteAssetConfiguration = config.GetSection("RemoteAsset").Get<RemoteAsset>();
            
            var sftpService = new SFTPService(remoteAssetConfiguration.Host, remoteAssetConfiguration.Port, remoteAssetConfiguration.Username, remoteAssetConfiguration.Password);

            var listObjectsResponse = await AWSS3Service.ListingObjectsAsync(awsS3AssetConfiguration.BucketName);

            foreach (S3Object entry in listObjectsResponse.S3Objects)
            {
                using (GetObjectResponse objectResponse = await AWSS3Service.GettingObjectAsync(awsS3AssetConfiguration.BucketName, entry.Key))
                using (Stream responseStream = objectResponse.ResponseStream)
                {
                    sftpService.UploadFile(responseStream, remoteAssetConfiguration.Directory + entry.Key);
                }
            }
        }
    }

    public class LocalAsset
    {
        public string Directory { get; set; }
    }

    public class AWSS3Asset
    {
        public String BucketName { get; set; }
    }

    public class RemoteAsset
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Directory { get; set; }
    }
}
