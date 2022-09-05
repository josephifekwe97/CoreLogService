using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.LogService.ExtensionService
{
    public class Extension
    {
        public string CreateDirectory(string collection)
        {

            string path = "D:\\Bello\\MidraSolution\\Core.LogService\\Logs\\" + collection;
            if (!(Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Directory Created Successfully");
                return path;
            }
            else
            {
                Console.WriteLine("Already Directory Exits with same Name");
                return null;
            }

        }
    }
}
