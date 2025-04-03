using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDater.Logger
{
    internal class ChangeInfo
    {
        public int ModedFileCount { get; set; }
        public int AddedFilesCount { get; set; }
        public int DeletedFileCount { get; set; }

        public int ChangesCount { get 
            { 
                return ModedFileCount + AddedFilesCount + DeletedFileCount;
            } }
    }
}
