using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_DB_Labb03.Model
{
    class Todo
    {
        // Id - String Guid
        // Datum och tid skapad - DateTime
        // Titel - String
        // Beskrivning - String
        // Delat med användare - Lista med strings(GUID)
        // Datum och tid avklarad - DateTime
        // Avklarad - Bool
        // Taggar - Lista<Tags>
        // Stjärnmärkning - Bool
      
        public string Id { get; set; }
        public DateTime StartTodo { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public List<string> SharedTodos { get; set; }
        public DateTime EndTodo { get; set; }
        public bool IsDone { get; set; }
        public List<Tag> Tags { get; set; }
        public bool IsStared { get; set; }

    }
}
