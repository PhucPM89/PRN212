using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKhachSan.Models
{
    internal class BookingDetailView
    {
        public DateOnly? BookingDate { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal? ActualPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public string RoomName { get; set; }
        public string RoomType { get; set; }
    }
}
