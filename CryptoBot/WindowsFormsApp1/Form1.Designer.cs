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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.minimize = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TimerView = new System.Windows.Forms.Label();
            this.USDBut = new System.Windows.Forms.Button();
            this.TranzBut = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.panel1.Controls.Add(this.minimize);
            this.panel1.Controls.Add(this.exit);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.USDBut);
            this.panel1.Controls.Add(this.TranzBut);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel1.Size = new System.Drawing.Size(279, 710);
            this.panel1.TabIndex = 0;
            // 
            // minimize
            // 
            this.minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimize.ForeColor = System.Drawing.Color.Silver;
            this.minimize.Image = ((System.Drawing.Image)(resources.GetObject("minimize.Image")));
            this.minimize.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.minimize.Location = new System.Drawing.Point(58, 658);
            this.minimize.Name = "minimize";
            this.minimize.Size = new System.Drawing.Size(40, 40);
            this.minimize.TabIndex = 5;
            this.minimize.UseVisualStyleBackColor = true;
            this.minimize.Click += new System.EventHandler(this.minimize_Click);
            // 
            // exit
            // 
            this.exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit.ForeColor = System.Drawing.Color.Silver;
            this.exit.Image = ((System.Drawing.Image)(resources.GetObject("exit.Image")));
            this.exit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.exit.Location = new System.Drawing.Point(12, 658);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(40, 40);
            this.exit.TabIndex = 4;
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.TimerView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(279, 52);
            this.panel2.TabIndex = 3;
            // 
            // TimerView
            // 
            this.TimerView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TimerView.Font = new System.Drawing.Font("Roboto Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TimerView.ForeColor = System.Drawing.Color.Silver;
            this.TimerView.Location = new System.Drawing.Point(0, 0);
            this.TimerView.Name = "TimerView";
            this.TimerView.Size = new System.Drawing.Size(279, 52);
            this.TimerView.TabIndex = 2;
            this.TimerView.Text = "0";
            this.TimerView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // USDBut
            // 
            this.USDBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.USDBut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.USDBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.USDBut.Font = new System.Drawing.Font("Roboto Thin", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.USDBut.ForeColor = System.Drawing.Color.Silver;
            this.USDBut.Location = new System.Drawing.Point(1, 133);
            this.USDBut.Name = "USDBut";
            this.USDBut.Size = new System.Drawing.Size(278, 69);
            this.USDBut.TabIndex = 1;
            this.USDBut.Text = "USD-COIN-BTC Algoritme";
            this.USDBut.UseVisualStyleBackColor = false;
            this.USDBut.Click += new System.EventHandler(this.USDBut_Click);
            // 
            // TranzBut
            // 
            this.TranzBut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.TranzBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TranzBut.Font = new System.Drawing.Font("Roboto Thin", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TranzBut.ForeColor = System.Drawing.Color.Silver;
            this.TranzBut.Location = new System.Drawing.Point(0, 58);
            this.TranzBut.Name = "TranzBut";
            this.TranzBut.Size = new System.Drawing.Size(278, 69);
            this.TranzBut.TabIndex = 0;
            this.TranzBut.Text = "Tranzaction Algoritme";
            this.TranzBut.UseVisualStyleBackColor = false;
            this.TranzBut.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.listView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(277, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1171, 710);
            this.panel3.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Roboto Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1171, 710);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1448, 710);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button TranzBut;
        private System.Windows.Forms.Label TimerView;
        private System.Windows.Forms.Button USDBut;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button minimize;
    }
}

