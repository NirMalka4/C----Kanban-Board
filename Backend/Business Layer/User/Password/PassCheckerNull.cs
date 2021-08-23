using System;

namespace IntroSE.Kanban.Backend.Business_Layer.User.Password
{
    class PassCheckerNull : BasePassword
    {
        internal PassCheckerNull(IPasswordChecker wrapee) : base(wrapee) { }

        public override void Check(string pass)
        {
            if (string.IsNullOrEmpty(pass))
                throw new ArgumentException("Password must be non empty string.");
            base.Check(pass);
        }

    }
}
