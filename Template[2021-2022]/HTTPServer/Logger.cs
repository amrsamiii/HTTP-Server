using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 
            string path = @"C:\Users\hp\Desktop\Template[2021-2022]\HTTPServer\log.txt";
            //FileStream file = new FileStream(path, FileMode.OpenOrCreate);
            //StreamWriter filewrite = new StreamWriter(file);
            //filewrite.WriteLine("Datetime:" + DateTime.Now.ToString());
            //filewrite.WriteLine("message :" + ex.Message);

            File.AppendAllLines(path, new List<string>() { DateTime.Now + " : " + ex.Message + System.Environment.NewLine });

            //file.Close();
        }
    }
}