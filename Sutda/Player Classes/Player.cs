using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    public abstract class Player
    {
        public Player()
        {
            Cards = new List<Card>();
            item = null;
        }
        #region MemberDeclare
        public List<Card> Cards { get; set; }
        public Items item { get; set; }
        public string Name { get; protected set; }
        public int Money { get; set; }

        public bool hasAgent { get; protected set; }
        public bool hasDJE { get; protected set; }
        public bool has49 { get; protected set; }
        public bool IsDied { get; set; }
        public bool hasDDaeng { get; protected set; }
        public bool hasKwang { get; protected set; }
        #endregion


        public void Init_hasFlag()
        {
            hasAgent = false;
            hasDJE = false;
            has49 = false;
            hasDDaeng = false;
            hasKwang = false;
        }

        public Card GetBikwang
        {
            get{
                if (Cards[0].IsBiKwang)
                    return Cards[0];
                else
                    return Cards[1];
            }
            set
            {
                if (Cards[0].IsBiKwang)
                    Cards[0] = value;
                else
                    Cards[1] = value;
            }
        }

        public abstract int CalculateScore();

        public string GetCardText()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var card in Cards)
                builder.Append(card.ToString() + "\t");

            return builder.ToString();
        }
        
    }
}
