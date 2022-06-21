using System;
using System.Collections.Generic;
using NP_Exam_Library;

namespace NP_Exam_Server.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Authorized { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        public User()
        {
            Messages = new List<Message>();
            UserGroups = new List<UserGroup>();
            Authorized = false;
        }

        public User(string username, string password, int id = -1)
        {
            Messages = new List<Message>();
            UserGroups = new List<UserGroup>();
            if (id != -1)
                Id = id;
            Username = username;
            Password = password;
            Authorized = false;
        }

        public User(RequestSender sender)
        {
            Messages = new List<Message>();
            UserGroups = new List<UserGroup>();
            Id = sender.Id;
            Username = sender.Username;
            Password = sender.Password;
            Authorized = false;
        }

        public User(DisplayUser du)
        {
            Messages = new List<Message>();
            UserGroups = new List<UserGroup>();
            Id = du.Id;
            Username = du.Username;
            Authorized = false;
        }

        public DisplayUser ToDisplayUser() => new DisplayUser(Id, Username);
    }
}
