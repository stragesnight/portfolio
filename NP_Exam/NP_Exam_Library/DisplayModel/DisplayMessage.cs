using System;

namespace NP_Exam_Library
{
    [Serializable]
    public class DisplayMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageText { get; set; }
        public string SenderUsername { get; set; }
        public object Attachment { get; set; }

        public DisplayMessage(int senderId, int receiverId, string messageText, string senderUsername, object attachment)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            MessageText = messageText;
            SenderUsername = senderUsername;
            Attachment = attachment;
        }

        public override string ToString() => $"[{SenderUsername}]: {MessageText}{(Attachment == null ? "" : " [прикреплённый файл]")}";
    }
}
