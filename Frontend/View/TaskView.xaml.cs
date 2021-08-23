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
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        private BackendController BC { get; set; }

        private TaskViewModel tvm;
        private UserModel UserM { get; set; }
        private BoardModel BoardM { get; set; }
        private TaskModel TaskM { get; set; }
        private ColumnModel ColM { get; set; }

        internal TaskView(BackendController bc, UserModel u, BoardModel b, TaskModel t, ColumnModel c)
        {
            InitializeComponent();
            this.BC = bc;
            this.UserM = u;
            this.BoardM = b;
            this.TaskM = t;
            this.DataContext = new TaskViewModel(t);
            tvm = (TaskViewModel)DataContext;
            this.ColM = c;
        }

        //constructor for inprogress tasks
        internal TaskView(UserModel u, TaskModel t)
        {
            InitializeComponent();
            this.BC = null;
            this.UserM = u;
            this.BoardM = null;
            this.TaskM = t;
            this.DataContext = new TaskViewModel(t);
            tvm = (TaskViewModel)DataContext;
            this.ColM = UserM.MyTasks;
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            if (BC != null) {
                new ColumnView(BC, UserM, BoardM, ColM).Show();
                this.Close();
            }
            else
            {
                new InProgressTasksView(UserM).Show();
                this.Close();
            }
        }
    }
}
