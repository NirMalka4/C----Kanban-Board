using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class TaskViewModel
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public UserModel Assignee { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreationDate { get; }

        private BoardModel BoardM { get; set; }
        private BackendController BC { get; set; }
        public TaskModel TaskM { get; set; }
        private UserModel UserM { get; set; }

        public TaskViewModel(TaskModel t)
        {
            this.TaskM = t;
        }
    }
}
