using BlazorChat.Models;
using BlazorChat.Server.Models;
using MongoDB.Driver;
using System.Linq;

namespace BlazorChat.Server.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IUserDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }


        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();


        public void Update(string id, User userIn) =>
            _users.ReplaceOne(user => user.Id == id, userIn);

    }
}