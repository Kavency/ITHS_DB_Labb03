using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using System.Collections.ObjectModel;
using MongoDB.Bson;
using System.Windows;
using MongoDB.Driver;


namespace ITHS_DB_Labb03.ViewModel
{
    internal class MainViewModel : VMBase
    {
        public static string connectionString = "mongodb://localhost:27017/";

        private UserViewModel _userViewModel;
        private Visibility _listViewVisibility;
        private TodoCollectionViewModel _todoCollectionViewModel;
        public TodoCollectionViewModel TodoCollectionViewModel { get => _todoCollectionViewModel; set { _todoCollectionViewModel = value; OnPropertyChanged(); } }
        public UserViewModel UserViewModel { get => _userViewModel; set { _userViewModel = value; OnPropertyChanged(); } }
        public Window AppWindow { get; set; }
        public AppState AppState { get; set; }
        public Visibility ListViewVisibility { get => _listViewVisibility; set { _listViewVisibility = value; OnPropertyChanged(); } }

        public RelayCommand WindowControlCMD { get; }

        public MainViewModel()
        {
            UserViewModel = new UserViewModel(this);
            TodoCollectionViewModel = new TodoCollectionViewModel(this);
            AppWindow = Application.Current.MainWindow;
            AppState = new AppState();
            TodoCollectionViewModel = new TodoCollectionViewModel(this);

            GetUsersAsync();
            CheckUserCollection();
            LoadAppState();

            WindowControlCMD = new RelayCommand(WindowControl);
        }


        private async Task GetUsersAsync()
        {
            using var db = new MongoClient(connectionString);
            var userCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");
            var result = userCollection.AsQueryable().ToList();
            UserViewModel.Users = new ObservableCollection<User>(result);
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
            using var db = new MongoClient(connectionString);
            var stateCollection = db.GetDatabase("todoapp").GetCollection<AppState>("AppState");
            var state = stateCollection.AsQueryable().FirstOrDefault();

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

            using var db = new MongoClient(connectionString);
            var documentCollection = db.GetDatabase("todoapp").GetCollection<AppState>("AppState");
            var documentCount = documentCollection.AsQueryable().Count();


            if (documentCount > 0)
            {
                await documentCollection.DeleteManyAsync(_ => true);
            }

            documentCollection.InsertOneAsync(AppState);
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
                {
                    AppState.WindowTop = AppWindow.Top;
                    AppState.WindowLeft = AppWindow.Left;
                    AppState.WindowWidth = AppWindow.Width;
                    AppState.WindowHeight = AppWindow.Height;
                    AppWindow.WindowState = WindowState.Maximized;
                }
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
