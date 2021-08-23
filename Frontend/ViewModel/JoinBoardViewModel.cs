using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class JoinBoardViewModel
    {
        private UserModel UserM { get; set; }
        public string CreatorEmail { get; set; }
        public string BoardName { get; set; }

        public MessageViewModel MSG { get; set; }

        internal JoinBoardViewModel(UserModel u)
        { 
            this.UserM = u;
            this.MSG = new MessageViewModel();
        }

        /// <summary>
        /// joins board named BoardName
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void JoinBoard()
        {
            try
            {
                UserM.JoinBoard(CreatorEmail, BoardName);
                MSG.Success("Joined Board");
            }
            catch(Exception e)
            {
                MSG.Fail(e.Message);
            }
        }
    }
}
