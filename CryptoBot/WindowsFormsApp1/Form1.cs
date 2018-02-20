using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.IO;
using WindowsFormsApp1.AIClass;

namespace WindowsFormsApp1
{
    //xcp 36 часов
    //etc 30 минут

    public partial class Form1 : Form
    {
        Nullable<decimal> Ballans;
        List<MainStrategy> MainSt = new List<MainStrategy>();
        TaskScheduler context;
        decimal BTCCost=10000;
        public Form1()
        {
            context = TaskScheduler.FromCurrentSynchronizationContext();
            InitializeComponent();
        }



        public static void CheakCoinsInfo(ref Dictionary<string, TransformInfo> first, Dictionary<string, TransformInfo> second, Dictionary<string, TransformInfo> therd, Dictionary<string, TransformInfo> fours, Dictionary<string, TransformInfo> fifs)
        {
            var temp = first.ToList();
            bool flag = true;
            foreach (var i in temp)
            {
                var Key = i.Key.Contains('_') ? i.Key.Split('_')[0].ToUpper() : i.Key;
                var t = second!=null?second.ContainsKey(Key):false;
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
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void UpdateCoins()
        {
            var MainCoins = new List<string>() { "BTC", "USDT"};
            var Coins = new List<string>() {"ETH", "ETC", "ZEC","LTC"};

            var Temp = new List<KeyValuePair<string, string>>();
            foreach (var item in Coins)
            {
                foreach (var item2 in MainCoins)
                {
                    Temp.Add(new KeyValuePair<string, string>(item2,item));
                }
            }
            LiveCoin livecoin = new LiveCoin();
            var LiveOrders = livecoin.Orders.GetOrdersAsync(Temp);
            //Poloniex poloniex = new Poloniex();
            //var PoloniexOrders = poloniex.Orders.GetOrdersAsync(Coins);
            Cryptopia crypto = new Cryptopia();
            var CryptOrders = crypto.Orders.GetOrdersAsync(Temp);
            Bittrex bittrex = new Bittrex();
            var BittrexOrders = bittrex.Orders.GetOrdersAsync(Temp);
            //MainSt.Add(CompairCoins.CompareByDollar(CryptOrders.Result.Where(x=>x.Key.Contains("USDT")), CryptOrders.Result.Where(x => x.Key.Contains("BTC")), crypto))
            Yobit yobit = new Yobit();
            var YobitOrders = yobit.Orders.GetOrdersAsync(Temp);
            Exmo exmo = new Exmo();
            var ExmoOrders = exmo.Orders.GetOrdersAsync(Temp);
            Gate gate = new Gate();
            var GateOrders = gate.Orders.GetOrdersAsync(Temp);

            //MainSt.AddRange(CompairCoins.CoinCompare(LiveOrders.Result, PoloniexOrders.Result, livecoin, poloniex));
            MainSt.AddRange(CompairCoins.CoinCompare(LiveOrders.Result, CryptOrders.Result, livecoin, crypto));
            MainSt.AddRange(CompairCoins.CoinCompare(LiveOrders.Result, BittrexOrders.Result, livecoin, bittrex));
            MainSt.AddRange(CompairCoins.CoinCompare(LiveOrders.Result, YobitOrders.Result, livecoin, yobit));
            MainSt.AddRange(CompairCoins.CoinCompare(LiveOrders.Result, ExmoOrders.Result, livecoin, exmo));
            MainSt.AddRange(CompairCoins.CoinCompare(LiveOrders.Result, GateOrders.Result, livecoin, gate));

            //MainSt.AddRange(CompairCoins.CoinCompare(BittrexOrders.Result, PoloniexOrders.Result, bittrex, poloniex));
            MainSt.AddRange(CompairCoins.CoinCompare(BittrexOrders.Result, CryptOrders.Result, bittrex, crypto));
            MainSt.AddRange(CompairCoins.CoinCompare(BittrexOrders.Result, YobitOrders.Result, bittrex, yobit));
            MainSt.AddRange(CompairCoins.CoinCompare(BittrexOrders.Result, ExmoOrders.Result, bittrex, exmo));
            MainSt.AddRange(CompairCoins.CoinCompare(BittrexOrders.Result, GateOrders.Result, bittrex, gate));

            //MainSt.AddRange(CompairCoins.CoinCompare(CryptOrders.Result, PoloniexOrders.Result, crypto, poloniex));
            MainSt.AddRange(CompairCoins.CoinCompare(CryptOrders.Result, YobitOrders.Result, crypto, yobit));
            MainSt.AddRange(CompairCoins.CoinCompare(CryptOrders.Result, ExmoOrders.Result, crypto, exmo));
            MainSt.AddRange(CompairCoins.CoinCompare(CryptOrders.Result, GateOrders.Result, crypto, gate));


            //MainSt.AddRange(CompairCoins.CoinCompare(YobitOrders.Result, PoloniexOrders.Result, yobit, poloniex));
            MainSt.AddRange(CompairCoins.CoinCompare(YobitOrders.Result, ExmoOrders.Result, yobit, exmo));
            MainSt.AddRange(CompairCoins.CoinCompare(YobitOrders.Result, GateOrders.Result, yobit, gate));

            MainSt.AddRange(CompairCoins.CoinCompare(ExmoOrders.Result, GateOrders.Result, exmo, gate));
            //MainSt.AddRange(CompairCoins.CoinCompare(ExmoOrders.Result, PoloniexOrders.Result, exmo, poloniex));

            //MainSt.AddRange(CompairCoins.CoinCompare(GateOrders.Result, PoloniexOrders.Result, gate, poloniex));


        }
        public void UpdateList()
        {
            MainSt.Clear();
            UpdateCoins();
            var token = Task.Factory.CancellationToken;
            Task.Factory.StartNew(() => {
                listView1.Clear();
                listView1.Columns.Add("Market", "Market");
                listView1.Columns.Add("BuyStock", "BuyStock");
                listView1.Columns.Add("SellStock", "SellStock");
                listView1.Columns.Add("BuyPrice", "BuyPrice");
                listView1.Columns.Add("SellPrice", "SellPrice");
                listView1.Columns.Add("Quantity", "Quantity");
                listView1.Columns.Add("AllProfit", "AllProfit");
                listView1.Columns.Add("StockProfit", "StockProfit");
            }, token, TaskCreationOptions.None, context);
            
            foreach (var item in MainSt)
            {
                ListViewItem item2 = new ListViewItem(item.MarketName);
                item2.SubItems.Add(item.BuyStockEX.StockName);
                item2.SubItems.Add(item.SellStockEX.StockName);


                decimal AverBuyPrice = Math.Round(item.StrategyBuy.Average(x => x.Key), 8);
                decimal AverSellPrice = Math.Round(item.StrategySell.Average(x => x.Key), 8);
                decimal Quantity = item.StrategyBuy.Sum(d => d.Value);

                item2.SubItems.Add(AverBuyPrice.ToString());
                item2.SubItems.Add(AverSellPrice.ToString());
                item2.SubItems.Add(Quantity.ToString());

                decimal BlackProfit = Math.Round((AverSellPrice - AverBuyPrice) * Quantity, 8);
                decimal FeeCost = Math.Round(((Quantity * AverBuyPrice) * (decimal)0.003), 8);

                item2.SubItems.Add(((BlackProfit - FeeCost) * (item.MarketName.Contains("USD") ? 0 : BTCCost)).ToString());
                item2.SubItems.Add("0");
                Task.Factory.StartNew(() => { listView1.Items.Add(item2); }, token, TaskCreationOptions.None, context);
            }
        }
        public void  Updatetimer(Object source, ElapsedEventArgs e)
        {
            var token = Task.Factory.CancellationToken;
            Task.Factory.StartNew(() => {
                timer.Text = (Convert.ToInt32(timer.Text) + 1).ToString();
                }, token, TaskCreationOptions.None, context);
        }
        private  void button1_Click(object sender, EventArgs e)
        {
            timer.Text = "0";
            Task h = Task.Factory.StartNew(() =>
            {
                System.Timers.Timer a = new System.Timers.Timer(1000);
                a.Elapsed += Updatetimer;
                a.Start();
                UpdateList();
                a.Stop();
            });
            
       
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MarketName = listView1.FocusedItem.Text; 
            var item = listView1.FocusedItem.SubItems[1].Text;
            var item2 = listView1.FocusedItem.SubItems[2].Text;
            var strategy = MainSt.Where(x => x.MarketName == MarketName&& x.BuyStockEX.StockName == item && x.SellStockEX.StockName == item2).FirstOrDefault();
            if (strategy!=null)
            {
                var form = new Form2(strategy);
                form.Show();
            }
            else
            {
                MessageBox.Show("No selected items");
            }
            
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
            // Call the sort method to manually sort.
            listView1.Sort();
        }
        class ListViewItemComparer : IComparer
        {
            private int col;
            public ListViewItemComparer()
            {
                col = 0;
            }
            public ListViewItemComparer(int column)
            {
                col = column;
            }
            public int Compare(object x, object y)
            {
                if (col >= 3)
                {
                    int returnVal = -1;
                    returnVal = decimal.Compare(Convert.ToDecimal(((ListViewItem)x).SubItems[col].Text),
                    Convert.ToDecimal(((ListViewItem)y).SubItems[col].Text));
                    return returnVal;
                }
                else
                {
                    int returnVal = -1;
                    returnVal = string.Compare(((ListViewItem)x).SubItems[col].Text,
                    ((ListViewItem)y).SubItems[col].Text);
                    return returnVal;
                }
            }
        }
    }
}
