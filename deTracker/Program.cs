using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace deTracker
{
    class Program
    {       
        static void Main(string[] args)
        {
            DataTable log = Operations.Logreader(@"F:\_BZ\BTSy\log.log");
            Operations.FindBTS_orange(log);
            //Operations.PrintToConsole(log);
            //Operations.SaveToCSV(log, @"C:\Users\BZaniews\Desktop\2018_11_07_16_48_55.csv");
            Console.ReadKey();
        }
    }
}
