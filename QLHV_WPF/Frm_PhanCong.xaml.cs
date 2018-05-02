using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QLHV_WPF
{
    /// <summary>
    /// Interaction logic for Frm_PhanCong.xaml
    /// </summary>
    public partial class Frm_PhanCong : Window
    {
        public Frm_PhanCong()
        {
            InitializeComponent();
            GetData();
            GetData_GiaoVien();
            GetData_Lop();
            GetData_Ca();
        }
        #region Lấy dữ liệu
        DataDataContext data = new DataDataContext();
        private void GetData_GiaoVien()
        {
            var a = from i in data.GetTable<GIAO_VIEN>()
                    select i;
            cb_giaovien.ItemsSource = a;
            cb_giaovien.DisplayMemberPath = "HoTenGiaoVien";
            cb_giaovien.SelectedValuePath = "MaGiaoVien";
        }
        private void GetData_Ca()
        {
            var a = from i in data.GetTable<CA_HOC>()
                    select i;
            cb_ca.ItemsSource = a;
            cb_ca.DisplayMemberPath = "MaCaHoc";
            cb_ca.SelectedValuePath = "MaCaHoc";
        }
        private void GetData_Lop()
        {
            var a = from i in data.GetTable<LOP_HOC>()
                    select i;
            cb_lop.ItemsSource = a;
            cb_lop.DisplayMemberPath = "TenLopHoc";
            cb_lop.SelectedValuePath = "MaLopHoc";
        }
        private void GetData()
        {
            var a = from x in data.GIAO_VIENs
                    from y in data.CA_HOCs
                    from z in data.LOP_HOCs
                    from i in data.PHAN_CONGs
                    where i.MaGiaoVien == x.MaGiaoVien && i.MaLopHoc == z.MaLopHoc && i.MaCaHoc == y.MaCaHoc
                    select new
                    {
                        lop = z.TenLopHoc,
                        buoi = y.Buoi,
                        gio = y.Gio,
                        giaovien = x.HoTenGiaoVien,

                    };
            datagrid.ItemsSource = a;
        }
        #endregion
        private void phancong()
        {
            var id = data.PHAN_CONGs.Where(u => u.MaCaHoc == int.Parse(cb_ca.SelectedValue.ToString())&&u.MaGiaoVien==cb_giaovien.SelectedValue&&u.MaLopHoc==cb_lop.SelectedValue).SingleOrDefault<PHAN_CONG>();
            if (id != null)
            {
                tb_thongbao.Text = "Không thể phân công!";
                return;
            }
            else
            {
                PHAN_CONG pc = new PHAN_CONG();
                pc.MaLopHoc = cb_lop.SelectedValue.ToString();
                pc.MaGiaoVien = cb_giaovien.SelectedValue.ToString();
                pc.MaCaHoc = int.Parse(cb_ca.SelectedValue.ToString());
                data.PHAN_CONGs.InsertOnSubmit(pc);
                data.SubmitChanges();
            }
        }
        private void huy()
        {
            try
            {

                if (System.Windows.Forms.MessageBox.Show("Xác nhận xoá? ", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    PHAN_CONG p = data.PHAN_CONGs.Single(u => u.MaCaHoc == int.Parse(cb_ca.SelectedValue.ToString()) && u.MaGiaoVien == cb_giaovien.SelectedValue && u.MaLopHoc == cb_lop.SelectedValue );
                    data.PHAN_CONGs.DeleteOnSubmit(p);
                  
                    data.SubmitChanges();
                    tb_thongbao.Text = "Đã Huỷ!";
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                GetData();
            }
        }
        List<Phancong> n = new List<Phancong>();
        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            n.Clear();
            var a = from x in data.GIAO_VIENs
                    from y in data.CA_HOCs
                    from z in data.LOP_HOCs
                    from i in data.PHAN_CONGs
                    where i.MaGiaoVien == x.MaGiaoVien && i.MaLopHoc == z.MaLopHoc && i.MaCaHoc == y.MaCaHoc
                    select new
                    {
                        ca = y.MaCaHoc,
                        lop = z.MaLopHoc,
                        gv = x.MaGiaoVien
                    };
            var b = a.OrderBy(x => Guid.NewGuid()).ToList();
            foreach (var c in b)
            {
                Phancong m = new Phancong();
                m.Maca = c.ca;
                m.Magiaovien = c.gv;
                m.Malop = c.lop;
                cb_ca.SelectedValue = m.Maca;
                cb_lop.SelectedValue = m.Malop;
                cb_giaovien.SelectedValue = m.Magiaovien;
            }
        }

        private void Btn_PhanCong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                phancong();
            }
            catch
            {

            }
            finally
            {
                GetData();
            }

        }
        private void Btn_Huy_Click(object sender, RoutedEventArgs e)
        {
            huy();
        }
        internal class Phancong
        {
            private string malop, magiaovien;
            private int maca;

            public string Malop { get => malop; set => malop = value; }
            public string Magiaovien { get => magiaovien; set => magiaovien = value; }
            public int Maca { get => maca; set => maca = value; }
        }


    }
}
