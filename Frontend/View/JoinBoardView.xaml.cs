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
    /// Interaction logic for JoinBoardView.xaml
    /// </summary>
    public partial class JoinBoardView : Window
    {
        private BackendController BC { get; set; }
        private UserModel UserM { get; set; }
        private JoinBoardViewModel jbvm;

        internal JoinBoardView(BackendController bc, UserModel u)
        {
            InitializeComponent();
            this.BC = bc;
            this.UserM = u;
            DataContext = new JoinBoardViewModel(UserM);
            jbvm = (JoinBoardViewModel)DataContext;
        }

        private void JoinBoard_Button(object sender, RoutedEventArgs e)
        {
            jbvm.JoinBoard();
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            new Boards(UserM).Show();
            this.Close();
        }
    }
}
