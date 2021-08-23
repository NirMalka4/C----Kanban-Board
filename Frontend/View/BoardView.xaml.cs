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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardModel BoardM { get; set; }
        private UserModel UserM { get; set; }
        private BackendController BC { get; set; }

        private BoardViewModel bvm;

        internal BoardView(BackendController bc, UserModel u, BoardModel b)
        {
            InitializeComponent();
            this.BC = bc;
            this.UserM = u;
            this.BoardM = b;
            DataContext = new BoardViewModel(BC, BoardM, UserM);
            bvm = (BoardViewModel)DataContext;
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            new Boards(UserM).Show();
            this.Close();
        }

        private void LoadColumn_Button(object sender, RoutedEventArgs e)
        {
            bvm.LoadColumn();
            this.Close();
        }

        private void AddColumn_Button(object sender, RoutedEventArgs e)
        {
            new AddColumnWindow(BC, UserM, BoardM).Show();
            this.Close();
        }

        private void RemoveColumn_Button(object sender, RoutedEventArgs e)
        {
            bvm.RemoveColumn();
        }

        private void MoveUp_Button(object sender, RoutedEventArgs e)
        {
            bvm.MoveUp();
        }

        private void MoveDown_Button(object sender, RoutedEventArgs e)
        {
            bvm.MoveDown();
        }

        private void Rename_Button(object sender, RoutedEventArgs e)
        {
            bvm.RenameColumn();
            this.Close();
        }

        private void Shift_Button(object sender, RoutedEventArgs e)
        {
            bvm.ShiftColumn();
            this.Close();
        }
    }
}
