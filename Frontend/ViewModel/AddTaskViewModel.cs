using Frontend.Model;
using Frontend.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class AddTaskViewModel : NotifiableObject
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        private UserModel User { get; set; }

        private BoardModel Board { get; set; }

        private BackendController BC { get; set; }
        private ColumnModel Col { get; set; }

        public MessageViewModel MSG { get; set; }
        internal AddTaskViewModel(UserModel u, BoardModel b, BackendController bc, ColumnModel c)
        {
            this.User = u;
            this.Board = b;
            this.BC = bc;
            this.Col = c;
            this.MSG = new MessageViewModel();
        }

        /// <summary>
        /// add new task to current ciolumn
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void AddTask()
        {
            try
            {
                Board.AddTask(Col, Title, Desc, new DateTime(Year, Month, Day));
                MSG.Success("Added Task Successfully");
            }
            catch(Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

    }
}
