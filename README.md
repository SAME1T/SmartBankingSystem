# 🏦 Smart Banking Automation

Modern ve kullanıcı dostu bir banka otomasyonu uygulaması. C# Windows Forms ve SQLite veritabanı kullanılarak geliştirilmiştir.

## 📸 Ekran Görüntüleri

### Giriş / Kayıt Ekranı
- 📝 Kullanıcı kaydı (Ad, Soyad, TC No, Telefon, Şifre)
- 🔐 Güvenli giriş sistemi (TC No + Şifre)
- 📊 Veritabanı görüntüleyici

### Ana Sayfa
- 👤 Kullanıcı bilgileri paneli
- 💳 Hesap açma (Vadeli/Vadesiz) - Vade süresi ve faiz seçimi
- 💰 Para yatırma/çekme
- 🔄 Gelişmiş para transferi - Yapıştırma, onaylama, alıcı gösterme
- 📊 Hesap bilgileri - Kopyalama özelliği
- 💳 Kredi kartı işlemleri

## 🚀 Özellikler

### 🔐 Güvenlik
- ✅ Şifreli kullanıcı hesapları
- ✅ TC No doğrulama
- ✅ Güvenli veritabanı bağlantısı
- ✅ Oturum yönetimi

### 💼 Banka İşlemleri
- ✅ Müşteri kaydı ve yönetimi
- ✅ **Gelişmiş hesap oluşturma** (Vadeli/Vadesiz)
  - 📅 Vadeli hesap için vade süresi seçimi (1-24 ay)
  - 💰 Faiz oranı seçimi (%15-%28)
  - 🎯 Otomatik vade sonu tutarı hesaplama
  - 💵 Başlangıç tutarı belirleme
- ✅ Para yatırma işlemleri
- ✅ Para çekme işlemleri
- ✅ **Gelişmiş hesaplar arası transfer**
  - 📥 Hesap numarası yapıştırma özelliği
  - 👤 Alıcının ismini otomatik gösterme
  - ✅ Transfer öncesi detaylı onaylama ekranı
- ✅ İşlem geçmişi takibi
- ✅ **Gelişmiş bakiye sorgulama**
  - 📋 Hesap numarası kopyalama özelliği
  - 📊 Detaylı hesap bilgileri görüntüleme

### 💳 Kredi Kartı İşlemleri
- ✅ **Kredi kartı başvurusu**
  - 📝 Yeni kredi kartı talep etme
  - 🏦 Otomatik kart numarası oluşturma
  - 💰 Kredi limiti belirleme
- ✅ **Kredi kartı ödeme işlemleri**
  - 💰 Borç ödeme sistemi
  - 📊 Mevcut borç durumu görüntüleme
- ✅ **Kredi kartı harcama işlemleri**
  - 🛒 Alışveriş simülasyonu
  - 📈 Harcama geçmişi takibi
  - 🏪 Mağaza bilgileri kaydetme
- ✅ **Kredi kartı yönetimi**
  - 📋 Tüm kartları görüntüleme
  - 💳 Kart detayları ve limitleri
  - 📊 Faiz oranı takibi

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
    MaturityDate DATETIME,              -- Vadeli hesap vade tarihi
    InterestRate DECIMAL(5,2) DEFAULT 0.00,  -- Faiz oranı
    MaturityAmount DECIMAL(10,2) DEFAULT 0.00, -- Vade sonu tutarı
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

### CreditCards Tablosu
```sql
CREATE TABLE CreditCards (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerId INTEGER NOT NULL,
    CardNumber TEXT NOT NULL UNIQUE,    -- Kredi kartı numarası
    CardName TEXT NOT NULL,             -- Kart adı
    CreditLimit DECIMAL(10,2) DEFAULT 0.00,   -- Kredi limiti
    AvailableLimit DECIMAL(10,2) DEFAULT 0.00, -- Kullanılabilir limit
    Debt DECIMAL(10,2) DEFAULT 0.00,    -- Mevcut borç
    InterestRate DECIMAL(5,2) DEFAULT 2.50,   -- Faiz oranı
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    ExpiryDate DATETIME,                -- Son kullanma tarihi
    IsActive INTEGER DEFAULT 1,         -- Kart aktif mi
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
```

### CreditCardTransactions Tablosu
```sql
CREATE TABLE CreditCardTransactions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CreditCardId INTEGER NOT NULL,
    TransactionType TEXT NOT NULL,      -- Harcama, Ödeme
    Amount DECIMAL(10,2) NOT NULL,
    Description TEXT,
    MerchantName TEXT,                  -- Mağaza adı
    TransactionDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CreditCardId) REFERENCES CreditCards(Id)
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
- **💳 Hesap Aç:** 
  - Vadeli/Vadesiz hesap seçimi
  - Vade süresi belirleme (Vadeli için)
  - Faiz oranı seçimi
  - Başlangıç tutarı girme
  - Otomatik vade sonu hesaplama
- **💰 Para Yatır:** Hesabınıza para yatırın
- **💸 Para Çek:** Hesabınızdan para çekin
- **🔄 Gelişmiş Para Transfer:** 
  - 📋 Hesap numarası kopyala/yapıştır
  - 👤 Alıcının ismini otomatik görüntüleme
  - ✅ Transfer öncesi onaylama ekranı
- **📊 Hesap Bilgileri:** 
  - 📋 Hesap numarası kopyalama
  - 📊 Detaylı bakiye ve işlem geçmişi
  - 📅 Vadeli hesap detayları

### 4. Kredi Kartı İşlemleri
- **📝 Kredi Kartı Başvurusu:**
  - Yeni kart talep etme
  - Limit belirleme
  - Otomatik kart numarası oluşturma
- **💰 Kredi Kartı Ödeme:**
  - Mevcut borç görüntüleme
  - Borç ödeme işlemi
- **🛒 Kredi Kartı Harcama:**
  - Alışveriş simülasyonu
  - Mağaza bilgisi girme
  - Harcama geçmişi takibi
- **📊 Kredi Kartı Yönetimi:**
  - Tüm kartları görüntüleme
  - Limit ve borç durumu
  - Faiz oranı bilgileri

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

- **Ana Geliştirici:** Samet Çiftci
- **Email:** scsametciftci@gmail.com
- **GitHub:** [@SAME1T]
- **Proje:** Smart Banking Automation v1.1.0

## 🙏 Teşekkürler

- .NET Community
- SQLite Team
- Windows Forms geliştiricileri

## 📈 Versiyon Geçmişi

### v1.1.0 (2025)
- ✅ **Gelişmiş Vadeli Hesap Sistemi**
  - 📅 Vade süresi seçimi (1-24 ay)
  - 💰 Faiz oranı seçimi (%15-%28)
  - 🎯 Otomatik vade sonu hesaplama
  - 💵 Başlangıç tutarı özelliği
- ✅ **Kredi Kartı İşlemleri**
  - 📝 Kredi kartı başvuru sistemi
  - 💰 Kredi kartı ödeme işlemleri
  - 🛒 Kredi kartı harcama sistemi
  - 📊 Kredi kartı yönetim paneli
- ✅ **Gelişmiş Transfer Sistemi**
  - 📥 Hesap numarası yapıştırma özelliği
  - 👤 Alıcının ismini otomatik gösterme
  - ✅ Transfer öncesi detaylı onaylama
- ✅ **Kullanıcı Deneyimi İyileştirmeleri**
  - 📋 Hesap numarası kopyalama özellikleri
  - 🎨 Gelişmiş UI tasarımı
  - 📊 Detaylı bilgi panelleri

### v1.0.0 (2024)
- ✅ Kullanıcı kayıt/giriş sistemi
- ✅ Temel banka işlemleri
- ✅ Modern UI tasarımı
- ✅ SQLite veritabanı entegrasyonu

---

💡 **İpucu:** Daha fazla yardım için GitHub Issues bölümünü kullanabilirsiniz.

🏦 **Smart Banking v1.1.0** - Gelişmiş vadeli hesap ve kredi kartı sistemi ile modern bankacılık deneyimi!