using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            //Start server
            // 1) Make server object on port 1000
            Server server = new Server(1000, "redirectionRules.txt");
            // 2) Start Server
            server.StartServer();
            Console.WriteLine("The server has started ...");
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"C:\Users\hp\Desktop\Template[2021-2022]\HTTPServer\redirectionRules.txt
            string path = @"C:\Users\hp\Desktop\Template[2021-2022]\HTTPServer\redirectionRules.txt";
            string rules = "aboutus.html,aboutus2.html";
            File.WriteAllText(path, rules);
            // means that when making request to aboustus.html,, it redirects me to aboutus2
        }
         
    }
}
