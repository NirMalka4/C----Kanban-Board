using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frontend.Model
{
    class TaskModel
    {
        internal const string MyTask = "BLUE";
        internal const string OverdueTask = "RED";
        internal const string CloseToDueDate = "ORANGE";

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
            }
        }

        public int ID { get; private set; }

        private string desc;
        public string Desc
        {
            get => desc;
            set
            {
                desc = value;
            }
        }

        private DateTime dueDate;
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
            }
        }

        private DateTime creationDate;
        public DateTime CreationDate
        {
            get => creationDate;
            set
            {
                creationDate = value;
            }
        }

        private BackendController BC { get; set; }

        public string EmailAssignee { get; set; }

        public TaskModel(Task src, BackendController bc)
        {
            this.BC = bc;
            this.Title = src.Title;
            this.Desc = src.Description;
            this.DueDate = src.DueDate;
            this.CreationDate = src.CreationTime;
            this.ID = src.Id;
            this.EmailAssignee = src.emailAssignee;
        }


        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }

        /// <summary>
        /// color task by its date
        /// </summary>
        internal void ColorByDate()
        {
            double timePassedInPercent = (100 * (DateTime.Now - creationDate)) / (DueDate - CreationDate);
            if (timePassedInPercent >= 100)
                this.BackgroundColor = OverdueTask;
            else if (timePassedInPercent > 75)
                this.BackgroundColor = CloseToDueDate;
        }
    }
}
