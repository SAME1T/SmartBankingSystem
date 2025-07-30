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
        private System.Windows.Forms.Button btnLogout;
        
        // İşlem paneli
        private System.Windows.Forms.Panel pnlOperations;
        private System.Windows.Forms.Label lblOperationTitle;

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
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(130, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(300, 32);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "🏦 Smart Banking'e Hoş Geldiniz";
            
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
            this.pnlMainMenu.Controls.Add(this.btnLogout);
            this.pnlMainMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMainMenu.Location = new System.Drawing.Point(0, 120);
            this.pnlMainMenu.Name = "pnlMainMenu";
            this.pnlMainMenu.Size = new System.Drawing.Size(250, 480);
            this.pnlMainMenu.TabIndex = 1;
            
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
            
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(20, 400);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(210, 50);
            this.btnLogout.TabIndex = 5;
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
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.pnlOperations);
            this.Controls.Add(this.pnlMainMenu);
            this.Controls.Add(this.pnlUserInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🏦 Smart Banking - Ana Sayfa";
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