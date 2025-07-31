namespace SmartBankingAutomation
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        // Profil paneli
        private System.Windows.Forms.Panel panelProfile;
        private System.Windows.Forms.PictureBox picUserAvatar;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserTc;
        private System.Windows.Forms.Label lblWelcome;
        
        // Dashboard paneli
        private System.Windows.Forms.Panel dashboardPanel;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.Panel pnlSummaryCards;
        private System.Windows.Forms.Label lblTotalAssets;
        private System.Windows.Forms.Label lblUsdValue;
        private System.Windows.Forms.Label lblEurValue;
        private System.Windows.Forms.DataGridView dgvAccountBalances;
        private System.Windows.Forms.DataGridView dgvMarketRates;
        private System.Windows.Forms.Button btnRefreshDashboard;
        private System.Windows.Forms.Label lblAccountBalancesTitle;
        private System.Windows.Forms.Label lblMarketRatesTitle;
        
        // Dinamik menü
        private System.Windows.Forms.FlowLayoutPanel flowMenu;
        private System.Windows.Forms.Button btnLogout;
        



        /// <summary>
        ///  Clean up any resources being used.
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

        private void InitializeComponent()
        {
            this.panelProfile = new System.Windows.Forms.Panel();
            this.picUserAvatar = new System.Windows.Forms.PictureBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserTc = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.dashboardPanel = new System.Windows.Forms.Panel();
            this.lblDashboardTitle = new System.Windows.Forms.Label();
            this.pnlSummaryCards = new System.Windows.Forms.Panel();
            this.lblTotalAssets = new System.Windows.Forms.Label();
            this.lblUsdValue = new System.Windows.Forms.Label();
            this.lblEurValue = new System.Windows.Forms.Label();
            this.dgvAccountBalances = new System.Windows.Forms.DataGridView();
            this.dgvMarketRates = new System.Windows.Forms.DataGridView();
            this.btnRefreshDashboard = new System.Windows.Forms.Button();
            this.lblAccountBalancesTitle = new System.Windows.Forms.Label();
            this.lblMarketRatesTitle = new System.Windows.Forms.Label();
            this.flowMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUserAvatar)).BeginInit();
            this.dashboardPanel.SuspendLayout();
            this.pnlSummaryCards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountBalances)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarketRates)).BeginInit();
            this.flowMenu.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // panelProfile (Kullanıcı Profil Paneli)
            // 
            this.panelProfile.BackColor = System.Drawing.Color.FromArgb(25, 42, 86);
            this.panelProfile.Controls.Add(this.picUserAvatar);
            this.panelProfile.Controls.Add(this.lblWelcome);
            this.panelProfile.Controls.Add(this.lblUserName);
            this.panelProfile.Controls.Add(this.lblUserTc);
            this.panelProfile.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelProfile.Location = new System.Drawing.Point(0, 0);
            this.panelProfile.Name = "panelProfile";
            this.panelProfile.Size = new System.Drawing.Size(1400, 100);
            this.panelProfile.TabIndex = 0;
            
            // 
            // picUserAvatar
            // 
            this.picUserAvatar.BackColor = System.Drawing.Color.White;
            this.picUserAvatar.Location = new System.Drawing.Point(20, 10);
            this.picUserAvatar.Name = "picUserAvatar";
            this.picUserAvatar.Size = new System.Drawing.Size(80, 80);
            this.picUserAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picUserAvatar.TabIndex = 0;
            this.picUserAvatar.TabStop = false;
            
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(350, 15);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(450, 32);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "🏛️ TÜRKİYE DİJİTAL BANK";
            
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.lblUserName.Location = new System.Drawing.Point(120, 25);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(150, 25);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "👤 Ad Soyad";
            
            // 
            // lblUserTc
            // 
            this.lblUserTc.AutoSize = true;
            this.lblUserTc.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblUserTc.ForeColor = System.Drawing.Color.LightGray;
            this.lblUserTc.Location = new System.Drawing.Point(120, 55);
            this.lblUserTc.Name = "lblUserTc";
            this.lblUserTc.Size = new System.Drawing.Size(100, 20);
            this.lblUserTc.TabIndex = 3;
            this.lblUserTc.Text = "🆔 TC No";
            
            // Dashboard subtitle
            var lblSubtitle = new System.Windows.Forms.Label();
            lblSubtitle.Text = "Modern Bankacılık • Güvenli • Hızlı • Kolay";
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            lblSubtitle.Location = new System.Drawing.Point(350, 55);
            lblSubtitle.AutoSize = true;
            this.panelProfile.Controls.Add(lblSubtitle);
            
            // 
            // dashboardPanel (Dashboard Panel)
            // 
            this.dashboardPanel.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.dashboardPanel.Controls.Add(this.lblDashboardTitle);
            this.dashboardPanel.Controls.Add(this.pnlSummaryCards);
            this.dashboardPanel.Controls.Add(this.lblAccountBalancesTitle);
            this.dashboardPanel.Controls.Add(this.dgvAccountBalances);
            this.dashboardPanel.Controls.Add(this.lblMarketRatesTitle);
            this.dashboardPanel.Controls.Add(this.dgvMarketRates);
            this.dashboardPanel.Controls.Add(this.btnRefreshDashboard);
            this.dashboardPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.dashboardPanel.Location = new System.Drawing.Point(700, 100);
            this.dashboardPanel.Name = "dashboardPanel";
            this.dashboardPanel.Size = new System.Drawing.Size(700, 600);
            this.dashboardPanel.TabIndex = 2;
            
            // 
            // lblDashboardTitle
            // 
            this.lblDashboardTitle.AutoSize = true;
            this.lblDashboardTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblDashboardTitle.ForeColor = System.Drawing.Color.FromArgb(25, 42, 86);
            this.lblDashboardTitle.Location = new System.Drawing.Point(15, 15);
            this.lblDashboardTitle.Name = "lblDashboardTitle";
            this.lblDashboardTitle.Size = new System.Drawing.Size(200, 30);
            this.lblDashboardTitle.TabIndex = 0;
            this.lblDashboardTitle.Text = "📊 Dashboard";
            
            // 
            // pnlSummaryCards
            // 
            this.pnlSummaryCards.BackColor = System.Drawing.Color.White;
            this.pnlSummaryCards.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSummaryCards.Controls.Add(this.lblTotalAssets);
            this.pnlSummaryCards.Controls.Add(this.lblUsdValue);
            this.pnlSummaryCards.Controls.Add(this.lblEurValue);
            this.pnlSummaryCards.Location = new System.Drawing.Point(15, 55);
            this.pnlSummaryCards.Name = "pnlSummaryCards";
            this.pnlSummaryCards.Size = new System.Drawing.Size(670, 80);
            this.pnlSummaryCards.TabIndex = 1;
            
            // 
            // lblTotalAssets
            // 
            this.lblTotalAssets.AutoSize = true;
            this.lblTotalAssets.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalAssets.ForeColor = System.Drawing.Color.FromArgb(25, 42, 86);
            this.lblTotalAssets.Location = new System.Drawing.Point(15, 15);
            this.lblTotalAssets.Name = "lblTotalAssets";
            this.lblTotalAssets.Size = new System.Drawing.Size(200, 25);
            this.lblTotalAssets.TabIndex = 0;
            this.lblTotalAssets.Text = "💰 Toplam Varlık: ₺0";
            
            // 
            // lblUsdValue
            // 
            this.lblUsdValue.AutoSize = true;
            this.lblUsdValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUsdValue.ForeColor = System.Drawing.Color.FromArgb(52, 73, 126);
            this.lblUsdValue.Location = new System.Drawing.Point(280, 15);
            this.lblUsdValue.Name = "lblUsdValue";
            this.lblUsdValue.Size = new System.Drawing.Size(100, 20);
            this.lblUsdValue.TabIndex = 1;
            this.lblUsdValue.Text = "🇺🇸 USD: ₺0";
            
            // 
            // lblEurValue
            // 
            this.lblEurValue.AutoSize = true;
            this.lblEurValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblEurValue.ForeColor = System.Drawing.Color.FromArgb(52, 73, 126);
            this.lblEurValue.Location = new System.Drawing.Point(280, 45);
            this.lblEurValue.Name = "lblEurValue";
            this.lblEurValue.Size = new System.Drawing.Size(100, 20);
            this.lblEurValue.TabIndex = 2;
            this.lblEurValue.Text = "🇪🇺 EUR: ₺0";
            
            // 
            // flowMenu (Dinamik Menü Panel)
            // 
            this.flowMenu.AutoScroll = true;
            this.flowMenu.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.flowMenu.Controls.Add(this.btnLogout);
            this.flowMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMenu.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowMenu.Location = new System.Drawing.Point(0, 100);
            this.flowMenu.Name = "flowMenu";
            this.flowMenu.Padding = new System.Windows.Forms.Padding(20);
            this.flowMenu.Size = new System.Drawing.Size(700, 600);
            this.flowMenu.TabIndex = 1;
            this.flowMenu.WrapContents = true;

            
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(23, 23);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(200, 50);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "🚪 Çıkış Yap";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            
            
            // 
            // Form1
            // 
            // 
            // lblAccountBalancesTitle
            // 
            this.lblAccountBalancesTitle.AutoSize = true;
            this.lblAccountBalancesTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAccountBalancesTitle.ForeColor = System.Drawing.Color.FromArgb(52, 73, 126);
            this.lblAccountBalancesTitle.Location = new System.Drawing.Point(15, 155);
            this.lblAccountBalancesTitle.Name = "lblAccountBalancesTitle";
            this.lblAccountBalancesTitle.Size = new System.Drawing.Size(150, 21);
            this.lblAccountBalancesTitle.TabIndex = 3;
            this.lblAccountBalancesTitle.Text = "💳 Hesap Bakiyeleri";
            
            // 
            // dgvAccountBalances
            // 
            this.dgvAccountBalances.AllowUserToAddRows = false;
            this.dgvAccountBalances.AllowUserToDeleteRows = false;
            this.dgvAccountBalances.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAccountBalances.BackgroundColor = System.Drawing.Color.White;
            this.dgvAccountBalances.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvAccountBalances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccountBalances.Location = new System.Drawing.Point(15, 185);
            this.dgvAccountBalances.Name = "dgvAccountBalances";
            this.dgvAccountBalances.ReadOnly = true;
            this.dgvAccountBalances.RowTemplate.Height = 25;
            this.dgvAccountBalances.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccountBalances.Size = new System.Drawing.Size(670, 150);
            this.dgvAccountBalances.TabIndex = 4;
            
            // 
            // lblMarketRatesTitle
            // 
            this.lblMarketRatesTitle.AutoSize = true;
            this.lblMarketRatesTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMarketRatesTitle.ForeColor = System.Drawing.Color.FromArgb(52, 73, 126);
            this.lblMarketRatesTitle.Location = new System.Drawing.Point(15, 355);
            this.lblMarketRatesTitle.Name = "lblMarketRatesTitle";
            this.lblMarketRatesTitle.Size = new System.Drawing.Size(150, 21);
            this.lblMarketRatesTitle.TabIndex = 5;
            this.lblMarketRatesTitle.Text = "💱 Piyasa Kurları";
            
            // 
            // dgvMarketRates
            // 
            this.dgvMarketRates.AllowUserToAddRows = false;
            this.dgvMarketRates.AllowUserToDeleteRows = false;
            this.dgvMarketRates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMarketRates.BackgroundColor = System.Drawing.Color.White;
            this.dgvMarketRates.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvMarketRates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMarketRates.Location = new System.Drawing.Point(15, 385);
            this.dgvMarketRates.Name = "dgvMarketRates";
            this.dgvMarketRates.ReadOnly = true;
            this.dgvMarketRates.RowTemplate.Height = 25;
            this.dgvMarketRates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMarketRates.Size = new System.Drawing.Size(670, 150);
            this.dgvMarketRates.TabIndex = 6;
            
            // 
            // btnRefreshDashboard
            // 
            this.btnRefreshDashboard.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.btnRefreshDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshDashboard.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRefreshDashboard.ForeColor = System.Drawing.Color.FromArgb(25, 42, 86);
            this.btnRefreshDashboard.Location = new System.Drawing.Point(580, 550);
            this.btnRefreshDashboard.Name = "btnRefreshDashboard";
            this.btnRefreshDashboard.Size = new System.Drawing.Size(105, 35);
            this.btnRefreshDashboard.TabIndex = 7;
            this.btnRefreshDashboard.Text = "🔄 Yenile";
            this.btnRefreshDashboard.UseVisualStyleBackColor = false;
            this.btnRefreshDashboard.Click += new System.EventHandler(this.btnRefreshDashboard_Click);
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1400, 700);
            this.Controls.Add(this.dashboardPanel);
            this.Controls.Add(this.flowMenu);
            this.Controls.Add(this.panelProfile);
            this.MinimumSize = new System.Drawing.Size(1200, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🏛️ Türkiye Digital Bank - Smart Banking";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelProfile.ResumeLayout(false);
            this.panelProfile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUserAvatar)).EndInit();
            this.dashboardPanel.ResumeLayout(false);
            this.dashboardPanel.PerformLayout();
            this.pnlSummaryCards.ResumeLayout(false);
            this.pnlSummaryCards.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountBalances)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarketRates)).EndInit();
            this.flowMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}