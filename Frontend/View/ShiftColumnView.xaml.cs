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
    /// Interaction logic for ShiftColumnView.xaml
    /// </summary>
    public partial class ShiftColumnView : Window
    {
        private BoardModel BoardM { get; set; }
        private UserModel UserM { get; set; }
        private BackendController BC { get; set; }
        private int Ordinal { get; set; }

        private ShiftColumnViewModel scvm;

        internal ShiftColumnView(BackendController bc, UserModel u, BoardModel b, int ordinal)
        {
            InitializeComponent();
            this.BC = bc;
            this.UserM = u;
            this.BoardM = b;
            Ordinal = ordinal;
            DataContext = new ShiftColumnViewModel(BoardM, Ordinal);
            this.scvm = (ShiftColumnViewModel)DataContext;
        }

        private void Shift_Button(object sender, RoutedEventArgs e)
        {
            scvm.ShiftColumn();
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            new BoardView(BC, UserM, BoardM).Show();
            this.Close();
        }
    }
}
