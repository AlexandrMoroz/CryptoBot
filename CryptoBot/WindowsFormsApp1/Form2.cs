using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        MainStrategy mainstratagy;
        CancellationToken token = Task.Factory.CancellationToken;
        TaskScheduler context;
        ProfitCalc profit;
        II BotAi;

        public Form2(MainStrategy arg)
        {
            context = TaskScheduler.FromCurrentSynchronizationContext();
            mainstratagy = arg;
            profit = new ProfitCalc(arg);
            BotAi = new II(arg);
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        public void SetForm()
        {
            decimal BtcQuantity = 0;
            foreach (var item in profit.GetBuyStrategy(false))
            {
                ListViewItem item2 = new ListViewItem(item.Key.ToString());
                item2.SubItems.Add(item.Value.ToString());

                BtcQuantity += item.Key * item.Value;
                item2.SubItems.Add((item.Key * item.Value).ToString());
                item2.SubItems.Add(BtcQuantity.ToString());

                Task.Factory.StartNew(() => { BuylistView.Items.Add(item2); }, token, TaskCreationOptions.None, context);
            }
            foreach (var item in profit.StrategySell)
            {
                ListViewItem item2 = new ListViewItem(item.Key.ToString());
                item2.SubItems.Add(item.Value.ToString());

                BtcQuantity += item.Key * item.Value;
                item2.SubItems.Add((item.Key * item.Value).ToString());
                item2.SubItems.Add(BtcQuantity.ToString());

                Task.Factory.StartNew(() => { SelllistView.Items.Add(item2); }, token, TaskCreationOptions.None, context);
            }
            BTCBuyTextBox.Text = profit.BuyBTC.ToString();
            BTCSellTextBox.Text = profit.SellBTC.ToString();
            BuyFeeTextBox.Text = profit.BuyFee.ToString();
            SellFeeTextBox.Text = profit.SellFee.ToString();
            BTCContains.Text = profit.GetBTCBallans(mainstratagy).ToString();
            ProfitTextBox.Text = profit.Profit.ToString();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            SetForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Ok_Click(object sender, EventArgs e)
        {

        }
    }
    }
