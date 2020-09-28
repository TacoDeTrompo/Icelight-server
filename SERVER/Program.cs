//SERVIDOR
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Server;
using SERVER;

public class SimpleTcpSrvr
{
    static UdpClient Receptor = new UdpClient(5000);
    static UdpClient Receptorb = new UdpClient(4000);

    //const string ArchivoUsuarios = "Data/Usuarios-Copy.txt";
    //const string ArchivoConversaciones = "Data/Conversaciones.txt";
    //const string ArchivoContactos = "Data/Contactos.txt";
    private static List<Socket> socs;
    private static List<Comunicacion> listcom;
    private static ControladorConversaciones cConversaciones = new ControladorConversaciones();
    private static ControladorUsuarios cUsuarios = new ControladorUsuarios();
    private static List<VideoChat> videoChats = new List<VideoChat>();

    public static void Main()
    {
        Receptor.Client.ReceiveTimeout = 10;
        Receptor.Client.Blocking = false;

        Receptorb.Client.ReceiveTimeout = 10;
        Receptorb.Client.Blocking = false;

        System.Timers.Timer pruebaTimer = new System.Timers.Timer();
        pruebaTimer.Elapsed += pruebaTimer_Elapsed;
        pruebaTimer.Interval = 50;

        System.Timers.Timer chatTimer = new System.Timers.Timer();
        chatTimer.Elapsed += ChatTimer_Elapsed;
        chatTimer.Interval = 50;

        socs = new List<Socket>();
        listcom = new List<Comunicacion>();

        int recv;
        byte[] data = new byte[1024];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);

        Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        newsock.Bind(ipep);
        newsock.Listen(10);

        Console.WriteLine("Esperando por un cliente...");


        Socket client;

        for (int a = 0; a < 100; a++)
        {
            client = newsock.Accept();

            socs.Add(client);

            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Conectado con {0} por el puerto {1}", clientep.Address, clientep.Port);

            Thread T = new Thread(MantenerConvo);
            T.Start();
        }

        newsock.Close();
        Console.ReadKey();
    }

    private static void ChatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
            Byte[] comoquieras = Receptorb.Receive(ref ip);
            // ip Address Cliente , Puerto Clientom

            foreach (VideoChat vc in videoChats)
            {

                Comunicacion comA = listcom.Find(x => x.getUsuario() == vc.comunicacions[0]);
                Comunicacion comB = listcom.Find(x => x.getUsuario() == vc.comunicacions[1]);

                if (ip.Address.Equals(comA.getIpEndPoint().Address))
                {
                    IPEndPoint ips = new IPEndPoint(comB.getIpEndPoint().Address, 11000);
                    Receptorb.Send(comoquieras, comoquieras.Length, ips);
                    Console.Write(comoquieras);
                }
                else
                {
                    IPEndPoint ips = new IPEndPoint(comA.getIpEndPoint().Address, 11000);
                    Receptorb.Send(comoquieras, comoquieras.Length, ips);
                    Console.Write(comoquieras);
                }
            }
        }
        catch (Exception x)
        {
            int a = 0;
        }
    }

    private static void pruebaTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
            Byte[] comoquieras = Receptor.Receive(ref ip);
            // ip Address Cliente , Puerto Clientom

            foreach (VideoChat vc in videoChats)
            {

                Comunicacion comA = listcom.Find(x => x.getUsuario() == vc.comunicacions[0]);
                Comunicacion comB = listcom.Find(x => x.getUsuario() == vc.comunicacions[1]);

                if (ip.Address.Equals(comA.getIpEndPoint().Address))
                {
                    IPEndPoint ips = new IPEndPoint(comB.getIpEndPoint().Address, 11000);
                    Receptor.Send(comoquieras, comoquieras.Length, ips);
                    Console.Write(comoquieras);
                }
                else
                {
                    IPEndPoint ips = new IPEndPoint(comA.getIpEndPoint().Address, 11000);
                    Receptor.Send(comoquieras, comoquieras.Length, ips);
                    Console.Write(comoquieras);
                }
            }
        }
        catch (Exception x)
        {
            int a = 0;
        }
    }

    private static void MantenerVChat(UdpClient cliente, VideoChat chat)
    {
        while (chat.isChatting())
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
                Byte[] comoquieras = cliente.Receive(ref ip);
                // ip Address Cliente , Puerto Clientom

                Comunicacion comA = listcom.Find(x => x.getUsuario() == chat.comunicacions[0]);
                Comunicacion comB = listcom.Find(x => x.getUsuario() == chat.comunicacions[1]);

                if (ip.Address.Equals(comA.getIpEndPoint().Address))
                {
                    IPEndPoint ips = new IPEndPoint(comB.getIpEndPoint().Address, 11000);
                    Console.Write(comA.getUsuario().username);
                    cliente.Send(comoquieras, comoquieras.Length, ips);
                }
                else
                {
                    IPEndPoint ips = new IPEndPoint(comA.getIpEndPoint().Address, 11000);
                    Console.Write(comB.getUsuario().username);
                    cliente.Send(comoquieras, comoquieras.Length, ips);
                }

            }
            catch (Exception x)
            {
                int a = 0;
            }
        }
        cliente.Close();
    }



    public static void MantenerConvo()
    {
        Socket client = socs[socs.Count - 1];

        byte[] data = new byte[1024];
        int recv;

        recv = client.Receive(data);
        string login = Encoding.ASCII.GetString(data, 0, recv);

        string[] receivedStrings = login.Split('#');
        if (receivedStrings[0] == "LOGIN")
        {
            if (cUsuarios.LoginUserVerification(receivedStrings[1], receivedStrings[2]))
            {
                UserProfile auxUser = new UserProfile();

                auxUser = cUsuarios.GetUserProfileByCredentials(receivedStrings[1], receivedStrings[2]);

                cUsuarios.ConnectedUserProfile(auxUser);

                string sendStringLogin = "LoginCorrecto#" + auxUser.idUser.ToString() + "#" + auxUser.name +
                    "#" + auxUser.username + "#" + auxUser.email + "#" + auxUser.status.ToString() +
                    "#" + auxUser.points.ToString();

                data = Encoding.ASCII.GetBytes(sendStringLogin);

                client.Send(data, data.Length, SocketFlags.None);

                client.Receive(data);

                string connectedUser = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine(connectedUser);

                //Enviar los Contactos del Usuario
                SendFriendsToClient(client, data, recv, auxUser);

                //Enviar las Conversaciones del Usuario
                SendConversationsToClient(client, data, recv, auxUser);

                //Enviar los participantes de conversaciones
                SendParticipantsToClient(client, data, recv, auxUser);

                //Enviar los mensajes
                SendMessagesToClient(client, data, recv, auxUser);

                Comunicacion local = new Comunicacion(auxUser, client);
                listcom.Add(local);

                /////////////////////////////
                foreach (UserProfile usu in cUsuarios.GetAllFriends(auxUser.idUser))
                {
                    Comunicacion com = listcom.Find(x => x.getUsuario().idUser == usu.idUser);
                    if (com != null)
                    {
                        com.SendMessage("SERVER$CHANGESTATE$" + auxUser.idUser + "$" + 1);
                    }
                }

                /////////////////////////

                //Recivir mensajes del Usuario
                while (true)
                {

                    data = new byte[1024];
                    recv = client.Receive(data);
                    if (recv == 0)
                    {
                        Console.WriteLine("Se ha Desconectado {0}", ((IPEndPoint)client.RemoteEndPoint).Address);
                        
                        cUsuarios.DesconectUserProfile(auxUser);
                        break;
                    }
                    string men = Encoding.ASCII.GetString(data, 0, recv);
                    string[] datos = men.Split('$');

                    Console.WriteLine(men);

                    //local.SendMessage(men);
                    if (datos[0] != "SERVER")
                    {

                        string mensajeConv = datos[1].Substring(datos[1].IndexOf('-') + 1);

                        cConversaciones.AddMessagetoConv(Int32.Parse(datos[0]), local.getUsuario(), mensajeConv);
                        Conversation conv = cConversaciones.GetConversation(Int32.Parse(datos[0]));

                        foreach (UserProfile usu in cConversaciones.GetAllParticipantsProfile(conv))
                        {
                            Comunicacion com = listcom.Find(x => x.getUsuario().idUser == usu.idUser);
                            if (com != null)
                            {
                                com.SendMessage(men);
                            }
                        }
                    }
                    else
                    {

                        switch (datos[1])
                        {
                            case "ASKCHAT":
                                Comunicacion comu = listcom.Find(x => x.getUsuario().idUser == int.Parse(datos[3]));
                                if (comu != null)
                                    comu.SendMessage("SERVER$ASKCHAT$" + datos[2]);
                                break;
                            case "ACCCHAT":
                                Comunicacion coma = listcom.Find(x => x.getUsuario().idUser == int.Parse(datos[3]));
                                Comunicacion comb = listcom.Find(x => x.getUsuario().idUser == int.Parse(datos[2]));
                                coma.SendMessage("SERVER$ACCCHAT$" + datos[2]);
                                comb.SendMessage("SERVER$ACCCHAT$" + datos[3]);
                                videoChats.Add(new VideoChat(coma.getUsuario(), comb.getUsuario(), listcom));

                                break;
                            case "DENCHAT":

                                break;
                            case "ADDUSER":

                                break;
                            case "ADDCONVE":
                                break;
                            case "CHANGESTATE":
                                foreach (UserProfile usu in cUsuarios.GetAllFriends(int.Parse(datos[2])))
                                {
                                    Comunicacion com = listcom.Find(x => x.getUsuario().idUser == usu.idUser);
                                    if (com != null)
                                    {
                                        com.SendMessage("SERVER$CHANGESTATE$" + datos[2] + "$" + datos[3]);
                                    }
                                }
                                break;
                            case "LEVELUP":
                                foreach (UserProfile usu in cUsuarios.GetAllFriends(int.Parse(datos[2])))
                                {
                                    Comunicacion com = listcom.Find(x => x.getUsuario().idUser == usu.idUser);
                                    if (com != null)
                                    {
                                        com.SendMessage("SERVER$LEVELUP$" + datos[2] + "$" + datos[3]);
                                    }
                                }
                                break;
                            case "BUZZ":
                                Conversation conv = cConversaciones.GetConversation(Int32.Parse(datos[2]));

                                foreach (UserProfile usu in cConversaciones.GetAllParticipantsProfile(conv))
                                {
                                    Comunicacion com = listcom.Find(x => x.getUsuario().idUser == usu.idUser);
                                    if (com != null)
                                    {
                                        com.SendMessage("SERVER$BUZZ");
                                    }
                                }
                                break;
                        }
                    }
                }

                listcom.Remove(local);
            }
            else
            {
                string res = "Incorrecto#Error";
                data = Encoding.ASCII.GetBytes(res);
                client.Send(data, data.Length, SocketFlags.None);
            }
        }
        else
        {
            if (receivedStrings[0] == "REG")
            {
                bool exists = cUsuarios.LoginUserVerification(receivedStrings[1], receivedStrings[2]);
                if (!exists)
                {
                    cUsuarios.RegisterUsuario(receivedStrings[1], receivedStrings[2], receivedStrings[3], receivedStrings[4]);
                    string res = "Cuenta registrada exitosamente. Bienvenido, " + receivedStrings[3];
                    data = Encoding.ASCII.GetBytes(res);
                    client.Send(data, data.Length, SocketFlags.None);
                }
                else
                {
                    string res = "Existe";
                    data = Encoding.ASCII.GetBytes(res);
                    client.Send(data, data.Length, SocketFlags.None);

                }
            }
        }
        socs.Remove(client);
        client.Close();
    }

    public static void SendFriendsToClient(Socket client, byte[] data, int recv, UserProfile auxUser)
    {
        data = Encoding.ASCII.GetBytes(cUsuarios.GetAllFriends(auxUser).Count.ToString());
        //Enviar el numero de contactos que tiene el Usuario
        client.Send(data, data.Length, SocketFlags.None);
        data = new byte[10];
        int a = client.Receive(data);
        string sendFriendString = "";
        foreach (UserProfile friend in cUsuarios.GetAllFriends(auxUser))
        {
            sendFriendString = friend.idUser.ToString() + "$" + friend.name + "$" + friend.username +
                "$" + friend.email + "$" + friend.status.ToString() + "$" + friend.points.ToString();
            data = Encoding.ASCII.GetBytes(sendFriendString);
            client.Send(data, data.Length, SocketFlags.None);
            data = new byte[50];
            client.Receive(data);
        }
    }

    public static void SendConversationsToClient(Socket client, byte[] data, int recv, UserProfile auxUser)
    {
        data = Encoding.ASCII.GetBytes(cConversaciones.GetAllConversations(auxUser).Count.ToString());
        //Enviar el numero de contactos que tiene el Usuario
        client.Send(data, data.Length, SocketFlags.None);
        data = new byte[50];
        client.Receive(data);
        foreach (Conversation convo in cConversaciones.GetAllConversations(auxUser))
        {
            string usuariosconvo = convo.idConversation.ToString() + "#" + convo.name;
            //foreach (Usuario uscon in convo.getUsuarios())
            //{
            //    if (uscon != aux)
            //    {
            //        usuariosconvo += uscon.getNombre() + "#";
            //    }
            //}
            //usuariosconvo = usuariosconvo.TrimEnd('#');

            ////////////////////////////////////////////////////////////7

            data = Encoding.ASCII.GetBytes(usuariosconvo);
            client.Send(data, data.Length, SocketFlags.None);
            data = new byte[50];
            client.Receive(data);


            ////Enviar Cantidad de Mensajes
            //data = Encoding.ASCII.GetBytes(convo.getMensajes().Count.ToString());
            //client.Send(data, data.Length, SocketFlags.None);
            //data = new byte[50];
            ////Recibir la Aceptacion de la Cantidad de Mensajes
            //client.Receive(data);

            ////Empezar a Enviar los Mensajes
            //foreach (Mensaje men in convo.getMensajes())
            //{
            //    string mensajeentero = men.getUsuario().getID().ToString() +
            //        "$" + men.getFechaHora().ToString("") +
            //        "$" + men.GetMensaje().Replace("$", "#m#");
            //    data = Encoding.ASCII.GetBytes(mensajeentero);
            //    client.Send(data, data.Length, SocketFlags.None);
            //    //Recibir la Aceptacion del Mensajes
            //    data = new byte[50];
            //    client.Receive(data);
            //}
            //data = new byte[50];
            //client.Receive(data);

            ///////////////////////////////////////////////////////////////////7
        }
    }

    public static void SendParticipantsToClient(Socket client, byte[] data, int recv, UserProfile auxUser)
    {
        List<ConversationUser> allParticipants = cConversaciones.GetAllParticipants(auxUser);

        data = Encoding.ASCII.GetBytes(allParticipants.Count.ToString());
        //Enviar el numero de contactos que tiene el Usuario
        client.Send(data, data.Length, SocketFlags.None);
        data = new byte[50];
        client.Receive(data);
        foreach (ConversationUser convo in allParticipants)
        {
            UserProfile secondAuxUser = cUsuarios.GetUserProfileById(convo.idUser);

            string usuariosconvo = secondAuxUser.idUser.ToString() + "#" + secondAuxUser.name + 
                "#" + secondAuxUser.username + "#" + secondAuxUser.email +
                "#" + secondAuxUser.status.ToString() + "#" + secondAuxUser.points.ToString() + 
                "#" + convo.idConversation.ToString();
            data = Encoding.ASCII.GetBytes(usuariosconvo);
            client.Send(data, data.Length, SocketFlags.None);
            data = new byte[50];
            client.Receive(data);
        }
    }

    public static void SendMessagesToClient(Socket client, byte[] data, int recv, UserProfile auxUser)
    {
        data = Encoding.ASCII.GetBytes(cConversaciones.GetAllMessages(auxUser).Count.ToString());
        //Enviar el numero de mensajes que tiene el Usuario
        client.Send(data, data.Length, SocketFlags.None);
        data = new byte[50];
        client.Receive(data);
        foreach (Message message in cConversaciones.GetAllMessages(auxUser))
        {
            string messageInfo = message.idMessage.ToString() + "#" + message.idSender.ToString() +
                "#" + message.message.ToString() + "#" + message.timestamp.ToString() +
                "#" + message.idConversation.ToString();

            data = Encoding.ASCII.GetBytes(messageInfo);
            client.Send(data, data.Length, SocketFlags.None);
            data = new byte[50];
            client.Receive(data);
        }
    }

    //public static void UserIsConnected()
    //{

    //    ///////////////////////777

    //    foreach (UserProfile usu in cUsuarios.GetAllFriends(auxUser.idUser))
    //    {
    //        Comunicacion com = listcom.Find(x => x.getUsuario().idUser == usu.idUser);
    //        if (com != null)
    //        {
    //            com.SendMessage("SERVER$CHANGESTATE$" + usu.idUser + "$" + 1);
    //        }
    //    }

    //    ////////////////////////////////7
    //}
}
