using System;
using System.Collections.Generic;
using System.Linq;
using IntroSE.Kanban.Backend.DataLayer.DTOs;

namespace IntroSE.Kanban.Backend.Business_Layer.Board
{
    internal class Board
    {
        //Defualt setting
        private readonly string[] columnNames = { "backlog", "in progress", "done" };
        private const int DefaultCapacity = 3, DefaultLeftColumn = 0, DefaultRightColumn = 2;

        /* Fields */
        private readonly string _CreatorEmail;
        private readonly string _BoardName;
        internal readonly Dictionary<int, Column> Columns;
        private readonly HashSet<string> _Members;
        private readonly DBoard MyDalObj;
        private volatile int _NextKey; /* next avaliable task key. */
        private int _Capacity; /* enumerate number of columns a board currently has.*/
        private volatile int _ColumnIndexer; /* enumerate total columns inserted. */
        private int leftMostColumn = DefaultLeftColumn, rightMostColumn; /* Column ordinal will be in the range [leftMostColumn, rightMostColumn]*/


        /* Create an instance with identified fields only: _CreatorEmail and _BoardName.
        * This constructor used by BoardController function 'GetBoardRef', to return a reference of existing instace.*/

        internal Board(string creator, string name, bool isDummyObj)
        {
            if (isDummyObj)
            {
                _CreatorEmail = creator;
                _BoardName = name;
            }
        }

        /* Create an instance and insert compatibale record to Boards table at DB. */
        internal Board(string creator, string name)
        {
            _CreatorEmail = creator;
            _BoardName = name;
            Columns = new Dictionary<int, Column>();
            _Members = new HashSet<string>();
            _Capacity = DefaultCapacity;
            _ColumnIndexer = DefaultCapacity;
            rightMostColumn = DefaultRightColumn;
            _NextKey = 1;
            MyDalObj = new DBoard(creator, name, _Capacity, _NextKey, _ColumnIndexer, rightMostColumn);
            Init();
        }

        ///<summary> 
        /// Initalize default columns without load previous data from DB. 
        ///</summary>
        private void Init()
        {
            int i = leftMostColumn;
            foreach (string name in columnNames)
            {
                Columns.Add(i, new Column(name, i, _CreatorEmail, _BoardName, i));
                i++;
            }
            MyDalObj.Insert();
        }

        /* Create an instance based on record from the DB.  */
        public Board(DBoard src)
        {
            _CreatorEmail = src.CreatorEmail;
            _BoardName = src.BoardName;
            Columns = new Dictionary<int, Column>();
            _Members = new HashSet<string>();
            MyDalObj = src;
            rightMostColumn = src.Right;
            _NextKey = MyDalObj.NextKey;
            _Capacity = MyDalObj.Capacity;
            _ColumnIndexer = MyDalObj.ColumnIndexer;
            InitFromDB();
        }

        ///<summary> 
        /// Initalize Columns and Members from DB.
        ///</summary>
        private void InitFromDB()
        {
            foreach (DColumn p in MyDalObj.SelectColumns())
                Columns.Add(p.Ordinal, new Column(p));
            foreach (DMember p in MyDalObj.SelectMembers())
                _Members.Add(p.UserEmail);
        }

        ///<summary> 
        ///Verify param ordinal is within the range [leftMostColumn, rightMostColumn] 
        ///</summary>
        private void ValidateColumnWithinBounds(int ordinal)
        {
            if (ordinal < leftMostColumn | ordinal > rightMostColumn)
                throw new ArgumentException($" Ordinal: {ordinal} exceed board: {_BoardName} of user: {_CreatorEmail} limits");
        }

        ///<summary> 
        /// Return a reference to a column at param ordinal
        ///</summary>
        private Column GetColumnRef(int ordinal)
        {
            ValidateColumnWithinBounds(ordinal);
            Column actualValue;
            if (!Columns.TryGetValue(ordinal, out actualValue))
                throw new ArgumentException($"There is no column of ordinal: {ordinal} in board: {_BoardName} of user: {_CreatorEmail}");
            return actualValue;
        }

        ///<summary> 
        ///Determine whether user is a member of this board. 
        ///</summary>
        private void IsMember(string userEmail)
        {
            if (userEmail is null || !(userEmail.Equals(_CreatorEmail) || _Members.Contains(userEmail)))
                throw new ArgumentException($"User: {userEmail} is not a member at board: {_BoardName}.");
        }

        ///<summary> 
        ///Determine whether user is the creator of this board. 
        ///</summary>
        private void IsCreator(string userEmail)
        {
            if (userEmail is null || !userEmail.Equals(_CreatorEmail))
                throw new ArgumentException("Only board creator can remove his board");
        }

        ///<summary> 
        /// Determine if column identified by param ordinal contains in progress tasks.
        ///</summary>
        private bool IsInProgressColumn(int ordinal) => ordinal != leftMostColumn & ordinal != rightMostColumn;

        ///<summary> 
        /// Return all tasks of specific user whom member at this board, exclude the righ most column.
        ///</summary>
        internal IList<BTask> GetInProgressTask(string userEmail)
        {
            IsMember(userEmail);
            IList<BTask> output = new List<BTask>();
            foreach (Column c in Columns.Values)
                output = IsInProgressColumn(c.Ordinal) ? output.Concat(c.GetInProgressTask(userEmail)).ToList() : output;
            return output;
        }

        ///<summary>
        ///Set param limit as new capacity of param columnOrdinal.
        ///</summary>
        internal void LimitColumn(int columnOrdinal, int limit) => GetColumnRef(columnOrdinal).Limit = limit;

        /// <summary> 
        /// Move task from specific column to its successor column.
        /// </summary>
        internal void AdvanceTask(string userEmail, int columnOrdinal, int advanceMe)
        {
            IsDone(columnOrdinal, advanceMe);
            Column current = GetColumnRef(columnOrdinal);
            BTask item = current.GetTask(userEmail, advanceMe);
            Column successor = GetColumnRef(columnOrdinal + 1);
            successor.AddTask(userEmail, item, advanceMe);
            current.RemoveTask(userEmail, advanceMe);
        }

        ///<summary>
        /// Delete all Columns and Members data from DB.
        /// Return boards members.
        ///</summary>
        private HashSet<string> Delete()
        {
            foreach (Column col in Columns.Values)
                col.Delete();
            foreach (DMember member in MyDalObj.SelectMembers())
                member.Delete();
            Columns.Clear();
            MyDalObj.Delete();
            return _Members;
        }

        ///<summary>
        ///Delete all Columns and Members data from DB. 
        ///return board members.
        ///.</summary>
        /// <param name="userEmail"> Must be board creator email. </param>

        internal HashSet<string> Remove(string userEmail)
        {
            IsCreator(userEmail);
            return Delete();
        }

        ///<summary> 
        ///Add Task to the leftmost column.
        ///</summary>
        internal BTask AddTask(string assignee, string title, string description, DateTime dueDate)
        {
            IsMember(assignee);
            Column leftmost = GetColumnRef(leftMostColumn);
            BTask output = leftmost.AddTask(assignee, title, description, dueDate, NextKey, _CreatorEmail, _BoardName);
            NextKey = _NextKey + 1;
            return output;
        }


        ///<summary> 
        /// Validate if tasks is done: it belongs to the right most column.
        ///</summary>
        private void IsDone(int ordinal, int id)
        {
            if (ordinal == rightMostColumn)
                throw new ArgumentException($"Task: {id} is done. Thus is can not be further chagned.");
        }

        ///<summary> 
        ///Update task due date.
        ///</summary>
        internal void Update(string userEmail, int columnOrdinal, int taskID, DateTime dueDate)
        {
            IsDone(columnOrdinal, taskID);
            GetColumnRef(columnOrdinal).Update(userEmail, taskID, dueDate);
        }
            

        ///<summary> 
        ///Update task description. 
        ///</summary>
        internal void Update(string userEmail, int columnOrdinal, int taskID, string description)
        {
            IsDone(columnOrdinal, taskID);
            GetColumnRef(columnOrdinal).Update(userEmail, taskID, description);
        }
        

        ///<summary>
        ///Update task title. 
        ///</summary>
        internal void UpdateT(string userEmail, int columnOrdinal, int taskID, string title)
        {
            IsDone(columnOrdinal, taskID);
            GetColumnRef(columnOrdinal).UpdateT(userEmail, taskID, title);
        }

        ///<summary>
        ///Add user to board members. Update DB. 
        ///</summary>
        internal void AddMember(string joinMe)
        {
            _Members.Add(joinMe);
            new DMember(_CreatorEmail, _BoardName, joinMe).Insert();
        }

        ///<summary> 
        /// Assign task to a member
        ///</summary>
        internal void AssignTask(int columnOrdinal, int taskID, string emailAssignee)
        {
            IsMember(emailAssignee);
            GetColumnRef(columnOrdinal).AssignTask(taskID, emailAssignee);
        }

        ///<summary> 
        /// Add column to specific board.
        /// Work as follows:
        /// 1. Validate param columnOrdinal is within the range [leftMostColumn, righMostColumn].
        /// 2. Shift right any successor of column at param columnOrdinal.
        /// 3. Insert the new column.
        /// 4. Update necessary fields.
        ///</summary>

        internal void AddColumn(int columnOrdinal, string columnName)
        {
            ValidateColumnWithinBounds(columnOrdinal);
            for(int i = rightMostColumn; i>= columnOrdinal; i--)
            {
                Column curr = GetColumnRef(i);
                curr.Ordinal = i + 1;
                Columns.Remove(i);
                Columns.Add(i+1, curr);
            }
            Columns.Add(columnOrdinal, new Column(columnName, columnOrdinal, _CreatorEmail, _BoardName, _ColumnIndexer));
            RightMostColumn = rightMostColumn + 1;
            Capacity = _Capacity + 1;
            ColumnIndexer = _ColumnIndexer + 1;
        }

        ///<summary> 
        /// Validate param columOrdinal is within the range [leftMostColumn, rightMostColumn] and _Capacity>2
        ///</summary>
        private void ColumnIsRemovable(int columnOrdinal)
        {
            ValidateColumnWithinBounds(columnOrdinal);
            if (_Capacity <= 2)
                throw new ArgumentException($"Can not delete column: {columnOrdinal} as board must contain at least 2 columns." +
                    $"currently has: {_Capacity}");
        }

        ///<summary> 
        /// Remove column of specific board.
        /// Work as follows:
        /// 1. Validate remove operation is legal.
        /// 2. Find the removed column.
        /// 3. Find the required receiver.
        /// 4. Transfer all removed column tasks to the receiver.
        /// 5. Delete the required column.
        /// 6. Decrease by one each column with ordinal>=columnOrdinal+1
        ///</summary>
        internal void RemoveColumn(int columnOrdinal)
        {
            ColumnIsRemovable(columnOrdinal);
            Column removed = GetColumnRef(columnOrdinal), receiver;
            /* if removed is the left most column, set receiver as the column successor. 
             * otherwise, set it as removed predecessor. i.e, column on its left */
            receiver = columnOrdinal == leftMostColumn ? GetColumnRef(columnOrdinal + 1) : GetColumnRef(columnOrdinal - 1);
            receiver.ReceiveTransfer(removed.TransferTasks(), removed.CurrentCapacity);
            Columns.Remove(columnOrdinal);
            for(int i = columnOrdinal + 1; i<=rightMostColumn; i++)
            {
                Column curr = GetColumnRef(i);
                curr.Ordinal = i - 1;
                Columns.Remove(i);
                Columns.Add(i-1, curr);
            }
            RightMostColumn = rightMostColumn - 1;
            Capacity = _Capacity-1;
        }

        ///<summary> 
        /// Set new name to specific column.
        ///</summary>
        internal void RenameColumn(int columnOrdinal, string name) =>
            GetColumnRef(columnOrdinal).Name = name;



        ///<summary> 
        /// Move column param shifSize times, relativly to its current location.
        /// Work as follows:
        /// 1. Validate param columnOrdinal+shiftsize is within bounds
        /// 2. Let shifted be the required column.
        /// 3. Validate shifted eligible for moving.
        /// 4. Let i a shifted ordinal value, i.e shiftMe+shiftSize.
        /// 5. Insert replaced to the dictionary at key i. if alredy exists a column there, replace them.
        /// 6. Update replaced ordinal.
        ///</summary>
        internal void MoveColumn(int shiftMe, int shiftSize)
        {
            ValidateColumnWithinBounds(shiftMe + shiftSize);
            Column shifted = GetColumnRef(shiftMe);
            shifted.IsMovable();
            Column replaced = null;
            int i = shiftMe + shiftSize;
            try
            {
                replaced = GetColumnRef(i); 
                Columns.Remove(i);
                replaced.Ordinal = shiftMe;
            }
            catch (Exception) { }// There is no column at ordinal i
            finally
            {
                Columns.Remove(shiftMe);
                shifted.Ordinal = i;
                Columns[i] = shifted;
                if (replaced is not null)
                    Columns[shiftMe] = replaced;
            }
        }

        internal string Name
        {
            get => _BoardName;
        }

        internal string Creator
        {
            get => _CreatorEmail;
        }

        internal IList<string> Members
        {
            get => _Members.ToList();
        }

        internal int Capacity
        {
            get => _Capacity;
            private set
            {
                _Capacity = value;
                MyDalObj.Capacity = value;
            }
        }

        internal int ColumnIndexer
        {
            get => _ColumnIndexer;
            private set
            {
                _ColumnIndexer = value;
                MyDalObj.ColumnIndexer = value;
            }
        }

        internal int NextKey
        {
            get => _NextKey;
            set
            {
                _NextKey = value;
                MyDalObj.NextKey = value;
            }
        }

        internal int RightMostColumn
        {
            get => rightMostColumn;
            private set
            {
                int oldVal = rightMostColumn;
                rightMostColumn = value;
                if(value != oldVal)
                    MyDalObj.Right = value;
            }
        }


        ///<summary> 
        /// Util functions used to inititate compatible SL column object. 
        ///</summary>
        public Column GetFullColumn(int columnOrdinal) => GetColumnRef(columnOrdinal);

        public override string ToString() => $"Creator:{ _CreatorEmail} Name:{_BoardName}.";
        public override bool Equals(Object other) =>
            other is Board && _CreatorEmail.Equals(((Board)other)._CreatorEmail) && _BoardName.Equals(((Board)other)._BoardName);
        public override int GetHashCode() => _CreatorEmail.GetHashCode() ^ _BoardName.GetHashCode();
    }

}
