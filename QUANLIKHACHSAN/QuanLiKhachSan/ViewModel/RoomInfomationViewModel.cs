using Microsoft.IdentityModel.Tokens;
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
    internal class RoomInfomationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        MVVMContext _context;

        private ObservableCollection<RoomInformation> _gridItemSource;
        public ObservableCollection<RoomInformation> GridItemSource
        {
            get { return _gridItemSource; }
            set
            {
                _gridItemSource = value;
                OnPropertyChanged(nameof(GridItemSource));
            }
        }

        private CustomCommand _LoadedCommand;
        public CustomCommand LoadedCommand
        {
            get { return _LoadedCommand; }
            set { _LoadedCommand = value; }
        }

        private RoomInformation _selectDataRow;
        public RoomInformation SelectDataRow
        {
            get { return _selectDataRow; }
            set
            {
                _selectDataRow = value;
                OnPropertyChanged(nameof(SelectDataRow));
                if (_selectDataRow != null)
                {
                    RoomNumberTextBox = _selectDataRow.RoomNumber;
                    RoomDetailDescriptionTextBox = _selectDataRow.RoomDetailDescription;
                    RoomMaxCapacityTextBox = (int)_selectDataRow.RoomMaxCapacity;
                    SelectedRoomType = _selectDataRow.RoomType;
                    RoomStatusTextBox = _selectDataRow.RoomStatus.ToString();
                    RoomPricePerDayTextBox = _selectDataRow.RoomPricePerDay.ToString();
                }
            }
        }

        private string _roomNumberTextBox;
        public string RoomNumberTextBox
        {
            get { return _roomNumberTextBox; }
            set
            {
                _roomNumberTextBox = value;
                OnPropertyChanged(nameof(RoomNumberTextBox));
            }
        }

        private string _roomDetailDescriptionTextBox;
        public string RoomDetailDescriptionTextBox
        {
            get { return _roomDetailDescriptionTextBox; }
            set
            {
                _roomDetailDescriptionTextBox = value;
                OnPropertyChanged(nameof(RoomDetailDescriptionTextBox));
            }
        }

        private int _roomMaxCapacityTextBox;
        public int RoomMaxCapacityTextBox
        {
            get { return _roomMaxCapacityTextBox; }
            set
            {
                _roomMaxCapacityTextBox = value;
                OnPropertyChanged(nameof(RoomMaxCapacityTextBox));
            }
        }

        private RoomType _selectedRoomType;
        public RoomType SelectedRoomType
        {
            get { return _selectedRoomType; }
            set
            {
                _selectedRoomType = value;
                OnPropertyChanged(nameof(SelectedRoomType));
                if (_selectedRoomType != null && SelectDataRow != null)
                {
                    SelectDataRow.RoomType = _selectedRoomType;
                }
            }
        }

        private ObservableCollection<RoomType> _roomTypes;
        public ObservableCollection<RoomType> RoomTypes
        {
            get { return _roomTypes; }
            set
            {
                _roomTypes = value;
                OnPropertyChanged(nameof(RoomTypes));
            }
        }

        private string _roomStatusTextBox;
        public string RoomStatusTextBox
        {
            get { return _roomStatusTextBox; }
            set
            {
                _roomStatusTextBox = value;
                OnPropertyChanged(nameof(RoomStatusTextBox));
            }
        }

        private string _roomPricePerDayTextBox;
        public string RoomPricePerDayTextBox
        {
            get { return _roomPricePerDayTextBox; }
            set
            {
                _roomPricePerDayTextBox = value;
                OnPropertyChanged(nameof(RoomPricePerDayTextBox));
            }
        }

        public CustomCommand AddRoomCommand { get; set; }
        public CustomCommand EditRoomCommand { get; set; }
        public CustomCommand DeleteRoomCommand { get; set; }

        public RoomInfomationViewModel()
        {
            _LoadedCommand = new CustomCommand(Loaded, (p) => true);
            AddRoomCommand = new CustomCommand(AddRoom, (p) => true);
            EditRoomCommand = new CustomCommand(EditRoom, (p) => true);
            DeleteRoomCommand = new CustomCommand(DeleteRoom, (p) => true);
        }

        private void DeleteRoom(object obj)
        {
            if (SelectDataRow == null)
            {
                MessageBox.Show("Vui lòng chọn phòng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa thông tin phòng này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _context = new MVVMContext();

                if (_context.HasBookingDetails(SelectDataRow.RoomId))
                {
                    SelectDataRow.RoomStatus = 0; 
                    _context.UpdateRoomInformation(SelectDataRow);
                    MessageBox.Show("Phòng đã được chuyển sang trạng thái không hoạt động vì phòng đang được thuê", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _context.DeleteRoomInformation(SelectDataRow);
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                Loaded(obj);
            }
        }

        private void EditRoom(object obj)
        {
            // Implementation of EditRoom method
            if (SelectDataRow == null)
            {
                MessageBox.Show("Vui lòng chọn phòng để sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(RoomNumberTextBox) || String.IsNullOrEmpty(RoomDetailDescriptionTextBox) || RoomMaxCapacityTextBox.ToString() == "" || SelectedRoomType == null || String.IsNullOrEmpty(RoomStatusTextBox) || String.IsNullOrEmpty(RoomPricePerDayTextBox))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin phòng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Bạn có muốn cập nhật thông tin phòng này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SelectDataRow.RoomNumber = RoomNumberTextBox;
                SelectDataRow.RoomDetailDescription = RoomDetailDescriptionTextBox;
                SelectDataRow.RoomMaxCapacity = RoomMaxCapacityTextBox;
                SelectDataRow.RoomTypeId = SelectedRoomType.RoomTypeId;
                SelectDataRow.RoomStatus = byte.Parse(RoomStatusTextBox);
                SelectDataRow.RoomPricePerDay = decimal.Parse(RoomPricePerDayTextBox);

                _context = new MVVMContext();
                _context.UpdateRoomInformation(SelectDataRow);
                Loaded(obj);
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddRoom(object obj)
        {
            if (String.IsNullOrEmpty(RoomNumberTextBox))
            {
                MessageBox.Show("Bạn chưa nhập tên phòng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(RoomDetailDescriptionTextBox))
            {
                MessageBox.Show("Bạn chưa nhập mô tả!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (RoomMaxCapacityTextBox == 0)
            {
                MessageBox.Show("Bạn chưa nhập số lượng người tối đa trong 1 phòng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedRoomType == null || String.IsNullOrEmpty(SelectedRoomType.RoomTypeName))
            {
                MessageBox.Show("Bạn chưa chọn tên kiểu phòng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(RoomStatusTextBox))
            {
                MessageBox.Show("Bạn chưa nhập trạng thái phòng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrEmpty(RoomPricePerDayTextBox))
            {
                MessageBox.Show("Bạn chưa nhập giá phòng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_context.DoesRoomNumberExist(RoomNumberTextBox))
            {
                MessageBox.Show("Số phòng đã tồn tại. Vui lòng chọn số phòng khác.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Bạn có muốn thêm thông tin phòng này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RoomInformation roomInformation = new RoomInformation
                {
                    RoomNumber = RoomNumberTextBox,
                    RoomDetailDescription = RoomDetailDescriptionTextBox,
                    RoomMaxCapacity = RoomMaxCapacityTextBox,
                    RoomTypeId = SelectedRoomType.RoomTypeId,
                    RoomStatus = byte.Parse(RoomStatusTextBox),
                    RoomPricePerDay = decimal.Parse(RoomPricePerDayTextBox)
                };

                _context = new MVVMContext();
                _context.InsertRoomInformation(roomInformation);
                Loaded(obj);
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Loaded(object obj)
        {
            _context = new MVVMContext();
            GridItemSource = _context.GetListRoomInformation();
            RoomTypes = _context.GetListRoomType();
        }
    }
}
