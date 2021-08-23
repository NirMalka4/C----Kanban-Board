using Frontend.Model;
using Frontend.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        public BoardModel BoardM { get; set; }
        private BackendController BC { get; set; }
        private UserModel UserM { get; set; }

        public MessageViewModel MSG { get; set; }
        internal BoardViewModel(BackendController bc, BoardModel b, UserModel u)
        {
            this.BC = bc;
            this.UserM = u;
            this.BoardM = b;
            this.MSG = new MessageViewModel();
        }

        private ColumnModel selectedColumn;
        public ColumnModel SelectedColumn
        {
            get => selectedColumn;
            set
            {
                selectedColumn = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedColumn");
            }
        }
        private bool enableForward = false;
        public bool EnableForward
        {
            get => enableForward;
            private set
            {
                enableForward = !enableForward;
                RaisePropertyChanged("EnableForward");
            }
        }


        /// <summary>
        /// loads selected column in columnView
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void LoadColumn()
        {
            try
            {
                new ColumnView(BC, UserM, BoardM, BoardM.Cols[SelectedColumn.Ordinal]).Show();
            }
            catch(Exception e)
            {
                MSG.Fail(e.Message);
            }
            
        }

        /// <summary>
        /// removes selected column
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void RemoveColumn()
        {
            try 
            {
                BoardM.RemoveColumn(SelectedColumn.Ordinal);
                MSG.Success("Column Removed");
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
            
        }

        /// <summary>
        /// shifts selected column by -1
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void MoveUp()
        {
            try 
            { 
                BoardM.MoveColumnUp(SelectedColumn.Ordinal);
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// shifts selected column by 1
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void MoveDown()
        {
            try
            {
                BoardM.MoveColumnDown(SelectedColumn.Ordinal);
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// opens renameView for selected column
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void RenameColumn()
        {
            try
            { 
                new RenameColumnView(BC, UserM, BoardM, SelectedColumn).Show();
            }
            catch (Exception e)
            {
                MSG.Fail(e.Message);
            }
        }

        /// <summary>
        /// opens shiftColumnView for selected column
        /// </summary>
        /// <retrns> displays error message if fails</retrns>
        internal void ShiftColumn()
        {
            new ShiftColumnView(BC, UserM, BoardM, SelectedColumn.Ordinal).Show();
        }
    }
}
