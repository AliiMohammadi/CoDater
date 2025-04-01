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

            loadedReport.Add(NewVersion);

            SaveLoad.SaveData(loadedReport, datapath);

            return loadedReport;
        }
        public ReportInfo GetChanges(ReportInfo LastReport , List<FileInfo> CurrentFiles)
        {
            ReportInfo FinalReport = new ReportInfo(new List<FileInfo>(),DateTime.Now,LastReport.Version++);

            //Version check steps:
            //1: Deleted files check.
            //2: New files check.
            //3: Moded files check.

            List<FileInfo> LastReportfiles = LastReport.GetOnlyFileInfo();

            //Determining the moded files.

            foreach (FileInfo lastfile in LastReportfiles)
            {
                FileInfo CurretnFoundedFile = FindFileByName(GetCurrentRemainedFiles(LastReportfiles, CurrentFiles), lastfile.Name);

                if (!AreSameFiles(lastfile, CurretnFoundedFile))
                    FinalReport.Files.Add(new FileState(CurretnFoundedFile, FileState.FileStatus.Changed));
                else
                    FinalReport.Files.Add(new FileState(CurretnFoundedFile, FileState.FileStatus.UnChanged));
            }
            
            //Determin the deleted files and Added files
            foreach (FileInfo item in GetAddedFiles(LastReportfiles, CurrentFiles))
                FinalReport.Files.Add(new FileState(item, FileState.FileStatus.Added));
            foreach (FileInfo item in GetDeletedFiles(LastReportfiles, CurrentFiles))
                FinalReport.Files.Add(new FileState(item, FileState.FileStatus.Removed));

            return FinalReport;
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

        List<FileInfo> Intersect(List<FileInfo> l1, List<FileInfo> l2)
        {
            return l1.Intersect(l2).ToList();
        }
        List<FileInfo> Exept(List<FileInfo> l1, List<FileInfo> l2)
        {
            return l1.Except(l2).ToList();
        }

        FileInfo FindFileByName(List<FileInfo> files,string filename)
        {
            return files.Where(x => Equals(x.Name, filename)).First();
        }
        #endregion
    }
}
