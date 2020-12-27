using BlazorChat.Server.Models;
using BlazorChat.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Linq;
using BC = BCrypt.Net.BCrypt;

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

        public bool AccountExists(string username)
        {
            var filter = Builders<Account>.Filter.Eq(a => a.Username, username);
            return _accounts.Find<Account>(filter).FirstOrDefault() != null;
        }

        public Account Login(LoginDto login)
        {
            var account = _accounts.Find<Account>(account => account.Username.Equals(login.Username)).FirstOrDefault();
            if (account == null || !BC.Verify(login.Password, account.Password))
            {
                return null;
            }
            return account;
        }

        public Account Register(string userId, RegisterDto registerDto)
        {
            Account account = new Account();
            account.Password = account.Password = BC.HashPassword(registerDto.Password);
            account.Username = registerDto.Username.ToLower();
            account.UserId = userId;
            _accounts.InsertOne(account);
            return account;
        }

        public Account Get(string id)
        {
            return _accounts.Find<Account>(account => account.Id == id).FirstOrDefault();
        }
    }
}