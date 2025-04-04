﻿using System;
using System.Collections.Generic;
using CoDater.Logger;
using System.IO;

namespace CoDater
{
    public class Program
    {
        static Reporter reporter = new Reporter();

        static void Main(string[] args)
        {
            Report(@"D:\Project");
            Console.ReadKey();
        }

        static void Report(string WorkDirectory)
        {
            reporter.WorkDirectory = new DirectoryInfo(WorkDirectory);

            Print("Reporting...");
            List<ReportInfo> reports = reporter.Report();

            foreach (var item in reports[reports.Count - 1].Files)
                switch (item.Status)
                {
                    case FileState.FileStatus.UnChanged:
                        Print($"File: {item.Name}");
                        break;
                    case FileState.FileStatus.Changed:
                        Print($"Moded File: {item.Name}",ConsoleColor.Yellow);
                        break;
                    case FileState.FileStatus.Added:
                        Print($"+File: {item.Name}",ConsoleColor.Green);
                        break;
                    case FileState.FileStatus.Removed:
                        Print($"-File: {item.Name}",ConsoleColor.Red);
                        break;
                    default:
                        break;
                }
            Print("");
            Print($"Version:{reports[reports.Count-1].Version}. Changes:{reports[reports.Count - 1].ChangesCount}");
            Print($"New files:{reports[reports.Count-1].AddedFilesCount}. Deleted files:{reports[reports.Count - 1].DeletedFileCount}");
        }

        static void Print(object message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Print(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void Print(object message)
        {
            Console.WriteLine(message);
        }
    }
}
