using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    public class PeekTheCard : Items
    {
        public override string itemName
        {
            get
            {
                return "패 엿보기";
            }
        }
        public override void useItem(ref List<Player>players, ref bool Rule)
        {
            Console.WriteLine($"1~{players.Count}의 플레이어 중 하나를 선택하세요");
            int playerIdx = int.Parse(Console.ReadLine());

            Console.WriteLine("1번과 2번 카드중 하나를 선택해주세요(1 또는 2 입력)");
            int cardIdx  = int.Parse(Console.ReadLine());

            Console.WriteLine("예림이 그 패 봐바!");
            System.Threading.Thread.Sleep(1000);

            string card = players[playerIdx - 1].Cards[cardIdx - 1].ToString();
            Console.WriteLine(card + "네? " + card + "여!");
        }
    }
}
