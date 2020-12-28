using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorChat.Server.Models
{
    public class Contact
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ChatId { get; set; }
    }
}