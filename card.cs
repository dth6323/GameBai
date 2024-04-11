using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBai
{
    [Serializable]
    public class card
    {
        private const string ImagePath = @"E:\\Code\\AI\\GameBai\\images\\";

        public card()
        {
        }

        public card(int value, int suit)
        {
            Value = value;
            Suit = suit;
        }

        public int Value { get; set; }

        public int Suit { get; set; }

        public string ImageName
        {
            get
            {
                return ImagePath + "card" + Suit + "_" + Value + ".png";
            }
        }
    }
}
