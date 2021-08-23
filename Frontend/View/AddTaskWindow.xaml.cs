using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private UserModel User { get; set; }
        private BoardModel Board { get; set; }
        private BackendController BC { get; set; }
        private ColumnModel ColM { get; set; }

        private AddTaskViewModel atvm;

        internal AddTaskWindow(UserModel u, BoardModel b, BackendController bc, ColumnModel c)
        {
            InitializeComponent();
            this.User = u;
            this.Board = b;
            this.BC = bc;
            this.ColM = c;
            DataContext = new AddTaskViewModel(User, Board, BC, ColM);
            this.atvm = (AddTaskViewModel)DataContext;
        }

        private void Add_Task(object sender, RoutedEventArgs e)
        {
            atvm.AddTask();
        }

        private void Back_Task(object sender, RoutedEventArgs e)
        {
            new ColumnView(BC, User, Board, Board.Cols[ColM.Ordinal]).Show();
            this.Close();
        }
    }
}
