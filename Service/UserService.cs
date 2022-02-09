using API.Cart_Account.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Cart_Account.Service
{
    public class UserService
    {
        private readonly IMongoCollection<Account> _carts;

        public UserService(IAPIAccountSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _carts = database.GetCollection<Account>(settings.APICollectionName);
        }

        public IEnumerable<Account> Get() =>
            _carts.Find(book => true).ToList();

        public bool ValidateAdmin(string username, string password)
        {
            var admin = _carts.Find(a => a.Email.Equals(username)).SingleOrDefault();
            return admin != null && new PasswordHasher<Account>().VerifyHashedPassword(new Account(), admin.Password, password) == PasswordVerificationResult.Success;
        }
        public Account GetAccount(string model)
        {
            return _carts.Find(x => x.Email.Equals(model) && x.Role.Equals("User")).SingleOrDefault();
        }
        public Account GetEmail(string email) =>
           _carts.Find(book => book.Email==email).FirstOrDefault();
        public Account Get(string id) =>
            _carts.Find(book => book.Id == id).FirstOrDefault();

        public Account Create(Account book)
        {
            _carts.InsertOne(book);
            return book;
        }
        public void Update(string id, Account bookIn) =>
            _carts.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Account bookIn) =>
            _carts.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _carts.DeleteOne(book => book.Id == id);
        public void RemoveAll() =>
            _carts.DeleteMany(b => b.Active);
    }
}
