using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_DB_Labb03.Model
{
    internal class AppSettings
    {
        public User CurrentUser { get; set; }
        public TodoCollection SelectedCollection { get; set; }

    }
}
