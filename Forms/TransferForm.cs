using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SmartBankingAutomation.Forms
{
    public partial class TransferForm : Form
    {
        private string dbPath = "Data/SmartBank.db";
        private string currentTcNo = "";

        // UI Controls
        private Panel pnlTransferMoney;
        private Label lblTransferTitle;
        private Label lblFromAccount;
        private ComboBox cmbFromAccount;
        private Label lblToAccount;
        private TextBox txtToAccount;
        private Label lblTransferAmount;
        private TextBox txtTransferAmount;
        private Button btnTransferSubmit;

        public TransferForm(string tcNo)
        {
            currentTcNo = tcNo;
            InitializeComponent();
            CreateTransferPanel();
            LoadUserAccounts(cmbFromAccount);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "🔄 Para Transfer";
            this.Size = new System.Drawing.Size(700, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.White;
            
            this.ResumeLayout(false);
        }

        private void CreateTransferPanel()
        {
            pnlTransferMoney = new Panel();
            pnlTransferMoney.Size = new System.Drawing.Size(600, 500);
            pnlTransferMoney.Location = new System.Drawing.Point(50, 50);
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
            cmbFromAccount.Size = new System.Drawing.Size(350, 25);
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
            
            // Kopyala/Yapıştır butonları
            Button btnCopyAccountTransfer = new Button();
            btnCopyAccountTransfer.Text = "📋";
            btnCopyAccountTransfer.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnCopyAccountTransfer.ForeColor = System.Drawing.Color.White;
            btnCopyAccountTransfer.FlatStyle = FlatStyle.Flat;
            btnCopyAccountTransfer.Font = new System.Drawing.Font("Segoe UI", 10);
            btnCopyAccountTransfer.Location = new System.Drawing.Point(460, 127);
            btnCopyAccountTransfer.Size = new System.Drawing.Size(35, 25);
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
            
            Button btnPasteAccountTransfer = new Button();
            btnPasteAccountTransfer.Text = "📥";
            btnPasteAccountTransfer.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnPasteAccountTransfer.ForeColor = System.Drawing.Color.White;
            btnPasteAccountTransfer.FlatStyle = FlatStyle.Flat;
            btnPasteAccountTransfer.Font = new System.Drawing.Font("Segoe UI", 10);
            btnPasteAccountTransfer.Location = new System.Drawing.Point(505, 127);
            btnPasteAccountTransfer.Size = new System.Drawing.Size(35, 25);
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
            btnTransferSubmit.Click += BtnTransferSubmit_Click;
            
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
            
            this.Controls.Add(pnlTransferMoney);
        }

        private void CheckRecipientName(string accountNumber, Label recipientLabel)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                recipientLabel.Text = "👤 Alıcı: Hesap numarası girin";
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
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["Name"]?.ToString() ?? "";
                                string surname = reader["Surname"]?.ToString() ?? "";
                                recipientLabel.Text = $"👤 Alıcı: {name} {surname}";
                                recipientLabel.ForeColor = System.Drawing.Color.FromArgb(0, 128, 0);
                            }
                            else
                            {
                                recipientLabel.Text = "❌ Hesap bulunamadı";
                                recipientLabel.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
                            }
                        }
                    }
                }
            }
            catch
            {
                recipientLabel.Text = "👤 Alıcı: Hesap kontrol edilemiyor";
                recipientLabel.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            }
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
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["Name"]?.ToString() ?? "";
                                string surname = reader["Surname"]?.ToString() ?? "";
                                return $"👤 Alıcı: {name} {surname}";
                            }
                        }
                    }
                }
            }
            catch { }
            
            return "👤 Alıcı bilgisi alınamadı";
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

        private void BtnTransferSubmit_Click(object sender, EventArgs e)
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