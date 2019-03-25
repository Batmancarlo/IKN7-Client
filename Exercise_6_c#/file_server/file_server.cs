using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace tcp
{
    class file_server
    {
        /// <summary>
        /// The PORT
        /// </summary>
        const int PORT = 9000;
        /// <summary>
        /// The BUFSIZE
        /// </summary>
        const int BUFSIZE = 1000;

		IPAddress localAddr = IPAddress.Parse("10.0.0.1");

        /// <summary>
        /// Initializes a new instance of the <see cref="file_server"/> class.
        /// Opretter en socket.
        /// Venter på en connect fra en klient.
        /// Modtager filnavn
        /// Finder filstørrelsen
        /// Kalder metoden sendFile
        /// Lukker socketen og programmet
         /// </summary>
        private file_server ()
        {
            // TO DO Your own code
            TcpListener serverSocket = new TcpListener(localAddr,PORT);
			serverSocket.Start();
            
			while (true)
			{
				TcpClient clientSocket = default(TcpClient);
                Console.WriteLine(" >> Server Started");
				clientSocket = serverSocket.AcceptTcpClient();
				Console.WriteLine(" >> Accept connection from client");
			    NetworkStream networkStream = clientSocket.GetStream();
            
				string file = tcp.LIB.readTextTCP(networkStream);

				if (new FileInfo(file).Exists)
				{
					sendFile(file, new FileInfo(file).Length, networkStream);
				}
				else
				{
					Console.Write("Invalid File Name");
				}
			


				clientSocket.Close();
				networkStream.Close();
				Console.Write("File send, connection closed.");
			}
          
            
        
        }

        /// <summary>
        /// Sends the file.
        /// </summary>
        /// <param name='fileName'>
        /// The filename.
        /// </param>
        /// <param name='fileSize'>
        /// The filesize.
        /// </param>
        /// <param name='io'>
        /// Network stream for writing to the client.
        /// </param>
        private void sendFile (String fileName, long fileSize, NetworkStream io)
		{
            
            tcp.LIB.writeTextTCP(io, fileSize.ToString());
			int byteswritten = 0;
			byte[] fileBytes = File.ReadAllBytes(fileName);
			int bytesSend = 0;

			for (int i = (int)fileSize; i != 0;)
			{
				if(i>BUFSIZE)
				{
					io.Write(fileBytes, bytesSend, (int)BUFSIZE);
					bytesSend += BUFSIZE;
					i -= BUFSIZE;
				}
				else
				{
					io.Write(fileBytes, bytesSend, i);
					i -= i;
				}

            }

           
            
        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name='args'>
        /// The command-line arguments.
        /// </param>
        public static void Main (string[] args)
        {
            Console.WriteLine ("Server starts...");
            new file_server();
        }
    }
}
