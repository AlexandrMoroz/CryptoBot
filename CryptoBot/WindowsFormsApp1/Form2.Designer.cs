namespace WindowsFormsApp1
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OrdersToBuy = new System.Windows.Forms.Label();
            this.BuylistView = new System.Windows.Forms.ListView();
            this.PriceCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Quantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BTCequals = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SumBTC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OrdersToSell = new System.Windows.Forms.Label();
            this.SelllistView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BTCBuyTextBox = new System.Windows.Forms.TextBox();
            this.BTCSellTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BuyFeeTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SellFeeTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ProfitTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.BTCContains = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OrdersToBuy
            // 
            this.OrdersToBuy.AutoSize = true;
            this.OrdersToBuy.Location = new System.Drawing.Point(27, 9);
            this.OrdersToBuy.Name = "OrdersToBuy";
            this.OrdersToBuy.Size = new System.Drawing.Size(69, 13);
            this.OrdersToBuy.TabIndex = 0;
            this.OrdersToBuy.Text = "OrdersToBuy";
            // 
            // BuylistView
            // 
            this.BuylistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PriceCol,
            this.Quantity,
            this.BTCequals,
            this.SumBTC});
            this.BuylistView.Location = new System.Drawing.Point(12, 25);
            this.BuylistView.Name = "BuylistView";
            this.BuylistView.Size = new System.Drawing.Size(319, 97);
            this.BuylistView.TabIndex = 1;
            this.BuylistView.UseCompatibleStateImageBehavior = false;
            this.BuylistView.View = System.Windows.Forms.View.Details;
            // 
            // PriceCol
            // 
            this.PriceCol.Text = "Price";
            // 
            // Quantity
            // 
            this.Quantity.Width = 108;
            // 
            // BTCequals
            // 
            this.BTCequals.Text = "BTC";
            this.BTCequals.Width = 83;
            // 
            // SumBTC
            // 
            this.SumBTC.Text = "SumBTC";
            // 
            // OrdersToSell
            // 
            this.OrdersToSell.AutoSize = true;
            this.OrdersToSell.Location = new System.Drawing.Point(30, 129);
            this.OrdersToSell.Name = "OrdersToSell";
            this.OrdersToSell.Size = new System.Drawing.Size(68, 13);
            this.OrdersToSell.TabIndex = 2;
            this.OrdersToSell.Text = "OrdersToSell";
            // 
            // SelllistView
            // 
            this.SelllistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.SelllistView.Location = new System.Drawing.Point(12, 145);
            this.SelllistView.Name = "SelllistView";
            this.SelllistView.Size = new System.Drawing.Size(319, 97);
            this.SelllistView.TabIndex = 3;
            this.SelllistView.UseCompatibleStateImageBehavior = false;
            this.SelllistView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Price";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 86;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "BTC";
            this.columnHeader3.Width = 103;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "SumBTC";
            // 
            // BTCBuyTextBox
            // 
            this.BTCBuyTextBox.Location = new System.Drawing.Point(393, 34);
            this.BTCBuyTextBox.Name = "BTCBuyTextBox";
            this.BTCBuyTextBox.Size = new System.Drawing.Size(100, 20);
            this.BTCBuyTextBox.TabIndex = 4;
            // 
            // BTCSellTextBox
            // 
            this.BTCSellTextBox.Location = new System.Drawing.Point(393, 84);
            this.BTCSellTextBox.Name = "BTCSellTextBox";
            this.BTCSellTextBox.Size = new System.Drawing.Size(100, 20);
            this.BTCSellTextBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(390, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "BTCBuy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(393, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "BTCSell";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(513, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "BTCBuyFee";
            // 
            // BuyFeeTextBox
            // 
            this.BuyFeeTextBox.Location = new System.Drawing.Point(513, 34);
            this.BuyFeeTextBox.Name = "BuyFeeTextBox";
            this.BuyFeeTextBox.Size = new System.Drawing.Size(100, 20);
            this.BuyFeeTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(513, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "BTCSellFee";
            // 
            // SellFeeTextBox
            // 
            this.SellFeeTextBox.Location = new System.Drawing.Point(513, 84);
            this.SellFeeTextBox.Name = "SellFeeTextBox";
            this.SellFeeTextBox.Size = new System.Drawing.Size(100, 20);
            this.SellFeeTextBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(513, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Profit";
            // 
            // ProfitTextBox
            // 
            this.ProfitTextBox.Location = new System.Drawing.Point(513, 129);
            this.ProfitTextBox.Name = "ProfitTextBox";
            this.ProfitTextBox.Size = new System.Drawing.Size(100, 20);
            this.ProfitTextBox.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(393, 218);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Ok_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(538, 218);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(393, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "BTCStockContains";
            // 
            // BTCContains
            // 
            this.BTCContains.Location = new System.Drawing.Point(393, 129);
            this.BTCContains.Name = "BTCContains";
            this.BTCContains.Size = new System.Drawing.Size(100, 20);
            this.BTCContains.TabIndex = 16;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 299);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BTCContains);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ProfitTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SellFeeTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BuyFeeTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTCSellTextBox);
            this.Controls.Add(this.BTCBuyTextBox);
            this.Controls.Add(this.SelllistView);
            this.Controls.Add(this.OrdersToSell);
            this.Controls.Add(this.BuylistView);
            this.Controls.Add(this.OrdersToBuy);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.Shown += new System.EventHandler(this.Form2_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OrdersToBuy;
        private System.Windows.Forms.ListView BuylistView;
        private System.Windows.Forms.ColumnHeader PriceCol;
        private System.Windows.Forms.ColumnHeader Quantity;
        private System.Windows.Forms.ColumnHeader BTCequals;
        private System.Windows.Forms.ColumnHeader SumBTC;
        private System.Windows.Forms.Label OrdersToSell;
        private System.Windows.Forms.ListView SelllistView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox BTCBuyTextBox;
        private System.Windows.Forms.TextBox BTCSellTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BuyFeeTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SellFeeTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ProfitTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox BTCContains;
    }
}