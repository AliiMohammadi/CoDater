using System;
using System.IO;

namespace CoDater.Logger
{
    internal class FileState : FileInfo
    {
        public enum FileStatus
        {
            UnChanged,Changed,Added,Removed
        }
        public FileStatus Status { get; set; }

        public FileState() { }
        public FileState(string file, FileStatus status) : base(file)
        {
            Status = status;    
        }
        public FileState(FileInfo file, FileStatus status)
        {
            Name = file.Name;
            FullName = file.FullName;
            LastWriteTime = file.LastWriteTime;
            Length = file.Length;
            Status = status;
        }
    }
}
