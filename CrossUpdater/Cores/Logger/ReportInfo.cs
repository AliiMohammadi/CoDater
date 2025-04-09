using System;
using System.Collections.Generic;
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
        public string WorkSpaceFullName { get; set; }

        public ReportInfo()
        {
            Files = new List<FileState>();
        }
        public ReportInfo(List<Workspace.FileInfo> files,DateTime date,int version , string workspaceadd)
        {
            Files = new List<FileState>();
            Date = date;
            Version = version;
            WorkSpaceFullName = workspaceadd;
            ConvertFileInfoToFileState(files);
        }

        void ConvertFileInfoToFileState(List<Workspace.FileInfo> files)
        {
            foreach (var item in files)
                Files.Add(new FileState(item, FileState.FileStatus.UnChanged));
        }
    }
}
