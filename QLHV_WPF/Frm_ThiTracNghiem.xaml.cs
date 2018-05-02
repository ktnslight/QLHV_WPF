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
using System.Windows.Threading;

namespace QLHV_WPF
{
    /// <summary>
    /// Interaction logic for Frm_ThiTracNghiem.xaml
    /// </summary>
    public partial class Frm_ThiTracNghiem : Window
    {
        public Frm_ThiTracNghiem()
        {
            InitializeComponent();
            GetData_Kithi();
        }
        public Frm_ThiTracNghiem(string _taikhoan)
        {
            InitializeComponent();
            GetData_Hocvien(_taikhoan);
            GetData_Kithi();
            tb_ngaythi.Text = ngaythi;
        }
        #region Đồng hồ đếm ngược + Ngày thi
        string ngaythi = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        private int time = 30;
        private DispatcherTimer Timer;
        private void Time()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick1;
            Timer.Start();
        }
        private void Timer_Tick1(object sender, EventArgs e)
        {
            if (time > 0)
            {


                if (time < 15)
                {
                    if (time < 5)
                    {
                        tb_time.Foreground = Brushes.Red;
                    }
                    else
                    {
                        tb_time.Foreground = Brushes.Black;
                    }
                    time--;
                    tb_time.Text = string.Format("00:0{0}:{1}", time / 60, time % 60);
                }
                else
                {
                    time--;
                    tb_time.Text = string.Format("00:0{0}:{1}", time / 60, time % 60);
                }
            }
            else
            {
                Timer.Stop();
                System.Windows.MessageBox.Show("Hết thời Gian");
                nopbaithi();
                phanthi.IsEnabled = false;
            }
        }
        #endregion
        #region Lấy dữ liệu
        DataDataContext data = new DataDataContext();
        private void GetData_Kithi()
        {
            var a = from i in data.GetTable<KI_THI>()
                    select i;
            cb_kithi.ItemsSource = a;
            cb_kithi.DisplayMemberPath = "TenKiThi";
            cb_kithi.SelectedValuePath = "MaKiThi";
        }
        private void GetData_DeThi()
        {
            var a = from i in data.GetTable<DETHI>()
                    where i.MaKiThi == cb_kithi.SelectedValue
                    select i;
            cb_dethi.ItemsSource = a;
            cb_dethi.DisplayMemberPath = "MaDeThi";
            cb_dethi.SelectedValuePath = "MaDeThi";
        }
        private void GetData_Monhoc()
        {
            var a = from i in data.GetTable<LOP_HOC>()
                    select i;
            cb_lop.ItemsSource = a;
            cb_lop.DisplayMemberPath = "TenLopHoc";
            cb_lop.SelectedValuePath = "MaLopHoc";
        }
        List<mCauhoi> cauhoi = new List<mCauhoi>();
        int cauhientai = 0;
        private void GetData_Cauhoi()
        {
            cauhoi.Clear();
            var a = from x in data.CAU_HOIs
                    from y in data.CT_DETHIs
                    where y.MaDeThi == int.Parse(cb_dethi.Text) && x.MaCauHoi == y.MaCauHoi && x.MaLopHoc == cb_lop.SelectedValue
                    select new
                    {
                        i = x.MaCauHoi,
                        ch = x.CauHoi,
                        a = x.DapAnA,
                        b = x.DapAnB,
                        c = x.DapAnC,
                        d = x.DapAnD,
                        tl = x.TraLoi,
                        l = x.MaLopHoc,
                    };
            var b = a.OrderBy(x => Guid.NewGuid()).ToList();
            foreach(var c in b)
            {
                mCauhoi q = new mCauhoi();
                q.Macauhoi = c.i;
                q.Cauhoi = c.ch;
                q.Dapana = c.a;
                q.Dapanb = c.b;
                q.Dapanc = c.c;
                q.Dapand = c.d;
                q.Traloi = c.tl;
                q.Malophoc = c.l;

                cauhoi.Add(q);
            }
            cauhientai = 0;
            ShowCH();
        }
        public void ShowCH()
        {
            txt_cauhoi.Text = cauhoi[cauhientai].Cauhoi;
            txt_A.Text = cauhoi[cauhientai].Dapana;
            txt_B.Text = cauhoi[cauhientai].Dapanb;
            txt_C.Text = cauhoi[cauhientai].Dapanc;
            txt_D.Text = cauhoi[cauhientai].Dapand;


            if (cauhoi[cauhientai].Cautraloi == "A")
            {
                rbA.IsChecked = true;
            }

            if (cauhoi[cauhientai].Cautraloi == "B")
            {
                rbB.IsChecked = true;
            }
            if (cauhoi[cauhientai].Cautraloi == "C")
            {
                rbC.IsChecked = true;
            }
            if (cauhoi[cauhientai].Cautraloi == "D")
            {
                rbD.IsChecked = true;
            }
            if (cauhoi[cauhientai].Cautraloi == "E")
            {
                rbA.IsChecked = false;
                rbB.IsChecked = false;
                rbC.IsChecked = false;
                rbD.IsChecked = false;
            }
        }
        
        private void cb_kithi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetData_DeThi();
        }
        private void cb_dethi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetData_Monhoc();
        }
        private void cb_lop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Btn_Batdau.IsEnabled = true;
        }
        #endregion
        #region Lấy thông tin Học viên

        List<mThongtin> thongtin = new List<mThongtin>();
        private void GetData_Hocvien(string taikhoan)
        {
            thongtin.Clear();
            var a = from x in data.HOC_VIENs
                    where x.TaiKhoan == taikhoan
                    select new
                    {
                        id = x.MaHocVien,
                        hoten = x.HoTenHocVien,
                        ngaysinh = x.NgaySinh,
                    };
            var b = a.OrderBy(x => Guid.NewGuid()).ToList();
            foreach (var c in b)
            {
                mThongtin t = new mThongtin();
                t.Mahv = c.id;
                t.Hoten = c.hoten;
                t.Ngaysinh = c.ngaysinh;
                thongtin.Add(t);
                tb_id.Text = c.id;
                tb_ngaysinh.Text = c.ngaysinh.ToShortDateString();
                tb_hoten.Text = c.hoten;
            }
        }

        #endregion
        #region Tính điểm, Lưu kết quả
        int diem=0;
        void capnhatch()
        {

            if (rbA.IsChecked == true)
            {
                cauhoi[cauhientai].Cautraloi = "A";
            }
            else if (rbB.IsChecked == true)
            {
                cauhoi[cauhientai].Cautraloi = "B";
            }
            else if (rbC.IsChecked == true)
            {
                cauhoi[cauhientai].Cautraloi = "C";

            }
            else if (rbD.IsChecked == true)
            {
                cauhoi[cauhientai].Cautraloi = "D";

            }
            else
            {
                cauhoi[cauhientai].Cautraloi = "E";
            }

        }
        private void LuuKQ()
        {
            THI kq = new THI();
            kq.MaDeThi = int.Parse(cb_dethi.SelectedValue.ToString());
            kq.MaHocVien = tb_id.Text;
            kq.MaLopHoc = cb_lop.SelectedValue.ToString();
            kq.KetQua = diem;
            kq.NgayThi = DateTime.Parse(ngaythi);
            data.THIs.InsertOnSubmit(kq);
            data.SubmitChanges();
        }
        public void nopbaithi()
        {
            Timer.Stop();
            foreach (mCauhoi item in cauhoi)
            {
                if (item.Cautraloi.Equals(item.Traloi.Trim().ToUpper()))
                    diem++;
            }

            tb_time.Text = "Điểm" + diem;
            LuuKQ();
            phanthi.IsEnabled = false;
            Btn_Nopbai.IsEnabled = false;
        }
        #endregion
        #region Button chuyển câu
        private void Btn_1_Click(object sender, RoutedEventArgs e)
        {
            cauhientai = 0;
            capnhatch();
            ShowCH();
        }

        private void Btn_2_Click(object sender, RoutedEventArgs e)
        {
            cauhientai = 1;
            capnhatch();
            ShowCH();
        }

        private void Btn_3_Click(object sender, RoutedEventArgs e)
        {
            cauhientai = 2;
            capnhatch();
            ShowCH();
        }

        private void Btn_4_Click(object sender, RoutedEventArgs e)
        {
            cauhientai = 3;
            capnhatch();
            ShowCH();
        }

        private void Btn_5_Click(object sender, RoutedEventArgs e)
        {
            cauhientai = 4;
            capnhatch();
            ShowCH();
        }

        private void Btn_6_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_7_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_8_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_9_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_10_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_Prev_Click(object sender, RoutedEventArgs e)
        {
            capnhatch();
            if (cauhientai == 0)
            {
                return;
            }

            cauhientai--;

            ShowCH();
        }

        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {
            capnhatch();
            if (cauhientai == cauhoi.Count - 1)
            {
                MessageBoxResult d_result = MessageBox.Show("Đã cuối bài thi bạn muốn nộp bài", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (d_result == MessageBoxResult.Yes)
                {
                    nopbaithi();

                }

                return;


            }
            rbA.IsChecked = false;
            rbB.IsChecked = false;
            rbC.IsChecked = false;
            rbD.IsChecked = false;


            cauhientai++;

            ShowCH();
        }
        #endregion
        #region Button
        private void Btn_Batdau_Click(object sender, RoutedEventArgs e)
        {
            GetData_Cauhoi();
            Btn_Batdau.IsEnabled = false;
            phanthi.IsEnabled = true;
            Btn_Nopbai.IsEnabled = true;
            kithi.IsEnabled = false;
            Time();
        }

        private void Btn_Nopbai_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn kết thúc không ?", "Thông báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                nopbaithi();
            }
            else
            {
                return;
            }
        }


        #endregion

        internal class mCauhoi
        {
            private string cauhoi, dapana, dapanb, dapanc, dapand, traloi, cautraloi,malophoc;
            private int macauhoi;

            public string Cauhoi { get => cauhoi; set => cauhoi = value; }
            public string Dapana { get => dapana; set => dapana = value; }
            public string Dapanb { get => dapanb; set => dapanb = value; }
            public string Dapanc { get => dapanc; set => dapanc = value; }
            public string Dapand { get => dapand; set => dapand = value; }
            public string Traloi { get => traloi; set => traloi = value; }
            public string Cautraloi { get => cautraloi; set => cautraloi = value; }
            public int Macauhoi { get => macauhoi; set => macauhoi = value; }
            public string Malophoc { get => malophoc; set => malophoc = value; }
        }
    }

    internal class mThongtin
    {
        private string mahv, hoten;
        private DateTime ngaysinh;

        public string Mahv { get => mahv; set => mahv = value; }
        public string Hoten { get => hoten; set => hoten = value; }
        public DateTime Ngaysinh { get => ngaysinh; set => ngaysinh = value; }
    }
}
