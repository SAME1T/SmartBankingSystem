namespace SmartBankingAutomation.Forms
{
    partial class CurrencyConverterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbAccounts = new System.Windows.Forms.ComboBox();
            this.clbCurrencies = new System.Windows.Forms.CheckedListBox();
            this.dgvRates = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAccounts = new System.Windows.Forms.Label();
            this.lblCurrencies = new System.Windows.Forms.Label();
            this.lblRates = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.nudAmount = new System.Windows.Forms.NumericUpDown();
            this.lblAmount = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbAccounts
            // 
            this.cmbAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccounts.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbAccounts.Location = new System.Drawing.Point(30, 95);
            this.cmbAccounts.Name = "cmbAccounts";
            this.cmbAccounts.Size = new System.Drawing.Size(300, 25);
            this.cmbAccounts.TabIndex = 0;
            this.cmbAccounts.SelectedIndexChanged += new System.EventHandler(this.cmbAccounts_SelectedIndexChanged);
            // 
            // clbCurrencies
            // 
            this.clbCurrencies.CheckOnClick = true;
            this.clbCurrencies.Enabled = false;
            this.clbCurrencies.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.clbCurrencies.FormattingEnabled = true;
            this.clbCurrencies.Items.AddRange(new object[] {
            "USD - Amerikan Doları",
            "EUR - Euro",
            "CHF - İsviçre Frangı",
            "GBP - İngiliz Sterlini", 
            "BHD - Bahreyn Dinarı",
            "SAR - Suudi Arabistan Riyali",
            "XAU - Altın"});
            this.clbCurrencies.Location = new System.Drawing.Point(30, 165);
            this.clbCurrencies.Name = "clbCurrencies";
            this.clbCurrencies.Size = new System.Drawing.Size(300, 220);
            this.clbCurrencies.TabIndex = 1;
            this.clbCurrencies.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbCurrencies_ItemCheck);
            // 
            // dgvRates
            // 
            this.dgvRates.AllowUserToAddRows = false;
            this.dgvRates.AllowUserToDeleteRows = false;
            this.dgvRates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRates.BackgroundColor = System.Drawing.Color.White;
            this.dgvRates.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvRates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRates.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dgvRates.Location = new System.Drawing.Point(360, 95);
            this.dgvRates.Name = "dgvRates";
            this.dgvRates.ReadOnly = true;
            this.dgvRates.RowTemplate.Height = 25;
            this.dgvRates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRates.Size = new System.Drawing.Size(700, 500);
            this.dgvRates.TabIndex = 2;
            // 
            // nudAmount
            // 
            this.nudAmount.DecimalPlaces = 2;
            this.nudAmount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.nudAmount.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudAmount.Location = new System.Drawing.Point(30, 425);
            this.nudAmount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAmount.Name = "nudAmount";
            this.nudAmount.Size = new System.Drawing.Size(300, 29);
            this.nudAmount.TabIndex = 9;
            this.nudAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudAmount.ThousandsSeparator = true;
            this.nudAmount.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAmount.Location = new System.Drawing.Point(30, 400);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(216, 20);
            this.lblAmount.TabIndex = 10;
            this.lblAmount.Text = "Çevrilecek Miktar (TL):";
            // 
            // btnConvert
            // 
            this.btnConvert.BackColor = System.Drawing.Color.FromArgb(52, 73, 126);
            this.btnConvert.Enabled = false;
            this.btnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvert.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnConvert.ForeColor = System.Drawing.Color.White;
            this.btnConvert.Location = new System.Drawing.Point(30, 470);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(145, 45);
            this.btnConvert.TabIndex = 11;
            this.btnConvert.Text = "💱 Çevir";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.btnRefresh.Enabled = false;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(25, 42, 86);
            this.btnRefresh.Location = new System.Drawing.Point(185, 470);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(145, 45);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄 Yenile";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(25, 42, 86);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(338, 32);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "💱 Döviz Portföy Yönetimi";
            // 
            // lblAccounts
            // 
            this.lblAccounts.AutoSize = true;
            this.lblAccounts.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAccounts.Location = new System.Drawing.Point(30, 70);
            this.lblAccounts.Name = "lblAccounts";
            this.lblAccounts.Size = new System.Drawing.Size(85, 20);
            this.lblAccounts.TabIndex = 5;
            this.lblAccounts.Text = "Hesap Seç:";
            // 
            // lblCurrencies
            // 
            this.lblCurrencies.AutoSize = true;
            this.lblCurrencies.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCurrencies.Location = new System.Drawing.Point(30, 140);
            this.lblCurrencies.Name = "lblCurrencies";
            this.lblCurrencies.Size = new System.Drawing.Size(198, 20);
            this.lblCurrencies.TabIndex = 6;
            this.lblCurrencies.Text = "Takip Edilecek Dövizler:";
            // 
            // lblRates
            // 
            this.lblRates.AutoSize = true;
            this.lblRates.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRates.Location = new System.Drawing.Point(360, 70);
            this.lblRates.Name = "lblRates";
            this.lblRates.Size = new System.Drawing.Size(152, 20);
            this.lblRates.TabIndex = 7;
            this.lblRates.Text = "Güncel Döviz Kurları:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(30, 530);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(300, 45);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "❌ Kapat";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CurrencyConverterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1090, 620);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.nudAmount);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblRates);
            this.Controls.Add(this.lblCurrencies);
            this.Controls.Add(this.lblAccounts);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvRates);
            this.Controls.Add(this.clbCurrencies);
            this.Controls.Add(this.cmbAccounts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CurrencyConverterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "💱 Döviz Portföy Yönetimi";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbAccounts;
        private System.Windows.Forms.CheckedListBox clbCurrencies;
        private System.Windows.Forms.DataGridView dgvRates;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAccounts;
        private System.Windows.Forms.Label lblCurrencies;
        private System.Windows.Forms.Label lblRates;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.NumericUpDown nudAmount;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Button btnConvert;
    }
}