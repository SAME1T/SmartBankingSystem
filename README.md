# 🏦 Smart Banking Automation

Modern ve kullanıcı dostu bir banka otomasyonu uygulaması. C# Windows Forms ve SQLite veritabanı kullanılarak geliştirilmiştir.

## 📸 Ekran Görüntüleri

### Giriş / Kayıt Ekranı
- 📝 Kullanıcı kaydı (Ad, Soyad, TC No, Telefon, Şifre)
- 🔐 Güvenli giriş sistemi (TC No + Şifre)
- 📊 Veritabanı görüntüleyici

### Ana Sayfa
- 👤 Kullanıcı bilgileri paneli
- 💳 Hesap açma
- 💰 Para yatırma/çekme
- 🔄 Para transferi
- 📊 Hesap bilgileri

## 🚀 Özellikler

### 🔐 Güvenlik
- ✅ Şifreli kullanıcı hesapları
- ✅ TC No doğrulama
- ✅ Güvenli veritabanı bağlantısı
- ✅ Oturum yönetimi

### 💼 Banka İşlemleri
- ✅ Müşteri kaydı ve yönetimi
- ✅ Hesap oluşturma (Vadeli/Vadesiz)
- ✅ Para yatırma işlemleri
- ✅ Para çekme işlemleri
- ✅ Hesaplar arası transfer
- ✅ İşlem geçmişi takibi
- ✅ Bakiye sorgulama

### 🎨 Kullanıcı Arayüzü
- ✅ Modern flat tasarım
- ✅ Kullanıcı dostu arayüz
- ✅ Responsive düzen
- ✅ Görsel avatar sistemi
- ✅ Renkli kategori butonları
- ✅ Emoji destekli menüler

## 🛠️ Teknolojiler

- **Framework:** .NET 6.0 Windows Forms
- **Veritabanı:** SQLite
- **Dil:** C#
- **IDE:** Visual Studio / VS Code
- **Platform:** Windows

## 📋 Gereksinimler

- Windows 10/11
- .NET 6.0 Runtime
- SQLite (dahil)

## 🚀 Kurulum

1. **Projeyi klonlayın:**
```bash
git clone https://github.com/kullaniciadi/SmartBankingAutomation.git
cd SmartBankingAutomation
```

2. **Bağımlılıkları yükleyin:**
```bash
dotnet restore
```

3. **Projeyi derleyin:**
```bash
dotnet build
```

4. **Uygulamayı çalıştırın:**
```bash
dotnet run
```

## 📊 Veritabanı Yapısı

### Customers Tablosu
```sql
CREATE TABLE Customers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Surname TEXT NOT NULL,
    TcNo TEXT NOT NULL UNIQUE,
    Phone TEXT,
    Password TEXT NOT NULL
);
```

### Accounts Tablosu
```sql
CREATE TABLE Accounts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerId INTEGER NOT NULL,
    AccountNumber TEXT NOT NULL UNIQUE,
    Balance DECIMAL(10,2) DEFAULT 0.00,
    AccountType TEXT DEFAULT 'Vadeli',
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
```

### Transactions Tablosu
```sql
CREATE TABLE Transactions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    AccountId INTEGER NOT NULL,
    TransactionType TEXT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    Description TEXT,
    TransactionDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
);
```

## 🎯 Kullanım

### 1. İlk Kullanım
1. Uygulamayı başlatın
2. "Kayıt Paneli"nden yeni kullanıcı oluşturun
3. Bilgilerinizi girin ve "💾 Kayıt Ol" butonuna tıklayın

### 2. Giriş Yapma
1. "Giriş Paneli"nde TC No ve şifrenizi girin
2. "🚪 Giriş Yap" butonuna tıklayın
3. Ana sayfaya yönlendirileceksiniz

### 3. Banka İşlemleri
- **💳 Hesap Aç:** Yeni banka hesabı açın
- **💰 Para Yatır:** Hesabınıza para yatırın
- **💸 Para Çek:** Hesabınızdan para çekin
- **🔄 Para Transfer:** Hesaplar arası transfer yapın
- **📊 Hesap Bilgileri:** Bakiye ve işlem geçmişini görün

## 🔧 Geliştirme

### Kod Yapısı
```
SmartBankingAutomation/
├── Form1.cs              # Ana sayfa (Banka işlemleri)
├── Form1.Designer.cs     # Ana sayfa tasarımı
├── LoginForm.cs          # Giriş/Kayıt sayfası
├── LoginForm.Designer.cs # Giriş/Kayıt tasarımı
├── Program.cs            # Uygulama başlatıcı
├── SmartBank.db          # SQLite veritabanı
└── README.md             # Bu dosya
```

### Yeni Özellik Ekleme
1. Veritabanı değişiklikleri için `CreateDatabaseAndTables()` metodunu güncelleyin
2. UI değişiklikleri için `.Designer.cs` dosyalarını düzenleyin
3. İş mantığı için `.cs` dosyalarına yeni metodlar ekleyin

## 🐛 Sorun Giderme

### Veritabanı Sorunları
- **"Database is locked" hatası:** Uygulamayı tamamen kapatıp yeniden başlatın
- **Tablo bulunamadı:** `SmartBank.db` dosyasını silin, otomatik olarak yeniden oluşturulacak

### Giriş Sorunları
- **TC No bulunamadı:** Önce kayıt olduğunuzdan emin olun
- **Şifre hatalı:** Kayıt olurken kullandığınız şifreyi kontrol edin

## 🤝 Katkıda Bulunma

1. Bu repository'yi fork edin
2. Yeni bir feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

## 👥 Geliştiriciler

- **Ana Geliştirici:** [İsminiz]
- **Email:** [email@example.com]
- **GitHub:** [@kullaniciadi]

## 🙏 Teşekkürler

- .NET Community
- SQLite Team
- Windows Forms geliştiricileri

## 📈 Versiyon Geçmişi

### v1.0.0 (2024)
- ✅ Kullanıcı kayıt/giriş sistemi
- ✅ Temel banka işlemleri
- ✅ Modern UI tasarımı
- ✅ SQLite veritabanı entegrasyonu

---

💡 **İpucu:** Daha fazla yardım için GitHub Issues bölümünü kullanabilirsiniz.

🏦 **Smart Banking** - Modern bankacılık deneyimi!