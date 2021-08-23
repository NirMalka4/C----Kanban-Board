using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class AddColumnViewModel
    {
        private BackendController BC { get; set; }
        private BoardModel BoardM { get; set; }
        private UserModel UserM { get; set; }

        public String ColName { get; set; }
        public int Ordinal { get; set; }

        public MessageViewModel MSG { get; set; }

        internal AddColumnViewModel(BackendController bc, UserModel u, BoardModel b)
        {
            this.BC = bc;
            this.UserM = u;
            this.BoardM = b;
            this.MSG = new MessageViewModel();
        }

        /// <summary>
        /// add new column to current
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void AddColumn()
        {
            try
            {
                BoardM.AddColumn(Ordinal, ColName);
                MSG.Success("Added Column Successfully");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }

        }

    }
}
