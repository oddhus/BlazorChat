using BlazorChat.Models;
using BlazorChat.Server.Models;
using BlazorChat.Shared.Dtos;
using MongoDB.Driver;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace BlazorChat.Server.Services
{
    public class AccountService
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _accounts = database.GetCollection<Account>(settings.AccountsCollectionName);
        }


        public Account Login(LoginDto login)
        {
            var account = _accounts.Find<Account>(account => account.Username == login.Username).FirstOrDefault();
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