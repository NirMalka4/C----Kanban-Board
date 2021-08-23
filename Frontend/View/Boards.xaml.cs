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
    /// Interaction logic for Boards.xaml
    /// </summary>
    public partial class Boards : Window
    {
        private BoardsModel bvm;
        private BackendController BC { get; set; }
        private UserModel UserM { get; set; }

        internal Boards(UserModel u)
        {
            InitializeComponent();
            DataContext = new BoardsModel(u);
            bvm = (BoardsModel)DataContext;
            this.UserM = u;

        }

        private void Load_Board(object sender, RoutedEventArgs e)
        {
            bvm.LoadBoard();
            this.Close();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            bvm.Logout();
            LoginWindow lw = new LoginWindow();
            lw.Show();
            this.Close();
        }

        private void Add_Board(object sender, RoutedEventArgs e)
        {
            AddBordView abv = new AddBordView(UserM);
            abv.Show();
            this.Close();
        }

        private void Remove_Board(object sender, RoutedEventArgs e)
        {
            bvm.RemoveBoard();
        }

        private void Join_Board(object sender, RoutedEventArgs e)
        {
            bvm.JoinBoard();
            this.Close();
        }

        private void InProgressTasks(object sender, RoutedEventArgs e)
        {
            bvm.InProgressTasks();
            this.Close();
        }
    }
}
