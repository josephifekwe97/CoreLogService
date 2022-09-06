using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.LogService.Data;
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

        public Task<bool> DeleteLog(string filter, string collection = "")
        {
            collection = string.IsNullOrEmpty(collection) ? _configuration.GetValue<string>("LogService:DefaultCollectionName") : collection;

            string filepath = getFolderPath(collection) + $"{getFileNameFromFilter(filter)}.txt";

            if (File.Exists(filepath))
            {
                File.Delete(filepath);

                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public Task<filterResponse> FindLog(string filter, string collection = "")
        {
            //filter = "ReferenceCode:23433

            var response = new filterResponse()
            {
                documents = new System.Collections.Generic.List<object>()
            };

            collection = string.IsNullOrEmpty(collection) ? _configuration.GetValue<string>("LogService:DefaultCollectionName") : collection;

            string filepath = getFolderPath(collection) + $"{getFileNameFromFilter(filter)}.txt";

            if (File.Exists(filepath))
            {
                var logtext = File.ReadAllText(filepath);

                response.documents.Add(JsonConvert.DeserializeObject(logtext));
            }

            return Task.FromResult(response);
        }

        public Task<bool> SaveLog(string data, string collection = "")
        {
            collection = string.IsNullOrEmpty(collection) ? _configuration.GetValue<string>("LogService:DefaultCollectionName") : collection;

            string filepath = getFolderPath(collection) + $"{getFileNameFromData(collection, data)}.txt";

            if (!File.Exists(filepath)) File.Create(filepath).Dispose();

            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.Write(data);
                sw.Flush();
                sw.Close();
            }

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
        private string getFileNameFromData(string colletion, string data)
        {
            //ToDo: We need to extract the filename of a log from the data payload (refer to the txt file i sent you)
            //collection: nip_accountblock_logs
            //keyfield: ReferenceCode
            //samplaepayload: { "SessionID":"9999992207261008042207261008044556","DestinationInstitutionCode":"","ChannelCode":"2","ReferenceCode":"",
            //            "TargetAccountName":"0000000149","TargetBankVerificationNumber":"","TargetAccountNumber":"0000000149",
            //            "ReasonCode":"1","Narration":"Test Narration"}

            FileStream path = new FileStream(@"D:\Bello\MidraSolution\filelogsample", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(path);
            string record;
            try
            {
                record = reader.ReadLine();
                while (record != null)
                {
                    if (record.Contains(colletion))
                    {
                        int lineNumber = 3;
                        string lineContents = ReadSpecificLine(path.ToString(), lineNumber);
                        Console.WriteLine(lineContents);
                        record = reader.ReadLine();
                        data = record;
                    }
                }
            }
            finally
            {
                reader.Close();
                path.Close();
            }
            switch (colletion)
            {
                case "nip_accountblock_logs":
                    var model = JsonConvert.DeserializeObject<nip_accountblock_logs>(data);
                    return model.ReferenceCode;
                default:
                    break;
            }
            return "";
        }

        private string getFileNameFromFilter(string filter)
        {
            try
            {
                return filter.Split(":")[1];
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Filter Data");
            }
        }
        #endregion
        static string ReadSpecificLine(string filePath, int lineNumber)
        {
            string content = null;
            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    for (int i = 1; 1 < lineNumber; i++)
                    {
                        file.ReadLine();
                        if (file.EndOfStream)
                        {
                            Console.WriteLine($"End of file. The file only contains{i} lines.");
                            break;
                        }
                    }
                    content = file.ReadLine();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("there was an error reading the file.");
                Console.WriteLine(ex.Message);

            }
            return content;
        }
    }
}
