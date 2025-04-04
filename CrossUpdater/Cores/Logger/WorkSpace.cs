using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace CoDater.Logger
{
    internal class WorkSpace
    {
        public DirectoryInfo WorkDirectory;
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

        public List<FileInfo> GetAllFilesAndSubFolderFiles()
        {
            return DirSearch(WorkDirectory.FullName);
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
