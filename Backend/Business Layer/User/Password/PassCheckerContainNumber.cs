using System;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.Business_Layer.User
{
    internal class PassCheckerContainNumber : IPasswordChecker
    {
        private const string NUMBER = @"[0-9]+";
        public void Check(string pass)
        {
            if (!Regex.IsMatch(pass, NUMBER))
                throw new ArgumentException("Password should contain at least one number");
        }

    }
}
