using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
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

    public ObservableCollection<Todo> Todos { get; set; }
    public ObservableCollection<TodoCollection> TodoCollection { get; set; }

    //public ObservableCollection<User> Users { get; set; } ?
    public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }

    public Visibility IsListTextVisible { get => _isListTextVisible; set { _isListTextVisible = value; OnPropertyChanged(); } }
    public Visibility IsListButtonVisible { get => _isListButtonVisible; set { _isListButtonVisible = value; OnPropertyChanged(); } }
    public Visibility IsTaskTextVisible { get => _isTaskTextVisible; set { _isTaskTextVisible = value; OnPropertyChanged(); } }
    public Visibility IsTaskButtonVisible { get => _isTaskButtonVisible; set { _isTaskButtonVisible = value; OnPropertyChanged(); } }


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

        AddNewTaskCMD = new RelayCommand(AddNewTask); //Byt namn till t ex Show..
        AddNewListCMD = new RelayCommand(AddNewList); //Byt namn till Show..

        CreateTodoCMD = new RelayCommand(CreateTodo); // Enter eller "Save"
        ReadTodoCMD = new RelayCommand(ReadTodo); //ta bort
        UpdateTodoCMD = new RelayCommand(UpdateTodo);
        DeleteTodoCMD = new RelayCommand(DeleteTodo);

        CreateListCMD = new RelayCommand(CreateList); // Enter eller "Save"
        ReadListCMD = new RelayCommand(ReadList); //ta bort
        UpdateListCMD = new RelayCommand(UpdateList);
        DeleteListCMD = new RelayCommand(DeleteList);

    }
    private void ShowListText(object obj)
    {
        IsListTextVisible = Visibility.Visible;
        IsListButtonVisible = Visibility.Collapsed;
    }

    private void ShowTaskText(object obj)
    {
        IsTaskTextVisible = Visibility.Visible;
        IsTaskButtonVisible = Visibility.Collapsed;
    }

    private void AddNewTask(object obj)
    {
        throw new NotImplementedException();
    }



    // Todo CRUD:
    private void CreateTodo(object obj)
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
        throw new NotImplementedException();
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
