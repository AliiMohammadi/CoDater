using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace CoDater.Logger
{
    internal class WorkSpace
    {
        public string GetPath
        {
            get { //return AppDomain.CurrentDomain.BaseDirectory;
                return @"D:\Project\"; 
                }
        }

        public List<FileInfo> GetAllFilesAndSubFolderFiles
        {
            get
            {
                return DirSearch(GetPath);
            }
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
