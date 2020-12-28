using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorChat.Server.Models
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ParticipantA { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ParticipantB { get; set; }
        public List<Message> Messages { get; set; }
    }
}