using Frontend.View;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Frontend.Model
{
    class ColumnModel : NotifiableObject
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private uint capacity;
        public uint Capacity
        {
            get => capacity;
            set
            {
                capacity = value;
            }
        }

        private int limit;
        public int Limit
        {
            get => limit;
            set
            {
                limit = value;
            }
        }

        private ObservableCollection<TaskModel> tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get => tasks;
            set
            {
                tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }

        private ObservableCollection<TaskModel> TasksToRecoverAfterFilter { get; set; }

        public int Ordinal { get; set; }

        internal BackendController BC { get; private set; }

        public ColumnModel(Column src, BackendController bc)
        {
            Tasks = new ObservableCollection<TaskModel>();
            TasksToRecoverAfterFilter = new ObservableCollection<TaskModel>();
            this.BC = bc;
            this.Name = src.Name;
            this.Limit = src.Limit;
            this.Capacity = src.Capacity;
            this.Ordinal = src.Ordinal;
            LoadTasks(src.Tasks);
        }

        //constructor for inprogresstasks
        public ColumnModel(string userEmail, IList<IntroSE.Kanban.Backend.ServiceLayer.Objects.Task> tsks, BackendController bc)
        {
            Tasks = new ObservableCollection<TaskModel>();
            TasksToRecoverAfterFilter = new ObservableCollection<TaskModel>();
            this.BC = bc;
            this.Name = "In Progress";
            LoadTasks(tsks);
            PaintTasks(userEmail);
        }

        /// <summary>
        /// load all tasks belong to this column from service layer
        /// </summary>
        /// <param name="STasks">list of tasks from service</param>
        private void LoadTasks(IList<IntroSE.Kanban.Backend.ServiceLayer.Objects.Task> STasks)
        {
            foreach (IntroSE.Kanban.Backend.ServiceLayer.Objects.Task t in STasks)
            {
                TaskModel newTask = new TaskModel(t, BC);
                Tasks.Add(newTask);
                TasksToRecoverAfterFilter.Add(newTask);
            }
        }

        /// <summary>
        /// add a task to this column
        /// </summary>
        /// <param name="userEmail">email of the user requestioin the action</param>
        /// <param name="creatorEmail">email of the board creator that this column belings to</param>
        /// <param name="boardName">the name of the board that this column beloings to</param>
        /// <param name="title">title for the new task</param>
        /// <param name="desc">description for the new task</param>
        /// <param name="dueDate">dueDate for the new task</param>
        public void AddTask(string userEmail, string creatorEmail, string boardName, String title, String desc, DateTime dueDate)
        {
            TaskModel tm = new TaskModel(BC.AddTask(userEmail, creatorEmail, boardName, title, desc, dueDate), BC);
            Tasks.Add(tm);
            PaintTask(userEmail, tm);
            TasksToRecoverAfterFilter.Add(tm);
            RaisePropertyChanged("Tasks");

        }

        /// <summary>
        /// remove a task from this column
        /// </summary>
        /// <param name="t">the task to be removed</param>
        public void RemoveTask(TaskModel t)
        {
            Tasks.Remove(t);
            TasksToRecoverAfterFilter.Remove(t);
            RaisePropertyChanged("Tasks");
        }

        /// <summary>
        /// advance a task from this column
        /// </summary>
        /// <param name="t">the task to be advanced</param>
        public void AdvancedTask(TaskModel t)
        {
            Tasks.Add(t);
            TasksToRecoverAfterFilter.Add(t);
            RaisePropertyChanged("Tasks");
        }

        /// <summary>
        /// paint the tasks accorfing to assignee and due date
        /// </summary>
        /// <param name="userEmail">of the user about toi view these tasks</param>
        internal void PaintTasks(string userEmail)
        {
            foreach(TaskModel t in Tasks)
            {
                if (userEmail.Equals(t.EmailAssignee))
                    t.BorderColor = "#FF003CFF";
                t.ColorByDate();
            }
        }

        /// <summary>
        /// paint a task according to assignee and due date
        /// </summary>
        /// <param name="userEmail">email of the user about to view these tasks</param>
        /// <param name="tm">task to paint</param>
        internal void PaintTask(string userEmail, TaskModel tm)
        {
            if (userEmail.Equals(tm.EmailAssignee))
                tm.BorderColor = "#FF003CFF";
            tm.ColorByDate();
            
        }

        /// <summary>
        /// sort the tasks by due date
        /// </summary>
        internal void SortByDueDate()
        {
            Tasks.OrderBy(t => t.DueDate);
        }

        /// <summary>
        /// filter tasks that have text in their title or description
        /// </summary>
        /// <param name="txt">text to filter by in title and descriptions</param>
        internal void FilterByText(string txt)
        {
            if (txt == null)
                return;

            Tasks = new ObservableCollection<TaskModel>();
            for (int i = 0; i < TasksToRecoverAfterFilter.Count; i++)
            {
                TaskModel t = TasksToRecoverAfterFilter[i];
                if (t.Title.Contains(txt) || t.Desc.Contains(txt))
                {
                    Tasks.Add(t);
                }
            }
        }

        // <summary>
        /// undo filtering by text
        /// </summary>
        internal void UnFilterByText(string txt)
        {
            this.Tasks = TasksToRecoverAfterFilter;
        }

        /// <summary>
        /// rename this column
        /// </summary>
        /// <param name="userEmail">email of the user about to view these tasks</param>
        /// <param name="creatorEmail">email of the creator of this board</param>
        /// /// <param name="boardName">name of the board</param>
        /// <param name="newColumnName">columns new name</param>
        internal void RenameColumn(string userEmail, string creatorEmail, string boardName, string newColumnName)
        {
            BC.RenameColumn(userEmail, creatorEmail, boardName, Ordinal, newColumnName);
        }

        /// <summary>
        /// shift this column location to a different one
        /// </summary>
        /// <param name="s">shift by s</param>
        internal void shift(int s)
        {
            this.Ordinal = s;
        }
    }

}
