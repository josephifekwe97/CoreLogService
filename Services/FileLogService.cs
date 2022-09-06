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

            return Task.FromResult(true);
        }

        public Task<bool> UpdateLog(string filter, string document, string collection = "")
        {
            collection = string.IsNullOrEmpty(collection) ? _configuration.GetValue<string>("LogService:DefaultCollectionName") : collection;

            string filepath = getFolderPath(collection) + $"{getFileNameFromFilter(filter)}.txt";

            if (File.Exists(filepath))
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.Write(document);
                    sw.Flush();
                    sw.Close();
                }
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }

        }
        #region HelperMethods
        private string getFolderPath(string collection)
        {
            string folderpath = Path.Combine(_serverPath, $"Logs/{collection}/");

            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }

            return folderpath;
        }
        public string getFileNameFromData(string colletion, string data)
        private string getFileNameFromData(string colletion, string data)
        {
            var extension = new Extension();
            var path = extension.CreateDirectory(collection);
            // create text file
            string fileLog = $"{path}\\document.txt";

            data = ReadData(colletion);
            switch (colletion)
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
        static string ReadSpecificLine(string filePath, int lineNumber)
        {
            string content = null;
            try
            {
                content = File.ReadLines(filePath).Skip(lineNumber + 1).Take(1).First();
            }
            catch (IOException ex)
            {
                Console.WriteLine("there was an error reading the file.");
                Console.WriteLine(ex.Message);
            }
            return content;
        }

        private string ReadData(string collection)
        {
            string content = null;
            try
            {

                foreach (var match in File.ReadLines(@"D:\Bello\MidraSolution\filelogsample")
                          .Select((text, index) => new { text, lineNumber = index + 1 })
                          .Where(x => x.text.Contains(collection)))
                {
                    content = ReadSpecificLine(@"D:\Bello\MidraSolution\filelogsample", match.lineNumber);

                }
                if (content != null)
                    return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return content;
        }
    }
}
