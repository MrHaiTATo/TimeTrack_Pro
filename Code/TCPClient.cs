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
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.GetStream().Write(data, 0, data.Length);                        
        }

        public void Write(byte[] data)
        {
            client.GetStream().Write(data, 0, data.Length);
        }

        public string Read()
        {                        
            List<byte> bytes = new List<byte>();
            int bytesRead = -1;
            while ((bytesRead = client.GetStream().ReadByte()) > -1)
            {
                bytes.Add((byte)bytesRead);
            }
            return Encoding.UTF8.GetString(bytes.ToArray());                        
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

        public void Close()
        {
            client.Close();
        }

    }
}
