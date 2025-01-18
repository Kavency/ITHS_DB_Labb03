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
        //private TodoCollectionViewModel _todoCollectionViewModel;

        public UserViewModel UserViewModel { get => _userViewModel; set { _userViewModel = value; OnPropertyChanged(); } }
        public Window AppWindow { get; set; }
        public AppState AppState { get; set; }
        public Visibility ListViewVisibility { get => _listViewVisibility; set { _listViewVisibility = value; OnPropertyChanged(); } }

        public RelayCommand WindowControlCMD { get; }

        public MainViewModel()
        {
            UserViewModel = new UserViewModel(this);
            AppWindow = Application.Current.MainWindow;
            AppState = new AppState();
            //TodoCollectionViewModel = new TodoCollectionViewModel(this);

            GetUsersFromDb();
            CheckUserCollection();
            LoadAppState();

            WindowControlCMD = new RelayCommand(WindowControl);

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
            if (UserViewModel.Users is null || UserViewModel.Users.Count == 0)
                ChangeView("userview");
            else
                ChangeView("listview");
        }


        private void LoadAppState()
        {
            using var db = new TodoDbContext();
            var state = db.AppState.FirstOrDefault();

            if (state is not null)
            {
                AppState = state;
                UserViewModel.CurrentUser = state.CurrentUser;
                AppWindow.WindowState = state.WindowState;
                AppWindow.Top = state.WindowTop;
                AppWindow.Left = state.WindowLeft;
                AppWindow.Width = state.WindowWidth;
                AppWindow.Height = state.WindowHeight;
            }
        }


        private async Task SaveAppState()
        {
            AppState.CurrentUser = UserViewModel.CurrentUser;
            AppState.WindowState = AppWindow.WindowState;
            AppState.WindowTop = AppWindow.Top;
            AppState.WindowLeft = AppWindow.Left;
            AppState.WindowWidth = AppWindow.Width;
            AppState.WindowHeight = AppWindow.Height;

            using var db = new TodoDbContext();

            var documentCount = await db.AppState.CountAsync();

            if (documentCount > 0)
            {
                var allDocuments = await db.AppState.ToListAsync();
                db.AppState.RemoveRange(allDocuments);
                await db.SaveChangesAsync();
            }

            await db.AppState.AddAsync(AppState);
            await db.SaveChangesAsync();
        }


        private async void WindowControl(object obj)
        {
            var param = obj.ToString().ToLower();
            if (param == "close")
            {
                var confirmDeletion = MessageBox.Show($"Do you want to quit?", "Quit application", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirmDeletion == MessageBoxResult.Yes)
                {
                    SaveAppState();
                    Application.Current.Shutdown();
                }
            }
            else if (param == "maximize")
            {
                if (AppWindow.WindowState == WindowState.Normal)
                    AppWindow.WindowState = WindowState.Maximized;
                else
                    AppWindow.WindowState = WindowState.Normal;
            }
            else if (param == "minimize")
            {
                AppWindow.WindowState = WindowState.Minimized;
            }
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
            else if (view.ToLower() == "userdetails")
            {
                UserViewModel.UserViewVisibility = Visibility.Hidden;
                UserViewModel.UserDetailsVisibility = Visibility.Visible;
                ListViewVisibility = Visibility.Hidden;
            }
            else if (view.ToLower() == "listview")
            {
                UserViewModel.UserViewVisibility = Visibility.Hidden;
                UserViewModel.UserDetailsVisibility = Visibility.Hidden;
                ListViewVisibility = Visibility.Visible;
            }
        }
    }
}
