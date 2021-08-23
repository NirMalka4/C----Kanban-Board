using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DAOs;
using IntroSE.Kanban.Backend.DataLayer.DTOs;
using System.Data.SQLite;


namespace IntroSE.Kanban.Backend.Business_Layer.Board
{
    internal class BoardController
    {
        
         /* Attach to each User a reference to all Boards whom associate with (member or creator).
         * Board is identified injecitvly via pair <CreatorEmail, BoardName>.*/

        private readonly Dictionary<string, HashSet<Board>> Boards = new Dictionary<string, HashSet<Board>>();

        /* Flag which indicates whether data from DB was loaded */ 
        private bool Loaded = false;

        private readonly DTO MyDalObject = new DBoard();
        public BoardController() {}

        ///<summary> 
        /// if a Board identified by the arguments is already exist ,return a reference. 
        /// else, return null.
        /// .</summary>
        private Board GetBoardRef(string creatorEmail, string boardName)
        {
            ValidateParms(creatorEmail, creatorEmail);
            if (!Boards.ContainsKey(creatorEmail))
                throw new ArgumentException($"User: {creatorEmail} has no boards.");
            Board equalValue = new Board(creatorEmail, boardName, true), actualValue;
            Boards[creatorEmail].TryGetValue(equalValue, out actualValue);
            if(actualValue is null)
                throw new ArgumentException($"Board: {boardName}, associated with Email: {creatorEmail} does not exists");
            return actualValue;
        }

        ///<summary> 
        ///Verify whether a board with same key value is already exsits.
        ///</summary>
        private void ContainsBoard(string userEmail, string boardName)
        {
            if(Boards.ContainsKey(userEmail))
            {
                try
                {
                    GetBoardRef(userEmail, boardName);
                    throw new ArgumentException($"Board: {boardName}, associated with Email: {userEmail} is already exists");
                }
                catch (ArgumentException) { } // Hence, the user does not has a board with param boardName
            }
        }

        private void ValidateParms(string creatorEmail, string boardName )
        {
            if(String.IsNullOrEmpty(creatorEmail)) 
                throw new ArgumentException("Null or Empty string is illegal parameter for Email propery.");
            if(String.IsNullOrEmpty(boardName)) throw new ArgumentException("Board name should be non-empty string.");
        }

        ///<summary> 
        /// Add board reference to a specific user.
        ///</summary>
        private void MapBoard(string userEmail, in Board memberAt)
        {
            if (!Boards.ContainsKey(userEmail))
                Boards[userEmail] = new HashSet<Board>();
            Boards[userEmail].Add(memberAt);
        }

        ///<summary> 
        /// Add a reference to board from each of its members
        ///</summary>
        private void MapMembers(IList<string> members, Board reference)
        {
            foreach (string member in members)
                MapBoard(member, reference);
        }

        ///<summary> 
        ///Loads the data from the persistance layer.  
        ///</summary>
        public void LoadData()
        {
            foreach(DBoard src in MyDalObject.Select())
            {
                Board loadMe = new Board(src);
                MapBoard(src.CreatorEmail, loadMe);  /* add a reference to board creator */
                MapMembers(loadMe.Members, loadMe); /* add a reference to each board member */
            }
            Loaded = true;
        }

        ///<summary>
        /// if(Loaded) - Delete the data from either persistance layer and runtime program memory allocation.  
        /// else, delete all data associated with board component from the DB.
        /// </summary>
        public void DeleteData()
        {
            if (Loaded)
            {
                /* Delete all boards creatred by specific user (this user is the dictionary key) */
                foreach (KeyValuePair<string, HashSet<Board>> p in Boards)
                {
                    foreach (Board b in p.Value)
                    {
                        if(b.Creator.Equals(p.Key))
                            b.Remove(p.Key); 
                    }
                    p.Value.Clear();
                }
            }
            else
                MyDalObject.Clear();
            Boards.Clear();
        }

        ///<summary>
        /// Create a new board. 
        ///</summary>
        public void AddBoard(string creatorEmail, string boardName) 
        {
            ContainsBoard(creatorEmail, boardName);
            Board addMe = new Board(creatorEmail, boardName);
            MapBoard(creatorEmail, addMe);
        }

        ///<summary> 
        ///Limit the number of tasks in a specific column. 
        ///</summary>
        public void LimitColumn(string creatorEmail, string boardName, int columnOrdinal, int limit)
            => GetBoardRef(creatorEmail, boardName).LimitColumn(columnOrdinal, limit);

        ///<summary> 
        ///Get all in progress tasks of a user whom he assigned to. 
        ///</summary>
        public IList<BTask> GetInProgressTask(string userEmail)
        {
            IList<BTask> output = new List<BTask>();
            if(Boards.ContainsKey(userEmail))
            { 
                /* Iterate over all the user boards, concate the user tasks from each. */
                foreach (Board b in Boards[userEmail])
                    output = output.Concat(b.GetInProgressTask(userEmail)).ToList();
            }
            return output;
        }

        ///<summary> 
        ///Get all in progress tasks of a specific user in each Board.
        ///</summary>
        public Dictionary<Tuple<string, string>, IList<BTask>> InProgressTask(string userEmail)
        {

            var output = new Dictionary<Tuple<string, string>, IList<BTask>>();
            if (Boards.ContainsKey(userEmail))
            {
                /* Iterate over all the user boards, add user tasks from each. */
                foreach (Board b in Boards[userEmail])
                    output.Add(new Tuple<string, string>(b.Creator, b.Name), b.GetInProgressTask(userEmail));
            }
            return output;
        }

        ///<summary> 
        ///Get all boards name of a user whom he is a member. 
        ///</summary>
        public IList<string> GetBoardNames(string member)
        {
            IList<string> output = new List<string>();
            if(Boards.ContainsKey(member))
            {
                foreach (Board b in Boards[member])
                    output.Add(b.Name);
            }
            return output;
        }

        ///<summary> 
        /// Removes a board.
        /// Delete all references from the board members. 
        ///</summary>
        public void Remove(string userEmail, string creatorEmail, string boardName)
        {
            Board item = GetBoardRef(creatorEmail, boardName);
            /* iterate over board members, remove reference from each to it. */
            HashSet<string> boardMembers = item.Remove(userEmail);
            foreach (string Member in boardMembers) 
                Boards[Member].Remove(item);
            Boards[userEmail].Remove(item);
            boardMembers.Clear();
        }
        ///<summary> 
        ///Advance a task to the next column. 
        ///</summary>
        public void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int advanceMe) =>
            GetBoardRef(creatorEmail, boardName).AdvanceTask(userEmail, columnOrdinal, advanceMe);

        ///<summary> 
        ///Add a new Task. 
        ///</summary>
        public BTask AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
            => GetBoardRef(creatorEmail, boardName).AddTask(userEmail, title, description, dueDate);

        ///<summary> 
        ///Update the due date of a task. 
        ///</summary>
        public void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, DateTime dueDate)
            => GetBoardRef(creatorEmail, boardName).Update(userEmail, columnOrdinal, taskID, dueDate);

        ///<summary> 
        ///Update the description of a task 
        ///</summary>
        public void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, string description)
            => GetBoardRef(creatorEmail, boardName).Update(userEmail, columnOrdinal, taskID, description);

        ///<summary> 
        ///Update the title of a task 
        ///</summary>
        public void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskID, string title)
            => GetBoardRef(creatorEmail, boardName).UpdateT(userEmail, columnOrdinal, taskID, title);

        ///<summary> 
        /// Set user as new member of a specific board 
        ///</summary>
        public void JoinBoard(string joinMe, string creatorEmail, string boardName)
        {
            Board item = GetBoardRef(creatorEmail, boardName);
            item.AddMember(joinMe);
            MapBoard(joinMe, item);
        }

        ///<summary> 
        ///Assigns a task to a user 
        ///</summary>
        public void AssignTask(string creatorEmail, string boardName, int columnOrdinal, int taskID, string emailAssignee)
            => GetBoardRef(creatorEmail, boardName).AssignTask(columnOrdinal, taskID, emailAssignee);

        ///<summary> 
        ///Get column.
        ///</summary>
        public Column GetFullColumn(string creatorEmail, string boardName, int columnOrdinal)
            => GetBoardRef(creatorEmail, boardName).GetFullColumn(columnOrdinal);


        /// <summary>
        /// Adds a new column
        /// </summary>
        public void AddColumn(string creatorEmail, string boardName, int columnOrdinal, string columnName)
            => GetBoardRef(creatorEmail, boardName).AddColumn(columnOrdinal, columnName);

        /// <summary>
        /// Removes a specific column
        /// </summary>
        public void RemoveColumn(string creatorEmail, string boardName, int columnOrdinal)
            => GetBoardRef(creatorEmail, boardName).RemoveColumn(columnOrdinal);


        /// <summary>
        /// Renames a specific column
        /// </summary>
        public void RenameColumn(string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
            => GetBoardRef(creatorEmail, boardName).RenameColumn(columnOrdinal, newColumnName);

        /// <summary>
        /// Moves a column shiftSize times relativly to its current location.
        /// </summary>
        public void MoveColumn(string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
            => GetBoardRef(creatorEmail, boardName).MoveColumn(columnOrdinal, shiftSize);

        public override string ToString() => Boards.ToString();
        public override bool Equals(object obj) =>
            obj is BoardController && ((BoardController)obj).Boards.Equals(Boards);
        public override int GetHashCode() => Boards.GetHashCode();




        public IList<Board> GetMyBoards(string member)
        {
            IList<Board> output = new List<Board>();
            if (Boards.ContainsKey(member))
            {
                foreach (Board b in Boards[member])
                    output.Add(b);
            }
            return output;
        }

    }
}
