using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace SmartBankingAutomation.Forms
{
    public partial class LoginForm : Form
    {
        private string dbPath = "Data/SmartBank.db";
        public LoginForm()
        {
            InitializeComponent();
            CreateDatabaseAndTables();
        }

        private void CreateDatabaseAndTables()
        {
            try
            {
                // Veritabanı dosyası yoksa oluştur
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
                            AccountType TEXT DEFAULT 'Vadesiz',
                            CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
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
                            PreviousBuyRate DECIMAL(10,4) DEFAULT 0.00,
                            PreviousSellRate DECIMAL(10,4) DEFAULT 0.00,
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
                            TransactionType TEXT NOT NULL,
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

                    // Hesap bazlı döviz portföyü tablosu
                    string createAccountCurrenciesTable = @"
                        CREATE TABLE IF NOT EXISTS AccountCurrencies (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            AccountId INTEGER NOT NULL,
                            CurrencyCode TEXT NOT NULL,
                            Amount DECIMAL(15,6) NOT NULL,
                            BaseRate DECIMAL(10,4) NOT NULL,
                            TransactionDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                            FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
                        );";

                    SQLiteCommand cmd9 = new SQLiteCommand(createAccountCurrenciesTable, conn);
                    cmd9.ExecuteNonQuery();

                    // Temel döviz kurlarını ekle
                    InitializeCurrencyRates(conn);
                    
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda sessizce devam et
                Console.WriteLine($"Veritabanı oluşturma hatası: {ex.Message}");
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

                // Güncel döviz kurlarını ekle
                string[] currencies = {
                    "INSERT INTO CurrencyRates (CurrencyCode, CurrencyName, BuyRate, SellRate) VALUES ('USD', 'Amerikan Doları', 32.45, 32.75)",
                    "INSERT INTO CurrencyRates (CurrencyCode, CurrencyName, BuyRate, SellRate) VALUES ('EUR', 'Euro', 35.15, 35.55)", 
                    "INSERT INTO CurrencyRates (CurrencyCode, CurrencyName, BuyRate, SellRate) VALUES ('GBP', 'İngiliz Sterlini', 41.20, 41.60)",
                    "INSERT INTO CurrencyRates (CurrencyCode, CurrencyName, BuyRate, SellRate) VALUES ('CHF', 'İsviçre Frangı', 36.75, 37.15)",
                    "INSERT INTO CurrencyRates (CurrencyCode, CurrencyName, BuyRate, SellRate) VALUES ('SAR', 'Suudi Arabistan Riyali', 8.65, 8.85)",
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtSurname.Text) || 
                string.IsNullOrWhiteSpace(txtTcNo.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = "INSERT INTO Customers (Name, Surname, TcNo, Phone, Password) VALUES (@Name, @Surname, @TcNo, @Phone, @Password)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Surname", txtSurname.Text.Trim());
                    cmd.Parameters.AddWithValue("@TcNo", txtTcNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Kayıt başarılı! Artık giriş yapabilirsiniz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Formu temizle
                        txtName.Clear();
                        txtSurname.Clear();
                        txtTcNo.Clear();
                        txtPhone.Clear();
                        txtPassword.Clear();
                    }
                    catch (SQLiteException ex)
                    {
                        if (ex.Message.Contains("UNIQUE constraint failed"))
                            MessageBox.Show("Bu TC No zaten kayıtlı! Giriş panelinden giriş yapabilirsiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                            MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTcLogin.Text) || string.IsNullOrWhiteSpace(txtPasswordLogin.Text))
            {
                MessageBox.Show("TC No ve şifre alanlarını doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Önce kullanıcı var mı kontrol et
                    string checkQuery = "SELECT Name, Surname, TcNo, Phone FROM Customers WHERE TcNo = @TcNo AND Password = @Password";
                    using (SQLiteCommand cmd = new SQLiteCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", txtTcLogin.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPasswordLogin.Text);
                        
                        object result = cmd.ExecuteScalar();
                        
                        if (result != null)
                        {
                            // Kullanıcı bilgilerini tekrar çek
                            using (SQLiteCommand detailCmd = new SQLiteCommand(checkQuery, conn))
                            {
                                detailCmd.Parameters.AddWithValue("@TcNo", txtTcLogin.Text.Trim());
                                detailCmd.Parameters.AddWithValue("@Password", txtPasswordLogin.Text);
                                
                                using (SQLiteDataReader reader = detailCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        string name = reader["Name"]?.ToString() ?? "";
                                        string surname = reader["Surname"]?.ToString() ?? "";
                                        string tcno = reader["TcNo"]?.ToString() ?? "";
                                        string phone = reader["Phone"]?.ToString() ?? "";
                                        
                                        reader.Close();
                                        conn.Close();
                                        
                                        Form1 mainForm = new Form1(name, surname, tcno, phone);
                                        mainForm.Show();
                                        this.Hide();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    
                    // Kullanıcı bulunamadı, TC No kontrolü yap
                    string tcCheckQuery = "SELECT COUNT(*) FROM Customers WHERE TcNo = @TcNo";
                    using (SQLiteCommand tcCmd = new SQLiteCommand(tcCheckQuery, conn))
                    {
                        tcCmd.Parameters.AddWithValue("@TcNo", txtTcLogin.Text.Trim());
                        int count = Convert.ToInt32(tcCmd.ExecuteScalar());
                        
                        if (count == 0)
                        {
                            MessageBox.Show("Bu TC No ile kayıtlı kullanıcı bulunamadı!\nLütfen önce kayıt olun.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giriş sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewData_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string query = "SELECT Id, Name, Surname, TcNo, Phone, Password FROM Customers";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        string data = "VERİTABANI İÇERİĞİ:\n" + new string('=', 50) + "\n\n";
                        
                        if (!reader.HasRows)
                        {
                            data += "Henüz kayıtlı kullanıcı yok.";
                        }
                        else
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                count++;
                                data += $"KAYIT {count}:\n";
                                data += $"ID: {reader["Id"]}\n";
                                data += $"Ad: {reader["Name"]}\n";
                                data += $"Soyad: {reader["Surname"]}\n";
                                data += $"TC No: {reader["TcNo"]}\n";
                                data += $"Telefon: {reader["Phone"]}\n";
                                data += $"Şifre: {reader["Password"]}\n";
                                data += new string('-', 30) + "\n\n";
                            }
                        }
                        
                        MessageBox.Show(data, "Veritabanı İçeriği", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}