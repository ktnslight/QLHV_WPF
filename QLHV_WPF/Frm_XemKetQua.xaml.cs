using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QLHV_WPF
{
    /// <summary>
    /// Interaction logic for Frm_XemKetQua.xaml
    /// </summary>
    public partial class Frm_XemKetQua : Window
    {
        public Frm_XemKetQua()
        {
            InitializeComponent();
            GetData();
        }
        DataDataContext data = new DataDataContext();
        private void GetData()
        {
            var a = from i in data.GetTable<THI>()
                    from x in data.GetTable<HOC_VIEN>()
                    from y in data.GetTable<LOP_HOC>()
                    where i.MaHocVien == x.MaHocVien && i.MaLopHoc == y.MaLopHoc
                    select new
                    {
                        dethi = i.MaDeThi,
                        mon = y.MaLopHoc,
                        hocvien = x.HoTenHocVien,
                        ngaythi = i.NgayThi,
                        diem = i.KetQua
                    };
            datagrid.ItemsSource = a;
        }
    }
}
