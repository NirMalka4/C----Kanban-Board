using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = IntroSE.Kanban.Backend.ServiceLayer.Objects.Task;

namespace Frontend.Model
{
    class UserModel : NotifiableObject
    {
        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
            }
        }

        private BackendController backendController;
        public BackendController BackendController
        {
            get => backendController;
            set
            {
                backendController = value;
            }
        }

        private ObservableCollection<BoardModel> myBoards;
        public ObservableCollection<BoardModel> MyBoards{
            get => myBoards;
            set
            {
                myBoards = value;
                RaisePropertyChanged("MyBoards");
            }
        }

        public UserModel(string email, BackendController bc)
        {
            this.Email = email;
            this.BackendController = bc;
            getMyBoards();
        }

        private ColumnModel myTasks;
        public ColumnModel MyTasks {
            get => myTasks;
            set 
            {
                myTasks = value;
            }
        }

        public Dictionary<TaskModel, Tuple<string, string>> MyTasksDict { get; set; }


        //only whem creating board so my email is both user and creator
        public void AddBoard(string bName)
        {
            BackendController.AddBoard(Email, bName);
            getMyBoards();
        }

        /// <summary>
        /// removes board from myBoards
        /// </summary>
        /// <param name="userEmail">email of the user about to view these tasks</param>
        public void RemoveBoard(BoardModel b)
        {
            BackendController.RemoveBoard(Email, b.Name);
            MyBoards.Remove(b);
        }

        /// <summary>
        /// gets all boards userr is member of
        /// </summary>
        private void getMyBoards()
        {
            IList<Board> boards = BackendController.GetMyBoards(Email);
            MyBoards = new ObservableCollection<BoardModel>();
            foreach (Board b in boards)
                MyBoards.Add(new BoardModel(email, b, BackendController));

        }

        /// <summary>
        /// join a board as a member
        /// </summary>
        /// <param name="creatorEmail">email of the user who created the board</param>
        /// <param name="boardName">boards name</param>
        public void JoinBoard(string creatorEmail, string boardName)
        {
            BackendController.JoinBoard(Email, creatorEmail, boardName);
            getMyBoards();
        }

        /// <summary>
        /// gets all inprogress tasks
        /// </summary>
        public void InProgressTasks()
        {

            Dictionary<Tuple<string, string>, Task> tsks = BackendController.GetMyInProgressTasks(Email);
            MyTasksDict = new Dictionary<TaskModel, Tuple<string, string>>();

            IList<Task> myTasks = new List<Task>();

            foreach (var p in tsks)
            {
                myTasks.Add(tsks[p.Key]);
            }

            MyTasks = new ColumnModel(Email, myTasks, BackendController);
            
            for(int i = 0; i < MyTasks.Tasks.Count; i++)
            {
                MyTasksDict.Add(MyTasks.Tasks[i], tsks.Keys.ToList()[i]);
            }

        }
    }
}
