﻿
using ITHS_DB_Labb03.Core;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ITHS_DB_Labb03;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            using (var db = new TodoDbContext())
            {
                db.Database.EnsureCreated();

                Debug.WriteLine($"Database connection: {db.Database.CanConnect()}");

            }
        }
    }
