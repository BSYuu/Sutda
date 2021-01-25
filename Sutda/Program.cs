using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sutda
{
    class Program
    {
        #region Member Declare
        private static string[] CharacterNames = {"짝귀", "고니", "예림이" };
        private const int SeedMoney = 1000;
        private const int OriginalBettingMoney = 100;
        private static int BettingMoney = OriginalBettingMoney;
        private static Player loser = null;
        private static Player Me;
        private static bool winRule = false;
        private static bool IsDraw = false;
        #endregion

        static void Main(string[] args)
        {
            #region Declare
            List<Player> players = new List<Player>();
            int RuleType_Input;
            RuleType ruleType;
            Dealer dealer = new Dealer();
            #endregion

            #region InputRuleType
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("룰 타입을 선택하세요. (1.Basic 2.Simple)");
                    
                    RuleType_Input = int.Parse(Console.ReadLine());
                    if (RuleType_Input != 1 && RuleType_Input != 2)
                        throw new FormatException();

                    ruleType = (RuleType)RuleType_Input;
                    break;
                }
                catch
                {
                    Console.WriteLine("1 또는 2 를 입력해주세요");
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
            }
            #endregion

            AddPlayers(ruleType, players);

            #region Distribute Money
            //플레이어 모두에게 기본금 주기
            foreach (var player in players)
                player.Money = SeedMoney;
            #endregion
            
            StartGame(ref players, ref dealer);
        }

        private static void StartGame(ref List<Player> players, ref Dealer dealer, int Round = 1)
        {
            while (true)
            {
                BettingMoney = OriginalBettingMoney;

                //다이 한 후 되돌리기
                if(Me.IsDied)
                {
                    players.Add(Me);
                    Me.IsDied = false;
                }

                //한명이 오링이면 게임 종료
                if (IsAnyoneOring(players) && !IsDraw)
                    break;

                dealer.InitCards();
                IsDraw = false;

                Console.Clear();
                Console.WriteLine($"[Round {Round++}]");
                

                //학교 출석
                foreach (var player in players)
                {
                    player.Money -= BettingMoney;
                    dealer.BetMoney(BettingMoney);
                }

                //카드 돌리기
                foreach (var player in players)
                {
                    player.Cards.Clear();
                    player.IsDied = false;
                    player.Init_hasFlag();
                    for (int i = 0; i < 2; i++)
                        player.Cards.Add(dealer.DrawCard());
                }

                // 비광든 플레이어 찾기
                Player biGwangPlayer;
                if ((biGwangPlayer = FindHavingBiGwang(players)) != null)       //못찾으면 정상 게임
                {
                    // 비광을 원하는 카드로 바꾸기
                    ChangebiGwang(ref biGwangPlayer);
                }

                //나의 패 보기
                Console.WriteLine("나의 패는 " + Me.GetCardText());


                //아이템 사용 여부 물어보기
                if (Me == loser)
                {
                    Console.WriteLine("아이템을 사용하시겠습니까?");
                    Console.WriteLine("아이템의 이름은 " + loser.item.itemName);
                    Console.WriteLine("Y / N");
                    while (true)
                    {
                        try
                        {
                            char input_c = char.Parse(Console.ReadLine().ToString());
                            if (input_c != 'Y' && input_c != 'N')
                                throw new FormatException();
                            if (input_c == 'Y')
                                loser.item.useItem(ref players, ref winRule);

                            break;
                        }
                        catch
                        {
                            Console.WriteLine("Y 또는 N을 입력해주세요, 대문자만 가능");
                            continue;
                        }
                    }
                }

                //배팅
                while (true)
                {
                    try
                    {
                        Console.WriteLine("1.콜   2.체크   3.다이");
                        int Bet_input = int.Parse(Console.ReadLine());

                        if (Bet_input != 1 && Bet_input != 2 && Bet_input != 3)
                            throw new FormatException();

                        //컴퓨터들은 모두 체크를 함
                        for (int i = 0; i < 3; i++)
                        {
                            if (players[i].Money >= BettingMoney)
                            {
                                players[i].Money -= BettingMoney;
                                dealer.BetMoney(BettingMoney);
                            }
                            else
                            {
                                dealer.BetMoney(players[i].Money);
                                players[i].Money = 0;
                            }
                            
                        }


                        //나의 배팅 선택
                        if (Bet_input == 1)         //콜
                        {
                            BettingMoney += 50;
                            if (Me.Money >= BettingMoney)
                            {
                                Me.Money -= BettingMoney;
                                dealer.BetMoney(BettingMoney);
                            }
                            else
                            {
                                dealer.BetMoney(Me.Money);
                                Me.Money = 0;
                                break;
                            }
                        }
                        else if (Bet_input == 2)        // 체크
                        {
                            if (Me.Money >= BettingMoney)
                            {
                                Me.Money -= BettingMoney;
                                dealer.BetMoney(BettingMoney);
                            }
                            else
                            {
                                dealer.BetMoney(Me.Money);
                                Me.Money = 0;
                            }
                            break;
                        }
                        else                        //다이
                        {
                            players.Remove(Me);
                            Me.IsDied = true;
                            foreach (var player in players)
                            {
                                Console.WriteLine(player.Name);
                            }
                            break;
                        }
                    
                        if (IsAnyoneOring(players))
                            break;
                    }
                    catch
                    {
                        Console.WriteLine("1,2,3 을 입력해주세요");
                        continue;
                    }
                }

                //승자 & 꼴찌 찾기
                try
                {
                    List<Player> winloseList = FindWinLose(ref players, winRule);
                    Player winner = winloseList[0];
                    loser = winloseList[players.Count-1];

                    //승자에게 상금주기
                    winner.Money += dealer.GetMoney();


                    //승자 출력
                    Console.WriteLine(winner.Name + "승리!");

                    //플레이어 현재 상태 출력
                    print_Players_status(players);

                    //꼴지에게 랜덤 아이템 주기
                    GiveItemsTo(loser);

                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    Console.ReadKey();

                }
                catch
                {
                    Console.WriteLine("묻고 다시가!");
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    Console.ReadKey();
                    IsDraw = true;
                    continue;
                }
            }
        }

        #region 기본 함수들
        private static void print_Players_status(List<Player> players)
        {   
            Console.WriteLine("----------------------------");
            foreach (var player in players)
            {
                Console.Write(player.Name + "\t");
            }
            Console.WriteLine();
            foreach (var player in players)
            {
                Console.Write(player.Money + "\t");
            }
            Console.WriteLine();
            foreach (var player in players)
            {
                Console.Write(player.CalculateScore() + "\t");
                player.item = null;
            }

            Console.WriteLine();
        }        
        private static void AddPlayers(RuleType ruleType, List<Player> players)
        {
            //컴퓨터 3명을 넣기
            foreach (string NickName in CharacterNames)
            {
                if (ruleType == RuleType.Basic)
                    players.Add(new BasicPlayer(NickName));
                else
                    players.Add(new SimplePlayer(NickName));
            }

            //플레이어블 캐릭터 생성
            if (ruleType == RuleType.Basic)
                Me = new BasicPlayer("나");
            else
                Me = new SimplePlayer("나");

            //나를 4번째 플레이어로 추가
            players.Add(Me);
        }
        private static void GiveItemsTo(Player loser)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);

            int which = rnd.Next() % 3;

            switch (which)
            {
                case 0:
                    loser.item = new PeekTheCard();
                    break;

                case 1:
                    loser.item = new Upsidedown();
                    break;

                case 2:
                    loser.item = new ExchangeCard();
                    break;
            }
            
        }
        private static List<Player> FindWinLose(ref List<Player> players, bool winRule)
        {
            List<Player> winlosePlayer;

            if (winRule)
                winlosePlayer = players.OrderBy(Player => Player.CalculateScore()).ToList();
            else
                winlosePlayer = players.OrderByDescending(Player => Player.CalculateScore()).ToList();

            //49파토
            if (Check49(players))
            {
                Console.WriteLine("49파토");
                return null;
            }
                

            //땡잡이
            DDaengJabE(players, ref winlosePlayer);

            //암행어사
            SecretAgent(players, ref winlosePlayer);

            foreach (var player in winlosePlayer)
            {
                Console.WriteLine(player.CalculateScore()); 
            }
            if (winlosePlayer[0].CalculateScore() != winlosePlayer[1].CalculateScore())
                return winlosePlayer;

            return null;
        }
        #endregion


        #region 비광(조커패) 함수
        private static void ChangebiGwang(ref Player player)
        {
            if (player != Me)
            {
                Random rnd1 = new Random((int)DateTime.Now.Ticks);
                Random rnd2 = new Random((int)DateTime.Now.Ticks);
                Console.WriteLine(player.Name + "의 패 " + player.GetCardText());
                bool isKwang = (rnd2.Next() % 2) == 1;
                Card newCard = new Card(rnd1.Next() % 10, isKwang, false);
                player.GetBikwang = newCard;
                
                Console.WriteLine("비광 교환 후 패 : " + player.GetCardText());
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("원하는 카드 번호를 입력해주세요");
                int input_cardNum = int.Parse(Console.ReadLine());

                while (true)
                {
                    try
                    {
                        if (input_cardNum == 1 || input_cardNum == 3 || input_cardNum == 8)
                        {
                            Console.WriteLine("광이 있는 카드를 원하십니까? (1:Yes, 2.No)");

                            while (Console.KeyAvailable)
                                Console.ReadKey();

                            bool isKwang = int.Parse(Console.ReadLine()) == 1;

                            Card newCard = new Card(input_cardNum, isKwang, false);
                            player.GetBikwang = newCard;
                            break;
                        }
                        else
                        {
                            Card newCard = new Card(input_cardNum, false, false);
                            player.GetBikwang = newCard;
                            break;  
                        }
                    }
                    catch
                    {
                        Console.WriteLine("1, 3, 8을 입력해주세요");
                        continue;
                    }

                }
            }

        }

        private static Player FindHavingBiGwang(List<Player> players)
        {
            foreach (var player in players)
            {
                if (player.Cards[0].IsBiKwang || player.Cards[1].IsBiKwang)
                    return player;
            }

            return null;
        }
        #endregion

       

        


        #region 특수 패 관련 함수
        private static Player CheckKwang(List<Player> players)
                {
                    foreach (var player in players)
                    {
                        if (player.hasKwang)
                            return player;
                    }

                    return null;
                }
        private static Player CheckAgent(List<Player> players)
                {
                    foreach (var player in players)
                    {
                        if (player.hasAgent)
                            return player;
                    }

                    return null;
                }
        private static Player CheckDJE(List<Player> players)
        {
            foreach (var player in players)
            {
                if (player.hasDJE)
                    return player;
            }

            return null;
        }
        private static Player CheckDDaeng(List<Player> players)
        {
            foreach (var player in players)
            {
                if (player.hasDDaeng)
                    return player;
            }

            return null;
        }
        private static bool Check49(List<Player> players)
        {
            foreach (var player in players)
            {
                if (player.has49)
                {
                    Console.WriteLine(player.Name + " 패 : " + player.GetCardText());
                    return true;
                }
                    
            }

            return false;
        }

        private static void SecretAgent(List<Player> players, ref List<Player> winlosePlayer)
        {
            Player Agent = null;
            Player KwangPlayer = null;

            Agent = CheckAgent(players);
            KwangPlayer = CheckKwang(players);
            if (Agent != null && KwangPlayer != null)
            {
                winlosePlayer.Remove(Agent);
                winlosePlayer.Remove(KwangPlayer);
                winlosePlayer.Add(KwangPlayer);
                winlosePlayer.Insert(0, Agent);
                Console.WriteLine("암행어사 출두요!");
                Console.WriteLine("암행어사 : " + Agent.Name + "패 : " + Agent.GetCardText());
                Console.WriteLine("광 : " + KwangPlayer.Name + "패 : " + KwangPlayer.GetCardText());
            }
        }
        private static void DDaengJabE(List<Player> players, ref List<Player> winlosePlayer)
        {
            Player DJE = null;
            Player DDaengePlayer = null;
            DJE = CheckDJE(players);
            DDaengePlayer = CheckDDaeng(players);
            if (DJE != null && DDaengePlayer != null)
            {
                winlosePlayer.Remove(DJE);
                winlosePlayer.Remove(DDaengePlayer);
                winlosePlayer.Add(DDaengePlayer);
                winlosePlayer.Insert(0, DJE);
                Console.WriteLine("땡잡이 발동");
                Console.WriteLine("땡잡이 : " + DJE.Name + "패 : " + DJE.GetCardText());
                Console.WriteLine("땡 : " + DDaengePlayer.Name + "패 : " + DDaengePlayer.GetCardText());
            }
        }
        #endregion


        private static bool IsAnyoneOring(List<Player> players)
        {
            foreach (Player player in players)
                if (player.Money < BettingMoney)
                    return true;

            return false;
        }
    }
}
