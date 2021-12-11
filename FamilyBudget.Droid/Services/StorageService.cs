using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FamilyBudget.Core.Services;
using AndroidContext = Android.App;

namespace FamilyBudget.Droid.Services
{
    public class StoragePlatformService : IPlatformService
    {
        private static string PackageName => AndroidContext.Application.Context.PackageManager
            .GetPackageInfo(AndroidContext.Application.Context.PackageName, 0).PackageName;

        private static string GetFileFullpath(string filename)
        {
            string location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(location, filename);
        }

        public void DeleteFile(string filename)
        {
            var fullpath = GetFileFullpath(filename);
            File.Delete(fullpath);
        }

        public Task<string> ReadFileContent(string filename)
        {
            return Task.FromResult(GetSetting(filename, null));
            //var fullpath = GetFileFullpath(filename);

            //if (!File.Exists(fullpath)) return null;

            //return Task.FromResult(File.ReadAllText(fullpath));
        }

        public async Task<bool> WriteToFile(string filename, string content)
        {
            SaveSetting(filename, content);
            //var fullpath = GetFileFullpath(filename);

            //using (var fs = File.CreateText(fullpath))
            //{
            //    await fs.WriteAsync(content);
            //}

            return await Task.FromResult(true);
        }

        public static void SaveSetting(string key, string value)
        {

            var prefs = AndroidContext.Application.Context.GetSharedPreferences(PackageName.Trim(), FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString(key, value);
            prefEditor.Commit();
        }



        public static string GetSetting(string key, string defValue = null)
        {
            var prefs = AndroidContext.Application.Context.GetSharedPreferences(PackageName.Trim(), FileCreationMode.Private);
            var settingValue = prefs.GetString(key, defValue);
            return settingValue;
        }
    }
}