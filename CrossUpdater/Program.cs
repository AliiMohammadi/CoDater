using System;
using System.Collections.Generic;
using CoDater.Logger;

namespace CoDater
{
    public class Program
    {
        static Reporter reporter = new Reporter();

        static void Main(string[] args)
        {
            Report();
            Console.ReadKey();
        }

        static void Report()
        {
            Console.WriteLine("Reporting...");
            List<ReportInfo> reports = reporter.Report();

            foreach (var item in reports[reports.Count-1].Files)
                switch (item.Status)
                {
                    case FileState.FileStatus.UnChanged:
                        Console.WriteLine($"File: {item.Fileinfo.Name}");
                        break;
                    case FileState.FileStatus.Changed:
                        Console.WriteLine($"Moded File: {item.Fileinfo.Name}");
                        break;
                    case FileState.FileStatus.Added:
                        Console.WriteLine($"+File: {item.Fileinfo.Name}");
                        break;
                    case FileState.FileStatus.Removed:
                        Console.WriteLine($"-File: {item.Fileinfo.Name}");
                        break;
                    default:
                        break;
                }
        }
    }
}
