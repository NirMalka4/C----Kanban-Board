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
    /// Interaction logic for UpdateTaskWindow.xaml
    /// </summary>
    public partial class UpdateTaskWindow : Window
    {
        private BackendController BC { get; set; }
        private BoardModel BoardM { get; set; }
        private TaskModel TaskM { get; set; }
        private UserModel UserM { get; set; }
        private UpdateTaskViewModel utvm { get; set; }
        private ColumnModel ColM { get; set; }

        internal UpdateTaskWindow(UserModel u, BoardModel b, TaskModel t, BackendController bc, ColumnModel c)
        {
            this.UserM = u;
            this.BC = bc;
            this.BoardM = b;
            this.TaskM = t;
            InitializeComponent();
            this.ColM = c;
            DataContext = new UpdateTaskViewModel(BoardM, BC, TaskM, ColM);
            utvm = (UpdateTaskViewModel)DataContext;
        }

        private void UpdateTitle_Button(object sender, RoutedEventArgs e)
        {
            utvm.UpdateTitle();
        }

        private void UpdateDesc_Button(object sender, RoutedEventArgs e)
        {
            utvm.UpdateDesc();
        }

        private void UpdateDueDate_Button(object sender, RoutedEventArgs e)
        {
            utvm.UpdateDueDate();
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            new ColumnView(BC, UserM, BoardM, ColM).Show();
            this.Close();
        }

        private void UpdateAssignee_Button(object sender, RoutedEventArgs e)
        {
            utvm.UpdateAssignee();
        }
    }
}
