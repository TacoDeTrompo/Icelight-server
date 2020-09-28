using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SERVER;
using System.Linq;

namespace Server
{
    class ControladorConversaciones
    {
        NuntiusDBEntities nuntiusModel;

        public ControladorConversaciones()
        {
            nuntiusModel = new NuntiusDBEntities();
        }

        public List<Conversation> GetAllConversations(UserProfile user)
        {
            List<Conversation> userMatch = (from mConversationUser in nuntiusModel.ConversationUser
                                           join mConversation in nuntiusModel.Conversation
                                           on mConversationUser.idConversation equals mConversation.idConversation
                                           where mConversationUser.idUser == user.idUser
                                           select mConversation).ToList();
            return userMatch;
        }

        public List<Message> GetAllMessages(UserProfile user)
        {
            List<Message> userMatch = (from mConversationUser in nuntiusModel.ConversationUser
                                       join mConversation in nuntiusModel.Conversation
                                       on mConversationUser.idConversation equals mConversation.idConversation
                                       join mMessage in nuntiusModel.Message
                                       on mConversation.idConversation equals mMessage.idConversation
                                       where mConversationUser.idUser == user.idUser
                                       select mMessage).ToList();
            return userMatch;
        }

        public List<ConversationUser> GetAllParticipants(UserProfile user)
        {
            //List<ConversationUserLink> allParticipants = new List<ConversationUserLink>();

            List<ConversationUser> allConversationsFromUser = (from mConversationUser in nuntiusModel.ConversationUser
                                                       join mConversation in nuntiusModel.Conversation
                                                       on mConversationUser.idConversation equals mConversation.idConversation
                                                       where mConversationUser.idUser == user.idUser
                                                       select mConversationUser).ToList();

            List<ConversationUser> allParticipants = new List<ConversationUser>();

            foreach (ConversationUser conversation in allConversationsFromUser)
            {
                List<ConversationUser> allUsersPerConversation = (from mConversationUser in nuntiusModel.ConversationUser
                                                                  where mConversationUser.idConversation == conversation.idConversation
                                                                  select mConversationUser).ToList();


                allParticipants.AddRange(allUsersPerConversation);
            }

            return allParticipants.Distinct().ToList();
        }

        public List<UserProfile> GetAllParticipantsProfile(Conversation conversation)
        {
            List<UserProfile> allParticipantsFromConversation = (from mConversationUser in nuntiusModel.ConversationUser
                                                               join mConversation in nuntiusModel.Conversation
                                                               on mConversationUser.idConversation equals mConversation.idConversation
                                                               where mConversationUser.idConversation == conversation.idConversation
                                                               select mConversationUser.UserProfile).ToList();

            return allParticipantsFromConversation.Distinct().ToList();
        }

        public void AddMessagetoConv(int idConversation, UserProfile user, string Message)
        {
            Message newMessage = new Message();

            newMessage.idSender = user.idUser;
            newMessage.message = Message;
            newMessage.timestamp = DateTime.Now;
            newMessage.idConversation = idConversation;

            nuntiusModel.Message.Add(newMessage);
            nuntiusModel.SaveChanges();
        }

        public Conversation GetConversation(int idConversation)
        {
            Conversation conversation = (from mConversation in nuntiusModel.Conversation
                                         where mConversation.idConversation == idConversation
                                         select mConversation).FirstOrDefault();
            return conversation;
        }


    }
}
