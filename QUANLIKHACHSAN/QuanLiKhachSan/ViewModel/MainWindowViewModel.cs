using QuanLiKhachSan.Models;
using QuanLiKhachSan.Support;
using QuanLiKhachSan.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKhachSan.ViewModel
{
    class MainWindowViewModel
    {
        private CustomCommand _ShowQLKHCommand;
        public CustomCommand ShowQLKHCommand
        {
            get { return _ShowQLKHCommand; }
            set { _ShowQLKHCommand = value; }
        }

        private CustomCommand _ShowQLPCommand;
        public CustomCommand ShowQLPCommand
        {
            get { return _ShowQLPCommand; }
            set { _ShowQLPCommand = value; }
        }

        private CustomCommand _ShowQLDPCommand;
        public CustomCommand ShowQLDPCommand
        {
            get { return _ShowQLDPCommand; }
            set { _ShowQLDPCommand = value; }
        }

        private CustomCommand _ShowBCCommand;
        public CustomCommand ShowBCCommand
        {
            get { return _ShowBCCommand; }
            set { _ShowBCCommand = value; }
        }

        public MainWindowViewModel() {
            _ShowQLKHCommand = new CustomCommand(ShowQLKH_Click, (p)=> true);
            _ShowQLPCommand = new CustomCommand(ShowQLP_Click,(p)=>true);
            _ShowQLDPCommand = new CustomCommand(ShowQDP_Click,(p)=>true);
            _ShowBCCommand = new CustomCommand(ShowBC_Click,(p)=>true);
        }

        private void ShowBC_Click(object obj)
        {
            BC bC = new BC();
            bC.ShowDialog();
        }

        private void ShowQDP_Click(object obj)
        {
            QLDP qLDP = new QLDP();
            qLDP.ShowDialog();
        }

        private void ShowQLP_Click(object obj)
        {
           RoomInfomationWindow roomInfomationWindow = new RoomInfomationWindow();
           roomInfomationWindow.ShowDialog();
        }

        private void ShowQLKH_Click(object obj)
        {
            QLKH qLKH = new QLKH();
            qLKH.ShowDialog();
        }
    }
}
