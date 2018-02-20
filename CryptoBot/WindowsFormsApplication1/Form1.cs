using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        public class StrategyField
        {
            //1 - price, 2 - count
            public Dictionary<decimal, decimal> Data = new Dictionary<decimal, decimal>();

            public override string ToString()
            {
                string temp = "";

                foreach (var i in Data)
                {
                    temp += "цена: " + i.Key + " Количество: " + i.Value + '\n';
                }
                return temp;
            }
        }
        public class MainStrategy
        {
            public string MarketName = "";
            public string BuyStockEX = "";
            public string SellStockEX = "";
            public StrategyField StrategyBuy = new StrategyField();
            public StrategyField StrategySell = new StrategyField();
        }
        public static List<MainStrategy> CoinCompare(Dictionary<string, TransforfField> arg1, Dictionary<string, TransforfField> arg2, string mainStockEx, string secoondStockEx)
        {

            Dictionary<string, TransforfField> main = arg1.Where(d => arg2.ContainsKey(d.Key)).ToDictionary(x => x.Key, y => new TransforfField() { asks = y.Value.asks.Take(20).ToDictionary(c => c.Key, v => v.Value), bids = y.Value.bids.Take(30).ToDictionary(c => c.Key, v => v.Value) });
            Dictionary<string, TransforfField> second = arg2.Where(d => arg1.ContainsKey(d.Key)).ToDictionary(x => x.Key, y => new TransforfField() { asks = y.Value.asks.Take(20).ToDictionary(c => c.Key, v => v.Value), bids = y.Value.bids.Take(30).ToDictionary(c => c.Key, v => v.Value) });
            #region сравнение валют и запись из в стратегию

            List<MainStrategy> MainSt = new List<MainStrategy>();

            foreach (KeyValuePair<string, TransforfField> i in main)
            {
                TransforfField temp;

                if (second.ContainsKey(i.Key))
                {
                    temp = second[i.Key];
                }
                else
                {
                    continue;
                }
                if (temp.asks.Count == 0)
                {
                    continue;
                }
                if (temp.bids.Count == 0)
                {
                    continue;
                }
                if (i.Value.asks.Count == 0)
                {
                    continue;
                }
                if (i.Value.bids.Count == 0)
                {
                    continue;
                }
                if (IsProfit(temp.bids.First().Key, i.Value.asks.First().Key))
                {
                    MainStrategy TempSt = new MainStrategy();
                    TempSt.MarketName = i.Key;
                    TempSt.BuyStockEX = mainStockEx;
                    TempSt.SellStockEX = secoondStockEx;
                    if (temp.asks.Count == 0)
                    {
                        continue;
                    }
                    if (temp.bids.Count == 0)
                    {
                        continue;
                    }
                    if (i.Value.asks.Count == 0)
                    {
                        continue;
                    }
                    if (i.Value.bids.Count == 0)
                    {
                        continue;
                    }
                    //buy on [livecoin] main sell on  [poloniex] several
                    while (IsProfit(temp.bids.First().Key, i.Value.asks.First().Key))
                    {

                        if (i.Value.asks.First().Value > temp.bids.First().Value)
                        {
                            //записать в стратегию значение цены и количество в цикле пока профит
                            if (TempSt.StrategyBuy.Data.ContainsKey(i.Value.asks.First().Key))
                            {
                                TempSt.StrategyBuy.Data[i.Value.asks.First().Key] += temp.bids.First().Value;
                            }
                            else
                            {
                                TempSt.StrategyBuy.Data.Add(i.Value.asks.First().Key, temp.bids.First().Value);
                            }
                            if (TempSt.StrategySell.Data.ContainsKey(temp.bids.First().Key))
                            {
                                TempSt.StrategySell.Data[temp.bids.First().Key] += temp.bids.First().Value;
                            }
                            else
                            {
                                TempSt.StrategySell.Data.Add(temp.bids.First().Key, temp.bids.First().Value);
                            }

                            //отнять в списках большее значение
                            i.Value.asks[i.Value.asks.First().Key] -= temp.bids.First().Value;
                            //удалить из списков меньшее значение
                            temp.bids.Remove(temp.bids.First().Value);

                        }
                        else if (i.Value.asks.First().Value < temp.bids.First().Value)
                        {
                            //записать в стратегию значение цены и количество
                            if (TempSt.StrategyBuy.Data.ContainsKey(i.Value.asks.First().Key))
                            {
                                TempSt.StrategyBuy.Data[i.Value.asks.First().Key] += i.Value.asks.First().Value;
                            }
                            else
                            {
                                TempSt.StrategyBuy.Data.Add(i.Value.asks.First().Key, i.Value.asks.First().Value);
                            }
                            if (TempSt.StrategySell.Data.ContainsKey(temp.bids.First().Key))
                            {
                                TempSt.StrategySell.Data[temp.bids.First().Key] += i.Value.asks.First().Value;
                            }
                            else
                            {
                                TempSt.StrategySell.Data.Add(temp.bids.First().Key, i.Value.asks.First().Value);
                            }
                            //отнять в списках большее значение
                            temp.bids[temp.bids.First().Key] -= i.Value.asks.First().Value;
                            //удалить из списков меньшее значение
                            i.Value.asks.Remove(i.Value.asks.First().Key);
                        }
                        if (temp.asks.Count == 0)
                        {
                            break;
                        }
                        if (temp.bids.Count == 0)
                        {
                            break;
                        }
                        if (i.Value.asks.Count == 0)
                        {
                            break;
                        }
                        if (i.Value.bids.Count == 0)
                        {
                            break;
                        }
                        //равные значения будут нужны для возврата валюты 
                        //else if (temp.bids.First()[1] == i.Value.asks.First()[1])
                        //{
                        //    //записать в стратегию значение цены и количество
                        //    MainSt.StrategyBuy.Data[temp.bids.First()[0]] += i.Value.asks.First()[1];
                        //    MainSt.StrategySell.Data[i.Value.asks.First()[0]] += i.Value.asks.First()[1];

                        //    //удалить из списков меньшее значение
                        //    temp.bids.First().Clear();
                        //    i.Value.asks.First().Clear();
                        //}

                    }
                    MainSt.Add(TempSt);
                }
                else if (IsProfit(i.Value.bids.First().Key, temp.asks.First().Key))
                {
                    //buy on [poloniex] main sell on [livecoin] several
                    MainStrategy TempSt = new MainStrategy();
                    TempSt.MarketName = i.Key;
                    TempSt.BuyStockEX = secoondStockEx;
                    TempSt.SellStockEX = mainStockEx;

                    //buy on [livecoin] main sell on  [poloniex] several
                    while (IsProfit(i.Value.bids.First().Key, temp.asks.First().Key))
                    {

                        if (temp.asks.First().Value > i.Value.bids.First().Value)
                        {
                            //записать в стратегию значение цены и количество в цикле пока профит
                            if (TempSt.StrategyBuy.Data.ContainsKey(temp.asks.First().Key))
                            {
                                TempSt.StrategyBuy.Data[temp.asks.First().Key] += i.Value.bids.First().Value;
                            }
                            else
                            {
                                TempSt.StrategyBuy.Data.Add(temp.asks.First().Key, i.Value.bids.First().Value);
                            }
                            if (TempSt.StrategySell.Data.ContainsKey(i.Value.bids.First().Key))
                            {
                                TempSt.StrategySell.Data[i.Value.bids.First().Key] += i.Value.bids.First().Value;
                            }
                            else
                            {
                                TempSt.StrategySell.Data.Add(i.Value.bids.First().Key, i.Value.bids.First().Value);
                            }

                            //отнять в списках большее значение
                            temp.asks[temp.asks.First().Key] -= -i.Value.bids.First().Value;
                            //удалить из списков меньшее значение
                            i.Value.bids.Remove(i.Value.bids.First().Key);


                        }
                        else if (temp.asks.First().Value < i.Value.bids.First().Value)
                        {
                            //записать в стратегию значение цены и количество
                            if (TempSt.StrategyBuy.Data.ContainsKey(temp.asks.First().Key))
                            {
                                TempSt.StrategyBuy.Data[temp.asks.First().Key] += temp.asks.First().Value;
                            }
                            else
                            {
                                TempSt.StrategyBuy.Data.Add(temp.asks.First().Key, temp.asks.First().Value);
                            }
                            if (TempSt.StrategySell.Data.ContainsKey(i.Value.bids.First().Key))
                            {
                                TempSt.StrategySell.Data[i.Value.bids.First().Key] += temp.asks.First().Value;
                            }
                            else
                            {
                                TempSt.StrategySell.Data.Add(i.Value.bids.First().Key, temp.asks.First().Value);
                            }
                            //отнять в списках большее значение
                            i.Value.bids[i.Value.bids.First().Key] -= temp.asks.First().Value;
                            //удалить из списков меньшее значение
                            temp.asks.Remove(temp.asks.First().Key);
                        }
                        if (temp.asks.Count == 0)
                        {
                            break;
                        }
                        if (temp.bids.Count == 0)
                        {
                            break;
                        }
                        if (i.Value.asks.Count == 0)
                        {
                            break;
                        }
                        if (i.Value.bids.Count == 0)
                        {
                            break;
                        }
                    }
                    MainSt.Add(TempSt);
                }

            }
            return MainSt;
            #endregion
        }
        public static bool IsProfit(decimal Bids, decimal Asks)
        {
            if (Bids - Asks > 0)
            {
                return true;
            }
            return false;
        }
        public static void CheakCoinsInfo(ref Dictionary<string, TransformInfo> first, Dictionary<string, TransformInfo> second, Dictionary<string, TransformInfo> therd, Dictionary<string, TransformInfo> fours, Dictionary<string, TransformInfo> fifs)
        {
            var temp = first.ToList();
            bool flag = true;
            foreach (var i in temp)
            {
                var Key = i.Key.Contains('_') ? i.Key.Split('_')[0].ToUpper() : i.Key;
                var t = second.ContainsKey(Key);
                if (t)
                {
                    continue;
                }
                else
                {
                    t = therd.ContainsKey(Key);
                    if (t)
                    {
                        continue;
                    }
                    else
                    {
                        t = fours.ContainsKey(Key);
                        if (t)
                        {
                            continue;
                        }
                        else
                        {
                            if (flag)
                            {
                                var t2 = fifs.Where(d => d.Key.Split('_').First().ToUpper() == i.Key).ToList();
                                if (t2.Count == 0)
                                {
                                    first.Remove(i.Key);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                t = fifs.ContainsKey(Key);
                                if (t)
                                {
                                    continue;
                                }
                            }

                        }
                    }
                }
            }

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LiveCoinInfo LiveInfo = new LiveCoinInfo();
            PoloniexCoinsInfo PolInfo = new PoloniexCoinsInfo();
            CryptoCoinInfo CryptInfo = new CryptoCoinInfo();
            BittrexCoinInfo BitInfo = new BittrexCoinInfo();
            YobitCoinIdenteti YouInfo = new YobitCoinIdenteti();
            Console.WriteLine("get info");
            CheakCoinsInfo(ref CryptInfo.Data, LiveInfo.Data, PolInfo.Data, BitInfo.Data, YouInfo.Data);
            CheakCoinsInfo(ref BitInfo.Data, LiveInfo.Data, PolInfo.Data, CryptInfo.Data, YouInfo.Data);
            CheakCoinsInfo(ref YouInfo.Data, LiveInfo.Data, PolInfo.Data, CryptInfo.Data, BitInfo.Data);


            LiveCoinCoins live = new LiveCoinCoins();
            PoloniexCoins poloniex = new PoloniexCoins();
            CryptoCoins crypto = new CryptoCoins(CryptInfo.Data);
            BittrexCoin bittrex = new BittrexCoin(BitInfo.Data);
            YobitCoins Youibit = new YobitCoins(YouInfo);

            //Дописать сравнение по имеющеййся на бирже валюте и констрктор
            List<MainStrategy> t = CoinCompare(live.Data, poloniex.Data, "live", "poloniex");
            List<MainStrategy> t2 = CoinCompare(live.Data, crypto.Data, "live", "crypto");
            List<MainStrategy> t3 = CoinCompare(live.Data, bittrex.Data, "live", "bittrex");
            List<MainStrategy> t4 = CoinCompare(bittrex.Data, poloniex.Data, "bittrex", "poloniex");
            List<MainStrategy> t5 = CoinCompare(crypto.Data, poloniex.Data, "crypto", "poloniex");
            List<MainStrategy> t6 = CoinCompare(crypto.Data, bittrex.Data, "crypto", "bittrex");
            List<MainStrategy> t7 = CoinCompare(Youibit.Data, live.Data, "Youibit", "live");
            List<MainStrategy> t8 = CoinCompare(Youibit.Data, poloniex.Data, "Youibit", "poloniex");
            List<MainStrategy> t9 = CoinCompare(Youibit.Data, crypto.Data, "live", "crypto");
            List<MainStrategy> t10 = CoinCompare(Youibit.Data, bittrex.Data, "live", "bittrex");

            AddToColums(t);
            AddToColums(t2);
            AddToColums(t3);
            AddToColums(t4);
            AddToColums(t5);
            AddToColums(t6);
            AddToColums(t7);
            AddToColums(t8);
            AddToColums(t9);
            AddToColums(t10);
        }

        public void AddToColums(List<MainStrategy> arg)
        {
            foreach (var item in arg)
            {
                listView1.Columns[0].Text = item.MarketName;
                listView1.Columns[1].Text = item.BuyStockEX;
                listView1.Columns[2].Text = item.SellStockEX;
                listView1.Columns[3].Text = (item.StrategyBuy.Data.Sum(d => d.Value) / item.StrategyBuy.Data.Sum(d => d.Key)).ToString();
                listView1.Columns[4].Text = (item.StrategySell.Data.Sum(d => d.Value) / item.StrategySell.Data.Sum(d => d.Key)).ToString();
                listView1.Columns[5].Text = item.StrategySell.Data.Sum(d => d.Value).ToString();
                var BlackProfit = (item.StrategySell.Data.First().Key - item.StrategyBuy.Data.First().Key) * item.StrategyBuy.Data.First().Value;
                var StockFee = ((item.StrategyBuy.Data.Sum(d => d.Value) * (item.StrategyBuy.Data.Sum(d => d.Key) / item.StrategyBuy.Data.Count)) * (decimal)0.004);
                var Bitcoin = 1700;
                listView1.Columns[6].Text = ((BlackProfit - StockFee) * Bitcoin).ToString();
            }

        }
    }
}
