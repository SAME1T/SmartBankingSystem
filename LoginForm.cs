using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace SmartBankingAutomation
{
    public partial class LoginForm : Form
    {
        private string dbPath = "SmartBank.db";
        public LoginForm()
        {
            InitializeComponent();
            CreateDatabaseAndTables();
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

                SQLiteCommand cmd1 = new SQLiteCommand(createCustomerTable, conn);
                cmd1.ExecuteNonQuery();
                
                conn.Close();
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