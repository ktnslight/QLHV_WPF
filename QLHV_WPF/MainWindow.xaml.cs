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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLHV_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void dkhocvien_Click(object sender, RoutedEventArgs e)
        {
            Frm_DangKiHocVien dangkihocvien = new Frm_DangKiHocVien();
            dangkihocvien.ShowDialog();
        }

        private void dshocvien_Click(object sender, RoutedEventArgs e)
        {
            Frm_DanhSachHocVien dshv = new Frm_DanhSachHocVien();
            dshv.ShowDialog();
        }

        private void dsgiaovien_Click(object sender, RoutedEventArgs e)
        {
            Frm_DanhSachGiaoVien dsgv = new Frm_DanhSachGiaoVien();
            dsgv.ShowDialog();
        }

        private void dslop_Click(object sender, RoutedEventArgs e)
        {
            Frm_DanhSachLop dslop = new Frm_DanhSachLop();
            dslop.ShowDialog();
        }

        private void dskhoa_Click(object sender, RoutedEventArgs e)
        {
            Frm_DanhSachKhoa dskhoa = new Frm_DanhSachKhoa();
            dskhoa.ShowDialog();
        }

        private void phancong_Click(object sender, RoutedEventArgs e)
        {
            Frm_PhanCong pc = new Frm_PhanCong();
            pc.ShowDialog();
        }

        private void taikhoan_Click(object sender, RoutedEventArgs e)
        {
            Frm_DanhSachTaiKhoan tk = new Frm_DanhSachTaiKhoan();
            tk.ShowDialog();
        }

        private void kithi_Click(object sender, RoutedEventArgs e)
        {
            Frm_TaoKiThi kt = new Frm_TaoKiThi();
            kt.ShowDialog();
        }

        private void thoat_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Frm_DangNhap a = new Frm_DangNhap();
            a.ShowDialog();
        }

        private void ketqua_Click(object sender, RoutedEventArgs e)
        {
            Frm_XemKetQua kq = new Frm_XemKetQua();
            kq.ShowDialog();
        }
    }
}
