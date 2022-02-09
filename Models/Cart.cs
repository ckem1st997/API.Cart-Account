using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Cart_Account.Models
{
    public class Cart
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CartId { get; set; }
        public string Name { get; set; }
        public int ProductID { get; set; }


        public decimal Price { get; set; }
        public string Image { get; set; }

        public int Count { get; set; }


        public string Size { get; set; }
        public string Color { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
