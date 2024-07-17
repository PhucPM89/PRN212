using QuanLiKhachSan.Models;
using QuanLiKhachSan.Support;
using QuanLiKhachSan.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanLiKhachSan.ViewModel
{
    class RegisterWindowViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        MVVMContext _context;

        private AdminAccountService _adminAccountService;

        private CustomCommand _btnHuy;
        public CustomCommand btnHuy
        {
            get { return _btnHuy; }
            set { _btnHuy = value; }
        }

        private CustomCommand _btnDangKy;
        public CustomCommand btnDangKy
        {
            get { return _btnDangKy; }
            set { _btnDangKy = value; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _repassword;
        public string Repassword
        {
            get { return _repassword; }
            set
            {
                _repassword = value;
                OnPropertyChanged(nameof(Repassword));
            }
        }

        public RegisterWindowViewModel()
        {
            _btnHuy = new CustomCommand(btnHuy_Click, (p) => true);
            _btnDangKy = new CustomCommand(BtnRegister_Click, (p) => true);
            _adminAccountService = new AdminAccountService();
        }

        private void BtnRegister_Click(object obj)
        {
            if(!CheckInputControl()) return;

            if (Password != Repassword)
            {
                MessageBox.Show("Mật khẩu không khớp","Thông báo",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            _context = new MVVMContext();
            var customer = new Customer()
            {
                EmailAddress = Username,
                Password = Password,
            };
            _context.Insert(customer);

            MessageBox.Show("Đăng kí thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnHuy_Click(object obj)
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow?.Close();
        }

        private bool CheckInputControl()
        {
            _context = new MVVMContext();
            if (String.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Bạn chưa nhập tài khoản ", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if(!IsValidMail(Username))
            {
                MessageBox.Show("Tài khoản phải là một email hợp lệ", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (_context.IsEmailRegistered(Username) || _adminAccountService.GetAdminAccount().Email == Username)
            {
                MessageBox.Show("Tài khoản đã tồn tại", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (String.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (String.IsNullOrEmpty(Repassword))
            {
                MessageBox.Show("Bạn chưa nhập lại mật khẩu", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private bool IsValidMail(string sEmail)
        {
            var r = new System.Text.RegularExpressions.Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*)@([0-9a-zA-Z][-.\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$");
            return !String.IsNullOrEmpty(sEmail) && r.IsMatch(sEmail);
        }

        private void CloseRegisterWindow(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}
