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
    /// Interaction logic for Frm_TaoKiThi.xaml
    /// </summary>
    public partial class Frm_TaoKiThi : Window
    {
        public Frm_TaoKiThi()
        {
            InitializeComponent();
            GetData_Kithi();
        }
        #region Load Dữ liệu
        DataDataContext data = new DataDataContext();
        private void GetData_Kithi()
        {
            var a = from i in data.GetTable<KI_THI>()
                    select i;
            kithi.ItemsSource = a;
        }
        private void GetData_DeThi()
        {
            var a = from i in data.GetTable<DETHI>()
                    where i.MaKiThi == txt_id_kt.Text
                    select i;
            dethi.ItemsSource = a;
            cb_dethi.ItemsSource = a;
            cb_dethi.DisplayMemberPath = "MaDeThi";
            cb_dethi.SelectedValuePath = "MaDeThi";
        }
        private void GetData_Cauhoi()
        {
            var a = from x in data.GetTable<CAU_HOI>()
                    from y in data.GetTable<CT_DETHI>()
                    from z in data.GetTable<DETHI>()
                    where y.MaDeThi==int.Parse(cb_dethi.Text) && y.MaDeThi == z.MaDeThi && y.MaCauHoi == x.MaCauHoi
                    select x;
            cauhoi.ItemsSource = a;
        }
        private void GetData_Monhoc()
        {
            var a = from i in data.GetTable<LOP_HOC>()
                    select i;
            cb_lop.ItemsSource = a;
            cb_lop.DisplayMemberPath = "TenLopHoc";
            cb_lop.SelectedValuePath = "MaLopHoc";
        }
        private void kithi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int rowindex = kithi.SelectedIndex;
                if (rowindex == -1)
                {
                    Clear();
                }
                else
                {
                    KI_THI a = (KI_THI)kithi.SelectedItem;
                    txt_id_kt.Text = a.MaKiThi;
                    txt_kithi.Text = a.TenKiThi;
                    GetData_DeThi();                   
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void dethi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int rowindex = kithi.SelectedIndex;
                if (rowindex == -1)
                {
                    Clear();
                }
                else
                {
                    DETHI a = (DETHI)dethi.SelectedItem;
                    cb_dethi.SelectedValue = a.MaDeThi;
                    GetData_Cauhoi();
                    GetData_Monhoc();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void cauhoi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int rowindex = kithi.SelectedIndex;
                if (rowindex == -1)
                {
                    
                }
                else
                {
                    CAU_HOI a = (CAU_HOI)cauhoi.SelectedItem;
                    txt_idq.Text = a.MaCauHoi.ToString();
                    txt_Cauhoi.Text = a.CauHoi;
                    txt_da1.Text = a.DapAnA;
                    txt_da2.Text = a.DapAnB;
                    txt_da3.Text = a.DapAnC;
                    txt_da4.Text = a.DapAnD;
                    if (a.TraLoi == "A")
                    {
                        daA.IsChecked = true;
                    }
                    else if (a.TraLoi =="B")
                    {
                        daB.IsChecked = true;
                    }
                    else if (a.TraLoi == "C")
                    {
                        daC.IsChecked = true;
                    }
                    else if (a.TraLoi == "D")
                    {
                        daD.IsChecked = true;
                    }
                    cb_lop.SelectedValue = a.MaLopHoc;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        #region Hàm lệnh
        private void Clear()
        {
            txt_id_kt.Text = "";
            txt_kithi.Text = "";
        }
        private void AddKiThi()
        { var id = data.KI_THIs.Where(u => u.MaKiThi == txt_id_kt.Text.Trim()).SingleOrDefault<KI_THI>();
            if (id != null)
            {
                tb_thongbao.Text = "Mã Kì thi đã tồn tại.";
                return;
            }
            else
            {
                KI_THI k = new KI_THI();
                k.MaKiThi = txt_id_kt.Text;
                k.TenKiThi = txt_kithi.Text;
                data.KI_THIs.InsertOnSubmit(k);
                data.SubmitChanges();
                tb_thongbao.Text = "Thêm kì thi thành công";
            }
        }
        private void AddDeThi()
        {
            var id = data.DETHIs.Where(u => u.MaDeThi == int.Parse(cb_dethi.Text)).SingleOrDefault<DETHI>();
            if (id != null)
            {
                tb_thongbao.Text = "Mã Đề thi đã tồn tại.";
                return;
            }
            else
            {
                DETHI d = new DETHI();
                d.MaKiThi = txt_id_kt.Text;
                d.MaDeThi = int.Parse(cb_dethi.Text);
                data.DETHIs.InsertOnSubmit(d);
                data.SubmitChanges();
                tb_thongbao.Text = "Thêm đề thi thành công";
            }
        }
        string traloi;
        private void AddCauHoi()
        {

            var id = data.CAU_HOIs.Where(u => u.MaCauHoi == int.Parse(txt_idq.Text)).SingleOrDefault<CAU_HOI>();
            if (id != null)
            {
                tb_thongbao.Text = "Mã Câu hỏi đã tồn tại.";
                return;
            }
            else
            {
                CAU_HOI h = new CAU_HOI();
                h.MaCauHoi = int.Parse(txt_idq.Text);
                h.MaLopHoc = cb_lop.SelectedValue.ToString();
                h.CauHoi = txt_Cauhoi.Text;
                h.DapAnA = txt_da1.Text;
                h.DapAnB = txt_da2.Text;
                h.DapAnC = txt_da3.Text;
                h.DapAnD = txt_da4.Text;
                if (daA.IsChecked == true)
                {
                    traloi = "A";
                }
                else if (daB.IsChecked == true)
                {
                    traloi = "B";
                }
                else if (daC.IsChecked == true)
                {
                    traloi = "C";
                }
                else if (daB.IsChecked == true)
                {
                    traloi = "D";
                }
                h.TraLoi = traloi;
                data.CAU_HOIs.InsertOnSubmit(h);
                data.SubmitChanges();
                CT_DETHI ct = new CT_DETHI();
                ct.MaDeThi = int.Parse(cb_dethi.Text);
                ct.MaCauHoi = int.Parse(txt_idq.Text);
                data.CT_DETHIs.InsertOnSubmit(ct);
                data.SubmitChanges();
                tb_thongbao.Text = "Thêm câu hỏi thành công!";
            }
        }
        private void UpdateKithi()
        {
            KI_THI k = (data.KI_THIs.Where(t => t.MaKiThi == txt_id_kt.Text).SingleOrDefault<KI_THI>());
            try
            {
                k.TenKiThi = txt_kithi.Text;
                data.SubmitChanges();
                tb_thongbao.Text = "Chỉnh sửa thành công!";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Lỗi");
            }
            finally
            {
                GetData_Kithi();
            }
        }
        private void UpdateCauHoi()
        {
            CAU_HOI h = (data.CAU_HOIs.Where(t => t.MaCauHoi == int.Parse(txt_idq.Text)).SingleOrDefault<CAU_HOI>());
            try
            {
                h.MaLopHoc = cb_lop.SelectedValue.ToString();
                h.CauHoi = txt_Cauhoi.Text;
                h.DapAnA = txt_da1.Text;
                h.DapAnB = txt_da2.Text;
                h.DapAnC = txt_da3.Text;
                h.DapAnD = txt_da4.Text;
                if (daA.IsChecked == true)
                {
                    traloi = "A";
                }
                else if (daB.IsChecked == true)
                {
                    traloi = "B";
                }
                else if (daC.IsChecked == true)
                {
                    traloi = "C";
                }
                else if (daB.IsChecked == true)
                {
                    traloi = "D";
                }
                h.TraLoi = traloi;
                data.SubmitChanges();
                tb_thongbao.Text = "Chỉnh sửa thành công!";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Lỗi");
            }
            finally
            {
                GetData_Cauhoi();
            }
        }
        private void RemoveKiThi()
        {
            try
            {

                if (System.Windows.Forms.MessageBox.Show("Xác nhận xoá? ", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    KI_THI k = data.KI_THIs.Single(item => item.MaKiThi == txt_id_kt.Text);
                    data.KI_THIs.DeleteOnSubmit(k);
                    //var id = data.DETHIs.Where(u => u.MaDeThi == int.Parse(cb_dethi.Text)).SingleOrDefault<DETHI>();
                    //if (id != null)
                    //{
                    //    DETHI d = data.DETHIs.Single(item => item.MaKiThi == txt_id_kt.Text);
                    //    data.DETHIs.DeleteOnSubmit(d);

                    //}
                    data.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                GetData_Kithi();
            }
        }
        private void RemoveDeThi()
        {
            try
            {

                if (System.Windows.Forms.MessageBox.Show("Xác nhận xoá? ", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    DETHI d = data.DETHIs.Single(item => item.MaDeThi == int.Parse(cb_dethi.Text));
                    data.DETHIs.DeleteOnSubmit(d);
                    data.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                GetData_DeThi();
            }
        }
        private void RemoveCauHoi()
        {
            try
            {
                if (System.Windows.Forms.MessageBox.Show("Xác nhận xoá? ", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    CAU_HOI c = data.CAU_HOIs.Single(item => item.MaCauHoi == int.Parse(txt_idq.Text));
                    data.CAU_HOIs.DeleteOnSubmit(c);
                    CT_DETHI ct = data.CT_DETHIs.Single(i => i.MaCauHoi == int.Parse(txt_idq.Text));
                    data.CT_DETHIs.DeleteOnSubmit(ct);
                    data.SubmitChanges();
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                GetData_Cauhoi();
            }
        }
        #endregion
        #region Button
        private void Btn_ThemKiThi_Click(object sender, RoutedEventArgs e)
        {
            if (txt_id_kt.Text == "" || txt_kithi.Text == "")
            {
                tb_thongbao.Text = "Vui lòng nhập đầy đủ thông tin!";
            }
            else
            {
                AddKiThi();
            }
            GetData_Kithi();
        }

        private void Btn_XoaKiThi_Click(object sender, RoutedEventArgs e)
        {
            RemoveKiThi();
            GetData_Kithi();
        }

        private void Btn_LuuKiThi_Click(object sender, RoutedEventArgs e)
        {
            UpdateKithi();
            GetData_Kithi();
        }

        private void Btn_Lammoi_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Btn_ThemDeThi_Click(object sender, RoutedEventArgs e)
        {
            if (cb_dethi.Text == "")
            {
                tb_thongbao.Text = "Vui lòng nhập đầy đủ thông tin đề thi!";
            }
            else
            {
                AddDeThi();
            }
            GetData_DeThi();
        }

        private void Btn_XoaDeThi_Click(object sender, RoutedEventArgs e)
        {
            RemoveDeThi();
            GetData_DeThi();
        }

        private void Btn_ThemCauHoi_Click(object sender, RoutedEventArgs e)
        {
            if (txt_idq.Text == "" || cb_lop.Text == "" || txt_da1.Text == "" || txt_da2.Text == "" || txt_da3.Text == "" || txt_da4.Text == "" || daA.IsChecked == false && daB.IsChecked == false && daC.IsChecked == false && daD.IsChecked == false)
            {
                tb_thongbao.Text = "Vui lòng điền đầy đủ thông tin câu hỏi!";
            }
            else
            {
                AddCauHoi();
            }
            GetData_Cauhoi();
        }

        private void Btn_LuuCauHoi_Click(object sender, RoutedEventArgs e)
        {
            UpdateCauHoi();
            GetData_Cauhoi();
        }

        private void Btn_XoaCauHoi_Click(object sender, RoutedEventArgs e)
        {
            RemoveCauHoi();
            GetData_Cauhoi();
        }

        private void Btn_TaoMoiCauHoi_Click(object sender, RoutedEventArgs e)
        {
            txt_idq.Text = "";
            txt_Cauhoi.Text = "";
            cb_lop.Text = "";
            txt_da1.Text = "";
            txt_da2.Text = "";
            txt_da3.Text = "";
            txt_da4.Text = "";
            daA.IsChecked = false;
            daB.IsChecked = false;
            daC.IsChecked = false;
            daD.IsChecked = false;
        }
        #endregion
    }
}
