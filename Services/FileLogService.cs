using System;
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
            throw new NotImplementedException();
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

        public async Task<bool> SaveLog(object data, string collection = "")
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLog(string filter, string document, string collection = "")
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
