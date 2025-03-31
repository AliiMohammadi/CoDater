using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDater
{
    public class Program
    {
        static Logger.WorkSpace logger = new Logger.WorkSpace();

        static void Main(string[] args)
        {
            List<string> l1=new List<string> { "Apple","Banana","Peach","watermelon"};
            List<string> l2=new List<string> { "Apple","Oringe", "Peach" };
            foreach (string s in l1.Intersect(l2))
                Console.WriteLine(s);
            Console.ReadKey();
        }
    }
}
