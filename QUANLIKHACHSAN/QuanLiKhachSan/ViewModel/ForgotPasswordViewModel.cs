using QuanLiKhachSan.Models;
using QuanLiKhachSan.Support;
using QuanLiKhachSan.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiKhachSan.ViewModel
{
    class ForgotPasswordViewModel
    {
        public string APP_EMAIL = "minhphuc2308031@gmail.com";
        public string APP_PASSWORD = "kkfd srhc xokr xizv";

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        MVVMContext _context;

        private CustomCommand _btnDong;
        public CustomCommand btnDong
        {
            get { return _btnDong; }
            set { _btnDong = value; }
        }

        private CustomCommand _btnSend;
        public CustomCommand btnSend
        {
            get { return _btnSend; }
            set { _btnSend = value; }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public ForgotPasswordViewModel()
        {
            _btnDong = new CustomCommand(btnDong_click, (p) => true);
            _btnSend = new CustomCommand(btnSend_click, (p) => true);
        }

        private void btnSend_click(object obj)
        {
            if (String.IsNullOrEmpty(Email))
            {
                MessageBox.Show("Vui lòng nhập email", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            _context = new MVVMContext();
            if (String.IsNullOrEmpty(Email))
            {
                MessageBox.Show("Vui lòng nhập email", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (_context.IsEmailRegistered(Email))
            {
                string newPassword = GenerateRandomPassword();
                _context.UpdatePassword(Email, newPassword);
                string sTo = Email;
                string sSubject = "MẬT KHẨU MỚI";
                string sBody = $"<p>Mật khẩu mới của bạn là: <strong>{newPassword}</strong></p>";
                SendPasswordToMail(APP_EMAIL, APP_PASSWORD, sTo, sSubject, sBody);
            }
            else
            {
                MessageBox.Show("Email không tồn tại trong hệ thống, vui lòng kiểm tra lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDong_click(object obj)
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow?.Close();
        }

        private void SendPasswordToMail(string sFrom, string sPass, string sTo, string sSubject, string sBody)
        {
            try
            {
                var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new System.Net.NetworkCredential(sFrom, sPass),
                    EnableSsl = true
                };
                var message = new System.Net.Mail.MailMessage(sFrom, sTo, sSubject, sBody)
                {
                    IsBodyHtml = true
                };
                client.Host = "smtp.gmail.com";
                client.Send(message);
                MessageBox.Show("Gửi mail thành công!", "Chúc mừng", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public string GenerateRandomPassword()
        {
            const string validChars = "1234567890";
            StringBuilder result = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                result.Append(validChars[random.Next(validChars.Length)]);
            }

            return result.ToString();
        }
    }
}
