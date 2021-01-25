using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    public class Upsidedown : Items
    {
        public override string itemName
        {
            get
            {
                return "낮은 사람 승리";
            }
        }
        public override void useItem(ref List<Player>players, ref bool Rule)
        {
            Rule = true;
        }
    }
}
