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


        // CLASS IS COMPLITED.  JUST NEED TO TEST THE UNITS.

        public Reporter()
        {
            workspace = new WorkSpace();
            SaveLoad = new SaveAndRetrieve.JsonSaveData();
        }

        public List<ReportInfo> Report()
        {
            try
            {
                List<FileInfo> CurrentVersion = workspace.GetFiles;

                string path = workspace.GetPath + "\\" + FileName;

                if (!File.Exists(path))
                {
                    List<ReportInfo> emptyreport = new List<ReportInfo>();
                    //Report file doesnt exist then creat and write defualt data.
                    emptyreport.Add(new ReportInfo(CurrentVersion, DateTime.Now, 1));
                    SaveLoad.SaveData(emptyreport, path);
                    return emptyreport;
                }

                //Read the file.
                
                List<ReportInfo> loadedReport = SaveLoad.LoadData<List<ReportInfo>>(path);
                
                //Determine the changes.
                
                ReportInfo Lastversion = loadedReport[loadedReport.Count-1];
                ReportInfo NewVersion = CheckVersionAndReport(Lastversion, CurrentVersion);
                
                loadedReport.Add(NewVersion);

                SaveLoad.SaveData(loadedReport, path);

                return loadedReport;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ReportInfo CheckVersionAndReport(ReportInfo LastReport , List<FileInfo> CurrentFiles)
        {
            ReportInfo result = new ReportInfo(new List<FileInfo>(),DateTime.Now,LastReport.Version++);

            //Version check steps:
            //1: Deleted files check.
            //2: New files check.
            //3: Moded files check.

            List<FileInfo> LastReportfiles = LastReport.GetOnlyFileInfo();
            
            //Determining the moded files.
            List<FileInfo> LastRemainedFiles = LastReportfiles.Intersect(CurrentFiles).ToList();
            List<FileInfo> CurrentRemainedFiles = CurrentFiles.Intersect(LastRemainedFiles).ToList();

            foreach (FileInfo lastfile in LastReportfiles)
            {
                FileInfo CurretnFoundedFile = FindFileByName(CurrentRemainedFiles, lastfile.Name);

                if (!AreSameFile(lastfile, CurretnFoundedFile))
                    result.Files.Add(new FileState(CurretnFoundedFile, FileState.FileStatus.Changed));
                else
                    result.Files.Add(new FileState(CurretnFoundedFile, FileState.FileStatus.UnChanged));
            }
            
            //Determin the deleted files and Added files
            foreach (FileInfo item in GetAddedFiles(LastReportfiles, CurrentFiles))
                result.Files.Add(new FileState(item, FileState.FileStatus.Added));
            foreach (FileInfo item in GetDeletedFiles(LastReportfiles, CurrentFiles))
                result.Files.Add(new FileState(item, FileState.FileStatus.Removed));

            return result;
        }

        public bool AreSameFile(FileInfo file1,FileInfo file2)
        {
            if(file1.CreationTime == file2.CreationTime)
                if(string.Equals(file1.Name,file2.Name))
                    if (file1.LastWriteTime == file2.LastWriteTime)
                        return true;

            return false;
        }

        List<FileInfo> GetAddedFiles(List<FileInfo> LastReportfiles, List<FileInfo> CurrentFiles)
        {
            return Exept(CurrentFiles,LastReportfiles);
        }
        List<FileInfo> GetDeletedFiles(List<FileInfo> LastReportfiles, List<FileInfo> CurrentFiles)
        {
            return Exept(LastReportfiles, CurrentFiles);
        }
        List<FileInfo> Exept(List<FileInfo> l1, List<FileInfo> l2)
        {
            return l1.Except(l2).ToList();
        }
        FileInfo FindFileByName(List<FileInfo> files,string filename)
        {
            return files.Where(x => Equals(x.Name, filename)).First();
        }
    }
}
