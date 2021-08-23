using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class MessageViewModel : NotifiableObject
    {
        private string message;
        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }

        private string msgColor;
        public string MsgColor
        {
            get => msgColor;
            set
            {
                this.msgColor = value;
                RaisePropertyChanged("MsgColor");
            }
        }
    
        public void Success(string text)
        {
            this.MsgColor = "BLUE";
            this.Message = text;
        }
        public void Fail(string text)
        {
            this.MsgColor = "RED";
            this.Message = text;
        }

        internal MessageViewModel()
        {

        }
    }
}
