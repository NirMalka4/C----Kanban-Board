using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Frontend.Model
{
    class BoardModel:NotifiableObject
    {
        public int Capacity { get; set; }

        private ObservableCollection<ColumnModel> cols;
        public ObservableCollection<ColumnModel> Cols
        {
            get => cols;
            set
            {
                cols = value;
                RaisePropertyChanged("Cols");
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set { name = value; }
        }

        private string creatorEmail;
        public string CreatorEmail
        {
            get => creatorEmail;
            set { creatorEmail = value; }
        }

        private string userEmail;
        public string UserEmail
        {
            get => userEmail;
            set { userEmail = value; }
        }

        private BackendController bc;
        public BackendController BC
        {
            get => bc;
            set { bc = value; }
        }

        internal BoardModel(string userEmail, string creatorEmail, string name, BackendController bc)
        {
            this.UserEmail = userEmail;
            this.CreatorEmail = creatorEmail;
            this.Name = name;
            this.BC = bc;
            this.Capacity = 0;
        }

        internal BoardModel(string userEmail, Board src, BackendController bc)
        {
            this.Name = src.Name;
            this.CreatorEmail = src.CreatorEmail;
            this.BC = bc;
            this.UserEmail = userEmail;
            this.Capacity = src.Capacity;
        }

        /// <summary>
        /// gets all the columns of the board and loads it into Cols
        /// </summary>
        internal void LoadBoard()
        {
            Cols = new ObservableCollection<ColumnModel>();
            for(int i = 0; i < Capacity; i++)
            {
                Cols.Add(BC.GetColumn(UserEmail, CreatorEmail, Name, i));
                Cols[i].PaintTasks(UserEmail);
            }
        }

        public string toString()
        {
            return Name;
        }

        /// <summary>
        /// advance task the next column
        /// </summary>
        /// <param name="t">the task to advance</param>
        /// <param name="col">the column contains the task at the moment</param>
        public void AdvanceTask(TaskModel t, ColumnModel col)
        {
            BC.AdvanceTask(UserEmail, CreatorEmail, Name, col.Ordinal, t.ID);
            col.RemoveTask(t);
            cols[col.Ordinal + 1].AdvancedTask(t);
        }

        /// <summary>
        /// update tasks title
        /// </summary>
        /// <param name="newTitlet">the new title</param>
        /// <param name="t">the task to update</param>
        /// <param name="ordinal">the column ordianal of the column that contains the task at the moment</param>
        public void UpdateTaskTitle(string newTitle, TaskModel t, int ordinal)
        {
            BC.UpdateTaskTitle(UserEmail, CreatorEmail, Name, ordinal, t.ID, newTitle);
            t.Title = newTitle;
        }

        /// <summary>
        /// update tasks description
        /// </summary>
        /// <param name="newDesc">the new description</param>
        /// <param name="t">the task to update</param>
        /// <param name="ordinal">the column ordianal of the column that contains the task at the moment</param>
        public void UpdateTaskDesc(string newDesc, TaskModel t, int ordinal)
        {
            BC.UpdateTaskDesc(UserEmail, CreatorEmail, Name, ordinal, t.ID, newDesc);
            t.Desc = newDesc;
        }

        /// <summary>
        /// update tasks DueDate
        /// </summary>
        /// <param name="newDateTime">the new DueDate</param>
        /// <param name="t">the task to update</param>
        /// <param name="ordinal">the column ordianal of the column that contains the task at the moment</param>
        public void UpdateTaskDueDate(DateTime newDateTime, TaskModel t, int ordinal)
        {
            BC.UpdateTaskDueDate(UserEmail, CreatorEmail, Name, ordinal, t.ID, newDateTime);
            t.DueDate = newDateTime;
        }

        /// <summary>
        /// update tasks assignee
        /// </summary>
        /// <param name="newAssignee">the new assignee</param>
        /// <param name="t">the task to update</param>
        /// <param name="ordinal">the column ordianal of the column that contains the task at the moment</param>
        public void UpdateAssignee(string newAssignee, TaskModel t, int ordinal)
        {
            BC.AssignTask(UserEmail, CreatorEmail, Name, ordinal, t.ID, newAssignee);
            t.EmailAssignee = newAssignee;
        }

        /// <summary>
        /// add column to current board
        /// </summary>
        /// <param name="ordinal">new columns ordinal to be added at</param>
        /// <param name="ColName">the new column name</param>
        internal void AddColumn(int ordinal, string ColName)
        {
            BC.AddColumn(UserEmail, CreatorEmail, Name, ordinal, ColName);
            Capacity++;
            LoadBoard();
        }

        /// <summary>
        /// remove a column from current board
        /// </summary>
        /// <param name="ordinal">columns ordinal to be removed</param>
        internal void RemoveColumn(int ordinal)
        {
            BC.RemoveColumn(UserEmail, CreatorEmail, Name, ordinal);
            foreach (ColumnModel c in Cols)
                if (c.Ordinal > ordinal)
                    c.Ordinal -= 1;

            Capacity--;
            Cols.Remove(Cols[ordinal]);
            RaisePropertyChanged("Cols");
        }

        /// <summary>
        /// shift a column
        /// </summary>
        /// <param name="ordinal">columns ordinal to be shifted</param>
        /// <param name="shift">how many steps to shift the column</param>
        internal void ShiftColumn(int ordinal, int shift)
        {
            BC.MoveColumn(UserEmail, CreatorEmail, Name, ordinal, shift);
            Cols[ordinal].Ordinal = ordinal + shift;
            Cols[ordinal + shift].Ordinal = ordinal;
            ColumnModel tmp = Cols[ordinal + shift];
            Cols[ordinal + shift] = Cols[ordinal];
            Cols[ordinal] = tmp;

            RaisePropertyChanged("Cols");
        }

        /// <summary>
        /// shift a column by -1
        /// </summary>
        /// <param name="ordinal">columns ordinal to be shifted</param>
        internal void MoveColumnUp(int ordinal)
        {
            ShiftColumn(ordinal, -1);
        }

        /// <summary>
        /// shift a column by 1
        /// </summary>
        /// <param name="ordinal">columns ordinal to be shifted</param>
        internal void MoveColumnDown(int ordinal)
        {
            ShiftColumn(ordinal, 1);
        }

        /// <summary>
        /// rename a column in this board
        /// </summary>
        /// <param name="ordinal">the ordinal of the column to be renamed</param>
        /// <param name="newColumnName">new columns name</param>
        internal void RenameColumn(int ordinal, string newColumnName)
        {
            BC.RenameColumn(UserEmail, CreatorEmail, Name, ordinal, newColumnName);
            Cols[ordinal].Name = newColumnName;
        }

        /// <summary>
        /// add a task to a column in this board
        /// </summary>
        /// <param name="col">the column to add the new task to</param>
        /// <param name="title">title of new task</param>
        /// <param name="desc">description of new task</param>
        /// <param name="dueDate">duedate of new task</param>
        internal void AddTask(ColumnModel col, string title, string desc, DateTime dueDate)
        {
            col.AddTask(userEmail, creatorEmail, Name, title, desc, dueDate);
        }

        
    }
}
