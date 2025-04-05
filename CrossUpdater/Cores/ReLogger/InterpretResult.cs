using CoDater.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDater.ReLogger
{
    internal class InterpretResult
    {
        public List<FileInfo> AddedFiles { get; set; }
        public List<FileInfo> DeletedFiles { get; set; }
        public List<FileInfo> ModedFiles { get; set; }

        public InterpretResult()
        {
            AddedFiles = new List<FileInfo>();
            DeletedFiles = new List<FileInfo>();
            ModedFiles = new List<FileInfo>();
        }

        public int ChangesCount { get {
            return AddedFiles.Count + DeletedFiles.Count + ModedFiles.Count;
            } }
    }
}
