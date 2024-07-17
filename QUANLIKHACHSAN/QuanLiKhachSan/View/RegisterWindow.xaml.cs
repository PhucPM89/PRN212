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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if(DataContext is RegisterWindowViewModel viewModel)
            {
                viewModel.Password = password.Password;
                viewModel.Repassword = repassword.Password;

                if (viewModel.btnDangKy.CanExecute(null))
                {
                    viewModel.btnDangKy.Execute(null);
                }
            }
        }
    }
}
