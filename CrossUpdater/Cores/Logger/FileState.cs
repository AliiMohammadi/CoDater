using System;

namespace CoDater.Logger
{
    internal class FileState : Workspace.FileInfo
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
        public FileState(Workspace.FileInfo file, FileStatus status)
        {
            Name = file.Name;
            FullName = file.FullName;
            LastWriteTime = file.LastWriteTime;
            Length = file.Length;
            Status = status;
        }
    }
}
