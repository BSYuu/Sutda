using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    public class Card
    {
        //생성자에서만 변경 가능
        public int Number { get; }

        //Card 클래스 내에서는 변경이 가능 -> private set
        public bool IsKwang { get; private set; }

        //카드가 비광인지 체크하는 플래그
        public bool IsBiKwang { get; private set; }
        public Card(int number, bool isKwang, bool isBiKwang)
        {
            Number = number;
            IsKwang = isKwang;
            IsBiKwang = isBiKwang;
        }

        public override string ToString()
        {
            if (IsKwang)
                return Number + "K";
            else
                return Number.ToString();
            
        }

    }
}
