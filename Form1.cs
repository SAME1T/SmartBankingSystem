using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SmartBankingAutomation.Forms;

namespace SmartBankingAutomation
{
    public partial class Form1 : Form
    {
        private string currentName = "";
        private string currentSurname = "";
        private string currentTcNo = "";
        private string currentPhone = "";
        private string dbPath = "Data/SmartBank.db";

        // Corporate Theme Colors
        private static readonly Color PrimaryBlue = Color.FromArgb(25, 42, 86);      // Koyu lacivert
        private static readonly Color SecondaryBlue = Color.FromArgb(52, 73, 126);   // Açık lacivert
        private static readonly Color AccentGold = Color.FromArgb(255, 193, 7);      // Altın/Sarı
        private static readonly Color BackgroundWhite = Color.FromArgb(248, 249, 250); // Açık gri-beyaz
        private static readonly Color TextDark = Color.FromArgb(33, 37, 41);         // Koyu metin

        public Form1()
        {
            InitializeComponent();
            SetUserInfo("Misafir", "Kullanıcı", "00000000000", "000-000-0000");
            LoadDynamicMenu();
            SetupDashboardTheme();
            LoadDashboard();
        }

        public Form1(string name, string surname, string tcno, string phone)
        {
            InitializeComponent();
            currentName = name;
            currentSurname = surname;
            currentTcNo = tcno;
            currentPhone = phone;
            SetUserInfo(name, surname, tcno, phone);
            LoadDynamicMenu();
            SetupDashboardTheme();
            LoadDashboard();
        }

        private void SetUserInfo(string name, string surname, string tcno, string phone)
        {
            lblUserName.Text = $"👤 {name} {surname}";
            lblUserTc.Text = $"🆔 TC: {tcno}";
            
            // Avatar için kullanıcının baş harfini göster
            picUserAvatar.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(System.Drawing.Brushes.LightBlue, 5, 5, 70, 70);
                var font = new System.Drawing.Font("Segoe UI", 24, System.Drawing.FontStyle.Bold);
                var brush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                string initial = string.IsNullOrEmpty(name) ? "?" : name.Substring(0, 1).ToUpper();
                var size = g.MeasureString(initial, font);
                g.DrawString(initial, font, brush, (80 - size.Width) / 2, (80 - size.Height) / 2);
            };
        }

        private void LoadDynamicMenu()
        {
            try
            {
                // Forms klasöründeki tüm Form sınıflarını bul
                var formTypes = GetFormTypesFromAssembly();
                
                // Her Form için buton oluştur
                foreach (var formType in formTypes)
                {
                    CreateMenuButton(formType);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Menü yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Type> GetFormTypesFromAssembly()
        {
            var formTypes = new List<Type>();
            
            try
            {
                // Mevcut assembly'deki tüm türleri al
                var assembly = Assembly.GetExecutingAssembly();
                var types = assembly.GetTypes();
                
                foreach (var type in types)
                {
                    // Form sınıfı mı ve SmartBankingAutomation.Forms namespace'inde mi?
                    if (type.IsSubclassOf(typeof(Form)) && 
                        type.Namespace == "SmartBankingAutomation.Forms" &&
                        type.Name != "LoginForm") // LoginForm hariç
                    {
                        formTypes.Add(type);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form türleri yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return formTypes;
        }

        private void CreateMenuButton(Type formType)
        {
            try
            {
                var button = new Button();
                
                // Buton özelliklerini ayarla
                button.Size = new Size(200, 50);
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                button.ForeColor = Color.White;
                button.UseVisualStyleBackColor = false;
                button.Margin = new Padding(3);
                
                // Form türüne göre buton metnini ve rengini ayarla
                SetButtonAppearance(button, formType);
                
                // Form türünü butonun Tag'ine ata
                button.Tag = formType;
                
                // Click eventi ekle
                button.Click += DynamicButton_Click;
                
                // FlowLayoutPanel'a ekle (logout butonundan önce)
                flowMenu.Controls.Add(button);
                flowMenu.Controls.SetChildIndex(button, flowMenu.Controls.Count - 2); // Logout'tan önce
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Buton oluşturulurken hata ({formType.Name}): {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetButtonAppearance(Button button, Type formType)
        {
            // DisplayName attribute'unu kontrol et
            var displayNameAttr = formType.GetCustomAttribute<DisplayNameAttribute>();
            string displayName = displayNameAttr?.DisplayName ?? GetFriendlyName(formType.Name);
            
            // Form türüne göre buton görünümünü ayarla (Kurumsal tema renkleri)
            switch (formType.Name)
            {
                case "AccountOpenForm":
                    button.Text = "💳 Hesap Aç";
                    button.BackColor = SecondaryBlue;
                    break;
                case "DepositForm":
                    button.Text = "💰 Para Yatır";
                    button.BackColor = Color.FromArgb(40, 167, 69);
                    break;
                case "WithdrawalForm":
                    button.Text = "💸 Para Çek";
                    button.BackColor = Color.FromArgb(220, 53, 69);
                    break;
                case "TransferForm":
                    button.Text = "🔄 Para Transfer";
                    button.BackColor = Color.FromArgb(255, 140, 0);
                    break;
                case "AccountInfoForm":
                    button.Text = "📊 Hesap Bilgileri";
                    button.BackColor = PrimaryBlue;
                    break;
                case "CreditCardForm":
                    button.Text = "💳 Kredi Kartı";
                    button.BackColor = Color.FromArgb(220, 53, 69);
                    break;
                case "CurrencyConverterForm":
                    button.Text = "💱 Döviz Çevir";
                    button.BackColor = Color.FromArgb(23, 162, 184);
                    break;
                default:
                    button.Text = $"📋 {displayName}";
                    button.BackColor = Color.FromArgb(108, 117, 125);
                    break;
            }
        }

        private string GetFriendlyName(string className)
        {
            // "Form" kelimesini kaldır ve camelCase'i boşluklarla ayır
            string name = className.Replace("Form", "");
            
            // CamelCase'i boşluklarla ayır
            var result = "";
            for (int i = 0; i < name.Length; i++)
            {
                if (i > 0 && char.IsUpper(name[i]))
                {
                    result += " ";
                }
                result += name[i];
            }
            
            return result;
        }

        private void DynamicButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is Type formType)
            {
                try
                {
                    // Form'u dinamik olarak oluştur
                    Form formInstance;
                    
                    // Constructor'da parametre olarak currentTcNo gereken formlar
                    if (RequiresTcNoParameter(formType))
                    {
                        if (formType.Name == "AccountOpenForm")
                        {
                            // AccountOpenForm özel constructor gerektirir
                            formInstance = (Form)Activator.CreateInstance(formType, currentTcNo, currentName, currentSurname);
                        }
                        else
                        {
                            formInstance = (Form)Activator.CreateInstance(formType, currentTcNo);
                        }
                    }
                    else
                    {
                        formInstance = (Form)Activator.CreateInstance(formType);
                    }
                    
                    // Form'u modal olarak aç
                    formInstance.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Form açılırken hata ({formType.Name}): {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool RequiresTcNoParameter(Type formType)
        {
            // TC No parametresi gereken formları kontrol et
            var constructors = formType.GetConstructors();
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length > 0 && parameters[0].ParameterType == typeof(string))
                {
                    return true;
                }
            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUserInfoFromDatabase();
        }

        private void LoadUserInfoFromDatabase()
        {
            if (string.IsNullOrEmpty(currentTcNo) || currentTcNo == "00000000000")
                return;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string query = "SELECT Name, Surname, Phone FROM Customers WHERE TcNo = @TcNo";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbName = reader["Name"]?.ToString() ?? currentName;
                                string dbSurname = reader["Surname"]?.ToString() ?? currentSurname;
                                string dbPhone = reader["Phone"]?.ToString() ?? currentPhone;
                                
                                // Bilgileri güncelle
                                currentName = dbName;
                                currentSurname = dbSurname;
                                currentPhone = dbPhone;
                                
                                SetUserInfo(dbName, dbSurname, currentTcNo, dbPhone);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Veritabanı hatası olursa sessizce devam et
                Console.WriteLine($"Kullanıcı bilgileri yüklenirken hata: {ex.Message}");
            }
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

        private void SetupDashboardTheme()
        {
            // DataGridView'lere tema uygula
            SetDataGridViewTheme(dgvAccountBalances);
            SetDataGridViewTheme(dgvMarketRates);
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

        private void LoadDashboard()
        {
            LoadAccountBalances();
            LoadMarketRates();
            UpdateSummaryCards();
        }

        private void LoadAccountBalances()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    string query = @"SELECT 
                                       a.AccountNumber as 'Hesap No',
                                       a.AccountType as 'Hesap Türü',
                                       a.Balance as 'TL Bakiye',
                                       COALESCE(
                                           (SELECT SUM(ac.Amount * ((cr.BuyRate + cr.SellRate) / 2))
                                            FROM AccountCurrencies ac 
                                            INNER JOIN CurrencyRates cr ON ac.CurrencyCode = cr.CurrencyCode
                                            WHERE ac.AccountId = a.Id), 0) as 'Döviz TL Karşılığı',
                                       (a.Balance + COALESCE(
                                           (SELECT SUM(ac.Amount * ((cr.BuyRate + cr.SellRate) / 2))
                                            FROM AccountCurrencies ac 
                                            INNER JOIN CurrencyRates cr ON ac.CurrencyCode = cr.CurrencyCode
                                            WHERE ac.AccountId = a.Id), 0)) as 'Toplam TL'
                                     FROM Accounts a 
                                     INNER JOIN Customers c ON a.CustomerId = c.Id 
                                     WHERE c.TcNo = @TcNo";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        
                        DataTable dt = new DataTable();
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        
                        dgvAccountBalances.DataSource = dt;
                        
                        if (dgvAccountBalances.Columns.Count > 0)
                        {
                            dgvAccountBalances.Columns["TL Bakiye"].DefaultCellStyle.Format = "₺#,0.00";
                            dgvAccountBalances.Columns["Döviz TL Karşılığı"].DefaultCellStyle.Format = "₺#,0.00";
                            dgvAccountBalances.Columns["Toplam TL"].DefaultCellStyle.Format = "₺#,0.00";
                            dgvAccountBalances.Columns["Toplam TL"].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                            dgvAccountBalances.Columns["Toplam TL"].DefaultCellStyle.ForeColor = SecondaryBlue;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Demo data göster
                var dt = new DataTable();
                dt.Columns.Add("Hesap No");
                dt.Columns.Add("Hesap Türü");
                dt.Columns.Add("TL Bakiye", typeof(decimal));
                dt.Columns.Add("Döviz TL Karşılığı", typeof(decimal));
                dt.Columns.Add("Toplam TL", typeof(decimal));
                
                dt.Rows.Add("1001-0001", "Vadesiz", 15000m, 0m, 15000m);
                dt.Rows.Add("1001-0002", "Vadeli", 50000m, 0m, 50000m);
                
                dgvAccountBalances.DataSource = dt;
            }
        }

        private void LoadMarketRates()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    string query = @"SELECT 
                                       CurrencyCode as 'Döviz',
                                       CurrencyName as 'Döviz Adı',
                                       BuyRate as 'Alış',
                                       SellRate as 'Satış',
                                       CASE 
                                           WHEN PreviousBuyRate > 0 THEN 
                                               ROUND(((BuyRate - PreviousBuyRate) / PreviousBuyRate) * 100, 2)
                                           ELSE 0 
                                       END as 'Değişim (%)'
                                     FROM CurrencyRates 
                                     WHERE CurrencyCode IN ('USD', 'EUR', 'GBP', 'CHF', 'SAR', 'GOLD')
                                     ORDER BY CurrencyCode";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        DataTable dt = new DataTable();
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        
                        dgvMarketRates.DataSource = dt;
                        
                        if (dgvMarketRates.Columns.Count > 0)
                        {
                            dgvMarketRates.Columns["Alış"].DefaultCellStyle.Format = "₺#,0.0000";
                            dgvMarketRates.Columns["Satış"].DefaultCellStyle.Format = "₺#,0.0000";
                            
                            // Değişim yüzdesine göre renklendirme
                            foreach (DataGridViewRow row in dgvMarketRates.Rows)
                            {
                                if (row.Cells["Değişim (%)"].Value != null)
                                {
                                    var changeValue = Convert.ToDecimal(row.Cells["Değişim (%)"].Value);
                                    if (changeValue > 0)
                                    {
                                        row.Cells["Değişim (%)"].Style.ForeColor = Color.Green;
                                        row.Cells["Değişim (%)"].Value = $"+{changeValue:F2}%";
                                    }
                                    else if (changeValue < 0)
                                    {
                                        row.Cells["Değişim (%)"].Style.ForeColor = Color.Red;
                                        row.Cells["Değişim (%)"].Value = $"{changeValue:F2}%";
                                    }
                                    else
                                    {
                                        row.Cells["Değişim (%)"].Value = "0.00%";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Demo data göster
                var dt = new DataTable();
                dt.Columns.Add("Döviz");
                dt.Columns.Add("Döviz Adı");
                dt.Columns.Add("Alış", typeof(decimal));
                dt.Columns.Add("Satış", typeof(decimal));
                dt.Columns.Add("Değişim (%)");
                
                dt.Rows.Add("USD", "Amerikan Doları", 32.45m, 32.75m, "+0.15%");
                dt.Rows.Add("EUR", "Euro", 35.15m, 35.55m, "-0.25%");
                dt.Rows.Add("GBP", "İngiliz Sterlini", 41.20m, 41.60m, "+0.35%");
                
                dgvMarketRates.DataSource = dt;
            }
        }

        private void UpdateSummaryCards()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Toplam TL bakiye
                    string tlQuery = @"SELECT COALESCE(SUM(a.Balance), 0) as TotalTL
                                     FROM Accounts a 
                                     INNER JOIN Customers c ON a.CustomerId = c.Id 
                                     WHERE c.TcNo = @TcNo";
                    
                    decimal totalTL = 0;
                    using (SQLiteCommand cmd = new SQLiteCommand(tlQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            totalTL = Convert.ToDecimal(result);
                    }
                    
                    // USD değeri
                    string usdQuery = @"SELECT COALESCE(SUM(ac.Amount * ((cr.BuyRate + cr.SellRate) / 2)), 0) as UsdValue
                                      FROM AccountCurrencies ac
                                      INNER JOIN Accounts a ON ac.AccountId = a.Id
                                      INNER JOIN Customers c ON a.CustomerId = c.Id
                                      INNER JOIN CurrencyRates cr ON ac.CurrencyCode = cr.CurrencyCode
                                      WHERE c.TcNo = @TcNo AND ac.CurrencyCode = 'USD'";
                    
                    decimal usdValue = 0;
                    using (SQLiteCommand cmd = new SQLiteCommand(usdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            usdValue = Convert.ToDecimal(result);
                    }
                    
                    // EUR değeri
                    string eurQuery = @"SELECT COALESCE(SUM(ac.Amount * ((cr.BuyRate + cr.SellRate) / 2)), 0) as EurValue
                                      FROM AccountCurrencies ac
                                      INNER JOIN Accounts a ON ac.AccountId = a.Id
                                      INNER JOIN Customers c ON a.CustomerId = c.Id
                                      INNER JOIN CurrencyRates cr ON ac.CurrencyCode = cr.CurrencyCode
                                      WHERE c.TcNo = @TcNo AND ac.CurrencyCode = 'EUR'";
                    
                    decimal eurValue = 0;
                    using (SQLiteCommand cmd = new SQLiteCommand(eurQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            eurValue = Convert.ToDecimal(result);
                    }
                    
                    // Toplam döviz değeri
                    string totalCurrencyQuery = @"SELECT COALESCE(SUM(ac.Amount * ((cr.BuyRate + cr.SellRate) / 2)), 0) as TotalCurrency
                                                FROM AccountCurrencies ac
                                                INNER JOIN Accounts a ON ac.AccountId = a.Id
                                                INNER JOIN Customers c ON a.CustomerId = c.Id
                                                INNER JOIN CurrencyRates cr ON ac.CurrencyCode = cr.CurrencyCode
                                                WHERE c.TcNo = @TcNo";
                    
                    decimal totalCurrency = 0;
                    using (SQLiteCommand cmd = new SQLiteCommand(totalCurrencyQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            totalCurrency = Convert.ToDecimal(result);
                    }
                    
                    // Label'ları güncelle
                    decimal totalAssets = totalTL + totalCurrency;
                    lblTotalAssets.Text = $"💰 Toplam Varlık: {totalAssets:₺#,0.00}";
                    lblUsdValue.Text = $"🇺🇸 USD: {usdValue:₺#,0.00}";
                    lblEurValue.Text = $"🇪🇺 EUR: {eurValue:₺#,0.00}";
                }
            }
            catch (Exception)
            {
                lblTotalAssets.Text = "💰 Toplam Varlık: ₺65,000.00";
                lblUsdValue.Text = "🇺🇸 USD: ₺0.00";
                lblEurValue.Text = "🇪🇺 EUR: ₺0.00";
            }
        }

        private void btnRefreshDashboard_Click(object sender, EventArgs e)
        {
            LoadDashboard();
            MessageBox.Show("Dashboard verileri yenilendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Form Load event'ini ayarla
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Form1_Load(this, e);
        }
    }
}