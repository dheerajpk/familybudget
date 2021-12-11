using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyBudget.Core.Services
{
    public class StorageService
    {
        private static IPlatformService _platformService;

        public Task<bool> WriteToFile(string filename, string content)
        {
            return _platformService.WriteToFile(filename, content);
        }

        public Task<string> ReadFileContent(string filename)
        {
            return _platformService.ReadFileContent(filename);
        }

        public void DeleteFile(string filename)
        {
            _platformService.DeleteFile(filename);
        }

        public static void Initialise(IPlatformService platformService)
        {
            _platformService = platformService;
        }
    }
}
