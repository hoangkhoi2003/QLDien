using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoQuanLyDienLuc
{
    public partial class XuLySuCo : Form
    {
        private DBConnect db = new DBConnect();
        private byte[] fileData = null;
        private string fileName = "";

        public XuLySuCo()
        {
            InitializeComponent();
            LoadDataToGrid();
            LoadLoaiSuCo();
            LoadTrangThai();
            LoadNhanVienKyThuat();
        }

        private void LoadLoaiSuCo()
        {
            // Thêm các loại sự cố vào ComboBox
            cboLoaiSuCo.Items.Clear();
            cboLoaiSuCo.Items.AddRange(new string[] {
                "Mất điện",
                "Điện yếu",
                "Sự cố đường dây",
                "Sự cố trạm biến áp",
                "Khác"
            });
        }

        private void LoadTrangThai()
        {
            
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new string[] {
                "Chờ xử lý",
                "Đang xử lý",
                "Đã xử lý"
             });
        }

        private void LoadDataToGrid()
        {
            try
            {
               
                string sql = @"SELECT 
                    xs.MaSuCo, 
                    xs.MaKhachHang, 
                    kh.TenKhachHang, 
                    xs.LoaiSuCo, 
                    xs.NgayBaoCao, 
                    xs.NgayXuLy, 
                    xs.TrangThai,
                    nv.MaNhanVien,  -- Thêm MaNhanVien
                    nv.TenNhanVien as NhanVienXuLy,  
                    CASE WHEN xs.BienBan IS NULL THEN N'Chưa có' 
                         ELSE N'Đã đính kèm' END as TrangThaiBienBan
                    FROM XuLySuCo xs
                    JOIN KhachHang kh ON xs.MaKhachHang = kh.MaKhachHang
                    LEFT JOIN NhanVien nv ON xs.MaNhanVien = nv.MaNhanVien  -- Đảm bảo join đúng
                    ORDER BY xs.NgayBaoCao DESC";

                DataTable dt = db.getDataTable(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dgvDanhSach.DataSource = dt;
                    
                    dgvDanhSach.Columns["MaSuCo"].HeaderText = "Mã sự cố";
                    dgvDanhSach.Columns["MaKhachHang"].HeaderText = "Mã KH";
                    dgvDanhSach.Columns["TenKhachHang"].HeaderText = "Tên khách hàng";
                    dgvDanhSach.Columns["LoaiSuCo"].HeaderText = "Loại sự cố";
                    dgvDanhSach.Columns["NgayBaoCao"].HeaderText = "Ngày báo cáo";
                    dgvDanhSach.Columns["NgayXuLy"].HeaderText = "Ngày xử lý";
                    dgvDanhSach.Columns["TrangThai"].HeaderText = "Trạng thái";
                    dgvDanhSach.Columns["NhanVienXuLy"].HeaderText = "Nhân viên xử lý";
                    dgvDanhSach.Columns["TrangThaiBienBan"].HeaderText = "Biên bản";

                    
                    if (dgvDanhSach.Columns.Contains("MaNhanVien"))
                    {
                        dgvDanhSach.Columns["MaNhanVien"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNhanVienKyThuat()
        {
            try
            {
                string sql = @"SELECT MaNhanVien, TenNhanVien 
                      FROM NhanVien nv 
                      JOIN TaiKhoan tk ON nv.MaTaiKhoan = tk.MaTaiKhoan 
                      WHERE tk.ChucVu = N'Nhân viên kỹ thuật'";

                DataTable dt = db.getDataTable(sql);
                cboNhanVien.DataSource = dt;
                cboNhanVien.DisplayMember = "TenNhanVien";
                cboNhanVien.ValueMember = "MaNhanVien";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load nhân viên: " + ex.Message);
            }
        }



        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiem.Text)) return;

            string sql = @"SELECT xs.MaSuCo, xs.MaKhachHang, kh.TenKhachHang, 
                          xs.LoaiSuCo, xs.NgayBaoCao, xs.NgayXuLy, xs.TrangThai,
                          CASE WHEN xs.FileDinhKem IS NULL THEN N'Chưa có' ELSE N'Đã đính kèm' END as TrangThaiFile
                          FROM XuLySuCo xs
                          JOIN KhachHang kh ON xs.MaKhachHang = kh.MaKhachHang
                          WHERE kh.TenKhachHang LIKE @Search OR kh.MaKhachHang LIKE @Search";

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Search", "%" + txtTimKiem.Text + "%")
            };

            dgvDanhSach.DataSource = db.getDataTable(sql, parameters);

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKH.Text) || cboLoaiSuCo.SelectedIndex == -1 || cboTrangThai.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra nếu trạng thái là "Đang xử lý" thì phải chọn nhân viên
            if (cboTrangThai.Text == "Đang xử lý" && cboNhanVien.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhân viên xử lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"INSERT INTO XuLySuCo (MaKhachHang, LoaiSuCo, NgayBaoCao, TrangThai, MaNhanVien) 
                  VALUES (@MaKH, @LoaiSuCo, @NgayBao, @TrangThai, @MaNhanVien)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaKH", txtMaKH.Text),
                new SqlParameter("@LoaiSuCo", cboLoaiSuCo.Text),
                new SqlParameter("@NgayBao", DateTime.Now),
                new SqlParameter("@TrangThai", cboTrangThai.Text),
                new SqlParameter("@MaNhanVien", (cboTrangThai.Text == "Đang xử lý") ? cboNhanVien.SelectedValue : DBNull.Value)
            };

            try
            {
                int result = db.getNonQuery(sql, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Thêm sự cố thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataToGrid();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sự cố cần cập nhật!");
                return;
            }

            if (cboNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên xử lý!");
                return;
            }

            string sql = @"UPDATE XuLySuCo 
                  SET TrangThai = @TrangThai,
                      MaNhanVien = @MaNhanVien,
                      NgayXuLy = CASE WHEN @TrangThai = N'Đã xử lý' THEN GETDATE() ELSE NgayXuLy END
                  WHERE MaSuCo = @MaSuCo";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TrangThai", cboTrangThai.Text),
                new SqlParameter("@MaNhanVien", cboNhanVien.SelectedValue),
                new SqlParameter("@MaSuCo", dgvDanhSach.SelectedRows[0].Cells["MaSuCo"].Value)
            };

            try
            {
                int result = db.getNonQuery(sql, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    LoadDataToGrid();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];
                    txtMaKH.Text = row.Cells["MaKhachHang"].Value.ToString();
                    LoadTenKhachHang(txtMaKH.Text);
                    cboLoaiSuCo.Text = row.Cells["LoaiSuCo"].Value.ToString();
                    cboTrangThai.Text = row.Cells["TrangThai"].Value.ToString();

                    if (row.Cells["MaNhanVien"].Value != DBNull.Value)
                    {
                        cboNhanVien.SelectedValue = row.Cells["MaNhanVien"].Value.ToString();
                    }
                    else
                    {
                        cboNhanVien.SelectedIndex = -1;
                    }

                    // Thêm load biên bản ảnh
                    string maSuCo = row.Cells["MaSuCo"].Value.ToString();
                    try
                    {
                        string query = "SELECT BienBan FROM XuLySuCo WHERE MaSuCo = @MaSuCo";
                        SqlParameter[] parameters = { new SqlParameter("@MaSuCo", maSuCo) };
                        using (SqlConnection conn = db.GetConnection())
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@MaSuCo", maSuCo);
                                byte[] imageData = cmd.ExecuteScalar() as byte[];

                                if (imageData != null)
                                {
                                    using (MemoryStream ms = new MemoryStream(imageData))
                                    {
                                        picHinhAnh.Image = Image.FromStream(ms);
                                        picHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
                                    }
                                }
                                else
                                {
                                    picHinhAnh.Image = null;
                                }
                            }
                        }
                    }
                    catch
                    {
                        picHinhAnh.Image = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBienBan(string maSuCo)
        {
            if (dgvDanhSach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sự cố cần upload biên bản!");
                return;
            }

            if (cboTrangThai.Text != "Đã xử lý")
            {
                MessageBox.Show("Chỉ được upload biên bản khi sự cố đã xử lý!");
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png";
                ofd.Title = "Chọn hình ảnh biên bản";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Đọc file hình ảnh
                        byte[] imageData = File.ReadAllBytes(ofd.FileName);

                        // Kiểm tra kích thước file
                        if (imageData.Length > 5242880) // 5MB
                        {
                            MessageBox.Show("Kích thước file không được vượt quá 5MB!");
                            return;
                        }

                        string sql = @"UPDATE XuLySuCo 
                            SET BienBan = @BienBan
                            WHERE MaSuCo = @MaSuCo";

                        SqlParameter[] parameters = {
                    new SqlParameter("@BienBan", imageData),
                    new SqlParameter("@MaSuCo", dgvDanhSach.SelectedRows[0].Cells["MaSuCo"].Value)
                };

                        int result = db.getNonQuery(sql, parameters);
                        if (result > 0)
                        {
                            MessageBox.Show("Upload biên bản thành công!");
                            // Hiển thị hình ảnh vừa upload
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                picHinhAnh.Image = Image.FromStream(ms);
                                picHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                            LoadDataToGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi upload hình ảnh: " + ex.Message);
                    }
                }
            }
        }

        private void LoadTenKhachHang(string maKH)
        {
            string sql = "SELECT TenKhachHang FROM KhachHang WHERE MaKhachHang = @MaKH";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@MaKH", maKH)
            };

            object result = db.getScalar(sql, parameters);
            if (result != null)
                txtTenKH.Text = result.ToString();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDanhSach.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn sự cố cần upload biên bản!");
                    return;
                }

                string trangThai = dgvDanhSach.SelectedRows[0].Cells["TrangThai"].Value.ToString();
                if (trangThai != "Đã xử lý")
                {
                    MessageBox.Show("Chỉ được upload biên bản khi sự cố đã xử lý!");
                    return;
                }

                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png|All files (*.*)|*.*";
                    ofd.FilterIndex = 1;
                    ofd.RestoreDirectory = true;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        // Hiển thị ảnh lên PictureBox
                        picHinhAnh.Image = Image.FromFile(ofd.FileName);
                        picHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;

                        // Chuyển ảnh thành byte array
                        byte[] imageData;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (Image img = Image.FromFile(ofd.FileName))
                            {
                                img.Save(ms, img.RawFormat);
                                imageData = ms.ToArray();
                            }
                        }

                        string sql = @"UPDATE XuLySuCo 
                             SET BienBan = @BienBan
                             WHERE MaSuCo = @MaSuCo";

                        SqlParameter[] parameters = {
                    new SqlParameter("@BienBan", imageData),
                    new SqlParameter("@MaSuCo", dgvDanhSach.SelectedRows[0].Cells["MaSuCo"].Value)
                };

                        int result = db.getNonQuery(sql, parameters);
                        if (result > 0)
                        {
                            MessageBox.Show("Upload biên bản thành công!");
                            LoadDataToGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi upload hình ảnh: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtMaKH.Clear();
            txtTenKH.Clear();
            cboLoaiSuCo.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = -1;
            fileData = null;
            fileName = "";

            lblFileName.Text = "";
        }
    }
}
