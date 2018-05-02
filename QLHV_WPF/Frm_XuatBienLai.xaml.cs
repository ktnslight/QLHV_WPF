using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for Frm_XuatBienLai.xaml
    /// </summary>
    public partial class Frm_XuatBienLai : Window
    {
        public Frm_XuatBienLai(string mahv)
        {
            InitializeComponent();
            GetData(mahv);
        }
        #region Lấy dữ liệu
        DataDataContext data = new DataDataContext();
        private void GetData(string mahv)
        {
            var a = from i1 in data.GetTable<BIEN_LAI>()
                    from i2 in data.GetTable<XUAT>()
                    from i3 in data.GetTable<HOC_VIEN>()
                    where i1.MaBL == i2.MaBL &&i3.MaHocVien == mahv && i2.MaHocVien == i3.MaHocVien
                    select new
                    {
                        tenhocvien = i3.HoTenHocVien,
                        noidung = i1.NoiDung,
                        sotien = i1.SoTien,
                        ngay = i1.NgayBL
                    };
            bienlai.ItemsSource = a;

        }
        #endregion
        private void ExportToExcelAndCsv()
        {
            DataGrid dg = bienlai;
            dg.SelectAllCells();
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);
            dg.UnselectAllCells();
            String Clipboardresult = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            StreamWriter swObj = new StreamWriter("bienlai.csv");
            swObj.WriteLine(Clipboardresult);
            swObj.Close();
            Process.Start("bienlai.csv");
        }

        private void Btn_Xuat_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcelAndCsv();
        }
    }
}
