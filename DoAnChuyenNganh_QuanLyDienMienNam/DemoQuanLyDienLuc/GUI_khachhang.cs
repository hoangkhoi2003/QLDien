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

namespace DemoQuanLyDienLuc
{
    public partial class GUI_khachhang : Form
    {
        private string TenKhachHang;
        private string MaKhachHang;
        private string MaHeThong;
        private string Role;
        private DBConnect db = new DBConnect();

        public GUI_khachhang(string tenKhachHang, string maKhachHang, string maHeThong, string role)
        {
            InitializeComponent();

            this.TenKhachHang = tenKhachHang;
            this.MaKhachHang = maKhachHang;
            this.MaHeThong = maHeThong;
            this.Role = role;
            LoadChiTietHoaDon(MaHeThong);
        }

        private void GUI_khachhang_Load(object sender, EventArgs e)
        {
            lblTenKhachHang.Text = $"Xin Chào! {TenKhachHang}";
            timer1.Start();
            timer1.Tick += Timer1_Tick;
            LoadComboBoxes();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        private void UpdateClock()
        {
            lblClock.Text = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
        }
        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                Application.Exit();
            }
        }

        private void LoadChiTietHoaDon(string maHeThong)
        {
            // Lấy thông tin tháng và năm hiện tại
            int thangHienTai = DateTime.Now.Month;
            int namHienTai = DateTime.Now.Year;

            string sqlHoaDon = @"
                            SELECT 
                                h.MaHoaDon,
                                k.TenKhachHang,
                                k.SoDienThoai,
                                k.DiaChiCuThe + ', ' + x.TenXa + ', ' + huyen.TenHuyen + ', ' + t.TenTinh as DiaChi,
                                h.ChiSoCu,
                                h.ChiSoMoi,
                                h.SoDienTieuThu,
                                h.TongTien,
                                h.Thang,
                                h.Nam
                            FROM HoaDon h
                            JOIN HeThongDien ht ON h.MaHeThong = ht.MaHeThong
                            JOIN KhachHang k ON ht.MaKhachHang = k.MaKhachHang
                            JOIN Xa x ON k.MaXa = x.MaXa
                            JOIN Huyen huyen ON x.MaHuyen = huyen.MaHuyen
                            JOIN Tinh t ON huyen.MaTinh = t.MaTinh
                            WHERE ht.MaHeThong = @MaHeThong AND h.Thang = @Thang AND h.Nam = @Nam";

            SqlParameter[] parameters = {
                        new SqlParameter("@MaHeThong", maHeThong),
                        new SqlParameter("@Thang", thangHienTai),
                        new SqlParameter("@Nam", namHienTai)
                    };

            DataTable dtHoaDon = db.getDataTable(sqlHoaDon, parameters);

            if (dtHoaDon.Rows.Count > 0)
            {
                DataRow dr = dtHoaDon.Rows[0];
                lblMaHoaDon.Text = dr["MaHoaDon"] != DBNull.Value ? dr["MaHoaDon"].ToString() : "Không có dữ liệu";
                LblKhachhang.Text = dr["TenKhachHang"] != DBNull.Value ? dr["TenKhachHang"].ToString() : "Không có dữ liệu";
                lblDiaChi.Text = dr["DiaChi"] != DBNull.Value ? dr["DiaChi"].ToString() : "Không có dữ liệu";
                lblSoDienThoai.Text = dr["SoDienThoai"] != DBNull.Value ? dr["SoDienThoai"].ToString() : "Không có dữ liệu";
                lblChiSoCu.Text = dr["ChiSoCu"] != DBNull.Value ? dr["ChiSoCu"].ToString() : "Không có dữ liệu";
                lblChiSoMoi.Text = dr["ChiSoMoi"] != DBNull.Value ? dr["ChiSoMoi"].ToString() : "Không có dữ liệu";

                lblKWSD.Text = dr["SoDienTieuThu"].ToString();
                lblTongTien.Text = string.Format("{0:N0}", dr["TongTien"]);

                // Load chi tiết các bậc điện liên quan đến mã hóa đơn
                string sqlChiTiet = @"
                            SELECT 
                                BacDien,
                                SoKwhTieuThu,
                                DonGia,
                                ThanhTien
                            FROM ChiTietHoaDon
                            WHERE MaHoaDon = @MaHoaDon
                            ORDER BY BacDien";

                SqlParameter[] chiTietParameters = { new SqlParameter("@MaHoaDon", dr["MaHoaDon"].ToString()) };
                DataTable dtChiTiet = db.getDataTable(sqlChiTiet, chiTietParameters);

                foreach (DataRow row in dtChiTiet.Rows)
                {
                    int bacDien = Convert.ToInt32(row["BacDien"]);
                    decimal thanhTien = Convert.ToDecimal(row["ThanhTien"]);
                    int kwh = Convert.ToInt32(row["SoKwhTieuThu"]);
                    decimal dongia = Convert.ToDecimal(row["DonGia"]);
                    switch (bacDien)
                    {
                        case 1:
                            lblKW1.Text = string.Format("{0:N0}", kwh);
                            lblDonGia1.Text = string.Format("{0:N0}", dongia);
                            lblThanhTien1.Text = string.Format("{0:N0}", thanhTien);
                            break;
                        case 2:
                            lblKW2.Text = string.Format("{0:N0}", kwh);
                            lblDonGia2.Text = string.Format("{0:N0}", dongia);
                            lblThanhTien2.Text = string.Format("{0:N0}", thanhTien);
                            break;
                        case 3:
                            lblKw3.Text = string.Format("{0:N0}", kwh);
                            lblDonGia3.Text = string.Format("{0:N0}", dongia);
                            lblThanhTien3.Text = string.Format("{0:N0}", thanhTien);
                            break;

                        default:
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadComboBoxes()
        {
            // Load dữ liệu cho ComboBox trạng thái thanh toán
           
            // Load dữ liệu cho ComboBox tháng
            cmbThang.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                cmbThang.Items.Add(i.ToString("D2")); // Thêm tháng từ 01 đến 12
            }
            cmbThang.SelectedIndex = DateTime.Now.Month - 1; // Mặc định là tháng hiện tại

            // Load dữ liệu cho ComboBox năm
            cmbNam.Items.Clear();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 5; i <= currentYear; i++) // Hiển thị từ năm hiện tại đến 5 năm trước
            {
                cmbNam.Items.Add(i.ToString());
            }
            cmbNam.SelectedIndex = cmbNam.Items.IndexOf(currentYear.ToString()); // Mặc định là năm hiện tại
        }


        private void btnLoc_Click(object sender, EventArgs e)
        {

            // Lấy thông tin từ các ComboBox
            int thang = cmbThang.SelectedItem != null ? int.Parse(cmbThang.SelectedItem.ToString()) : 0;
            int nam = cmbNam.SelectedItem != null ? int.Parse(cmbNam.SelectedItem.ToString()) : 0;

            // Tạo danh sách điều kiện và tham số SQL
            List<string> conditions = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            // Thêm MaHeThong vào danh sách tham số
            parameters.Add(new SqlParameter("@MaHeThong", MaHeThong));

            // Thêm điều kiện lọc theo tháng và năm nếu có
            string whereClause = "";

            if (thang > 0)
            {
                whereClause += " AND h.Thang = @Thang";
                parameters.Add(new SqlParameter("@Thang", thang));
            }

            if (nam > 0)
            {
                whereClause += " AND h.Nam = @Nam";
                parameters.Add(new SqlParameter("@Nam", nam));
            }

         
         

            // Câu lệnh SQL
            string sqlHoaDon = $@"
                    SELECT 
                        h.MaHoaDon,
                        k.TenKhachHang,
                        k.SoDienThoai,
                        k.DiaChiCuThe + ', ' + x.TenXa + ', ' + huyen.TenHuyen + ', ' + t.TenTinh AS DiaChi,
                        h.ChiSoCu,
                        h.ChiSoMoi,
                        h.SoDienTieuThu,
                        h.TongTien,
                        h.Thang,
                        h.Nam,
                        h.TrangThaiThanhToan
                    FROM HoaDon h
                    JOIN HeThongDien ht ON h.MaHeThong = ht.MaHeThong
                    JOIN KhachHang k ON ht.MaKhachHang = k.MaKhachHang
                    JOIN Xa x ON k.MaXa = x.MaXa
                    JOIN Huyen huyen ON x.MaHuyen = huyen.MaHuyen
                    JOIN Tinh t ON huyen.MaTinh = t.MaTinh
                    WHERE ht.MaHeThong = @MaHeThong {whereClause}";

            // Thực thi câu lệnh SQL
            DataTable dtHoaDon = db.getDataTable(sqlHoaDon, parameters.ToArray());

            if (dtHoaDon.Rows.Count > 0)
            {
                DataRow dr = dtHoaDon.Rows[0];

                lblMaHoaDon.Text = dr["MaHoaDon"] != DBNull.Value ? dr["MaHoaDon"].ToString() : "Không có dữ liệu";
                LblKhachhang.Text = dr["TenKhachHang"] != DBNull.Value ? dr["TenKhachHang"].ToString() : "Không có dữ liệu";
                lblDiaChi.Text = dr["DiaChi"] != DBNull.Value ? dr["DiaChi"].ToString() : "Không có dữ liệu";
                lblSoDienThoai.Text = dr["SoDienThoai"] != DBNull.Value ? dr["SoDienThoai"].ToString() : "Không có dữ liệu";
                lblChiSoCu.Text = dr["ChiSoCu"] != DBNull.Value ? dr["ChiSoCu"].ToString() : "Không có dữ liệu";
                lblChiSoMoi.Text = dr["ChiSoMoi"] != DBNull.Value ? dr["ChiSoMoi"].ToString() : "Không có dữ liệu";
                lblKWSD.Text = dr["SoDienTieuThu"].ToString();
                lblTongTien.Text = string.Format("{0:N0}", dr["TongTien"]);

                // Lấy chi tiết hóa đơn
                string sqlChiTiet = @"
                        SELECT 
                            BacDien,
                            SoKwhTieuThu,
                            DonGia,
                            ThanhTien
                        FROM ChiTietHoaDon
                        WHERE MaHoaDon = @MaHoaDon
                        ORDER BY BacDien";

                SqlParameter[] chiTietParameters = { new SqlParameter("@MaHoaDon", dr["MaHoaDon"].ToString()) };
                DataTable dtChiTiet = db.getDataTable(sqlChiTiet, chiTietParameters);

                // Duyệt qua từng dòng chi tiết hóa đơn và hiển thị
                foreach (DataRow row in dtChiTiet.Rows)
                {
                    int bacDien = Convert.ToInt32(row["BacDien"]);
                    decimal thanhTien = Convert.ToDecimal(row["ThanhTien"]);
                    int kwh = Convert.ToInt32(row["SoKwhTieuThu"]);
                    decimal dongia = Convert.ToDecimal(row["DonGia"]);

                    switch (bacDien)
                    {
                        case 1:
                            lblKW1.Text = string.Format("{0:N0}", kwh);
                            lblDonGia1.Text = string.Format("{0:N0}", dongia);
                            lblThanhTien1.Text = string.Format("{0:N0}", thanhTien);
                            break;
                        case 2:
                            lblKW2.Text = string.Format("{0:N0}", kwh);
                            lblDonGia2.Text = string.Format("{0:N0}", dongia);
                            lblThanhTien2.Text = string.Format("{0:N0}", thanhTien);
                            break;
                        case 3:
                            lblKw3.Text = string.Format("{0:N0}", kwh);
                            lblDonGia3.Text = string.Format("{0:N0}", dongia);
                            lblThanhTien3.Text = string.Format("{0:N0}", thanhTien);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

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

                int kq1 = db.getNonQuery(sqlUpdateHoaDon, paramsHoaDon);

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

                int kq2 = db.getNonQuery(sqlInsertGiaoDich, paramsGiaoDich);

                if (kq1 > 0 && kq2 > 0)
                {
                    MessageBox.Show($"Thanh toán thành công qua {phuongThucThanhToan}!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                   
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
    }
}
