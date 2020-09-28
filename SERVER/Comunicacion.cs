using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using SERVER;

namespace Server
{
    class Comunicacion
    {
        UserProfile user;
        IPEndPoint ipep;
        Socket soc;
        
        public Comunicacion(UserProfile user, Socket socket)
        {
            ipep = (IPEndPoint)socket.RemoteEndPoint;
            
            this.user = user;
            soc = socket;
        }
        public void SendMessage(string message)
        {
            //soc.Send()

            byte[] data = Encoding.ASCII.GetBytes(message);
            soc.Send(data, data.Length, SocketFlags.None);
        }
        public UserProfile getUsuario()
        {
            return user;
        }
        public IPEndPoint getIpEndPoint()
        {
            return ipep;
        }
    }
}
