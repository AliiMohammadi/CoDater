using System;
using System.Collections.Generic;
using System.Linq;
using SaveAndRetrieve;
using CoDater.Workspace;

namespace CoDater.Logger
{
    /// <summary>
    /// وظیفه گزارش گیری از پروژه فعلی را دارد
    /// 
    /// </summary>
    internal class Reporter
    {
        /// <summary>
        /// اسم فایل گزارش 
        /// </summary>
        public string FileName = "CoDater.dat";

        WorkSpace workspace;
        JsonSaveData SaveLoad;

        public Reporter()
        {
            workspace = new WorkSpace();
            SaveLoad = new JsonSaveData();
        }
        public Reporter(System.IO.DirectoryInfo workdirectory)
        {
            workspace = new WorkSpace(workdirectory);
            SaveLoad = new JsonSaveData();
        }

        /// <summary>
        /// گزارش گیری از نسخه ای که درال اظر هست
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<ReportInfo> Report()
        {
            List<FileInfo> CurrentVersion = workspace.GetAllFilesAndSubFolderFiles();

            string datapath = workspace.WorkDirectory + "\\" + FileName;

            if (!System.IO.File.Exists(datapath))
            {
                List<ReportInfo> emptyreport = new List<ReportInfo>();
                //Report file doesnt exist then creat and write defualt data.
                emptyreport.Add(new ReportInfo(CurrentVersion, DateTime.Now, 1 , workspace.WorkDirectory.FullName));
                SaveLoad.SaveData(emptyreport, datapath);
                return emptyreport;
            }

            //Read the file.

            List<ReportInfo> loadedReport = SaveLoad.LoadData<List<ReportInfo>>(datapath);

            //Determine the changes.

            ReportInfo Lastversion = loadedReport[loadedReport.Count - 1];
            ReportInfo NewVersion = GetChanges(Lastversion, CurrentVersion);

            if(NewVersion.ChangesCount == 0)
                throw new Exception("No changes detected.");

            loadedReport.Add(NewVersion);

            SaveLoad.SaveData(loadedReport, datapath);

            return loadedReport;
        }
        /// <summary>
        /// تغییرات دو را برمیگردادند 
        /// </summary>
        /// <param name="LastReport"></param>
        /// <param name="CurrentFiles"></param>
        /// <returns></returns>
        public ReportInfo GetChanges(ReportInfo LastReport , List<Workspace.FileInfo> CurrentFiles)
        {
            ReportInfo result = new ReportInfo(new List<FileInfo>(), DateTime.Now, ++LastReport.Version, workspace.WorkDirectory.FullName);

            //Version check steps:
            //1: Deleted files check.
            //2: New files check.
            //3: Moded files check.

            List<FileState> LastReportfiles = LastReport.Files.Where(x=>x.Status!=FileState.FileStatus.Removed).ToList();
            List<FileInfo> LastReportfilesCONVERTED = ConvertSTATEtoINFO(LastReportfiles);

            //Determining the moded files.
            //A funtion that Gives two FileInfo list. 
            //الان این فایل هایی رو برمیگردون که اصلا تغییر نکردن

            List<FileInfo> CurrentRemainedFiles = GetCurrentRemainedFiles(CurrentFiles, LastReportfilesCONVERTED);

            foreach (FileInfo lastfile in LastReportfiles)
            {
                if (!CurrentRemainedFiles.Exists(x => x.FullName == lastfile.FullName))
                    continue;

                FileInfo CurretnFoundedFile = FindFileByFullName(CurrentRemainedFiles, lastfile.FullName);

                if (!AreSameFiles(lastfile, CurretnFoundedFile))
                {
                    result.Files.Add(new FileState(CurretnFoundedFile, FileState.FileStatus.Changed));
                    result.ModedFileCount++;
                }
                else
                    result.Files.Add(new FileState(CurretnFoundedFile, FileState.FileStatus.UnChanged));
            }

            //Determin the deleted files and Added files
            foreach (FileInfo item in GetAddedFiles(LastReportfilesCONVERTED, CurrentFiles))
            {
                result.Files.Add(new FileState(item, FileState.FileStatus.Added));
                result.AddedFilesCount++;
            }
            foreach (FileInfo item in GetDeletedFiles(LastReportfilesCONVERTED, CurrentFiles))
            {
                result.Files.Add(new FileState(item, FileState.FileStatus.Removed));
                result.DeletedFileCount++;
            }

            return result;
        }

        public System.IO.DirectoryInfo WorkDirectory
        {
            get
            {
                return workspace.WorkDirectory;
            }
            set
            {
                workspace.WorkDirectory = value;
            }
        }

        #region Local Funtions
        bool AreSameFiles(FileInfo file1,FileInfo file2)
        {
            if (string.Equals(file1.Name,file2.Name))
                if (AreSameDates(file1.LastWriteTime.ToLocalTime(), file2.LastWriteTime))
                    if(file1.Length == file2.Length)
                        return true;

            return false;
        }
        bool AreSameDates(DateTime d1, DateTime d2)
        {
            return (d1.Year == d2.Year && d1.Month == d2.Month && d1.Day == d2.Day && d1.Hour == d2.Hour && d1.Minute == d2.Minute);
        }

        List<FileInfo> GetCurrentRemainedFiles(List<FileInfo> CurrentFiles, List<FileInfo> LastReportfilesCONVERTED)
        {
            return Intersect(CurrentFiles, LastReportfilesCONVERTED); 
        }
        List<FileInfo> GetAddedFiles(List<FileInfo> LastReportfiles, List<FileInfo> CurrentFiles)
        {
            return Except(CurrentFiles,LastReportfiles);
        }
        List<FileInfo> GetDeletedFiles(List<FileInfo> LastReportfiles, List<FileInfo> CurrentFiles)
        {
            return Except(LastReportfiles, CurrentFiles);
        }

        List<FileInfo> Intersect(List<FileInfo> l1, List<FileInfo> l2)
        {
            List<FileInfo> final = new List<FileInfo>();

            foreach (string l in SelectFullName(l1).Intersect(SelectFullName(l2)).ToList())
                final.Add(l1.Where(x => x.FullName == l).First());

            return final;
        }
        List<FileInfo> Except(List<FileInfo> l1, List<FileInfo> l2)
        {
            List<FileInfo> final = new List<FileInfo>();

            foreach (string l in SelectFullName(l1).Except(SelectFullName(l2)).ToList())
                final.Add(l1.Where(x => x.FullName == l).First());

            return final;

        }

        List<FileInfo> ConvertSTATEtoINFO(List<FileState> state)
        {
            List<FileInfo> newlist = new List<FileInfo>();

            foreach(FileState l in state)
            {
                FileInfo newinfo = new FileInfo();

                newinfo.FullName = l.FullName;
                newinfo.Length = l.Length;
                newinfo.LastWriteTime = l.LastWriteTime;
                newinfo.Name = l.Name;

                newlist.Add(newinfo);
            }

            return newlist;
        }

        List<string> SelectFullName(List<FileInfo> list)
        {
            return list.Select(l => l.FullName).ToList();
        }

        FileInfo FindFileByFullName(List<FileInfo> files,string fullname)
        {
            return files.Where(x => Equals(x.FullName, fullname)).FirstOrDefault();
        }
        #endregion
    }
}
