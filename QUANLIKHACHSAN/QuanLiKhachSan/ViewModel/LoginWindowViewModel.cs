using QuanLiKhachSan.Models;
using QuanLiKhachSan.Support;
using QuanLiKhachSan.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiKhachSan.ViewModel
{
    class LoginWindowViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        MVVMContext _context;

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

        private CustomCommand _LoginCommand;

        public CustomCommand LoginCommand
        {
            get { return _LoginCommand; }
            set { _LoginCommand = value; }
        }

        private CustomCommand _btnCLoseCommand;

        public CustomCommand btnCLoseCommand
        {
            get { return _btnCLoseCommand; }
            set { _btnCLoseCommand = value; }
        }

        private CustomCommand _lblDangKyCommand;
        public CustomCommand lblDangKyCommand
        {
            get { return _lblDangKyCommand; }
            set { _lblDangKyCommand = value; }
        }



        private CustomCommand _lblForgotCommand;
        public CustomCommand lblForgotCommand
        {
            get { return _lblForgotCommand; }
            set { _lblForgotCommand = value; }
        }

        private AdminAccountService _adminAccountService;

        public LoginWindowViewModel()
        {
            _LoginCommand = new CustomCommand(btnLogin_Click, (p) => true);
            _adminAccountService = new AdminAccountService();
            _btnCLoseCommand = new CustomCommand(btnCLose_Click, (p) => true);
            _lblDangKyCommand = new CustomCommand(lblDangKy_PreviewMouseDown, (p) => true);
            _lblForgotCommand = new CustomCommand(lblForgot_PreviewMouseDown, (p) => true);
        }

        private void lblForgot_PreviewMouseDown(object obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
                forgotPasswordWindow.ShowDialog();
            });
        }

        private void lblDangKy_PreviewMouseDown(object obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RegisterWindow registerWindow = new RegisterWindow();
                registerWindow.ShowDialog();
            });
        }

        private void btnCLose_Click(object obj)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object obj)
        {
            _context = new MVVMContext();
            var adminAccount = _adminAccountService.GetAdminAccount();
            Console.WriteLine($"Loaded AdminAccount: Email={adminAccount.Email}, Password={adminAccount.Password}"); // Debugging line
            if (Username == adminAccount.Email && Password == adminAccount.Password)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    Application.Current.MainWindow.Close();
                });
            }
            else if (_context.checkLogin(Username, Password) == true && Username != "" && Password != "")
            {
                var customer = _context.GetCustomerByEmail(Username);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CustomerWindow customerWindow = new CustomerWindow(customer);
                    customerWindow.ShowDialog();
                    Application.Current.MainWindow.Close();
                });
            }

            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không hợp lệ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
