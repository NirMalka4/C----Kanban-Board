using System;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.Business_Layer.User
{
    internal class PassCheckerContainLetter : BasePassword
    {
        private const string LETTER = @"[a-z]+";

        internal PassCheckerContainLetter(IPasswordChecker wrapee) : base(wrapee) { }
        public override void Check(string pass)
        {
            if (!Regex.IsMatch(pass, LETTER))
                throw new ArgumentException("Password should contain at least one lower char");
            base.Check(pass);
        }
    }
}
