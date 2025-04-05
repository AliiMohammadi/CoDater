using System;
using CoDater.Workspace;
using CoDater.Logger;
using SaveAndRetrieve;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace CoDater.ReLogger
{
    internal class Relog
    {
        public string FileName = "CoDater.dat";

        string RepositoryURL;
        WorkSpace workspace;
        JsonSaveData SaveLoad;
        JavaScriptSerializer serializer;
        CloudReader cloudReader;

        public Relog(System.IO.DirectoryInfo workdirectory, string repositoryURL)
        {
            workspace = new WorkSpace(workdirectory);
            SaveLoad = new JsonSaveData();
            cloudReader = new CloudReader();
            serializer = new JavaScriptSerializer();
            RepositoryURL = repositoryURL;
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
                throw new Exception("The report data file not found.");

            string OnlineDatapath = RepositoryURL + "\\" + FileName;
            string OnlineReportdata = cloudReader.Read(OnlineDatapath);

            InterpretResult result = new InterpretResult();

            List<ReportInfo> LocalReports = SaveLoad.LoadData<List<ReportInfo>>(datapath);
            List<ReportInfo> OnlineReports = serializer.Deserialize<List<ReportInfo>>(OnlineReportdata);

            InterpretResult Changes = CompareVersions(LocalReports, OnlineReports);
            ApplyChangesToVersion(Changes,workspace);
            return result;
        }

        InterpretResult CompareVersions(List<ReportInfo> v1, List<ReportInfo> v2)
        {
            //steps to compare two versions:
            //1: calculate the distance version.
            //2: Add deleted files to a list and deleted the files if the removed files exists.
            //3: Gader all added files and moded files to a list and determin if they exist then remove them.
            //4: Download and replace newest files from list from step 3.
            ReportInfo LastReport = v1.Last();

            int VersionDistance = v2.Last().Version - LastReport.Version;
        }

        void ApplyChangesToVersion(InterpretResult changes, WorkSpace workspace)
        {

        }
    }
}
