using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDater.Logger
{
    internal class ReportInfo : ChangeInfo
    {
        public DateTime Date { get; set; }
        public int Version { get; set; }
        public List<FileState> Files { get; set; }

        public ReportInfo()
        {
            Files = new List<FileState>();
        }
        public ReportInfo(List<FileInfo> files,DateTime date,int version)
        {
            Files = new List<FileState>();
            Date = date;
            Version = version;
            ConvertFileInfoToFileState(files);
        }

        void ConvertFileInfoToFileState(List<FileInfo> files)
        {
            foreach (var item in files)
                Files.Add(new FileState(item, FileState.FileStatus.UnChanged));
        }
    }
}
