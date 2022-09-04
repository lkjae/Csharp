namespace TCP_ServerClient
{
	class Program
	{
		static void Main(string[] args)
		{
			//서버 소켓이 동작하는 스레드
			Thread serverThread = new Thread(serverFunc);
			serverThread.IsBackground = true;
			serverThread.Start();
			Thread.Sleep(500);	//소켓 서버용 스레드가 실행될 시간을 주기 위한 sleep
			
			// 클라이언트 소켓이 동작하는 스레드
			Thread clientThread = new Thread(clientFunc);
			clientThread.IsBackground = true;
			clientThread.Start();
			clientThread.Join();
			
			Console.WriteLine("종료하려면 엔터키를 누르세요...");
			Console.ReadLine();
		}
	
		private static void serverFunc(object obj)
		{
			using(Socket srvSocket = new Socket(AddressFamily.InterNetwork,
												SocketType.Stream,
												ProtocolType.Tcp))	// TCP 통신소켓 생성
			{
				IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11200);	//endPoint로 어디에 보낼것인지 IP주소와 Port번호
			
				srvSocket.Bind(endPoint);	
				srvSocket.Listen(10);			// backlog 큐 연결 개수
			
				while(true)
				{
					Socket clntSocket = srvSocket.Accept();	// 클라이언트의 연결을 기다림
				
					byte[] recvBytes = new byte[1024];		// 수신된 데이터를 담을 공간
					int nRecv = clntSocket.Receive(recvBytes);	// 수신된 데이터를 recvBytes에 저장 후 nRecv에 int형 recvBytes의 길이 저장
				
					string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);	//GetString(인코딩할 문자 집합, 인코딩할 첫번째 인덱스, 인코딩할 문자의 길이)
					byte[] sendBytes = Encoding.UTF8.GetBytes("Hello: " + txt);
					clntSocket.Send(sendBytes);		//송신
					clntSocket.Close();
				}
			}
		}
		
		private static void clientFunc(object obj)
		{
			Socket socket = new Socket(AddressFamily.InterNetwork,
										SocketType.Stream,
										ProtocolType.Tcp);
			EndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 11200);
			socket.Connect(serverEP);
			
			byte[] buf = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
			socket.Send(buf);
			
			Byte[] recvBytes = new byte[1024];
			int nRecv = socket.Receive(recvBytes);
			
			string txt = Encoding.UTF.GetString(recvBytes, 0, nRecv	);
			Console.WriteLine(txt);
			socket.Close();
			Console.WriteLine("TCP Client socket: Closed");
		}
	}
}
			
		
							
