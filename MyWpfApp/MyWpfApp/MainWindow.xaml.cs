using MyWpfApp.ViewModel;

using System.Collections.Generic;
using System.Windows;

namespace MyWpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindwoViewModel() { SelectedItemsText = "", Items = new List<string>{ "aaa", "bbb", "ccc"} };
        }
    }
}
