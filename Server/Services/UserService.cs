using BlazorChat.Server.Models;
using BlazorChat.Shared.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace BlazorChat.Server.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoClient client)
        {
            var database = client.GetDatabase("BlazorChat");
            _users = database.GetCollection<User>("users");
        }

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();


        public User GetUserSettings(string id)
        {
            var fieldsBuilder = Builders<User>.Projection;
            var fields = fieldsBuilder.Include(u => u.DarkTheme).Include(u => u.Notifications);
            return _users.Find<User>(u => u.Id == id).Project<User>(fields).FirstOrDefault();
        }

        public void CreateUser(User user)
        {
            user.Contacts = new List<Contact>();
            user.Firstname = user.Firstname.ToLower();
            user.Lastname = user.Lastname.ToLower();
            user.Address = user.Address.ToLower();
            _users.InsertOne(user);
        }

        public List<User> SearchUsers(string firstname, string lastname)
        {

            var filter = Builders<User>.Filter.Empty;
            if (!string.IsNullOrEmpty(firstname))
            {
                firstname = firstname.ToLower();
                filter &= (Builders<User>.Filter.Regex(x => x.Firstname, new BsonRegularExpression(firstname)));
            }
            if (!string.IsNullOrEmpty(lastname))
            {
                lastname = lastname.ToLower();
                filter &= (Builders<User>.Filter.Regex(x => x.Lastname, new BsonRegularExpression(lastname)));
            }
            return _users.Find(filter).Limit(10).ToList();
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

        public User GetUserContacts(string userId)
        {
            var fieldsBuilder = Builders<User>.Projection;
            var fields = fieldsBuilder.Include(u => u.Contacts);
            return _users.Find<User>(u => u.Id == userId).Project<User>(fields).FirstOrDefault();
        }

        public User AddUserContacts(string userId, Contact contact)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.AddToSet<Contact>(u => u.Contacts, contact);
            var opts = new FindOneAndUpdateOptions<User>()
            {
                IsUpsert = false,
                ReturnDocument = ReturnDocument.After
            };
            return _users.FindOneAndUpdate(filter, update, opts);
        }

        public User UpdateUserContacts(string userId, string contactId, string chatId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId)
                & Builders<User>.Filter.ElemMatch(x => x.Contacts, Builders<Contact>.Filter.Eq(x => x.Id, contactId));
            var update = Builders<User>.Update.Set(c => c.Contacts[-1].ChatId, chatId);
            var opts = new FindOneAndUpdateOptions<User>()
            {
                IsUpsert = false,
                ReturnDocument = ReturnDocument.After
            };
            return _users.FindOneAndUpdate(filter, update, opts);
        }

        public User RemoveUserContacts(string userId, string contactId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var filterContacts = Builders<Contact>.Filter.Eq(c => c.Id, contactId);
            var update = Builders<User>.Update.PullFilter(u => u.Contacts, filterContacts);
            var opts = new FindOneAndUpdateOptions<User>()
            {
                IsUpsert = false,
                ReturnDocument = ReturnDocument.After
            };
            return _users.FindOneAndUpdate(filter, update, opts);
        }
    }
}