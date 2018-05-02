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
    /// Interaction logic for Frm_DanhSachTaiKhoan.xaml
    /// </summary>
    public partial class Frm_DanhSachTaiKhoan : Window
    {
        public Frm_DanhSachTaiKhoan()
        {
            InitializeComponent();
            GetData();
        }
        #region Lấy dữ liệu
        DataDataContext data = new DataDataContext();
        private void GetData()
        {
            var a = from i in data.GetTable<TAI_KHOAN>()
                    select i;
            datagrid.ItemsSource = a;
            var b = from u in data.GetTable<PHAN_QUYEN>()
                    select u;
            cb_loai.ItemsSource = b;
            cb_loai.DisplayMemberPath = "PhanQuyen";
            cb_loai.SelectedValuePath = "MaLoai";
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                    TAI_KHOAN tk = (TAI_KHOAN)datagrid.SelectedItem;
                    txt_username.Text = tk.TaiKhoan;
                    txt_password.Text = tk.MatKhau;
                    cb_loai.SelectedValue = tk.MaLoai;
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
            txt_username.Text = "";
            txt_password.Text = "";
            cb_loai.Text = "";
        }
        private void CheckTextBox()
        {
            if (txt_username.Text == "" || txt_password.Text == "" || cb_loai.Text == "")
            {
                tb_thongbao.Text = "Vui lòng nhập đầy đủ thông tin!";
            }
        }
        private void Add()
        {
            var id = data.TAI_KHOANs.Where(u => u.TaiKhoan == txt_username.Text.Trim()).SingleOrDefault<TAI_KHOAN>();
            if (id != null)
            {
                tb_thongbao.Text = "Tên Tài khoản đã tồn tại.";
                return;
            }
            else
            {
                try
                {
                    TAI_KHOAN tk = new TAI_KHOAN();
                    tk.TaiKhoan = txt_username.Text;
                    tk.MatKhau = txt_password.Text;
                    tk.MaLoai = int.Parse(cb_loai.SelectedValue.ToString());
                    data.TAI_KHOANs.InsertOnSubmit(tk);
                    data.SubmitChanges();
                }
                catch
                {

                }
                finally
                {
                    GetData();
                }
            }
        }
        private void Remove()
        {
            try
            {

                if (System.Windows.Forms.MessageBox.Show("Xác nhận xoá? ", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    TAI_KHOAN tk = data.TAI_KHOANs.Single(item => item.TaiKhoan == txt_username.Text);
                    data.TAI_KHOANs.DeleteOnSubmit(tk);
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
            TAI_KHOAN tk = (data.TAI_KHOANs.Where(t => t.TaiKhoan == txt_username.Text).SingleOrDefault<TAI_KHOAN>());
            try
            {
                tk.MatKhau = txt_password.Text;
                tk.MaLoai = int.Parse(cb_loai.SelectedValue.ToString());
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
        private void Btn_addnew_Click(object sender, RoutedEventArgs e)
        {
            CheckTextBox();
            Add();
        }

        private void Btn_remove_Click(object sender, RoutedEventArgs e)
        {
            Remove();
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            CheckTextBox();
            Update();
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
    }
}
