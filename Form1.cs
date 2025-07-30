using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace SmartBankingAutomation
{
    public partial class Form1 : Form
    {
        private string dbPath = "SmartBank.db";
        private string currentName = "";
        private string currentSurname = "";
        private string currentTcNo = "";
        private string currentPhone = "";

        public Form1()
        {
            InitializeComponent();
            CreateDatabaseAndTables();
            SetUserInfo("Misafir", "Kullanıcı", "00000000000", "000-000-0000");
        }

        public Form1(string name, string surname, string tcno, string phone)
        {
            InitializeComponent();
            CreateDatabaseAndTables();
            currentName = name;
            currentSurname = surname;
            currentTcNo = tcno;
            currentPhone = phone;
            SetUserInfo(name, surname, tcno, phone);
        }

        private void SetUserInfo(string name, string surname, string tcno, string phone)
        {
            lblUserName.Text = $"👤 {name} {surname}";
            lblUserTc.Text = $"🆔 TC: {tcno}";
            lblUserPhone.Text = $"📞 Tel: {phone}";
            
            // Avatar için kullanıcının baş harfini göster
            picUserAvatar.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.FillEllipse(System.Drawing.Brushes.LightBlue, 5, 5, 70, 70);
                var font = new System.Drawing.Font("Segoe UI", 24, System.Drawing.FontStyle.Bold);
                var brush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                string initial = string.IsNullOrEmpty(name) ? "?" : name.Substring(0, 1).ToUpper();
                var size = g.MeasureString(initial, font);
                g.DrawString(initial, font, brush, (80 - size.Width) / 2, (80 - size.Height) / 2);
            };
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "Çıkış", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }

        private void HideAllPanels()
        {
            foreach (Control control in pnlOperations.Controls)
            {
                if (control is Panel && control != lblOperationTitle)
                    control.Visible = false;
            }
            lblOperationTitle.Visible = true;
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ShowCreateAccountPanel();
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ShowDepositPanel();
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ShowWithdrawPanel();
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ShowTransferPanel();
        }

        private void btnAccountInfo_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ShowAccountInfoPanel();
        }

        private void btnCreditCard_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ShowCreditCardPanel();
        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ShowCurrencyPanel();
        }

        private void ShowCreditCardPanel()
        {
            if (pnlCreditCard == null)
            {
                CreateCreditCardPanel();
            }
            FixCreditCardLimits(); // Önce limit sorunlarını düzelt
            LoadCreditCards();
            pnlCreditCard.Visible = true;
            pnlCreditCard?.BringToFront();
        }

        private void CreateCreditCardPanel()
        {
            pnlCreditCard = new Panel();
            pnlCreditCard.Size = new System.Drawing.Size(700, 550);
            pnlCreditCard.Location = new System.Drawing.Point(50, 50);
            pnlCreditCard.BackColor = System.Drawing.Color.White;
            
            lblCreditCardTitle = new Label();
            lblCreditCardTitle.Text = "💳 Kredi Kartı İşlemleri";
            lblCreditCardTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblCreditCardTitle.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
            lblCreditCardTitle.Location = new System.Drawing.Point(20, 20);
            lblCreditCardTitle.AutoSize = true;
            
            // Kredi kartı başvuru butonu
            btnApplyCreditCard = new Button();
            btnApplyCreditCard.Text = "📝 Kredi Kartı Başvurusu";
            btnApplyCreditCard.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnApplyCreditCard.ForeColor = System.Drawing.Color.White;
            btnApplyCreditCard.FlatStyle = FlatStyle.Flat;
            btnApplyCreditCard.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnApplyCreditCard.Location = new System.Drawing.Point(20, 70);
            btnApplyCreditCard.Size = new System.Drawing.Size(200, 45);
            btnApplyCreditCard.Click += btnApplyCreditCard_Click;
            
            // Kredi kartı ödeme butonu
            btnCreditCardPayment = new Button();
            btnCreditCardPayment.Text = "💰 Kredi Kartı Ödeme";
            btnCreditCardPayment.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnCreditCardPayment.ForeColor = System.Drawing.Color.White;
            btnCreditCardPayment.FlatStyle = FlatStyle.Flat;
            btnCreditCardPayment.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnCreditCardPayment.Location = new System.Drawing.Point(240, 70);
            btnCreditCardPayment.Size = new System.Drawing.Size(200, 45);
            btnCreditCardPayment.Click += btnCreditCardPayment_Click;
            
            // Kredi kartı harcama butonu
            btnCreditCardTransactions = new Button();
            btnCreditCardTransactions.Text = "🛒 Harcama Yap";
            btnCreditCardTransactions.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            btnCreditCardTransactions.ForeColor = System.Drawing.Color.White;
            btnCreditCardTransactions.FlatStyle = FlatStyle.Flat;
            btnCreditCardTransactions.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnCreditCardTransactions.Location = new System.Drawing.Point(460, 70);
            btnCreditCardTransactions.Size = new System.Drawing.Size(200, 45);
            btnCreditCardTransactions.Click += btnCreditCardTransactions_Click;

            // Kredi kartı işlem geçmişi butonu
            btnCreditCardHistory = new Button();
            btnCreditCardHistory.Text = "📊 İşlem Geçmişi";
            btnCreditCardHistory.BackColor = System.Drawing.Color.FromArgb(128, 0, 128);
            btnCreditCardHistory.ForeColor = System.Drawing.Color.White;
            btnCreditCardHistory.FlatStyle = FlatStyle.Flat;
            btnCreditCardHistory.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnCreditCardHistory.Location = new System.Drawing.Point(20, 125);
            btnCreditCardHistory.Size = new System.Drawing.Size(200, 45);
            btnCreditCardHistory.Click += btnCreditCardHistory_Click;
            
            // Kredi kartları listesi
            Label lblCreditCardsLabel = new Label();
            lblCreditCardsLabel.Text = "💳 Kredi Kartlarınız";
            lblCreditCardsLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblCreditCardsLabel.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            lblCreditCardsLabel.Location = new System.Drawing.Point(20, 185);
            lblCreditCardsLabel.AutoSize = true;
            
            dgvCreditCards = new DataGridView();
            dgvCreditCards.Location = new System.Drawing.Point(20, 215);
            dgvCreditCards.Size = new System.Drawing.Size(650, 350);
            dgvCreditCards.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCreditCards.ReadOnly = true;
            dgvCreditCards.AllowUserToAddRows = false;
            dgvCreditCards.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCreditCards.MultiSelect = false;
            
            pnlCreditCard.Controls.Add(lblCreditCardTitle);
            pnlCreditCard.Controls.Add(btnApplyCreditCard);
            pnlCreditCard.Controls.Add(btnCreditCardPayment);
            pnlCreditCard.Controls.Add(btnCreditCardTransactions);
            pnlCreditCard.Controls.Add(btnCreditCardHistory);
            pnlCreditCard.Controls.Add(lblCreditCardsLabel);
            pnlCreditCard.Controls.Add(dgvCreditCards);
            
            pnlOperations.Controls.Add(pnlCreditCard);
        }

        private void ShowCreateAccountPanel()
        {
            if (pnlCreateAccount == null)
            {
                CreateAccountPanel();
            }
            pnlCreateAccount.Visible = true;
            pnlCreateAccount?.BringToFront();
        }

        private void CreateAccountPanel()
        {
            pnlCreateAccount = new Panel();
            pnlCreateAccount.Size = new System.Drawing.Size(600, 500);
            pnlCreateAccount.Location = new System.Drawing.Point(50, 50);
            pnlCreateAccount.BackColor = System.Drawing.Color.White;
            
            lblCreateAccountTitle = new Label();
            lblCreateAccountTitle.Text = "💳 Yeni Hesap Açma";
            lblCreateAccountTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblCreateAccountTitle.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            lblCreateAccountTitle.Location = new System.Drawing.Point(20, 20);
            lblCreateAccountTitle.AutoSize = true;
            
            lblAccountType = new Label();
            lblAccountType.Text = "Hesap Türü:";
            lblAccountType.Font = new System.Drawing.Font("Segoe UI", 11);
            lblAccountType.Location = new System.Drawing.Point(20, 80);
            lblAccountType.AutoSize = true;
            
            cmbAccountType = new ComboBox();
            cmbAccountType.Items.AddRange(new string[] { "Vadeli", "Vadesiz" });
            cmbAccountType.SelectedIndex = 0;
            cmbAccountType.Font = new System.Drawing.Font("Segoe UI", 11);
            cmbAccountType.Location = new System.Drawing.Point(150, 77);
            cmbAccountType.Size = new System.Drawing.Size(200, 25);
            cmbAccountType.SelectedIndexChanged += (s, e) => ToggleMaturityFields();
            
            // Vadeli hesap için vade süresi
            Label lblMaturityPeriod = new Label();
            lblMaturityPeriod.Text = "📅 Vade Süresi:";
            lblMaturityPeriod.Font = new System.Drawing.Font("Segoe UI", 11);
            lblMaturityPeriod.Location = new System.Drawing.Point(20, 120);
            lblMaturityPeriod.AutoSize = true;
            lblMaturityPeriod.Name = "lblMaturityPeriod";
            
            ComboBox cmbMaturityPeriod = new ComboBox();
            cmbMaturityPeriod.Items.AddRange(new string[] { "1 Ay", "3 Ay", "6 Ay", "12 Ay", "24 Ay" });
            cmbMaturityPeriod.SelectedIndex = 2; // Varsayılan 6 ay
            cmbMaturityPeriod.Font = new System.Drawing.Font("Segoe UI", 11);
            cmbMaturityPeriod.Location = new System.Drawing.Point(150, 117);
            cmbMaturityPeriod.Size = new System.Drawing.Size(200, 25);
            cmbMaturityPeriod.Name = "cmbMaturityPeriod";
            cmbMaturityPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // Vadeli hesap için faiz oranı
            Label lblInterestRate = new Label();
            lblInterestRate.Text = "💰 Faiz Oranı:";
            lblInterestRate.Font = new System.Drawing.Font("Segoe UI", 11);
            lblInterestRate.Location = new System.Drawing.Point(20, 160);
            lblInterestRate.AutoSize = true;
            lblInterestRate.Name = "lblInterestRate";
            
            ComboBox cmbInterestRate = new ComboBox();
            cmbInterestRate.Items.AddRange(new string[] { "15% (1 Ay)", "18% (3 Ay)", "22% (6 Ay)", "25% (12 Ay)", "28% (24 Ay)" });
            cmbInterestRate.SelectedIndex = 2; // Varsayılan 6 ay faizi
            cmbInterestRate.Font = new System.Drawing.Font("Segoe UI", 11);
            cmbInterestRate.Location = new System.Drawing.Point(150, 157);
            cmbInterestRate.Size = new System.Drawing.Size(200, 25);
            cmbInterestRate.Name = "cmbInterestRate";
            cmbInterestRate.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // Başlangıç tutarı
            Label lblInitialAmount = new Label();
            lblInitialAmount.Text = "💵 Başlangıç Tutarı:";
            lblInitialAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            lblInitialAmount.Location = new System.Drawing.Point(20, 200);
            lblInitialAmount.AutoSize = true;
            
            TextBox txtInitialAmount = new TextBox();
            txtInitialAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            txtInitialAmount.Location = new System.Drawing.Point(150, 197);
            txtInitialAmount.Size = new System.Drawing.Size(200, 25);
            txtInitialAmount.PlaceholderText = "Min: 1000 TL";
            txtInitialAmount.Name = "txtInitialAmount";
            
            btnCreateAccountSubmit = new Button();
            btnCreateAccountSubmit.Text = "✅ Hesap Aç";
            btnCreateAccountSubmit.BackColor = System.Drawing.Color.FromArgb(34, 139, 34);
            btnCreateAccountSubmit.ForeColor = System.Drawing.Color.White;
            btnCreateAccountSubmit.FlatStyle = FlatStyle.Flat;
            btnCreateAccountSubmit.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnCreateAccountSubmit.Location = new System.Drawing.Point(150, 250);
            btnCreateAccountSubmit.Size = new System.Drawing.Size(150, 40);
            btnCreateAccountSubmit.Click += btnCreateAccountSubmit_Click;
            
            pnlCreateAccount.Controls.Add(lblCreateAccountTitle);
            pnlCreateAccount.Controls.Add(lblAccountType);
            pnlCreateAccount.Controls.Add(cmbAccountType);
            pnlCreateAccount.Controls.Add(lblMaturityPeriod);
            pnlCreateAccount.Controls.Add(cmbMaturityPeriod);
            pnlCreateAccount.Controls.Add(lblInterestRate);
            pnlCreateAccount.Controls.Add(cmbInterestRate);
            pnlCreateAccount.Controls.Add(lblInitialAmount);
            pnlCreateAccount.Controls.Add(txtInitialAmount);
            pnlCreateAccount.Controls.Add(btnCreateAccountSubmit);
            
            pnlOperations.Controls.Add(pnlCreateAccount);
        }

        private void ToggleMaturityFields()
        {
            if (pnlCreateAccount == null) return;
            
            bool isTimeDeposit = cmbAccountType.Text == "Vadeli";
            
            // Vadeli hesap alanlarını göster/gizle
            foreach (Control control in pnlCreateAccount.Controls)
            {
                if (control.Name == "lblMaturityPeriod" || control.Name == "cmbMaturityPeriod" ||
                    control.Name == "lblInterestRate" || control.Name == "cmbInterestRate")
                {
                    control.Visible = isTimeDeposit;
                }
            }
        }

        private void btnCreateAccountSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Başlangıç tutarı kontrolü
                decimal initialAmount = 0;
                TextBox txtInitialAmount = pnlCreateAccount.Controls.OfType<TextBox>().FirstOrDefault(x => x.Name == "txtInitialAmount");
                if (txtInitialAmount != null && !string.IsNullOrWhiteSpace(txtInitialAmount.Text))
                {
                    if (!decimal.TryParse(txtInitialAmount.Text, out initialAmount) || initialAmount < 0)
                    {
                        MessageBox.Show("Geçerli bir başlangıç tutarı girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    if (cmbAccountType.Text == "Vadeli" && initialAmount < 1000)
                    {
                        MessageBox.Show("Vadeli hesap için minimum 1000 TL gereklidir!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Müşteri ID'sini bul
                    string findCustomerQuery = "SELECT Id FROM Customers WHERE TcNo = @TcNo";
                    int customerId;
                    using (SQLiteCommand cmd = new SQLiteCommand(findCustomerQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Müşteri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        customerId = Convert.ToInt32(result);
                    }
                    
                    // Hesap numarası oluştur
                    string accountNumber = GenerateAccountNumber();
                    
                    // Vadeli hesap bilgilerini hesapla
                    DateTime? maturityDate = null;
                    decimal interestRate = 0;
                    decimal maturityAmount = initialAmount;
                    
                    if (cmbAccountType.Text == "Vadeli")
                    {
                        ComboBox cmbMaturityPeriod = pnlCreateAccount.Controls.OfType<ComboBox>().FirstOrDefault(x => x.Name == "cmbMaturityPeriod");
                        ComboBox cmbInterestRateCombo = pnlCreateAccount.Controls.OfType<ComboBox>().FirstOrDefault(x => x.Name == "cmbInterestRate");
                        
                        if (cmbMaturityPeriod != null && cmbInterestRateCombo != null)
                        {
                            // Vade süresi hesaplama
                            int months = cmbMaturityPeriod.SelectedIndex switch
                            {
                                0 => 1,   // 1 Ay
                                1 => 3,   // 3 Ay
                                2 => 6,   // 6 Ay
                                3 => 12,  // 12 Ay
                                4 => 24,  // 24 Ay
                                _ => 6
                            };
                            
                            // Faiz oranı hesaplama
                            interestRate = cmbInterestRateCombo.SelectedIndex switch
                            {
                                0 => 15m, // 1 Ay
                                1 => 18m, // 3 Ay
                                2 => 22m, // 6 Ay
                                3 => 25m, // 12 Ay
                                4 => 28m, // 24 Ay
                                _ => 22m
                            };
                            
                            maturityDate = DateTime.Now.AddMonths(months);
                            maturityAmount = initialAmount * (1 + (interestRate / 100) * (months / 12m));
                        }
                    }
                    
                    // Hesap oluştur
                    string createAccountQuery = @"INSERT INTO Accounts 
                        (CustomerId, AccountNumber, AccountType, Balance, MaturityDate, InterestRate, MaturityAmount) 
                        VALUES (@CustomerId, @AccountNumber, @AccountType, @Balance, @MaturityDate, @InterestRate, @MaturityAmount)";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(createAccountQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        cmd.Parameters.AddWithValue("@AccountType", cmbAccountType.Text);
                        cmd.Parameters.AddWithValue("@Balance", initialAmount);
                        cmd.Parameters.AddWithValue("@MaturityDate", (object)maturityDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@InterestRate", interestRate);
                        cmd.Parameters.AddWithValue("@MaturityAmount", maturityAmount);
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Başlangıç tutarı varsa işlem kaydı ekle
                    if (initialAmount > 0)
                    {
                        // Hesap ID'sini al
                        string getAccountIdQuery = "SELECT Id FROM Accounts WHERE AccountNumber = @AccountNumber";
                        int accountId;
                        using (SQLiteCommand cmd = new SQLiteCommand(getAccountIdQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                            accountId = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        
                        // İşlem kaydı ekle
                        string insertTransactionQuery = "INSERT INTO Transactions (AccountId, TransactionType, Amount, Description) VALUES (@AccountId, @Type, @Amount, @Description)";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertTransactionQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@AccountId", accountId);
                            cmd.Parameters.AddWithValue("@Type", "Para Yatırma");
                            cmd.Parameters.AddWithValue("@Amount", initialAmount);
                            cmd.Parameters.AddWithValue("@Description", "Hesap açılış tutarı");
                            cmd.ExecuteNonQuery();
                        }
                    }
                    
                    // Yeni hesap detaylarını göster
                    ShowNewAccountDetails(accountNumber, cmbAccountType.Text, initialAmount, maturityDate, interestRate, maturityAmount);
                    
                    HideAllPanels();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateAccountNumber()
        {
            Random random = new Random();
            return $"{random.Next(1000, 9999)}-{random.Next(1000, 9999)}-{random.Next(1000, 9999)}";
        }

        private void ShowNewAccountDetails(string accountNumber, string accountType, decimal balance, DateTime? maturityDate = null, decimal interestRate = 0, decimal maturityAmount = 0)
        {
            // Yeni hesap detay paneli oluştur
            Panel pnlAccountDetails = new Panel();
            pnlAccountDetails.Size = new System.Drawing.Size(550, accountType == "Vadeli" ? 450 : 350);
            pnlAccountDetails.Location = new System.Drawing.Point(200, 100);
            pnlAccountDetails.BackColor = System.Drawing.Color.White;
            pnlAccountDetails.BorderStyle = BorderStyle.FixedSingle;
            
            // Başlık
            Label lblTitle = new Label();
            lblTitle.Text = "🎉 Hesap Başarıyla Açıldı!";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(34, 139, 34);
            lblTitle.Location = new System.Drawing.Point(20, 20);
            lblTitle.AutoSize = true;
            
            // Hesap Numarası
            Label lblAccountNoLabel = new Label();
            lblAccountNoLabel.Text = "💳 Hesap Numarası:";
            lblAccountNoLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblAccountNoLabel.Location = new System.Drawing.Point(20, 80);
            lblAccountNoLabel.AutoSize = true;
            
            Label lblAccountNo = new Label();
            lblAccountNo.Text = accountNumber;
            lblAccountNo.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblAccountNo.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            lblAccountNo.Location = new System.Drawing.Point(20, 110);
            lblAccountNo.AutoSize = true;
            
            // Kopyala butonu - Hesap numarası için
            Button btnCopyAccountNo = new Button();
            btnCopyAccountNo.Text = "📋 Kopyala";
            btnCopyAccountNo.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnCopyAccountNo.ForeColor = System.Drawing.Color.White;
            btnCopyAccountNo.FlatStyle = FlatStyle.Flat;
            btnCopyAccountNo.Font = new System.Drawing.Font("Segoe UI", 10);
            btnCopyAccountNo.Location = new System.Drawing.Point(300, 105);
            btnCopyAccountNo.Size = new System.Drawing.Size(100, 30);
            btnCopyAccountNo.Click += (s, e) => {
                Clipboard.SetText(accountNumber);
                MessageBox.Show("Hesap numarası panoya kopyalandı!", "Kopyalandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            
            // Hesap Türü
            Label lblAccountTypeLabel = new Label();
            lblAccountTypeLabel.Text = "📊 Hesap Türü:";
            lblAccountTypeLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblAccountTypeLabel.Location = new System.Drawing.Point(20, 160);
            lblAccountTypeLabel.AutoSize = true;
            
            Label lblAccountTypeValue = new Label();
            lblAccountTypeValue.Text = accountType;
            lblAccountTypeValue.Font = new System.Drawing.Font("Segoe UI", 12);
            lblAccountTypeValue.Location = new System.Drawing.Point(150, 160);
            lblAccountTypeValue.AutoSize = true;
            
            // Bakiye
            Label lblBalanceLabel = new Label();
            lblBalanceLabel.Text = "💰 Başlangıç Bakiyesi:";
            lblBalanceLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblBalanceLabel.Location = new System.Drawing.Point(20, 190);
            lblBalanceLabel.AutoSize = true;
            
            Label lblBalanceValue = new Label();
            lblBalanceValue.Text = $"{balance:C}";
            lblBalanceValue.Font = new System.Drawing.Font("Segoe UI", 12);
            lblBalanceValue.Location = new System.Drawing.Point(200, 190);
            lblBalanceValue.AutoSize = true;
            
            int yPosition = 220;
            
            // Vadeli hesap için ek bilgiler
            if (accountType == "Vadeli" && maturityDate.HasValue)
            {
                // Vade Tarihi
                Label lblMaturityDateLabel = new Label();
                lblMaturityDateLabel.Text = "📅 Vade Tarihi:";
                lblMaturityDateLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                lblMaturityDateLabel.Location = new System.Drawing.Point(20, yPosition);
                lblMaturityDateLabel.AutoSize = true;
                
                Label lblMaturityDateValue = new Label();
                lblMaturityDateValue.Text = maturityDate.Value.ToString("dd.MM.yyyy");
                lblMaturityDateValue.Font = new System.Drawing.Font("Segoe UI", 12);
                lblMaturityDateValue.Location = new System.Drawing.Point(150, yPosition);
                lblMaturityDateValue.AutoSize = true;
                
                yPosition += 30;
                
                // Faiz Oranı
                Label lblInterestRateLabel = new Label();
                lblInterestRateLabel.Text = "💰 Faiz Oranı:";
                lblInterestRateLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                lblInterestRateLabel.Location = new System.Drawing.Point(20, yPosition);
                lblInterestRateLabel.AutoSize = true;
                
                Label lblInterestRateValue = new Label();
                lblInterestRateValue.Text = $"%{interestRate:F1}";
                lblInterestRateValue.Font = new System.Drawing.Font("Segoe UI", 12);
                lblInterestRateValue.ForeColor = System.Drawing.Color.FromArgb(0, 128, 0);
                lblInterestRateValue.Location = new System.Drawing.Point(150, yPosition);
                lblInterestRateValue.AutoSize = true;
                
                yPosition += 30;
                
                // Vade Sonu Tutarı
                Label lblMaturityAmountLabel = new Label();
                lblMaturityAmountLabel.Text = "🎯 Vade Sonu Tutarı:";
                lblMaturityAmountLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                lblMaturityAmountLabel.Location = new System.Drawing.Point(20, yPosition);
                lblMaturityAmountLabel.AutoSize = true;
                
                Label lblMaturityAmountValue = new Label();
                lblMaturityAmountValue.Text = $"{maturityAmount:C}";
                lblMaturityAmountValue.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
                lblMaturityAmountValue.ForeColor = System.Drawing.Color.FromArgb(0, 128, 0);
                lblMaturityAmountValue.Location = new System.Drawing.Point(200, yPosition);
                lblMaturityAmountValue.AutoSize = true;
                
                yPosition += 40;
                
                pnlAccountDetails.Controls.Add(lblMaturityDateLabel);
                pnlAccountDetails.Controls.Add(lblMaturityDateValue);
                pnlAccountDetails.Controls.Add(lblInterestRateLabel);
                pnlAccountDetails.Controls.Add(lblInterestRateValue);
                pnlAccountDetails.Controls.Add(lblMaturityAmountLabel);
                pnlAccountDetails.Controls.Add(lblMaturityAmountValue);
            }
            
            // Kapat butonu
            Button btnClose = new Button();
            btnClose.Text = "✅ Tamam";
            btnClose.BackColor = System.Drawing.Color.FromArgb(34, 139, 34);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            btnClose.Location = new System.Drawing.Point(200, yPosition + 20);
            btnClose.Size = new System.Drawing.Size(120, 40);
            btnClose.Click += (s, e) => {
                this.Controls.Remove(pnlAccountDetails);
            };
            
            // Kontrolleri panele ekle
            pnlAccountDetails.Controls.Add(lblTitle);
            pnlAccountDetails.Controls.Add(lblAccountNoLabel);
            pnlAccountDetails.Controls.Add(lblAccountNo);
            pnlAccountDetails.Controls.Add(btnCopyAccountNo);
            pnlAccountDetails.Controls.Add(lblAccountTypeLabel);
            pnlAccountDetails.Controls.Add(lblAccountTypeValue);
            pnlAccountDetails.Controls.Add(lblBalanceLabel);
            pnlAccountDetails.Controls.Add(lblBalanceValue);
            pnlAccountDetails.Controls.Add(btnClose);
            
            // Paneli forma ekle ve en öne getir
            this.Controls.Add(pnlAccountDetails);
            pnlAccountDetails.BringToFront();
        }

        private void ShowDepositPanel()
        {
            if (pnlDeposit == null)
            {
                CreateDepositPanel();
            }
            LoadUserAccounts(cmbDepositAccount);
            pnlDeposit.Visible = true;
            pnlDeposit?.BringToFront();
        }

        private void CreateDepositPanel()
        {
            pnlDeposit = new Panel();
            pnlDeposit.Size = new System.Drawing.Size(600, 400);
            pnlDeposit.Location = new System.Drawing.Point(50, 100);
            pnlDeposit.BackColor = System.Drawing.Color.White;
            
            lblDepositTitle = new Label();
            lblDepositTitle.Text = "💰 Para Yatırma";
            lblDepositTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblDepositTitle.ForeColor = System.Drawing.Color.FromArgb(0, 123, 255);
            lblDepositTitle.Location = new System.Drawing.Point(20, 20);
            lblDepositTitle.AutoSize = true;
            
            lblDepositAccount = new Label();
            lblDepositAccount.Text = "Hesap Seçin:";
            lblDepositAccount.Font = new System.Drawing.Font("Segoe UI", 11);
            lblDepositAccount.Location = new System.Drawing.Point(20, 80);
            lblDepositAccount.AutoSize = true;
            
            cmbDepositAccount = new ComboBox();
            cmbDepositAccount.Font = new System.Drawing.Font("Segoe UI", 11);
            cmbDepositAccount.Location = new System.Drawing.Point(150, 77);
            cmbDepositAccount.Size = new System.Drawing.Size(300, 25);
            cmbDepositAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            
            lblDepositAmount = new Label();
            lblDepositAmount.Text = "Yatırılacak Tutar:";
            lblDepositAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            lblDepositAmount.Location = new System.Drawing.Point(20, 130);
            lblDepositAmount.AutoSize = true;
            
            txtDepositAmount = new TextBox();
            txtDepositAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            txtDepositAmount.Location = new System.Drawing.Point(150, 127);
            txtDepositAmount.Size = new System.Drawing.Size(200, 25);
            txtDepositAmount.PlaceholderText = "0.00";
            
            btnDepositSubmit = new Button();
            btnDepositSubmit.Text = "💰 Para Yatır";
            btnDepositSubmit.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnDepositSubmit.ForeColor = System.Drawing.Color.White;
            btnDepositSubmit.FlatStyle = FlatStyle.Flat;
            btnDepositSubmit.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnDepositSubmit.Location = new System.Drawing.Point(150, 180);
            btnDepositSubmit.Size = new System.Drawing.Size(150, 40);
            btnDepositSubmit.Click += btnDepositSubmit_Click;
            
            pnlDeposit.Controls.Add(lblDepositTitle);
            pnlDeposit.Controls.Add(lblDepositAccount);
            pnlDeposit.Controls.Add(cmbDepositAccount);
            pnlDeposit.Controls.Add(lblDepositAmount);
            pnlDeposit.Controls.Add(txtDepositAmount);
            pnlDeposit.Controls.Add(btnDepositSubmit);
            
            pnlOperations.Controls.Add(pnlDeposit);
        }

        private void LoadUserAccounts(ComboBox combo)
        {
            combo.Items.Clear();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string query = @"SELECT a.AccountNumber, a.AccountType, a.Balance 
                                   FROM Accounts a 
                                   INNER JOIN Customers c ON a.CustomerId = c.Id 
                                   WHERE c.TcNo = @TcNo";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string accountNumber = reader["AccountNumber"]?.ToString() ?? "";
                                string accountType = reader["AccountType"]?.ToString() ?? "";
                                decimal balance = Convert.ToDecimal(reader["Balance"]);
                                
                                combo.Items.Add($"{accountNumber} - {accountType} (Bakiye: {balance:C})");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hesaplar yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDepositSubmit_Click(object sender, EventArgs e)
        {
            if (cmbDepositAccount.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir hesap seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDepositAmount.Text))
            {
                MessageBox.Show("Lütfen tutarı girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtDepositAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Geçerli bir tutar girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string selectedAccount = cmbDepositAccount.SelectedItem?.ToString() ?? "";
                string accountNumber = selectedAccount.Split(' ')[0];

                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Hesap ID'sini bul
                    string findAccountQuery = "SELECT Id, Balance FROM Accounts WHERE AccountNumber = @AccountNumber";
                    int accountId;
                    decimal currentBalance;
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(findAccountQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Hesap bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            accountId = Convert.ToInt32(reader["Id"]);
                            currentBalance = Convert.ToDecimal(reader["Balance"]);
                        }
                    }
                    
                    // Bakiyeyi güncelle
                    decimal newBalance = currentBalance + amount;
                    string updateBalanceQuery = "UPDATE Accounts SET Balance = @Balance WHERE Id = @Id";
                    using (SQLiteCommand cmd = new SQLiteCommand(updateBalanceQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Balance", newBalance);
                        cmd.Parameters.AddWithValue("@Id", accountId);
                        cmd.ExecuteNonQuery();
                    }
                    
                    // İşlem kaydı ekle
                    string insertTransactionQuery = "INSERT INTO Transactions (AccountId, TransactionType, Amount, Description) VALUES (@AccountId, @Type, @Amount, @Description)";
                    using (SQLiteCommand cmd = new SQLiteCommand(insertTransactionQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountId", accountId);
                        cmd.Parameters.AddWithValue("@Type", "Para Yatırma");
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Description", $"Para yatırma işlemi - {amount:C}");
                        cmd.ExecuteNonQuery();
                    }
                    
                    MessageBox.Show($"✅ Para yatırma başarılı!\n\nYatırılan Tutar: {amount:C}\nYeni Bakiye: {newBalance:C}", 
                        "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    txtDepositAmount.Clear();
                    LoadUserAccounts(cmbDepositAccount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowWithdrawPanel()
        {
            if (pnlWithdraw == null)
            {
                CreateWithdrawPanel();
            }
            LoadUserAccounts(cmbWithdrawAccount);
            pnlWithdraw.Visible = true;
            pnlWithdraw?.BringToFront();
        }

        private void CreateWithdrawPanel()
        {
            pnlWithdraw = new Panel();
            pnlWithdraw.Size = new System.Drawing.Size(600, 400);
            pnlWithdraw.Location = new System.Drawing.Point(50, 100);
            pnlWithdraw.BackColor = System.Drawing.Color.White;
            
            lblWithdrawTitle = new Label();
            lblWithdrawTitle.Text = "💸 Para Çekme";
            lblWithdrawTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblWithdrawTitle.ForeColor = System.Drawing.Color.FromArgb(220, 20, 60);
            lblWithdrawTitle.Location = new System.Drawing.Point(20, 20);
            lblWithdrawTitle.AutoSize = true;
            
            lblWithdrawAccount = new Label();
            lblWithdrawAccount.Text = "Hesap Seçin:";
            lblWithdrawAccount.Font = new System.Drawing.Font("Segoe UI", 11);
            lblWithdrawAccount.Location = new System.Drawing.Point(20, 80);
            lblWithdrawAccount.AutoSize = true;
            
            cmbWithdrawAccount = new ComboBox();
            cmbWithdrawAccount.Font = new System.Drawing.Font("Segoe UI", 11);
            cmbWithdrawAccount.Location = new System.Drawing.Point(150, 77);
            cmbWithdrawAccount.Size = new System.Drawing.Size(300, 25);
            cmbWithdrawAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            
            lblWithdrawAmount = new Label();
            lblWithdrawAmount.Text = "Çekilecek Tutar:";
            lblWithdrawAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            lblWithdrawAmount.Location = new System.Drawing.Point(20, 130);
            lblWithdrawAmount.AutoSize = true;
            
            txtWithdrawAmount = new TextBox();
            txtWithdrawAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            txtWithdrawAmount.Location = new System.Drawing.Point(150, 127);
            txtWithdrawAmount.Size = new System.Drawing.Size(200, 25);
            txtWithdrawAmount.PlaceholderText = "0.00";
            
            btnWithdrawSubmit = new Button();
            btnWithdrawSubmit.Text = "💸 Para Çek";
            btnWithdrawSubmit.BackColor = System.Drawing.Color.FromArgb(220, 20, 60);
            btnWithdrawSubmit.ForeColor = System.Drawing.Color.White;
            btnWithdrawSubmit.FlatStyle = FlatStyle.Flat;
            btnWithdrawSubmit.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnWithdrawSubmit.Location = new System.Drawing.Point(150, 180);
            btnWithdrawSubmit.Size = new System.Drawing.Size(150, 40);
            btnWithdrawSubmit.Click += btnWithdrawSubmit_Click;
            
            pnlWithdraw.Controls.Add(lblWithdrawTitle);
            pnlWithdraw.Controls.Add(lblWithdrawAccount);
            pnlWithdraw.Controls.Add(cmbWithdrawAccount);
            pnlWithdraw.Controls.Add(lblWithdrawAmount);
            pnlWithdraw.Controls.Add(txtWithdrawAmount);
            pnlWithdraw.Controls.Add(btnWithdrawSubmit);
            
            pnlOperations.Controls.Add(pnlWithdraw);
        }

        private void btnWithdrawSubmit_Click(object sender, EventArgs e)
        {
            if (cmbWithdrawAccount.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir hesap seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtWithdrawAmount.Text))
            {
                MessageBox.Show("Lütfen tutarı girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtWithdrawAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Geçerli bir tutar girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string selectedAccount = cmbWithdrawAccount.SelectedItem?.ToString() ?? "";
                string accountNumber = selectedAccount.Split(' ')[0];

                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Hesap bilgilerini bul
                    string findAccountQuery = "SELECT Id, Balance FROM Accounts WHERE AccountNumber = @AccountNumber";
                    int accountId;
                    decimal currentBalance;
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(findAccountQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Hesap bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            accountId = Convert.ToInt32(reader["Id"]);
                            currentBalance = Convert.ToDecimal(reader["Balance"]);
                        }
                    }
                    
                    // Bakiye kontrolü
                    if (currentBalance < amount)
                    {
                        MessageBox.Show($"Yetersiz bakiye!\n\nMevcut Bakiye: {currentBalance:C}\nÇekmek İstediğiniz: {amount:C}", 
                            "Yetersiz Bakiye", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    // Bakiyeyi güncelle
                    decimal newBalance = currentBalance - amount;
                    string updateBalanceQuery = "UPDATE Accounts SET Balance = @Balance WHERE Id = @Id";
                    using (SQLiteCommand cmd = new SQLiteCommand(updateBalanceQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Balance", newBalance);
                        cmd.Parameters.AddWithValue("@Id", accountId);
                        cmd.ExecuteNonQuery();
                    }
                    
                    // İşlem kaydı ekle
                    string insertTransactionQuery = "INSERT INTO Transactions (AccountId, TransactionType, Amount, Description) VALUES (@AccountId, @Type, @Amount, @Description)";
                    using (SQLiteCommand cmd = new SQLiteCommand(insertTransactionQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountId", accountId);
                        cmd.Parameters.AddWithValue("@Type", "Para Çekme");
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Description", $"Para çekme işlemi - {amount:C}");
                        cmd.ExecuteNonQuery();
                    }
                    
                    MessageBox.Show($"✅ Para çekme başarılı!\n\nÇekilen Tutar: {amount:C}\nKalan Bakiye: {newBalance:C}", 
                        "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    txtWithdrawAmount.Clear();
                    LoadUserAccounts(cmbWithdrawAccount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowTransferPanel()
        {
            if (pnlTransferMoney == null)
            {
                CreateTransferPanel();
            }
            LoadUserAccounts(cmbFromAccount);
            pnlTransferMoney.Visible = true;
            pnlTransferMoney?.BringToFront();
        }

        private void CreateTransferPanel()
        {
            pnlTransferMoney = new Panel();
            pnlTransferMoney.Size = new System.Drawing.Size(600, 400);
            pnlTransferMoney.Location = new System.Drawing.Point(50, 100);
            pnlTransferMoney.BackColor = System.Drawing.Color.White;
            
            lblTransferTitle = new Label();
            lblTransferTitle.Text = "🔄 Para Transfer";
            lblTransferTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblTransferTitle.ForeColor = System.Drawing.Color.FromArgb(255, 140, 0);
            lblTransferTitle.Location = new System.Drawing.Point(20, 20);
            lblTransferTitle.AutoSize = true;
            
            lblFromAccount = new Label();
            lblFromAccount.Text = "Gönderen Hesap:";
            lblFromAccount.Font = new System.Drawing.Font("Segoe UI", 11);
            lblFromAccount.Location = new System.Drawing.Point(20, 80);
            lblFromAccount.AutoSize = true;
            
            cmbFromAccount = new ComboBox();
            cmbFromAccount.Font = new System.Drawing.Font("Segoe UI", 11);
            cmbFromAccount.Location = new System.Drawing.Point(150, 77);
            cmbFromAccount.Size = new System.Drawing.Size(300, 25);
            cmbFromAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            
            lblToAccount = new Label();
            lblToAccount.Text = "Alıcı Hesap No:";
            lblToAccount.Font = new System.Drawing.Font("Segoe UI", 11);
            lblToAccount.Location = new System.Drawing.Point(20, 130);
            lblToAccount.AutoSize = true;
            
            txtToAccount = new TextBox();
            txtToAccount.Font = new System.Drawing.Font("Segoe UI", 11);
            txtToAccount.Location = new System.Drawing.Point(150, 127);
            txtToAccount.Size = new System.Drawing.Size(250, 25);
            txtToAccount.PlaceholderText = "Örn: 1234-5678-9012";
            
            // Kopyala butonu - Transfer işlemi için
            Button btnCopyAccountTransfer = new Button();
            btnCopyAccountTransfer.Text = "📋";
            btnCopyAccountTransfer.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnCopyAccountTransfer.ForeColor = System.Drawing.Color.White;
            btnCopyAccountTransfer.FlatStyle = FlatStyle.Flat;
            btnCopyAccountTransfer.Font = new System.Drawing.Font("Segoe UI", 10);
            btnCopyAccountTransfer.Location = new System.Drawing.Point(410, 127);
            btnCopyAccountTransfer.Size = new System.Drawing.Size(40, 25);
            btnCopyAccountTransfer.Click += (s, e) => {
                if (!string.IsNullOrWhiteSpace(txtToAccount.Text))
                {
                    Clipboard.SetText(txtToAccount.Text);
                    MessageBox.Show("Hesap numarası panoya kopyalandı!", "Kopyalandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Kopyalanacak hesap numarası yok!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            
            // Yapıştır butonu - Transfer işlemi için
            Button btnPasteAccountTransfer = new Button();
            btnPasteAccountTransfer.Text = "📥";
            btnPasteAccountTransfer.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnPasteAccountTransfer.ForeColor = System.Drawing.Color.White;
            btnPasteAccountTransfer.FlatStyle = FlatStyle.Flat;
            btnPasteAccountTransfer.Font = new System.Drawing.Font("Segoe UI", 10);
            btnPasteAccountTransfer.Location = new System.Drawing.Point(460, 127);
            btnPasteAccountTransfer.Size = new System.Drawing.Size(40, 25);
            btnPasteAccountTransfer.Click += (s, e) => {
                try
                {
                    if (Clipboard.ContainsText())
                    {
                        string clipboardText = Clipboard.GetText();
                        txtToAccount.Text = clipboardText;
                        MessageBox.Show("Hesap numarası yapıştırıldı!", "Yapıştırıldı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Panoda metin bulunamadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Yapıştırma sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            
            // Alıcı ismi gösterme
            Label lblRecipientName = new Label();
            lblRecipientName.Text = "👤 Alıcı: Hesap numarası girin";
            lblRecipientName.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Italic);
            lblRecipientName.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            lblRecipientName.Location = new System.Drawing.Point(150, 155);
            lblRecipientName.Size = new System.Drawing.Size(350, 20);
            
            // Hesap numarası değiştiğinde alıcı ismini göster
            txtToAccount.TextChanged += (s, e) => {
                CheckRecipientName(txtToAccount.Text, lblRecipientName);
            };
            
            lblTransferAmount = new Label();
            lblTransferAmount.Text = "Transfer Tutarı:";
            lblTransferAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            lblTransferAmount.Location = new System.Drawing.Point(20, 200);
            lblTransferAmount.AutoSize = true;
            
            txtTransferAmount = new TextBox();
            txtTransferAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            txtTransferAmount.Location = new System.Drawing.Point(150, 197);
            txtTransferAmount.Size = new System.Drawing.Size(200, 25);
            txtTransferAmount.PlaceholderText = "0.00";
            
            btnTransferSubmit = new Button();
            btnTransferSubmit.Text = "🔄 Transfer Yap";
            btnTransferSubmit.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            btnTransferSubmit.ForeColor = System.Drawing.Color.White;
            btnTransferSubmit.FlatStyle = FlatStyle.Flat;
            btnTransferSubmit.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnTransferSubmit.Location = new System.Drawing.Point(150, 250);
            btnTransferSubmit.Size = new System.Drawing.Size(150, 40);
            btnTransferSubmit.Click += btnTransferSubmit_Click;
            
            pnlTransferMoney.Controls.Add(lblTransferTitle);
            pnlTransferMoney.Controls.Add(lblFromAccount);
            pnlTransferMoney.Controls.Add(cmbFromAccount);
            pnlTransferMoney.Controls.Add(lblToAccount);
            pnlTransferMoney.Controls.Add(txtToAccount);
            pnlTransferMoney.Controls.Add(btnCopyAccountTransfer);
            pnlTransferMoney.Controls.Add(btnPasteAccountTransfer);
            pnlTransferMoney.Controls.Add(lblRecipientName);
            pnlTransferMoney.Controls.Add(lblTransferAmount);
            pnlTransferMoney.Controls.Add(txtTransferAmount);
            pnlTransferMoney.Controls.Add(btnTransferSubmit);
            
            pnlOperations.Controls.Add(pnlTransferMoney);
        }

        private void btnTransferSubmit_Click(object sender, EventArgs e)
        {
            if (cmbFromAccount.SelectedItem == null)
            {
                MessageBox.Show("Lütfen gönderen hesabı seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtToAccount.Text))
            {
                MessageBox.Show("Lütfen alıcı hesap numarasını girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTransferAmount.Text))
            {
                MessageBox.Show("Lütfen transfer tutarını girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtTransferAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Geçerli bir tutar girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string fromAccountSelected = cmbFromAccount.SelectedItem?.ToString() ?? "";
                string fromAccountNumber = fromAccountSelected.Split(' ')[0];
                string toAccountNumber = txtToAccount.Text.Trim();

                if (fromAccountNumber == toAccountNumber)
                {
                    MessageBox.Show("Aynı hesaba transfer yapamazsınız!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Transfer onaylama sorusu
                string recipientInfo = GetRecipientInfo(toAccountNumber);
                string confirmMessage = $"💰 Transfer Onayı\n\n" +
                                      $"Gönderen: {fromAccountSelected}\n" +
                                      $"Alıcı: {toAccountNumber}\n" +
                                      $"{recipientInfo}\n" +
                                      $"Tutar: {amount:C}\n\n" +
                                      $"Bu transferi yapmak istediğinizden emin misiniz?";

                DialogResult result = MessageBox.Show(confirmMessage, "Transfer Onayı", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Gönderen hesap kontrolü
                    string fromAccountQuery = "SELECT Id, Balance FROM Accounts WHERE AccountNumber = @AccountNumber";
                    int fromAccountId;
                    decimal fromBalance;
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(fromAccountQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountNumber", fromAccountNumber);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Gönderen hesap bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            fromAccountId = Convert.ToInt32(reader["Id"]);
                            fromBalance = Convert.ToDecimal(reader["Balance"]);
                        }
                    }
                    
                    // Alıcı hesap kontrolü
                    string toAccountQuery = "SELECT Id, Balance FROM Accounts WHERE AccountNumber = @AccountNumber";
                    int toAccountId;
                    decimal toBalance;
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(toAccountQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountNumber", toAccountNumber);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Alıcı hesap bulunamadı!\nLütfen hesap numarasını kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            toAccountId = Convert.ToInt32(reader["Id"]);
                            toBalance = Convert.ToDecimal(reader["Balance"]);
                        }
                    }
                    
                    // Bakiye kontrolü
                    if (fromBalance < amount)
                    {
                        MessageBox.Show($"Yetersiz bakiye!\n\nMevcut Bakiye: {fromBalance:C}\nTransfer Tutarı: {amount:C}", 
                            "Yetersiz Bakiye", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    // Transfer işlemi
                    decimal newFromBalance = fromBalance - amount;
                    decimal newToBalance = toBalance + amount;
                    
                    // Gönderen hesap güncelleme
                    string updateFromQuery = "UPDATE Accounts SET Balance = @Balance WHERE Id = @Id";
                    using (SQLiteCommand cmd = new SQLiteCommand(updateFromQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Balance", newFromBalance);
                        cmd.Parameters.AddWithValue("@Id", fromAccountId);
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Alıcı hesap güncelleme
                    string updateToQuery = "UPDATE Accounts SET Balance = @Balance WHERE Id = @Id";
                    using (SQLiteCommand cmd = new SQLiteCommand(updateToQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Balance", newToBalance);
                        cmd.Parameters.AddWithValue("@Id", toAccountId);
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Gönderen için işlem kaydı
                    string insertFromTransactionQuery = "INSERT INTO Transactions (AccountId, TransactionType, Amount, Description) VALUES (@AccountId, @Type, @Amount, @Description)";
                    using (SQLiteCommand cmd = new SQLiteCommand(insertFromTransactionQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountId", fromAccountId);
                        cmd.Parameters.AddWithValue("@Type", "Para Transferi (Giden)");
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Description", $"Transfer - Alıcı: {toAccountNumber}");
                        cmd.ExecuteNonQuery();
                    }
                    
                    // Alıcı için işlem kaydı
                    string insertToTransactionQuery = "INSERT INTO Transactions (AccountId, TransactionType, Amount, Description) VALUES (@AccountId, @Type, @Amount, @Description)";
                    using (SQLiteCommand cmd = new SQLiteCommand(insertToTransactionQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountId", toAccountId);
                        cmd.Parameters.AddWithValue("@Type", "Para Transferi (Gelen)");
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Description", $"Transfer - Gönderen: {fromAccountNumber}");
                        cmd.ExecuteNonQuery();
                    }
                    
                    MessageBox.Show($"✅ Transfer başarılı!\n\nGönderilen Tutar: {amount:C}\nYeni Bakiyeniz: {newFromBalance:C}\n\nAlıcı Hesap: {toAccountNumber}", 
                        "Transfer Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    txtToAccount.Clear();
                    txtTransferAmount.Clear();
                    LoadUserAccounts(cmbFromAccount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAccountInfoPanel()
        {
            if (pnlAccountInfo == null)
            {
                CreateAccountInfoPanel();
            }
            LoadAccountInfo();
            pnlAccountInfo.Visible = true;
            pnlAccountInfo?.BringToFront();
        }

        private void CreateAccountInfoPanel()
        {
            pnlAccountInfo = new Panel();
            pnlAccountInfo.Size = new System.Drawing.Size(650, 450);
            pnlAccountInfo.Location = new System.Drawing.Point(50, 50);
            pnlAccountInfo.BackColor = System.Drawing.Color.White;
            
            lblAccountInfoTitle = new Label();
            lblAccountInfoTitle.Text = "📊 Hesap Bilgileri ve İşlem Geçmişi";
            lblAccountInfoTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblAccountInfoTitle.ForeColor = System.Drawing.Color.FromArgb(128, 0, 128);
            lblAccountInfoTitle.Location = new System.Drawing.Point(20, 20);
            lblAccountInfoTitle.AutoSize = true;
            
            lblAccountsLabel = new Label();
            lblAccountsLabel.Text = "💳 Hesaplarınız";
            lblAccountsLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblAccountsLabel.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            lblAccountsLabel.Location = new System.Drawing.Point(20, 70);
            lblAccountsLabel.AutoSize = true;
            
            dgvAccounts = new DataGridView();
            dgvAccounts.Location = new System.Drawing.Point(20, 100);
            dgvAccounts.Size = new System.Drawing.Size(500, 120);
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccounts.ReadOnly = true;
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.MultiSelect = false;
            
            // Hesap numarası kopyala butonu
            Button btnCopyAccountInfo = new Button();
            btnCopyAccountInfo.Text = "📋 Seçili Hesap\nNumarasını Kopyala";
            btnCopyAccountInfo.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnCopyAccountInfo.ForeColor = System.Drawing.Color.White;
            btnCopyAccountInfo.FlatStyle = FlatStyle.Flat;
            btnCopyAccountInfo.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            btnCopyAccountInfo.Location = new System.Drawing.Point(530, 130);
            btnCopyAccountInfo.Size = new System.Drawing.Size(100, 60);
            btnCopyAccountInfo.Click += btnCopyAccountInfo_Click;
            
            lblTransactionsLabel = new Label();
            lblTransactionsLabel.Text = "📋 Son İşlemleriniz";
            lblTransactionsLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblTransactionsLabel.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            lblTransactionsLabel.Location = new System.Drawing.Point(20, 240);
            lblTransactionsLabel.AutoSize = true;
            
            dgvTransactions = new DataGridView();
            dgvTransactions.Location = new System.Drawing.Point(20, 270);
            dgvTransactions.Size = new System.Drawing.Size(600, 150);
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.AllowUserToAddRows = false;
            
            pnlAccountInfo.Controls.Add(lblAccountInfoTitle);
            pnlAccountInfo.Controls.Add(lblAccountsLabel);
            pnlAccountInfo.Controls.Add(dgvAccounts);
            pnlAccountInfo.Controls.Add(btnCopyAccountInfo);
            pnlAccountInfo.Controls.Add(lblTransactionsLabel);
            pnlAccountInfo.Controls.Add(dgvTransactions);
            
            pnlOperations.Controls.Add(pnlAccountInfo);
        }

        private void LoadAccountInfo()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Hesap bilgilerini yükle
                    string accountQuery = @"SELECT a.AccountNumber, a.AccountType, a.Balance, a.CreatedDate 
                                          FROM Accounts a 
                                          INNER JOIN Customers c ON a.CustomerId = c.Id 
                                          WHERE c.TcNo = @TcNo";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(accountQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            da.Fill(dt);
                            
                            // Kolon başlıklarını Türkçe yap
                            if (dt.Columns.Contains("AccountNumber")) dt.Columns["AccountNumber"].ColumnName = "Hesap Numarası";
                            if (dt.Columns.Contains("AccountType")) dt.Columns["AccountType"].ColumnName = "Hesap Türü";
                            if (dt.Columns.Contains("Balance")) dt.Columns["Balance"].ColumnName = "Bakiye";
                            if (dt.Columns.Contains("CreatedDate")) dt.Columns["CreatedDate"].ColumnName = "Açılış Tarihi";
                            
                            dgvAccounts.DataSource = dt;
                        }
                    }
                    
                    // İşlem geçmişini yükle
                    string transactionQuery = @"SELECT t.TransactionType, t.Amount, t.Description, t.TransactionDate, a.AccountNumber
                                              FROM Transactions t
                                              INNER JOIN Accounts a ON t.AccountId = a.Id
                                              INNER JOIN Customers c ON a.CustomerId = c.Id
                                              WHERE c.TcNo = @TcNo
                                              ORDER BY t.TransactionDate DESC
                                              LIMIT 20";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(transactionQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            da.Fill(dt);
                            
                            // Kolon başlıklarını Türkçe yap
                            if (dt.Columns.Contains("TransactionType")) dt.Columns["TransactionType"].ColumnName = "İşlem Türü";
                            if (dt.Columns.Contains("Amount")) dt.Columns["Amount"].ColumnName = "Tutar";
                            if (dt.Columns.Contains("Description")) dt.Columns["Description"].ColumnName = "Açıklama";
                            if (dt.Columns.Contains("TransactionDate")) dt.Columns["TransactionDate"].ColumnName = "Tarih";
                            if (dt.Columns.Contains("AccountNumber")) dt.Columns["AccountNumber"].ColumnName = "Hesap No";
                            
                            dgvTransactions.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bilgiler yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreditCardHistory_Click(object sender, EventArgs e)
        {
            ShowCreditCardTransactionsPanel();
        }

        private void ShowCreditCardTransactionsPanel()
        {
            // Kredi kartı işlem geçmişi formu oluştur
            Form transactionForm = new Form();
            transactionForm.Text = "💳 Kredi Kartı İşlem Geçmişi";
            transactionForm.Size = new System.Drawing.Size(800, 600);
            transactionForm.StartPosition = FormStartPosition.CenterParent;
            transactionForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            transactionForm.MaximizeBox = false;

            // Başlık
            Label lblTitle = new Label();
            lblTitle.Text = "💳 Kredi Kartı İşlem Geçmişi";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
            lblTitle.Location = new System.Drawing.Point(20, 20);
            lblTitle.AutoSize = true;

            // Kart seçimi
            Label lblSelectCard = new Label();
            lblSelectCard.Text = "Kart Seçin:";
            lblSelectCard.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            lblSelectCard.Location = new System.Drawing.Point(20, 70);
            lblSelectCard.AutoSize = true;

            ComboBox cmbCardSelect = new ComboBox();
            cmbCardSelect.Location = new System.Drawing.Point(120, 67);
            cmbCardSelect.Size = new System.Drawing.Size(400, 30);
            cmbCardSelect.DropDownStyle = ComboBoxStyle.DropDownList;

            // İşlem geçmişi tablosu
            DataGridView dgvTransactionHistory = new DataGridView();
            dgvTransactionHistory.Location = new System.Drawing.Point(20, 110);
            dgvTransactionHistory.Size = new System.Drawing.Size(740, 400);
            dgvTransactionHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactionHistory.ReadOnly = true;
            dgvTransactionHistory.AllowUserToAddRows = false;
            dgvTransactionHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Toplam harcama etiketi
            Label lblTotalSpent = new Label();
            lblTotalSpent.Text = "💰 Toplam Harcama: ₺0,00";
            lblTotalSpent.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblTotalSpent.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
            lblTotalSpent.Location = new System.Drawing.Point(20, 530);
            lblTotalSpent.AutoSize = true;

            // Kartları yükle
            LoadCreditCardsToCombo(cmbCardSelect);

            // Kart seçimi değiştiğinde işlem geçmişini yükle
            cmbCardSelect.SelectedIndexChanged += (s, e) => {
                if (cmbCardSelect.SelectedItem != null)
                {
                    string selectedCard = cmbCardSelect.SelectedItem.ToString();
                    string cardNumber = "";
                    if (selectedCard.Contains(" - "))
                    {
                        cardNumber = selectedCard.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim();
                    }
                    LoadCreditCardTransactionHistory(dgvTransactionHistory, cardNumber, lblTotalSpent);
                }
            };

            // Kontrolleri forma ekle
            transactionForm.Controls.Add(lblTitle);
            transactionForm.Controls.Add(lblSelectCard);
            transactionForm.Controls.Add(cmbCardSelect);
            transactionForm.Controls.Add(dgvTransactionHistory);
            transactionForm.Controls.Add(lblTotalSpent);

            // İlk kartı seç ve geçmişi yükle
            if (cmbCardSelect.Items.Count > 0)
            {
                cmbCardSelect.SelectedIndex = 0;
            }

            transactionForm.ShowDialog();
        }

        private void LoadCreditCardTransactionHistory(DataGridView dgv, string cardNumber, Label lblTotal)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();

                    string query = @"SELECT 
                                       cct.TransactionDate as 'İşlem Tarihi',
                                       cct.TransactionType as 'İşlem Türü', 
                                       cct.Amount as 'Tutar',
                                       cct.MerchantName as 'Mağaza',
                                       cct.Description as 'Açıklama'
                                   FROM CreditCardTransactions cct
                                   INNER JOIN CreditCards cc ON cct.CreditCardId = cc.Id
                                   WHERE cc.CardNumber = @CardNumber
                                   ORDER BY cct.TransactionDate DESC";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CardNumber", cardNumber);
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            da.Fill(dt);
                            dgv.DataSource = dt;

                            // Toplam harcamayı hesapla
                            decimal totalSpent = 0;
                            foreach (System.Data.DataRow row in dt.Rows)
                            {
                                if (row["İşlem Türü"].ToString() == "Harcama")
                                {
                                    totalSpent += Convert.ToDecimal(row["Tutar"]);
                                }
                            }
                            lblTotal.Text = $"💰 Toplam Harcama: {totalSpent:C}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İşlem geçmişi yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCreditCardsToCombo(ComboBox cmb)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    string query = @"SELECT cc.CardNumber, cc.CardName 
                                   FROM CreditCards cc 
                                   INNER JOIN Customers c ON cc.CustomerId = c.Id 
                                   WHERE c.TcNo = @TcNo AND cc.IsActive = 1";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            cmb.Items.Clear();
                            while (reader.Read())
                            {
                                string cardNumber = reader["CardNumber"]?.ToString() ?? "";
                                string cardName = reader["CardName"]?.ToString() ?? "";
                                cmb.Items.Add($"{cardNumber} - {cardName}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kredi kartları yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowCurrencyPanel()
        {
            if (pnlCurrency == null)
            {
                CreateCurrencyPanel();
            }
            LoadCurrencyData();
            pnlCurrency.Visible = true;
            pnlCurrency?.BringToFront();
        }

        private void CreateCurrencyPanel()
        {
            pnlCurrency = new Panel();
            pnlCurrency.Size = new System.Drawing.Size(700, 600);
            pnlCurrency.Location = new System.Drawing.Point(50, 50);
            pnlCurrency.BackColor = System.Drawing.Color.White;

            lblCurrencyTitle = new Label();
            lblCurrencyTitle.Text = "💰 Döviz İşlemleri";
            lblCurrencyTitle.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
            lblCurrencyTitle.ForeColor = System.Drawing.Color.FromArgb(255, 193, 7);
            lblCurrencyTitle.Location = new System.Drawing.Point(20, 20);
            lblCurrencyTitle.AutoSize = true;

            // Döviz Al butonu
            btnBuyCurrency = new Button();
            btnBuyCurrency.Text = "💵 Döviz/Altın AL";
            btnBuyCurrency.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnBuyCurrency.ForeColor = System.Drawing.Color.White;
            btnBuyCurrency.FlatStyle = FlatStyle.Flat;
            btnBuyCurrency.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            btnBuyCurrency.Location = new System.Drawing.Point(20, 70);
            btnBuyCurrency.Size = new System.Drawing.Size(150, 40);
            btnBuyCurrency.Click += btnBuyCurrency_Click;

            // Döviz Sat butonu
            btnSellCurrency = new Button();
            btnSellCurrency.Text = "💸 Döviz/Altın SAT";
            btnSellCurrency.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            btnSellCurrency.ForeColor = System.Drawing.Color.White;
            btnSellCurrency.FlatStyle = FlatStyle.Flat;
            btnSellCurrency.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            btnSellCurrency.Location = new System.Drawing.Point(180, 70);
            btnSellCurrency.Size = new System.Drawing.Size(150, 40);
            btnSellCurrency.Click += btnSellCurrency_Click;

            // Döviz kurları başlığı
            Label lblRatesTitle = new Label();
            lblRatesTitle.Text = "📊 Güncel Kurlar";
            lblRatesTitle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblRatesTitle.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            lblRatesTitle.Location = new System.Drawing.Point(20, 130);
            lblRatesTitle.AutoSize = true;

            // Döviz kurları tablosu
            dgvCurrencyRates = new DataGridView();
            dgvCurrencyRates.Location = new System.Drawing.Point(20, 160);
            dgvCurrencyRates.Size = new System.Drawing.Size(650, 180);
            dgvCurrencyRates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCurrencyRates.ReadOnly = true;
            dgvCurrencyRates.AllowUserToAddRows = false;

            // Portföy başlığı
            Label lblPortfolioTitle = new Label();
            lblPortfolioTitle.Text = "💼 Döviz Portföyüm";
            lblPortfolioTitle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblPortfolioTitle.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            lblPortfolioTitle.Location = new System.Drawing.Point(20, 360);
            lblPortfolioTitle.AutoSize = true;

            // Portföy tablosu
            dgvMyPortfolio = new DataGridView();
            dgvMyPortfolio.Location = new System.Drawing.Point(20, 390);
            dgvMyPortfolio.Size = new System.Drawing.Size(650, 180);
            dgvMyPortfolio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMyPortfolio.ReadOnly = true;
            dgvMyPortfolio.AllowUserToAddRows = false;

            pnlCurrency.Controls.Add(lblCurrencyTitle);
            pnlCurrency.Controls.Add(btnBuyCurrency);
            pnlCurrency.Controls.Add(btnSellCurrency);
            pnlCurrency.Controls.Add(lblRatesTitle);
            pnlCurrency.Controls.Add(dgvCurrencyRates);
            pnlCurrency.Controls.Add(lblPortfolioTitle);
            pnlCurrency.Controls.Add(dgvMyPortfolio);

            pnlOperations.Controls.Add(pnlCurrency);
        }

        private void LoadCurrencyData()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();

                    // Döviz kurlarını yükle
                    string ratesQuery = "SELECT CurrencyCode as 'Kod', CurrencyName as 'Para Birimi', BuyRate as 'Alış', SellRate as 'Satış' FROM CurrencyRates ORDER BY CurrencyCode";
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(ratesQuery, conn))
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        da.Fill(dt);
                        dgvCurrencyRates.DataSource = dt;
                    }

                    // Müşteri portföyünü yükle
                    string portfolioQuery = @"SELECT cc.CurrencyCode as 'Para Birimi', 
                                                   cc.Amount as 'Miktar', 
                                                   cr.CurrencyName as 'Açıklama',
                                                   (cc.Amount * cr.SellRate) as 'TL Değeri'
                                            FROM CustomerCurrency cc 
                                            INNER JOIN CurrencyRates cr ON cc.CurrencyCode = cr.CurrencyCode
                                            INNER JOIN Customers c ON cc.CustomerId = c.Id 
                                            WHERE c.TcNo = @TcNo AND cc.Amount > 0
                                            ORDER BY cc.CurrencyCode";

                    using (SQLiteCommand cmd = new SQLiteCommand(portfolioQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            da.Fill(dt);
                            dgvMyPortfolio.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Döviz verileri yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuyCurrency_Click(object sender, EventArgs e)
        {
            // Döviz/Altın alma işlemi - placeholder
            MessageBox.Show("Döviz/Altın alma özelliği yakında eklenecek!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSellCurrency_Click(object sender, EventArgs e)
        {
            // Döviz/Altın satma işlemi - placeholder
            MessageBox.Show("Döviz/Altın satma özelliği yakında eklenecek!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetRecipientInfo(string accountNumber)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    string query = @"SELECT c.Name, c.Surname 
                                   FROM Customers c 
                                   INNER JOIN Accounts a ON c.Id = a.CustomerId 
                                   WHERE a.AccountNumber = @AccountNumber";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber.Trim());
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["Name"]?.ToString() ?? "";
                                string surname = reader["Surname"]?.ToString() ?? "";
                                return $"👤 Hesap Sahibi: {name} {surname}";
                            }
                            else
                            {
                                return "⚠️ Hesap sahibi bilgisi bulunamadı";
                            }
                        }
                    }
                }
            }
            catch
            {
                return "❌ Bilgi alınamadı";
            }
        }

        private void CheckRecipientName(string accountNumber, Label lblRecipientName)
        {
            if (string.IsNullOrWhiteSpace(accountNumber) || accountNumber.Length < 10)
            {
                lblRecipientName.Text = "👤 Alıcı: Hesap numarası girin";
                lblRecipientName.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    string query = @"SELECT c.Name, c.Surname 
                                   FROM Customers c 
                                   INNER JOIN Accounts a ON c.Id = a.CustomerId 
                                   WHERE a.AccountNumber = @AccountNumber";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber.Trim());
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["Name"]?.ToString() ?? "";
                                string surname = reader["Surname"]?.ToString() ?? "";
                                lblRecipientName.Text = $"👤 Alıcı: {name} {surname}";
                                lblRecipientName.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
                            }
                            else
                            {
                                lblRecipientName.Text = "⚠️ Hesap bulunamadı";
                                lblRecipientName.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblRecipientName.Text = $"❌ Hata: {ex.Message}";
                lblRecipientName.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
            }
        }

        private void btnCopyAccountInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAccounts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Lütfen kopyalamak istediğiniz hesabı seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Seçili satırdan hesap numarasını al
                DataGridViewRow selectedRow = dgvAccounts.SelectedRows[0];
                string accountNumber = selectedRow.Cells["Hesap Numarası"]?.Value?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    MessageBox.Show("Hesap numarası bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Panoya kopyala
                Clipboard.SetText(accountNumber);
                MessageBox.Show($"Hesap numarası panoya kopyalandı!\n\nKopyalanan: {accountNumber}", "Kopyalandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kopyalama sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateDatabaseAndTables()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                
                // Müşteriler tablosu
                string createCustomerTable = @"
                    CREATE TABLE IF NOT EXISTS Customers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Surname TEXT NOT NULL,
                        TcNo TEXT NOT NULL UNIQUE,
                        Phone TEXT,
                        Password TEXT NOT NULL
                    );";

                // Hesaplar tablosu
                string createAccountsTable = @"
                    CREATE TABLE IF NOT EXISTS Accounts (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerId INTEGER NOT NULL,
                        AccountNumber TEXT NOT NULL UNIQUE,
                        Balance DECIMAL(10,2) DEFAULT 0.00,
                        AccountType TEXT DEFAULT 'Vadeli',
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        MaturityDate DATETIME,
                        InterestRate DECIMAL(5,2) DEFAULT 0.00,
                        MaturityAmount DECIMAL(10,2) DEFAULT 0.00,
                        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
                    );";

                // İşlemler tablosu
                string createTransactionsTable = @"
                    CREATE TABLE IF NOT EXISTS Transactions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        AccountId INTEGER NOT NULL,
                        TransactionType TEXT NOT NULL,
                        Amount DECIMAL(10,2) NOT NULL,
                        Description TEXT,
                        TransactionDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
                    );";

                // Kredi kartları tablosu
                string createCreditCardsTable = @"
                    CREATE TABLE IF NOT EXISTS CreditCards (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerId INTEGER NOT NULL,
                        CardNumber TEXT NOT NULL UNIQUE,
                        CardName TEXT NOT NULL,
                        CreditLimit DECIMAL(10,2) DEFAULT 0.00,
                        AvailableLimit DECIMAL(10,2) DEFAULT 0.00,
                        Debt DECIMAL(10,2) DEFAULT 0.00,
                        InterestRate DECIMAL(5,2) DEFAULT 2.50,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        ExpiryDate DATETIME,
                        IsActive INTEGER DEFAULT 1,
                        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
                    );";

                // Kredi kartı işlemleri tablosu
                string createCreditCardTransactionsTable = @"
                    CREATE TABLE IF NOT EXISTS CreditCardTransactions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CreditCardId INTEGER NOT NULL,
                        TransactionType TEXT NOT NULL,
                        Amount DECIMAL(10,2) NOT NULL,
                        Description TEXT,
                        MerchantName TEXT,
                        TransactionDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (CreditCardId) REFERENCES CreditCards(Id)
                    );";

                // Döviz kurları tablosu
                string createCurrencyRatesTable = @"
                    CREATE TABLE IF NOT EXISTS CurrencyRates (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CurrencyCode TEXT NOT NULL,
                        CurrencyName TEXT NOT NULL,
                        BuyRate DECIMAL(10,4) NOT NULL,
                        SellRate DECIMAL(10,4) NOT NULL,
                        LastUpdated DATETIME DEFAULT CURRENT_TIMESTAMP
                    );";

                // Müşteri döviz portföyü tablosu
                string createCustomerCurrencyTable = @"
                    CREATE TABLE IF NOT EXISTS CustomerCurrency (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerId INTEGER NOT NULL,
                        CurrencyCode TEXT NOT NULL,
                        Amount DECIMAL(15,6) DEFAULT 0.00,
                        TotalBuyValue DECIMAL(15,2) DEFAULT 0.00,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
                    );";

                // Döviz işlemleri tablosu
                string createCurrencyTransactionsTable = @"
                    CREATE TABLE IF NOT EXISTS CurrencyTransactions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerId INTEGER NOT NULL,
                        CurrencyCode TEXT NOT NULL,
                        TransactionType TEXT NOT NULL,  -- BUY, SELL
                        Amount DECIMAL(15,6) NOT NULL,
                        Rate DECIMAL(10,4) NOT NULL,
                        TLAmount DECIMAL(15,2) NOT NULL,
                        Description TEXT,
                        TransactionDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
                    );";

                SQLiteCommand cmd1 = new SQLiteCommand(createCustomerTable, conn);
                cmd1.ExecuteNonQuery();

                SQLiteCommand cmd2 = new SQLiteCommand(createAccountsTable, conn);
                cmd2.ExecuteNonQuery();

                SQLiteCommand cmd3 = new SQLiteCommand(createTransactionsTable, conn);
                cmd3.ExecuteNonQuery();

                SQLiteCommand cmd4 = new SQLiteCommand(createCreditCardsTable, conn);
                cmd4.ExecuteNonQuery();

                SQLiteCommand cmd5 = new SQLiteCommand(createCreditCardTransactionsTable, conn);
                cmd5.ExecuteNonQuery();

                SQLiteCommand cmd6 = new SQLiteCommand(createCurrencyRatesTable, conn);
                cmd6.ExecuteNonQuery();

                SQLiteCommand cmd7 = new SQLiteCommand(createCustomerCurrencyTable, conn);
                cmd7.ExecuteNonQuery();

                SQLiteCommand cmd8 = new SQLiteCommand(createCurrencyTransactionsTable, conn);
                cmd8.ExecuteNonQuery();

                // Temel döviz kurlarını ekle
                InitializeCurrencyRates(conn);

                conn.Close();
            }
        }

        // Temel döviz kurlarını başlat
        private void InitializeCurrencyRates(SQLiteConnection conn)
        {
            try
            {
                // Önce mevcut kurları kontrol et
                string checkQuery = "SELECT COUNT(*) FROM CurrencyRates";
                using (SQLiteCommand cmd = new SQLiteCommand(checkQuery, conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0) return; // Zaten kurlar mevcut
                }

                // Temel döviz kurlarını ekle
                string[] currencies = {
                    "INSERT INTO CurrencyRates (CurrencyCode, CurrencyName, BuyRate, SellRate) VALUES ('USD', 'Amerikan Doları', 32.50, 32.80)",
                    "INSERT INTO CurrencyRates (CurrencyCode, CurrencyName, BuyRate, SellRate) VALUES ('EUR', 'Euro', 35.20, 35.60)", 
                    "INSERT INTO CurrencyRates (CurrencyCode, CurrencyName, BuyRate, SellRate) VALUES ('CHF', 'İsviçre Frangı', 36.80, 37.20)",
                    "INSERT INTO CurrencyRates (CurrencyCode, CurrencyName, BuyRate, SellRate) VALUES ('GOLD', 'Altın (gr)', 2650.00, 2680.00)"
                };

                foreach (string query in currencies)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda sessizce devam et
                Console.WriteLine($"Döviz kurları başlatma hatası: {ex.Message}");
            }
        }

        // Mevcut kredi kartlarının AvailableLimit değerlerini düzelt
        private void FixCreditCardLimits()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Önce mevcut durumu kontrol et
                    string checkQuery = @"SELECT Id, CardNumber, CreditLimit, AvailableLimit, Debt 
                                        FROM CreditCards 
                                        WHERE IsActive = 1";
                    
                    bool needsFix = false;
                    string reportMsg = "🔍 Kredi Kartı Durum Raporu:\n\n";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(checkQuery, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                decimal creditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                                decimal availableLimit = Convert.ToDecimal(reader["AvailableLimit"]);
                                decimal debt = Convert.ToDecimal(reader["Debt"]);
                                decimal expectedAvailable = creditLimit - debt;
                                
                                string cardNumber = reader["CardNumber"].ToString();
                                
                                reportMsg += $"💳 Kart: {cardNumber}\n";
                                reportMsg += $"   • Kredi Limiti: {creditLimit:C}\n";
                                reportMsg += $"   • Mevcut Borç: {debt:C}\n";
                                reportMsg += $"   • Kullanılabilir Limit: {availableLimit:C}\n";
                                reportMsg += $"   • Olması Gereken: {expectedAvailable:C}\n";
                                
                                if (Math.Abs(availableLimit - expectedAvailable) > 0.01m)
                                {
                                    reportMsg += $"   ⚠️ DÜZELTİLMESİ GEREKİYOR!\n";
                                    needsFix = true;
                                }
                                else
                                {
                                    reportMsg += $"   ✅ Doğru\n";
                                }
                                reportMsg += "\n";
                            }
                        }
                    }
                    
                    MessageBox.Show(reportMsg, "Kredi Kartı Durum Raporu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    if (needsFix)
                    {
                        // Tüm kartları düzelt
                        string updateQuery = @"UPDATE CreditCards 
                                             SET AvailableLimit = CreditLimit - Debt 
                                             WHERE IsActive = 1";
                        
                        using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn))
                        {
                            int updatedCount = cmd.ExecuteNonQuery();
                            MessageBox.Show($"✅ {updatedCount} kredi kartının limit bilgileri düzeltildi.", "Düzeltme Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Limit kontrolü sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kredi kartı verilerini yükleme metodu
        private void LoadCreditCards()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    string query = @"SELECT cc.CardNumber, cc.CardName, cc.CreditLimit, cc.AvailableLimit, 
                                          cc.Debt, cc.InterestRate, cc.ExpiryDate, cc.IsActive
                                   FROM CreditCards cc 
                                   INNER JOIN Customers c ON cc.CustomerId = c.Id 
                                   WHERE c.TcNo = @TcNo AND cc.IsActive = 1";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            da.Fill(dt);
                            
                            // Kolon başlıklarını Türkçe yap
                            if (dt.Columns.Contains("CardNumber")) dt.Columns["CardNumber"].ColumnName = "Kart Numarası";
                            if (dt.Columns.Contains("CardName")) dt.Columns["CardName"].ColumnName = "Kart Adı";
                            if (dt.Columns.Contains("CreditLimit")) dt.Columns["CreditLimit"].ColumnName = "Kredi Limiti";
                            if (dt.Columns.Contains("AvailableLimit")) dt.Columns["AvailableLimit"].ColumnName = "Kullanılabilir Limit";
                            if (dt.Columns.Contains("Debt")) dt.Columns["Debt"].ColumnName = "Borç";
                            if (dt.Columns.Contains("InterestRate")) dt.Columns["InterestRate"].ColumnName = "Faiz Oranı (%)";
                            if (dt.Columns.Contains("ExpiryDate")) dt.Columns["ExpiryDate"].ColumnName = "Son Kullanma";
                            
                            dgvCreditCards.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kredi kartları yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kredi kartı başvuru event handler
        private void btnApplyCreditCard_Click(object sender, EventArgs e)
        {
            try
            {
                // Kredi kartı başvuru formu
                Form applyForm = new Form();
                applyForm.Text = "💳 Kredi Kartı Başvurusu";
                applyForm.Size = new System.Drawing.Size(400, 300);
                applyForm.StartPosition = FormStartPosition.CenterParent;
                applyForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                applyForm.MaximizeBox = false;

                Label lblCardName = new Label();
                lblCardName.Text = "Kart Adı:";
                lblCardName.Location = new System.Drawing.Point(20, 20);
                lblCardName.AutoSize = true;

                TextBox txtCardName = new TextBox();
                txtCardName.Location = new System.Drawing.Point(20, 45);
                txtCardName.Size = new System.Drawing.Size(300, 25);

                Label lblCreditLimit = new Label();
                lblCreditLimit.Text = "Talep Edilen Kredi Limiti:";
                lblCreditLimit.Location = new System.Drawing.Point(20, 80);
                lblCreditLimit.AutoSize = true;

                TextBox txtCreditLimit = new TextBox();
                txtCreditLimit.Location = new System.Drawing.Point(20, 105);
                txtCreditLimit.Size = new System.Drawing.Size(300, 25);

                Button btnSubmitApply = new Button();
                btnSubmitApply.Text = "Başvuru Yap";
                btnSubmitApply.BackColor = System.Drawing.Color.Green;
                btnSubmitApply.ForeColor = System.Drawing.Color.White;
                btnSubmitApply.Location = new System.Drawing.Point(20, 150);
                btnSubmitApply.Size = new System.Drawing.Size(100, 30);

                btnSubmitApply.Click += (s, args) => {
                    if (string.IsNullOrEmpty(txtCardName.Text) || string.IsNullOrEmpty(txtCreditLimit.Text))
                    {
                        MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!decimal.TryParse(txtCreditLimit.Text, out decimal creditLimit) || creditLimit <= 0)
                    {
                        MessageBox.Show("Geçerli bir kredi limiti girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Kart numarası oluştur (scope dışına taşındı)
                    Random rnd = new Random();
                    string cardNumber = $"4{rnd.Next(100, 999)}-{rnd.Next(1000, 9999)}-{rnd.Next(1000, 9999)}-{rnd.Next(1000, 9999)}";

                    // Kredi kartı oluştur
                    using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                    {
                        conn.Open();

                        // Müşteri ID'sini al
                        string getCustomerIdQuery = "SELECT Id FROM Customers WHERE TcNo = @TcNo";
                        int customerId = 0;
                        using (SQLiteCommand cmd = new SQLiteCommand(getCustomerIdQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                            object result = cmd.ExecuteScalar();
                            if (result != null) customerId = Convert.ToInt32(result);
                        }

                        // Son kullanma tarihi (3 yıl sonra)
                        DateTime expiryDate = DateTime.Now.AddYears(3);

                        string insertQuery = @"INSERT INTO CreditCards (CustomerId, CardNumber, CardName, CreditLimit, AvailableLimit, Debt, InterestRate, ExpiryDate) 
                                             VALUES (@CustomerId, @CardNumber, @CardName, @CreditLimit, @AvailableLimit, 0, 2.50, @ExpiryDate)";

                        using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@CustomerId", customerId);
                            cmd.Parameters.AddWithValue("@CardNumber", cardNumber);
                            cmd.Parameters.AddWithValue("@CardName", txtCardName.Text);
                            cmd.Parameters.AddWithValue("@CreditLimit", creditLimit);
                            cmd.Parameters.AddWithValue("@AvailableLimit", creditLimit); // Ayrı parametre eklendi
                            cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"Kredi kartı başvurunuz onaylandı!\nKart Numarası: {cardNumber}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    applyForm.Close();
                    LoadCreditCards(); // Listeyi yenile
                };

                applyForm.Controls.Add(lblCardName);
                applyForm.Controls.Add(txtCardName);
                applyForm.Controls.Add(lblCreditLimit);
                applyForm.Controls.Add(txtCreditLimit);
                applyForm.Controls.Add(btnSubmitApply);

                applyForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kredi kartı başvurusu sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kredi kartı ödeme event handler
        private void btnCreditCardPayment_Click(object sender, EventArgs e)
        {
            try
            {
                // Önce mevcut kredi kartlarını al
                List<string> creditCards = new List<string>();
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string query = @"SELECT cc.Id, cc.CardNumber, cc.CardName, cc.Debt 
                                   FROM CreditCards cc 
                                   INNER JOIN Customers c ON cc.CustomerId = c.Id 
                                   WHERE c.TcNo = @TcNo AND cc.IsActive = 1";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string cardInfo = $"{reader["CardNumber"]} - {reader["CardName"]} (Borç: {reader["Debt"]:C})";
                                creditCards.Add(cardInfo);
                            }
                        }
                    }
                }

                if (creditCards.Count == 0)
                {
                    MessageBox.Show("Henüz kredi kartınız bulunmuyor!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kullanıcının hesaplarını al
                List<string> userAccounts = new List<string>();
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string accountQuery = @"SELECT a.AccountNumber, a.Balance 
                                          FROM Accounts a 
                                          INNER JOIN Customers c ON a.CustomerId = c.Id 
                                          WHERE c.TcNo = @TcNo";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(accountQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string accountInfo = $"{reader["AccountNumber"]} (Bakiye: {reader["Balance"]:C})";
                                userAccounts.Add(accountInfo);
                            }
                        }
                    }
                }

                if (userAccounts.Count == 0)
                {
                    MessageBox.Show("Ödeme yapacak hesabınız bulunmuyor!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ödeme formu
                Form paymentForm = new Form();
                paymentForm.Text = "💰 Kredi Kartı Ödeme";
                paymentForm.Size = new System.Drawing.Size(450, 350);
                paymentForm.StartPosition = FormStartPosition.CenterParent;
                paymentForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                paymentForm.MaximizeBox = false;

                Label lblSelectCard = new Label();
                lblSelectCard.Text = "💳 Ödenecek Kart:";
                lblSelectCard.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                lblSelectCard.Location = new System.Drawing.Point(20, 20);
                lblSelectCard.AutoSize = true;

                ComboBox cmbCards = new ComboBox();
                cmbCards.Location = new System.Drawing.Point(20, 45);
                cmbCards.Size = new System.Drawing.Size(380, 25);
                cmbCards.DropDownStyle = ComboBoxStyle.DropDownList;
                creditCards.ForEach(card => cmbCards.Items.Add(card));

                // Güncel borç bilgisi
                Label lblCurrentDebt = new Label();
                lblCurrentDebt.Text = "💰 Güncel Borç: Kart seçiniz";
                lblCurrentDebt.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                lblCurrentDebt.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
                lblCurrentDebt.Location = new System.Drawing.Point(20, 80);
                lblCurrentDebt.AutoSize = true;

                // Kart seçimi değiştiğinde borç bilgisini güncelle
                cmbCards.SelectedIndexChanged += (s, e) => {
                    if (cmbCards.SelectedItem != null)
                    {
                        string selectedCard = cmbCards.SelectedItem.ToString();
                        // Borç bilgisini selectedCard string'inden çıkar
                        var match = System.Text.RegularExpressions.Regex.Match(selectedCard, @"Borç: ([\d.,]+)");
                        if (match.Success)
                        {
                            lblCurrentDebt.Text = $"💰 Güncel Borç: ₺{match.Groups[1].Value}";
                        }
                    }
                };

                Label lblSelectAccount = new Label();
                lblSelectAccount.Text = "🏦 Ödeme Yapılacak Hesap:";
                lblSelectAccount.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                lblSelectAccount.Location = new System.Drawing.Point(20, 115);
                lblSelectAccount.AutoSize = true;

                ComboBox cmbAccounts = new ComboBox();
                cmbAccounts.Location = new System.Drawing.Point(20, 140);
                cmbAccounts.Size = new System.Drawing.Size(380, 25);
                cmbAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
                userAccounts.ForEach(account => cmbAccounts.Items.Add(account));

                Label lblPaymentAmount = new Label();
                lblPaymentAmount.Text = "💰 Ödeme Tutarı:";
                lblPaymentAmount.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                lblPaymentAmount.Location = new System.Drawing.Point(20, 175);
                lblPaymentAmount.AutoSize = true;

                TextBox txtPaymentAmount = new TextBox();
                txtPaymentAmount.Location = new System.Drawing.Point(20, 200);
                txtPaymentAmount.Size = new System.Drawing.Size(380, 25);

                Button btnSubmitPayment = new Button();
                btnSubmitPayment.Text = "💰 Ödeme Yap";
                btnSubmitPayment.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
                btnSubmitPayment.ForeColor = System.Drawing.Color.White;
                btnSubmitPayment.FlatStyle = FlatStyle.Flat;
                btnSubmitPayment.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                btnSubmitPayment.Location = new System.Drawing.Point(20, 240);
                btnSubmitPayment.Size = new System.Drawing.Size(150, 35);

                btnSubmitPayment.Click += (s, args) => {
                    if (cmbCards.SelectedIndex == -1 || cmbAccounts.SelectedIndex == -1 || string.IsNullOrEmpty(txtPaymentAmount.Text))
                    {
                        MessageBox.Show("Lütfen kart seçin, hesap seçin ve ödeme tutarını girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!decimal.TryParse(txtPaymentAmount.Text, out decimal paymentAmount) || paymentAmount <= 0)
                    {
                        MessageBox.Show("Geçerli bir ödeme tutarı girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Seçilen kartın bilgilerini al - DÜZELTME
                    string selectedCard = cmbCards.SelectedItem.ToString();
                    // "4293-3227-8044-2030 - Samet Çiftci (Borç: ₺20,000.00)" formatından kart numarasını al
                    string cardNumber = "";
                    if (selectedCard.Contains(" - "))
                    {
                        cardNumber = selectedCard.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim();
                    }
                    else
                    {
                        cardNumber = selectedCard.Split('-')[0].Trim();
                    }

                    // Seçilen hesap bilgilerini al
                    string selectedAccount = cmbAccounts.SelectedItem.ToString();
                    string accountNumber = selectedAccount.Split(new char[] { '(' })[0].Trim();

                    // Değişkenleri scope dışında tanımla
                    decimal accountBalance = 0;
                    decimal currentDebt = 0;

                    // Ödeme işlemi
                    using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                    {
                        conn.Open();

                        // Hesap bakiyesini kontrol et
                        string checkAccountQuery = "SELECT Id, Balance FROM Accounts WHERE AccountNumber = @AccountNumber";
                        int accountId = 0;

                        using (SQLiteCommand cmd = new SQLiteCommand(checkAccountQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    accountId = Convert.ToInt32(reader["Id"]);
                                    accountBalance = Convert.ToDecimal(reader["Balance"]);
                                }
                            }
                        }

                        if (paymentAmount > accountBalance)
                        {
                            MessageBox.Show($"Hesabınızda yeterli bakiye yok!\nMevcut Bakiye: {accountBalance:C}\nÖdeme Tutarı: {paymentAmount:C}", "Yetersiz Bakiye", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Mevcut borcu kontrol et
                        string checkDebtQuery = "SELECT Id, Debt FROM CreditCards WHERE CardNumber = @CardNumber";
                        int cardId = 0;

                        using (SQLiteCommand cmd = new SQLiteCommand(checkDebtQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@CardNumber", cardNumber);
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    cardId = Convert.ToInt32(reader["Id"]);
                                    currentDebt = Convert.ToDecimal(reader["Debt"]);
                                }
                            }
                        }

                        if (paymentAmount > currentDebt)
                        {
                            MessageBox.Show($"Ödeme tutarı mevcut borçtan ({currentDebt:C}) fazla olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Hesaptan para çek
                        string updateAccountQuery = "UPDATE Accounts SET Balance = Balance - @PaymentAmount WHERE Id = @AccountId";
                        using (SQLiteCommand cmd = new SQLiteCommand(updateAccountQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                            cmd.Parameters.AddWithValue("@AccountId", accountId);
                            cmd.ExecuteNonQuery();
                        }

                        // Hesap işlem kaydı ekle
                        string insertAccountTransactionQuery = @"INSERT INTO Transactions (AccountId, TransactionType, Amount, Description) 
                                                               VALUES (@AccountId, 'Kredi Kartı Ödemesi', @Amount, @Description)";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertAccountTransactionQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@AccountId", accountId);
                            cmd.Parameters.AddWithValue("@Amount", paymentAmount);
                            cmd.Parameters.AddWithValue("@Description", $"Kredi kartı ödemesi - Kart: {cardNumber}");
                            cmd.ExecuteNonQuery();
                        }

                        // Borcu güncelle
                        string updateDebtQuery = "UPDATE CreditCards SET Debt = Debt - @PaymentAmount, AvailableLimit = AvailableLimit + @PaymentAmount WHERE Id = @CardId";
                        using (SQLiteCommand cmd = new SQLiteCommand(updateDebtQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                            cmd.Parameters.AddWithValue("@CardId", cardId);
                            cmd.ExecuteNonQuery();
                        }

                        // Kredi kartı işlem kaydı ekle
                        string insertTransactionQuery = @"INSERT INTO CreditCardTransactions (CreditCardId, TransactionType, Amount, Description) 
                                                        VALUES (@CardId, 'Ödeme', @Amount, @Description)";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertTransactionQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@CardId", cardId);
                            cmd.Parameters.AddWithValue("@Amount", paymentAmount);
                            cmd.Parameters.AddWithValue("@Description", $"Borç ödemesi - Hesap: {accountNumber}");
                            cmd.ExecuteNonQuery();
                        }
                    }

                    decimal newBalance = accountBalance - paymentAmount;
                    decimal newDebt = currentDebt - paymentAmount;
                    
                    MessageBox.Show($"✅ Ödeme başarılı!\n\n💰 Ödenen Tutar: {paymentAmount:C}\n🏦 Hesap: {accountNumber}\n💳 Kart: {cardNumber}\n\n📊 Güncel Durum:\n• Hesap Bakiyesi: {newBalance:C}\n• Kart Borcu: {newDebt:C}", 
                        "Ödeme Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    paymentForm.Close();
                    LoadCreditCards(); // Listeyi yenile
                };

                paymentForm.Controls.Add(lblSelectCard);
                paymentForm.Controls.Add(cmbCards);
                paymentForm.Controls.Add(lblCurrentDebt);
                paymentForm.Controls.Add(lblSelectAccount);
                paymentForm.Controls.Add(cmbAccounts);
                paymentForm.Controls.Add(lblPaymentAmount);
                paymentForm.Controls.Add(txtPaymentAmount);
                paymentForm.Controls.Add(btnSubmitPayment);

                paymentForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kredi kartı ödemesi sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kredi kartı harcama/işlemler event handler
        private void btnCreditCardTransactions_Click(object sender, EventArgs e)
        {
            try
            {
                // Önce mevcut kredi kartlarını al
                List<string> creditCards = new List<string>();
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string query = @"SELECT cc.Id, cc.CardNumber, cc.CardName, cc.AvailableLimit 
                                   FROM CreditCards cc 
                                   INNER JOIN Customers c ON cc.CustomerId = c.Id 
                                   WHERE c.TcNo = @TcNo AND cc.IsActive = 1";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string cardInfo = $"{reader["CardNumber"]} - {reader["CardName"]} (Limit: {reader["AvailableLimit"]:C})";
                                creditCards.Add(cardInfo);
                            }
                        }
                    }
                }

                if (creditCards.Count == 0)
                {
                    MessageBox.Show("Henüz kredi kartınız bulunmuyor!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Harcama formu
                Form transactionForm = new Form();
                transactionForm.Text = "🛒 Kredi Kartı ile Harcama";
                transactionForm.Size = new System.Drawing.Size(400, 320);
                transactionForm.StartPosition = FormStartPosition.CenterParent;
                transactionForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                transactionForm.MaximizeBox = false;

                Label lblSelectCard = new Label();
                lblSelectCard.Text = "Kart Seçin:";
                lblSelectCard.Location = new System.Drawing.Point(20, 20);
                lblSelectCard.AutoSize = true;

                ComboBox cmbCards = new ComboBox();
                cmbCards.Location = new System.Drawing.Point(20, 45);
                cmbCards.Size = new System.Drawing.Size(300, 25);
                cmbCards.DropDownStyle = ComboBoxStyle.DropDownList;
                creditCards.ForEach(card => cmbCards.Items.Add(card));

                Label lblMerchant = new Label();
                lblMerchant.Text = "Mağaza Adı:";
                lblMerchant.Location = new System.Drawing.Point(20, 80);
                lblMerchant.AutoSize = true;

                TextBox txtMerchant = new TextBox();
                txtMerchant.Location = new System.Drawing.Point(20, 105);
                txtMerchant.Size = new System.Drawing.Size(300, 25);

                Label lblAmount = new Label();
                lblAmount.Text = "Harcama Tutarı:";
                lblAmount.Location = new System.Drawing.Point(20, 140);
                lblAmount.AutoSize = true;

                TextBox txtAmount = new TextBox();
                txtAmount.Location = new System.Drawing.Point(20, 165);
                txtAmount.Size = new System.Drawing.Size(300, 25);

                Label lblDescription = new Label();
                lblDescription.Text = "Açıklama:";
                lblDescription.Location = new System.Drawing.Point(20, 200);
                lblDescription.AutoSize = true;

                TextBox txtDescription = new TextBox();
                txtDescription.Location = new System.Drawing.Point(20, 225);
                txtDescription.Size = new System.Drawing.Size(300, 25);

                Button btnSubmitTransaction = new Button();
                btnSubmitTransaction.Text = "Harcama Yap";
                btnSubmitTransaction.BackColor = System.Drawing.Color.Red;
                btnSubmitTransaction.ForeColor = System.Drawing.Color.White;
                btnSubmitTransaction.Location = new System.Drawing.Point(20, 260);
                btnSubmitTransaction.Size = new System.Drawing.Size(100, 30);

                btnSubmitTransaction.Click += (s, args) => {
                    if (cmbCards.SelectedIndex == -1 || string.IsNullOrEmpty(txtMerchant.Text) || string.IsNullOrEmpty(txtAmount.Text))
                    {
                        MessageBox.Show("Lütfen tüm zorunlu alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
                    {
                        MessageBox.Show("Geçerli bir harcama tutarı girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Seçilen kartın bilgilerini al - DÜZELTME
                    string selectedCard = cmbCards.SelectedItem.ToString();
                    // "4293-3227-8044-2030 - Samet Çiftci (Limit: ₺20,000.00)" formatından kart numarasını al
                    string cardNumber = "";
                    if (selectedCard.Contains(" - "))
                    {
                        cardNumber = selectedCard.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim();
                    }
                    else
                    {
                        cardNumber = selectedCard.Split('-')[0].Trim();
                    }

                    // Harcama işlemi
                    using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                    {
                        conn.Open();

                        // Mevcut limiti kontrol et - Daha detaylı sorgu
                        string checkLimitQuery = "SELECT Id, CardNumber, CreditLimit, AvailableLimit, Debt FROM CreditCards WHERE CardNumber = @CardNumber AND IsActive = 1";
                        decimal availableLimit = 0;
                        decimal creditLimit = 0;
                        decimal debt = 0;
                        int cardId = 0;
                        string foundCardNumber = "";

                        using (SQLiteCommand cmd = new SQLiteCommand(checkLimitQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@CardNumber", cardNumber);
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    cardId = Convert.ToInt32(reader["Id"]);
                                    foundCardNumber = reader["CardNumber"].ToString();
                                    creditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                                    availableLimit = Convert.ToDecimal(reader["AvailableLimit"]);
                                    debt = Convert.ToDecimal(reader["Debt"]);
                                }
                            }
                        }

                        // Debug - kaldırılabilir
                        // MessageBox.Show($"Debug: Kart ID: {cardId}, Limit: {availableLimit:C}, Harcama: {amount:C}", "Debug");

                        // Eğer kart bulunamadıysa
                        if (cardId == 0)
                        {
                            MessageBox.Show($"❌ Kart bulunamadı!\n\nAranan kart numarası: '{cardNumber}'\n\nLütfen kredi kartı listesini yenileyin.", "Kart Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (amount > availableLimit)
                        {
                            MessageBox.Show($"⚠️ Harcama tutarı kullanılabilir limitten fazla!\n\n" +
                                          $"💳 Kullanılabilir Limit: {availableLimit:C}\n" +
                                          $"💰 Harcama Tutarı: {amount:C}\n" +
                                          $"🚫 Fark: {(amount - availableLimit):C}", 
                                          "Limit Aşımı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Limiti güncelle ve borcu artır
                        string updateLimitQuery = "UPDATE CreditCards SET AvailableLimit = AvailableLimit - @Amount, Debt = Debt + @Amount WHERE Id = @CardId";
                        using (SQLiteCommand cmd = new SQLiteCommand(updateLimitQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Amount", amount);
                            cmd.Parameters.AddWithValue("@CardId", cardId);
                            cmd.ExecuteNonQuery();
                        }

                        // İşlem kaydı ekle
                        string insertTransactionQuery = @"INSERT INTO CreditCardTransactions (CreditCardId, TransactionType, Amount, Description, MerchantName) 
                                                        VALUES (@CardId, 'Harcama', @Amount, @Description, @MerchantName)";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertTransactionQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@CardId", cardId);
                            cmd.Parameters.AddWithValue("@Amount", amount);
                            cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                            cmd.Parameters.AddWithValue("@MerchantName", txtMerchant.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"Harcama başarılı!\nMağaza: {txtMerchant.Text}\nTutar: {amount:C}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    transactionForm.Close();
                    LoadCreditCards(); // Listeyi yenile
                };

                transactionForm.Controls.Add(lblSelectCard);
                transactionForm.Controls.Add(cmbCards);
                transactionForm.Controls.Add(lblMerchant);
                transactionForm.Controls.Add(txtMerchant);
                transactionForm.Controls.Add(lblAmount);
                transactionForm.Controls.Add(txtAmount);
                transactionForm.Controls.Add(lblDescription);
                transactionForm.Controls.Add(txtDescription);
                transactionForm.Controls.Add(btnSubmitTransaction);

                transactionForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kredi kartı harcaması sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}