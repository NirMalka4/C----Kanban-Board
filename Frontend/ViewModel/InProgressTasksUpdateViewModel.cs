using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class InProgressTasksUpdateViewModel
    {
        private BackendController BC { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Assignee { get; set; }
        public TaskModel TaskM { get; set; }
        private UserModel UserM { get; set; }

        public MessageViewModel MSG { get; set; }

        public InProgressTasksUpdateViewModel(BackendController bc, TaskModel t, UserModel u)
        {
            this.BC = bc;
            this.TaskM = t;
            this.UserM = u;
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
                BC.UpdateTaskTitle(UserM.Email, UserM.MyTasksDict[TaskM].Item1, UserM.MyTasksDict[TaskM].Item2, 1, TaskM.ID, Title);
                UserM.InProgressTasks();
                MSG.Success("new title has been set");
            }
            catch (Exception e)
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
                BC.UpdateTaskDesc(UserM.Email, UserM.MyTasksDict[TaskM].Item1, UserM.MyTasksDict[TaskM].Item2, 1, TaskM.ID, Desc);
                UserM.InProgressTasks();
                MSG.Success("new description has been set");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// update selected tasks duedate
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void UpdateDueDate()
        {
            try
            {
                BC.UpdateTaskDueDate(UserM.Email, UserM.MyTasksDict[TaskM].Item1, UserM.MyTasksDict[TaskM].Item2, 1, TaskM.ID, new DateTime(Year, Month, Day));
                UserM.InProgressTasks();
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
                BC.AssignTask(UserM.Email, UserM.MyTasksDict[TaskM].Item1, UserM.MyTasksDict[TaskM].Item2, 1, TaskM.ID, Assignee);
                UserM.InProgressTasks();
                MSG.Success("new assignee has been set");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }
    }
}
