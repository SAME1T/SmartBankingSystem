namespace SmartBankingAutomation
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        // Kullanıcı bilgileri paneli
        private System.Windows.Forms.Panel pnlUserInfo;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserTc;
        private System.Windows.Forms.Label lblUserPhone;
        private System.Windows.Forms.PictureBox picUserAvatar;
        
        // Ana menü butonları
        private System.Windows.Forms.Panel pnlMainMenu;
        private System.Windows.Forms.Button btnCreateAccount;
        private System.Windows.Forms.Button btnDeposit;
        private System.Windows.Forms.Button btnWithdraw;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnAccountInfo;
        private System.Windows.Forms.Button btnCreditCard;
        private System.Windows.Forms.Button btnCurrency;
        private System.Windows.Forms.Button btnLogout;
        
        // İşlem paneli
        private System.Windows.Forms.Panel pnlOperations;
        private System.Windows.Forms.Label lblOperationTitle;
        
        // Hesap Açma Kontrolleri
        private System.Windows.Forms.Panel pnlCreateAccount;
        private System.Windows.Forms.Label lblAccountType;
        private System.Windows.Forms.ComboBox cmbAccountType;
        private System.Windows.Forms.Button btnCreateAccountSubmit;
        private System.Windows.Forms.Label lblCreateAccountTitle;
        
        // Para Yatırma Kontrolleri
        private System.Windows.Forms.Panel pnlDeposit;
        private System.Windows.Forms.Label lblDepositTitle;
        private System.Windows.Forms.Label lblDepositAccount;
        private System.Windows.Forms.ComboBox cmbDepositAccount;
        private System.Windows.Forms.Label lblDepositAmount;
        private System.Windows.Forms.TextBox txtDepositAmount;
        private System.Windows.Forms.Button btnDepositSubmit;
        
        // Para Çekme Kontrolleri
        private System.Windows.Forms.Panel pnlWithdraw;
        private System.Windows.Forms.Label lblWithdrawTitle;
        private System.Windows.Forms.Label lblWithdrawAccount;
        private System.Windows.Forms.ComboBox cmbWithdrawAccount;
        private System.Windows.Forms.Label lblWithdrawAmount;
        private System.Windows.Forms.TextBox txtWithdrawAmount;
        private System.Windows.Forms.Button btnWithdrawSubmit;
        
        // Para Transfer Kontrolleri
        private System.Windows.Forms.Panel pnlTransferMoney;
        private System.Windows.Forms.Label lblTransferTitle;
        private System.Windows.Forms.Label lblFromAccount;
        private System.Windows.Forms.ComboBox cmbFromAccount;
        private System.Windows.Forms.Label lblToAccount;
        private System.Windows.Forms.TextBox txtToAccount;
        private System.Windows.Forms.Label lblTransferAmount;
        private System.Windows.Forms.TextBox txtTransferAmount;
        private System.Windows.Forms.Button btnTransferSubmit;
        
        // Hesap Bilgileri Kontrolleri
        private System.Windows.Forms.Panel pnlAccountInfo;
        private System.Windows.Forms.Label lblAccountInfoTitle;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.DataGridView dgvTransactions;
        private System.Windows.Forms.Label lblAccountsLabel;
        private System.Windows.Forms.Label lblTransactionsLabel;
        
        // Kredi Kartı Kontrolleri
        private System.Windows.Forms.Panel pnlCreditCard;
        private System.Windows.Forms.Label lblCreditCardTitle;
        private System.Windows.Forms.Button btnApplyCreditCard;
        private System.Windows.Forms.Button btnCreditCardPayment;
        private System.Windows.Forms.Button btnCreditCardTransactions;
        private System.Windows.Forms.Button btnCreditCardHistory;
        private System.Windows.Forms.DataGridView dgvCreditCards;

        // Döviz İşlemleri Kontrolleri
        private System.Windows.Forms.Panel pnlCurrency;
        private System.Windows.Forms.Label lblCurrencyTitle;
        private System.Windows.Forms.Button btnBuyCurrency;
        private System.Windows.Forms.Button btnSellCurrency;
        private System.Windows.Forms.DataGridView dgvCurrencyRates;
        private System.Windows.Forms.DataGridView dgvMyPortfolio;

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
            this.pnlUserInfo = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserTc = new System.Windows.Forms.Label();
            this.lblUserPhone = new System.Windows.Forms.Label();
            this.picUserAvatar = new System.Windows.Forms.PictureBox();
            this.pnlMainMenu = new System.Windows.Forms.Panel();
            this.btnCreateAccount = new System.Windows.Forms.Button();
            this.btnDeposit = new System.Windows.Forms.Button();
            this.btnWithdraw = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnAccountInfo = new System.Windows.Forms.Button();
            this.btnCreditCard = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlOperations = new System.Windows.Forms.Panel();
            this.lblOperationTitle = new System.Windows.Forms.Label();
            this.pnlUserInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUserAvatar)).BeginInit();
            this.pnlMainMenu.SuspendLayout();
            this.pnlOperations.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // pnlUserInfo (Kullanıcı Bilgileri Paneli)
            // 
            this.pnlUserInfo.BackColor = System.Drawing.Color.FromArgb(25, 25, 112);
            this.pnlUserInfo.Controls.Add(this.picUserAvatar);
            this.pnlUserInfo.Controls.Add(this.lblWelcome);
            this.pnlUserInfo.Controls.Add(this.lblUserName);
            this.pnlUserInfo.Controls.Add(this.lblUserTc);
            this.pnlUserInfo.Controls.Add(this.lblUserPhone);
            this.pnlUserInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUserInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlUserInfo.Name = "pnlUserInfo";
            this.pnlUserInfo.Size = new System.Drawing.Size(1000, 120);
            this.pnlUserInfo.TabIndex = 0;
            
            // 
            // picUserAvatar
            // 
            this.picUserAvatar.BackColor = System.Drawing.Color.White;
            this.picUserAvatar.Location = new System.Drawing.Point(30, 20);
            this.picUserAvatar.Name = "picUserAvatar";
            this.picUserAvatar.Size = new System.Drawing.Size(80, 80);
            this.picUserAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picUserAvatar.TabIndex = 0;
            this.picUserAvatar.TabStop = false;
            
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(280, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(500, 80);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "🏛️ TÜRKİYE DİJİTAL BANK \n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n   💎 MODERN BANKACILIK • GÜVENLİ • HIZLI • KOLAYDİJİTAL";
            
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.Gold;
            this.lblUserName.Location = new System.Drawing.Point(130, 55);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(150, 21);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "👤 Ad Soyad";
            
            // 
            // lblUserTc
            // 
            this.lblUserTc.AutoSize = true;
            this.lblUserTc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUserTc.ForeColor = System.Drawing.Color.LightGray;
            this.lblUserTc.Location = new System.Drawing.Point(130, 80);
            this.lblUserTc.Name = "lblUserTc";
            this.lblUserTc.Size = new System.Drawing.Size(100, 19);
            this.lblUserTc.TabIndex = 3;
            this.lblUserTc.Text = "🆔 TC No";
            
            // 
            // lblUserPhone
            // 
            this.lblUserPhone.AutoSize = true;
            this.lblUserPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUserPhone.ForeColor = System.Drawing.Color.LightGray;
            this.lblUserPhone.Location = new System.Drawing.Point(350, 80);
            this.lblUserPhone.Name = "lblUserPhone";
            this.lblUserPhone.Size = new System.Drawing.Size(100, 19);
            this.lblUserPhone.TabIndex = 4;
            this.lblUserPhone.Text = "📞 Telefon";
            
            // 
            // pnlMainMenu (Ana Menü Paneli)
            // 
            this.pnlMainMenu.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);
            this.pnlMainMenu.Controls.Add(this.btnCreateAccount);
            this.pnlMainMenu.Controls.Add(this.btnDeposit);
            this.pnlMainMenu.Controls.Add(this.btnWithdraw);
            this.pnlMainMenu.Controls.Add(this.btnTransfer);
            this.pnlMainMenu.Controls.Add(this.btnAccountInfo);
            this.pnlMainMenu.Controls.Add(this.btnCreditCard);
            this.pnlMainMenu.Controls.Add(this.btnCurrency);
            this.pnlMainMenu.Controls.Add(this.btnLogout);
            this.pnlMainMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMainMenu.Location = new System.Drawing.Point(0, 120);
            this.pnlMainMenu.Name = "pnlMainMenu";
            this.pnlMainMenu.Size = new System.Drawing.Size(250, 600);
            this.pnlMainMenu.TabIndex = 1;
            this.pnlMainMenu.AutoScroll = true; // Scroll özelliği eklendi
            
            // 
            // btnCreateAccount
            // 
            this.btnCreateAccount.BackColor = System.Drawing.Color.FromArgb(34, 139, 34);
            this.btnCreateAccount.FlatAppearance.BorderSize = 0;
            this.btnCreateAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateAccount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCreateAccount.ForeColor = System.Drawing.Color.White;
            this.btnCreateAccount.Location = new System.Drawing.Point(20, 30);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new System.Drawing.Size(210, 50);
            this.btnCreateAccount.TabIndex = 0;
            this.btnCreateAccount.Text = "💳 Hesap Aç";
            this.btnCreateAccount.UseVisualStyleBackColor = false;
            this.btnCreateAccount.Click += new System.EventHandler(this.btnCreateAccount_Click);
            
            // 
            // btnDeposit
            // 
            this.btnDeposit.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnDeposit.FlatAppearance.BorderSize = 0;
            this.btnDeposit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeposit.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnDeposit.ForeColor = System.Drawing.Color.White;
            this.btnDeposit.Location = new System.Drawing.Point(20, 100);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.Size = new System.Drawing.Size(210, 50);
            this.btnDeposit.TabIndex = 1;
            this.btnDeposit.Text = "💰 Para Yatır";
            this.btnDeposit.UseVisualStyleBackColor = false;
            this.btnDeposit.Click += new System.EventHandler(this.btnDeposit_Click);
            
            // 
            // btnWithdraw
            // 
            this.btnWithdraw.BackColor = System.Drawing.Color.FromArgb(220, 20, 60);
            this.btnWithdraw.FlatAppearance.BorderSize = 0;
            this.btnWithdraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWithdraw.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnWithdraw.ForeColor = System.Drawing.Color.White;
            this.btnWithdraw.Location = new System.Drawing.Point(20, 170);
            this.btnWithdraw.Name = "btnWithdraw";
            this.btnWithdraw.Size = new System.Drawing.Size(210, 50);
            this.btnWithdraw.TabIndex = 2;
            this.btnWithdraw.Text = "💸 Para Çek";
            this.btnWithdraw.UseVisualStyleBackColor = false;
            this.btnWithdraw.Click += new System.EventHandler(this.btnWithdraw_Click);
            
            // 
            // btnTransfer
            // 
            this.btnTransfer.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            this.btnTransfer.FlatAppearance.BorderSize = 0;
            this.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransfer.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnTransfer.ForeColor = System.Drawing.Color.White;
            this.btnTransfer.Location = new System.Drawing.Point(20, 240);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(210, 50);
            this.btnTransfer.TabIndex = 3;
            this.btnTransfer.Text = "🔄 Para Transfer";
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            
            // 
            // btnAccountInfo
            // 
            this.btnAccountInfo.BackColor = System.Drawing.Color.FromArgb(128, 0, 128);
            this.btnAccountInfo.FlatAppearance.BorderSize = 0;
            this.btnAccountInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccountInfo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAccountInfo.ForeColor = System.Drawing.Color.White;
            this.btnAccountInfo.Location = new System.Drawing.Point(20, 310);
            this.btnAccountInfo.Name = "btnAccountInfo";
            this.btnAccountInfo.Size = new System.Drawing.Size(210, 50);
            this.btnAccountInfo.TabIndex = 4;
            this.btnAccountInfo.Text = "📊 Hesap Bilgileri";
            this.btnAccountInfo.UseVisualStyleBackColor = false;
            this.btnAccountInfo.Click += new System.EventHandler(this.btnAccountInfo_Click);
            
            // 
            // btnCreditCard
            // 
            this.btnCreditCard.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnCreditCard.FlatAppearance.BorderSize = 0;
            this.btnCreditCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreditCard.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCreditCard.ForeColor = System.Drawing.Color.White;
            this.btnCreditCard.Location = new System.Drawing.Point(20, 380);
            this.btnCreditCard.Name = "btnCreditCard";
            this.btnCreditCard.Size = new System.Drawing.Size(210, 50);
            this.btnCreditCard.TabIndex = 5;
            this.btnCreditCard.Text = "💳 Kredi Kartı";
            this.btnCreditCard.UseVisualStyleBackColor = false;
            this.btnCreditCard.Click += new System.EventHandler(this.btnCreditCard_Click);
            
            // 
            // btnCurrency
            // 
            this.btnCurrency = new System.Windows.Forms.Button();
            this.btnCurrency.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.btnCurrency.FlatAppearance.BorderSize = 0;
            this.btnCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCurrency.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCurrency.ForeColor = System.Drawing.Color.White;
            this.btnCurrency.Location = new System.Drawing.Point(20, 420);
            this.btnCurrency.Name = "btnCurrency";
            this.btnCurrency.Size = new System.Drawing.Size(210, 50);
            this.btnCurrency.TabIndex = 6;
            this.btnCurrency.Text = "💰 Döviz İşlemleri";
            this.btnCurrency.UseVisualStyleBackColor = false;
            this.btnCurrency.Click += new System.EventHandler(this.btnCurrency_Click);
            
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(20, 480);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(210, 50);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "🚪 Çıkış Yap";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            
            // 
            // pnlOperations (İşlemler Paneli)
            // 
            this.pnlOperations.BackColor = System.Drawing.Color.White;
            this.pnlOperations.Controls.Add(this.lblOperationTitle);
            this.pnlOperations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOperations.Location = new System.Drawing.Point(250, 120);
            this.pnlOperations.Name = "pnlOperations";
            this.pnlOperations.Size = new System.Drawing.Size(750, 480);
            this.pnlOperations.TabIndex = 2;
            
            // 
            // lblOperationTitle
            // 
            this.lblOperationTitle.AutoSize = true;
            this.lblOperationTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblOperationTitle.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            this.lblOperationTitle.Location = new System.Drawing.Point(50, 50);
            this.lblOperationTitle.Name = "lblOperationTitle";
            this.lblOperationTitle.Size = new System.Drawing.Size(400, 30);
            this.lblOperationTitle.TabIndex = 0;
            this.lblOperationTitle.Text = "✨ Hoş Geldiniz! Yapmak istediğiniz işlemi seçin.";
            
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 720);
            this.Controls.Add(this.pnlOperations);
            this.Controls.Add(this.pnlMainMenu);
            this.Controls.Add(this.pnlUserInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🏛️ Türkiye Digital Bank - Dijital Bankacılık Sistemi";
            this.pnlUserInfo.ResumeLayout(false);
            this.pnlUserInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUserAvatar)).EndInit();
            this.pnlMainMenu.ResumeLayout(false);
            this.pnlOperations.ResumeLayout(false);
            this.pnlOperations.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}