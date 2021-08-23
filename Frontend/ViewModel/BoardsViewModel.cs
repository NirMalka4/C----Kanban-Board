using Frontend.Model;
using Frontend.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class BoardsModel : NotifiableObject
    {
        private BackendController BC { get; set; }
        public UserModel UserM { get; set; }

        internal BoardsModel(UserModel u)
        {
            this.UserM = u;
            this.BC = u.BackendController;
            Title = $"Hello {UserM.Email}";
            this.MSG = new MessageViewModel();
        }

        public string Title { get; private set; }

        public MessageViewModel MSG { get; set; }

        public void Logout()
        {
            BC.Logout(UserM.Email);
        }

        private BoardModel selectedBoard;
        public BoardModel SelectedBoard
        {
            get => selectedBoard;
            set
            {
                selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }
        private bool enableForward = false;
        public bool EnableForward
        {
            get => enableForward;
            private set
            {
                enableForward = selectedBoard != null;
                RaisePropertyChanged("EnableForward");
            }
        }

        /// <summary>
        /// Removes board
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void RemoveBoard()
        {
            try
            {
                UserM.RemoveBoard(SelectedBoard);
                MSG.Success("Removed Board Successfully");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// loads selected board to boardview
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void LoadBoard()
        {
            try
            {
                SelectedBoard.LoadBoard();
                new BoardView(BC, UserM, SelectedBoard).Show();
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
            
        }

        /// <summary>
        /// open join boardview
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void JoinBoard()
        {
            new JoinBoardView(BC, UserM).Show();
        }

        /// <summary>
        /// opens inprogresstasks view
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        public void InProgressTasks()
        {
            try
            {
                UserM.InProgressTasks();
                new InProgressTasksView(UserM).Show();
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }
    }
}
