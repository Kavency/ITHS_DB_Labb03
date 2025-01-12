using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_DB_Labb03.Model
{
    internal class TodoCollection
    {

        public string Id { get; set; }
        public List<User> Users { get; set; }
        public List<Todo> Todos { get; set; }
        public DateTime CollectionCreated { get; set; }

    }
}
