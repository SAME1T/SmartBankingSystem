using System;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;

namespace SmartBankingAutomation.Forms
{
    public partial class AccountOpenForm : Form
    {
        private string dbPath = "Data/SmartBank.db";
        private string currentTcNo = "";
        private string currentName = "";
        private string currentSurname = "";

        // UI Controls
        private Panel pnlCreateAccount;
        private Label lblCreateAccountTitle;
        private Label lblAccountType;
        private ComboBox cmbAccountType;
        private Button btnCreateAccountSubmit;

        public AccountOpenForm(string tcNo, string name, string surname)
        {
            currentTcNo = tcNo;
            currentName = name;
            currentSurname = surname;
            InitializeComponent();
            CreateAccountPanel();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "💳 Hesap Açma";
            this.Size = new System.Drawing.Size(700, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.White;
            
            this.ResumeLayout(false);
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
            btnCreateAccountSubmit.Click += BtnCreateAccountSubmit_Click;
            
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
            
            this.Controls.Add(pnlCreateAccount);
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

        private void BtnCreateAccountSubmit_Click(object sender, EventArgs e)
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
                        (CustomerId, AccountNumber, AccountType, Balance, InterestRate, MaturityAmount) 
                        VALUES (@CustomerId, @AccountNumber, @AccountType, @Balance, @InterestRate, @MaturityAmount)";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(createAccountQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        cmd.Parameters.AddWithValue("@AccountType", cmbAccountType.Text);
                        cmd.Parameters.AddWithValue("@Balance", initialAmount);
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
                    
                    // Formu kapat
                    this.DialogResult = DialogResult.OK;
                    this.Close();
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
            pnlAccountDetails.Location = new System.Drawing.Point(75, 100);
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
            
            int yPosition = 160;
            
            // Hesap Türü
            Label lblAccountTypeLabel = new Label();
            lblAccountTypeLabel.Text = "📊 Hesap Türü: " + accountType;
            lblAccountTypeLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblAccountTypeLabel.Location = new System.Drawing.Point(20, yPosition);
            lblAccountTypeLabel.AutoSize = true;
            
            yPosition += 30;
            
            // Bakiye
            Label lblBalanceLabel = new Label();
            lblBalanceLabel.Text = $"💰 Başlangıç Bakiyesi: {balance:C}";
            lblBalanceLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblBalanceLabel.Location = new System.Drawing.Point(20, yPosition);
            lblBalanceLabel.AutoSize = true;
            
            yPosition += 40;
            
            // Vadeli hesap için ek bilgiler
            if (accountType == "Vadeli" && maturityDate.HasValue)
            {
                // Vade Tarihi
                Label lblMaturityDateLabel = new Label();
                lblMaturityDateLabel.Text = $"📅 Vade Tarihi: {maturityDate.Value:dd.MM.yyyy}";
                lblMaturityDateLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                lblMaturityDateLabel.Location = new System.Drawing.Point(20, yPosition);
                lblMaturityDateLabel.AutoSize = true;
                
                yPosition += 30;
                
                // Faiz Oranı
                Label lblInterestRateLabel = new Label();
                lblInterestRateLabel.Text = $"💰 Faiz Oranı: %{interestRate:F1}";
                lblInterestRateLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                lblInterestRateLabel.ForeColor = System.Drawing.Color.FromArgb(0, 128, 0);
                lblInterestRateLabel.Location = new System.Drawing.Point(20, yPosition);
                lblInterestRateLabel.AutoSize = true;
                
                yPosition += 30;
                
                // Vade Sonu Tutarı
                Label lblMaturityAmountLabel = new Label();
                lblMaturityAmountLabel.Text = $"🎯 Vade Sonu Tutarı: {maturityAmount:C}";
                lblMaturityAmountLabel.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
                lblMaturityAmountLabel.ForeColor = System.Drawing.Color.FromArgb(0, 128, 0);
                lblMaturityAmountLabel.Location = new System.Drawing.Point(20, yPosition);
                lblMaturityAmountLabel.AutoSize = true;
                
                yPosition += 40;
                
                pnlAccountDetails.Controls.Add(lblMaturityDateLabel);
                pnlAccountDetails.Controls.Add(lblInterestRateLabel);
                pnlAccountDetails.Controls.Add(lblMaturityAmountLabel);
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
            pnlAccountDetails.Controls.Add(lblBalanceLabel);
            pnlAccountDetails.Controls.Add(btnClose);
            
            // Paneli forma ekle ve en öne getir
            this.Controls.Add(pnlAccountDetails);
            pnlAccountDetails.BringToFront();
        }
    }
}