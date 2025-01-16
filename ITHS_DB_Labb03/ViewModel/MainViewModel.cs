using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using System.Collections.ObjectModel;
using MongoDB.Bson;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace ITHS_DB_Labb03.ViewModel
{
    internal class MainViewModel : VMBase
    {
        private UserViewModel _userViewModel;
        private Visibility _listViewVisibility;
        private AppState _appState;

        public UserViewModel UserViewModel { get =>_userViewModel; set { _userViewModel = value; OnPropertyChanged(); } }
        public AppState AppState { get => _appState; set { _appState = value; OnPropertyChanged(); } }
        public Visibility ListViewVisibility { get => _listViewVisibility; set { _listViewVisibility = value; OnPropertyChanged(); } }


        public MainViewModel()
        {
            UserViewModel = new UserViewModel(this);

            GetUsersFromDb();

            CheckUserCollection();
        }

        private async void GetUsersFromDb()
        {
            UserViewModel.Users = await GetUsersAsync();
        }

        private async Task<ObservableCollection<User>> GetUsersAsync()
        {
            using var db = new TodoDbContext();
            var result = await db.Users.ToListAsync();
            return new ObservableCollection<User>(result);
        }


        private void CheckUserCollection()
        {
            if(UserViewModel.Users is null || UserViewModel.Users.Count == 0)
                ChangeView("userview");
            else
                ChangeView("listview");
        }


        internal void ChangeView(string view)
        {
            if (view.ToLower() == "userview")
            {
                UserViewModel.UserViewVisibility = Visibility.Visible;
                UserViewModel.UserDetailsVisibility = Visibility.Hidden;
                ListViewVisibility = Visibility.Hidden;
            }
            else if(view.ToLower() == "userdetails")
            {
                UserViewModel.UserViewVisibility = Visibility.Hidden;
                UserViewModel.UserDetailsVisibility = Visibility.Visible;
                ListViewVisibility = Visibility.Hidden;
            }
            else if(view.ToLower() == "listview")
            {
                UserViewModel.UserViewVisibility = Visibility.Hidden;
                UserViewModel.UserDetailsVisibility = Visibility.Hidden;
                ListViewVisibility = Visibility.Visible;
            }
        }
    }
}
