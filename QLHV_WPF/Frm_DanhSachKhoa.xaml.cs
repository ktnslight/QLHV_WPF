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
    /// Interaction logic for Frm_DanhSachKhoa.xaml
    /// </summary>
    public partial class Frm_DanhSachKhoa : Window
    {
        public Frm_DanhSachKhoa()
        {
            InitializeComponent();
            GetData();
        }
        #region Lấy dữ liệu
        DataDataContext data = new DataDataContext();
        private void GetData()
        {
            var a = from i in data.GetTable<KHOA_HOC>()
                    select i;
            datagrid.ItemsSource = a;
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
                    KHOA_HOC k = (KHOA_HOC)datagrid.SelectedItem;
                    txt_id.Text = k.MaKhoaHoc;
                    txt_khoa.Text = k.TenKhoaHoc;
                    d_ngaybd.DateTime = k.NgayBD;
                    d_ngaykt.DateTime = k.NgayKT;
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
            txt_id.Text = "";
            txt_khoa.Text = "";
            d_ngaybd.Text = "";
            d_ngaykt.Text = "";
        }
        private void CheckTextBox()
        {
            if (txt_id.Text == "" || txt_khoa.Text == "" || d_ngaybd.Text == "" || d_ngaykt.Text == "")
            {
                tb_Thongbao.Text = "Vui lòng điền đầy đủ thông tin";
            }
        }
        private void Add()
        {
            var id = data.KHOA_HOCs.Where(u => u.MaKhoaHoc == txt_id.Text.Trim()).SingleOrDefault<KHOA_HOC>();
            if (id != null)
            {
                tb_Thongbao.Text = "Mã Khoá học đã tồn tại.";
                return;
            }
            else
            {
                try
                {
                    KHOA_HOC k = new KHOA_HOC();
                    k.MaKhoaHoc = txt_id.Text;
                    k.TenKhoaHoc = txt_khoa.Text;
                    k.NgayBD = d_ngaybd.DateTime;
                    k.NgayKT = d_ngaykt.DateTime;
                    tb_Thongbao.Text = "Thêm thành công!";
                    data.KHOA_HOCs.InsertOnSubmit(k);
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
        private void Update()
        {
            KHOA_HOC k = (data.KHOA_HOCs.Where(t => t.MaKhoaHoc == txt_id.Text).SingleOrDefault<KHOA_HOC>());
            try
            {
                k.TenKhoaHoc = txt_khoa.Text;
                k.NgayBD = d_ngaybd.DateTime;
                k.NgayKT = d_ngaykt.DateTime;
                
                data.SubmitChanges();
                tb_Thongbao.Text = "Chỉnh sửa thành công!";
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
        private void Remove()
        {
            try
            {

                if (System.Windows.Forms.MessageBox.Show("Xác nhận xoá? ", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    KHOA_HOC k = data.KHOA_HOCs.Single(item => item.MaKhoaHoc == txt_id.Text);
                    data.KHOA_HOCs.DeleteOnSubmit(k);
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
    }
    #endregion
}
