using System;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataLayer.DTOs;
using IntroSE.Kanban.Backend.Business_Layer.User.Password;
using IntroSE.Kanban.Backend.Business_Layer.User;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace IntroSE.Kanban.Backend.Business_Layer

/* Business Layer User object */
{
    internal class BUser
    {
        private string _Email;
        private Password _Password;
        internal bool Status { get; set; }
        private readonly DTO MyDalObj;
        private const string EMAIL_PATTERN = @"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))"
                               + "@"
                               + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])"
                               + "|"
                               + @"(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

        internal BUser(string email, string pass)
        {
            Email = email;
            _Password = new Password(pass);
            Status = false;
            MyDalObj = new DUser(email, pass);
            MyDalObj.Insert();

        }

        ///<summary> 
        ///Create an instance basen upon DB data.
        ///</summary>
        internal BUser(DUser src)
        {
            _Email = src.Email;
            _Password = new Password(src.Password);
            MyDalObj = src;
        }

        ///<summary> 
        ///Validate param email is aligned with email structure rules. 
        ///</summary>
        private bool ValidateLegalEmail(string email) { return email != null && Regex.IsMatch(email, EMAIL_PATTERN); }

        internal string Email
        {
            get => _Email;
            private set
            {
                if (!ValidateLegalEmail(value))
                    throw new ArgumentException(String.Format("Email: {0} is illegal", value));
                _Email = value;
            }
        }


        ///<summary>
        ///Login current user iff param pass is equal to password property(one setted at creation time). 
        ///</summary>
        internal void Login(string pass)
        {
            if (!_Password.Equals(pass)) throw new ArgumentException("Incorrect Password");
            Status = true;
        }

        internal void Delete() { MyDalObj.Delete(); }
        public override string ToString() => $"User Email: {Email}. User Status: {Status}";
    }
}
