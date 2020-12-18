using BlazorChat.Models;
using BlazorChat.Server.Models;
using BlazorChat.Shared.Dtos;
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

        public User GetUserSettings(string id)
        {
            var fieldsBuilder = Builders<User>.Projection;
            var fields = fieldsBuilder.Include(u => u.DarkTheme).Include(u => u.Notifications);
            return _users.Find<User>(u => u.Id == id).Project<User>(fields).FirstOrDefault();
        }

        public void Update(string id, User user, UserUpdateDto userIn)
        {
            user.Firstname = userIn.Firstname;
            user.Lastname = userIn.Lastname;
            user.Address = userIn.Address;
            _users.ReplaceOne(user => user.Id == id, user);
        }

        public void UpdateUserSettings(string id, UserSettingsDto userIn)
        {
            var update = Builders<User>.Update.Set(u => u.Notifications, userIn.Notifications);
            update = update.Set(u => u.DarkTheme, userIn.DarkTheme);

            var option = new FindOneAndUpdateOptions<User> { IsUpsert = false };

            _users.FindOneAndUpdate<User>(user => user.Id == id, update, option);
        }
    }
}