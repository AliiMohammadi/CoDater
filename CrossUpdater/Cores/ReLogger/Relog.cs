using System;
using CoDater.Workspace;
using CoDater.Logger;
using SaveAndRetrieve;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using static SaveSystem.SaveData;
using System.Security.Policy;
using CrossUpdater.Cores.ReLogger;
using System.Data;

namespace CoDater.ReLogger
{
    internal class Relog
    {
        public string FileName = "CoDater.dat";

        RepositoryURL RepositoryURL;
        WorkSpace workspace;
        JsonSaveData SaveLoad;
        JavaScriptSerializer serializer;
        CloudReader cloudReader;

        public Relog(System.IO.DirectoryInfo workdirectory, Url repositoryURL)
        {
            workspace = new WorkSpace(workdirectory);
            SaveLoad = new JsonSaveData();
            cloudReader = new CloudReader();
            serializer = new JavaScriptSerializer();
            RepositoryURL = new RepositoryURL(repositoryURL);
        }

        public InterpretResult Interpret()
        {
            //Steps to interpret:
            //1: Check if file is exist.
            //2: Read the file.
            //3: Read the update online.
            //4: Compare reportes with the current version.
            //5: Apply new changes to the current work space.
            //6: Update the report data file.
            //Note: Steps 4 , 5 should implement to a sprate funtion.

            string datapath = workspace.WorkDirectory + "\\" + FileName;

            if (!System.IO.File.Exists(datapath))
                throw new Exception("The patch report file not found.");

            //string OnlineDatapath = RepositoryURL.Address.Value + "/" + FileName.Replace("\\", "/");
            string OnlineDatapath = ConvertFileNameToGitHubRawLink(RepositoryURL, new FileInfo(datapath));
            string OnlineReportdata = cloudReader.Read(OnlineDatapath);

            if(OnlineReportdata == null) 
                throw new Exception("Unable to get patch report file online.");

            List<ReportInfo> LocalReports = SaveLoad.LoadData<List<ReportInfo>>(datapath);
            List<ReportInfo> OnlineReports = serializer.Deserialize<List<ReportInfo>>(OnlineReportdata);

            //steps to compare two versions:
            //1: calculate the distance version.
            //2: Add deleted files to a list and delete the files if the removed files exists.
            //3: Gader all added files and moded files to a list and determin if they exist then remove them.
            //4: Download and replace newest files from list from step 3.

            InterpretResult Changes = CompareVersions(LocalReports, OnlineReports);

            ApplyChangesToVersion(Changes,workspace);

            return Changes;
        }

        private InterpretResult CompareVersions(List<ReportInfo> v1, List<ReportInfo> v2)
        {
            InterpretResult result = new InterpretResult();

            ReportInfo LastReport = v1.Last();

            List<ReportInfo> NewVersions = new List<ReportInfo>(v2);

            int VersionDistance = v2.Last().Version - LastReport.Version;

            NewVersions.RemoveRange(0, v2.Count - VersionDistance - 1);

            foreach (var item in NewVersions)
                foreach (var item2 in item.Files)
                    switch (item2.Status)
                    {
                        case FileState.FileStatus.Changed:
                            AddIfNotExist(result.ModedFiles, item2);
                            break;
                        case FileState.FileStatus.Removed:
                            AddIfNotExist(result.DeletedFiles, item2);
                            break;
                        case FileState.FileStatus.Added:
                            AddIfNotExist(result.AddedFiles,item2);
                            break;
                        default:
                            break;
                    }

            return result;
        }
        private void ApplyChangesToVersion(InterpretResult changes, WorkSpace workspace)
        {
            List<FileInfo> FilesToDelete = new List<FileInfo>();
            List<FileInfo> FilesToDonwload = new List<FileInfo>();

            FilesToDelete.AddRange(changes.DeletedFiles);
            FilesToDelete.AddRange(changes.ModedFiles);

            FilesToDonwload.AddRange(changes.ModedFiles);
            FilesToDonwload.AddRange(changes.AddedFiles);

            foreach (var item in FilesToDelete)
                DeleteIfExist(item);
            //A problem, Wha t about the times, when some file deleted and have problem to downloading file.
            foreach (var item in FilesToDonwload)
                DownloadFile(new Url(ConvertFileNameToGitHubRawLink(RepositoryURL,item)), item);
        }

        void AddIfNotExist(List<FileInfo> list ,FileState file )
        {

            if (!list.Exists(x => x.WorkName == workspace.WorkName(file)))
                list.Add(file);
        }
        void DeleteIfExist(FileInfo file)
        {
            string path = workspace.WorkDirectory.FullName + file.WorkName;

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
        void DownloadFile(Url url , FileInfo file )
        {
            try
            {
                string path = workspace.WorkDirectory.FullName + file.WorkName;

                string directo = System.IO.Path.GetDirectoryName(path);

                if (!System.IO.Directory.Exists(directo))
                    System.IO.Directory.CreateDirectory(directo);

                DownloadManager.Donwloader.DownloadFile(url.Value, path);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        string ConvertFileNameToGitHubRawLink(RepositoryURL repolink,FileInfo file)
        {
            string x = file.WorkName;

            x = x.Remove(0, x.IndexOf(repolink.Name) + repolink.Name.Length).Replace("\\", "/");
            x = @"https://raw.githubusercontent.com/" + repolink.Username+"/"+repolink.Name+ @"/refs/heads/master" + x;

            return x;
        }
        Url ConvertGitHubUrlFileToRawUrl(RepositoryURL url)
        {
            //need to test

            string final = url.Address.Value.Replace(@"https://github.com/", @"https://raw.githubusercontent.com/").Replace("/blob/master/", "/refs/heads/master/");
            return new Url(final);
        }

        //Report: Bug accure when you make a report and after you change the project folder location somewhere else.
    }
}
