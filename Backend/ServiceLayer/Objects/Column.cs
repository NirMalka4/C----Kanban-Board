using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public class Column
    {
        public readonly string Name;
        public readonly uint Capacity;
        public readonly int Limit;
        public readonly int Ordinal;
        public readonly IList<Task> Tasks;

        public Column(string n, uint c, int l, int o, IList<Task> t) 
        {
            Name = n;
            Capacity = c;
            Limit = l;
            Ordinal = o;
            Tasks = t;
        }

        /* Initiate instance based upon BL compatibale instace  */
        internal Column(Business_Layer.Board.Column src)
        {
            Name = src.Name;
            Capacity = src.CurrentCapacity;
            Limit = src.Limit;
            Ordinal = src.Ordinal;
            Tasks = new List<Task>();
            foreach (Business_Layer.Board.BTask task in src.Tasks)
                Tasks.Add(new Task(task));
        }
    }
}
