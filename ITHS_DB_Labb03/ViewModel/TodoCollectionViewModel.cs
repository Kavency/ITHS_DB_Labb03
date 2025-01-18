using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_DB_Labb03.ViewModel;

    internal class TodoCollectionViewModel : VMBase
    {
		private MainViewModel _mainViewModel;
    private Visibility _isListTextVisible;
    private Visibility _isListButtonVisible;
    private string _newListName;

    public ObservableCollection<Todo> Todos { get; set; }
    public ObservableCollection<TodoCollection> TodoCollection { get; set; }

    //public ObservableCollection<User> Users { get; set; } ?
		public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }

    public Visibility IsListTextVisible { get => _isListTextVisible; set { _isListTextVisible = value; OnPropertyChanged(); } }
    public Visibility IsListButtonVisible { get => _isListButtonVisible; set { _isListButtonVisible = value; OnPropertyChanged(); } }
    public string NewListName { get => _newListName; set { _newListName = value; OnPropertyChanged(); } }


    public RelayCommand ShowListTextCMD { get; set; }
    public RelayCommand AddNewTaskCMD { get; }
    public RelayCommand AddNewListCMD { get; }
    public RelayCommand CreateTodoCMD { get; }
    public RelayCommand ReadTodoCMD { get; }
    public RelayCommand UpdateTodoCMD { get; }
    public RelayCommand DeleteTodoCMD { get; }
    public RelayCommand CreateListCMD { get; }
    public RelayCommand ReadListCMD { get; }
    public RelayCommand UpdateListCMD { get; }
    public RelayCommand DeleteListCMD { get; }

        public TodoCollectionViewModel(MainViewModel mainViewModel)
        {

            MainViewModel = mainViewModel;
        Todos = new ObservableCollection<Todo>();
        TodoCollection = new ObservableCollection<TodoCollection>();

        ShowListTextCMD = new RelayCommand(ShowListText);

        AddNewTaskCMD = new RelayCommand(AddNewTask); //Lägger till task
        AddNewListCMD = new RelayCommand(AddNewList); //Lägger till en lista

        CreateTodoCMD = new RelayCommand(CreateTodo); // Enter eller "Save"
        ReadTodoCMD = new RelayCommand(ReadTodo);
        UpdateTodoCMD = new RelayCommand(UpdateTodo);
        DeleteTodoCMD = new RelayCommand(DeleteTodo);

        CreateListCMD = new RelayCommand(CreateList); // Enter eller "Save"
        ReadListCMD = new RelayCommand(ReadList);
        UpdateListCMD = new RelayCommand(UpdateList);
        DeleteListCMD = new RelayCommand(DeleteList);

    }
    private void ShowListText(object obj)
    {
        IsListTextVisible = Visibility.Visible;
        IsListButtonVisible = Visibility.Collapsed;
    }

    private void AddNewList(object obj)
    {
        if (!string.IsNullOrWhiteSpace(NewListName))
        {
            TodoCollection.Add(new TodoCollection { Title = NewListName });
            NewListName = string.Empty;
            IsListTextVisible = Visibility.Collapsed;
            IsListButtonVisible = Visibility.Visible;
    }
        }

    }
}
