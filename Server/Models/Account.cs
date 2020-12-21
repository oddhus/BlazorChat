using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorChat.Server.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}