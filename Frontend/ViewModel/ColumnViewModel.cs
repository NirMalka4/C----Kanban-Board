using Frontend.Model;
using Frontend.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class ColumnViewModel : NotifiableObject
    {
        private ColumnModel colM;
        public ColumnModel ColM {
            get => colM;
            set
            {
                colM = value;
                RaisePropertyChanged("ColM");
            }
        }
        private BackendController BC { get; set; }
        private UserModel UserM { get; set; }
        private BoardModel BoardM { get; set; }

        public MessageViewModel MSG { get; set; }
        public string PageTitle { get; set; }
        internal ColumnViewModel(BackendController bc, UserModel u, BoardModel b, ColumnModel c)
        {
            this.BC = bc;
            this.UserM = u;
            this.BoardM = b;
            this.ColM = c;
            this.PageTitle = c.Name;
            this.MSG = new MessageViewModel();
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

        public string FilterText { get; set; }

        /// <summary>
        /// advance selectedTask
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void AdvanceTask()
        {
            try
            {
                BoardM.AdvanceTask(SelectedTask, ColM);
                MSG.Success("Task Advanced");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// open updateTaskView for selected task
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void UpdateTask()
        {
            new UpdateTaskWindow(UserM, BoardM, SelectedTask, BC, ColM).Show();
        }

        /// <summary>
        /// open taskView for selected task
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void ViewTask()
        {
            new TaskView(BC, UserM, BoardM, SelectedTask, ColM).Show();
        }

        /// <summary>
        /// sort tasks by dueDate
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void SortByDueDate()
        {
            ColM.SortByDueDate();
        }

        /// <summary>
        /// filters tasks display by inserted text
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void FilterByText()
        {
            ColM.FilterByText(FilterText);
        }

        /// <summary>
        /// undos task filtering by text
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void UnFilterByText()
        {
            ColM.UnFilterByText(FilterText);
        }
    }
}
