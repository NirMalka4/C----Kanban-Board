using System;

namespace IntroSE.Kanban.Backend.Business_Layer.User
{
    internal abstract class BasePassword : IPasswordChecker
    {
        protected IPasswordChecker _wrapee;

        internal BasePassword(IPasswordChecker wrapee)
        {
            _wrapee = wrapee;
        }

        public virtual void Check(string pass)
        {
            if (_wrapee is null)
                throw new Exception($" No wrapped component was supplied.");
            _wrapee.Check(pass);
        }

    }
}
