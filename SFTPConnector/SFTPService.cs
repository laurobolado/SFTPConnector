using System;
using System.Collections.Generic;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace SFTPConnector
{
    public class SFTPService
    {
        private string _host;
        private int _port;
        private string _username;
        private string _password;

        public SFTPService(string host, int port, string username, string password)
        {
            this._host = host;
            this._port = port;
            this._username = username;
            this._password = password;
        }

        public void DeleteFile(string remoteFilePath)
        {
            using var client = new SftpClient(this._host, this._port == 0 ? 22 : this._port, this._username, this._password);
            try
            {
                client.Connect();
                client.DeleteFile(remoteFilePath);
                Console.WriteLine($"File [{remoteFilePath}] deleted.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString(), $"Failed in deleting file [{remoteFilePath}]");
            }
            finally
            {
                client.Disconnect();
            }
        }

        public IEnumerable<SftpFile> ListAllFiles(string remoteDirectory = ".")
        {
            using var client = new SftpClient(this._host, this._port == 0 ? 22 : this._port, this._username, this._password);
            try
            {
                client.Connect();
                return client.ListDirectory(remoteDirectory);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString(), $"Failed in listing files under [{remoteDirectory}]");
                return null;
            }
            finally
            {
                client.Disconnect();
            }
        }

        public void UploadFile(string localFilePath, string remoteFilePath)
        {
            using var client = new SftpClient(this._host, this._port == 0 ? 22 : this._port, this._username, this._password);
            try
            {
                client.Connect();

                using var s = File.OpenRead(localFilePath);

                client.UploadFile(s, remoteFilePath);
                Console.WriteLine($"Finished uploading file [{localFilePath}] to [{remoteFilePath}]");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString(), $"Failed in uploading file [{localFilePath}] to [{remoteFilePath}]");
            }
            finally
            {
                client.Disconnect();
            }
        }

        public void UploadFile(Stream fileStream, string remoteFilePath)
        {
            using var client = new SftpClient(this._host, this._port == 0 ? 22 : this._port, this._username, this._password);
            try
            {
                client.Connect();

                client.UploadFile(fileStream, remoteFilePath);
                Console.WriteLine($"Finished uploading to [{remoteFilePath}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString(), $"Failed in uploading file to [{remoteFilePath}]");
            }
            finally
            {
                client.Disconnect();
            }
        }
    }
}
