using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Mensaje
    {
        Usuario usuario;
        string mensaje;
        DateTime datetime;

        public Mensaje(Usuario usuario,string mensaje)
        {
            datetime = DateTime.Now;
            this.usuario = usuario;
            this.mensaje = mensaje;
        }
        public Mensaje(Usuario usuario, string mensaje,DateTime dateTime)
        {
            this.datetime = dateTime;
            this.usuario = usuario;
            this.mensaje = mensaje;
        }
        public DateTime getFechaHora()
        {
            return datetime;
        }
        public Usuario getUsuario()
        {
            return usuario;
        }
        public string GetMensaje()
        {
            return mensaje;
        }
    }
}
