using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class LoginViewModel : NotifiableObject
    {
        private string email;
        public string Email
        {
            get => email;
            set
            {
                this.email = value;
            }
        }


        private string password;
        public string Password { 
            get => password;
            set
            {
                this.password = value; 
            }
        }

        private BackendController BC { get; set; }

        public MessageViewModel MSG { get; set; }

        public LoginViewModel()
        {
            this.BC = new BackendController();
            this.MSG = new MessageViewModel();
        }

        /// <summary>
        /// trys to register via Email, Password
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void Register()
        {
            try
            {
                BC.Register(Email, Password);
                MSG.Success("Registered Successfully");
                
            }
            catch(Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// trys loggin in via Email, Password
        /// opens boardsview if succeeded
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal UserModel Login()
        {
            try
            {
                UserModel u = BC.Login(Email, Password);
                MSG.Success("LoggedInSuccessfully");
                return u;
            }
            catch(Exception e)
            {
                MSG.Fail(e.Message);
                return null;
            }
        }

    }
}
