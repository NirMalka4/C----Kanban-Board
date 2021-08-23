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
    /// Interaction logic for ColumnView.xaml
    /// </summary>
    public partial class ColumnView : Window
    {
        internal ColumnModel ColM { get; set; }
        private BackendController BC { get; set; }
        private UserModel UserM { get; set; }
        private BoardModel BoardM { get; set; }
        private ColumnViewModel cvm;

        internal ColumnView(BackendController bc, UserModel u, BoardModel b, ColumnModel c)
        {
            InitializeComponent();
            this.BC = bc;
            this.UserM = u;
            this.BoardM = b;
            this.ColM = c;
            DataContext = new ColumnViewModel(BC, UserM, BoardM, ColM);
            cvm = (ColumnViewModel)DataContext;
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            new BoardView(BC, UserM, BoardM).Show();
            this.Close();
        }

        private void AddTask_Button(object sender, RoutedEventArgs e)
        {
            new AddTaskWindow(UserM, BoardM, BC, ColM).Show();
            this.Close();
        }

        private void ViewTask_Button(object sender, RoutedEventArgs e)
        {
            cvm.ViewTask();
            this.Close();
        }

        private void UpdateTask_Button(object sender, RoutedEventArgs e)
        {
            cvm.UpdateTask();
            this.Close();
        }

        private void AdvanceTask_Button(object sender, RoutedEventArgs e)
        {
            cvm.AdvanceTask();
        }

        private void SortByDueDate_Button(object sender, RoutedEventArgs e)
        {
            cvm.SortByDueDate();
        }

        private void FilterByText_Button(object sender, RoutedEventArgs e)
        {
            cvm.FilterByText();
        }

        private void UnFilterByText_Button(object sender, RoutedEventArgs e)
        {
            cvm.UnFilterByText();
        }
    }
}
