using System;
using System.Collections.Generic;
using System.Text;

namespace CommandExecutor
{
    public class Command
    {
        public delegate void Dmethosd();
        public string Name { get; set; }
        public Dmethosd Function { get; set; }

        public Command(string name , Dmethosd function)
        {
            Name = name;
            Function = function;
        }
    }
}
