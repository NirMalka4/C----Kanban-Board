using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public class Board
    {
        public readonly string CreatorEmail;
        public readonly string Name;
        public readonly IList<string> Members;
        public readonly int Capacity;

        public Board(string ce, string n, IList<string> m)
        {
            CreatorEmail = ce;
            Name = n;
            Members = m;
        }

        internal Board(Business_Layer.Board.Board src)
        {
            CreatorEmail = src.Creator;
            Name = src.Name;
            Members = src.Members;
            Capacity = src.Capacity;
        }
    }
}
