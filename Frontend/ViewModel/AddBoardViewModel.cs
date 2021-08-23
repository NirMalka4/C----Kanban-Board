using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class AddBoardViewModel : NotifiableObject
    {

        private string bname;
        public string BName
        {
            get => bname;
            set
            {
                bname = value;
            }
        }

        public UserModel UserM { get; private set; }

        private BackendController BC;

        public MessageViewModel MSG { get; set; }

        internal AddBoardViewModel(UserModel u)
        {
            this.UserM = u;
            this.BC = u.BackendController;
            this.MSG = new MessageViewModel();
        }

        /// <summary>
        /// add new board
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void AddBoard()
        {
            try
            {
                UserM.AddBoard(BName);
                MSG.Success("Successfully added Board");
            }
            catch(Exception e)
            {
                MSG.Fail(e.Message);
            }
        }
    }
}
