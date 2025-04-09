using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

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
                
            get { return AppDomain.CurrentDomain.BaseDirectory;}
        }

        public WorkSpace()
        {
            WorkDirectory = new DirectoryInfo(GetApplicationBasePath);
        }
        public WorkSpace(string workdirectory)
        {
            WorkDirectory = new DirectoryInfo(workdirectory);
        }
        public WorkSpace(DirectoryInfo workdirectory)
        {
            WorkDirectory = workdirectory;
        }

        /// <summary>
        /// لیست تمامی فایل ها را برمیگرداند        /// </summary>
        /// <returns></returns>
        public List<FileInfo> GetAllFilesAndSubFolderFiles()
        {
            return DirSearch(WorkDirectory.FullName);
        }
        public string WorkName(FileInfo file)
        {
            return file.FullName.Replace(WorkDirectory.FullName,"\\");
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
