using System;
using System.Collections.Generic;
using CoDater.Logger;
using CoDater.Workspace;
using CommandExecutor;

namespace CoDater
{
    public class Program
    {
        static Reporter reporter = new Reporter();
        static Executor ex = new Executor();

        static void Main(string[] args)
        {
            ex.AddCommand("report", ()=>{ Report(@"D:\CodaterTestRepoProject"); });
            ex.AddCommand("relog", () => { Relog(); });
            //Report(@"D:\CodaterTestRepoProject");
            Relog();
            //ex.Execute(args[0]);

            //oxidan

            Console.ReadKey();
        }

        static void Report(string WorkDirectory)
        {
            try
            {
                Print("Reporting...");
                reporter.WorkDirectory = new System.IO.DirectoryInfo(WorkDirectory);

                List<ReportInfo> reports = reporter.Report();

                foreach (var item in reports[reports.Count - 1].Files)
                    switch (item.Status)
                    {
                        case FileState.FileStatus.UnChanged:
                            Print($".: {item.WorkName}");
                            break;
                        case FileState.FileStatus.Changed:
                            Print($"*: {item.WorkName}", ConsoleColor.Yellow);
                            break;
                        case FileState.FileStatus.Added:
                            Print($"+: {item.WorkName}", ConsoleColor.Green);
                            break;
                        case FileState.FileStatus.Removed:
                            Print($"-: {item.WorkName}", ConsoleColor.Red);
                            break;
                        default:
                            break;
                    }

                Print("");
                Print($"Version:{reports[reports.Count - 1].Version}. Changes:{reports[reports.Count - 1].ChangesCount}");
                Print($"New files:{reports[reports.Count - 1].AddedFilesCount}. Deleted files:{reports[reports.Count - 1].DeletedFileCount}");
            }
            catch (Exception e)
            {

                Print(e.Message,ConsoleColor.Red);
            }

        }
        static void Relog()
        {
            try
            {
                Print("Reloging ...");
                ReLogger.Relog loger = new ReLogger.Relog(new System.IO.DirectoryInfo(@"C:\Users\msi PC\Desktop\CodaterTestRepoProject"), new System.Security.Policy.Url(@"https://github.com/AliiMohammadi/CodaterTestRepoProject"));
                ReLogger.InterpretResult res = loger.Interpret();

                foreach (var item in res.DeletedFiles)
                {
                    Print("-" + item.WorkName, ConsoleColor.Red);
                }
                foreach (var item in res.ModedFiles)
                {
                    Print("*" + item.WorkName, ConsoleColor.Yellow);
                }
                foreach (var item in res.AddedFiles)
                {
                    Print("+" + item.WorkName, ConsoleColor.Green);
                }

                Print("relog compelted.");
            }
            catch (Exception e)
            {
                Print(e.Message, ConsoleColor.Red);
            }
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
