using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ITHS_DB_Labb03.ViewModel;

internal class TodoCollectionViewModel : VMBase
{
    private MainViewModel _mainViewModel;
    private Visibility _isListTextVisible;
    private Visibility _isListButtonVisible;
    private Visibility _isTaskTextVisible;
    private Visibility _isTaskButtonVisible;
    
    private TodoCollection _currentTodoCollection;
    private Todo _currentTodo;
    
    public Visibility IsListTextVisible { get => _isListTextVisible; set { _isListTextVisible = value; OnPropertyChanged(); } }
    public Visibility IsListButtonVisible { get => _isListButtonVisible; set { _isListButtonVisible = value; OnPropertyChanged(); } }
    public Visibility IsTaskTextVisible { get => _isTaskTextVisible; set { _isTaskTextVisible = value; OnPropertyChanged(); } }
    public Visibility IsTaskButtonVisible { get => _isTaskButtonVisible; set { _isTaskButtonVisible = value; OnPropertyChanged(); } }

    public ObservableCollection<Todo> Todos { get; set; }
    public ObservableCollection<TodoCollection> TodoCollections { get; set; }
    public TodoCollection CurrentTodoCollection { get => _currentTodoCollection; set { _currentTodoCollection = value; OnPropertyChanged(); } }
    public Todo CurrentTodo { get => _currentTodo; set { _currentTodo = value; OnPropertyChanged(); } }
    

    public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }
    public RelayCommand ShowListTextCMD { get; }
    public RelayCommand ShowTaskTextCMD { get; }
    public RelayCommand CreateTaskCMD { get; }
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
        TodoCollections = new ObservableCollection<TodoCollection>();
        
        ShowListTextCMD = new RelayCommand(ShowListText);
        ShowTaskTextCMD = new RelayCommand(ShowTaskText);


        CreateTaskCMD = new RelayCommand(CreateTask);
        ReadTodoCMD = new RelayCommand(ReadTodo); //ta bort?
        UpdateTodoCMD = new RelayCommand(UpdateTodo);
        DeleteTodoCMD = new RelayCommand(DeleteTodo);

        CreateListCMD = new RelayCommand(CreateList);
        ReadListCMD = new RelayCommand(ReadList); //ta bort?
        UpdateListCMD = new RelayCommand(UpdateList);
        DeleteListCMD = new RelayCommand(DeleteList);

    }

    private void ShowTaskText(object obj)
    {
        IsTaskTextVisible = Visibility.Visible;
        IsTaskButtonVisible = Visibility.Collapsed;
    }

    private void ShowListText(object obj)
    {
        IsListTextVisible = Visibility.Visible;
        IsListButtonVisible = Visibility.Collapsed;
    }

    
    // Task CRUD:
    private void CreateTask(object obj)
    {
        throw new NotImplementedException();
    }
    private void ReadTodo(object obj)
    {
        throw new NotImplementedException();
    }
    private void UpdateTodo(object obj)
    {
        throw new NotImplementedException();
    }
    private void DeleteTodo(object obj)
    {
        throw new NotImplementedException();
    }

    // List CRUD:

    private void CreateList(object obj)
    {
        string? newListName = obj.ToString();
        if (!string.IsNullOrWhiteSpace(newListName))
        {
            using var db = new TodoDbContext();

            var newCollection = new TodoCollection { Id = ObjectId.GenerateNewId(), Title = newListName, Users = new List<User>(), Todos = new List<Todo>() };
            db.TodoCollections.Add(newCollection);
            db.SaveChanges();
            newCollection.Users.Add(MainViewModel.UserViewModel.CurrentUser);
            TodoCollections.Add(newCollection);
            db.TodoCollections.Update(newCollection);
            db.SaveChanges();

            CurrentTodoCollection = newCollection;

            OnPropertyChanged(nameof(CurrentTodoCollection));

            newListName = string.Empty;
            IsListTextVisible = Visibility.Collapsed;
            IsListButtonVisible = Visibility.Visible;
        }
    }
    private void ReadList(object obj)
    {
        throw new NotImplementedException();
    }
    private void UpdateList(object obj)
    {
        throw new NotImplementedException();
    }
    private void DeleteList(object obj)
    {
        throw new NotImplementedException();
    }
}
