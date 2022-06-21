using System;
using NP_Exam_Library;

namespace NP_Exam_Server.Model
{
    public class UserGroup
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public virtual User User { get; set; }
        public virtual Group Group { get; set; }

        public UserGroup()
        { }

        public UserGroup(int userId, int groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }

        public UserGroup(DisplayUserGroup dug)
        {
            Id = dug.Id;
            UserId = dug.UserId;
            GroupId = dug.GroupId;
        }

        public DisplayUserGroup ToDisplayUserGroup() => new DisplayUserGroup(Id, UserId, GroupId);
    }
}
