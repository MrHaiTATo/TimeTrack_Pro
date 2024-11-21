using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Code
{
    public class TCPClient
    {
        private string ip;
        private int port;
        private TcpClient client;
        public TCPClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            client = new TcpClient();
        }

        public bool Connected => client.Connected;       

        public void Connect()
        {            
            client.Connect(ip, port);
        }

        public void Write(string message)
        {                       
            byte[] data = Encoding.ASCII.GetBytes(message);
            client.GetStream().Write(data, 0, data.Length);                        
        }

        public void Write(byte[] data)
        {
            client.GetStream().Write(data, 0, data.Length);
        }

        public string Read()
        {                                    
            return Encoding.ASCII.GetString(ReadBytes());                        
        }

        public async Task<string> ReadAsync()
        {
            byte[] bytes = await ReadBytesAsync();
            return Encoding.ASCII.GetString(bytes);
        }

        public byte[] ReadBytes()
        {
            List<byte> bytes = new List<byte>();
            int bytesRead = -1;
            while ((bytesRead = client.GetStream().ReadByte()) > -1)
            {
                bytes.Add((byte)bytesRead);
            }
            return bytes.ToArray();
        }

        public async Task<byte[]> ReadBytesAsync()
        {
            List<byte> data = new List<byte>();
            int ret = 0;
            byte[] bytes = new byte[1024];
            do
            {
                ret = await client.GetStream().ReadAsync(bytes, 0, bytes.Length);
                data.AddRange(bytes);
                Array.Clear(bytes, 0, 1024);
                if (0 <= ret && ret < 1024)
                {
                    break;
                }
            }
            while (true);
            return data.ToArray();
        }

        public void Close()
        {
            client.GetStream().Close();
            client.Close();
        }

    }
}
