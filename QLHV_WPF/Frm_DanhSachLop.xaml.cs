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
    /// Interaction logic for Frm_DanhSachLop.xaml
    /// </summary>
    public partial class Frm_DanhSachLop : Window
    {
        public Frm_DanhSachLop()
        {
            InitializeComponent();
            GetData();
            LoadKhoa();
        }
        #region Lấy dữ liệu
        DataDataContext data = new DataDataContext();
        private void GetData()
        {
            var a = from i in data.GetTable<LOP_HOC>()
                    select i;
            datagrid.ItemsSource = a;
        }
        private void LoadKhoa()
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
                    LOP_HOC lop = (LOP_HOC)datagrid.SelectedItem;
                    txt_id.Text = lop.MaLopHoc;
                    txt_lop.Text = lop.TenLopHoc;
                    txt_hocphi.Text = lop.HocPhi.ToString();
                    txt_ss.Text = lop.SiSoDK.ToString();
                    cb_khoa.SelectedValue = lop.MaKhoaHoc;
                }
            }
            catch(Exception ex)
            {
            }
        }
        #endregion
        #region Hàm lệnh
        private void Clear()
        {
            txt_id.Text = "";
            txt_lop.Text = "";
            txt_ss.Text = "";
            txt_hocphi.Text = "";
            cb_khoa.Text = "";
            tb_Thongbao.Text = "";
        }
        private void CheckTextBox()
        {
            if (txt_id.Text == "" || txt_lop.Text == "" || txt_hocphi.Text ==""|| cb_khoa.Text == "")
            {
                tb_Thongbao.Text = "Vui lòng điền đầy đủ thông tin";
            }
        }
        private void Add()
        {
            var id = data.LOP_HOCs.Where(u => u.MaLopHoc == txt_id.Text.Trim()).SingleOrDefault<LOP_HOC>();
            if (id != null)
            {
                tb_Thongbao.Text = "Mã Lớp đã tồn tại.";
                return;
            }
            else
            {
                try
                {
                    LOP_HOC l = new LOP_HOC();
                    l.MaKhoaHoc = cb_khoa.SelectedValue.ToString();
                    l.MaLopHoc = txt_id.Text;
                    l.SiSoDK = 0;
                    l.HocPhi = int.Parse(txt_hocphi.Text);
                    l.TenLopHoc = txt_lop.Text;
                    data.LOP_HOCs.InsertOnSubmit(l);
                    data.SubmitChanges();
                    tb_Thongbao.Text = "Thêm thành công!";
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
                    LOP_HOC l = data.LOP_HOCs.Single(item => item.MaLopHoc == txt_id.Text);
                    data.LOP_HOCs.DeleteOnSubmit(l);
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
            LOP_HOC l = (data.LOP_HOCs.Where(t => t.MaLopHoc == txt_id.Text).SingleOrDefault<LOP_HOC>());
            try
            {
                l.HocPhi = int.Parse(txt_hocphi.Text);
                l.TenLopHoc = txt_lop.Text;
                
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