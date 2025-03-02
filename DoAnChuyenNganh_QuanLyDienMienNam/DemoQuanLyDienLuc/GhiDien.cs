﻿using Newtonsoft.Json;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


using System.Windows.Forms;


namespace DemoQuanLyDienLuc
{

    public partial class GhiDien : Form
    {
        private DBConnect db;
        private int currentDongHoIndex = 0;
        private List<string> danhSachDongHo = new List<string>();
        private string selectedMaHeThong = "";
        private string maNhanVienDangNhap;
        private string tenNhanVienDangNhap;
        private bool isInitializing = true;

        public GhiDien(string maNV, string tenNV)
        {
            InitializeComponent();
            isInitializing = true; 
            db = new DBConnect();
            maNhanVienDangNhap = maNV;
            tenNhanVienDangNhap = tenNV;

            LoadThongTinNhanVien();
            cboTinh.SelectedIndexChanged += cboTinh_SelectedIndexChanged;
            cboHuyen.SelectedIndexChanged += cboHuyen_SelectedIndexChanged;
            cboXa.SelectedIndexChanged += cboXa_SelectedIndexChanged;
            cboDongHo.SelectedIndexChanged += cboDongHo_SelectedIndexChanged;

            lblThangNam.Text = $"Tháng {DateTime.Now.Month}/{DateTime.Now.Year}";
            dgvThongTinDH();
            LocHoaDonDien();
            isInitializing = false;
            dgvTienDien.CellClick += dgvTienDien_CellClick;
        }

        private void LoadThongTinNhanVien()
        {
            try
            {
                string query = @"SELECT SoDienThoai 
                           FROM NhanVien 
                           WHERE MaNhanVien = @MaNV";

                SqlParameter[] parameters = {
                new SqlParameter("@MaNV", maNhanVienDangNhap)
            };

                DataTable dt = db.getDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    string soDienThoai = dt.Rows[0]["SoDienThoai"].ToString();
                    lblNhanVienGhiDien.Text = $"Nhân viên: {tenNhanVienDangNhap} - SĐT: {soDienThoai}";

                    LoadDiaChi(maNhanVienDangNhap);
                    LoadHuyen(maNhanVienDangNhap);
                    LoadXa(maNhanVienDangNhap);
                    LoadKhuVucQuanLy(maNhanVienDangNhap);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin nhân viên: {ex.Message}");
            }
        }

        private string GetTrangThaiGhiDien(string maDongHo, int thang, int nam)
        {
            try
            {
                string query = @"SELECT COUNT(*) 
                        FROM HoaDon h 
                        JOIN HeThongDien ht ON h.MaHeThong = ht.MaHeThong
                        WHERE ht.MaDongHo = @MaDongHo 
                        AND h.Thang = @Thang 
                        AND h.Nam = @Nam";

                SqlParameter[] parameters = {
            new SqlParameter("@MaDongHo", maDongHo),
            new SqlParameter("@Thang", thang),
            new SqlParameter("@Nam", nam)
        };

                int count = Convert.ToInt32(db.getScalar(query, parameters));
                return count > 0 ? "✓" : "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void dgvThongTinDH()
        {
            dgvTienDien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTienDien.AutoGenerateColumns = false;
            dgvTienDien.AllowUserToAddRows = false;
            dgvTienDien.ReadOnly = true;

            dgvTienDien.Columns.Clear();

            // Thêm các cột mới
            dgvTienDien.Columns.Add("MaHoaDon", "Mã hóa đơn");
            dgvTienDien.Columns.Add("MaDongHo", "Mã đồng hồ");
            dgvTienDien.Columns.Add("TenKhachHang", "Tên khách hàng");
            dgvTienDien.Columns.Add("Thang", "Tháng");
            dgvTienDien.Columns["Thang"].DataPropertyName = "Thang";
            dgvTienDien.Columns.Add("Nam", "Năm");
            dgvTienDien.Columns["Nam"].DataPropertyName = "Nam";
            dgvTienDien.Columns.Add("ChiSoCu", "Chỉ số cũ");
            dgvTienDien.Columns.Add("ChiSoMoi", "Chỉ số mới");
            dgvTienDien.Columns.Add("SoDienTieuThu", "Số điện tiêu thụ");
            dgvTienDien.Columns.Add("TongTien", "Tổng tiền");
            dgvTienDien.Columns.Add("DiaChi", "Địa chỉ");

           
            dgvTienDien.Columns["MaHoaDon"].Width = 150;
            dgvTienDien.Columns["MaDongHo"].Width = 150;
            dgvTienDien.Columns["TenKhachHang"].Width = 250;
            dgvTienDien.Columns["Thang"].Width = 80;
            dgvTienDien.Columns["Nam"].Width = 80;
            dgvTienDien.Columns["ChiSoCu"].Width = 120;
            dgvTienDien.Columns["ChiSoMoi"].Width = 120;
            dgvTienDien.Columns["SoDienTieuThu"].Width = 150;
            dgvTienDien.Columns["TongTien"].Width = 150;
            dgvTienDien.Columns["DiaChi"].Width = 200;

       
            dgvTienDien.ColumnHeadersHeight = 40;
            dgvTienDien.RowTemplate.Height = 40;

            foreach (DataGridViewColumn col in dgvTienDien.Columns)
            {
                col.DataPropertyName = col.Name;
            }
        }


        private void LoadKhuVucQuanLy(string maNhanVien)
        {
            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetKhuVucQuanLy", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                        DataSet ds = new DataSet();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        // Xử lý ComboBox - Result set 1
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable displayDt = new DataTable();
                            displayDt.Columns.Add("MaDongHo");
                            displayDt.Columns.Add("Display");

                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                string maDongHo = row["MaDongHo"].ToString();
                                string tenKhachHang = row["TenKhachHang"].ToString();
                                string trangThai = GetTrangThaiGhiDien(maDongHo, DateTime.Now.Month, DateTime.Now.Year);

                                string display = $"{maDongHo} - {tenKhachHang} {trangThai}";
                                displayDt.Rows.Add(maDongHo, display);
                            }

                            cboDongHo.DataSource = displayDt;
                            cboDongHo.DisplayMember = "Display";
                            cboDongHo.ValueMember = "MaDongHo";

                            // Cập nhật danh sách đồng hồ cho next/previous
                            danhSachDongHo = ds.Tables[0].AsEnumerable()
                                            .Select(row => row.Field<string>("MaDongHo"))
                                            .ToList();
                            currentDongHoIndex = 0;
                        }

                        // Xử lý DataGridView - Result set 2
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            dgvTienDien.DataSource = ds.Tables[1];

                            // Format các cột số
                            dgvTienDien.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                            dgvTienDien.Columns["SoDienTieuThu"].DefaultCellStyle.Format = "N0";
                        }
                        else
                        {
                            dgvTienDien.DataSource = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải khu vực quản lý: {ex.Message}");
            }
        }


        private void LoadChiSoDien(string maDongHo)
        {
            try
            {
                string query = @"SELECT TOP 1 h.ChiSoCu, h.ChiSoMoi
                   FROM HoaDon h
                   JOIN HeThongDien ht ON h.MaHeThong = ht.MaHeThong
                   WHERE ht.MaDongHo = @MaDongHo
                   AND h.Thang = @Thang 
                   AND h.Nam = @Nam";

                SqlParameter[] parameters = {
            new SqlParameter("@MaDongHo", maDongHo),
            new SqlParameter("@Thang", DateTime.Now.Month),
            new SqlParameter("@Nam", DateTime.Now.Year)
        };

                DataTable dt = db.getDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    // Đã có dữ liệu trong tháng này
                    lblSoCu.Text = $"Chỉ số cũ: {dt.Rows[0]["ChiSoCu"]}";
                    if (dt.Rows[0]["ChiSoMoi"] != DBNull.Value)
                    {
                        lblSoMoi.Text = $"Chỉ số mới: {dt.Rows[0]["ChiSoMoi"]}";
                    }
                    else
                    {
                        lblSoMoi.Text = "Chỉ số mới: ";
                    }
                }
                else
                {
                    // Chưa có dữ liệu trong tháng này - lấy chỉ số cũ từ tháng trước
                    string queryPrevMonth = @"SELECT TOP 1 h.ChiSoMoi
                       FROM HoaDon h
                       JOIN HeThongDien ht ON h.MaHeThong = ht.MaHeThong
                       WHERE ht.MaDongHo = @MaDongHo
                       ORDER BY h.Nam DESC, h.Thang DESC";

                    DataTable dtPrev = db.getDataTable(queryPrevMonth, new SqlParameter[] { new SqlParameter("@MaDongHo", maDongHo) });

                    if (dtPrev.Rows.Count > 0)
                    {
                        lblSoCu.Text = $"Chỉ số cũ: {dtPrev.Rows[0]["ChiSoMoi"]}";
                    }
                    else
                    {
                        lblSoCu.Text = "Chỉ số cũ: 0";
                    }
                    lblSoMoi.Text = "Chỉ số mới: ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load chỉ số điện: {ex.Message}");
            }
        }




        private void LoadDiaChi(string maNhanVien)
        {

            string query = @"SELECT DISTINCT t.MaTinh, t.TenTinh 
                    FROM NhanVien nv 
                    JOIN Tinh t ON nv.MaTinh = t.MaTinh 
                    WHERE nv.MaNhanVien = @MaNhanVien";

            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@MaNhanVien", maNhanVien)
            };

            DataTable dt = db.getDataTable(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                cboTinh.DataSource = dt;
                cboTinh.DisplayMember = "TenTinh";
                cboTinh.ValueMember = "MaTinh";
            }
            else
            {
                cboTinh.DataSource = null;
            }
        }

        private void cboDongHo_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!isInitializing && cboDongHo.SelectedValue != null)
            {
                string maDongHo = cboDongHo.SelectedValue.ToString();
                LoadHoaDon(maDongHo);
                LoadChiSoDien(maDongHo);
            }
        }

        private void LoadHuyen(string maNhanVien)
        {
            string query = @"SELECT DISTINCT h.MaHuyen, h.TenHuyen 
                    FROM NhanVien nv 
                    JOIN Huyen h ON nv.MaHuyen = h.MaHuyen 
                    WHERE nv.MaNhanVien = @MaNhanVien";

            SqlParameter[] parameters = new SqlParameter[]
            {
                   new SqlParameter("@MaNhanVien", maNhanVien)
            };

            DataTable dt = db.getDataTable(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                cboHuyen.DataSource = dt;
                cboHuyen.DisplayMember = "TenHuyen";
                cboHuyen.ValueMember = "MaHuyen";
            }
            else
            {
                cboHuyen.DataSource = null;
            }
        }

        private void LoadXa(string maNhanVien)
        {
            string query = @"SELECT DISTINCT x.MaXa, x.TenXa 
                    FROM NhanVien nv 
                    JOIN Xa x ON nv.MaXa = x.MaXa 
                    WHERE nv.MaNhanVien = @MaNhanVien";

            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@MaNhanVien", maNhanVien)
            };

            DataTable dt = db.getDataTable(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                cboXa.DataSource = dt;
                cboXa.DisplayMember = "TenXa";
                cboXa.ValueMember = "MaXa";
            }
            else
            {
                cboXa.DataSource = null;
            }
        }

        private void LoadDongHo(string maXa)
        {
            try
            {
                string query = @"SELECT dh.MaDongHo, dh.NgaySx, ht.MaHeThong,
                        kh.TenKhachHang, kh.DiaChiCuThe
                        FROM DongHoDien dh
                        JOIN HeThongDien ht ON dh.MaDongHo = ht.MaDongHo
                        JOIN KhachHang kh ON ht.MaKhachHang = kh.MaKhachHang
                        WHERE kh.MaXa = @MaXa
                        ORDER BY kh.TenKhachHang";

                SqlParameter[] parameters = { new SqlParameter("@MaXa", maXa) };
                DataTable dt = db.getDataTable(query, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Tạo DataTable mới với cột hiển thị
                    DataTable displayDt = new DataTable();
                    displayDt.Columns.Add("MaDongHo");
                    displayDt.Columns.Add("Display");
                    displayDt.Columns.Add("TenKhachHang");
                    displayDt.Columns.Add("MaHeThong");

                    foreach (DataRow row in dt.Rows)
                    {
                        string maDongHo = row["MaDongHo"].ToString();
                        string tenKhachHang = row["TenKhachHang"].ToString();
                        string trangThai = GetTrangThaiGhiDien(maDongHo, DateTime.Now.Month, DateTime.Now.Year);

                        // Tạo text hiển thị
                        string display = $"{maDongHo} - {tenKhachHang} {trangThai}";

                        displayDt.Rows.Add(maDongHo, display, tenKhachHang, row["MaHeThong"]);
                    }

                    cboDongHo.DataSource = displayDt;
                    cboDongHo.DisplayMember = "Display";
                    cboDongHo.ValueMember = "MaDongHo";

                    danhSachDongHo = dt.AsEnumerable()
                                      .Select(row => row.Field<string>("MaDongHo"))
                                      .ToList();
                    currentDongHoIndex = 0;

                    if (danhSachDongHo.Count > 0)
                    {
                        LoadHoaDon(danhSachDongHo[currentDongHoIndex]);
                    }
                }
                else
                {
                    cboDongHo.DataSource = null;
                    danhSachDongHo.Clear();
                    currentDongHoIndex = -1;
                    dgvTienDien.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load đồng hồ: {ex.Message}");
            }
        }

        private void LoadHoaDon(string maDongHo)
        {
            try
            {
                string query = @"SELECT h.MaHoaDon, ht.MaDongHo, kh.TenKhachHang, h.Thang, h.Nam, 
                       h.ChiSoCu, h.ChiSoMoi, h.SoDienTieuThu, h.TongTien, 
                       CONCAT(kh.DiaChiCuThe, ', ', x.TenXa, ', ', huyen.TenHuyen, ', ', t.TenTinh) as DiaChi
                       FROM HoaDon h
                       JOIN HeThongDien ht ON h.MaHeThong = ht.MaHeThong
                       JOIN KhachHang kh ON ht.MaKhachHang = kh.MaKhachHang
                       JOIN Xa x ON kh.MaXa = x.MaXa
                       JOIN Huyen huyen ON x.MaHuyen = huyen.MaHuyen
                       JOIN Tinh t ON huyen.MaTinh = t.MaTinh
                       WHERE ht.MaDongHo = @MaDongHo 
                       AND h.Thang = MONTH(GETDATE()) 
                       AND h.Nam = YEAR(GETDATE())";

                SqlParameter[] parameters = {
            new SqlParameter("@MaDongHo", maDongHo)
        };

                DataTable dt = db.getDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    dgvTienDien.DataSource = dt;
                    dgvTienDien.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    dgvTienDien.Columns["SoDienTieuThu"].DefaultCellStyle.Format = "N0";
                }
                else
                {
                    dgvTienDien.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load hóa đơn: {ex.Message}");
            }
        }


        private void dgvTienDien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvTienDien.Rows.Count)
            {
                DataGridViewRow row = dgvTienDien.Rows[e.RowIndex];

                // Kiểm tra null trước khi truy cập giá trị
                var chiSoCu = row.Cells["ChiSoCu"].Value;
                var chiSoMoi = row.Cells["ChiSoMoi"].Value;
                var maDongHo = row.Cells["MaDongHo"].Value;

                if (chiSoCu != null && chiSoMoi != null)
                {
                    lblSoCu.Text = $"Chỉ số cũ: {chiSoCu}";
                    lblSoMoi.Text = $"Chỉ số mới: {chiSoMoi}";
                }

                if (maDongHo != null && cboDongHo.DataSource != null)
                {
                    DataTable dt = (DataTable)cboDongHo.DataSource;
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        if (dtRow["MaDongHo"].ToString() == maDongHo.ToString())
                        {
                            cboDongHo.SelectedValue = maDongHo;
                            break;
                        }
                    }
                }
            }
        }


        private void cboXa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTinh.SelectedValue != null && cboTinh.SelectedValue is string maXa)
            {
                LoadDongHo(maXa);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboDongHo.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn đồng hồ!");
                    return;
                }

                if (string.IsNullOrEmpty(txtChiSoMoi.Text))
                {
                    MessageBox.Show("Vui lòng nhập chỉ số mới!");
                    return;
                }

                // Lấy mã hệ thống 
                string queryHT = @"SELECT ht.MaHeThong 
                         FROM HeThongDien ht 
                         WHERE ht.MaDongHo = @MaDongHo";

                SqlParameter[] paramHT = {
           new SqlParameter("@MaDongHo", cboDongHo.SelectedValue.ToString())
       };

                DataTable dtHT = db.getDataTable(queryHT, paramHT);

                if (dtHT.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hệ thống điện cho đồng hồ này!");
                    return;
                }

                string maHeThong = dtHT.Rows[0]["MaHeThong"].ToString();

                // Kiểm tra đã ghi điện tháng này chưa
                string queryCheckThang = @"SELECT COUNT(*) 
                                FROM HoaDon 
                                WHERE MaHeThong = @MaHeThong 
                                AND Thang = @Thang 
                                AND Nam = @Nam";

                SqlParameter[] paramCheck = {
                   new SqlParameter("@MaHeThong", maHeThong),
                   new SqlParameter("@Thang", DateTime.Now.Month),
                   new SqlParameter("@Nam", DateTime.Now.Year)
       };

                int count = Convert.ToInt32(db.getScalar(queryCheckThang, paramCheck));

                if (count > 0)
                {
                    MessageBox.Show("Bạn đã ghi điện cho tháng này rồi!");
                    return;
                }

                // Kiểm tra chỉ số mới
                if (!int.TryParse(txtChiSoMoi.Text, out int chiSoMoi))
                {
                    MessageBox.Show("Chỉ số mới không hợp lệ!");
                    return;
                }

                // Lấy chỉ số cũ
                string queryChiSoCu = @"SELECT TOP 1 ChiSoMoi as ChiSoCu
                             FROM HoaDon
                             WHERE MaHeThong = @MaHeThong
                             ORDER BY Nam DESC, Thang DESC";

                SqlParameter[] paramChiSoCu = {
                new SqlParameter("@MaHeThong", maHeThong)
       };

                DataTable dtChiSoCu = db.getDataTable(queryChiSoCu, paramChiSoCu);

                int chiSoCu = 0;
                if (dtChiSoCu.Rows.Count > 0)
                {
                    chiSoCu = Convert.ToInt32(dtChiSoCu.Rows[0]["ChiSoCu"]);
                }

                if (chiSoMoi < chiSoCu)
                {
                    MessageBox.Show("Chỉ số không được nhỏ hơn chỉ số cũ!");
                    return;
                }

                // Thêm hóa đơn mới
                string insertQuery = @"INSERT INTO HoaDon (MaHeThong, Thang, Nam, ChiSoCu, ChiSoMoi, 
                            NgayGhiSo, TrangThaiThanhToan)
                            VALUES (@MaHeThong, @Thang, @Nam, @ChiSoCu, @ChiSoMoi,
                            GETDATE(), N'Chưa thanh toán')";

                SqlParameter[] parameters = {
                   new SqlParameter("@MaHeThong", maHeThong),
                   new SqlParameter("@Thang", DateTime.Now.Month),
                   new SqlParameter("@Nam", DateTime.Now.Year),
                   new SqlParameter("@ChiSoCu", chiSoCu),
                   new SqlParameter("@ChiSoMoi", chiSoMoi)
       };

                int result = db.getNonQuery(insertQuery, parameters);

                if (result > 0)
                {
                    // Lấy mã hóa đơn vừa tạo
                    string queryMaHD = "SELECT TOP 1 MaHoaDon FROM HoaDon WHERE MaHeThong = @MaHeThong ORDER BY MaHoaDon DESC";
                    SqlParameter[] paramMaHD = { new SqlParameter("@MaHeThong", maHeThong) };
                    string maHoaDon = db.getScalar(queryMaHD, paramMaHD).ToString();

                    // Lấy email khách hàng
                    string email = GetEmailFromHoaDon(maHoaDon);

                    if (!string.IsNullOrEmpty(email))
                    {
                        // Gửi email thông báo
                        string noiDung = GetThongTinChiTiet(maHoaDon);
                        if (!string.IsNullOrEmpty(noiDung))
                        {
                            GuiEmailThongBao(email, maHoaDon, noiDung);
                        }
                    }

                    MessageBox.Show("Thêm chỉ số điện và gửi thông báo thành công!");
                    LoadKhuVucQuanLy(maNhanVienDangNhap);
                    txtChiSoMoi.Clear();
                }
                else
                {
                    MessageBox.Show("Thêm chỉ số điện thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            if (danhSachDongHo.Count == 0) return;

            currentDongHoIndex++;
            if (currentDongHoIndex >= danhSachDongHo.Count)
                currentDongHoIndex = 0;

            string selectedMaDongHo = danhSachDongHo[currentDongHoIndex];
            cboDongHo.SelectedValue = selectedMaDongHo;
            LoadHoaDon(selectedMaDongHo);
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (danhSachDongHo.Count == 0) return;

            currentDongHoIndex--;
            if (currentDongHoIndex < 0)
                currentDongHoIndex = danhSachDongHo.Count - 1;


            string selectedMaDongHo = danhSachDongHo[currentDongHoIndex];
            cboDongHo.SelectedValue = selectedMaDongHo;
            LoadHoaDon(selectedMaDongHo);
        }

        private void cboTinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (dgvTienDien.CurrentRow != null)
            {
                string maHoaDon = dgvTienDien.CurrentRow.Cells["MaHoaDon"].Value.ToString();
                string maDongHo = dgvTienDien.CurrentRow.Cells["MaDongHo"].Value.ToString();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF file (*.pdf)|*.pdf";
                sfd.FileName = $"PhieuDien_{maHoaDon}.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //PhieuDienPDF xuatPhieu = new PhieuDienPDF();
                    //xuatPhieu.XuatPhieuDien(maHoaDon, maDongHo, sfd.FileName);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xuất phiếu!");
            }
        }

        private void GhiDien_Load(object sender, EventArgs e)
        {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"SELECT h.MaHoaDon, ht.MaDongHo, kh.TenKhachHang, h.Thang, h.Nam, 
               h.ChiSoCu, h.ChiSoMoi, h.SoDienTieuThu, h.TongTien, 
               CONCAT(kh.DiaChiCuThe, ', ', x.TenXa, ', ', huyen.TenHuyen, ', ', t.TenTinh) as DiaChi
               FROM HoaDon h
               JOIN HeThongDien ht ON h.MaHeThong = ht.MaHeThong
               JOIN KhachHang kh ON ht.MaKhachHang = kh.MaKhachHang
               JOIN Xa x ON kh.MaXa = x.MaXa
               JOIN Huyen huyen ON x.MaHuyen = huyen.MaHuyen
               JOIN Tinh t ON huyen.MaTinh = t.MaTinh
               WHERE 1=1";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(txtTimKhachHang.Text))
                {
                    query += " AND kh.TenKhachHang LIKE @TenKH";
                    parameters.Add(new SqlParameter("@TenKH", "%" + txtTimKhachHang.Text + "%"));
                }

                if (cboThangLoc.SelectedIndex > 0)
                {
                    query += " AND h.Thang = @Thang";
                    parameters.Add(new SqlParameter("@Thang", cboThangLoc.SelectedIndex));
                }

                if (cboNamLoc.SelectedIndex > 0)
                {
                    query += " AND h.Nam = @Nam";
                    parameters.Add(new SqlParameter("@Nam", int.Parse(cboNamLoc.SelectedItem.ToString().Substring(4))));
                }

                DataTable dt = db.getDataTable(query, parameters.ToArray());
                dgvTienDien.DataSource = dt;
                dgvTienDien.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                dgvTienDien.Columns["SoDienTieuThu"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LocHoaDonDien()
        {
            // Load ComboBox Tháng
            cboThangLoc.Items.Clear();
            cboThangLoc.Items.Add("-- Chọn tháng --");
            for (int i = 1; i <= 12; i++)
            {
                cboThangLoc.Items.Add($"Tháng {i}");
            }
            cboThangLoc.SelectedIndex = 0;

            // Load ComboBox Năm
            cboNamLoc.Items.Clear();
            cboNamLoc.Items.Add("-- Chọn năm --");
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 5; i <= currentYear; i++)
            {
                cboNamLoc.Items.Add($"Năm {i}");
            }
            cboNamLoc.SelectedIndex = 0;


            cboThangLoc.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            cboNamLoc.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            btnTimKiem.Click += btnTimKiem_Click;
        }

        private void lblSoCu_Click(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboThangLoc.SelectedIndex = 0;
            cboNamLoc.SelectedIndex = 0;
            txtTimKhachHang.Clear();
            if (cboDongHo.SelectedValue != null)
            {
                LoadHoaDon(cboDongHo.SelectedValue.ToString());
            }
        }

        private void btnGuiThongBao_Click(object sender, EventArgs e)
        {
            if (dgvTienDien.CurrentRow != null)
            {
                string maHoaDon = dgvTienDien.CurrentRow.Cells["MaHoaDon"].Value.ToString();
                string email = GetEmailFromHoaDon(maHoaDon);

                if (!string.IsNullOrEmpty(email))
                {
                    string noiDung = GetThongTinChiTiet(maHoaDon);
                    if (!string.IsNullOrEmpty(noiDung))
                    {
                        GuiEmailThongBao(email, maHoaDon, noiDung);
                    }
                    else
                    {
                        MessageBox.Show("Không thể lấy thông tin chi tiết hóa đơn!");
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy email khách hàng!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần gửi thông báo!");
            }
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

        private void GuiEmailThongBao(string email, string maHoaDon, string noiDung)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("api-key", "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");

                    var content = new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            sender = new { name = "Điện lực Miền Nam", email = "hoangkhoitl2003@gmail.com" },
                            to = new[] { new { email = email } },
                            subject = $"THÔNG BÁO TIỀN ĐIỆN - {maHoaDon}",
                            textContent = noiDung
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = client.PostAsync("https://api.sendinblue.com/v3/smtp/email", content).Result;

                    MessageBox.Show(response.IsSuccessStatusCode ?
                        "Gửi thông báo thành công!" :
                        $"Lỗi: {response.Content.ReadAsStringAsync().Result}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private string GetThongTinChiTiet(string maHoaDon)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetThongTinHoaDon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow r = ds.Tables[0].Rows[0];
                        StringBuilder noiDung = new StringBuilder();

                        noiDung.AppendLine("THÔNG BÁO TIỀN ĐIỆN");
                        noiDung.AppendLine("-----------------------------------");
                        noiDung.AppendLine($"Kính gửi: {r["TenKhachHang"]}");
                        noiDung.AppendLine();
                        noiDung.AppendLine("THÔNG TIN HÓA ĐƠN TIỀN ĐIỆN:");
                        noiDung.AppendLine($"Mã Khách hàng: {r["MaKhachHang"]}");
                        noiDung.AppendLine($"Địa chỉ: {r["DiaChiCuThe"]}, {r["TenXa"]}, {r["TenHuyen"]}, {r["TenTinh"]}");
                        noiDung.AppendLine($"Kỳ hóa đơn: Tháng {r["Thang"]}/{r["Nam"]}");
                        noiDung.AppendLine();
                        noiDung.AppendLine("CHỈ SỐ CÔNG TƠ:");
                        noiDung.AppendLine($"- Chỉ số cũ: {r["ChiSoCu"]:N0}");
                        noiDung.AppendLine($"- Chỉ số mới: {r["ChiSoMoi"]:N0}");
                        noiDung.AppendLine($"- Điện năng tiêu thụ: {r["SoDienTieuThu"]:N0} kWh");
                        noiDung.AppendLine();
                        noiDung.AppendLine("CHI TIẾT TIỀN ĐIỆN:");

                        // Thêm chi tiết các bậc điện
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow detail in ds.Tables[1].Rows)
                            {
                                noiDung.AppendLine($"Bậc {detail["BacDien"]}: {detail["SoKwhTieuThu"]:N0} kWh x {detail["DonGia"]:N0} đ = {detail["ThanhTien"]:N0} đ");
                            }
                        }

                        noiDung.AppendLine();
                        noiDung.AppendLine($"TỔNG CỘNG: {string.Format("{0:N0}", r["TongTien"])} VNĐ");
                        noiDung.AppendLine();
                        noiDung.AppendLine("Quý khách vui lòng thanh toán trước ngày " +
                            DateTime.Parse(r["NgayGhiSo"].ToString()).AddDays(14).ToString("dd/MM/yyyy"));
                        noiDung.AppendLine();
                        noiDung.AppendLine("Hình thức thanh toán:");
                        noiDung.AppendLine("1. Thanh toán trực tuyến qua ứng dụng EVNSPC");
                        noiDung.AppendLine("2. Thanh toán tại các điểm thu hộ");
                        noiDung.AppendLine("3. Chuyển khoản qua ngân hàng");
                        noiDung.AppendLine();
                        noiDung.AppendLine("Mọi thắc mắc xin liên hệ:");
                        noiDung.AppendLine("- Hotline: 19009000");
                        noiDung.AppendLine("- Email: cskh@evnspc.vn");
                        noiDung.AppendLine();
                        noiDung.AppendLine("Trân trọng cảm ơn quý khách!");

                        return noiDung.ToString();
                    }
                    return null;
                }
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
