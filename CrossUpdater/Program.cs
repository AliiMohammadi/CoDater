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

            Console.ReadKey();
        }

        static void Report(string WorkDirectory)
        {
            try
            {
                reporter.WorkDirectory = new System.IO.DirectoryInfo(WorkDirectory);

                List<ReportInfo> reports = reporter.Report();
                Print("Reporting...");

                foreach (var item in reports[reports.Count - 1].Files)
                    switch (item.Status)
                    {
                        case FileState.FileStatus.UnChanged:
                            Print($".: {item.Name}");
                            break;
                        case FileState.FileStatus.Changed:
                            Print($"*: {item.Name}", ConsoleColor.Yellow);
                            break;
                        case FileState.FileStatus.Added:
                            Print($"+: {item.Name}", ConsoleColor.Green);
                            break;
                        case FileState.FileStatus.Removed:
                            Print($"-: {item.Name}", ConsoleColor.Red);
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
                ReLogger.Relog loger = new ReLogger.Relog(new System.IO.DirectoryInfo(@"C:\Users\msi PC\Desktop\CodaterTestRepoProjectOLD"), new System.Security.Policy.Url(@"https://github.com/AliiMohammadi/CodaterTestRepoProject"));
                ReLogger.InterpretResult res = loger.Interpret();
                Print("Reloging ...");

                foreach (var item in res.DeletedFiles)
                {
                    Print("-" + item.Name, ConsoleColor.Red);
                }
                foreach (var item in res.ModedFiles)
                {
                    Print("*" + item.Name, ConsoleColor.Yellow);
                }
                foreach (var item in res.AddedFiles)
                {
                    Print("+" + item.Name, ConsoleColor.Green);
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
