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
    /// Interaction logic for AddBordView.xaml
    /// </summary>
    public partial class AddBordView : Window
    {
        private UserModel UserM { get; set; }
        private AddBoardViewModel abvm;
        

        internal AddBordView(UserModel u)
        {
            InitializeComponent();
            this.UserM = u;
            DataContext = new AddBoardViewModel(UserM);
            abvm = (AddBoardViewModel)DataContext;
        }

        private void Add_Board(object sender, RoutedEventArgs e)
        {
            abvm.AddBoard();
        }

        private void Back_Board(object sender, RoutedEventArgs e)
        {
            Boards bw = new Boards(UserM);
            bw.Show();
            this.Close();
        }
    }
}
