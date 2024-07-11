using ProjectPRN2.Support;
using ProjectPRN2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectPRN2.ViewModel
{
    public class MainWindowViewModel
    {
        public CustomCommand _QLLopHocCommand;

        public CustomCommand QLLopHocCommand
        {
            get { return _QLLopHocCommand;}
            set {  _QLLopHocCommand = value;}
        }

        public MainWindowViewModel()
        {
            //Tham số 1: là hàm được thực thi khi ấn vào button
            //Tham số 2: là điều kiện để hàm (tham số 1) thực thi (=true) là hàm luôn được thực thi
            _QLLopHocCommand = new CustomCommand(btnQLLopHoc_click, (p) => true);
        }

        private void btnQLLopHoc_click(object obj)
        {
            //tham số thứ 2 bằng true nên hàm sẽ được thực hiện
            try
            {
                //MessageBox.Show("Bạn vừa click vào button QL lớp học","Thông báo",System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                QLLH frm = new QLLH();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
