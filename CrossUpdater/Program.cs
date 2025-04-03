using System;
using System.Collections.Generic;
using CoDater.Logger;
using System.Linq;

namespace CoDater
{
    public class Program
    {
        static Reporter reporter = new Reporter();

        static void Main(string[] args)
        {
            ExeptTest();
            Console.ReadKey();

        }

        static void ExeptTest()
        {

            List<FileState> fileStates = new List<FileState>();

            FileState f1 = new FileState(@"D:\MyFile1.txt", FileState.FileStatus.UnChanged);
            FileState f2 = new FileState(@"D:\MyFile2.txt", FileState.FileStatus.UnChanged);
            FileState f3 = new FileState(@"D:\MyFile3.txt", FileState.FileStatus.UnChanged);

            fileStates.Add(f1);
            fileStates.Add(f2);
            fileStates.Add(f3);

            List<FileInfo> fileinfo = new List<FileInfo>();

            FileInfo y1 = new FileInfo(@"D:\MyFile1.txt");
            FileInfo y3 = new FileInfo(@"D:\MyFile3.txt");
            FileInfo y4 = new FileInfo(@"D:\MyFile4.txt");


            //y3.LastWriteTime = DateTime.Now;
            //y3.Length = y3.Length + 5;

            fileinfo.Add(y1);
            fileinfo.Add(y3);
            fileinfo.Add(y4);

            List<FileInfo> exe = reporter.ConvertSTATEtoINFO(fileStates).Except(fileinfo).ToList();

            Console.WriteLine("Deleted files only :");

            foreach (FileInfo x in exe)
                Console.WriteLine(x.Name);

        }
        static void IntersectTest()
        {

            List<FileState> fileStates = new List<FileState>();

            FileState f1 = new FileState(@"D:\MyFile1.txt", FileState.FileStatus.UnChanged);
            FileState f2 = new FileState(@"D:\MyFile2.txt", FileState.FileStatus.UnChanged);
            FileState f3 = new FileState(@"D:\MyFile3.txt", FileState.FileStatus.UnChanged);

            fileStates.Add(f1);
            fileStates.Add(f2);
            fileStates.Add(f3);

            List<FileInfo> fileinfo = new List<FileInfo>();

            FileInfo y1 = new FileInfo(@"D:\MyFile1.txt");
            FileInfo y3 = new FileInfo(@"D:\MyFile3.txt");
            FileInfo y4 = new FileInfo(@"D:\MyFile4.txt");

            y3.LastWriteTime = DateTime.Now;
            y3.Length = y3.Length + 5;

            fileinfo.Add(y1);
            fileinfo.Add(y3);
            fileinfo.Add(y4);

            List<FileInfo> intersect = reporter.Intersect(reporter.ConvertSTATEtoINFO(fileStates), fileinfo);

            Console.WriteLine("Remained files(No changed files.) :");

            foreach (FileInfo x in intersect)
                Console.WriteLine(x.Name);

        }
        static void Report()
        {
            Console.WriteLine("Reporting...");
            List<ReportInfo> reports = reporter.Report();

            foreach (var item in reports[reports.Count - 1].Files)
                switch (item.Status)
                {
                    case FileState.FileStatus.UnChanged:
                        Console.WriteLine($"File: {item.Name}");
                        break;
                    case FileState.FileStatus.Changed:
                        Console.WriteLine($"Moded File: {item.Name}");
                        break;
                    case FileState.FileStatus.Added:
                        Console.WriteLine($"+File: {item.Name}");
                        break;
                    case FileState.FileStatus.Removed:
                        Console.WriteLine($"-File: {item.Name}");
                        break;
                    default:
                        break;
                }
        }
    }
}
