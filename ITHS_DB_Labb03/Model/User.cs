using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace ITHS_DB_Labb03.Model
{
    internal class User
    {

        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime UserCreated { get; set; }
        public ObservableCollection<TodoCollection> TodoCollections { get; set; }

    }
}
