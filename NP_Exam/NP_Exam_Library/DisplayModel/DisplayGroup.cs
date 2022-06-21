using System;

namespace NP_Exam_Library
{
    [Serializable]
    public class DisplayGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

        public DisplayGroup(int id, string groupName)
        {
            Id = id;
            GroupName = groupName;
        }

        public override string ToString() => GroupName;
    }
}
