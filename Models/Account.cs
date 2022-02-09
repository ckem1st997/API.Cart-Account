using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Cart_Account.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public string Email { get; set; }

       // [BsonIgnore]
        [BsonRequired]
        public string Password { get; set; }

        public string Fullname { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public DateTime CreateDate { get; set; }

        public string HomePage { get; set; }

        public bool Active { get; set; }

        public string Role { get; set; }

        public bool ConfirmEmail { get; set; }

        public string token { get; set; }

        public bool LockAccount { get; set; }

     

    }
}
