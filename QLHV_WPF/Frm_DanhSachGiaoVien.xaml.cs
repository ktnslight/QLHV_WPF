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
    /// Interaction logic for Frm_DanhSachGiaoVien.xaml
    /// </summary>
    public partial class Frm_DanhSachGiaoVien : Window
    {
        public Frm_DanhSachGiaoVien()
        {
            InitializeComponent();
            GetData();
        }
        #region Lấy dữ liệu
        DataDataContext data = new DataDataContext();
        private void GetData()
        {
            var a = from i in data.GetTable<GIAO_VIEN>()
                    select i;
            datagrid.ItemsSource = a;
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
                    GIAO_VIEN gv = (GIAO_VIEN)datagrid.SelectedItem;
                    txt_id.Text = gv.MaGiaoVien;
                    txt_ten.Text = gv.HoTenGiaoVien;
                    date_ngaysinh.DateTime = gv.NgaySinh;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Hàm lệnh
        private void Clear()
        {
            txt_id.Text = "";
            txt_ten.Text = "";
            date_ngaysinh.Text = "";
            tb_thongbao.Text = "";
            txt_diachi.Text = "";
            txt_email.Text = "";
        }
        private void Add()
        {
            var id = data.GIAO_VIENs.Where(u => u.MaGiaoVien == txt_id.Text.Trim()).SingleOrDefault<GIAO_VIEN>();
            if (id != null)
            {
                tb_thongbao.Text = "Mã Giáo viên đã tồn tại.";
                return;
            }
            else
            {
                GIAO_VIEN gv = new GIAO_VIEN();
                gv.MaGiaoVien = txt_id.Text;
                gv.HoTenGiaoVien = txt_ten.Text;
                gv.NgaySinh = date_ngaysinh.DateTime;
                gv.DiaChi = txt_diachi.Text;
                gv.Email = txt_email.Text;
                data.GIAO_VIENs.InsertOnSubmit(gv);
                data.SubmitChanges();
            }

        }
        private void Remove()
        {
            try
            {

                if (System.Windows.Forms.MessageBox.Show("Xác nhận xoá? ", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    GIAO_VIEN gv = data.GIAO_VIENs.Single(item => item.MaGiaoVien == txt_id.Text);
                    data.GIAO_VIENs.DeleteOnSubmit(gv);
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
            GIAO_VIEN gv = (data.GIAO_VIENs.Where(t => t.MaGiaoVien == txt_id.Text).SingleOrDefault<GIAO_VIEN>());
            try
            {
                gv.HoTenGiaoVien = txt_ten.Text;
                gv.NgaySinh = date_ngaysinh.DateTime;
                gv.Email = txt_email.Text;
                gv.Email = txt_email.Text;
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
        #endregion
        #region Button
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Add();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                GetData();
            }
        }

        private void Btn_Remove_Click(object sender, RoutedEventArgs e)
        {
            Remove();
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }
        #endregion
    }
}
