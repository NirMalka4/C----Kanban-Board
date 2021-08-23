using System;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.Business_Layer.User
{
    internal class PassCheckerLength : BasePassword
    {
        private const string LENGTH = "^.{4,20}$";

        internal PassCheckerLength(IPasswordChecker wrapee) : base(wrapee) { }
        public override void Check(string pass)
        {
            if (!Regex.IsMatch(pass, LENGTH))
                throw new ArgumentException("Password should contain at least 4 and at most 20 characters");
            base.Check(pass);
        }
    }
}
