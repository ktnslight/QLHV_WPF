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
    /// Interaction logic for Frm_DangNhap.xaml
    /// </summary>
    public partial class Frm_DangNhap
    {
        DataDataContext data = new DataDataContext();
        public Frm_DangNhap()
        {
            InitializeComponent();
        }
        private void CheckTextBox()
        {
            if(Txt_Tendangnhap.Text =="" || Txt_Matkhau.Password == "")
            {
                Tb_Thongbao.Text = "Vui lòng điền đầy đủ thông tin Tài khoản!";
            }
        }
        //Kiểm tra tài khoàn/mật khẩu
        private bool IsvaildUser(string userName, string password)
        {

            var u = from a in data.TAI_KHOANs
                    where a.TaiKhoan == Txt_Tendangnhap.Text
                    && a.MatKhau == Txt_Matkhau.Password
                    select a;
            if (u.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region Button_Click
        List<mTaikhoan> pq = new List<mTaikhoan>();
        private void Btn_DangNhap_Click(object sender, RoutedEventArgs e)
        {
            CheckTextBox();
            if (IsvaildUser(Txt_Tendangnhap.Text, Txt_Matkhau.Password))
            {
                pq.Clear();
                var a = from i in data.TAI_KHOANs
                        where i.TaiKhoan == Txt_Tendangnhap.Text
                        select i;
                var b = a.OrderBy(x => Guid.NewGuid()).ToList();
                foreach(var c in b)
                {
                    mTaikhoan tk = new mTaikhoan();
                    tk.Taikhoan = c.TaiKhoan;
                    tk.Loai = c.MaLoai;
                    pq.Add(tk);
                }
                if (pq[0].Loai == 1)
                {
                    MainWindow main = new MainWindow();
                    Close();
                    main.ShowDialog();
                    
                }
                else if (pq[0].Loai == 2)
                {
                    Frm_ThiTracNghiem thi = new Frm_ThiTracNghiem(Txt_Tendangnhap.Text);
                    Close();
                    thi.ShowDialog();                    
                }
            }
            else
            {
                Tb_Thongbao.Text = "Tài khoản hoặc Mật khẩu không chính xác!";
            }
        }

        private void Btn_Thoat_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát chương trình không?", "Thông báo!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        #endregion
    }

    internal class mTaikhoan
    {
        private string taikhoan;
        int loai;

        public string Taikhoan { get => taikhoan; set => taikhoan = value; }
        public int Loai { get => loai; set => loai = value; }
    }
}
