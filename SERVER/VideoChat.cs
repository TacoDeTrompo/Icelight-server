using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using SERVER;

namespace Server
{
    class VideoChat
    {
        UdpClient Receptor = new UdpClient(2000);
        UdpClient Receptorb = new UdpClient(3000);
        public List<UserProfile> comunicacions;
        private List<Comunicacion> listcom;
        bool chatting;

        public VideoChat(UserProfile com1, UserProfile com2, List<Comunicacion> listcom)
        {
            Receptor.Client.ReceiveTimeout = 50;
            Receptorb.Client.ReceiveTimeout = 50;
            Receptorb.Client.Blocking = false;
            Receptor.Client.Blocking = false;


            comunicacions = new List<UserProfile>();
            this.listcom = listcom;
            chatting = true;
            comunicacions.Add(com1);
            comunicacions.Add(com2);

            Thread thread2 = new Thread(() => MantenerVChat(Receptorb, this, 2000));
            Thread thread = new Thread(() => MantenerVChat(Receptor, this, 3000));

            thread2.Start();
            thread.Start();
        }

        public bool isChatting()
        {
            return chatting;
        }
        public bool inChat(IPAddress address)
        {
            //foreach (IPAddress com in comunicacions)
            //{
            //    if (com.Equals(address))
            //        return true;
            //}
            //return false;
            return false;
        }
        private void MantenerVChat(UdpClient cliente,  VideoChat chat, int port)
        {
            Comunicacion aEnviar = null;
            while (chat.isChatting())
            {
                try
                {
                    IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
                    //Console.Write("Intento " + cliente.Client.LocalEndPoint.ToString());


                    Byte[] comoquieras = cliente.Receive(ref ip);
                    // ip Address Cliente , Puerto Clientom
                    Console.WriteLine("Recivido de " + cliente.Client.LocalEndPoint.ToString());

                    if (aEnviar == null)
                    {
                        Comunicacion comA = listcom.Find(x => x.getUsuario() == chat.comunicacions[0]);
                        Comunicacion comB = listcom.Find(x => x.getUsuario() == chat.comunicacions[1]);
                        if (ip.Address.Equals(comA.getIpEndPoint().Address))
                        {
                            aEnviar = comB;
                        }
                        else
                        {
                            aEnviar = comA;
                        }
                    }
                    //IPEndPoint ips = new IPEndPoint(comB.getIpEndPoint().Address, 11000);
                    //recep.Send(comoquieras, comoquieras.Length);

                    //IPEndPoint ips = new IPEndPoint(com.getIpEndPoint().Address, 11000);
                    //cliente.Send(comoquieras, comoquieras.Length, ips);
                    //Console.WriteLine("Enviado de " + cliente.Client.LocalEndPoint.ToString()
                    //     + " a " + com.getIpEndPoint().Address.ToString());
                    IPEndPoint ips = new IPEndPoint(aEnviar.getIpEndPoint().Address, port);
                    Console.WriteLine(aEnviar.getUsuario().username + " enviado de " + cliente.Client.LocalEndPoint.ToString()
                             + " a " + aEnviar.getIpEndPoint().Address.ToString());
                    cliente.Send(comoquieras, comoquieras.Length, ips);

                    //if (ip.Address.Equals(comA.getIpEndPoint().Address))
                    //{
                    //    IPEndPoint ips = new IPEndPoint(comB.getIpEndPoint().Address, 11000);
                    //    Console.WriteLine(comA.getUsuario().getNombre() + " enviado de " + cliente.Client.LocalEndPoint.ToString()
                    //         + " a " + comB.getIpEndPoint().Address.ToString());
                    //    cliente.Send(comoquieras, comoquieras.Length, ips);
                    //}
                    //else
                    //{
                    //    IPEndPoint ips = new IPEndPoint(comA.getIpEndPoint().Address, 11000);
                    //    Console.WriteLine(comB.getUsuario().getNombre() + " enviado de " + cliente.Client.LocalEndPoint.ToString() + " a " + comA.getIpEndPoint().Address.ToString());
                    //    cliente.Send(comoquieras, comoquieras.Length, ips);
                    //}

                }
                catch (Exception x)
                {
                    //Console.WriteLine(" fallido");
                    int a = 0;
                }
            }
            cliente.Close();
        }
    }
}
