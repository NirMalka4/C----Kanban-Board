using System;
using System.Collections.Generic;
using System.Linq;
using IntroSE.Kanban.Backend.DataLayer.DTOs;
using IntroSE.Kanban.Backend.DataLayer.DAOs;
using System.Collections;

namespace IntroSE.Kanban.Backend.Business_Layer.Board
{
    internal class Column
    {
        // Magic Numbers
        private const short DEFAULT_COLUMN_LIMIT = -1;

        //Instance fields
        private string _Name;
        private int _Limit = -1;
        private int _Ordinal;
        private uint _CurrentCapacity;
        /* attach each user the tasks id whom assigned to in this column */
        internal Dictionary<string, IList<int>> _Mapper = new Dictionary<string, IList<int>>();
        /* map tasks upon their ID */
        internal Dictionary<int, BTask> _Tasks = new Dictionary<int, BTask>();
        DColumn MyDalObj;

        internal Column(string n, int o, string creator, string bname, int pos)
        {
            _Name = n;
            _Ordinal = o;
            _CurrentCapacity = 0;
            MyDalObj = new DColumn(creator, bname, o, _Name, _Limit, _CurrentCapacity, pos);
            MyDalObj.Insert();
        }

        ///<summary> 
        ///Create an instance basen upon DB data. 
        ///</summary>
        internal Column(DColumn src)
        {
            _Ordinal = src.Ordinal;
            _Name = src.Name;
            _CurrentCapacity = src.CurrentCapacity;
            MyDalObj = src;
            foreach (DTask item in MyDalObj.SelectTasks()) // get all tasks of this column
            {
                BTask loadMe = new BTask(item);
                _Tasks[item.ID] = loadMe;
                MapTask(item.Assignee, item.ID);
            }
        }

        ///<summary>
        /// Move content from other to this. Remove other afterwards.
        ///</summary>
        internal void Move(ref Column other)
        {
            Name = other.Name;
            Limit = other.Limit;
            CurrentCapacity = 0;
            ReceiveTransfer(other.TransferTasks(), other.CurrentCapacity);
        }

        ///<summary>
        ///Determine whether the new limit value is legal 
        ///</summary>
        /// <param name="newLimit"> Should be one of the above:.
        /// 1. Default column limit => -1
        /// 2. None negative integer and bigger than the current capacity.
        /// </param>

        private void ValidateLegalLimit(int newLimit)
        {
            if (newLimit != DEFAULT_COLUMN_LIMIT && (newLimit < 0 | newLimit < CurrentCapacity))
                throw new ArgumentException($"Column: {Name} has {_CurrentCapacity} tasks. Thus, " +
                    $"it limit can not be reduced to {newLimit}");
        }

        ///<summary> 
        /// Attach task (id only) to specific user.
        ///</summary>
        private void MapTask(string userEmail, int taskId)
        {
            if (!_Mapper.ContainsKey(userEmail))
                _Mapper[userEmail] = new List<int>();
            _Mapper[userEmail].Add(taskId);
        }

        ///<summary> 
        /// Add tasks from preceding column to specific user.
        ///</summary>
        private void MapTask(string userEmail, IList<int> taskLst)
        {
            if (!_Mapper.ContainsKey(userEmail))
                _Mapper[userEmail] = taskLst;
            else
                _Mapper[userEmail] = _Mapper[userEmail].Concat(taskLst).ToList();
        }


        ///<summary> 
        /// Validate if specific column is able to recieve param n tasks.
        ///</summary>
        private void ValidateTransfer(uint n)
        {
            if (_Limit != DEFAULT_COLUMN_LIMIT & _Limit - CurrentCapacity < n)
                throw new ArgumentException($"Column: {_Name} can not recieve additional {n} tasks. " +
                    $"It's avaliable capacity is: {_Limit - _CurrentCapacity}");
        }


        ///<summary>
        /// Return Btask only if its eligible for modficition.
        ///</summary>
        internal BTask GetTask(string userEmail, int checkMe)
        {
            if (!_Tasks.ContainsKey(checkMe)) throw new ArgumentException($"Task:{checkMe} is not belong to Column: {_Name}.");
            _Tasks[checkMe].IsAssignee(userEmail);
            return _Tasks[checkMe];
        }

        ///<summary>
        ///Return in progress tasks of specific user. 
        ///</summary>
        internal IList<BTask> GetInProgressTask(string userEmail)
        {
            IList<BTask> output = new List<BTask>();
            if (_Mapper.ContainsKey(userEmail))
            {
                foreach (int taskID in _Mapper[userEmail]) /* iterate over all tasks ID assigned to the user   */
                    output.Add(_Tasks[taskID]); /* for each retrieved task ID, get its BTask object. */
            }
            return output;
        }

        ///<summary> 
        ///Return all tasks of this column. 
        ///</summary>
        internal IList<BTask> GetColumn()
        {
            List<BTask> output = new List<BTask>();
            foreach (KeyValuePair<int, BTask> p in _Tasks)
                output.Add(p.Value);
            return output;
        }

        ///<summary> 
        ///Add task to specific user.
        ///</summary>
        internal BTask AddTask(string assignee, string title, string description, DateTime dueDate, int key, string creator, string bname)
        {
            ValidateTransfer(1);
            BTask item = new BTask(key, title, description, dueDate, assignee, creator, bname, Ordinal);
            MapTask(assignee, key);
            _Tasks[key] = item;
            CurrentCapacity = _CurrentCapacity+1;
            return item;
        }

        ///<summary>
        ///Add undone task id to specific user task list. 
        ///The added task was belonged to the same user at the preceding column.
        ///</summary>
        internal void AddTask(string userEmail, BTask item, int itemID)
        {
            ValidateTransfer(1);
            item.Ordinal = _Ordinal;
            MapTask(userEmail, itemID);
            _Tasks[itemID] = item;
            CurrentCapacity = _CurrentCapacity+1;
        }

        ///<summary>
        ///Remove undone task from specific user task list. 
        ///The return task will be added to user task list at the successive column 
        ///</summary>
        internal BTask RemoveTask(string userEmail, int removeMe)
        {
            BTask item = GetTask(userEmail, removeMe);
            _Mapper[userEmail].Remove(removeMe);
            _Tasks.Remove(removeMe);
            CurrentCapacity = _CurrentCapacity-1;
            return item;
        }

        ///<summary> 
        ///Delete Tasks data from DB .
        ///</summary>
        internal void Delete()
        {
            foreach (KeyValuePair<int, BTask> p in _Tasks)
                p.Value.Delete();
            foreach (KeyValuePair<string, IList<int>> p in _Mapper)
                p.Value.Clear();
            _Tasks.Clear();
            _Mapper.Clear();
            MyDalObj.Delete();
        }

        ///<summary> 
        ///Update task due date.
        ///</summary>
        internal void Update(string userEmail, int taskId, DateTime dueDate)
        {
            BTask updateMe = GetTask(userEmail, taskId);
            updateMe.DueDate = dueDate;
        }

        ///<summary> 
        ///Update task description.
        ///</summary>
        internal void Update(string userEmail, int taskId, string description)
        {
            BTask updateMe = GetTask(userEmail, taskId);
            updateMe.Description = description;
        }

        ///<summary> 
        ///Update task title.
        ///</summary>
        internal void UpdateT(string userEmail, int taskId, string title)
        {
            BTask updateMe = GetTask(userEmail, taskId);
            updateMe.Title = title;

        }

        ///<summary> 
        ///Replace task assignee. 
        ///</summary>
        internal void AssignTask(int assignMe, string emailAssignee)
        {
            string currentAssignee = _Tasks[assignMe].Assignee;
            _Tasks[assignMe].Assignee = emailAssignee;
            _Mapper[currentAssignee].Remove(assignMe);
            MapTask(emailAssignee, assignMe);
        }

        ///<summary> 
        /// Return and remove all column tasks and their mapping 
        ///</summary>
        internal Tuple<Dictionary<int, BTask>, Dictionary<string, IList<int>>> TransferTasks()
        {
            var output = new Tuple<Dictionary<int, BTask>, Dictionary<string, IList<int>>>(_Tasks, _Mapper);
            _Tasks = null;
            _Mapper = null;
            MyDalObj.Delete();
            return output;
        }


        ///<summary> 
        /// Add successor column (recently removed) content.
        /// Update each added task ordinal to this column ordinal.
        ///</summary>
        internal void ReceiveTransfer(Tuple<Dictionary<int, BTask>, Dictionary<string, IList<int>>> data, uint n)
        {
            ValidateTransfer(n);
            CurrentCapacity = _CurrentCapacity + n;
            foreach (KeyValuePair<int, BTask> pair in data.Item1)
            {
                _Tasks[pair.Key] = pair.Value;
                pair.Value.Ordinal = Ordinal;
            }
            foreach (KeyValuePair<string, IList<int>> pair in data.Item2)
                MapTask(pair.Key, pair.Value);
        }

        internal void IsMovable()
        {
            if (_Tasks.Count != 0)
                throw new InvalidOperationException($"Only empty columns can be moved.");
        }

        internal uint CurrentCapacity 
        { 
            get => _CurrentCapacity;
            set
            {
                _CurrentCapacity = value;
                MyDalObj.CurrentCapacity = value;
            }
        }

        internal int Ordinal 
        { 
            get => _Ordinal;
            set
            {
                _Ordinal = value;
                MyDalObj.Ordinal = value;
            } 
        }

        internal IList<BTask> Tasks 
        { 
            get => _Tasks.Values.ToList(); 
        }

        internal string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                MyDalObj.Name = value;
            }
        }

        internal int Limit
        {
            get => _Limit;
            set
            {
                ValidateLegalLimit(value);
                _Limit = value;
                MyDalObj.Limit = value;
            }
        }

        public override bool Equals(object obj) => obj is Column && ((Column)obj).Ordinal == Ordinal;
        public override int GetHashCode() => Ordinal.GetHashCode();
        public override string ToString() => $"Column: {_Name} Ordinal: {_Ordinal}";
    }
}
