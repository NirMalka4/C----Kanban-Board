using Frontend.Model;
using Frontend.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class InProgressTasksViewModel : NotifiableObject
    {
        public UserModel UserM { get; set; }
        public MessageViewModel MSG { get; set; }

        public InProgressTasksViewModel(UserModel u)
        {
            this.UserM = u;
            MSG = new MessageViewModel();
        }

        /// <summary>
        /// opens taskview for selected task
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void ViewTask()
        {
            try
            {
                new TaskView(UserM, SelectedTask).Show();
            }
            catch(Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// opens updateinprogresstaskview for selected task
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void UpdateTask()
        {
            try
            {
                new InProgressTaskUpdateView(UserM, SelectedTask, UserM.BackendController).Show();
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }


        public void AdvanceTask()
        {
            try
            {
                UserM.BackendController.AdvanceTask(UserM.Email, UserM.MyTasksDict[SelectedTask].Item1, UserM.MyTasksDict[SelectedTask].Item2, 1, SelectedTask.ID);
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        private TaskModel selectedTask;
        public TaskModel SelectedTask
        {
            get => selectedTask;
            set
            {
                selectedTask = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }
        private bool enableForward = false;
        public bool EnableForward
        {
            get => enableForward;
            private set
            {
                enableForward = true;
                RaisePropertyChanged("EnableForward");
            }
        }

    }
}
