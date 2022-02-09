using API.Cart_Account.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Cart_Account.Service
{
    public class CartService
    {
        private readonly IMongoCollection<Cart> _carts;

        public CartService(IAPIDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _carts = database.GetCollection<Cart>(settings.APICollectionName);
        }

        public List<Cart> Get() =>
            _carts.Find(book => true).ToList();

        public List<Cart> GetAllCartId(string cartid)
        {
            return _carts.Find<Cart>(b => b.CartId == cartid).ToList();
        }
        public Cart GetCSL(Cart cart)
        {
            return _carts.Find(b => b.Color == cart.Color && b.Size == cart.Size && b.CartId == cart.CartId &&b.ProductID==cart.ProductID).FirstOrDefault();
        }
        public Cart Get(string id) =>
            _carts.Find<Cart>(book => book.Id == id).FirstOrDefault();

        public Cart Create(Cart book)
        {
            _carts.InsertOne(book);
            return book;
        }
        public void Update(string id, Cart bookIn) =>
            _carts.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Cart bookIn) =>
            _carts.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _carts.DeleteOne(book => book.Id == id);
        public void RemoveAll() =>
            _carts.DeleteMany(b => b.Count > 0);
    }
}
