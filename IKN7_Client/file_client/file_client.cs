using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;

      
		private file_client (string[] args)
		{
			string ServerIP = "10.0.0.2";
			Console.Write("Type in L or l for loadaverage from server\nType in U og u for server uptime\n");
			while(true)
			{

			string CommandToSend = Console.ReadLine();
			byte[] CommandToSendByte = Encoding.ASCII.GetBytes(CommandToSend);

            UdpClient client = new UdpClient();
            IPEndPoint ClientEndPoint = new IPEndPoint(IPAddress.Parse(ServerIP), PORT);
            client.Connect(ClientEndPoint);
			client.Send(CommandToSendByte, CommandToSendByte.Length);
            client.Close();
            
			UdpClient server = new UdpClient(PORT);
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, PORT);
            byte[] DataRecievedByte = server.Receive(ref serverEndPoint);
			string DataRecieved = Encoding.ASCII.GetString(DataRecievedByte);


			Console.Write(DataRecieved);


			}
        }

	

		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
