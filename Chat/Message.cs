namespace Chat
{
    public class Message
    {
        public int UserId { get; set; }
        public string MsgType { get; set; }
        public string MsgContent { get; set; }

        public Message(int userId, string msgType, string msgContent)
        {
            UserId = userId;
            MsgType = msgType;
            MsgContent = msgContent;
        }
    }
}
