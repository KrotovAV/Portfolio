﻿using DataBaseMessage.BD;
using DataBaseMessage;

namespace MessageService.Repository
{
    public class MessagRepository : IMessageRepository
    {
        public List<Message> GetAllMessages(string receiverName)
        {
            using (var context = new MessageContext())
            {
                var messages = context.Messages
                    .Where(message => message.ReceiverName == receiverName)
                    .ToList();
                foreach (var message in messages)
                {
                    message.IsReceived = true;
                    context.SaveChanges();
                }

                return messages;
            }
        }
        public List<Message> GetUnreceivedMessages(string receiverName)
        {
            using (var context = new MessageContext())
            {
                var messages = context.Messages
                    .Where(message => message.ReceiverName == receiverName && message.IsReceived == false)
                    .ToList();
                foreach (var message in messages)
                {
                    message.IsReceived = true;
                    context.SaveChanges();
                }

                return messages;
            }
        }
        public Guid SendMessage(string text, string senderName, string receiverName)
        {
            using (var context = new MessageContext())
            {
                if (senderName != null && receiverName != null)
                {
                    var id = Guid.NewGuid();
                    var message = new Message()
                    {
                        Id = id,
                        SenderName = senderName,
                        ReceiverName = receiverName,
                        Text = text,
                        IsReceived = false,
                        DateTime = DateTime.UtcNow
                    };
                    context.Messages.Add(message);
                    context.SaveChanges();
                    return id;
                }
            }
            throw new ArgumentException("There are no such registered users in the system");
        }

        public bool DeleteMessage(Guid id)
        {
            using (var context = new MessageContext())
            {
                var message = context.Messages.FirstOrDefault(x => x.Id == id);

                if (message != null)
                {
                    context.Messages.Remove(message);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }    
        }
    }
}
