namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.Market = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BuyStock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SellStock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BuyPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SellPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Quantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Profit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.YobitBalans = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.LiveBal = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CryptoBalans = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BittrexBalans = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PoloniexBalans = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Market,
            this.BuyStock,
            this.SellStock,
            this.BuyPrice,
            this.SellPrice,
            this.Quantity,
            this.Profit});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1097, 495);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // Market
            // 
            this.Market.Text = "Market";
            this.Market.Width = 99;
            // 
            // BuyStock
            // 
            this.BuyStock.Text = "BuyStock";
            this.BuyStock.Width = 99;
            // 
            // SellStock
            // 
            this.SellStock.Text = "SellStock";
            this.SellStock.Width = 98;
            // 
            // BuyPrice
            // 
            this.BuyPrice.Text = "BuyPrice";
            this.BuyPrice.Width = 94;
            // 
            // SellPrice
            // 
            this.SellPrice.Text = "SellPrice";
            this.SellPrice.Width = 85;
            // 
            // Quantity
            // 
            this.Quantity.Text = "Quantity";
            this.Quantity.Width = 93;
            // 
            // Profit
            // 
            this.Profit.Text = "Profit";
            this.Profit.Width = 130;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.YobitBalans);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.LiveBal);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.CryptoBalans);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.BittrexBalans);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.PoloniexBalans);
            this.panel1.Controls.Add(this.timer);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(918, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 495);
            this.panel1.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "YobitBal";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(756, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 11;
            // 
            // YobitBalans
            // 
            this.YobitBalans.Location = new System.Drawing.Point(77, 143);
            this.YobitBalans.Name = "YobitBalans";
            this.YobitBalans.Size = new System.Drawing.Size(100, 20);
            this.YobitBalans.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "LiveBal";
            // 
            // LiveBal
            // 
            this.LiveBal.Location = new System.Drawing.Point(77, 108);
            this.LiveBal.Name = "LiveBal";
            this.LiveBal.Size = new System.Drawing.Size(100, 20);
            this.LiveBal.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "CryptoBal";
            // 
            // CryptoBalans
            // 
            this.CryptoBalans.Location = new System.Drawing.Point(77, 73);
            this.CryptoBalans.Name = "CryptoBalans";
            this.CryptoBalans.Size = new System.Drawing.Size(100, 20);
            this.CryptoBalans.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "BittrexBal";
            // 
            // BittrexBalans
            // 
            this.BittrexBalans.Location = new System.Drawing.Point(77, 40);
            this.BittrexBalans.Name = "BittrexBalans";
            this.BittrexBalans.Size = new System.Drawing.Size(100, 20);
            this.BittrexBalans.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "PoloniexBal";
            // 
            // PoloniexBalans
            // 
            this.PoloniexBalans.Location = new System.Drawing.Point(77, 5);
            this.PoloniexBalans.Name = "PoloniexBalans";
            this.PoloniexBalans.Size = new System.Drawing.Size(100, 20);
            this.PoloniexBalans.TabIndex = 2;
            // 
            // timer
            // 
            this.timer.AutoSize = true;
            this.timer.Location = new System.Drawing.Point(74, 449);
            this.timer.Name = "timer";
            this.timer.Size = new System.Drawing.Size(35, 13);
            this.timer.TabIndex = 1;
            this.timer.Text = "label1";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 465);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(179, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 495);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader Market;
        private System.Windows.Forms.ColumnHeader BuyStock;
        private System.Windows.Forms.ColumnHeader SellStock;
        private System.Windows.Forms.ColumnHeader BuyPrice;
        private System.Windows.Forms.ColumnHeader SellPrice;
        private System.Windows.Forms.ColumnHeader Quantity;
        private System.Windows.Forms.ColumnHeader Profit;
        private System.Windows.Forms.Label timer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox YobitBalans;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox LiveBal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CryptoBalans;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox BittrexBalans;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PoloniexBalans;
    }
}

