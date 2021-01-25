using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sutda;

namespace SutdaUnittest
{
    [TestClass]
    public class DealerTest
    {
        [TestMethod]
        public void 스무장의_카드에는_광이_3장_들어있어야_함()
        {
            Dealer dealer = new Dealer();
            List<Card> cards = new List<Card>();

            for (int i = 0; i < 20; i++)
            {
                cards.Add(dealer.DrawCard());
            }

            int KwangCount = 0;

            foreach (Card card in cards)
                if (card.IsKwang)
                    KwangCount++;

            Console.WriteLine("광의 개수" + KwangCount);


            dealer.BetMoney(100);
            int prize = dealer.GetMoney();

            //어떤 조건을 체크하기
            Assert.AreEqual(3, KwangCount);
        }

        [TestMethod]
        public void 카드가1에서10까지2장씩들어있어야함()
        {
            Dealer dealer = new Dealer();
            List<Card> cards = new List<Card>();
            int[] cardNums = new int[10];
            for (int i = 0; i < 20; i++)
            {
                cards.Add(dealer.DrawCard());
            }

            foreach (Card card in cards)
                cardNums[card.Number]++;

           
            dealer.BetMoney(100);
            int prize = dealer.GetMoney();

            //어떤 조건을 체크하기
            foreach (int cardNo in cardNums)
            {
                Assert.AreEqual(2, cardNo);
            }
        }
    }
}
