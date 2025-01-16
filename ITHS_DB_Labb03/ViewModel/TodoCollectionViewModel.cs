using ITHS_DB_Labb03.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_DB_Labb03.ViewModel
{
    internal class TodoCollectionViewModel : VMBase
    {
		private MainViewModel _mainViewModel;

		public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }


        public TodoCollectionViewModel(MainViewModel mainViewModel)
        {

            MainViewModel = mainViewModel;

        }

    }
}
