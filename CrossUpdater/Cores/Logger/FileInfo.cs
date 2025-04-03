using System;

namespace CoDater.Logger
{
    internal class FileInfo
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public DateTime LastWriteTime { get; set; }
        public long Length { get; set; }

        public FileInfo() { }
        public FileInfo(string file)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(file);
            Name = info.Name;
            FullName = info.FullName;
            LastWriteTime = info.LastWriteTime;
            Length = info.Length;

        }
    }
}
