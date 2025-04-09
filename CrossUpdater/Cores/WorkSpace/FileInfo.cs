using System;

namespace CoDater.Workspace
{
    internal class FileInfo
    {
        //Example:
        //D://Myproject/DocumentFolder/MyFile.txt

        //MyFile.txt
        public string Name { get; set; }
        //D://Myproject/DocumentFolder/MyFile.txt
        public string WorkName { get; set; }
        public DateTime LastWriteTime { get; set; }
        public long Length { get; set; }

        public FileInfo() 
        {

        }
        public FileInfo(string file)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(file);
            Name = info.Name;
            WorkName = info.FullName;
            LastWriteTime = info.LastWriteTime;
            Length = info.Length;
        }
        public FileInfo(string file , string workname)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(file);
            Name = info.Name;
            WorkName = workname;
            LastWriteTime = info.LastWriteTime;
            Length = info.Length;
        }
    }
}
