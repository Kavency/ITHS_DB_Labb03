﻿using ITHS_DB_Labb03.Core;
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
    public ObservableCollection<Todo> Todos { get; set; }
    public ObservableCollection<TodoCollection> TodoCollection { get; set; }

    //public ObservableCollection<User> Users { get; set; } ?
		public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }

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

        }

    }
}
