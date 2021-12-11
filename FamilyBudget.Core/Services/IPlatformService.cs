using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyBudget.Core.Services
{
    public interface IPlatformService
    {
        Task<bool> WriteToFile(string filename, string content);

        Task<string> ReadFileContent(string filename);

        void DeleteFile(string filename);
    }
}
