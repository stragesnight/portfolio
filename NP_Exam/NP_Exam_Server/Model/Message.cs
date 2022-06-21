using System;
using NP_Exam_Library;

namespace NP_Exam_Server.Model
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageText { get; set; }
        public object Attachment { get; set; }

        public virtual User Sender { get; set; }
        public virtual Group Receiver { get; set; }

        public Message()
        { }

        public Message(int senderId, int receiverId, string messageText, object attachment)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            MessageText = messageText;
            Attachment = attachment;
        }

        public Message(DisplayMessage dm)
        {
            Id = dm.Id;
            SenderId = dm.SenderId;
            ReceiverId = dm.ReceiverId;
            MessageText = dm.MessageText;
            Attachment = dm.Attachment;
        }

        public DisplayMessage ToDisplayMessage() => new DisplayMessage(Id, SenderId, MessageText, Sender.Username, Attachment);
    }
}
