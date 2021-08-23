using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DTOs;
using IntroSE.Kanban.Backend.DataLayer.DAOs;

namespace IntroSE.Kanban.Backend.Business_Layer.Board
{
    /* Business Layer Task object */
    internal class BTask
    {
        //Magic Numbers
        private const ushort MAX_TITLE_LENGTH = 50;
        private const ushort MAX_DESCRIPTION_LENGTH = 300;

        // Instance fields
        internal DateTime CreationTime { get; }
        private string _Title;
        private object _Description;
        private DateTime _DueDate;
        private string _Assignee;
        private readonly DTask MyDalObj;


        internal BTask(int id, string title, string description, DateTime dueDate,string assignee, string creator, string bname, int ordinal)
        {
            _Title = title;
            _Description = description;
            _DueDate = dueDate;
            _Assignee = assignee;
            CreationTime = DateTime.Now;
            MyDalObj = new DTask(id, CreationTime, dueDate, title, description, assignee, creator, bname, ordinal);
            MyDalObj.Insert();
        }

        ///<summary> 
        ///Create an instance basen upon DB data. 
        ///</summary>
        internal BTask(DTask src)
        {
            _Title = src.Title;
            _Description = src.Description;
            _DueDate = DateTime.Parse(src.DueDate);
            _Assignee = src.Assignee;
            CreationTime = DateTime.Parse(src.CreationTime);
            MyDalObj = src;
        }

        ///<summary> 
        ///Validate param title is of none-negative size and below 50. 
        ///</summary>
        private void ValidateTitleRules(string title)
        {
            if (title == null) throw new ArgumentException("Null is illegal argument for Task title property");
            int size = title.Length;
            if (size == 0)
                throw new ArgumentException("Empty title is illegal");
            if (size > MAX_TITLE_LENGTH)
                throw new ArgumentException($"Title length exceeds title length limit: {MAX_TITLE_LENGTH}");
        }

        ///<summary> 
        ///Validate param description is legal: empty or size below 300. 
        ///</summary>
        private void ValidateDescriptionRules(object description)
        {
            if(description is not null && description is string &&  ((string)description).Length > MAX_DESCRIPTION_LENGTH)
                throw new ArgumentException(String.Format("Description length exceeds description length limit: {0}", MAX_DESCRIPTION_LENGTH));
        }

        ///<summary>
        ///Validate param dueDate is later than creation task date and later than now. 
        ///</summary>
        private void ValidateLegalDueDate(DateTime dueDate)
        {
            if (dueDate < CreationTime)
                throw new ArgumentException($"Due date should be later than: {CreationTime};");
            if(dueDate < DateTime.Now)
                throw new ArgumentException($"Due date should be later than: {DateTime.Now};");
        }

        internal object Description
        {
            get => _Description;
            set 
            {
                ValidateDescriptionRules(value);
                _Description = value;
                MyDalObj.Description = value;
            }
        }

        internal string Title
        {
            get => _Title;
            set
            {
               ValidateTitleRules(value);
               _Title = value;
                MyDalObj.Title = value;
            }
        }

        internal DateTime DueDate
        {
            get => _DueDate;
            set
            {
                ValidateLegalDueDate(value);
                _DueDate = value;
                MyDalObj.DueDate = value.ToString();
            }
        }

        internal string Assignee
        {
            get => _Assignee;
            set
            {
                _Assignee = value;
                MyDalObj.Assignee = value;
            }
        }

        internal int Ordinal
        {
           set
            {
                MyDalObj.Ordinal = value;
            }
        }

        internal int ID { get => MyDalObj.ID; }
        internal void Delete() { MyDalObj.Delete(); }

        internal void IsAssignee (string userEmail)
        {
            if(! _Assignee.Equals(userEmail))
                throw new ArgumentException($"Task is not assigned to User: {userEmail}.");
        }

        public override string ToString() => $"ID: {MyDalObj.ID} Board: {MyDalObj.BoardName} Assignee: {_Assignee}";

    }

}
