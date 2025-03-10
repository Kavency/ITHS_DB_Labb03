﻿using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using System.Collections.ObjectModel;
using System.Windows;
using MongoDB.Driver;

namespace ITHS_DB_Labb03.ViewModel
{
    internal class MainViewModel : VMBase
    {
        public static string DbName { get; private set; } = "MarcusRenvallMagnusHellman";
        public static string ConnectionString { get; private set; } = "mongodb://localhost:27017/";

        private UserViewModel _userViewModel;
        private TagViewModel _tagViewModel;
        private TodoCollectionViewModel _todoCollectionViewModel;
        private double _oldWindowTop;
        private double _oldWindowLeft;
        private double _oldWindowWidth;
        private double _oldWindowHeight;
        private Visibility _listViewVisibility;
        private Visibility _userViewVisibility;
        private Visibility _userDetailsVisibility;
        private Visibility _editListViewVisibility;
        private Visibility _editTodoViewVisibility;

        public Visibility ListViewVisibility { get => _listViewVisibility; set { _listViewVisibility = value; OnPropertyChanged(); } }
        public Visibility UserViewVisibility { get => _userViewVisibility; set { _userViewVisibility = value; OnPropertyChanged(); } }
        public Visibility UserDetailsVisibility { get => _userDetailsVisibility; set { _userDetailsVisibility = value; OnPropertyChanged(); } }
        public Visibility EditListViewVisibility { get => _editListViewVisibility; set { _editListViewVisibility = value; OnPropertyChanged(); } }
        public Visibility EditTodoViewVisibility { get => _editTodoViewVisibility; set { _editTodoViewVisibility = value; OnPropertyChanged(); } }
        public TodoCollectionViewModel TodoCollectionViewModel { get => _todoCollectionViewModel; set { _todoCollectionViewModel = value; OnPropertyChanged(); } }
        public UserViewModel UserViewModel { get => _userViewModel; set { _userViewModel = value; OnPropertyChanged(); } }
        public TagViewModel TagViewModel { get => _tagViewModel; set { _tagViewModel = value; OnPropertyChanged(); } }
        public AppState AppState { get; set; }

        public RelayCommand WindowControlCMD { get; }

        public MainViewModel()
        {
            UserViewModel = new UserViewModel(this);
            TagViewModel = new TagViewModel(this);
            TodoCollectionViewModel = new TodoCollectionViewModel(this);
            AppState = new AppState();
            //TodoCollectionViewModel = new TodoCollectionViewModel(this);

            GetUsersAsync();
            CheckUserCollection();
            LoadAppState();

            WindowControlCMD = new RelayCommand(WindowControl);
        }

        private async Task GetUsersAsync()
        {
            using var db = new MongoClient(ConnectionString);
            var userCollection = db.GetDatabase(DbName).GetCollection<User>("Users");
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
            using var db = new MongoClient(ConnectionString);
            var stateCollection = db.GetDatabase(DbName).GetCollection<AppState>("AppState");
            var state = stateCollection.AsQueryable().FirstOrDefault();

            if (state is not null)
            {
                AppState = state;
                Application.Current.MainWindow.WindowState = state.WindowState;
                Application.Current.MainWindow.Top = state.WindowTop;
                Application.Current.MainWindow.Left = state.WindowLeft;
                Application.Current.MainWindow.Width = state.WindowWidth;
                Application.Current.MainWindow.Height = state.WindowHeight;
                SetOldWindowSize();

                if (state.CurrentUser is not null)
                {
                    UserViewModel.CurrentUser = state.CurrentUser;
                    TodoCollectionViewModel.TodoCollections = new ObservableCollection<TodoCollection>(UserViewModel.CurrentUser.TodoCollections);
                }
                if(state.CurrentCollection is not null)
                {
                    TodoCollectionViewModel.CurrentTodoCollection = state.CurrentCollection;
                }
            }
        }


        public async Task SaveAppState()
        {
            AppState.CurrentUser = UserViewModel.CurrentUser;
            AppState.CurrentCollection = TodoCollectionViewModel.CurrentTodoCollection;

            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                AppState.WindowState = Application.Current.MainWindow.WindowState;
                AppState.WindowTop = _oldWindowTop;
                AppState.WindowLeft = _oldWindowLeft;
                AppState.WindowWidth = _oldWindowWidth;
                AppState.WindowHeight = _oldWindowHeight;
            }
            else
            {
                AppState.WindowState = Application.Current.MainWindow.WindowState;
                AppState.WindowTop = Application.Current.MainWindow.Top;
                AppState.WindowLeft = Application.Current.MainWindow.Left;
                AppState.WindowWidth = Application.Current.MainWindow.Width;
                AppState.WindowHeight = Application.Current.MainWindow.Height;
            }

            using var db = new MongoClient(ConnectionString);
            var documentCollection = db.GetDatabase(DbName).GetCollection<AppState>("AppState");
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
                if (Application.Current.MainWindow.WindowState == WindowState.Normal)
                {
                    SetOldWindowSize();
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                }
                else
                {
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                    GetOldWindowSize();
                }
            }
            else if (param == "minimize")
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }
        }


        private void SetOldWindowSize()
        {
            _oldWindowTop = Application.Current.MainWindow.Top;
            _oldWindowLeft = Application.Current.MainWindow.Left;
            _oldWindowWidth = Application.Current.MainWindow.Width;
            _oldWindowHeight = Application.Current.MainWindow.Height;
        }

        private void GetOldWindowSize()
        {
            Application.Current.MainWindow.Top = _oldWindowTop;
            Application.Current.MainWindow.Left = _oldWindowLeft;
            Application.Current.MainWindow.Width = _oldWindowWidth;
            Application.Current.MainWindow.Height = _oldWindowHeight;
        }


        /// <summary>
        /// Sets the current view. UserView show all users. UserDetails shows
        /// details on a choosen user and ListView show the lists and tasks for choosen user.
        /// </summary>
        /// <param name="view">Valid params: userview, userdetails, listview, editlistview</param>
        internal void ChangeView(string view)
        {
            if (view.ToLower() == "userview")
            {
                UserViewVisibility = Visibility.Visible;
                UserDetailsVisibility = Visibility.Hidden;
                ListViewVisibility = Visibility.Hidden;
                EditTodoViewVisibility = Visibility.Hidden;
                EditListViewVisibility = Visibility.Hidden;
            }
            else if (view.ToLower() == "userdetails")
            {
                UserViewVisibility = Visibility.Hidden;
                UserDetailsVisibility = Visibility.Visible;
                ListViewVisibility = Visibility.Hidden;
                EditTodoViewVisibility = Visibility.Hidden;
                EditListViewVisibility = Visibility.Hidden;

            }
            else if (view.ToLower() == "listview")
            {
                UserViewVisibility = Visibility.Hidden;
                UserDetailsVisibility = Visibility.Hidden;
                ListViewVisibility = Visibility.Visible;
                EditTodoViewVisibility = Visibility.Hidden;
                EditListViewVisibility = Visibility.Hidden;
            }
            else if(view.ToLower() == "editlistview")
            {
                UserViewVisibility = Visibility.Hidden;
                UserDetailsVisibility = Visibility.Hidden;
                ListViewVisibility = Visibility.Hidden;
                EditListViewVisibility = Visibility.Visible;
                EditTodoViewVisibility = Visibility.Hidden;
            }
            else if (view.ToLower() == "edittodoview")
            {
                EditListViewVisibility = Visibility.Hidden;
                UserViewVisibility = Visibility.Hidden;
                UserDetailsVisibility = Visibility.Hidden;
                ListViewVisibility = Visibility.Hidden;
                EditTodoViewVisibility = Visibility.Visible;
            }
        }
    }
}
