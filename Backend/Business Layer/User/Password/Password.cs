namespace IntroSE.Kanban.Backend.Business_Layer.User.Password
{
    /* Password implementation based on Decorator pattern. */
    internal class Password : IPasswordChecker
    {
        private string _password;
        private IPasswordChecker _checker;

        internal Password(string password)
        {
            /* Note: The following verification already cover the top 20 password published by the National Cyber Security Centre. */

            IPasswordChecker numberChcker = new PassCheckerContainNumber();  // check pass contains a number.
            IPasswordChecker capitalChecker = new PassCheckerContainCapital(numberChcker); // check pass contains a capital letter.
            IPasswordChecker letterChecker = new PassCheckerContainLetter(capitalChecker); // check pass contains a letter.
            IPasswordChecker lengthChecker = new PassCheckerLength(letterChecker); // check pass length is within the range [4,20].
            IPasswordChecker NCSCChecker = new PassCheckerNCSC(lengthChecker); // check current password isnt one of the top 20 NCSC passwords.
            _checker = new PassCheckerNull(NCSCChecker); // base case: check pass is neither null nor empty.
            Check(password);
            _password = password;
        }

        //delegate
        public void Check(string pass) => _checker.Check(pass);

        public override bool Equals(object obj) => obj is string && ((string)obj).Equals(_password);

        public override int GetHashCode() => _password.GetHashCode() ^ _checker.GetHashCode();

    }
}
