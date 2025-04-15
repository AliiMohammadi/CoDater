using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CoDater.Workspace
{
    /// <summary>
    /// کلاس برای مدیریت کردن فولدر محیط کاری 
    /// اطلاعاتی درمورد فایل ها و ...
    /// </summary>
    internal class WorkSpace
    {
        public DirectoryInfo WorkDirectory;
        /// <summary>
        /// ادرس برنامه ای که درال اجراس
        /// </summary>
        public string GetApplicationBasePath
        {

            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
        public List<string> IgnoreList;

        public WorkSpace()
        {
            WorkDirectory = new DirectoryInfo(GetApplicationBasePath);
            IgnoreList = new List<string>();
        }
        public WorkSpace(string workdirectory)
        {
            WorkDirectory = new DirectoryInfo(workdirectory);
            IgnoreList = new List<string>();

        }
        public WorkSpace(DirectoryInfo workdirectory)
        {
            WorkDirectory = workdirectory;
            IgnoreList = new List<string>();

        }

        /// <summary>
        /// لیست تمامی فایل ها را برمیگرداند        /// </summary>
        /// <returns></returns>
        public List<FileInfo> GetAllFilesAndSubFolderFiles()
        {
            List<FileInfo> files = DirSearch(WorkDirectory.FullName);
            
            if(IgnoreList.Count == 0)
                return files;

            foreach (var item in IgnoreList)
            {
                int f = files.IndexOf(files.Where(x => string.Equals(x.Name, item)).First());

                if(f > -1)
                    files.RemoveAt(f);
            }

            return files;
        }
        public string WorkName(FileInfo file)
        {
            return file.WorkName.Replace(WorkDirectory.FullName,"\\");
        }

        List<FileInfo> DirSearch(string sdir)
        {
            List<FileInfo> files = new List<FileInfo>();

            try
            {
                foreach (string f in Directory.GetFiles(sdir))
                    files.Add(new FileInfo(f));

                foreach (string d in Directory.GetDirectories(sdir))
                    files.AddRange(DirSearch(d));

                return files;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
