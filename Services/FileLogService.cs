using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.LogService.ExtensionService;
using Core.LogService.Interface;
using Core.LogService.Models;
using Core.LogService.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Core.LogService.Services
{
    public class FileLogService : ILogService
    {
        public IConfiguration _configuration;
        private static readonly string _serverPath = ServerPath.RootPath();

        public FileLogService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> DeleteLog(string filter, string collection = "")
        {
            var extension = new Extension();
            var path = extension.CreateDirectory(collection);
            // create text file
            string fileLog = $"{path}\\document.txt";
            // readAllLine in document.text
            var readBeforeDelete = File.ReadAllLines(fileLog);
            var newLines = readBeforeDelete.Where(line => !line.Contains(filter));
            await File.WriteAllLinesAsync(fileLog, newLines);
            FileStream fileStream = new FileStream(fileLog, FileMode.Append);
            fileStream.Close();
            return true;
        }


        public Task<filterResponse> FindLog(string filter, string collection = "")
        {
            var extension = new Extension();
            var path = extension.CreateDirectory(collection);
            // create text file
            string fileLog = $"{path}\\document.txt";
            // o
        }

        public async Task<bool> SaveLog(object data, string collection = "")
        {
            //create a collection name and check if is exists
            var extension = new Extension();
            var path = extension.CreateDirectory(collection);
            // create text file
            string fileLog = $"{path}\\document.txt";
            if (File.Exists(fileLog))
            {
                using (StreamWriter writer = new StreamWriter(fileLog))
                    await writer.WriteAsync(data.ToString());
                return true;

            }
            return false;



        #region HelperMethods
        private string getFolderPath(string collection)
        {
            string folderpath = Path.Combine(_serverPath, $"Logs/{collection}/");

            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
        }

        public async Task<bool> UpdateLog(string filter, object data, string collection = "")
        {
            var extension = new Extension();
            var path = extension.CreateDirectory(collection);
            // create text file
            string fileLog = $"{path}\\document.txt";

            if (!string.IsNullOrEmpty(filter))
            {
                StreamReader reader = new StreamReader(fileLog);
                string content = reader.ReadToEnd();
                reader.Close();
                content = content.Replace(content, data.ToString());
                StreamWriter writer = new StreamWriter(fileLog);
                await writer.WriteAsync(content);
                writer.Close();
                return true;
            }

            return false;
        }
        #endregion
    }
}
