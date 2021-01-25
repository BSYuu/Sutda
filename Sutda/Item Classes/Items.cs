using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    public abstract class Items
    {
        public virtual string itemName { get;}
        public abstract void useItem(ref List<Player> players, ref bool Rule);
    }
}
