using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_DB_Labb03.ViewModel
{
    internal class MainViewModel : VMBase
    {
        private User _currentUser;

        public User NewUser { get; set; }
        public User CurrentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(); } }
        public ObservableCollection<User> Users { get; set; }

        public MainViewModel()
        {
            GetUsers();
        }


        private void GetUsers()
    {
            using var db = new TodoDbContext();
            var result = db.Users.ToList();
            Users = new ObservableCollection<User>(result);
    }
}
