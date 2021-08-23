using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DAOs;

namespace IntroSE.Kanban.Backend.DataLayer.DTOs
{
    class DMember: DTO
    {
        /* Instance fields */
        private readonly DAO<DMember> _Controller = DMembers.Instance;
        internal string CreatorEmail { get; }
        internal string BoardName { get; }
        internal string UserEmail { get; }

        internal DMember(string creator, string bname, string member )
        {
            CreatorEmail = creator;
            BoardName = bname;
            UserEmail = member;
        }

        public void Insert() => _Controller.Insert(this);

        public void Delete() => _Controller.Delete(this);

        public IList<DTO> Select() => _Controller.Select();

        public void Clear() => _Controller.Clear();
    }
}
