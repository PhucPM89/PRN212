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

namespace QuanLiKhachSan.ViewModel
{
    internal class CustomerWindowViewModel : INotifyPropertyChanged
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

        private ObservableCollection<BookingDetailView> _gridItemSource;
        public ObservableCollection<BookingDetailView> GridItemSource
        {
            get { return _gridItemSource; }
            set
            {
                _gridItemSource = value;
                OnPropertyChanged(nameof(GridItemSource));
            }
        }


        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        private CustomCommand _btnLuu;

        public CustomCommand btnLuu
        {
            get { return _btnLuu; }
            set { _btnLuu = value; }
        }

        public DateTime? CustomerBirthdayDateTime
        {
            get
            {
                if (Customer.CustomerBirthday.HasValue)
                {
                    var date = Customer.CustomerBirthday.Value;
                    return new DateTime(date.Year, date.Month, date.Day);
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    var date = value.Value;
                    Customer.CustomerBirthday = new DateOnly(date.Year, date.Month, date.Day);
                }
                else
                {
                    Customer.CustomerBirthday = null;
                }
                OnPropertyChanged(nameof(CustomerBirthdayDateTime));
            }
        }

        public CustomerWindowViewModel(Customer customer)
        {
            Customer = customer;
            _LoadedCommand = new CustomCommand(Loaded,(p)=>true);
            _btnLuu = new CustomCommand(btnLuu_Click,(p)=>true);
        }

        private void btnLuu_Click(object obj)
        {
            using (var _context = new FuminiHotelManagementContext())
            {
                var customerInDb = _context.Customers.FirstOrDefault(c => c.CustomerId == Customer.CustomerId);
                if (customerInDb != null)
                {
                    customerInDb.CustomerFullName = Customer.CustomerFullName;
                    customerInDb.Telephone = Customer.Telephone;
                    customerInDb.EmailAddress = Customer.EmailAddress;
                    customerInDb.Password = Customer.Password;
                    customerInDb.CustomerBirthday = Customer.CustomerBirthday;
                    _context.SaveChanges();
                }
            }
            MessageBox.Show("Cập nhật thông tin thành công","Thông báo",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void Loaded(object obj)
        {
            using (var _context = new FuminiHotelManagementContext())
            {
                var bookingDetails = (from bd in _context.BookingDetails
                                      join br in _context.BookingReservations on bd.BookingReservationId equals br.BookingReservationId
                                      join ri in _context.RoomInformations on bd.RoomId equals ri.RoomId
                                      join rt in _context.RoomTypes on ri.RoomTypeId equals rt.RoomTypeId
                                      where br.CustomerId == Customer.CustomerId
                                      select new BookingDetailView
                                      {
                                          BookingDate = br.BookingDate,
                                          StartDate = bd.StartDate,
                                          EndDate = bd.EndDate,
                                          ActualPrice = bd.ActualPrice,
                                          TotalPrice = br.TotalPrice,
                                          RoomName = ri.RoomNumber,
                                          RoomType = rt.RoomTypeName
                                      }).ToList();
                GridItemSource = new ObservableCollection<BookingDetailView>(bookingDetails);
            }
        }
    }
}
