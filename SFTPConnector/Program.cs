using System.IO;
using Microsoft.Extensions.Configuration;

namespace SFTPConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false);

            IConfiguration config = builder.Build();

            var localAssetConfiguration = config.GetSection("LocalAsset").Get<LocalAsset>();
            var remoteAssetConfiguration = config.GetSection("RemoteAsset").Get<RemoteAsset>();
            var sftpService = new SFTPService(remoteAssetConfiguration.Host, remoteAssetConfiguration.Port, remoteAssetConfiguration.Username, remoteAssetConfiguration.Password);

            string[] listFiles = FileBrowser.ListFiles(localAssetConfiguration.Directory);
            
            foreach (string fileName in listFiles)
            {
                sftpService.UploadFile(fileName, remoteAssetConfiguration.Directory + fileName.Substring(fileName.LastIndexOf("/") + 1));
            }
        }
    }

    public class LocalAsset
    {
        public string Directory { get; set; }
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
