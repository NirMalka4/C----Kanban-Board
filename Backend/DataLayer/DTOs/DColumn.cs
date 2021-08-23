using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DAOs;

namespace IntroSE.Kanban.Backend.DataLayer.DTOs
{
    class DColumn : DTO 
    {
        /* Table column names. */
        private const string ThirdColumn = "Ordinal";
        private const string ForthColumn = "Name";
        private const string FifthColumn = "\"Limit\"";
        private const string SixthColumn = "CurrentCapacity";

        /* Instance field  */
        private readonly DAO<DColumn> Columns_Controller = DColumns.Instance;
        private readonly DTasks Tasks_Controller = DTasks.Instance;

        /* _Position: Describe ith place in the insertion sequence. 
         * First inserted DColumn will have _Position=0, second _Position=1 and so on.
         * Add this identifier since previous primary key(part of), _Ordinal , may be changed now. */
        private readonly int _Position; 
        private int _Limit, _Ordinal;
        private string _CreatorEmail, _BoardName, _Name;
        private uint _CurrentCapacity;
        
        internal string CreatorEmail { get => _CreatorEmail; }
        internal string BoardName { get => _BoardName; }

        internal string Name { get => _Name; set { _Name = value; Columns_Controller.Update(this, ForthColumn, "'"+value+"'"); } }
        internal int Limit { get => _Limit; set { _Limit = value; Columns_Controller.Update(this, FifthColumn, value); } }
        internal uint CurrentCapacity { get => _CurrentCapacity; 
                                        set { _CurrentCapacity = value; Columns_Controller.Update(this, SixthColumn, value); } }
        internal int Position { get => _Position; }

        internal DColumn(string creator, string bname, int ordinal, string name, int limit, uint cc, int pos)
        {
            _CreatorEmail = creator;
            _BoardName = bname;
            _Ordinal = ordinal;
            _Name = name;
            _Limit = limit;
            _CurrentCapacity = cc;
            _Position = pos;
        }

        internal int Ordinal
        {
            get => _Ordinal;
            set
            {
                /* Update all tasks ordinal belong to the changed column. */
                Tasks_Controller.Update(_CreatorEmail, _BoardName, _Ordinal, value);
                _Ordinal = value;
                Columns_Controller.Update(this, ThirdColumn, value);
            }
        }
        public void Insert() => Columns_Controller.Insert(this);

        public void Delete() => Columns_Controller.Delete(this);

        public IList<DTO> Select() => Columns_Controller.Select();

        public void Clear() => Columns_Controller.Clear();

        ///<summary> 
        /// Return all tasks records belong to specific column.
        ///</summary>
        public IList<DTO> SelectTasks() => Tasks_Controller.Select(_CreatorEmail, _BoardName, _Ordinal);

    }
}
