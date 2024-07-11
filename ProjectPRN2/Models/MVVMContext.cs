using ProjectPRN2.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPRN2.Models
{
    public class MVVMContext
    {
        QuanlylophocContext _entities;

        internal ObservableCollection<LopHoc> GetList()
        {
            _entities = new QuanlylophocContext();
            return new ObservableCollection<LopHoc>(_entities.LopHocs);
        }

        #region QL Lớp Học
        internal void Insert(LopHoc lopHoc)
        {
            _entities = new QuanlylophocContext();
            _entities.LopHocs.Add(lopHoc);
            _entities.SaveChanges();
        }
        #endregion

        internal void Update(LopHoc lopHoc)
        {
            _entities = new QuanlylophocContext();
            if(lopHoc.IdlopHoc != null)
            {
                LopHoc lh = _entities.LopHocs.First(p => p.IdlopHoc == lopHoc.IdlopHoc);
                if(lh != null)
                {
                    lh.TenLopHoc = lopHoc.TenLopHoc;
                    lh.SlhocSinh = lopHoc.SlhocSinh;
                    lh.Gvcn = lopHoc.Gvcn;
                }
                else
                {
                    _entities.LopHocs.Add(lopHoc);
                }
            }
            _entities.SaveChanges() ;
        }

        internal void Delete(LopHoc lopHoc)
        {
            _entities = new QuanlylophocContext() ;
            _entities.Remove(lopHoc);
            _entities.SaveChanges() ;
        }
    }
}
