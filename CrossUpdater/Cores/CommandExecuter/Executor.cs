using System;
using System.Collections.Generic;
using static CommandExecutor.Command;

namespace CommandExecutor
{
    //A class for excuting function or method using user input.
    public class Executor
    {
        /// <summary>
        /// All Commands
        /// </summary>
        public List<Command> Commands = new List<Command>();
        public int CommandCount
        {
            get
            {
                return Commands.Count;
            }
        }
        
        public void AddCommand(string CommandName , Dmethosd function)
        {
            Commands.Add(new Command(CommandName, function));
        }
        /// <summary>
        /// Executing Command with a string.If command not defined,It will return false
        /// </summary>
        /// <param name="command">Name of the command</param>
        /// <returns>It will return false if command not defined</returns>
        public bool Execute(string command)
        {
            bool Valid = false;
            command = StandardString(command);

            if (!Valid)
            {
                foreach (Command item in Commands)
                {
                    string Ap = StandardString(item.Name);

                    if (string.Equals(command,Ap))
                    {
                        item.Function();
                        Valid = true;
                        goto Quit;
                    }
                }
            }

            goto Quit;

            Quit:
            return Valid;
        }
        public bool DeletCommand(string CommandName)
        {
            bool Valid = false;

            if (!Valid)
            {
                foreach (Command item in Commands)
                {
                    string Ap = StandardString(item.Name);

                    if (string.Equals(CommandName, Ap))
                    {
                        Commands.Remove(item);
                        Valid = true;
                        goto Quit;
                    }
                }
            }

            goto Quit;

            Quit:
            return Valid;
        }
        public void DeletCommand(int CommandIndex)
        {
            try
            {
                Commands.RemoveAt(CommandIndex);
            }
            catch { }   
        }

        string StandardString(string Command)
        {
            string Std = Command;

            try
            {
                Std = Std.ToLower();
            }
            catch { }
            try
            {
                Std = Std.Replace(" ", "");
            }
            catch { }

            return Std;
        }
    }
}
//All copy right for Ali Mohammadi- CrossDeve
