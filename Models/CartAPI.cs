using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Cart_Account.Models
{
    public class APIDatabaseSettings : IAPIDatabaseSettings
    {
        public string APICollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IAPIDatabaseSettings
    {
        string APICollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}