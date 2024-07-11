using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectPRN2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        //private void btnLogin_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!AllowLogin())
        //    {
        //        return;
        //    }

        //    //Lấy dữ liệu từ database so sánh
        //    DataTable dtData = ConnectDatabase.Connect.DataTransport("SELECT * FROM [QuanLySinhVien].[dbo].[SinhVien]");

        //    for (int i = 0; i < dtData.Rows.Count; i++)
        //    {
        //        //Kiểm tra tài khoản có trong database không
        //        if(txtUserName.Text.Trim() == Convert.ToString(dtData.Rows[i]["Email"]).Trim())
        //        {
        //            //tài khoản đúng thì kiểm tra tiếp mật khẩu
        //            if(txtPassword.Password == Convert.ToString(dtData.Rows[i]["MatKhau"]))
        //            {
        //                //Đăng nhập thành công
        //                MessageBox.Show("Đăng nhập thành công", "Chúc mừng", MessageBoxButton.OK, MessageBoxImage.Information);
        //                return;
        //            }
        //            else
        //            {
        //                MessageBox.Show("Mật khẩu bạn nhập không chính xác", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Tài khoản bạn nhập không chính xác","Lỗi",MessageBoxButton.OK,MessageBoxImage.Error);
        //            txtUserName.Focus();
        //            return;
        //        }
        //    }
        //}

        //private bool AllowLogin()
        //{
        //    if (txtUserName.Text.Trim() == "")
        //    {
        //        MessageBox.Show("Bạn chưa nhập Tài khoản","Cảnh báo",MessageBoxButton.OK,MessageBoxImage.Warning);
        //        txtUserName.Focus();
        //        return false; 
        //    }
        //    if (txtPassword.Password.Trim() == "")
        //    {
        //        MessageBox.Show("Bạn chưa nhập Mật khẩu", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        txtPassword.Focus();
        //        return false;
        //    }
        //    return true;
        //}

        //private void btnClose_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close(); 
        //}
    }
}