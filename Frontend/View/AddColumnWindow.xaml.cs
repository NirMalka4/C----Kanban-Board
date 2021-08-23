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
    /// Interaction logic for AddColumnWindow.xaml
    /// </summary>
    public partial class AddColumnWindow : Window
    {
        private BackendController BC { get; set; }
        private UserModel UserM { get; set; }
        private BoardModel BoardM { get; set; }
        private AddColumnViewModel acvm;

        internal AddColumnWindow(BackendController bc, UserModel u, BoardModel b)
        {
            InitializeComponent();
            this.BC = BC;
            this.UserM = u;
            this.BoardM = b;
            DataContext = new AddColumnViewModel(BC, UserM, BoardM);
            acvm = (AddColumnViewModel)DataContext;
            
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            new BoardView(BC, UserM, BoardM).Show();
            this.Close();
        }

        private void AddColumn_Button(object sender, RoutedEventArgs e)
        {
            acvm.AddColumn();
        }
    }
}
