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
    /// Interaction logic for InProgressTaskUpdateView.xaml
    /// </summary>
    public partial class InProgressTaskUpdateView : Window
    {
        private BackendController BC { get; set; }
        private TaskModel TaskM { get; set; }
        private UserModel UserM { get; set; }
        private InProgressTasksUpdateViewModel iptuvm;

        internal InProgressTaskUpdateView(UserModel u, TaskModel t, BackendController bc)
        {
            InitializeComponent();
            this.UserM = u;
            this.BC = bc;
            this.TaskM = t;
            this.DataContext = new InProgressTasksUpdateViewModel(BC, TaskM, UserM);
            iptuvm = (InProgressTasksUpdateViewModel)DataContext;
        }

        private void UpdateDesc_Button(object sender, RoutedEventArgs e)
        {
            iptuvm.UpdateDesc();
        }

        private void UpdateTitle_Button(object sender, RoutedEventArgs e)
        {
            iptuvm.UpdateTitle();
        }

        private void UpdateAssignee_Button(object sender, RoutedEventArgs e)
        {
            iptuvm.UpdateAssignee();
        }

        private void UpdateDueDate_Button(object sender, RoutedEventArgs e)
        {
            iptuvm.UpdateDueDate();
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            new InProgressTasksView(UserM).Show();
            this.Close();
        }
    }
}
