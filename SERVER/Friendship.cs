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
    
    public partial class Friendship
    {
        public int idFriendship { get; set; }
        public int idUser { get; set; }
        public int idFriend { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual UserProfile UserProfile1 { get; set; }
    }
}
