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
using System.Timers;
using WindowsFormsApp1.AIClass;

namespace WindowsFormsApp1
{
    //xcp 36 часов
    //etc 30 минут

    public partial class Form1 : Form
    {
        Nullable<decimal> Ballans;
        List<MainStrategy> MainSt = new List<MainStrategy>();
        List<BTCToUSDStrategy> MainStUSD = new List<BTCToUSDStrategy>();
        TaskScheduler context;
        decimal BTCCost = 6600m;
        decimal USDTransactionFee = 0.06m;
        decimal StockFee = 0.004m;
        List<string> MainCoins = new List<string>() {  "USDT","USD","BTC"};
        List<string> Coins = new List<string>() { "ETH", "ETC", "ZEC", "LTC", "XRP","DOGE" };
        List<string> AllCoins = new List<string>();

        List<KeyValuePair<string, string>> Markets { get; set; }
  
        public Form1()
        {
            context = TaskScheduler.FromCurrentSynchronizationContext();
            InitializeComponent();
            Markets = new List<KeyValuePair<string, string>>();
            foreach (var item in Coins)
            {
                foreach (var item2 in MainCoins)
                {
                    Markets.Add(new KeyValuePair<string, string>(item2, item));
                }
            }

        }

             
        public void UpdateCoins()
        {
            
            var Cover = new List<OrdersCover2Stock>() {
                new OrdersCover2Stock(new LiveCoin(),Markets),
                new OrdersCover2Stock(new Cryptopia(),Markets),
                new OrdersCover2Stock(new Bittrex(),Markets),
                new OrdersCover2Stock(new Yobit(),Markets),
                new OrdersCover2Stock(new Exmo(),Markets),
                new OrdersCover2Stock(new Gate(),Markets)
            };

            CombineOrders(Cover);
        }
        public void CombineOrders(List<OrdersCover2Stock> arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                for (int j = i + 1; j < arr.Count; j++)
                {
                    MainSt.AddRange(CompairCoins.CoinCompare(arr[i].order.Result, arr[j].order.Result, arr[i].stock, arr[j].stock));
                }
            }
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

                decimal divided = item.StrategyBuy.Sum(x => x.Key * x.Value);
                decimal divider = item.StrategyBuy.Sum(x => x.Value);
                decimal AverBuyPrice = Math.Round(divided / divider, 8);

                decimal divided2 = item.StrategySell.Sum(x => x.Key * x.Value);
                decimal divider2 = item.StrategySell.Sum(x => x.Value);
                decimal AverSellPrice = Math.Round(divided2 / divider2, 8);

                decimal Quantity = item.StrategyBuy.Sum(d => d.Value);

                item2.SubItems.Add(AverBuyPrice.ToString());
                item2.SubItems.Add(AverSellPrice.ToString());
                item2.SubItems.Add(Quantity.ToString());

                decimal BlackProfit = Math.Round((AverSellPrice - AverBuyPrice) * Quantity, 8);
                decimal FeeCost = Math.Round(((Quantity * AverBuyPrice) * StockFee), 8);
                decimal Profit = (BlackProfit - FeeCost);

                if (item.MarketName.Split('-')[0] == "USDT")
                {
                    Profit -= 15m;
                }
                else if (item.MarketName.Split('-')[0] == "USD")
                {
                    Profit -= ((AverBuyPrice * Quantity) * USDTransactionFee);
                }

                item2.SubItems.Add(Profit.ToString());
                item2.SubItems.Add("0");
                Task.Factory.StartNew(() => { listView1.Items.Add(item2);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }, token, TaskCreationOptions.None, context);
            }
        }
        public void Updatetimer(Object source, ElapsedEventArgs e)
        {
            var token = Task.Factory.CancellationToken;
            Task.Factory.StartNew(() => {
                TimerView.Text = (Convert.ToInt32(TimerView.Text) + 1).ToString();
                }, token, TaskCreationOptions.None, context);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            TimerView.Text = "0";
            Task h = Task.Factory.StartNew(() =>
            {
                System.Timers.Timer a = new System.Timers.Timer(1000);
                a.Elapsed += Updatetimer;
                a.Start();
                UpdateList();
                a.Stop();
            });
        }

        private void USDBut_Click(object sender, EventArgs e)
        {
            TimerView.Text = "0";
            Task h = Task.Factory.StartNew(() =>
            {
                System.Timers.Timer a = new System.Timers.Timer(1000);
                a.Elapsed += Updatetimer;
                a.Start();
                UpdateListUSDAlg();
                a.Stop();
            });
        }
        //public string MarketName = "";
        //public Stock stock;
        //public decimal CoinBuyPrice;
        //public decimal CoinSellPrice;
        //public decimal BTCSellPrice;
        private void UpdateListUSDAlg()
        {
            MainStUSD.Clear();
            UpdateCoinsUSDAlg();
            var token = Task.Factory.CancellationToken;
            Task.Factory.StartNew(() => {
                listView1.Clear();
                listView1.Columns.Add("Coin", "Coin");
                listView1.Columns.Add("Stock", "Stock");
                listView1.Columns.Add("CoinBuyQuantity", "CoinBuyQuantity");
                listView1.Columns.Add("CoinSellPrice", "CoinSellPrice");
                listView1.Columns.Add("BTCSellPrice", "BTCSellPrice");
                listView1.Columns.Add("AllProfit", "AllProfit");
                listView1.Columns.Add("StockProfit", "StockProfit");

            }, token, TaskCreationOptions.None, context);

            foreach (var item in MainStUSD)
            {
                ListViewItem item2 = new ListViewItem(item.MarketName);
                item2.SubItems.Add(item.stock.StockName);

             
             
                item2.SubItems.Add(item.CoinBuyQuantity.ToString());
                item2.SubItems.Add(item.CoinSellPrice.ToString());
                item2.SubItems.Add(item.BTCSellPrice.ToString());
                var prof = item.CoinBuyQuantity - item.CoinBuyQuantity;
                item2.SubItems.Add(prof.ToString());
                Task.Factory.StartNew(() => {
                    listView1.Items.Add(item2);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }, token, TaskCreationOptions.None, context);
            }
        }
        private void UpdateCoinsUSDAlg()
        {
            var temp = new GateInfo().GetPairs(); 
            foreach(var item in temp)
            {
                if (!MainCoins.Contains(item.Split('_')[0]))
                {
                    AllCoins.Add(item.Split('_')[0]);
                }
            }
            AllCoins = AllCoins.Distinct().ToList();
            var Cover = new List<OrdersCoverOneStock>() {
                new OrdersCoverOneStock(new LiveCoin(),AllCoins,"USD","BTC"),
                //new OrdersCoverOneStock(new Cryptopia(),AllCoins,"BTC","USDT"),
                //new OrdersCoverOneStock(new Bittrex(),AllCoins,"USDT","BTC"),
                //new OrdersCoverOneStock(new Yobit(),AllCoins,"USD","BTC"),
                //new OrdersCoverOneStock(new Exmo(),AllCoins,"USDT","BTC"),
                //new OrdersCoverOneStock(new Gate(),AllCoins,"USDT","BTC")
            };

            UpdateCoins(Cover);
        }
        public void UpdateCoins(List<OrdersCoverOneStock> arr)
        {
            foreach (var item in arr)
            {
                MainStUSD.AddRange(CompairCoins.CompareByDollar(item.CoinMainCoin.Result, item.CoinValue.Result, item.MainCoinValue.Result, 0.01m, item.stock));
            }
            
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var MarketName = listView1.FocusedItem.Text;
            var item = listView1.FocusedItem.SubItems[1].Text;
            var item2 = listView1.FocusedItem.SubItems[2].Text;
            var strategy = MainSt.Where(x => x.MarketName == MarketName && x.BuyStockEX.StockName == item && x.SellStockEX.StockName == item2).FirstOrDefault();
            if (strategy != null)
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
            listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
   
}
