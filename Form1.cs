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