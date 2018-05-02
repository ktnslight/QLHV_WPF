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
    /// Interaction logic for Frm_DanhSachHocVien.xaml
    /// </summary>
    public partial class Frm_DanhSachHocVien : Window
    {
        DataDataContext data = new DataDataContext();
        public Frm_DanhSachHocVien()
        {
            InitializeComponent();
            GetData();
        }
        #region Load dữ liệu
        private void GetData()
        {
            var hocvien = from i in data.GetTable<HOC_VIEN>() select i;
            datagrid.ItemsSource = hocvien;
        }
        private void Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int rowindex = datagrid.SelectedIndex;
                if (rowindex == -1)
                {
                    Clear();
                }
                else
                {
                    HOC_VIEN hv = (HOC_VIEN)datagrid.SelectedItem;
                    txt_id.Text = hv.MaHocVien;
                    txt_hoten.Text = hv.HoTenHocVien;
                    txt_nghenghiep.Text = hv.NgheNghiep;
                    txt_diachi.Text = hv.DiaChi;
                    txt_email.Text = hv.Email;
                    txt_gioitinh.Text = hv.GioiTinh;
                    txt_dt.Text = hv.SoDT.ToString();
                    date_ngaysinh.DateTime = hv.NgaySinh;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion
        #region Lệnh
        private void Remove()
        {
            try
            {

                if (System.Windows.Forms.MessageBox.Show("Xoá Học viên này? ", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    HOC_VIEN hv = data.HOC_VIENs.Single(item => item.MaHocVien == txt_id.Text);
                    data.HOC_VIENs.DeleteOnSubmit(hv);
                    data.SubmitChanges();
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
        private void Update()
        {
            HOC_VIEN hv = (data.HOC_VIENs.Where(t => t.MaHocVien == txt_id.Text).SingleOrDefault<HOC_VIEN>());
            try
            {
                hv.HoTenHocVien = txt_hoten.Text;
                hv.GioiTinh = txt_gioitinh.Text;
                hv.NgheNghiep = txt_nghenghiep.Text;
                hv.NgaySinh = date_ngaysinh.DateTime;
                hv.DiaChi = txt_diachi.Text;
                hv.Email = txt_email.Text;
                hv.SoDT = txt_dt.Text;
                data.SubmitChanges();
                tb_thongbao.Text = "Chỉnh sửa thành công!";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Lỗi");
            }
            finally
            {
                GetData();
            }
        }
        private void Clear()
        {
            txt_id.Text = "";
            txt_hoten.Text = "";
            txt_nghenghiep.Text = "";
            txt_diachi.Text = "";
            txt_email.Text = "";
            txt_gioitinh.Text = "";
            txt_dt.Text = "";
            date_ngaysinh.Text = "";
        }
        private void Timkiem()
        {
            if (txt_hoten.Text == "")
            {
                GetData();
            }
            else
            {
                datagrid.ItemsSource = data.HOC_VIENs.Where(item =>
        item.HoTenHocVien.Contains(txt_hoten.Text));
            }
        }
        #endregion
        #region Button
        private void Btn_addnew_Click(object sender, RoutedEventArgs e)
        {
            Frm_DangKiHocVien dk = new Frm_DangKiHocVien();
            dk.ShowDialog();
            Close();
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            Update();
            GetData();
        }
        private void Btn_remove_Click(object sender, RoutedEventArgs e)
        {
            Remove();
            GetData();
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            Timkiem();
        }
    }
}
