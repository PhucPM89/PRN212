using QuanLiKhachSan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLiKhachSan.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginWindowViewModel viewModel)
            {
                Console.WriteLine($"PasswordBox Password: {txtPassword.Password}");
                viewModel.Password = txtPassword.Password;
                if (viewModel.LoginCommand.CanExecute(null))
                {
                    viewModel.LoginCommand.Execute(null);
                }
            }
        }

        private void lblDangKy_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is LoginWindowViewModel viewModel)
            {
                if (viewModel.lblDangKyCommand.CanExecute(null))
                {
                    viewModel.lblDangKyCommand.Execute(null);
                }
            }
        }

        private void lblForgot_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is LoginWindowViewModel viewModel)
            {
                if (viewModel.lblForgotCommand.CanExecute(null))
                {
                    viewModel.lblForgotCommand.Execute(null);
                }
            }
        }
    }
}
