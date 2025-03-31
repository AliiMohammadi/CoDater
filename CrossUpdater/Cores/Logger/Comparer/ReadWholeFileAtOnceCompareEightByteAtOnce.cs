using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDater.Logger.Comparer
{
    public class ReadWholeFileAtOnceCompareEightByteAtOnce : FileComparer
    {

        public ReadWholeFileAtOnceCompareEightByteAtOnce(string filePath01, string filePath02) : base(filePath01, filePath02)
        {
        }

        protected override bool OnCompare()
        {
            var fileContents01 = File.ReadAllBytes(FileInfo1.FullName);
            var fileContents02 = File.ReadAllBytes(FileInfo2.FullName);

            int lastBlockIndex = fileContents01.Length - (fileContents01.Length % sizeof(ulong));

            var totalProcessed = 0;
            while (totalProcessed < lastBlockIndex)
            {
                if (BitConverter.ToUInt64(fileContents01, totalProcessed) != BitConverter.ToUInt64(fileContents02, totalProcessed))
                {
                    return false;
                }
                totalProcessed += sizeof(ulong);
            }
            return true;
        }
    }
}
