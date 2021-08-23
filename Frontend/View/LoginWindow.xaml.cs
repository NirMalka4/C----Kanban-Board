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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel lvm;

        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
            this.lvm = (LoginViewModel)DataContext;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            lvm.Register();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = lvm.Login();
            if (u != null)
            {
                Boards b = new Boards(u);
                b.Show();
                this.Close();
            }
        }
    }
}
