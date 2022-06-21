using System;

namespace NP_Exam_Library
{
    [Serializable]
    public class DisplayUser
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public DisplayUser(int id, string username)
        {
            Id = id;
            Username = username;
        }
    }
}
