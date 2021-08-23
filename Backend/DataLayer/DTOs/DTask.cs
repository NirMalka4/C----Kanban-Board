using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DAOs;

namespace IntroSE.Kanban.Backend.DataLayer.DTOs
{
    class DTask : DTO
    {
        /* Table column names.  */
        private const string ThirdColumn = "DueDate";
        private const string ForthColumn = "Title";
        private const string FifthColumn = "Description";
        private const string SixthColumn = "Assignee";
        private const string NinethColumn = "Ordinal";

        /* Instance field  */
        private readonly DAO<DTask> _Controller = DTasks.Instance;
        private int _ID, _Ordinal;
        private string _CreationTime, _DueDate; /* save dates as string since SQL server does not support DateTime type. */ 
        private string _Title, _Assignee, _CreatorEmail, _BoardName;
        private object  _Description;

        internal int ID { get => _ID; }
        internal string CreationTime { get => _CreationTime; }
        internal string DueDate { get =>_DueDate;  set { _DueDate = value.ToString(); _Controller.Update(this, ThirdColumn, value); } }
        internal string Title { get => _Title;  set { _Title = value; _Controller.Update(this,ForthColumn, value); } }
        internal object Description { get => _Description;  set { _Description = value; _Controller.Update(this,FifthColumn ,value);} }
        internal string Assignee { get => _Assignee;  set { _Assignee = value; _Controller.Update(this, SixthColumn ,value); } }
        internal string CreatorEmail { get => _CreatorEmail; }
        internal string BoardName { get => _BoardName; }
        internal int Ordinal { get => _Ordinal; set { _Ordinal = value; _Controller.Update(this, NinethColumn, value); } }

        internal DTask(int id, DateTime ct, DateTime dd, string t, object desc, string ass, string creator, string bname,int ordinal)
        {
            _ID = id;
            _CreationTime = ct.ToString();
            _DueDate = dd.ToString();
            _Title = t;
            _Description = desc;
            _Assignee = ass;
            _CreatorEmail = creator;
            _BoardName = bname;
            _Ordinal = ordinal;
        }

        /* Used by ReaderToDTO function. 
         * same as the above constructor except for ct and dd params which are string (as SQL does not support DateTime) */
        internal DTask(int id, string ct, string dd, string t, object desc, string ass, string creator, string bname, int ordinal)
        {
            _ID = id;
            _CreationTime = ct;
            _DueDate = dd;
            _Title = t;
            _Description = desc;
            _Assignee = ass;
            _CreatorEmail = creator;
            _BoardName = bname;
            _Ordinal = ordinal;
        }

        public void Insert() => _Controller.Insert(this);

        public void Delete() => _Controller.Delete(this);

        public IList<DTO> Select() => _Controller.Select();

        public void Clear() => _Controller.Clear();


    }
}
