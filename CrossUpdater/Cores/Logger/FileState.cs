using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDater.Logger
{
    internal class FileState 
    {
        public enum FileStatus
        {
            UnChanged,Changed,Added,Removed
        }

        public FileInfo Fileinfo { get; set; }
        public FileStatus Status { get; set; }

        public FileState(FileInfo file, FileStatus status)
        {
            this.Status = status;
            Fileinfo = file;
        }

    }
}
