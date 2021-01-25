using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    public class BasicPlayer :Player
    {
        public BasicPlayer(string name)
        {
            Name = name;
        }
        public override int CalculateScore()
        {
            if (Cards[0].IsKwang && Cards[1].IsKwang)
            {
                if (!((Cards[0].Number == 3 && Cards[1].Number == 8) ||     //38광땡은 광으로 취급 x 무조건 이김
                    (Cards[0].Number == 8 && Cards[1].Number == 3)))
                        hasKwang = true;

                return Cards[0].Number * 10 + Cards[1].Number * 10; //30~110
            }

            else if (Cards[0].Number == Cards[1].Number)
            {
                hasDDaeng = true;
                return Cards[0].Number + 10; // 11~20
            }

            else
            {
                if (((Cards[0].Number == 3) && (Cards[1].Number == 7)) ||
                    ((Cards[0].Number == 7) && (Cards[1].Number == 3)))
                    hasDJE = true; // 땡잡이

                else if (((Cards[0].Number == 4) && (Cards[1].Number == 7)) ||
                        ((Cards[0].Number == 7) && (Cards[1].Number == 4)))
                    hasAgent = true;// 암행어사 -> 13광땡, 18광땡 잡음

                else if (((Cards[0].Number == 4) && (Cards[1].Number == 9)) ||
                         ((Cards[0].Number == 9) && (Cards[1].Number == 4)))
                    has49 = true; // 49파토 -> 판 리셋


                return (Cards[0].Number + Cards[1].Number) % 10; // 3~9
            }
                
        }

    }
}
