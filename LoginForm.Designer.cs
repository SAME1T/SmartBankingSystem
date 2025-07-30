namespace SmartBankingAutomation
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblSurname;
        private System.Windows.Forms.Label lblTcNo;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtSurname;
        private System.Windows.Forms.TextBox txtTcNo;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblTcLogin;
        private System.Windows.Forms.Label lblPasswordLogin;
        private System.Windows.Forms.TextBox txtTcLogin;
        private System.Windows.Forms.TextBox txtPasswordLogin;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnViewData;
        private System.Windows.Forms.GroupBox groupRegister;
        private System.Windows.Forms.GroupBox groupLogin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupRegister = new System.Windows.Forms.GroupBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblSurname = new System.Windows.Forms.Label();
            this.lblTcNo = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.txtTcNo = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.groupLogin = new System.Windows.Forms.GroupBox();
            this.lblTcLogin = new System.Windows.Forms.Label();
            this.lblPasswordLogin = new System.Windows.Forms.Label();
            this.txtTcLogin = new System.Windows.Forms.TextBox();
            this.txtPasswordLogin = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnViewData = new System.Windows.Forms.Button();
            this.groupRegister.SuspendLayout();
            this.groupLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupRegister
            // 
            this.groupRegister.Controls.Add(this.lblName);
            this.groupRegister.Controls.Add(this.lblSurname);
            this.groupRegister.Controls.Add(this.lblTcNo);
            this.groupRegister.Controls.Add(this.lblPhone);
            this.groupRegister.Controls.Add(this.lblPassword);
            this.groupRegister.Controls.Add(this.txtName);
            this.groupRegister.Controls.Add(this.txtSurname);
            this.groupRegister.Controls.Add(this.txtTcNo);
            this.groupRegister.Controls.Add(this.txtPhone);
            this.groupRegister.Controls.Add(this.txtPassword);
            this.groupRegister.Controls.Add(this.btnRegister);
            this.groupRegister.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);
            this.groupRegister.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupRegister.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            this.groupRegister.Location = new System.Drawing.Point(20, 20);
            this.groupRegister.Name = "groupRegister";
            this.groupRegister.Size = new System.Drawing.Size(360, 200);
            this.groupRegister.TabIndex = 0;
            this.groupRegister.TabStop = false;
            this.groupRegister.Text = "📝 Kayıt Paneli";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            this.lblName.Location = new System.Drawing.Point(20, 30);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(29, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Ad:";
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtName.Location = new System.Drawing.Point(100, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(230, 23);
            this.txtName.TabIndex = 1;
            // 
            // lblSurname
            // 
            this.lblSurname.AutoSize = true;
            this.lblSurname.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSurname.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            this.lblSurname.Location = new System.Drawing.Point(20, 60);
            this.lblSurname.Name = "lblSurname";
            this.lblSurname.Size = new System.Drawing.Size(47, 15);
            this.lblSurname.TabIndex = 2;
            this.lblSurname.Text = "Soyad:";
            // 
            // txtSurname
            // 
            this.txtSurname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSurname.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSurname.Location = new System.Drawing.Point(100, 57);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(230, 23);
            this.txtSurname.TabIndex = 3;
            // 
            // lblTcNo
            // 
            this.lblTcNo.AutoSize = true;
            this.lblTcNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTcNo.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            this.lblTcNo.Location = new System.Drawing.Point(20, 90);
            this.lblTcNo.Name = "lblTcNo";
            this.lblTcNo.Size = new System.Drawing.Size(44, 15);
            this.lblTcNo.TabIndex = 4;
            this.lblTcNo.Text = "TC No:";
            // 
            // txtTcNo
            // 
            this.txtTcNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTcNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTcNo.Location = new System.Drawing.Point(100, 87);
            this.txtTcNo.Name = "txtTcNo";
            this.txtTcNo.Size = new System.Drawing.Size(230, 23);
            this.txtTcNo.TabIndex = 5;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPhone.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            this.lblPhone.Location = new System.Drawing.Point(20, 120);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(51, 15);
            this.lblPhone.TabIndex = 6;
            this.lblPhone.Text = "Telefon:";
            // 
            // txtPhone
            // 
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPhone.Location = new System.Drawing.Point(100, 117);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(230, 23);
            this.txtPhone.TabIndex = 7;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(25, 25, 112);
            this.lblPassword.Location = new System.Drawing.Point(20, 150);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(36, 15);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Şifre:";
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPassword.Location = new System.Drawing.Point(100, 147);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(230, 23);
            this.txtPassword.TabIndex = 9;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(34, 139, 34);
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(230, 155);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(100, 30);
            this.btnRegister.TabIndex = 10;
            this.btnRegister.Text = "💾 Kayıt Ol";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // groupLogin
            // 
            this.groupLogin.Controls.Add(this.lblTcLogin);
            this.groupLogin.Controls.Add(this.lblPasswordLogin);
            this.groupLogin.Controls.Add(this.txtTcLogin);
            this.groupLogin.Controls.Add(this.txtPasswordLogin);
            this.groupLogin.Controls.Add(this.btnLogin);
            this.groupLogin.BackColor = System.Drawing.Color.FromArgb(248, 248, 255);
            this.groupLogin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupLogin.ForeColor = System.Drawing.Color.FromArgb(139, 0, 0);
            this.groupLogin.Location = new System.Drawing.Point(20, 240);
            this.groupLogin.Name = "groupLogin";
            this.groupLogin.Size = new System.Drawing.Size(360, 120);
            this.groupLogin.TabIndex = 1;
            this.groupLogin.TabStop = false;
            this.groupLogin.Text = "🔐 Giriş Paneli";
            // 
            // lblTcLogin
            // 
            this.lblTcLogin.AutoSize = true;
            this.lblTcLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTcLogin.ForeColor = System.Drawing.Color.FromArgb(139, 0, 0);
            this.lblTcLogin.Location = new System.Drawing.Point(20, 35);
            this.lblTcLogin.Name = "lblTcLogin";
            this.lblTcLogin.Size = new System.Drawing.Size(44, 15);
            this.lblTcLogin.TabIndex = 0;
            this.lblTcLogin.Text = "TC No:";
            // 
            // txtTcLogin
            // 
            this.txtTcLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTcLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTcLogin.Location = new System.Drawing.Point(100, 32);
            this.txtTcLogin.Name = "txtTcLogin";
            this.txtTcLogin.Size = new System.Drawing.Size(230, 23);
            this.txtTcLogin.TabIndex = 1;
            // 
            // lblPasswordLogin
            // 
            this.lblPasswordLogin.AutoSize = true;
            this.lblPasswordLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPasswordLogin.ForeColor = System.Drawing.Color.FromArgb(139, 0, 0);
            this.lblPasswordLogin.Location = new System.Drawing.Point(20, 65);
            this.lblPasswordLogin.Name = "lblPasswordLogin";
            this.lblPasswordLogin.Size = new System.Drawing.Size(36, 15);
            this.lblPasswordLogin.TabIndex = 2;
            this.lblPasswordLogin.Text = "Şifre:";
            // 
            // txtPasswordLogin
            // 
            this.txtPasswordLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPasswordLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPasswordLogin.Location = new System.Drawing.Point(100, 62);
            this.txtPasswordLogin.Name = "txtPasswordLogin";
            this.txtPasswordLogin.PasswordChar = '*';
            this.txtPasswordLogin.Size = new System.Drawing.Size(230, 23);
            this.txtPasswordLogin.TabIndex = 3;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(70, 130, 180);
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(230, 85);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 25);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "🚪 Giriş Yap";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 420);
            this.Controls.Add(this.btnViewData);
            this.Controls.Add(this.groupLogin);
            this.Controls.Add(this.groupRegister);
            // 
            // btnViewData
            // 
            this.btnViewData.BackColor = System.Drawing.Color.FromArgb(128, 0, 128);
            this.btnViewData.FlatAppearance.BorderSize = 0;
            this.btnViewData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnViewData.ForeColor = System.Drawing.Color.White;
            this.btnViewData.Location = new System.Drawing.Point(150, 375);
            this.btnViewData.Name = "btnViewData";
            this.btnViewData.Size = new System.Drawing.Size(100, 30);
            this.btnViewData.TabIndex = 2;
            this.btnViewData.Text = "📊 Verileri Gör";
            this.btnViewData.UseVisualStyleBackColor = false;
            this.btnViewData.Click += new System.EventHandler(this.btnViewData_Click);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🏦 Smart Banking - Giriş / Kayıt";
            this.groupRegister.ResumeLayout(false);
            this.groupRegister.PerformLayout();
            this.groupLogin.ResumeLayout(false);
            this.groupLogin.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}