using System;

namespace NP_Exam_Library
{
    [Serializable]
    public class DisplayUserGroup
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public DisplayUserGroup(int id, int userId, int groupId)
        {
            Id = id;
            UserId = userId;
            GroupId = groupId;
        }
    }
}
