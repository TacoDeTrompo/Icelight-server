//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SERVER
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserProfile
    {
        public UserProfile()
        {
            this.ConversationUser = new HashSet<ConversationUser>();
            this.Friendship = new HashSet<Friendship>();
            this.Friendship1 = new HashSet<Friendship>();
        }
    
        public int idUser { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public byte[] photo { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> points { get; set; }
    
        public virtual ICollection<ConversationUser> ConversationUser { get; set; }
        public virtual ICollection<Friendship> Friendship { get; set; }
        public virtual ICollection<Friendship> Friendship1 { get; set; }
    }
}
