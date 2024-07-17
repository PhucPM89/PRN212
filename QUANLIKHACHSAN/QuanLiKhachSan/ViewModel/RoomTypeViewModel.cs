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
    internal class RoomTypeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        MVVMContext _context;

        private ObservableCollection<RoomType> _gridItemSource;

        public ObservableCollection<RoomType> GridItemSource
        {
            get { return _gridItemSource; }
            set
            {
                _gridItemSource = value;
                OnPropertyChanged("GridItemSource");
            }
        }


        private CustomCommand _LoadedCommand;

        public CustomCommand LoadedCommand
        {
            get { return _LoadedCommand; }
            set { _LoadedCommand = value; }
        }

        private CustomCommand _AddRoomTypeCommand;

        public CustomCommand AddRoomTypeCommand
        {
            get { return _AddRoomTypeCommand; }
            set { _AddRoomTypeCommand = value; }
        }

        private CustomCommand _EditRoomTypeCommand;

        public CustomCommand EditRoomTypeCommand
        {
            get { return _EditRoomTypeCommand; }
            set { _EditRoomTypeCommand = value; }
        }

        private CustomCommand _DeleteRoomTypeCommand;

        public CustomCommand DeleteRoomTypeCommand
        {
            get { return _DeleteRoomTypeCommand; }
            set { _DeleteRoomTypeCommand = value; }
        }

        private int _RoomTypeTextBox;

        public int RoomTypeTextBox
        {
            get { return _RoomTypeTextBox; }
            set
            {
                _RoomTypeTextBox = value;
                OnPropertyChanged("RoomTypeTextBox");
            }
        }

        private string _RoomTypeNameTextBox;

        public string RoomTypeNameTextBox
        {
            get { return _RoomTypeNameTextBox; }
            set
            {
                _RoomTypeNameTextBox = value;
                OnPropertyChanged("RoomTypeNameTextBox");
            }
        }

        private string _TypeDescriptionTextBox;

        public string TypeDescriptionTextBox
        {
            get { return _TypeDescriptionTextBox; }
            set
            {
                _TypeDescriptionTextBox = value;
                OnPropertyChanged("TypeDescriptionTextBox");
            }
        }

        private string _NoteTextBox;

        public string NoteTextBox
        {
            get { return _NoteTextBox; }
            set
            {
                _NoteTextBox = value;
                OnPropertyChanged("NoteTextBox");
            }
        }

        private RoomType _selectDataRow;

        public RoomType SelectDataRow
        {
            get { return _selectDataRow; }
            set
            {
                _selectDataRow = value;
                OnPropertyChanged("SelectDataRow");
                if (_selectDataRow != null)
                {

                    RoomTypeTextBox = _selectDataRow.RoomTypeId;
                    RoomTypeNameTextBox = _selectDataRow.RoomTypeName;
                    TypeDescriptionTextBox = _selectDataRow.TypeDescription;
                    NoteTextBox = _selectDataRow.TypeNote;
                }
            }
        }

        public RoomTypeViewModel()
        {
            _LoadedCommand = new CustomCommand(Loaded, (p) => true);
            _AddRoomTypeCommand = new CustomCommand(btnAdd_Click, (p) => true);
            _EditRoomTypeCommand = new CustomCommand(btnEdit_Click, (p) => true);
            _DeleteRoomTypeCommand = new CustomCommand(btnDelete_Click, (p) => true);
        }

        private void btnDelete_Click(object obj)
        {
            if (_selectDataRow == null)
            {
                MessageBox.Show("Bạn chưa chọn kiểu phòng muốn xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _context = new MVVMContext();
            var roomInformations = _context.GetRoomInformationsByRoomTypeId(_selectDataRow.RoomTypeId);

            if (roomInformations.Any())
            {
                MessageBox.Show("Không thể xóa kiểu phòng này vì đã tồn tại thông tin phòng liên quan.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Bạn có muốn xóa kiểu phòng này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _context = new MVVMContext();
                _context.DeleteRoomType(_selectDataRow);
                Loaded(obj);
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void btnEdit_Click(object obj)
        {
            try
            {
                if (SelectDataRow == null)
                {
                    MessageBox.Show("Bạn chưa chọn kiểu phòng muốn sửa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MessageBox.Show("Bạn có muốn cập nhật thông tin kiểu phòng này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    RoomType roomType = new RoomType();
                    roomType.RoomTypeId = _selectDataRow.RoomTypeId;
                    roomType.RoomTypeName = _RoomTypeNameTextBox;
                    roomType.TypeDescription = _TypeDescriptionTextBox;
                    roomType.TypeNote = _NoteTextBox;
                    _context = new MVVMContext();
                    _context.UpdateRoomType(roomType);
                    Loaded(obj);
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btnAdd_Click(object obj)
        {
            if (String.IsNullOrEmpty(_RoomTypeNameTextBox))
            {
                MessageBox.Show("Bạn chưa nhập tên kiểu phòng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (String.IsNullOrEmpty(_TypeDescriptionTextBox))
            {
                MessageBox.Show("Bạn chưa nhập mô tả", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (String.IsNullOrEmpty(_NoteTextBox))
            {
                MessageBox.Show("Bạn chưa nhập ghi chú", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có muốn thêm kiểu phòng này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RoomType roomType = new RoomType();
                roomType.RoomTypeName = _RoomTypeNameTextBox;
                roomType.TypeDescription = _TypeDescriptionTextBox;
                roomType.TypeNote = _NoteTextBox;

                _context = new MVVMContext();
                _context.InsertRoomType(roomType);
                Loaded(obj);
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                return;
            }

        }

        private void Loaded(object obj)
        {
            try
            {
                _context = new MVVMContext();
                GridItemSource = _context.GetListRoomType();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
