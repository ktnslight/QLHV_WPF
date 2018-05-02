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
    /// Interaction logic for Frm_DangKiHocVien.xaml
    /// </summary>
    public partial class Frm_DangKiHocVien : Window
    {
        DataDataContext data = new DataDataContext();
        public Frm_DangKiHocVien()
        {
            InitializeComponent();
            d_ngaydangki.Text = DateTime.Today.ToShortDateString();
            LoadKhoaHoc();
            LoadCa();
        }
        #region LoadData
        private void LoadKhoaHoc()
        {
            var a = from i in data.GetTable<KHOA_HOC>()
                    select new
                    {
                        makhoahoc = i.MaKhoaHoc,
                        tenkhoahoc = i.TenKhoaHoc
                    };
            cb_khoa.ItemsSource = a.ToList();
            cb_khoa.DisplayMemberPath = "tenkhoahoc";
            cb_khoa.SelectedValuePath = "makhoahoc";
        }
        private void LoadLophoc()
        {
            var a = from i in data.GetTable<LOP_HOC>()
                    where i.MaKhoaHoc == cb_khoa.SelectedValue
                    select new
                    {
                        malop = i.MaLopHoc,
                        tenlop = i.TenLopHoc,
                        hocphi = i.HocPhi
                    };
            cb_lop.ItemsSource = a.ToList();
            cb_lop.DisplayMemberPath = "tenlop";
            cb_lop.SelectedValuePath = "malop";
            
            
        }
        private void LoadCa()
        {
            var a = from i in data.GetTable<CA_HOC>()
                    select new
                    {
                        maca = i.MaCaHoc,
                    };
            cb_ca.ItemsSource = a.ToList();
            cb_ca.DisplayMemberPath = "maca";
            cb_ca.SelectedValuePath = "maca";
        }
        private void LoadHocphi()
        {
            var a = from i in data.LOP_HOCs
                    where i.MaLopHoc == cb_lop.SelectedValue
                    select new { hocphi = i.HocPhi };
            foreach (var hp in a)
            {
                txt_hocphi.Text = hp.hocphi.ToString();
            }
        }
        private void cb_khoa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadLophoc();
        }

        private void cb_lop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadHocphi();
        }
        #endregion
        #region Hàm lệnh
        private void Kiemtra()
        {
            if (txt_ten.Text == "" || txt_id.Text == "" || d_ngaysinh.Text == "" || txt_diachi.Text == "" || rNam.IsChecked == false && rNu.IsChecked == false || cb_khoa.Text == "" || cb_lop.Text == "" || cb_ca.Text == ""||txt_hocphi.Text=="")
            {
                tb_thongbao.Text = "Vui lòng nhập đủ những thông tin cần thiết!";
            }
        }
        string gioitinh;
        private void Dangki()
        {
            var id = data.HOC_VIENs.Where(u => u.MaHocVien == txt_id.Text.Trim()).SingleOrDefault<HOC_VIEN>();
            if (id != null)
            {
                tb_thongbao.Text = "Mã Học viên đã tồn tại.";
                return;
            }
            else
            {
                if (rNam.IsChecked == true) { gioitinh = "Nam"; }
                else if (rNu.IsChecked == true) { gioitinh = "Nữ"; }
                HOC_VIEN h = new HOC_VIEN();
                h.MaHocVien = txt_id.Text;
                h.HoTenHocVien = txt_ten.Text;
                h.NgaySinh = d_ngaysinh.DateTime;
                h.GioiTinh = gioitinh;
                h.NgheNghiep = txt_nghenghiep.Text;
                h.SoDT = txt_phone.Text;
                h.DiaChi = txt_diachi.Text;
                h.Email = txt_email.Text;
                data.HOC_VIENs.InsertOnSubmit(h);
                data.SubmitChanges();
                HOC hoc = new HOC();
                hoc.MaHocVien = txt_id.Text;
                hoc.MaLopHoc = cb_lop.SelectedValue.ToString();
                data.HOCs.InsertOnSubmit(hoc);
                data.SubmitChanges();
                var ss = data.LOP_HOCs.Where(u => u.MaLopHoc == cb_lop.SelectedValue).Single();
                ss.SiSoDK = ss.SiSoDK + 1;
                data.SubmitChanges();
            }

        }
        private void Taomoi()
        {
            txt_id.Text = "";
            txt_ten.Text = "";
            txt_phone.Text = "";
            txt_diachi.Text = "";
            txt_email.Text = "";
            txt_nghenghiep.Text = "";
            txt_hocphi.Text = "";
            rNam.IsChecked = false;
            rNu.IsChecked = false;
            cb_ca.Text = "";
            cb_khoa.Text = "";
            tb_thongbao.Text = "";
        }
        #endregion
        #region Button
        private void Btn_Them_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dangki();
                tb_thongbao.Text = "Đăng kí Thành công!";
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Btn_Taomoi_Click(object sender, RoutedEventArgs e)
        {
            Taomoi();
        }

        private void Btn_Dong_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
        int mabl=1;
        private void Btn_Xuat_Click(object sender, RoutedEventArgs e)
        {
            var id = data.BIEN_LAIs.Where(u => u.MaBL == mabl).SingleOrDefault<BIEN_LAI>();
            if (id != null)
            {
                mabl++;
            }
                try
                {
                    BIEN_LAI bl = new BIEN_LAI();
                    bl.MaBL = mabl;
                    bl.NgayBL = d_ngaydangki.DateTime;
                    bl.SoTien = int.Parse(txt_hocphi.Text);
                    bl.NoiDung = "Tiền học phí";
                    data.BIEN_LAIs.InsertOnSubmit(bl);
                    data.SubmitChanges();
                    XUAT x = new XUAT();
                    x.MaBL = mabl;
                    x.MaHocVien = txt_id.Text;
                    data.XUATs.InsertOnSubmit(x);
                    data.SubmitChanges();
                }
                catch(Exception ex)
                {
                MessageBox.Show(ex.Message);
                }
                finally
                {

                    Frm_XuatBienLai xbl = new Frm_XuatBienLai(txt_id.Text);
                    xbl.ShowDialog();
                }
        }
    }
}
