using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBai
{
    [Serializable]
    internal class SocketData
    {
        private int command;
        private string message;
        private card cards;
        public int Command { get => command; set => command = value; }
        internal card Cards { get => cards; set => cards = value; }
        public string Message { get => message; set => message = value; }

        public SocketData(int command,string message, card cards) {
            this.command = command;
            this.cards = cards;
            this.message = message;
        }
    }
    public enum SocketCommand
    {
        SEND_CARD,
        NOTIFY,
        NEW_GAME,
        QUIT,
        SEND_HAND,
        SEND_PLAYCARD
    }
}
