using QuanLiKhachSan.Models;
using QuanLiKhachSan.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiKhachSan.ViewModel
{
    class QLKHViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(object propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName.ToString()));
        }

        MVVMContext _context;

        private CustomCommand _LoadedCommand;

        public CustomCommand LoadedCommand
        {
            get { return _LoadedCommand; }
            set { _LoadedCommand = value; }
        }

        private ObservableCollection<Customer> _gridItemSource;

        public ObservableCollection<Customer> GridItemSource
        {
            get { return _gridItemSource; }
            set
            {
                _gridItemSource = value;
                OnPropertyChanged("GridItemSource");
            }
        }

        private Customer _selectDataRow;

        public Customer SelectDataRow
        {
            get { return _selectDataRow; }
            set
            {
                _selectDataRow = value;
                OnPropertyChanged("SelectDataRow");
                if (_selectDataRow != null)
                {
                    CustomerIdTextBox = _selectDataRow.CustomerId; 
                    NameTextBox = _selectDataRow.CustomerFullName ?? ""; 
                    PhoneTextBox = _selectDataRow.Telephone ?? "";
                    EmailTextBox = _selectDataRow.EmailAddress ?? "";
                    BirthdayDateTime = _selectDataRow.CustomerBirthday.HasValue ? _selectDataRow.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
                    StatusTextBox = _selectDataRow.CustomerStatus.HasValue ? (byte)_selectDataRow.CustomerStatus : (byte)0; // Xử lý giá trị null
                }
            }
        }

        private int _CustomerIdTextBox;

        public int CustomerIdTextBox
        {
            get { return _CustomerIdTextBox; }
            set
            {
                _CustomerIdTextBox = value;
                OnPropertyChanged("CustomerIdTextBox");
            }
        }

        private string _NameTextBox;

        public string NameTextBox
        {
            get { return _NameTextBox; }
            set
            {
                _NameTextBox = value;
                OnPropertyChanged("NameTextBox");
            }
        }


        private string _EmailTextBox;

        public string EmailTextBox
        {
            get { return _EmailTextBox; }
            set
            {
                _EmailTextBox = value;
                OnPropertyChanged("EmailTextBox");
            }
        }


        private string _PhoneTextBox;

        public string PhoneTextBox
        {
            get { return _PhoneTextBox; }
            set
            {
                _PhoneTextBox = value;
                OnPropertyChanged("PhoneTextBox");
            }
        }


        private DateOnly _BirthdayTextBox;

        public DateOnly BirthdayTextBox
        {
            get { return _BirthdayTextBox; }
            set
            {
                _BirthdayTextBox = value;
                OnPropertyChanged("BirthdayTextBox");
            }
        }

        private DateTime? _BirthdayDateTime;
        public DateTime? BirthdayDateTime
        {
            get { return _BirthdayDateTime; }
            set
            {
                _BirthdayDateTime = value;
                if (_BirthdayDateTime.HasValue)
                {
                    BirthdayTextBox = DateOnly.FromDateTime(_BirthdayDateTime.Value);
                }
                OnPropertyChanged(nameof(BirthdayDateTime));
            }
        }

        private byte _StatusTextBox;

        public byte StatusTextBox
        {
            get { return _StatusTextBox; }
            set
            {
                _StatusTextBox = value;
                OnPropertyChanged("StatusTextBox");
            }
        }

        private CustomCommand _btnAdd;

        public CustomCommand btnAdd
        {
            get { return _btnAdd; }
            set { _btnAdd = value; }
        }

        private CustomCommand _btnEdit;

        public CustomCommand btnEdit
        {
            get { return _btnEdit; }
            set { _btnEdit = value; }
        }


        public QLKHViewModel()
        {
            _LoadedCommand = new CustomCommand(Loaded, (p) => true);
            //_btnAdd = new CustomCommand(btnAdd_Click, (p) => true);
            _btnEdit = new CustomCommand(btnEdit_Click,(p)=>true);
        }


        private void btnEdit_Click(object obj)
        {
            try
            {
                if (SelectDataRow == null)
                {
                    MessageBox.Show("Bạn chưa chọn thông tin khách hàng cần sửa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MessageBox.Show("Bạn có muốn cập nhật thông tin khách hàng này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Customer customer = new Customer();
                    customer.CustomerId = _selectDataRow.CustomerId;
                    customer.Telephone = _PhoneTextBox;
                    customer.CustomerFullName = _NameTextBox;
                    customer.CustomerBirthday = _BirthdayTextBox;
                    customer.EmailAddress = _EmailTextBox;
                    customer.CustomerStatus = _StatusTextBox;
                    _context = new MVVMContext();
                    _context.Update(customer);
                    Loaded(obj);
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    //FocusRow(customer);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void btnAdd_Click(object obj)
        //{
        //    try
        //    {
        //        if (((QuanLiKhachSan.View.QLKH)(obj)).NameTextBox.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Bạn chưa nhập tên khách hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        if (((QuanLiKhachSan.View.QLKH)(obj)).EmailTextBox.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Bạn chưa nhập email", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        if (((QuanLiKhachSan.View.QLKH)(obj)).PhoneTextBox.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Bạn chưa nhập số điện thoại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }


        //        if (((QuanLiKhachSan.View.QLKH)(obj)).BirthdayTextBox.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Bạn chưa chọn ngày sinh", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        if (((QuanLiKhachSan.View.QLKH)(obj)).StatusTextBox.Text.Trim() == "")
        //        {
        //            MessageBox.Show("Bạn chưa chọn trạng thái", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        if (MessageBox.Show("Bạn có muốn thêm khách hàng này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes)
        //        {
        //            Customer customer = new Customer();
        //            customer.Telephone = _PhoneTextBox;
        //            customer.CustomerFullName = _NameTextBox;
        //            customer.CustomerBirthday = _BirthdayTextBox;
        //            customer.EmailAddress = _EmailTextBox;
        //            customer.CustomerStatus = _StatusTextBox;
        //            _context = new MVVMContext();
        //            _context.Insert(customer);
        //            Loaded(obj);
                    
        //            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //            FocusRow(customer);
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Đã có lỗi xảy ra!","Thông báo",MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void FocusRow(Customer customer)
        //{
        //    var mainWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(window => window.GetType() == typeof(QuanLiKhachSan.View.QLKH));
        //    if (mainWindow != null)
        //    {
        //        var dataGrid = mainWindow.FindName("CustomerDataGrid") as DataGrid;
        //        if (dataGrid != null && customer != null && dataGrid.Items.Contains(customer))
        //        {
        //            dataGrid.UpdateLayout();
        //            dataGrid.ScrollIntoView(customer);
        //            dataGrid.SelectedItem = customer;
        //        }
        //    }
        //}

        private void Loaded(object obj)
        {
            try
            {
                _context = new MVVMContext();
                GridItemSource = _context.GetList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
