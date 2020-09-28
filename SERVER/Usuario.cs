using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    internal enum statusContacto
    {
        conectado,
        desconectado,
        ausente
    }
    class Usuario
    {
        private static int allUsers=0;
        private int id;
        private string nombre;
        private string correo;
        private string password;
        private int points;
        private List<Usuario> contactos;
        private List<Conversacion> conversaciones;
        private statusContacto status;

        public Usuario(string nombre, string correo, string password)
        {
            contactos = new List<Usuario>();
            conversaciones = new List<Conversacion>();
            this.nombre = nombre;
            this.correo = correo;
            this.password = password;

            


            id = ++allUsers;
            points = 0;
        }
        public string getNombre()
        {
            return nombre;
        }
        public string getCorreo()
        {
            return correo;
        }
        public int getID()
        {
            return id;
        }
        public string getPassword()
        {
            return password;
        }
        public void AddContacto(Usuario contacto)
        {
            contactos.Add(contacto);
        }
        public void AddConversacion(Conversacion conversacion)
        {
            conversaciones.Add(conversacion);
        }
        public List<Usuario> getContactos()
        {
            return contactos;
        }
        public List<Conversacion> getConversaciones()
        {
            return conversaciones;
        }
        public int getPoints()
        {
            return points;
        }
        public void setPoints(int points)
        {
            this.points = points;
        }
        public void AddPoints(int points)
        {
            points += points;
        }
        public void setStatus(statusContacto status)
        {
            this.status = status;
        }
        public statusContacto GetStatus()
        {
            return status;
        }
    }
}
