using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.Business_Layer.Board;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using Column = IntroSE.Kanban.Backend.ServiceLayer.Objects.Column;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class BoardService
    {
        private readonly BoardController BC; /* Board Controller  */

        internal BoardService() { BC = new BoardController(); }

        ///<summary> 
        ///Loads the data from the persistance. 
        ///</summary>
        internal void LoadData() => BC.LoadData(); 

        ///<summary> 
        ///Removes all persistent data.
        ///</summary>
        internal void DeleteData() => BC.DeleteData(); 

        ///<summary>
        ///Limit the number of tasks in a specific column.
        ///</summary>
        internal void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
            => BC.LimitColumn(email, boardName, columnOrdinal, limit);

        ///<summary> 
        ///Add task to specific board. Return serive layer Task object. 
        ///</summary>
        internal Task AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
            => new Task(BC.AddTask(userEmail, creatorEmail, boardName, title, description, dueDate));

        //<summary> 
        ///Update task due date.
        ///</summary>
        internal void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, DateTime dueDate)
            =>BC.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskID, dueDate);

        ///<summary> 
        ///Update task title.
        ///</summary>
        internal void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, string title)
            => BC.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskID, title);

        ///<summary> 
        ///Update task description.
        ///</summary>
        internal void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, string description)
            =>BC.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskID, description);

        ///<summary> 
        ///Advance task to adjacent (right neighbor) column. 
        ///</summary>
        internal void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int advanceMe)
            => BC.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, advanceMe);

        ///<summary>
        ///Add new board with the supplied params.
        ///</summary>
        internal void AddBoard(string creatorEmail, string boardName)
            => BC.AddBoard(creatorEmail, boardName);

        ///<summary>
        ///Adds a board created by another user to the logged-in user.
        ///</summary>
        internal void JoinBoard(string joinMe, string creatorEmail, string boardName)
            => BC.JoinBoard(joinMe, creatorEmail, boardName);

        ///<summary> 
        ///Removes a board. 
        ///</summary>
        internal void Remove(string userEmail, string creatorEmail, string boardName)
            => BC.Remove(userEmail, creatorEmail, boardName);

        ///<summary> 
        ///Get all in progress tasks of a user whom he assigned to. 
        ///</summary>
        internal Column GetInProgressTask(string userEmail)
        {
            IList<Task> output = new List<Task>();
            foreach (BTask item in BC.GetInProgressTask(userEmail))
                output.Add(new Task(item));

            return new Column("‘in progress", (UInt32)output.Count, -1, 1, output);

        }

        ///<summary> 
        ///Assigns a task to a user.
        ///</summary>
        internal void AssignTask(string creatorEmail, string boardName, int columnOrdinal, int taskID, string emailAssignee)
            => BC.AssignTask(creatorEmail, boardName, columnOrdinal, taskID, emailAssignee);

        ///<summary> 
        ///Get all boards name of a user whom he is a member. 
        ///</summary>
        internal IList<string> GetBoardNames(string member)
            => BC.GetBoardNames(member);

        ///<summary> 
        ///Get service Column.
        ///</summary>
        internal Column GetFullColumn(string creatorEmail, string boardName, int columnOrdinal)
            => new Column(BC.GetFullColumn(creatorEmail, boardName, columnOrdinal));

        /// <summary>
        /// Adds a new column
        /// </summary>
        internal void AddColumn(string creatorEmail, string boardName, int columnOrdinal, string columnName) =>
           BC.AddColumn(creatorEmail, boardName, columnOrdinal, columnName);

        /// <summary>
        /// Removes a specific column
        /// </summary>
        internal void RemoveColumn(string creatorEmail, string boardName, int columnOrdinal) =>
            BC.RemoveColumn(creatorEmail, boardName, columnOrdinal);

        /// <summary>
        /// Renames a specific column
        /// </summary>
        internal void RenameColumn(string creatorEmail, string boardName, int columnOrdinal, string newColumnName) =>
            BC.RenameColumn(creatorEmail, boardName, columnOrdinal, newColumnName);

        /// <summary>
        /// Moves a column shiftSize times relativly to its current location.
        /// </summary>
        internal void MoveColumn(string creatorEmail, string boardName, int columnOrdinal, int shiftSize) =>
            BC.MoveColumn(creatorEmail, boardName, columnOrdinal, shiftSize);


        ///<summary> 
        ///Get all in progress tasks of a specific user in each Board.
        ///</summary>
        internal Dictionary<Tuple<string, string>, Task> InProgressTask(string userEmail)
        {
            var v = BC.InProgressTask(userEmail);
            var output = new Dictionary<Tuple<string, string>, Task>();
            // convert each BTask to SL Task object.
            foreach (var p in v)
            {
                var tasksLst = new List<Task>();
                foreach (BTask t in p.Value)
                    output.Add(p.Key, new Task(t));
            }
            return output;
        }

        internal IList<Objects.Board> GetMyBoards(string member)
        {
            IList<Business_Layer.Board.Board> boards = BC.GetMyBoards(member);
            IList<Objects.Board> sBoards = new List<Objects.Board>();

            foreach (Business_Layer.Board.Board b in boards)
            {
                sBoards.Add(new Objects.Board(b));
            }

            return sBoards;
        }
    }

}
