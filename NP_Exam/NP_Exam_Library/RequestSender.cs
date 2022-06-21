using System;

namespace NP_Exam_Library
{
    [Serializable]
    public class RequestSender
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public RequestSender(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
