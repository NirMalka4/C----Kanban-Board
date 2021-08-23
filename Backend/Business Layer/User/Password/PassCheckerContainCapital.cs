using System;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.Business_Layer.User
{
    internal class PassCheckerContainCapital : BasePassword
    {
        private const string CAPITAL = @"[A-Z]+";

        internal PassCheckerContainCapital(IPasswordChecker wrapee) : base(wrapee) { }
        public override void Check(string pass)
        {
            if (!Regex.IsMatch(pass, CAPITAL))
                throw new ArgumentException("Password should contain at least one upper char");
            base.Check(pass);
        }
    }
}
