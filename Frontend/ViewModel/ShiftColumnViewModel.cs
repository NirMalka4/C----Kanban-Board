using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class ShiftColumnViewModel
    {
        private BoardModel BoardM { get; set; }
        public MessageViewModel MSG {get;set;}
        public int Shift { get; set; }
        private int Ordinal { get; set; }

        internal ShiftColumnViewModel(BoardModel b,  int ordinal)
        {
            Ordinal = ordinal;
            this.BoardM = b;
            MSG = new MessageViewModel();
        }

        /// <summary>
        /// shifts column at ordinal by Shift
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void ShiftColumn()
        {
            try
            {
                BoardM.ShiftColumn(Ordinal, Shift);
                MSG.Success("Column moved");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }
    
    }
}
