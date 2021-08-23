using Frontend.Model;
using Frontend.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class UpdateTaskViewModel
    {
        private BackendController BC { get; set; }
        private BoardModel BoardM { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Assignee { get; set; }
        public TaskModel TaskM { get; set; }
        private ColumnModel ColM { get; set; }

        public MessageViewModel MSG { get; set; }

        public UpdateTaskViewModel(BoardModel b, BackendController bc, TaskModel t, ColumnModel c)
        {
            this.BC = bc;
            this.BoardM = b;
            this.TaskM = t;
            this.ColM = c;
            this.MSG = new MessageViewModel();
        }

        /// <summary>
        /// update selected tasks title
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void UpdateTitle()
        {
            try
            {
                BoardM.UpdateTaskTitle(Title, TaskM, ColM.Ordinal);
                MSG.Success("new title has been set");
            }
            catch(Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// update selected tasks description
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void UpdateDesc()
        {
            try
            {
                BoardM.UpdateTaskDesc(Desc, TaskM, ColM.Ordinal);
                MSG.Success("new description has been set");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// update selected tasks dueDate
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void UpdateDueDate()
        {
            try
            {
                BoardM.UpdateTaskDueDate(new DateTime(Year, Month, Day), TaskM, ColM.Ordinal);
                MSG.Success("new dueDate has been set");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// update selected tasks assignee
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void UpdateAssignee()
        {
            try
            {
                BoardM.UpdateAssignee(Assignee, TaskM, ColM.Ordinal);
                MSG.Success("new assignee has been set");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }
    }
}
