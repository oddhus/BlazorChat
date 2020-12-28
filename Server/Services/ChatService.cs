using BlazorChat.Server.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace BlazorChat.Server.Services
{
    public class ChatService
    {
        private readonly IMongoCollection<Chat> _chats;

        public ChatService(IMongoClient client)
        {
            var database = client.GetDatabase("BlazorChat");
            _chats = database.GetCollection<Chat>("chats");
        }

        public Chat GetChat(string chatId)
        {
            var filter = Builders<Chat>.Filter.Eq(c => c.Id, chatId);
            return _chats.Find<Chat>(filter).FirstOrDefault();
        }

        public Chat GetChat(string userId, string recipientId)
        {
            var filter = Builders<Chat>.Filter.Where(c => c.ParticipantA == userId && c.ParticipantB == recipientId)
                | Builders<Chat>.Filter.Where(c => c.ParticipantA == recipientId && c.ParticipantB == userId);
            return _chats.Find<Chat>(filter).FirstOrDefault();
        }

        public Chat StartChat(string userId, string recipientId)
        {
            Chat chat = new Chat();
            chat.Messages = new List<Message>();
            chat.ParticipantA = userId;
            chat.ParticipantB = recipientId;
            _chats.InsertOne(chat);
            return chat;
        }

        public Chat GetMessages(string chatId)
        {
            return _chats.Find(c => c.Id == chatId).FirstOrDefault();
        }

        public Message AddMessage(string chatId, Message message)
        {
            var filter = Builders<Chat>.Filter.Eq(c => c.Id, chatId);
            var update = Builders<Chat>.Update.Push(c => c.Messages, message);
            var opts = new FindOneAndUpdateOptions<Chat>()
            {
                IsUpsert = false,
                ReturnDocument = ReturnDocument.After
            };
            _chats.FindOneAndUpdate(filter, update, opts);
            return message;
        }
    }
}