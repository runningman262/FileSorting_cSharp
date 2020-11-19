using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSorting
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // 1. set the directory of the files to sort
            Console.Write("Enter the origin file directory: ");
            string origin = Console.ReadLine();

            Console.Write("Enter the destination file directory: ");
            string destination = Console.ReadLine();

            Files files = new Files(origin, destination);
            Console.WriteLine("\n" + files.MoveFiles() + "\n");

        }
    }
}
