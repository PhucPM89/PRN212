using Newtonsoft.Json;
using QuanLiKhachSan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKhachSan.Support
{
    public class AdminAccountService
    {
        private const string FilePath = "C:\\Users\\minhp\\OneDrive\\Desktop\\C#\\QUANLIKHACHSAN\\QuanLiKhachSan\\appsettings.json";

        public AdminAccount GetAdminAccount()
        {
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("Admin account file not found.");
            }

            string json = File.ReadAllText(FilePath);
            var adminAccountWrapper = JsonConvert.DeserializeObject<AdminAccountWrapper>(json);
            return adminAccountWrapper.AdminAccount;
        }
    }

    public class AdminAccountWrapper
    {
        public AdminAccount AdminAccount { get; set; }
    }

    public class AdminAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
