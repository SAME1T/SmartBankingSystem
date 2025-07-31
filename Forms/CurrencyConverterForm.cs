using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SmartBankingAutomation.Forms
{
    public partial class CurrencyConverterForm : Form
    {
        private string dbPath = "Data/SmartBank.db";
        private string currentTcNo = "";
        private readonly HttpClient httpClient = new HttpClient();
        private int currentAccountId = 0;

        // Corporate Theme Colors
        private static readonly Color PrimaryBlue = Color.FromArgb(25, 42, 86);
        private static readonly Color SecondaryBlue = Color.FromArgb(52, 73, 126);
        private static readonly Color AccentGold = Color.FromArgb(255, 193, 7);
        private static readonly Color BackgroundWhite = Color.FromArgb(248, 249, 250);

        // API response model
        public class ExchangeRateResponse
        {
            public string? Base { get; set; }
            public Dictionary<string, decimal>? Rates { get; set; }
        }

        public CurrencyConverterForm()
        {
            InitializeComponent();
            SetupDataGridView();
            LoadDummyAccounts();
        }

        public CurrencyConverterForm(string tcNo) : this()
        {
            currentTcNo = tcNo;
            LoadAccounts();
            LoadPortfolioData();
        }

        private void SetupDataGridView()
        {
            dgvRates.Columns.Clear();
            dgvRates.Columns.Add("Currency", "Para Birimi");
            dgvRates.Columns.Add("Amount", "Miktar (Döviz)");
            dgvRates.Columns.Add("BaseRate", "İşlem Anı Kuru");
            dgvRates.Columns.Add("CurrentRate", "Güncel Kur");
            dgvRates.Columns.Add("CurrentValue", "Güncel TL Karşılığı");
            dgvRates.Columns.Add("Change", "Değişim (%)");

            dgvRates.Columns["Currency"].Width = 130;
            dgvRates.Columns["Amount"].Width = 100;
            dgvRates.Columns["BaseRate"].Width = 90;
            dgvRates.Columns["CurrentRate"].Width = 90;
            dgvRates.Columns["CurrentValue"].Width = 120;
            dgvRates.Columns["Change"].Width = 100;

            // Kurumsal tema uygula
            dgvRates.ColumnHeadersDefaultCellStyle.BackColor = PrimaryBlue;
            dgvRates.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRates.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvRates.ColumnHeadersDefaultCellStyle.SelectionBackColor = SecondaryBlue;
            
            dgvRates.DefaultCellStyle.BackColor = Color.White;
            dgvRates.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvRates.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvRates.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            dgvRates.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            
            dgvRates.AlternatingRowsDefaultCellStyle.BackColor = BackgroundWhite;
            dgvRates.GridColor = Color.FromArgb(200, 200, 200);
        }

        private void LoadDummyAccounts()
        {
            if (string.IsNullOrEmpty(currentTcNo))
            {
                cmbAccounts.Items.Add("1|Demo Hesap - Vadesiz - 15,000.00 TL");
                cmbAccounts.Items.Add("2|Demo Hesap - Vadeli - 50,000.00 TL");
            }
        }

        private void LoadAccounts()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string query = @"SELECT a.Id, a.AccountNumber, a.AccountType, a.Balance 
                                   FROM Accounts a 
                                   INNER JOIN Customers c ON a.CustomerId = c.Id 
                                   WHERE c.TcNo = @TcNo";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TcNo", currentTcNo);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            cmbAccounts.Items.Clear();
                            while (reader.Read())
                            {
                                string accountInfo = $"{reader["Id"]}|{reader["AccountNumber"]} - {reader["AccountType"]} - {reader["Balance"]:F2} TL";
                                cmbAccounts.Items.Add(accountInfo);
                            }
                        }
                    }
                }

                // Eğer hesap yoksa demo hesap ekle
                if (cmbAccounts.Items.Count == 0)
                {
                    LoadDummyAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hesaplar yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadDummyAccounts();
            }
        }

        private void LoadPortfolioData()
        {
            dgvRates.Rows.Clear();
            
            if (currentAccountId <= 0) return;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    // Kullanıcının döviz portföyünü yükle
                    string query = @"SELECT ac.CurrencyCode, SUM(ac.Amount) as TotalAmount, 
                                           AVG(ac.BaseRate) as AvgBaseRate,
                                           cr.BuyRate, cr.SellRate, cr.CurrencyName
                                    FROM AccountCurrencies ac
                                    INNER JOIN CurrencyRates cr ON ac.CurrencyCode = cr.CurrencyCode
                                    WHERE ac.AccountId = @AccountId
                                    GROUP BY ac.CurrencyCode";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountId", currentAccountId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string currencyCode = reader["CurrencyCode"]?.ToString() ?? "";
                                string currencyName = reader["CurrencyName"]?.ToString() ?? "";
                                decimal totalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                decimal avgBaseRate = Convert.ToDecimal(reader["AvgBaseRate"]);
                                decimal buyRate = Convert.ToDecimal(reader["BuyRate"]);
                                decimal sellRate = Convert.ToDecimal(reader["SellRate"]);
                                
                                decimal currentRate = (buyRate + sellRate) / 2;
                                decimal currentValue = totalAmount * currentRate;
                                decimal changePercent = avgBaseRate != 0 ? ((currentRate - avgBaseRate) / avgBaseRate) * 100 : 0;

                                int rowIndex = dgvRates.Rows.Add(
                                    $"{currencyCode} - {currencyName}",
                                    totalAmount.ToString("F4"),
                                    avgBaseRate.ToString("F4"),
                                    currentRate.ToString("F4"),
                                    currentValue.ToString("F2") + " TL",
                                    (changePercent >= 0 ? "+" : "") + changePercent.ToString("F2") + "%"
                                );

                                // Renklendirme
                                if (changePercent > 0)
                                {
                                    dgvRates.Rows[rowIndex].Cells["Change"].Style.ForeColor = Color.Green;
                                    dgvRates.Rows[rowIndex].Cells["Change"].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                                }
                                else if (changePercent < 0)
                                {
                                    dgvRates.Rows[rowIndex].Cells["Change"].Style.ForeColor = Color.Red;
                                    dgvRates.Rows[rowIndex].Cells["Change"].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Portföy verileri yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            // Eğer portföy boşsa demo verileri göster
            if (dgvRates.Rows.Count == 0)
            {
                LoadDemoRates();
            }
        }

        private void LoadDemoRates()
        {
            dgvRates.Rows.Clear();
            
            // Demo portföy verileri
            dgvRates.Rows.Add("USD - Amerikan Doları", "0.0000", "32.35", "32.50", "0.00 TL", "0.00%");
            dgvRates.Rows.Add("EUR - Euro", "0.0000", "35.40", "35.25", "0.00 TL", "0.00%");
            dgvRates.Rows.Add("GBP - İngiliz Sterlini", "0.0000", "40.95", "41.15", "0.00 TL", "0.00%");
            dgvRates.Rows.Add("CHF - İsviçre Frangı", "0.0000", "36.75", "36.80", "0.00 TL", "0.00%");
            dgvRates.Rows.Add("SAR - Suudi Arabistan Riyali", "0.0000", "8.64", "8.67", "0.00 TL", "0.00%");
            dgvRates.Rows.Add("XAU - Altın", "0.0000", "2650.00", "2665.00", "0.00 TL", "0.00%");
        }

        private void cmbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            clbCurrencies.Enabled = cmbAccounts.SelectedIndex >= 0;
            btnRefresh.Enabled = cmbAccounts.SelectedIndex >= 0;
            btnConvert.Enabled = cmbAccounts.SelectedIndex >= 0;
            
            if (cmbAccounts.SelectedIndex >= 0)
            {
                string selectedAccount = cmbAccounts.SelectedItem?.ToString() ?? "";
                if (selectedAccount.Contains("|"))
                {
                    string accountIdStr = selectedAccount.Split('|')[0];
                    if (int.TryParse(accountIdStr, out int accountId))
                    {
                        currentAccountId = accountId;
                        LoadPortfolioData();
                    }
                }
            }
        }

        private void clbCurrencies_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Checkbox seçimi değişikliği için artık özel bir işlem yapmıyoruz
            // Çünkü portföy verilerini görüntülüyoruz
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (currentAccountId <= 0)
            {
                MessageBox.Show("Lütfen önce bir hesap seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nudAmount.Value <= 0)
            {
                MessageBox.Show("Lütfen geçerli bir miktar girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Seçili dövizleri kontrol et
            var selectedCurrencies = new List<string>();
            for (int i = 0; i < clbCurrencies.Items.Count; i++)
            {
                if (clbCurrencies.GetItemChecked(i))
                {
                    string currencyText = clbCurrencies.Items[i].ToString() ?? "";
                    string currencyCode = currencyText.Split(' ')[0];
                    selectedCurrencies.Add(currencyCode);
                }
            }

            if (selectedCurrencies.Count == 0)
            {
                MessageBox.Show("Lütfen en az bir döviz seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal totalTlAmount = nudAmount.Value;
                decimal amountPerCurrency = totalTlAmount / selectedCurrencies.Count;

                ProcessCurrencyPurchase(selectedCurrencies, amountPerCurrency);
                LoadPortfolioData(); // Portföyü yenile
                
                MessageBox.Show($"Toplam {totalTlAmount:F2} TL ile {selectedCurrencies.Count} farklı döviz satın alındı!", 
                              "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Döviz çevirimi sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessCurrencyPurchase(List<string> currencyList, decimal tlAmountPerCurrency)
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (string currencyCode in currencyList)
                        {
                            // Güncel kuru al
                            decimal buyRate = GetCurrentBuyRate(conn, currencyCode);
                            if (buyRate <= 0) continue;

                            // Döviz miktarını hesapla
                            decimal currencyAmount = tlAmountPerCurrency / buyRate;

                            // AccountCurrencies tablosuna ekle
                            string insertQuery = @"INSERT INTO AccountCurrencies 
                                                 (AccountId, CurrencyCode, Amount, BaseRate, TransactionDate)
                                                 VALUES (@AccountId, @CurrencyCode, @Amount, @BaseRate, @TransactionDate)";

                            using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@AccountId", currentAccountId);
                                cmd.Parameters.AddWithValue("@CurrencyCode", currencyCode);
                                cmd.Parameters.AddWithValue("@Amount", currencyAmount);
                                cmd.Parameters.AddWithValue("@BaseRate", buyRate);
                                cmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                                cmd.ExecuteNonQuery();
                            }

                            // Hesap bakiyesinden TL düş
                            string updateAccountQuery = @"UPDATE Accounts 
                                                        SET Balance = Balance - @Amount 
                                                        WHERE Id = @AccountId";

                            using (SQLiteCommand cmd = new SQLiteCommand(updateAccountQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Amount", tlAmountPerCurrency);
                                cmd.Parameters.AddWithValue("@AccountId", currentAccountId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private decimal GetCurrentBuyRate(SQLiteConnection conn, string currencyCode)
        {
            try
            {
                string query = "SELECT BuyRate FROM CurrencyRates WHERE CurrencyCode = @Code";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Code", currencyCode);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 0;
                }
            }
            catch
            {
                // Demo kurlar
                return currencyCode switch
                {
                    "USD" => 32.50m,
                    "EUR" => 35.25m,
                    "GBP" => 41.15m,
                    "CHF" => 36.80m,
                    "BHD" => 86.25m,
                    "SAR" => 8.67m,
                    "XAU" => 2665.00m,
                    _ => 1.0m
                };
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            if (cmbAccounts.SelectedIndex < 0)
            {
                MessageBox.Show("Lütfen önce bir hesap seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnRefresh.Enabled = false;
            btnRefresh.Text = "⏳ Güncelleniyor...";

            try
            {
                await RefreshCurrencyRates();
                LoadPortfolioData(); // Portföy verilerini yenile
                MessageBox.Show("Döviz kurları ve portföy değerleri güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kurlar güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRefresh.Enabled = true;
                btnRefresh.Text = "🔄 Yenile";
            }
        }

        private async Task RefreshCurrencyRates()
        {
            try
            {
                // Ücretsiz API kullanımı (exchangerate-api.com)
                string apiUrl = "https://api.exchangerate-api.com/v4/latest/TRY";
                
                using (HttpResponseMessage response = await httpClient.GetAsync(apiUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<ExchangeRateResponse>(json);
                        
                        if (data?.Rates != null)
                        {
                            UpdateDatabaseRates(data.Rates);
                        }
                    }
                    else
                    {
                        throw new Exception($"API yanıtı başarısız: {response.StatusCode}");
                    }
                }
            }
            catch (Exception)
            {
                // API başarısız olursa mock data kullan
                UseMockData();
            }
        }

        private void UseMockData()
        {
            // Mock data ile test
            var mockRates = new Dictionary<string, decimal>
            {
                {"USD", 0.0307m}, // 1 TL = 0.0307 USD yani 1 USD = 32.57 TL
                {"EUR", 0.0281m}, // 1 TL = 0.0281 EUR yani 1 EUR = 35.58 TL
                {"GBP", 0.0243m}, // 1 TL = 0.0243 GBP yani 1 GBP = 41.15 TL
                {"CHF", 0.0276m}, // 1 TL = 0.0276 CHF yani 1 CHF = 36.23 TL
                {"SAR", 0.115m},  // 1 TL = 0.115 SAR yani 1 SAR = 8.70 TL
                {"BHD", 0.0116m}  // 1 TL = 0.0116 BHD yani 1 BHD = 86.21 TL
            };

            UpdateDatabaseRates(mockRates);
        }

        private void UpdateDatabaseRates(Dictionary<string, decimal> rates)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    
                    foreach (var rate in rates)
                    {
                        if (rate.Key == "USD" || rate.Key == "EUR" || rate.Key == "GBP" || 
                            rate.Key == "CHF" || rate.Key == "SAR" || rate.Key == "BHD")
                        {
                            // Önce mevcut kurları önceki kur olarak kaydet
                            string getPrevQuery = "SELECT BuyRate, SellRate FROM CurrencyRates WHERE CurrencyCode = @Code";
                            decimal prevBuyRate = 0, prevSellRate = 0;
                            
                            using (SQLiteCommand getPrevCmd = new SQLiteCommand(getPrevQuery, conn))
                            {
                                getPrevCmd.Parameters.AddWithValue("@Code", rate.Key);
                                using (SQLiteDataReader reader = getPrevCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        prevBuyRate = Convert.ToDecimal(reader["BuyRate"]);
                                        prevSellRate = Convert.ToDecimal(reader["SellRate"]);
                                    }
                                }
                            }

                            // 1 TL = X Foreign Currency şeklinde geliyor, tersini alıyoruz
                            decimal tlRate = 1 / rate.Value;
                            decimal buyRate = tlRate * 0.995m; // %0.5 spread
                            decimal sellRate = tlRate * 1.005m;

                            string updateQuery = @"UPDATE CurrencyRates 
                                                 SET BuyRate = @BuyRate, SellRate = @SellRate, 
                                                     PreviousBuyRate = @PrevBuyRate, PreviousSellRate = @PrevSellRate,
                                                     LastUpdated = CURRENT_TIMESTAMP 
                                                 WHERE CurrencyCode = @Code";
                            
                            using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@BuyRate", buyRate);
                                cmd.Parameters.AddWithValue("@SellRate", sellRate);
                                cmd.Parameters.AddWithValue("@PrevBuyRate", prevBuyRate);
                                cmd.Parameters.AddWithValue("@PrevSellRate", prevSellRate);
                                cmd.Parameters.AddWithValue("@Code", rate.Key);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // Altın için ayrı güncelleme (gram bazında)
                    string goldGetPrevQuery = "SELECT BuyRate, SellRate FROM CurrencyRates WHERE CurrencyCode = 'GOLD'";
                    decimal goldPrevBuyRate = 0, goldPrevSellRate = 0;
                    
                    using (SQLiteCommand goldGetPrevCmd = new SQLiteCommand(goldGetPrevQuery, conn))
                    {
                        using (SQLiteDataReader reader = goldGetPrevCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                goldPrevBuyRate = Convert.ToDecimal(reader["BuyRate"]);
                                goldPrevSellRate = Convert.ToDecimal(reader["SellRate"]);
                            }
                        }
                    }
                    
                    Random rnd = new Random();
                    decimal goldChange = (decimal)(rnd.NextDouble() * 20 - 10); // -10 ile +10 TL arası
                    
                    string goldQuery = @"UPDATE CurrencyRates 
                                       SET BuyRate = @BuyRate, SellRate = @SellRate, 
                                           PreviousBuyRate = @PrevBuyRate, PreviousSellRate = @PrevSellRate,
                                           LastUpdated = CURRENT_TIMESTAMP 
                                       WHERE CurrencyCode = 'GOLD'";
                    
                    using (SQLiteCommand goldCmd = new SQLiteCommand(goldQuery, conn))
                    {
                        goldCmd.Parameters.AddWithValue("@BuyRate", 2665.00m + goldChange);
                        goldCmd.Parameters.AddWithValue("@SellRate", 2670.00m + goldChange);
                        goldCmd.Parameters.AddWithValue("@PrevBuyRate", goldPrevBuyRate);
                        goldCmd.Parameters.AddWithValue("@PrevSellRate", goldPrevSellRate);
                        goldCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Veritabanı hatası olursa sessizce devam et
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                httpClient?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}