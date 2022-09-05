using System;
using System.Threading.Tasks;
using Core.LogService.Interface;
using Core.LogService.Models;
using Microsoft.Extensions.Configuration;

namespace Core.LogService.Services
{
    public class FileLogService : ILogService
    {
        public IConfiguration _configuration;

        public FileLogService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> DeleteLog(string filter, string collection = "")
        {
            throw new NotImplementedException();
        }

        public Task<filterResponse> FindLog(string filter, string collection = "")
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveLog(string data, string collection = "")
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLog(string filter, string document, string collection = "")
        {
            throw new NotImplementedException();
        }
    }
}
