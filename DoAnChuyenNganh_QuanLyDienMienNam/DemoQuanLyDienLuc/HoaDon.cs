using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.IO;
using Font = iTextSharp.text.Font;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

namespace DemoQuanLyDienLuc
{
    public partial class HoaDon : Form
    {

        private DBConnect db = new DBConnect();
        private string maNhanVienHienTai;

        public HoaDon(string maNV)
        {
            InitializeComponent();
            this.maNhanVienHienTai = maNV;
            LoadCboTrangThai();
            LoadDanhSachHoaDon();
        }

        private void LoadCboTrangThai()
        {
            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.Items.Add("Chưa thanh toán");
            cboTrangThai.Items.Add("Đã thanh toán");
            cboTrangThai.SelectedIndex = 0;


            cboThang.Items.Add("Tất cả");
            for (int i = 1; i <= 12; i++)
            {
                cboThang.Items.Add($"Tháng {i}");
            }
            cboThang.SelectedIndex = 0;


            cboNam.Items.Add("Tất cả");
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 5; i <= currentYear; i++)
            {
                cboNam.Items.Add(i.ToString());
            }
            cboNam.SelectedIndex = 0;
        }

        private string GetEmailFromHoaDon(string maHoaDon)
        {
            string query = @"SELECT kh.Email 
                            FROM KhachHang kh
                            JOIN HeThongDien ht ON kh.MaKhachHang = ht.MaKhachHang
                            JOIN HoaDon hd ON ht.MaHeThong = hd.MaHeThong
                            WHERE hd.MaHoaDon = @MaHoaDon";
            SqlParameter[] param = { new SqlParameter("@MaHoaDon", maHoaDon) };
            return db.getScalar(query, param)?.ToString();
        }

        private void LoadDanhSachHoaDon()
        {
            string sql = @"
                SELECT 
                    h.MaHoaDon,
                    k.TenKhachHang,
                    h.ChiSoCu,
                    h.ChiSoMoi,
                    h.SoDienTieuThu,
                    h.TongTien,
                    h.TrangThaiThanhToan,
                    h.Thang,
                    h.Nam
                FROM HoaDon h
                JOIN HeThongDien ht ON h.MaHeThong = ht.MaHeThong
                JOIN KhachHang k ON ht.MaKhachHang = k.MaKhachHang
                WHERE 1=1";

            List<SqlParameter> parameters = new List<SqlParameter>();

            // Thêm điều kiện lọc theo mã hóa đơn
            if (!string.IsNullOrEmpty(txtMaHoaDon.Text.Trim()))
            {
                sql += " AND h.MaHoaDon LIKE @MaHD";
                parameters.Add(new SqlParameter("@MaHD", "%" + txtMaHoaDon.Text.Trim() + "%"));
            }

            // Thêm điều kiện lọc theo trạng thái
            if (cboTrangThai.SelectedIndex > 0)
            {
                sql += " AND h.TrangThaiThanhToan = @TrangThai";
                parameters.Add(new SqlParameter("@TrangThai", cboTrangThai.Text));
            }

            // Thêm điều kiện lọc theo tháng
            if (cboThang.SelectedIndex > 0)
            {
                sql += " AND h.Thang = @Thang";
                parameters.Add(new SqlParameter("@Thang", cboThang.SelectedIndex));
            }

            // Thêm điều kiện lọc theo năm
            if (cboNam.SelectedIndex > 0)
            {
                sql += " AND h.Nam = @Nam";
                parameters.Add(new SqlParameter("@Nam", int.Parse(cboNam.Text)));
            }

            sql += " ORDER BY h.NgayGhiSo DESC";

            dgvHoaDon.DataSource = db.getDataTable(sql, parameters.ToArray());
            FormatDGV();
        }

        private void FormatDGV()
        {
            dgvHoaDon.Columns["MaHoaDon"].HeaderText = "Mã hóa đơn";
            dgvHoaDon.Columns["TenKhachHang"].HeaderText = "Khách hàng";
            dgvHoaDon.Columns["ChiSoCu"].HeaderText = "Chỉ số cũ";
            dgvHoaDon.Columns["ChiSoMoi"].HeaderText = "Chỉ số mới";
            dgvHoaDon.Columns["SoDienTieuThu"].HeaderText = "Tiêu thụ";
            dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng tiền";
            dgvHoaDon.Columns["TrangThaiThanhToan"].HeaderText = "Trạng thái";

            dgvHoaDon.Columns["MaHoaDon"].Width = 100;
            dgvHoaDon.Columns["TenKhachHang"].Width = 150;
            dgvHoaDon.Columns["ChiSoCu"].Width = 70;
            dgvHoaDon.Columns["ChiSoMoi"].Width = 70;
            dgvHoaDon.Columns["SoDienTieuThu"].Width = 50;
            dgvHoaDon.Columns["TongTien"].Width = 50;
            dgvHoaDon.Columns["TrangThaiThanhToan"].Width = 100;

            dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0";
            dgvHoaDon.Columns["Thang"].Visible = false;
            dgvHoaDon.Columns["Nam"].Visible = false;

            dgvHoaDon.ColumnHeadersHeight = 40;
            dgvHoaDon.RowTemplate.Height = 40;

        }

        private void LoadChiTietHoaDon(string maHD)
        {
            // Load thông tin hóa đơn
            string sqlHoaDon = @"
            SELECT 
                h.MaHoaDon,
                k.TenKhachHang,
                k.SoDienThoai,
                k.DiaChiCuThe + ', ' + x.TenXa + ', ' + huyen.TenHuyen + ', ' + t.TenTinh as DiaChi,
                h.ChiSoCu,
                h.ChiSoMoi,
                h.SoDienTieuThu,
                h.TongTien
            FROM HoaDon h
            JOIN HeThongDien ht ON h.MaHeThong = ht.MaHeThong
            JOIN KhachHang k ON ht.MaKhachHang = k.MaKhachHang
            JOIN Xa x ON k.MaXa = x.MaXa
            JOIN Huyen huyen ON x.MaHuyen = huyen.MaHuyen
            JOIN Tinh t ON huyen.MaTinh = t.MaTinh
            WHERE h.MaHoaDon = @MaHD";

            SqlParameter[] parameters = { new SqlParameter("@MaHD", maHD) };
            DataTable dtHoaDon = db.getDataTable(sqlHoaDon, parameters);

            if (dtHoaDon.Rows.Count > 0)
            {
                DataRow dr = dtHoaDon.Rows[0];
                lblMaHoaDon.Text = dr["MaHoaDon"].ToString();
                lblKhachHang.Text = dr["TenKhachHang"].ToString();
                lblDiaChi.Text = dr["DiaChi"].ToString();
                lblSoDienThoai.Text = dr["SoDienThoai"].ToString();
                lblChiSoCu.Text = dr["ChiSoCu"].ToString();
                lblChiSoMoi.Text = dr["ChiSoMoi"].ToString();
                lblTongTien.Text = string.Format("{0:N0}", dr["TongTien"]);

                // Load chi tiết các bậc điện
                string sqlChiTiet = @"
                SELECT 
                    BacDien,
                    SoKwhTieuThu,
                    DonGia,
                    ThanhTien
                FROM ChiTietHoaDon
                WHERE MaHoaDon = @MaHD
                ORDER BY BacDien";

                DataTable dtChiTiet = db.getDataTable(sqlChiTiet, parameters);


                foreach (DataRow row in dtChiTiet.Rows)
                {
                    int bacDien = Convert.ToInt32(row["BacDien"]);
                    decimal thanhTien = Convert.ToDecimal(row["ThanhTien"]);

                    switch (bacDien)
                    {
                        case 1:
                            lblBac1.Text = string.Format("{0:N0}", thanhTien);
                            break;
                        case 2:
                            lblBac2.Text = string.Format("{0:N0}", thanhTien);
                            break;

                    }
                }
            }
        }


        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maHD = dgvHoaDon.Rows[e.RowIndex].Cells["MaHoaDon"].Value.ToString();
                LoadChiTietHoaDon(maHD);
            }
        }

        private void btnThanhToanTienMat_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lblMaHoaDon.Text))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn cần thanh toán!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra trạng thái thanh toán
                string sqlCheck = "SELECT TrangThaiThanhToan FROM HoaDon WHERE MaHoaDon = @MaHD";
                SqlParameter[] param = { new SqlParameter("@MaHD", lblMaHoaDon.Text) };
                DataTable dt = db.getDataTable(sqlCheck, param);

                if (dt.Rows.Count > 0)
                {
                    string trangThai = dt.Rows[0]["TrangThaiThanhToan"].ToString();
                    if (trangThai == "Đã thanh toán")
                    {
                        MessageBox.Show("Hóa đơn này đã được thanh toán!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                ThanhToanHoaDon("Tiền mặt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thanh toán: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ThanhToanHoaDon(string phuongThucThanhToan)
        {
            try
            {
                // 1. Cập nhật trạng thái hóa đơn
                string sqlUpdateHoaDon = @"
            UPDATE HoaDon 
            SET TrangThaiThanhToan = N'Đã thanh toán'
            WHERE MaHoaDon = @MaHD";

                SqlParameter[] paramsHoaDon = {
            new SqlParameter("@MaHD", lblMaHoaDon.Text)
        };

                int ketQua1 = db.getNonQuery(sqlUpdateHoaDon, paramsHoaDon);

                // 2. Thêm giao dịch thanh toán
                string sqlInsertGiaoDich = @"
            INSERT INTO GiaoDichThanhToan (MaHoaDon, NgayThanhToan, SoTienThanhToan, PhuongThucThanhToan)
            VALUES (@MaHD, GETDATE(), @SoTien, @PhuongThuc)";

                decimal soTien = decimal.Parse(lblTongTien.Text.Replace(",", ""));
                SqlParameter[] paramsGiaoDich = {
            new SqlParameter("@MaHD", lblMaHoaDon.Text),
            new SqlParameter("@SoTien", soTien),
            new SqlParameter("@PhuongThuc", phuongThucThanhToan)
        };

                int ketQua2 = db.getNonQuery(sqlInsertGiaoDich, paramsGiaoDich);

                if (ketQua1 > 0 && ketQua2 > 0)
                {
                    string email = GetEmailFromHoaDon(lblMaHoaDon.Text);

                    if (!string.IsNullOrEmpty(email))
                    {
                        string pdfPath = TaoPDFHoaDon(lblMaHoaDon.Text);
                        GuiEmailHoaDon(email, lblMaHoaDon.Text, pdfPath);
                    }

                    MessageBox.Show($"Thanh toán thành công qua {phuongThucThanhToan}!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDanhSachHoaDon();
                    ClearChiTietHoaDon();
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi thanh toán!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void GuiEmailHoaDon(string email, string maHoaDon, string pdfPath)
        {
            try
            {
                // Đọc file PDF thành base64
                byte[] pdfBytes = File.ReadAllBytes(pdfPath);
                string pdfBase64 = Convert.ToBase64String(pdfBytes);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("api-key", "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");

                    var content = new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            sender = new { name = "Điện lực Miền Nam", email = "hoangkhoitl2003@gmail.com" },
                            to = new[] { new { email = email } },
                            subject = $"HÓA ĐƠN TIỀN ĐIỆN - {maHoaDon}",
                            textContent = $"Kính gửi Quý khách hàng,\n\n" +
                                        $"Cảm ơn quý khách đã thanh toán hóa đơn tiền điện. " +
                                        $"Chúng tôi gửi kèm theo đây hóa đơn chi tiết.\n\n" +
                                        $"Trân trọng,\nĐiện lực Miền Nam",
                            attachment = new[]
                            {
                            new
                            {
                                name = $"HoaDon_{maHoaDon}.pdf",
                                content = pdfBase64
                            }
                            }
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await client.PostAsync("https://api.sendinblue.com/v3/smtp/email", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Lỗi gửi email: {errorContent}");
                    }
                    else
                    {
                        MessageBox.Show("Gửi hóa đơn qua email thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi email: {ex.Message}");
            }
            finally
            {
                // Xóa file PDF tạm sau khi gửi
                if (File.Exists(pdfPath))
                    File.Delete(pdfPath);
            }
        }

        private void ClearChiTietHoaDon()
        {
            lblMaHoaDon.Text = "";
            lblKhachHang.Text = "";
            lblDiaChi.Text = "";
            lblSoDienThoai.Text = "";
            lblChiSoCu.Text = "";
            lblChiSoMoi.Text = "";
            lblBac1.Text = "";
            lblBac2.Text = "";
            lblTongTien.Text = "";
        }

        private async void btnMOMO_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lblMaHoaDon.Text))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn cần thanh toán!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra trạng thái thanh toán
                string sqlCheck = "SELECT TrangThaiThanhToan FROM HoaDon WHERE MaHoaDon = @MaHD";
                SqlParameter[] param = { new SqlParameter("@MaHD", lblMaHoaDon.Text) };
                DataTable dt = db.getDataTable(sqlCheck, param);

                if (dt.Rows.Count > 0)
                {
                    string trangThai = dt.Rows[0]["TrangThaiThanhToan"].ToString();
                    if (trangThai == "Đã thanh toán")
                    {
                        MessageBox.Show("Hóa đơn này đã được thanh toán!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                var momoTest = new MomoTest();
                string orderId = $"{lblMaHoaDon.Text}_{DateTime.Now.Ticks}";

                string rawAmount = lblTongTien.Text.Replace(",", "").Replace(".", "").Trim();
                if (!long.TryParse(rawAmount, out long amount))
                {
                    MessageBox.Show("Số tiền không hợp lệ!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string orderInfo = $"Thanh toan hoa don {lblMaHoaDon.Text}";
                string payUrl = await momoTest.CreateTestPayment(orderId, amount, orderInfo);
                System.Diagnostics.Process.Start(payUrl);

                if (MessageBox.Show("Đã hoàn tất thanh toán qua MOMO?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ThanhToanHoaDon("MOMO");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thanh toán MOMO: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string TaoPDFHoaDon(string maHD)
        {
            string pdfPath = Path.Combine(Application.StartupPath, $"HoaDon_{maHD}.pdf");

            using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // Font cho tiếng Việt
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font fontTieuDe = new Font(baseFont, 14);
                Font fontNoiDung = new Font(baseFont, 11L);
                Font fontHeader = new Font(baseFont, 12);

                // Header
                Paragraph header = new Paragraph();
                header.Add(new Chunk("CÔNG TY ĐIỆN LỰC MIỀN NAM\n", fontHeader));
                header.Add(new Chunk("72 Hai Bà Trưng, P. Bến Nghé, Q.1, TP.HCM\n", fontNoiDung));
                header.Add(new Chunk("Điện thoại: (028) 3822 0223\n\n", fontNoiDung));
                header.Alignment = Element.ALIGN_CENTER;
                document.Add(header);

                // Tiêu đề hóa đơn
                Paragraph title = new Paragraph("HÓA ĐƠN TIỀN ĐIỆN", fontTieuDe);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 10f;
                document.Add(title);

                // Số hóa đơn và ngày tháng
                Paragraph invoiceInfo = new Paragraph();
                invoiceInfo.Add(new Chunk($"Số: {lblMaHoaDon.Text}\n", fontHeader));
                invoiceInfo.Add(new Chunk($"Ngày: {DateTime.Now:dd/MM/yyyy}\n\n", fontNoiDung));
                invoiceInfo.Alignment = Element.ALIGN_CENTER;
                document.Add(invoiceInfo);

                // Thông tin khách hàng
                document.Add(new Paragraph("I. THÔNG TIN KHÁCH HÀNG:", fontHeader));
                document.Add(new Paragraph($"Tên khách hàng: {lblKhachHang.Text}", fontNoiDung));
                document.Add(new Paragraph($"Địa chỉ: {lblDiaChi.Text}", fontNoiDung));
                document.Add(new Paragraph($"Điện thoại: {lblSoDienThoai.Text}\n", fontNoiDung));

                // Thông tin chỉ số điện
                document.Add(new Paragraph("II. CHỈ SỐ CÔNG TƠ:", fontHeader));
                document.Add(new Paragraph($"Chỉ số cũ: {lblChiSoCu.Text}", fontNoiDung));
                document.Add(new Paragraph($"Chỉ số mới: {lblChiSoMoi.Text}", fontNoiDung));
                document.Add(new Paragraph($"Điện năng tiêu thụ: {(float.Parse(lblChiSoMoi.Text) - float.Parse(lblChiSoCu.Text))} kWh\n", fontNoiDung));

                // Chi tiết tiền điện
                document.Add(new Paragraph("III. CÁCH TÍNH TIỀN ĐIỆN:", fontHeader));

                if (!string.IsNullOrEmpty(lblBac1.Text))
                {
                    document.Add(new Paragraph("Bậc 1 (0 - 50 kWh):", fontNoiDung));
                    document.Add(new Paragraph($"Số kWh: 50 x 1.806 đ/kWh = {lblBac1.Text} đồng", fontNoiDung));
                }

                if (!string.IsNullOrEmpty(lblBac2.Text))
                {
                    document.Add(new Paragraph("Bậc 2 (51 - 100 kWh):", fontNoiDung));
                    document.Add(new Paragraph($"Số kWh: 50 x 1.866 đ/kWh = {lblBac2.Text} đồng", fontNoiDung));
                }

                document.Add(new Paragraph("\nIV. TỔNG CỘNG:", fontHeader));
                document.Add(new Paragraph($"Tổng tiền: {lblTongTien.Text} đồng", fontHeader));

                // Thông tin thanh toán
                document.Add(new Paragraph("\nV. THÔNG TIN THANH TOÁN:", fontHeader));
                document.Add(new Paragraph("1. Thanh toán qua Internet Banking:", fontNoiDung));
                document.Add(new Paragraph("   - Ngân hàng: TPBANK", fontNoiDung));
                document.Add(new Paragraph("   - Số tài khoản: 09870153703", fontNoiDung));
                document.Add(new Paragraph("   - Đơn vị thụ hưởng: Bùi Hoàng Khôi - Công ty Điện lực Miền Nam", fontNoiDung));
                document.Add(new Paragraph("   - Nội dung: Thanh toán hóa đơn " + lblMaHoaDon.Text, fontNoiDung));

                document.Add(new Paragraph("2. Thanh toán qua ứng dụng:", fontNoiDung));
                document.Add(new Paragraph("   - Momo, VNPay, ZaloPay", fontNoiDung));
                document.Add(new Paragraph("   - Quét mã QR trên hóa đơn\n", fontNoiDung));

                // Chân trang
                Paragraph footer = new Paragraph();
                footer.Add(new Chunk("\nMọi thắc mắc xin liên hệ: ", fontNoiDung));
                footer.Add(new Chunk("Hotline: 1900 1006", fontHeader));
                footer.Alignment = Element.ALIGN_CENTER;
                document.Add(footer);

                document.Close();
            }
            return pdfPath;
        }
        private void btnNganHang_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (string.IsNullOrEmpty(lblMaHoaDon.Text))
            //    {
            //        MessageBox.Show("Vui lòng chọn hóa đơn cần thanh toán!", "Thông báo",
            //            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    //var vnPayTest = new VNPayTest();
            //    string orderId = lblMaHoaDon.Text;

            //    // Xử lý số tiền
            //    string rawAmount = lblTongTien.Text.Replace(",", "").Replace(".", "").Trim();
            //    if (!long.TryParse(rawAmount, out long amount))
            //    {
            //        MessageBox.Show("Số tiền không hợp lệ!", "Lỗi",
            //            MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    string orderInfo = $"Thanh toan hoa don {orderId}";
            //    string paymentUrl = vnPayTest.CreatePaymentUrl(orderId, amount, orderInfo);

            //    // Mở URL trong trình duyệt mặc định
            //    System.Diagnostics.Process.Start(paymentUrl);

            //    // Sau khi thanh toán thành công
            //    if (MessageBox.Show("Đã hoàn tất thanh toán qua VNPay?", "Xác nhận",
            //        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        ThanhToanHoaDon("VNPay");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Lỗi thanh toán VNPay: {ex.Message}", "Lỗi",
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        }
}
