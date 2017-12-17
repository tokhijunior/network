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

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket

            IPEndPoint iep = new IPEndPoint(IPAddress.Any, portNumber);
            this.serverSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            this.serverSocket.Bind(iep);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(1000);

            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                Socket clientSocket = this.serverSocket.Accept();

                Thread newthread = new Thread(new ParameterizedThreadStart(HandleConnection));

                newthread.Start(clientSocket);
                //TODO: accept connections and start thread for each accepted connection.

            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket clientSock = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            clientSock.ReceiveTimeout = 0;

            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] requestRecived = new byte[1024 * 1024];
                   int receivedLen = clientSock.Receive(requestRecived);

                    // TODO: break the while loop if receivedLen==0
                    if (receivedLen == 0)
                        break;
                    // TODO: Call HandleRequest Method that returns the response
                    string requeststring = Encoding.ASCII.GetString(requestRecived, 0, receivedLen);
                    Request request = new Request(requeststring);

                   Response response = HandleRequest(request);

                    // TODO: Send Response back to client
                    byte[] responsebytes= Encoding.ASCII.GetBytes( response.ResponseString);
                    clientSock.Send(responsebytes);

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSock.Close();
        }

        Response HandleRequest(Request request)
        {
            throw new NotImplementedException();
            string content;
            try
            {
                
                //TODO: check for bad request 
                
                if (request.ParseRequest())
                {
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                }

                //TODO: map the relativeURI in request to get the physical path of the resource.
                string physicalPath = Path.Combine(Configuration.RootPath, request.relativeURI);


                //TODO: check for redirect

                string redirectedPath = GetRedirectionPagePathIFExist(request.relativeURI);
                if (redirectedPath != "")
                {
                    physicalPath = redirectedPath;
                    
                }


                //TODO: check file exists
                if (!File.Exists(physicalPath))
                {
                    content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                }


                //TODO: read the physical file



                // Create OK response


            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 

            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty

            string redirectedPage = Configuration.RedirectionRules[relativePath];
            string filePath = Path.Combine(Configuration.RootPath, redirectedPage);
            if (File.Exists(filePath))
            {
                return filePath;
            }


            return string.Empty;
        }



        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string

          
            if (!File.Exists(filePath))
           {


           }
            
            // else read file and return its content
            return string.Empty;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                Configuration.RedirectionRules = new Dictionary<string, string>();
                // TODO: using the filepath paramter read the redirection rules from file 
                var lines = File.ReadLines(filePath);
                foreach (var line in lines)
                {
                    string[] Line = line.Split(',');
                    Configuration.RedirectionRules.Add(Line[0], Line[1]);

                }

                // then fill Configuration.RedirectionRules dictionary 
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
