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
            pnlCreateAccount.Size = new System.Drawing.Size(600, 400);
            pnlCreateAccount.Location = new System.Drawing.Point(50, 100);
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
            
            btnCreateAccountSubmit = new Button();
            btnCreateAccountSubmit.Text = "✅ Hesap Aç";
            btnCreateAccountSubmit.BackColor = System.Drawing.Color.FromArgb(34, 139, 34);
            btnCreateAccountSubmit.ForeColor = System.Drawing.Color.White;
            btnCreateAccountSubmit.FlatStyle = FlatStyle.Flat;
            btnCreateAccountSubmit.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnCreateAccountSubmit.Location = new System.Drawing.Point(150, 130);
            btnCreateAccountSubmit.Size = new System.Drawing.Size(150, 40);
            btnCreateAccountSubmit.Click += btnCreateAccountSubmit_Click;
            
            pnlCreateAccount.Controls.Add(lblCreateAccountTitle);
            pnlCreateAccount.Controls.Add(lblAccountType);
            pnlCreateAccount.Controls.Add(cmbAccountType);
            pnlCreateAccount.Controls.Add(btnCreateAccountSubmit);
            
            pnlOperations.Controls.Add(pnlCreateAccount);
        }

        private void btnCreateAccountSubmit_Click(object sender, EventArgs e)
        {
            try
            {
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
                    
                    // Hesap oluştur
                    string createAccountQuery = "INSERT INTO Accounts (CustomerId, AccountNumber, AccountType) VALUES (@CustomerId, @AccountNumber, @AccountType)";
                    using (SQLiteCommand cmd = new SQLiteCommand(createAccountQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        cmd.Parameters.AddWithValue("@AccountType", cmbAccountType.Text);
                        cmd.ExecuteNonQuery();
                    }
                    
                    MessageBox.Show($"✅ Hesap başarıyla açıldı!\n\nHesap Numarası: {accountNumber}\nHesap Türü: {cmbAccountType.Text}", 
                        "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
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
            txtToAccount.Size = new System.Drawing.Size(300, 25);
            txtToAccount.PlaceholderText = "Örn: 1234-5678-9012";
            
            lblTransferAmount = new Label();
            lblTransferAmount.Text = "Transfer Tutarı:";
            lblTransferAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            lblTransferAmount.Location = new System.Drawing.Point(20, 180);
            lblTransferAmount.AutoSize = true;
            
            txtTransferAmount = new TextBox();
            txtTransferAmount.Font = new System.Drawing.Font("Segoe UI", 11);
            txtTransferAmount.Location = new System.Drawing.Point(150, 177);
            txtTransferAmount.Size = new System.Drawing.Size(200, 25);
            txtTransferAmount.PlaceholderText = "0.00";
            
            btnTransferSubmit = new Button();
            btnTransferSubmit.Text = "🔄 Transfer Yap";
            btnTransferSubmit.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            btnTransferSubmit.ForeColor = System.Drawing.Color.White;
            btnTransferSubmit.FlatStyle = FlatStyle.Flat;
            btnTransferSubmit.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnTransferSubmit.Location = new System.Drawing.Point(150, 230);
            btnTransferSubmit.Size = new System.Drawing.Size(150, 40);
            btnTransferSubmit.Click += btnTransferSubmit_Click;
            
            pnlTransferMoney.Controls.Add(lblTransferTitle);
            pnlTransferMoney.Controls.Add(lblFromAccount);
            pnlTransferMoney.Controls.Add(cmbFromAccount);
            pnlTransferMoney.Controls.Add(lblToAccount);
            pnlTransferMoney.Controls.Add(txtToAccount);
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
            dgvAccounts.Size = new System.Drawing.Size(600, 120);
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccounts.ReadOnly = true;
            dgvAccounts.AllowUserToAddRows = false;
            
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

                SQLiteCommand cmd1 = new SQLiteCommand(createCustomerTable, conn);
                cmd1.ExecuteNonQuery();

                SQLiteCommand cmd2 = new SQLiteCommand(createAccountsTable, conn);
                cmd2.ExecuteNonQuery();

                SQLiteCommand cmd3 = new SQLiteCommand(createTransactionsTable, conn);
                cmd3.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}