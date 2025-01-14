using ITHS_DB_Labb03.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_DB_Labb03.ViewModel
{
    internal class UserViewModel : VMBase
    {
        private readonly MainViewModel mainViewModel;

        public UserViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;


        }
    }
}
