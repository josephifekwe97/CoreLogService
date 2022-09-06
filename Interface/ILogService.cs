using System;
using System.Threading.Tasks;

namespace Core.LogService.Interface
{
    public interface ILogService
    {
        Task<Models.filterResponse> FindLog(string filter, string collection = "");

        Task<bool> DeleteLog(string filter, string collection = "");

        Task<bool> SaveLog(object data, string collection = "");

        Task<bool> UpdateLog(string filter, string document, string collection = "");
    }
}
