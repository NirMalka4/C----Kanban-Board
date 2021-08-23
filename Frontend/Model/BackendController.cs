using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = IntroSE.Kanban.Backend.ServiceLayer.Objects.Task;

namespace Frontend.Model
{
    class BackendController
    {
        Service s;

        public BackendController()
        {
            s = new Service();
            s.LoadData();
            //s.DeleteData();
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="email">the email which the user wants to register with</param>
        /// <param name="password">the password which the user wants to register with</param>
        /// <returns>throws an exception ic case the email is taken of the password is illegal</returns>
        public void Register(string email, string password)
        {
            Response res = s.Register(email, password);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// login
        /// </summary>
        /// <param name="email">the email which the user wants to login with</param>
        /// <param name="password">the password suppose to correspond to the email</param>
        /// <returns>throws an exception ic case the email is invalid or the password is not correct</returns>
        public UserModel Login(string email, string password)
        {
            Response<User> res = s.Login(email, password);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);

            return new UserModel(email, this);
        }

        /// <summary>
        /// logout
        /// </summary>
        /// <param name="email">the email which the user wants to logout with</param>
        /// <returns>throws an exception ic case the email is invalid</returns>
        public void Logout(string email)
        {
            s.Logout(email);
        }

        /// <summary>
        /// gets list of Board objexts that user is member of
        /// </summary>
        /// <param name="email">the email that appears as a member in the boards</param>
        /// <returns>list of Board objects</returns>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public IList<Board> GetMyBoards(string email)
        {
            Response<IList<Board>> res = s.GetMyBoards(email); 
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);

            return res.Value;
        }

        /// <summary>
        /// add a new board
        /// </summary>
        /// <param name="email">the email of the user creating a new boards</param>
        /// <param name="bName">the name for the new board</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public void AddBoard(string email, string bName)
        {
            Response res = s.AddBoard(email, bName);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// Remove an exsisting board created by logged in user
        /// </summary>
        /// <param name="userEmail">the email of the user created the boards</param>
        /// <param name="bName">the name of the board to delete</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public void RemoveBoard(string userEmail, string bName)
        {
            Response res = s.RemoveBoard(userEmail, userEmail, bName);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// gets column
        /// </summary>
        /// <param name="userEmail">the email of the requesting the column</param>
        /// <param name="creatorEmail">the email of the user creatwed the board in wich the column in</param>
        /// <param name="boardName">the name of the board wich the column is in</param>
        /// <param name="columnOrdinal">columns ordinal</param>
        /// <returns>Column object</returns>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public ColumnModel GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            Response<Column> res = s.GetFullColumn(userEmail, creatorEmail, boardName, columnOrdinal);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);

            return new ColumnModel(res.Value, this);
        }

        /// <summary>
        /// add a enw task
        /// </summary>
        /// <param name="userEmail">the email of the user adding</param>
        /// <param name="creatorEmail">the email of the user creatwed the board to wich the user wants to add new task</param>
        /// <param name="boardName">the name of the board which the new task will be added to</param>
        /// <param name="title">new tasks title</param>
        /// <param name="description">new tasks description</param>
        /// <param name="dueDate">new tasks duedate</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public Task AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            Response<Task> res = s.AddTask(userEmail, creatorEmail, boardName, title, description, dueDate);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);

            return res.Value;
        }

        /// <summary>
        /// advance a task to the next column
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board in which the operation will happend</param>
        /// <param name="columnOrdinal">the tasks column ordinal</param>
        /// <param name="taskId">tasks id</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            Response res = s.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// update tasks title
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board in which the operation will happend</param>
        /// <param name="columnOrdinal">the tasks column ordinal</param>
        /// <param name="taskId">tasks id</param>
        /// <param name="newTitle">tasks new title</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string newTitle)
        {
            Response res = s.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, newTitle);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// update tasks title
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board in which the operation will happend</param>
        /// <param name="columnOrdinal">the tasks column ordinal</param>
        /// <param name="taskId">tasks id</param>
        /// <param name="newDesc">tasks new description</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public void UpdateTaskDesc(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string newDesc)
        {
            Response res = s.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, newDesc);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// update tasks DueDate
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board in which the operation will happend</param>
        /// <param name="columnOrdinal">the tasks column ordinal</param>
        /// <param name="taskId">tasks id</param>
        /// <param name="newdd">tasks new DueDate</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime newdd)
        {
            Response res = s.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, newdd);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// update tasks assignee
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board in which the operation will happend</param>
        /// <param name="columnOrdinal">the tasks column ordinal</param>
        /// <param name="taskId">tasks id</param>
        /// <param name="emailAssignee">tasks new assigneees email</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response res = s.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// join an exsisting board
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board that a user wants to join as a member</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        public void JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            Response res = s.JoinBoard(userEmail, creatorEmail, boardName);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// add a new column
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board to where user wants to add a new column</param>
        /// <param name="columnOrdinal">the ordinal for the new column</param>
        /// <param name="columnName">the name of the new column</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        internal void AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            Response res =  s.AddColumn(userEmail, creatorEmail, boardName, columnOrdinal, columnName);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// Remove column
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board to where user wants to remove a column</param>
        /// <param name="columnOrdinal">the ordinal of the column to remove</param>
        /// <exceptions>throws an exception in case the the operation faild</exceptions>
        internal void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            Response res = s.RemoveColumn(userEmail, creatorEmail, boardName, columnOrdinal);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// shift column location in columns order
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board where the operation wioll happend</param>
        /// <param name="columnOrdinal">the ordinal for the column to shift</param>
        /// <param name="shiftSize">number by howm many stepsd to shift the column</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        internal void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            Response res = s.MoveColumn(userEmail, creatorEmail, boardName, columnOrdinal, shiftSize);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// rename an exsisting column
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        /// <param name="creatorEmail">the email of the user created the board in which the operation will happend</param>
        /// <param name="boardName">the name of the board to where user wants to add a new column</param>
        /// <param name="columnOrdinal">the ordinal for the column</param>
        /// <param name="newColumnName">the new name for the column</param>
        /// <exceptions>throws an exception ic case the the operation faild</exceptions>
        internal void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            Response res = s.RenameColumn(userEmail, creatorEmail, boardName, columnOrdinal, newColumnName);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// get users inprogresstasks
        /// </summary>
        /// <param name="userEmail">the email of the user requesting</param>
        ///<returns>dictionary keys are the name of the creator and the board and value is the tasks</returns>
        /// <exceptions>throws an exception in case the the operation faild</exceptions>
        internal Dictionary<Tuple<string, string>, Task> GetMyInProgressTasks(string userEmail)
        {
            Response<Dictionary<Tuple<string, string>, Task>> res = s.GetMyInProgressTasks(userEmail);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);

            return res.Value;
        }
    }
}
