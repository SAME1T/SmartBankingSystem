using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace SmartBankingAutomation.Forms
{
    public partial class AccountInfoForm : Form
    {
        private string dbPath = "Data/SmartBank.db";
        private string currentTcNo = "";

        // Corporate Theme Colors
        private static readonly Color PrimaryBlue = Color.FromArgb(25, 42, 86);      // Koyu lacivert
        private static readonly Color SecondaryBlue = Color.FromArgb(52, 73, 126);   // Açık lacivert
        private static readonly Color AccentGold = Color.FromArgb(255, 193, 7);      // Altın/Sarı
        private static readonly Color BackgroundWhite = Color.FromArgb(248, 249, 250); // Açık gri-beyaz
        private static readonly Color TextDark = Color.FromArgb(33, 37, 41);         // Koyu metin

        // UI Controls
        private Panel pnlHeader;
        private PictureBox picLogo;
        private Label lblCompanyTitle;
        private Panel pnlAccountInfo;
        private Label lblAccountInfoTitle;
        private Label lblAccountsLabel;
        private DataGridView dgvAccounts;
        private Button btnCopyAccountInfo;
        private Label lblCurrencyPortfolioLabel;
        private DataGridView dgvCurrencyPortfolio;
        private Label lblTransactionsLabel;
        private DataGridView dgvTransactions;
        private Button btnRefreshAll;

        public AccountInfoForm(string tcNo)
        {
            currentTcNo = tcNo;
            InitializeComponent();
            CreateHeaderPanel();
            CreateAccountInfoPanel();
            LoadAccountInfo();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "📊 Hesap Bilgileri - Türkiye Dijital Bank";
            this.Size = new System.Drawing.Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = BackgroundWhite;
            this.Font = new Font("Segoe UI", 9F);
            
            this.ResumeLayout(false);
        }

        private void CreateHeaderPanel()
        {
            pnlHeader = new Panel();
            pnlHeader.Size = new Size(1170, 80);
            pnlHeader.Location = new Point(10, 10);
            pnlHeader.BackColor = PrimaryBlue;
            
            picLogo = new PictureBox();
            picLogo.Size = new Size(60, 60);
            picLogo.Location = new Point(15, 10);
            picLogo.BackColor = Color.White;
            picLogo.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(new SolidBrush(AccentGold), 5, 5, 50, 50);
                var font = new Font("Segoe UI", 18, FontStyle.Bold);
                var brush = new SolidBrush(PrimaryBlue);
                g.DrawString("₺", font, brush, 18, 15);
            };
            
            lblCompanyTitle = new Label();
            lblCompanyTitle.Text = "🏛️ TÜRKİYE DİJİTAL BANK";
            lblCompanyTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblCompanyTitle.ForeColor = Color.White;
            lblCompanyTitle.Location = new Point(90, 15);
            lblCompanyTitle.AutoSize = true;
            
            var lblSubtitle = new Label();
            lblSubtitle.Text = "Modern Bankacılık • Güvenli • Hızlı • Kolay";
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = AccentGold;
            lblSubtitle.Location = new Point(90, 45);
            lblSubtitle.AutoSize = true;
            
            pnlHeader.Controls.Add(picLogo);
            pnlHeader.Controls.Add(lblCompanyTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            
            this.Controls.Add(pnlHeader);
        }

        private void CreateAccountInfoPanel()
        {
            pnlAccountInfo = new Panel();
            pnlAccountInfo.Size = new Size(1170, 680);
            pnlAccountInfo.Location = new Point(10, 100);
            pnlAccountInfo.BackColor = Color.White;
            pnlAccountInfo.BorderStyle = BorderStyle.FixedSingle;
            
            lblAccountInfoTitle = new Label();
            lblAccountInfoTitle.Text = "📊 Hesap Bilgileri ve Portföy Durumu";
            lblAccountInfoTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblAccountInfoTitle.ForeColor = PrimaryBlue;
            lblAccountInfoTitle.Location = new Point(20, 15);
            lblAccountInfoTitle.AutoSize = true;
            
            // Sol taraf - Hesaplar
            lblAccountsLabel = new Label();
            lblAccountsLabel.Text = "💳 TL Hesaplarınız";
            lblAccountsLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblAccountsLabel.ForeColor = SecondaryBlue;
            lblAccountsLabel.Location = new Point(20, 55);
            lblAccountsLabel.AutoSize = true;
            
            dgvAccounts = new DataGridView();
            dgvAccounts.Location = new Point(20, 85);
            dgvAccounts.Size = new Size(560, 180);
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccounts.ReadOnly = true;
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.MultiSelect = false;
            dgvAccounts.BackgroundColor = BackgroundWhite;
            dgvAccounts.BorderStyle = BorderStyle.Fixed3D;
            SetDataGridViewTheme(dgvAccounts);
            
            // Sağ taraf - Döviz Portföyü
            lblCurrencyPortfolioLabel = new Label();
            lblCurrencyPortfolioLabel.Text = "💱 Döviz Portföyünüz";
            lblCurrencyPortfolioLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblCurrencyPortfolioLabel.ForeColor = SecondaryBlue;
            lblCurrencyPortfolioLabel.Location = new Point(600, 55);
            lblCurrencyPortfolioLabel.AutoSize = true;
            
            dgvCurrencyPortfolio = new DataGridView();
            dgvCurrencyPortfolio.Location = new Point(600, 85);
            dgvCurrencyPortfolio.Size = new Size(550, 180);
            dgvCurrencyPortfolio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCurrencyPortfolio.ReadOnly = true;
            dgvCurrencyPortfolio.AllowUserToAddRows = false;
            dgvCurrencyPortfolio.BackgroundColor = BackgroundWhite;
            dgvCurrencyPortfolio.BorderStyle = BorderStyle.Fixed3D;
            SetDataGridViewTheme(dgvCurrencyPortfolio);
            
            // Butonlar
            btnCopyAccountInfo = new Button();
            btnCopyAccountInfo.Text = "📋 Hesap No\nKopyala";
            btnCopyAccountInfo.BackColor = SecondaryBlue;
            btnCopyAccountInfo.ForeColor = Color.White;
            btnCopyAccountInfo.FlatStyle = FlatStyle.Flat;
            btnCopyAccountInfo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCopyAccountInfo.Location = new Point(20, 280);
            btnCopyAccountInfo.Size = new Size(120, 50);
            btnCopyAccountInfo.Click += BtnCopyAccountInfo_Click;
            
            btnRefreshAll = new Button();
            btnRefreshAll.Text = "🔄 Tümünü\nYenile";
            btnRefreshAll.BackColor = AccentGold;
            btnRefreshAll.ForeColor = PrimaryBlue;
            btnRefreshAll.FlatStyle = FlatStyle.Flat;
            btnRefreshAll.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRefreshAll.Location = new Point(160, 280);
            btnRefreshAll.Size = new Size(120, 50);
            btnRefreshAll.Click += BtnRefreshAll_Click;
            
            // Alt taraf - İşlem Geçmişi
            lblTransactionsLabel = new Label();
            lblTransactionsLabel.Text = "📋 Son İşlemleriniz";
            lblTransactionsLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTransactionsLabel.ForeColor = SecondaryBlue;
            lblTransactionsLabel.Location = new Point(20, 350);
            lblTransactionsLabel.AutoSize = true;
            
            dgvTransactions = new DataGridView();
            dgvTransactions.Location = new Point(20, 380);
            dgvTransactions.Size = new Size(1130, 280);
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.BackgroundColor = BackgroundWhite;
            dgvTransactions.BorderStyle = BorderStyle.Fixed3D;
            SetDataGridViewTheme(dgvTransactions);
            
            pnlAccountInfo.Controls.Add(lblAccountInfoTitle);
            pnlAccountInfo.Controls.Add(lblAccountsLabel);
            pnlAccountInfo.Controls.Add(dgvAccounts);
            pnlAccountInfo.Controls.Add(lblCurrencyPortfolioLabel);
            pnlAccountInfo.Controls.Add(dgvCurrencyPortfolio);
            pnlAccountInfo.Controls.Add(btnCopyAccountInfo);
            pnlAccountInfo.Controls.Add(btnRefreshAll);
            pnlAccountInfo.Controls.Add(lblTransactionsLabel);
            pnlAccountInfo.Controls.Add(dgvTransactions);
            
            this.Controls.Add(pnlAccountInfo);
        }

        private void SetDataGridViewTheme(DataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = SecondaryBlue;
            
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = TextDark;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            dgv.DefaultCellStyle.SelectionForeColor = TextDark;
            
            dgv.AlternatingRowsDefaultCellStyle.BackColor = BackgroundWhite;
            dgv.GridColor = Color.FromArgb(200, 200, 200);
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
                            DataTable dt = new DataTable();
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
                            DataTable dt = new DataTable();
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

        private void BtnCopyAccountInfo_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.SelectedRows.Count > 0)
            {
                // Seçili satırdan hesap numarasını al
                string accountNumber = dgvAccounts.SelectedRows[0].Cells["Hesap Numarası"].Value?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(accountNumber))
                {
                    Clipboard.SetText(accountNumber);
                    MessageBox.Show($"Hesap numarası panoya kopyalandı!\n\n{accountNumber}", "Kopyalandı", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Geçerli bir hesap numarası bulunamadı!", "Uyarı", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen kopyalamak istediğiniz hesabı seçin!", "Uyarı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefreshAll_Click(object sender, EventArgs e)
        {
            try
            {
                LoadAccountInfo();
                MessageBox.Show("Tüm bilgiler yenilendi!", "Başarılı", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bilgiler yenilenirken hata oluştu: {ex.Message}", "Hata", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}