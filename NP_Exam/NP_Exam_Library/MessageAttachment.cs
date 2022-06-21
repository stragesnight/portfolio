using System;

namespace NP_Exam_Library
{
    [Serializable]
    public class MessageAttachment
    {
        public string Filename { get; set; }
        public byte[] Data { get; set; }

        public MessageAttachment(string filename, byte[] data)
        {
            Filename = filename;
            Data = data;
        }
    }
}
