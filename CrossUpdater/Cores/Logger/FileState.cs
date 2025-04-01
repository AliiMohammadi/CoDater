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

        public string Name { get; set; }
        public string Fullname { get; set; }
        public DateTime LastWriteTime { get; set; }
        public long Length { get; set; }

        public FileStatus Status { get; set; }

        public FileState(FileInfo file, FileStatus status)
        {
            Name = file.Name;
            Fullname = file.FullName;
            LastWriteTime = file.LastWriteTime;
            Length = file.Length;
            Status = status;
        }
    }
}
