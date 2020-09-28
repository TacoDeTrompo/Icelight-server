using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SERVER;
using System.Linq;

namespace Server
{
    class ControladorUsuarios
    {
        NuntiusDBEntities nuntiusModel;
        List<UserProfile> userProfiles;
        List<Usuario> usuarios;

        public ControladorUsuarios()
        {
            nuntiusModel = new NuntiusDBEntities();
            usuarios = new List<Usuario>();
            userProfiles = new List<UserProfile>();
        }

        public void RegisterUsuario(string name, string username, string email, string password)
        {
            UserProfile userToAdd = new UserProfile();
            userToAdd.name = name;
            userToAdd.username = username;
            userToAdd.email = email;
            userToAdd.password = password;
            userToAdd.points = 0;
            userToAdd.status = 1;

            nuntiusModel.UserProfile.Add(userToAdd);
            nuntiusModel.SaveChanges();
        }

        public Usuario getUsuario(int id)
        {
            return usuarios.Find(x => x.getID() == id);
        }

        public Usuario getUsuario(string nombre)
        {
            return usuarios.Find(x => x.getNombre() == nombre);
        }

        public bool isUsuario(string nombre)
        {
            bool res = false;

            Usuario us = usuarios.Find(x => x.getNombre() == nombre);
            if (us != null)
                res = true;
            return res;
        }
        public bool existsCorreo(string correo)
        {
            bool res = false;
            Usuario us = usuarios.Find(x => x.getCorreo() == correo);
            if (us != null)
                res = true;
            return res;
        }


        public bool LoginUserVerification(string username, string password)
        {
            bool res = false;

            UserProfile usuarioMatch = (from mUsuario in nuntiusModel.UserProfile
                                where mUsuario.username == username && mUsuario.password == password
                                select mUsuario).FirstOrDefault();

                if (usuarioMatch != null)
                    res = true;

            return res;
        }
        public List<Usuario> getUsuarios()
        {
            return usuarios;
        }

        public UserProfile GetUserProfileByCredentials(string username, string password)
        {

            UserProfile userMatch = (from mUsuario in nuntiusModel.UserProfile
                                        where mUsuario.username == username && mUsuario.password == password
                                        select mUsuario).FirstOrDefault();

            return userMatch;
        }

        public UserProfile GetUserProfileById(int idUser)
        {

            UserProfile userMatch = (from mUsuario in nuntiusModel.UserProfile
                                     where mUsuario.idUser == idUser
                                     select mUsuario).FirstOrDefault();

            return userMatch;
        }

        public List<UserProfile> GetAllFriends(UserProfile user)
        {

            List<UserProfile> userMatch = (from mFriendship in nuntiusModel.Friendship
                                           join mUserProfile in nuntiusModel.UserProfile 
                                           on mFriendship.idUser equals mUserProfile.idUser
                                           join mFriendProfile in nuntiusModel.UserProfile
                                           on mFriendship.idFriend equals mFriendProfile.idUser
                                           where mFriendship.idUser == user.idUser
                                           select mFriendProfile).ToList();

            return userMatch;
        }

        public List<UserProfile> GetAllFriends(int idUser)
        {

            List<UserProfile> userMatch = (from mFriendship in nuntiusModel.Friendship
                                           join mUserProfile in nuntiusModel.UserProfile
                                           on mFriendship.idUser equals mUserProfile.idUser
                                           join mFriendProfile in nuntiusModel.UserProfile
                                           on mFriendship.idFriend equals mFriendProfile.idUser
                                           where mFriendship.idUser == idUser
                                           select mFriendProfile).ToList();

            return userMatch;
        }

        public void DesconectUserProfile(UserProfile user)
        {
            user.status = 3;
            nuntiusModel.SaveChanges();
        }

        public void ConnectedUserProfile(UserProfile user)
        {
            user.status = 1;
            nuntiusModel.SaveChanges();
        }
    }
}
