using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QuanLiKhachSan.Models
{
    class MVVMContext
    {
        FuminiHotelManagementContext _entities;

        internal ObservableCollection<Customer> GetList()
        {
            _entities = new FuminiHotelManagementContext();
            return new ObservableCollection<Customer>(_entities.Customers);
        }

        internal ObservableCollection<RoomInformation> GetListRoom()
        {
            _entities = new FuminiHotelManagementContext();
            return new ObservableCollection<RoomInformation>(_entities.RoomInformations);
        }

        internal ObservableCollection<RoomType> GetListRoomType()
        {
            _entities = new FuminiHotelManagementContext();
            return new ObservableCollection<RoomType>(_entities.RoomTypes);
        }

        internal ObservableCollection<RoomInformation> GetListRoomInformation()
        {
            using (var _entities = new FuminiHotelManagementContext())
            {
                var roomInformations = _entities.RoomInformations
                                                .Include(ri => ri.RoomType)
                                                .ToList();
                return new ObservableCollection<RoomInformation>(roomInformations);
            }
        }



        internal void Insert(Customer customer)
        {
            _entities = new FuminiHotelManagementContext();
            _entities.Customers.Add(customer);
            _entities.SaveChanges();
        }

        internal void InsertRoomType(RoomType roomType)
        {
            _entities = new FuminiHotelManagementContext();
            _entities.RoomTypes.Add(roomType);
            _entities.SaveChanges();
        }

        internal void InsertRoomInformation(RoomInformation roomInformation)
        {
            _entities = new FuminiHotelManagementContext();
            _entities.RoomInformations.Add(roomInformation);
            _entities.SaveChanges();
        }

        internal void Update(Customer customer)
        {
            _entities = new FuminiHotelManagementContext();
            if(customer.CustomerId != null) {
                Customer c = _entities.Customers.First(p =>  p.CustomerId == customer.CustomerId);
                if (c!=null)
                {
                    c.CustomerId = customer.CustomerId;
                    c.CustomerFullName = customer.CustomerFullName;
                    c.Telephone = customer.Telephone;
                    c.CustomerStatus = customer.CustomerStatus;
                    c.CustomerBirthday = customer.CustomerBirthday;
                    c.EmailAddress = customer.EmailAddress;
                    _entities.SaveChanges();
                }
            }
        }

        internal void UpdateRoomType(RoomType roomType)
        {
            _entities = new FuminiHotelManagementContext();
            if (roomType.RoomTypeId != null)
            {
                RoomType c = _entities.RoomTypes.First(p => p.RoomTypeId == roomType.RoomTypeId);
                if (c != null)
                {
                    c.RoomTypeId = roomType.RoomTypeId;
                    c.RoomTypeName = roomType.RoomTypeName;
                    c.TypeDescription = roomType.TypeDescription;
                    c.TypeNote = roomType.TypeNote;
                    _entities.SaveChanges();
                }
            }
        }

        internal void UpdateRoomInformation(RoomInformation roomInformation)
        {
            _entities = new FuminiHotelManagementContext();
            var existingRoomInformation = _entities.RoomInformations.FirstOrDefault(p => p.RoomId == roomInformation.RoomId);
            if (existingRoomInformation != null)
            {
                existingRoomInformation.RoomNumber = roomInformation.RoomNumber;
                existingRoomInformation.RoomDetailDescription = roomInformation.RoomDetailDescription;
                existingRoomInformation.RoomMaxCapacity = roomInformation.RoomMaxCapacity;
                existingRoomInformation.RoomTypeId = roomInformation.RoomTypeId;
                existingRoomInformation.RoomStatus = roomInformation.RoomStatus;
                existingRoomInformation.RoomPricePerDay = roomInformation.RoomPricePerDay;
                _entities.SaveChanges();
            }
        }

        internal void Delete(Customer customer)
        {
            _entities = new FuminiHotelManagementContext();
            _entities.Remove(customer);
            _entities.SaveChanges();
        }

        internal void DeleteRoomType(RoomType roomType)
        {
            _entities = new FuminiHotelManagementContext();
            _entities.Remove(roomType);
            _entities.SaveChanges();
        }

        internal void DeleteRoomInformation(RoomInformation roomInformation)
        {
            _entities = new FuminiHotelManagementContext();
            _entities.RoomInformations.Remove(roomInformation);
            _entities.SaveChanges();
        }

        internal ObservableCollection<RoomInformation> GetRoomInformationsByRoomTypeId(int roomTypeId)
        {
            _entities = new FuminiHotelManagementContext();
            return new ObservableCollection<RoomInformation>(_entities.RoomInformations.Where(ri => ri.RoomTypeId == roomTypeId));
        }

        internal bool checkLogin(string username, string password)
        {
            _entities = new FuminiHotelManagementContext();
            var account = _entities.Customers.Where(x => x.EmailAddress == username && x.Password == password).Count();
            if(account > 0)
            {
                return true;
            }
            return false;
        }

        internal bool IsEmailRegistered(string email) {
            _entities = new FuminiHotelManagementContext();
            return _entities.Customers.Any(c => c.EmailAddress == email);
        }

        internal void UpdatePassword(string email, string newPassword)
        {
            _entities = new FuminiHotelManagementContext();
            var customer = _entities.Customers.FirstOrDefault(c => c.EmailAddress == email);
            if (customer != null)
            {
                customer.Password = newPassword;
                _entities.SaveChanges();
            }
        }

        internal bool DoesRoomNumberExist(string roomNumber)
        {
            if (_entities == null)
            {
                _entities = new FuminiHotelManagementContext();
            }

            return _entities.RoomInformations.Any(r => r.RoomNumber == roomNumber);
        }

        internal bool HasBookingDetails(int roomId)
        {
            using (var _entities = new FuminiHotelManagementContext())
            {
                return _entities.BookingDetails.Any(bd => bd.RoomId == roomId);
            }
        }

        internal Customer GetCustomerByEmail(string email)
        {
            using(var _entities = new FuminiHotelManagementContext())
            {
                return _entities.Customers.SingleOrDefault(c => c.EmailAddress == email);
            }
        }
    }
}
