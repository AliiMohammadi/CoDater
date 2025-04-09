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
        public string FullName { get; set; }
        public DateTime LastWriteTime { get; set; }
        public long Length { get; set; }

        public FileInfo() 
        {

        }
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
