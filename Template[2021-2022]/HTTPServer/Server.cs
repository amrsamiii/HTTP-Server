using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;
        int portNumber;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            this.portNumber = portNumber;

            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);

            //TODO: initialize this.serverSocket

            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            IPEndPoint serverEndPoint = iPEndPoint;
            serverSocket.Bind(serverEndPoint);

        }

        public void StartServer()
        {
            Console.WriteLine("Waiting for Connections");
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(100);

            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {

                //TODO: accept connections and start thread for each accepted connection.
                Socket clientSocket = this.serverSocket.Accept();
                Thread newthread = new Thread(new ParameterizedThreadStart(HandleConnection));
                // START THREAD
                newthread.Start(clientSocket);


            }
        }

        public void HandleConnection(object obj)
        {
            Console.WriteLine("Conneceted to client");
            // TODO: Create client socket - Casting
            Socket clientSocket = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            clientSocket.ReceiveTimeout = 0;

            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] receivedData = new byte[1024*1024];
                    int receivedLen = clientSocket.Receive(receivedData);
                    string data = Encoding.ASCII.GetString(receivedData);
                    // TODO: break the while loop if receivedLen==0
                    if (receivedLen == 0)
                    {
                        break;
                    }
                    // TODO: Create a Request object using received request string
                    Request clientReq = new Request(data);
                    // TODO: Call HandleRequest Method that returns the response
                    Response servRes = HandleRequest(clientReq);
                    string resp = servRes.ResponseString;
                    byte[] response = Encoding.ASCII.GetBytes(resp);
                    Console.WriteLine(resp);
                    // TODO: Send Response back to client
                    clientSocket.Send(response);

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            // throw new NotImplementedException();
            string content;
            string redirectionPath = string.Empty;
            StatusCode status_code = StatusCode.OK;
           
            try
            {
                //TODO: check for bad request
                if (!request.ParseRequest())
                {

                    status_code = StatusCode.BadRequest;



                }

                //TODO: map the relativeURI in request to get the physical path of the resource.

                string physPath = Configuration.RootPath + request.relativeURI;

                //TODO: check for redirect

                redirectionPath = GetRedirectionPagePathIFExist(request.relativeURI);
                if (String.IsNullOrEmpty(redirectionPath) != true)
                {
                    content = LoadDefaultPage(Configuration.RedirectionDefaultPageName);
                    physPath = Configuration.RootPath + '\\' + Configuration.RedirectionDefaultPageName;
                   
                    status_code = StatusCode.Redirect;
                }


                //TODO: check file exists
                if (!File.Exists(physPath))
                {
                    physPath = Configuration.RootPath + '\\' + Configuration.NotFoundDefaultPageName;

                    
                    status_code = StatusCode.NotFound;

                }

                
                // Create OK response
                content = LoadDefaultPage(physPath);

            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                String physPath = Configuration.RootPath + '\\' + "InternalError.html";

                content = File.ReadAllText(physPath);
                status_code = StatusCode.InternalServerError;
            }

            Response resp = new Response(status_code, "text/html", content, redirectionPath);
            return resp;
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            string RedirectionPath;
            if (relativePath[0] == '/')
            {
                relativePath = relativePath.Substring(1);
            }
            bool pageExist = Configuration.RedirectionRules.TryGetValue(relativePath, out RedirectionPath);
            if (pageExist)
            {
                return RedirectionPath;
            }
            else
            {
                return string.Empty;
            }


        }

        private string LoadDefaultPage(string defaultPageName)
        {
            String content = " ";
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            try
            {
                if (File.Exists(filePath))
                {
                    content = File.ReadAllText(filePath);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

            }

            // else read file and return its content
            return content;

        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: Using filepath parameter read rediriction 
                FileStream fs = new FileStream(filePath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                // Fill Configuration.Redirection rules Dicitonary 
                Configuration.RedirectionRules = new Dictionary<string, string>();
                while (sr.Peek() != -1)
                {
                    string lineadd = sr.ReadLine();
                    string[] dicData = lineadd.Split(',');
                    if (dicData[0] == "")
                    {
                        break;
                    }
                    Configuration.RedirectionRules.Add(dicData[0], dicData[1]);
                }
                fs.Close();
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
