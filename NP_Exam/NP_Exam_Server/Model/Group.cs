using System;
using System.Collections.Generic;
using NP_Exam_Library;

namespace NP_Exam_Server.Model
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        public Group()
        {
            Messages = new List<Message>();
            UserGroups = new List<UserGroup>();
        }

        public Group(string groupName)
        {
            GroupName = groupName;
            Messages = new List<Message>();
            UserGroups = new List<UserGroup>();
        }

        public Group(DisplayGroup dg)
        {
            Id = dg.Id;
            GroupName = dg.GroupName;
            Messages = new List<Message>();
            UserGroups = new List<UserGroup>();
        }

        public DisplayGroup ToDisplayGroup() => new DisplayGroup(Id, GroupName);
    }
}
