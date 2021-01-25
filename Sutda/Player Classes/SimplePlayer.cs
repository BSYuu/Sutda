using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    public class SimplePlayer : Player
    {
        public SimplePlayer(string name)
        {
            Name = name;
        }
        public override int CalculateScore()
        {
            return Cards[0].Number + Cards[1].Number;
        }
    }
}
