using BlazorChat.Server.Models;
using BlazorChat.Shared.Dtos;
using MongoDB.Driver;
using System.Linq;

namespace BlazorChat.Server.Services
{
    public class AccountService
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountService(IMongoClient client)
        {
            var database = client.GetDatabase("BlazorChat");
            _accounts = database.GetCollection<Account>("accounts");
        }


        public Account Login(LoginDto login)
        {
            var account = _accounts.Find<Account>(account => account.Username.Equals(login.Username)).FirstOrDefault();
            if (account == null || account.Password != login.Password)
            {
                return null;
            }
            return account;
        }

        public void Register(Account account)
        {
            _accounts.InsertOne(account);
        }

        public Account Get(string id)
        {
            return _accounts.Find<Account>(account => account.Id == id).FirstOrDefault();
        }
    }
}