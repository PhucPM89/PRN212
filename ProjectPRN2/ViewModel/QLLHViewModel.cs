using ClosedXML.Excel;
using Microsoft.Win32;
using ProjectPRN2.Models;
using ProjectPRN2.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace ProjectPRN2.ViewModel
{
    public class QLLHViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(object propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName.ToString()));
        }

        private TrangThaiForm _trangThaiForm;
        private CustomCommand _LoadedCommand;
        public CustomCommand LoadedCommand
        {
            get { return _LoadedCommand; }
            set { _LoadedCommand = value; }
        }

        private CustomCommand _barAddCommand;

        public CustomCommand barAddCommand
        {
            get { return _barAddCommand; }
            set { _barAddCommand = value; }
        }

        private CustomCommand _barEditCommand;

        public CustomCommand barEditCommand
        {
            get { return _barEditCommand; }
            set { _barEditCommand = value; }
        }

        private CustomCommand _barDeleteCommand;

        public CustomCommand barDeleteCommand
        {
            get { return _barDeleteCommand; }
            set { _barDeleteCommand = value; }
        }

        private CustomCommand _barExportExcelCommand;

        public CustomCommand barExportExcelCommand
        {
            get { return _barExportExcelCommand; }
            set { _barExportExcelCommand = value; }
        }

        private CustomCommand _btnKhongLuuCommand;

        public CustomCommand btnKhongLuuCommand
        {
            get { return _btnKhongLuuCommand; }
            set { _btnKhongLuuCommand = value; }
        }

        private CustomCommand _btnLuuCommand;

        public CustomCommand btnLuuCommand
        {
            get { return _btnLuuCommand; }
            set { _btnLuuCommand = value; }
        }

        private ObservableCollection<LopHoc> _gridItemSource;

        public ObservableCollection<LopHoc> GridItemSource
        {
            get { return _gridItemSource; }
            set
            {
                _gridItemSource = value;
                OnPropertyChanged("GridItemSource");
            }
        }

        private LopHoc _selectDataRow;

        public LopHoc SelectDataRow
        {
            get { return _selectDataRow; }
            set
            {
                _selectDataRow = value;
                OnPropertyChanged("SelectDataRow");
                if (_selectDataRow != null)
                {
                    //Load dữ liệu từ lưới lên 3 control
                    txtLopHoc = _selectDataRow.TenLopHoc;
                    txtSoLuongHocSinh = Convert.ToInt32(_selectDataRow.SlhocSinh);
                    txtGVCN = _selectDataRow.Gvcn;
                }
            }
        }


        private string _sLopHoc = "";

        public string txtLopHoc
        {
            get { return _sLopHoc; }
            set
            {
                _sLopHoc = value;
                OnPropertyChanged("txtLopHoc");
            }
        }

        private int _iSoLuongHocSinh = 0;

        public int txtSoLuongHocSinh
        {
            get { return _iSoLuongHocSinh; }
            set
            {
                _iSoLuongHocSinh = value;
                OnPropertyChanged("txtSoLuongHocSinh");
            }
        }

        private string _sGVCN = "";

        public string txtGVCN
        {
            get { return _sGVCN; }
            set
            {
                _sGVCN = value;
                OnPropertyChanged("txtGVCN");
            }
        }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                ApplyFilter();
            }
        }

        MVVMContext _context;
        public QLLHViewModel()
        {
            //Tham số thứ 2 sẽ quy định xem hàm trong tham số thứ 1 có được thực thi hay không
            _LoadedCommand = new CustomCommand(Loaded, (p) => true);
            _barAddCommand = new CustomCommand(barAdd_Click, (p) => true);
            _barEditCommand = new CustomCommand(barEdit_Click, (p) => true);
            _barDeleteCommand = new CustomCommand(barDelete_Click,(p) => true);
            _barExportExcelCommand = new CustomCommand(barExportExcel_Click,(p) => true);
            _btnKhongLuuCommand = new CustomCommand(btnKhongLuu_Click, (p) => true);
            _btnLuuCommand = new CustomCommand(btnLuu_Click, (p) => true);
        }


        private void btnLuu_Click(object obj)
        {
            try
            {
                string sID = "";
                LopHoc lopHoc = null;
                if (((ProjectPRN2.View.QLLH)(obj)).txtLopHoc.Text.Trim() == "" || ((ProjectPRN2.View.QLLH)(obj)).txtGVCN.Text.Trim() == "")
                {
                    MessageBox.Show("Bạn cần nhập đẩy đủ thông tin!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_trangThaiForm == TrangThaiForm.TrangThaiThem)
                {
                    lopHoc = new LopHoc();
                    string sIdlopHoc = "ID_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss").Replace("/", "").Replace(":", "").Replace(" ", "");
                    lopHoc.IdlopHoc = sID = sIdlopHoc;
                    lopHoc.TenLopHoc = _sLopHoc;
                    lopHoc.SlhocSinh = _iSoLuongHocSinh;
                    lopHoc.Gvcn = _sGVCN;
                    _context = new MVVMContext();
                    _context.Insert(lopHoc);
                }
                else if (_trangThaiForm == TrangThaiForm.TrangThaiSua)
                {
                    _context = new MVVMContext();
                    lopHoc = new LopHoc();
                    lopHoc.IdlopHoc = sID = _selectDataRow.IdlopHoc;
                    lopHoc.TenLopHoc = _sLopHoc;
                    lopHoc.SlhocSinh = Convert.ToInt32(_iSoLuongHocSinh);
                    lopHoc.Gvcn = _sGVCN;
                    _context.Update(lopHoc);
                }

                MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                //Lưu thành công thì form sẽ chuyển về trạng thái Xem
                _trangThaiForm = TrangThaiForm.TrangThaiXem;
                //Load lại lưới
                LoadDataGridControl();
                // Focus vào row vừa sửa
                FocusRow(lopHoc);
                SetLoadForm(obj);
                setMenuLoadForm(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lưu thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnKhongLuu_Click(object obj)
        {
            if (((ProjectPRN2.View.QLLH)(obj)).txtSoLuongHocSinh.Text.Trim() != "")
            {
                if (MessageBox.Show("Bạn có muốn lưu dữ liệu này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //Không lưu thì trở về trạng thái xem
                    //Khi nhấn button Không Lưu thì form sẽ trở về trạng thái xem
                    _trangThaiForm = TrangThaiForm.TrangThaiXem;
                    if (((ProjectPRN2.View.QLLH)(obj)).gridControl.Items.Count <= 0)
                    {
                        ResetText(obj);
                    }
                    SetLoadForm(obj);
                    setMenuLoadForm(obj);
                }
                else
                {
                    //Chạy hàm save của butotn Lưu
                    btnLuu_Click(obj);
                }
            }
        }

        private void barAdd_Click(object obj)
        {
            _trangThaiForm = TrangThaiForm.TrangThaiThem;
            ResetText(obj);
            SetLoadForm(obj);
            DefaultValue(obj);
        }

        private void barEdit_Click(object obj)
        {
            if (_selectDataRow == null || String.IsNullOrEmpty(_selectDataRow.IdlopHoc))
            {
                MessageBox.Show("Bạn chưa lựa chọn dữ liệu cần sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _trangThaiForm = TrangThaiForm.TrangThaiSua;
            SetLoadForm(obj);
        }

        private void barDelete_Click(object obj)
        {
            if (_selectDataRow == null || String.IsNullOrEmpty(_selectDataRow.IdlopHoc))
            {
                MessageBox.Show("Bạn chưa lựa chọn dữ liệu cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa lớp: " + _selectDataRow.TenLopHoc + "?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No){
                return;
            }
            int index = _gridItemSource.IndexOf(_selectDataRow);
            _context = new MVVMContext();
            _context.Delete(_selectDataRow);
            MessageBox.Show("Xóa thành công","Thông báo",MessageBoxButton.OK, MessageBoxImage.Information);
            LoadDataGridControl();
            setMenuLoadForm(obj);
            // khi xóa xong thì focus vào dòng tiếp theo sau dòng muốn xóa
            if(GridItemSource.Count == 0)
            {
                _selectDataRow = null;
                ResetText(obj);
            }
            else
            {
                _selectDataRow = _gridItemSource[index];
            }
            FocusRow(_selectDataRow);
        }

        private void barExportExcel_Click(object obj)
        {
            try
            {
                var mainWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(window => window.GetType() == typeof(ProjectPRN2.View.QLLH));
                if (mainWindow != null)
                {
                    var dataGrid = mainWindow.FindName("gridControl") as DataGrid;
                    if (dataGrid != null)
                    {
                        ExportDataGridToExcel(dataGrid);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xuất excel thất bại! " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExportDataGridToExcel(DataGrid dataGrid)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FilterIndex = 2,
                    RestoreDirectory = true,
                    FileName = "ExportedData"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("DataGrid Data");

                        int columnIndex = 1;

                        // Add DataGrid headers
                        for (int i = 0; i < dataGrid.Columns.Count; i++)
                        {
                            if (dataGrid.Columns[i].Visibility == Visibility.Visible)
                            {
                                worksheet.Cell(1, columnIndex).Value = dataGrid.Columns[i].Header.ToString();
                                columnIndex++;
                            }
                        }

                        // Add DataGrid rows
                        for (int i = 0; i < dataGrid.Items.Count; i++)
                        {
                            columnIndex = 1;
                            for (int j = 0; j < dataGrid.Columns.Count; j++)
                            {
                                if (dataGrid.Columns[j].Visibility == Visibility.Visible)
                                {
                                    var cellContent = dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) as TextBlock;
                                    worksheet.Cell(i + 2, columnIndex).Value = cellContent != null ? cellContent.Text : "";
                                    columnIndex++;
                                }
                            }
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show("Xuất excel thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xuất excel thất bại! " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Loaded(object obj)
        {
            //Set trang thái ban đầu
            _trangThaiForm = TrangThaiForm.TrangThaiXem;
            //Load dữ liệu cho lưới
            LoadDataGridControl();
            //Khi vừa mở lên thì form ở trạng thái Xem, lúc này các control sẽ lock
            SetLoadForm(obj);
            setMenuLoadForm(obj);
        }

        private void LoadDataGridControl()
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



        private void setMenuLoadForm(object obj)
        {
            if (((ProjectPRN2.View.QLLH)(obj)).gridControl.Items.Count <= 0)
            {
                //Menu thêm mới sẽ luôn sáng
                //Nếu lưới không có dữ liệu thì 3 menu sẽ mờ
                ((ProjectPRN2.View.QLLH)(obj)).barEdit.IsEnabled = false;
                ((ProjectPRN2.View.QLLH)(obj)).barDelete.IsEnabled = false;
                ((ProjectPRN2.View.QLLH)(obj)).barExportExcel.IsEnabled = false;
            }
            else
            {
                ((ProjectPRN2.View.QLLH)(obj)).barEdit.IsEnabled = true;
                ((ProjectPRN2.View.QLLH)(obj)).barDelete.IsEnabled = true;
                ((ProjectPRN2.View.QLLH)(obj)).barExportExcel.IsEnabled = true;

            }
        }

        private void SetLoadForm(object obj)
        {
            switch (_trangThaiForm)
            {
                case TrangThaiForm.TrangThaiXem:
                    ((ProjectPRN2.View.QLLH)(obj)).txtLopHoc.IsReadOnly = true;
                    ((ProjectPRN2.View.QLLH)(obj)).txtSoLuongHocSinh.IsReadOnly = true;
                    ((ProjectPRN2.View.QLLH)(obj)).txtGVCN.IsReadOnly = true;
                    //Mở khóa lưới    
                    ((ProjectPRN2.View.QLLH)(obj)).gridControl.IsEnabled = true;
                    //Mờ nút button 
                    ((ProjectPRN2.View.QLLH)(obj)).btnLuu.IsEnabled = false;
                    ((ProjectPRN2.View.QLLH)(obj)).btnKhongLuu.IsEnabled = false;
                    break;

                case TrangThaiForm.TrangThaiThem:
                    //Khi nhấn vào menu Thêm thì nó sẽ mở khóa các control để nhập liệu + sáng 2 button Lưu + không Lưu + khóa lưới
                    ((ProjectPRN2.View.QLLH)(obj)).txtLopHoc.IsReadOnly = false;
                    ((ProjectPRN2.View.QLLH)(obj)).txtSoLuongHocSinh.IsReadOnly = false;
                    ((ProjectPRN2.View.QLLH)(obj)).txtGVCN.IsReadOnly = false;
                    //Khóa lưới    
                    ((ProjectPRN2.View.QLLH)(obj)).gridControl.IsEnabled = false;
                    //Sáng button 
                    ((ProjectPRN2.View.QLLH)(obj)).btnLuu.IsEnabled = true;
                    ((ProjectPRN2.View.QLLH)(obj)).btnKhongLuu.IsEnabled = true;
                    //focus vào control đầu tiên
                    ((ProjectPRN2.View.QLLH)(obj)).txtLopHoc.Focus();
                    break;

                case TrangThaiForm.TrangThaiSua:
                    //Khi nhấn vào menu Thêm thì nó sẽ mở khóa các control để nhập liệu + sáng 2 button Lưu + không Lưu + khóa lưới
                    ((ProjectPRN2.View.QLLH)(obj)).txtLopHoc.IsReadOnly = false;
                    ((ProjectPRN2.View.QLLH)(obj)).txtSoLuongHocSinh.IsReadOnly = false;
                    ((ProjectPRN2.View.QLLH)(obj)).txtGVCN.IsReadOnly = false;
                    //Khóa lưới    
                    ((ProjectPRN2.View.QLLH)(obj)).gridControl.IsEnabled = false;
                    //Sáng button 
                    ((ProjectPRN2.View.QLLH)(obj)).btnLuu.IsEnabled = true;
                    ((ProjectPRN2.View.QLLH)(obj)).btnKhongLuu.IsEnabled = true;
                    //focus vào control đầu tiên
                    ((ProjectPRN2.View.QLLH)(obj)).txtLopHoc.Focus();
                    ((ProjectPRN2.View.QLLH)(obj)).txtLopHoc.SelectAll();
                    break;
            }
        }

        private void ResetText(object obj)
        {
            ((ProjectPRN2.View.QLLH)(obj)).txtLopHoc.Text = String.Empty;
            ((ProjectPRN2.View.QLLH)(obj)).txtSoLuongHocSinh.Text = String.Empty;
            ((ProjectPRN2.View.QLLH)(obj)).txtGVCN.Text = String.Empty;
        }

        private void DefaultValue(object obj)
        {
            //Khởi tạo giá trị mặc định cho control "Số lượng học sinh"
            ((ProjectPRN2.View.QLLH)(obj)).txtSoLuongHocSinh.Text = "0";
        }

        private void FocusRow(LopHoc lopHoc)
        {
            var mainWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(window => window.GetType() == typeof(ProjectPRN2.View.QLLH));
            if (mainWindow != null)
            {
                var dataGrid = mainWindow.FindName("gridControl") as DataGrid;
                if (dataGrid != null && lopHoc != null && dataGrid.Items.Contains(lopHoc))
                {
                    dataGrid.UpdateLayout();
                    dataGrid.ScrollIntoView(lopHoc);
                    dataGrid.SelectedItem = lopHoc;
                }
            }
        }


        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
            {
                GridItemSource = new ObservableCollection<LopHoc>(_context.GetList());
            }
            else
            {
                GridItemSource = new ObservableCollection<LopHoc>(_context.GetList().Where(lh => lh.TenLopHoc.Contains(_searchText, StringComparison.OrdinalIgnoreCase) || lh.Gvcn.Contains(_searchText, StringComparison.OrdinalIgnoreCase)).ToList());
            }
        }

        private enum TrangThaiForm
        {
            TrangThaiXem = 0,
            TrangThaiThem = 1,
            TrangThaiSua = 2
        }
    }
}
