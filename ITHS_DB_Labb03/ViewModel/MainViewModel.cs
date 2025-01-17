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

        public UserViewModel UserViewModel { get =>_userViewModel; set { _userViewModel = value; OnPropertyChanged(); } }
        public Window AppWindow { get; set; }
        public AppState AppState { get; set; }
        public Visibility ListViewVisibility { get => _listViewVisibility; set { _listViewVisibility = value; OnPropertyChanged(); } }


        public MainViewModel()
        {
            UserViewModel = new UserViewModel(this);
            AppWindow = Application.Current.MainWindow;
            AppState = new AppState();
            GetUsersFromDb();
            CheckUserCollection();
            LoadAppState();
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


        private void LoadAppState()
        {
            using var db = new TodoDbContext();
            var state = db.AppState.FirstOrDefault();
            AppState = state;

            if (state is not null)
            {
                AppWindow.WindowState = state.WindowState;
                AppWindow.Top = state.WindowTop;
                AppWindow.Left = state.WindowLeft;
                AppWindow.Width = state.WindowWidth;
                AppWindow.Height = state.WindowHeight;
            }
        }


        private void SaveAppState()
        {

        }

        /// <summary>
        /// Sets the current view. UserView show all users. UserDetails shows
        /// details on a choosen user and ListView show the lists and tasks for choosen user.
        /// </summary>
        /// <param name="view">Valid params: userview, userdetails, listview</param>
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
