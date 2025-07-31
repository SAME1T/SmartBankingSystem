using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SmartBankingAutomation.Forms
{
    public partial class CreditCardForm : Form
    {
        private string dbPath = "Data/SmartBank.db";
        private string currentTcNo = "";

        // UI Controls
        private Panel pnlCreditCard;
        private Label lblCreditCardTitle;
        private Button btnApplyCreditCard;
        private Button btnCreditCardPayment;
        private Button btnCreditCardTransactions;
        private Button btnCreditCardHistory;
        private DataGridView dgvCreditCards;

        public CreditCardForm(string tcNo)
        {
            currentTcNo = tcNo;
            InitializeComponent();
            CreateCreditCardPanel();
            FixCreditCardLimits();
            LoadCreditCards();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "💳 Kredi Kartı İşlemleri";
            this.Size = new System.Drawing.Size(800, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.White;
            
            this.ResumeLayout(false);
        }

        private void CreateCreditCardPanel()
        {
            pnlCreditCard = new Panel();
            pnlCreditCard.Size = new System.Drawing.Size(750, 600);
            pnlCreditCard.Location = new System.Drawing.Point(25, 25);
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
            btnApplyCreditCard.Size = new System.Drawing.Size(160, 45);
            btnApplyCreditCard.Click += BtnApplyCreditCard_Click;
            
            // Kredi kartı ödeme butonu
            btnCreditCardPayment = new Button();
            btnCreditCardPayment.Text = "💰 Kredi Kartı Ödeme";
            btnCreditCardPayment.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnCreditCardPayment.ForeColor = System.Drawing.Color.White;
            btnCreditCardPayment.FlatStyle = FlatStyle.Flat;
            btnCreditCardPayment.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnCreditCardPayment.Location = new System.Drawing.Point(190, 70);
            btnCreditCardPayment.Size = new System.Drawing.Size(160, 45);
            btnCreditCardPayment.Click += BtnCreditCardPayment_Click;
            
            // Kredi kartı harcama butonu
            btnCreditCardTransactions = new Button();
            btnCreditCardTransactions.Text = "🛒 Harcama Yap";
            btnCreditCardTransactions.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            btnCreditCardTransactions.ForeColor = System.Drawing.Color.White;
            btnCreditCardTransactions.FlatStyle = FlatStyle.Flat;
            btnCreditCardTransactions.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnCreditCardTransactions.Location = new System.Drawing.Point(360, 70);
            btnCreditCardTransactions.Size = new System.Drawing.Size(160, 45);
            btnCreditCardTransactions.Click += BtnCreditCardTransactions_Click;

            // Kredi kartı işlem geçmişi butonu
            btnCreditCardHistory = new Button();
            btnCreditCardHistory.Text = "📊 İşlem Geçmişi";
            btnCreditCardHistory.BackColor = System.Drawing.Color.FromArgb(128, 0, 128);
            btnCreditCardHistory.ForeColor = System.Drawing.Color.White;
            btnCreditCardHistory.FlatStyle = FlatStyle.Flat;
            btnCreditCardHistory.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnCreditCardHistory.Location = new System.Drawing.Point(530, 70);
            btnCreditCardHistory.Size = new System.Drawing.Size(160, 45);
            btnCreditCardHistory.Click += BtnCreditCardHistory_Click;
            
            // Kredi kartları listesi
            Label lblCreditCardsLabel = new Label();
            lblCreditCardsLabel.Text = "💳 Kredi Kartlarınız";
            lblCreditCardsLabel.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblCreditCardsLabel.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            lblCreditCardsLabel.Location = new System.Drawing.Point(20, 135);
            lblCreditCardsLabel.AutoSize = true;
            
            dgvCreditCards = new DataGridView();
            dgvCreditCards.Location = new System.Drawing.Point(20, 165);
            dgvCreditCards.Size = new System.Drawing.Size(700, 400);
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
            
            this.Controls.Add(pnlCreditCard);
        }

        private void FixCreditCardLimits()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Müşteri ID'sini bul
                    string findCustomerQuery = "SELECT Id FROM Customers WHERE TcNo = @TcNo";
                    using (SQLiteCommand cmd = new SQLiteCommand(findCustomerQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        object result = cmd.ExecuteScalar();
                        if (result == null) return;
                        
                        int customerId = Convert.ToInt32(result);
                        
                        // Kullanılabilir limitin doğru olduğundan emin ol
                        string fixLimitsQuery = @"UPDATE CreditCards 
                                                SET AvailableLimit = CreditLimit - Debt 
                                                WHERE CustomerId = @CustomerId";
                        
                        using (SQLiteCommand fixCmd = new SQLiteCommand(fixLimitsQuery, conn))
                        {
                            fixCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            fixCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch { }
        }

        private void LoadCreditCards()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    string query = @"SELECT cc.CardNumber as 'Kart Numarası', 
                                           cc.CardName as 'Kart Adı',
                                           cc.CreditLimit as 'Kredi Limiti',
                                           cc.AvailableLimit as 'Kullanılabilir Limit',
                                           cc.Debt as 'Borç',
                                           cc.ExpiryDate as 'Son Kullanma Tarihi',
                                           CASE WHEN cc.IsActive = 1 THEN 'Aktif' ELSE 'Pasif' END as 'Durum'
                                    FROM CreditCards cc 
                                    INNER JOIN Customers c ON cc.CustomerId = c.Id 
                                    WHERE c.TcNo = @TcNo";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvCreditCards.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kredi kartı bilgileri yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnApplyCreditCard_Click(object sender, EventArgs e)
        {
            // Kredi kartı başvuru işlemleri
            MessageBox.Show("Kredi kartı başvuru işlemleri için müşteri temsilcimizle iletişime geçiniz.\n\nTelefon: 444 0 123", 
                "Kredi Kartı Başvurusu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCreditCardPayment_Click(object sender, EventArgs e)
        {
            // Kredi kartı ödeme işlemleri
            MessageBox.Show("Kredi kartı ödeme işlemleri için internet bankacılığınızı kullanabilirsiniz.", 
                "Kredi Kartı Ödeme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCreditCardTransactions_Click(object sender, EventArgs e)
        {
            // Kredi kartı harcama işlemleri
            MessageBox.Show("Kredi kartı harcama işlemleri mağazalarımızda veya online platformlarda gerçekleştirilebilir.", 
                "Kredi Kartı Harcama", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCreditCardHistory_Click(object sender, EventArgs e)
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
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgv.DataSource = dt;

                            // Toplam harcamayı hesapla
                            decimal totalSpent = 0;
                            foreach (DataRow row in dt.Rows)
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
    }
}