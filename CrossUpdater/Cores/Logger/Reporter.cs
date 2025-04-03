using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SaveAndRetrieve;

namespace CoDater.Logger
{
    internal class Reporter
    {
        public string FileName = "CoDater.dat";

        WorkSpace workspace;
        JsonSaveData SaveLoad;

        public Reporter()
        {
            workspace = new WorkSpace();
            SaveLoad = new JsonSaveData();
        }

        public List<ReportInfo> Report()
        {
            List<FileInfo> CurrentVersion = workspace.GetAllFilesAndSubFolderFiles;

            string datapath = workspace.GetPath + "\\" + FileName;

            if (!File.Exists(datapath))
            {
                List<ReportInfo> emptyreport = new List<ReportInfo>();
                //Report file doesnt exist then creat and write defualt data.
                emptyreport.Add(new ReportInfo(CurrentVersion, DateTime.Now, 1));
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
        public ReportInfo GetChanges(ReportInfo LastReport , List<FileInfo> CurrentFiles)
        {
            ReportInfo result = new ReportInfo(new List<FileInfo>(), DateTime.Now, LastReport.Version++);

            //Version check steps:
            //1: Deleted files check.
            //2: New files check.
            //3: Moded files check.

            List<FileState> LastReportfiles = LastReport.Files;
            List<FileInfo> LastReportfilesCONVERTED = ConvertSTATEtoINFO(LastReportfiles);

            //Determining the moded files.
            //A funtion that Gives two FileInfo list. 
            //
            //الان این فایل هایی رو برمیگردون که اصلا تغییر نکردن
            List<FileInfo> LastRemainedFiles = Intersect(LastReportfilesCONVERTED, CurrentFiles); 
            List<FileInfo> CurrentRemainedFiles = Intersect( CurrentFiles, LastReportfilesCONVERTED);

            foreach (FileInfo lastfile in LastReportfiles)
            {
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

        public bool AreSameFiles(string file1, string file2)
        {
            return AreSameFiles(new FileInfo(file1),new FileInfo(file2));
        }
        public bool AreSameFiles(FileInfo file1,FileInfo file2)
        {
            if(string.Equals(file1.Name,file2.Name))
                if (file1.LastWriteTime == file2.LastWriteTime)
                    if(file1.Length == file2.Length)
                        return true;

            return false;
        }

        #region Local Funtions
        List<FileInfo> GetCurrentRemainedFiles(List<FileInfo> LastReportfiles, List<FileInfo> CurrentFiles)
        {
            return Intersect(CurrentFiles, Intersect(LastReportfiles, CurrentFiles));
        }
        List<FileInfo> GetAddedFiles(List<FileInfo> LastReportfiles, List<FileInfo> CurrentFiles)
        {
            return Exept(CurrentFiles,LastReportfiles);
        }
        List<FileInfo> GetDeletedFiles(List<FileInfo> LastReportfiles, List<FileInfo> CurrentFiles)
        {
            return Exept(LastReportfiles, CurrentFiles);
        }

        public List<FileInfo> Intersect(List<FileInfo> l1, List<FileInfo> l2)
        {
            List<string> ls1 = l1.Select(l => l.FullName).ToList();
            List<string> ls2 = l2.Select(l => l.FullName).ToList();

            List<string> lsex = ls1.Intersect(ls2).ToList();

            List<FileInfo> final = new List<FileInfo>();

            foreach (string l in lsex)
            //{
            //    FileInfo y = l1.Where(x => x.FullName == l).First();

            //    if(AreSameFiles(y, l2.Where(x => x.FullName == l).First()))
            //final.Add(y);

            //}
            final.Add(l1.Where(x => x.FullName == l).First());


            return final;
        }
        public List<FileInfo> Exept(List<FileInfo> l1, List<FileInfo> l2)
        {
            //THIS FUNTION NEEDS TO UNIT TEST
            //The funtion have issues. Need to fix.
            return l1.Except(l2).ToList();
        }

        FileInfo FindFileByFullName(List<FileInfo> files,string fullname)
        {
            return files.Where(x => Equals(x.FullName, fullname)).First();
        }

        public List<FileInfo> ConvertSTATEtoINFO(List<FileState> state)
        {
            List<FileInfo> newlist = new List<FileInfo>();

            foreach(FileState l in state)
                newlist.Add(new FileInfo(l.FullName));

            return newlist;
        }
        #endregion
    }
}
