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
    /// Interaction logic for RenameColumnView.xaml
    /// </summary>
    public partial class RenameColumnView : Window
    {
        private BoardModel BoardM { get; set; }
        private UserModel UserM { get; set; }
        private BackendController BC { get; set; }
        private ColumnModel Col { get; set; }
        private RenameColumnViewModel rcvm;

        internal RenameColumnView(BackendController bc, UserModel u, BoardModel b, ColumnModel c)
        {
            InitializeComponent();
            this.BC = bc;
            this.UserM = u;
            this.BoardM = b;
            this.Col = c;
            DataContext = new RenameColumnViewModel(BC, UserM, BoardM, Col);
            rcvm = (RenameColumnViewModel)DataContext;
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            new BoardView(BC, UserM, BoardM).Show();
            this.Close();
        }

        private void Update_Button(object sender, RoutedEventArgs e)
        {
            rcvm.RenameColumn();
        }
    }
}
