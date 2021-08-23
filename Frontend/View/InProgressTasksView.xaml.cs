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
    /// Interaction logic for InProgressTasksView.xaml
    /// </summary>
    public partial class InProgressTasksView : Window
    {
        private UserModel UserM { get; set; }
        private InProgressTasksViewModel iptvm;

        internal InProgressTasksView(UserModel u)
        {
            InitializeComponent();
            this.UserM = u;
            this.DataContext = new InProgressTasksViewModel(UserM);
            iptvm = (InProgressTasksViewModel)DataContext;

        }

        private void ViewTask_Button(object sender, RoutedEventArgs e)
        {
            iptvm.ViewTask();
            this.Close();
        }

        private void UpdateTask_Button(object sender, RoutedEventArgs e)
        {
            iptvm.UpdateTask();
            this.Close();
        }

        private void AdvanceTask_Button(object sender, RoutedEventArgs e)
        {
            iptvm.AdvanceTask();
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            new Boards(UserM).Show();
            this.Close();
        }
    }
}
