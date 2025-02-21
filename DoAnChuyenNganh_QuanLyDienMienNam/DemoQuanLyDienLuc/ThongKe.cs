using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;


namespace DemoQuanLyDienLuc
{
    public partial class ThongKe : Form
    {
        private DBConnect db = new DBConnect();

        public ThongKe()
        {
            InitializeComponent();
            db = new DBConnect();
            LoadComboBoxes();
            

        }

        private void CapNhatBieuDo(DataTable dtBieuDo)
        {
          
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Load ComboBox Tháng
                cboThang.Items.Add("-- Tháng --");
                for (int i = 1; i <= 12; i++)
                {
                    cboThang.Items.Add($"{i}");
                }
                cboThang.SelectedIndex = 0;

                // Load ComboBox Năm
                cboNam.Items.Add("-- Năm --");
                int currentYear = DateTime.Now.Year;
                for (int i = currentYear - 5; i <= currentYear; i++)
                {
                    cboNam.Items.Add(i.ToString());
                }
                cboNam.SelectedIndex = 0;

                // Load ComboBox Tiêu chí
                cboTieuChi.Items.AddRange(new string[] {
                    "Điện tiêu thụ",
                    "Doanh thu",
                    "Số khách hàng",
                    "Tỷ lệ đúng hạn"
                });
                cboTieuChi.SelectedIndex = 0;

                // Load ComboBox Huyện
                string sqlHuyen = @"SELECT h.MaHuyen, h.TenHuyen 
                         FROM Huyen h 
                         JOIN Tinh t ON h.MaTinh = t.MaTinh 
                         WHERE t.TenTinh = N'Thành phố Hồ Chí Minh'";

                DataTable dtHuyen = db.getDataTable(sqlHuyen);

                DataTable newDtHuyen = new DataTable();
                newDtHuyen.Columns.Add("MaHuyen", typeof(int));
                newDtHuyen.Columns.Add("TenHuyen", typeof(string));

                newDtHuyen.Rows.Add(DBNull.Value, "-- Chọn Quận --");
                foreach (DataRow row in dtHuyen.Rows)
                {
                    newDtHuyen.Rows.Add(row["MaHuyen"], row["TenHuyen"]);
                }

                cboHuyen.DataSource = newDtHuyen;
                cboHuyen.DisplayMember = "TenHuyen";
                cboHuyen.ValueMember = "MaHuyen";

                // Reset ComboBox Xã
                cboXa.DataSource = null;
                cboXa.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load ComboBox: " + ex.Message);
            }
        }

        private void LoadComboBoxTinh()
        {
            
        }


        private void cboHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboHuyen.SelectedValue != null && cboHuyen.SelectedValue != DBNull.Value)
                {
                    int? maHuyen = null;
                    if (int.TryParse(cboHuyen.SelectedValue.ToString(), out int result))
                    {
                        maHuyen = result;
                    }

                    if (maHuyen.HasValue)
                    {
                        string sqlXa = "SELECT MaXa, TenXa FROM Xa WHERE MaHuyen = @MaHuyen";
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@MaHuyen", maHuyen.Value)
                        };

                        DataTable dtXa = db.getDataTable(sqlXa, parameters);

                        DataTable newDtXa = new DataTable();
                        newDtXa.Columns.Add("MaXa", typeof(int));
                        newDtXa.Columns.Add("TenXa", typeof(string));

                        newDtXa.Rows.Add(DBNull.Value, "-- Chọn Phường/Xã --");
                        foreach (DataRow row in dtXa.Rows)
                        {
                            newDtXa.Rows.Add(row["MaXa"], row["TenXa"]);
                        }

                        cboXa.DataSource = newDtXa;
                        cboXa.DisplayMember = "TenXa";
                        cboXa.ValueMember = "MaXa";
                    }
                }
                else
                {
                    cboXa.DataSource = null;
                    cboXa.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load Xã: " + ex.Message);
            }
        }

        private void cboXa_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void LoadThongKe()
        {
            try
            {
                string whereClause = " AND t.TenTinh = N'Thành phố Hồ Chí Minh'";
                List<SqlParameter> parameters = new List<SqlParameter>();

                // Thêm điều kiện tháng năm
                if (cboThang.SelectedIndex > 0)
                {
                    whereClause += " AND hd.Thang = @Thang";
                    parameters.Add(new SqlParameter("@Thang", cboThang.SelectedIndex));
                }

                if (cboNam.SelectedIndex > 0)
                {
                    whereClause += " AND hd.Nam = @Nam";
                    parameters.Add(new SqlParameter("@Nam", int.Parse(cboNam.Text)));
                }

                // Điều kiện quận huyện, xã
                if (cboHuyen.SelectedValue != null && cboHuyen.SelectedValue != DBNull.Value)
                {
                    whereClause += " AND k.MaHuyen = @MaHuyen";
                    parameters.Add(new SqlParameter("@MaHuyen", Convert.ToInt32(cboHuyen.SelectedValue)));
                }

                if (cboXa.SelectedValue != null && cboXa.SelectedValue != DBNull.Value)
                {
                    whereClause += " AND k.MaXa = @MaXa";
                    parameters.Add(new SqlParameter("@MaXa", Convert.ToInt32(cboXa.SelectedValue)));
                }

                string sql = $@"SELECT 
                    COUNT(DISTINCT k.MaKhachHang) as SoKhachHang,
                    SUM(hd.SoDienTieuThu) as TongDienTieuThu,
                    SUM(hd.TongTien) as DoanhThu,
                    COUNT(CASE WHEN hd.TrangThaiThanhToan = N'Đã thanh toán' THEN 1 END) * 100.0 / 
                        NULLIF(COUNT(hd.MaHoaDon), 0) as TyLeDungHan
                    FROM HoaDon hd
                    JOIN HeThongDien ht ON hd.MaHeThong = ht.MaHeThong
                    JOIN KhachHang k ON ht.MaKhachHang = k.MaKhachHang
                    JOIN Tinh t ON k.MaTinh = t.MaTinh
                    WHERE 1=1 {whereClause}";

                DataTable dt = db.getDataTable(sql, parameters.ToArray());

                if (dt.Rows.Count > 0)
                {
                    lblKhachHang.Text = dt.Rows[0]["SoKhachHang"]?.ToString() ?? "0";
                    lblTongKWh.Text = String.Format("{0:N0} kWh", dt.Rows[0]["TongDienTieuThu"] ?? 0);
                    lblDoanhThu.Text = String.Format("{0:N0} VNĐ", dt.Rows[0]["DoanhThu"] ?? 0);
                    lblTiLeDungHan.Text = String.Format("{0:0.##}%", dt.Rows[0]["TyLeDungHan"] ?? 0);

                    LoadBieuDo(whereClause, parameters.ToArray(), cboTieuChi.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadBieuDo(string whereClause, SqlParameter[] parameters, string tieuChi)
        {
            string selectColumn, seriesName;
            switch (tieuChi)
            {
                case "Doanh thu":
                    selectColumn = "SUM(hd.TongTien)";
                    seriesName = "Doanh thu (VNĐ)";
                    break;
                case "Số khách hàng":
                    selectColumn = "COUNT(DISTINCT k.MaKhachHang)";
                    seriesName = "Số khách hàng";
                    break;
                case "Tỷ lệ đúng hạn":
                    selectColumn = "COUNT(CASE WHEN hd.TrangThaiThanhToan = N'Đã thanh toán' THEN 1 END) * 100.0 / COUNT(hd.MaHoaDon)";
                    seriesName = "Tỷ lệ đúng hạn (%)";
                    break;
                default: // Điện tiêu thụ
                    selectColumn = "SUM(hd.SoDienTieuThu)";
                    seriesName = "Điện tiêu thụ (kWh)";
                    break;
            }

            string sql = $@"SELECT 
                t.TenTinh,
                h.TenHuyen,
                {selectColumn} as GiaTri
                FROM HoaDon hd
                JOIN HeThongDien ht ON hd.MaHeThong = ht.MaHeThong
                JOIN KhachHang k ON ht.MaKhachHang = k.MaKhachHang
                JOIN Tinh t ON k.MaTinh = t.MaTinh
                JOIN Huyen h ON k.MaHuyen = h.MaHuyen
                WHERE 1=1 {whereClause}
                GROUP BY t.TenTinh, h.TenHuyen";

            DataTable dt = db.getDataTable(sql, parameters);

            chartThongKe.Series.Clear();
            chartThongKe.Series.Add(seriesName);
            chartThongKe.Series[seriesName].ChartType = SeriesChartType.Column;

            foreach (DataRow row in dt.Rows)
            {
                string khuVuc = row["TenTinh"].ToString() + "-" + row["TenHuyen"].ToString();
                double giaTri = Convert.ToDouble(row["GiaTri"]);
                chartThongKe.Series[seriesName].Points.AddXY(khuVuc, giaTri);
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadThongKe();
        }

        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu đã lọc từ SQL theo tham số đã chọn
                List<SqlParameter> parameters = new List<SqlParameter> {
            new SqlParameter("@Thang", SqlDbType.Int) { Value = cboThang.SelectedIndex > 0 ? cboThang.SelectedIndex : (object)DBNull.Value },
            new SqlParameter("@Nam", SqlDbType.Int) { Value = cboNam.SelectedIndex > 0 ? int.Parse(cboNam.Text) : (object)DBNull.Value },
            new SqlParameter("@MaHuyen", SqlDbType.Int) { Value = cboHuyen.SelectedValue != DBNull.Value ? Convert.ToInt32(cboHuyen.SelectedValue) : (object)DBNull.Value },
            new SqlParameter("@MaXa", SqlDbType.Int) { Value = cboXa.SelectedValue != DBNull.Value ? Convert.ToInt32(cboXa.SelectedValue) : (object)DBNull.Value }
        };

                // Tạo workbook và worksheet
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Thống kê");

                // Định dạng tiêu đề
                worksheet.Cell("A1").Value = "BÁO CÁO THỐNG KÊ ĐIỆN NĂNG";
                worksheet.Range("A1:H1").Merge().Style.Font.Bold = true;
                worksheet.Range("A1:H1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Thêm thông tin lọc
                worksheet.Cell("A3").Value = "Thời gian:";
                worksheet.Cell("B3").Value = $"{cboThang.Text} - {cboNam.Text}";
                worksheet.Cell("A4").Value = "Khu vực:";
                worksheet.Cell("B4").Value = $"{cboHuyen.Text} - {cboXa.Text}";

                // Thêm thông tin tổng hợp
                worksheet.Cell("A6").Value = "Số khách hàng:";
                worksheet.Cell("B6").Value = lblKhachHang.Text;
                worksheet.Cell("A7").Value = "Tổng điện tiêu thụ:";
                worksheet.Cell("B7").Value = lblTongKWh.Text;
                worksheet.Cell("A8").Value = "Doanh thu:";
                worksheet.Cell("B8").Value = lblDoanhThu.Text;
                worksheet.Cell("A9").Value = "Tỷ lệ đúng hạn:";
                worksheet.Cell("B9").Value = lblTiLeDungHan.Text;

                // Thêm dữ liệu chi tiết
                string sql = @"SELECT 
                                k.MaKhachHang,
                                k.TenKhachHang,
                                h.TenHuyen,
                                x.TenXa,
                                k.DiaChiCuThe,
                                SUM(hd.SoDienTieuThu) as TongDienTieuThu,
                                SUM(hd.TongTien) as TongTien,
                                CASE WHEN hd.TrangThaiThanhToan = N'Đã thanh toán' THEN N'Đã thanh toán' 
                                     ELSE N'Chưa thanh toán' END as TrangThai
                                FROM KhachHang k
                                JOIN HeThongDien ht ON k.MaKhachHang = ht.MaKhachHang
                                JOIN HoaDon hd ON ht.MaHeThong = hd.MaHeThong
                                JOIN Huyen h ON k.MaHuyen = h.MaHuyen
                                JOIN Xa x ON k.MaXa = x.MaXa
                                WHERE (@Thang IS NULL OR hd.Thang = @Thang)
                                AND (@Nam IS NULL OR hd.Nam = @Nam)
                                AND (@MaHuyen IS NULL OR k.MaHuyen = @MaHuyen)
                                AND (@MaXa IS NULL OR k.MaXa = @MaXa)
                                GROUP BY k.MaKhachHang, k.TenKhachHang, h.TenHuyen, x.TenXa, k.DiaChiCuThe, hd.TrangThaiThanhToan";

                using (SqlConnection conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                        conn.Open();
                        DataTable dt = new DataTable();
                        new SqlDataAdapter(cmd).Fill(dt);

                        // Thêm tiêu đề bảng
                        var headerRow = worksheet.Row(11);
                        headerRow.Style.Font.Bold = true;
                        worksheet.Cell("A11").Value = "Mã KH";
                        worksheet.Cell("B11").Value = "Tên khách hàng";
                        worksheet.Cell("C11").Value = "Quận/Huyện";
                        worksheet.Cell("D11").Value = "Phường/Xã";
                        worksheet.Cell("E11").Value = "Địa chỉ";
                        worksheet.Cell("F11").Value = "Điện tiêu thụ";
                        worksheet.Cell("G11").Value = "Tổng tiền";
                        worksheet.Cell("H11").Value = "Trạng thái";

                        // Thêm dữ liệu
                        int row = 12;
                        foreach (DataRow dr in dt.Rows)
                        {
                            worksheet.Cell($"A{row}").Value = dr["MaKhachHang"].ToString();
                            worksheet.Cell($"B{row}").Value = dr["TenKhachHang"].ToString();
                            worksheet.Cell($"C{row}").Value = dr["TenHuyen"].ToString();
                            worksheet.Cell($"D{row}").Value = dr["TenXa"].ToString();
                            worksheet.Cell($"E{row}").Value = dr["DiaChiCuThe"].ToString();
                            worksheet.Cell($"F{row}").Value = Convert.ToDouble(dr["TongDienTieuThu"]);
                            worksheet.Cell($"G{row}").Value = Convert.ToDouble(dr["TongTien"]);
                            worksheet.Cell($"H{row}").Value = dr["TrangThai"].ToString();
                            row++;
                        }

                        // Thêm biểu đồ
                        string chartImagePath = $"{Application.StartupPath}\\temp_chart.png";
                        chartThongKe.SaveImage(chartImagePath, ChartImageFormat.Png);
                        var image = worksheet.AddPicture(chartImagePath)
                            .MoveTo(worksheet.Cell($"A{row + 2}"))
                            .WithSize(800, 400);
                        File.Delete(chartImagePath);
                    }
                }

                // Định dạng cột
                worksheet.Columns().AdjustToContents();

                // Lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Lưu báo cáo thống kê"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files|*.pdf",
                    Title = "Lưu báo cáo PDF"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Khởi tạo document với encoding UTF-8
                    Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    document.Open();

                    // Đăng ký font Unicode
                    string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    BaseFont baseFont = BaseFont.CreateFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                    // Tạo các font
                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font boldFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);

                    // Tiêu đề
                    Paragraph title = new Paragraph("BÁO CÁO THỐNG KÊ ĐIỆN NĂNG", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 20f;
                    document.Add(title);

                    // Thông tin thời gian
                    Paragraph timeInfo = new Paragraph();
                    timeInfo.Add(new Chunk("Thời gian: ", boldFont));
                    timeInfo.Add(new Chunk($"{cboThang.Text} - {cboNam.Text}", normalFont));
                    document.Add(timeInfo);

                    // Thông tin khu vực
                    Paragraph areaInfo = new Paragraph();
                    areaInfo.Add(new Chunk("Khu vực: ", boldFont));
                    areaInfo.Add(new Chunk($"{cboHuyen.Text} - {cboXa.Text}", normalFont));
                    document.Add(areaInfo);
                    document.Add(new Paragraph("\n"));

                    // Thông tin thống kê
                    document.Add(new Paragraph("THÔNG TIN TỔNG HỢP", boldFont));

                    // Số khách hàng
                    Paragraph customers = new Paragraph();
                    customers.Add(new Chunk("Số khách hàng: ", boldFont));
                    customers.Add(new Chunk(lblKhachHang.Text, normalFont));
                    document.Add(customers);

                    // Tổng điện tiêu thụ
                    Paragraph power = new Paragraph();
                    power.Add(new Chunk("Tổng điện tiêu thụ: ", boldFont));
                    power.Add(new Chunk(lblTongKWh.Text, normalFont));
                    document.Add(power);

                    // Doanh thu
                    Paragraph revenue = new Paragraph();
                    revenue.Add(new Chunk("Doanh thu: ", boldFont));
                    revenue.Add(new Chunk(lblDoanhThu.Text, normalFont));
                    document.Add(revenue);

                    // Tỷ lệ đúng hạn
                    Paragraph rate = new Paragraph();
                    rate.Add(new Chunk("Tỷ lệ đúng hạn: ", boldFont));
                    rate.Add(new Chunk(lblTiLeDungHan.Text, normalFont));
                    document.Add(rate);

                    document.Add(new Paragraph("\n"));

                    // Thêm biểu đồ
                    string tempPath = Path.Combine(Path.GetTempPath(), "temp_chart.png");
                    chartThongKe.SaveImage(tempPath, ChartImageFormat.Png);

                    using (var imageStream = new FileStream(tempPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var chartImage = iTextSharp.text.Image.GetInstance(imageStream);
                        float maxWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                        float maxHeight = 300f;

                        if (chartImage.Width > maxWidth)
                        {
                            float ratio = maxWidth / chartImage.Width;
                            chartImage.ScalePercent(ratio * 100);
                        }

                        document.Add(chartImage);
                    }

                    // Xóa file tạm sau khi sử dụng
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }

                    document.Close();
                    MessageBox.Show("Xuất PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
