using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    public class ExchangeCard : Items
    {
        public override string itemName
        {
            get
            {
                return "패 바꾸기";
            }
        }
        public override void useItem(ref List<Player> players, ref bool Rule)
        {
            Console.WriteLine("교환 하실 카드 번호 1, 2를 선택해주세요.");
            int myExchange = int.Parse(Console.ReadLine()) - 1;

            Console.WriteLine($"교환할 상대 플레이어를 선택하세요(1~{players.Count - 1})");
            int targetPlayer = int.Parse(Console.ReadLine()) - 1;

            Console.WriteLine("교환할 상대방의 카드 번호 1, 2를 선택해주세요.");
            int yourExchange = int.Parse(Console.ReadLine()) - 1;

            Card temp;
            temp = players[3].Cards[myExchange];
            players[3].Cards[myExchange] = players[targetPlayer].Cards[yourExchange];
            players[targetPlayer].Cards[yourExchange] = temp;

            Console.WriteLine("당신의 패는" + players[3].GetCardText());
        }
    }
}
