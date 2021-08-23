using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class RenameColumnViewModel
    {
        public string NewColumnName { get; set; }
        private BoardModel BoardM { get; set; }
        private UserModel UserM { get; set; }
        private BackendController BC { get; set; }

        private ColumnModel Col { get; set; }

        public MessageViewModel MSG { get; set; }

        internal RenameColumnViewModel(BackendController bc, UserModel u, BoardModel b, ColumnModel c)
        {
            this.BC = bc;
            this.UserM = u;
            this.BoardM = b;
            this.Col = c;
            this.MSG = new MessageViewModel();
        }

        /// <summary>
        /// rename current column
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void RenameColumn()
        {
            try
            {
                BoardM.RenameColumn(Col.Ordinal, NewColumnName);
                MSG.Success("Column Renamed");
            }
            catch(Exception e)
            {
                MSG.Fail(e.Message);
            }
        }
    }
}
