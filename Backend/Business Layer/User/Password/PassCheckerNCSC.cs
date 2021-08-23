using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.Business_Layer.User.Password
{
    class PassCheckerNCSC : BasePassword
    {
        private readonly string[] NCSC_TOP20 = { "123456", "123456789", "qwerty", "password", "1111111", "12345678", "abc123",
        "1234567", "password1", "12345", "1234567890", "123123", "000000", "Iloveyou", "1234", "1q2w3e4r5t", "Qwertyuiop", "123",
        "Monkey", "Dragon"};

        internal PassCheckerNCSC(IPasswordChecker wrapee) : base(wrapee) { }

        public override void  Check(string pass)
        {
            if (NCSC_TOP20.Contains(pass))
                throw new ArgumentException("Weak Password.");
            base.Check(pass);
        }
    }
}
