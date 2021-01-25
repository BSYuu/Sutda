using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    //그냥 class면 해당 프로젝트 내에서만 접근가능
    //public class면 다른 프로젝트에서도 가능
    public class Dealer
    {
        #region money
        private int _money;
        public void BetMoney(int bettingMoney)
        {
            _money += bettingMoney;
        }

        public int GetMoney()
        {
            int moneyToReturn = _money;
            _money = 0;
            return moneyToReturn;
        }
        #endregion

        private List<Card> _cards = new List<Card>();

        private int _cardIndex;

        public Card DrawCard()
        {
            return _cards[_cardIndex++];
        }

        public Dealer()
        {
            InitCards();
        }

        public void InitCards()
        {
            _cards.Clear();
            _cardIndex = 0;
            //카드 만들기
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    bool isKwang = (j == 1) && (i == 1 || i == 3 || i == 8);
                    Card card = new Card(i, isKwang, false);
                    _cards.Add(card);
                }
            }
            Card cardbiKwang = new Card(0, false, true);    //비광
            _cards.Add(cardbiKwang);

            _cards = _cards.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
