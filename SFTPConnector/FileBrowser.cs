using System.IO;

namespace SFTPConnector
{
    public class FileBrowser
    {
        public static string[] ListFiles(string targetDirectory)
        {
            return Directory.GetFiles(targetDirectory);
        }
    }
}
