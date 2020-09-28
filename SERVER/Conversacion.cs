using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Conversacion
    {
        private List<Usuario> usuarios;
        static int allConv=0;
        private int id;
        private List<Mensaje> mensajes;

        public Conversacion()
        {
            usuarios = new List<Usuario>();
            mensajes = new List<Mensaje>();
            id = ++allConv;
        }
        public void AddUsuario(Usuario usuario)
        {
            usuarios.Add(usuario);
        }
        public void AddMessage(Usuario usuario, string Message)
        {
            mensajes.Add(new Mensaje(usuario, Message));
        }
        public void AddMessage(Mensaje mensaje)
        {
            mensajes.Add(mensaje);
        }
        public int getId()
        {
            return id;
        }
        public List<Usuario> getUsuarios()
        {
            return usuarios;
        }
        public List<Mensaje> getMensajes()
        {
            return mensajes;
        }
    }
}
