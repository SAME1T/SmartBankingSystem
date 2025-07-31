using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SmartBankingAutomation.Forms
{
    public partial class WithdrawalForm : Form
    {
        private string dbPath = "Data/SmartBank.db";
        private string currentTcNo = "";

        // UI Controls
        private Panel pnlWithdraw;
        private Label lblWithdrawTitle;
        private Label lblWithdrawAccount;
        private ComboBox cmbWithdrawAccount;
        private Label lblWithdrawAmount;
        private TextBox txtWithdrawAmount;
        private Button btnWithdrawSubmit;

        public WithdrawalForm(string tcNo)
        {
            currentTcNo = tcNo;
            InitializeComponent();
            CreateWithdrawPanel();
            LoadUserAccounts(cmbWithdrawAccount);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "💸 Para Çekme";
            this.Size = new System.Drawing.Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.White;
            
            this.ResumeLayout(false);
        }

        private void CreateWithdrawPanel()
        {
            pnlWithdraw = new Panel();
            pnlWithdraw.Size = new System.Drawing.Size(600, 400);
            pnlWithdraw.Location = new System.Drawing.Point(50, 50);
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
            cmbWithdrawAccount.Size = new System.Drawing.Size(350, 25);
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
            btnWithdrawSubmit.Click += BtnWithdrawSubmit_Click;
            
            pnlWithdraw.Controls.Add(lblWithdrawTitle);
            pnlWithdraw.Controls.Add(lblWithdrawAccount);
            pnlWithdraw.Controls.Add(cmbWithdrawAccount);
            pnlWithdraw.Controls.Add(lblWithdrawAmount);
            pnlWithdraw.Controls.Add(txtWithdrawAmount);
            pnlWithdraw.Controls.Add(btnWithdrawSubmit);
            
            this.Controls.Add(pnlWithdraw);
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

        private void BtnWithdrawSubmit_Click(object sender, EventArgs e)
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
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}