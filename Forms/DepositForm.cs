using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SmartBankingAutomation.Forms
{
    public partial class DepositForm : Form
    {
        private string dbPath = "Data/SmartBank.db";
        private string currentTcNo = "";

        // UI Controls
        private Panel pnlDeposit;
        private Label lblDepositTitle;
        private Label lblDepositAccount;
        private ComboBox cmbDepositAccount;
        private Label lblDepositAmount;
        private TextBox txtDepositAmount;
        private Button btnDepositSubmit;

        public DepositForm(string tcNo)
        {
            currentTcNo = tcNo;
            InitializeComponent();
            CreateDepositPanel();
            LoadUserAccounts(cmbDepositAccount);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "💰 Para Yatırma";
            this.Size = new System.Drawing.Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.White;
            
            this.ResumeLayout(false);
        }

        private void CreateDepositPanel()
        {
            pnlDeposit = new Panel();
            pnlDeposit.Size = new System.Drawing.Size(600, 400);
            pnlDeposit.Location = new System.Drawing.Point(50, 50);
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
            cmbDepositAccount.Size = new System.Drawing.Size(350, 25);
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
            btnDepositSubmit.Click += BtnDepositSubmit_Click;
            
            pnlDeposit.Controls.Add(lblDepositTitle);
            pnlDeposit.Controls.Add(lblDepositAccount);
            pnlDeposit.Controls.Add(cmbDepositAccount);
            pnlDeposit.Controls.Add(lblDepositAmount);
            pnlDeposit.Controls.Add(txtDepositAmount);
            pnlDeposit.Controls.Add(btnDepositSubmit);
            
            this.Controls.Add(pnlDeposit);
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

        private void BtnDepositSubmit_Click(object sender, EventArgs e)
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