using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DAOs;


namespace IntroSE.Kanban.Backend.DataLayer.DTOs
{
    class DBoard : DTO
    {
        /* Table column names. */
        private const string ThirdColumn = "Capacity";
        private const string ForthColumn = "NextKey";
        private const string FifthColumn = "ColumnIndexer";
        private const string SixthColumn = "Right";

        /* Instance fields */
        private readonly DAO<DBoard> Boards_Controller = DBoards.Instance;
        private readonly DColumns Columns_Controller = DColumns.Instance;
        private readonly DMembers Members_Controller = DMembers.Instance;
        private int _Capacity, _NextKey, _ColumnIndexer, _right;

        internal string CreatorEmail { get; }
        internal string BoardName { get; }
        internal int Capacity { get => _Capacity; set { _Capacity = value; Boards_Controller.Update(this, ThirdColumn, value); } }
        internal int NextKey { get => _NextKey; set { _NextKey = value; Boards_Controller.Update(this, ForthColumn, value); } }
        internal int ColumnIndexer { get => _ColumnIndexer; set { _ColumnIndexer = value; Boards_Controller.Update(this, FifthColumn, value); } }
        internal int Right { get => _right; set { _right = value; Boards_Controller.Update(this, SixthColumn, value); } }

        internal DBoard(string creator, string bname, int capacity, int nextKey, int indexer, int right)
        {
            CreatorEmail = creator;
            BoardName = bname;
            _Capacity = capacity;
            _NextKey = nextKey;
            _ColumnIndexer = indexer;
            _right = right;
        }

        internal DBoard() { }

        public void Insert() => Boards_Controller.Insert(this);

        public void Delete() => Boards_Controller.Delete(this);

        public IList<DTO> Select() => Boards_Controller.Select();

        public void Clear() => Boards_Controller.Clear();

        ///<summary> 
        /// Return all column records belong to specific board.
        ///</summary>
        public IList<DTO> SelectColumns() => Columns_Controller.Select(CreatorEmail, BoardName);

        ///<summary> 
        /// Return all members records of specific baord.
        ///</summary>
        public IList<DTO> SelectMembers() => Members_Controller.Select(CreatorEmail, BoardName);
    }
}
