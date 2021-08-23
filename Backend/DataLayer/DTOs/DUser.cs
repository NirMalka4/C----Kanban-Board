using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataLayer.DAOs;

namespace IntroSE.Kanban.Backend.DataLayer.DTOs
{
    class DUser: DTO
    {
        /* Instance fields */
        private readonly DAO<DUser> _Controller = DUsers.Instance;
        internal string Email { get; }
        internal string Password { get; }

        internal DUser(string email, string pass)
        {
            Email = email;
            Password = pass;
        }

        internal DUser() { }
        public void Insert() => _Controller.Insert(this);

        public void Delete() => _Controller.Delete(this);

        public IList<DTO> Select() => _Controller.Select();

        public void Clear() => _Controller.Clear();
    }
}
