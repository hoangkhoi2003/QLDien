using BCrypt.Net;
using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DemoQuanLyDienLuc
{
    public partial class MaHoaMatKhau : Form
    {
        private DBConnect db;
        private Guna2DataGridView dgvTaiKhoan;
        private Guna2Button btnMaHoa;
        private Guna2HtmlLabel lblThongKe;
        private Guna2ShadowForm shadow;

        public MaHoaMatKhau()
        {
            InitializeComponent();
            db = new DBConnect();
        }

        private void MaHoaMatKhau_Load(object sender, EventArgs e)
        {
            // Tạo shadow cho form
            shadow = new Guna2ShadowForm(this);
            shadow.SetShadowForm(this);

            // Load dữ liệu tài khoản
            LoadDanhSachTaiKhoan();
        }

        private void LoadDanhSachTaiKhoan()
        {
            string query = @"SELECT MaTaiKhoan, TenDangNhap, MatKhau, ChucVu 
                           FROM TaiKhoan 
                           ORDER BY MaTaiKhoan";
            dgvTaiKhoan.DataSource = db.getDataTable(query);

            // Cập nhật label thống kê
            CapNhatThongKe();
        }

        private void CapNhatThongKe()
        {
            int totalAccounts = dgvTaiKhoan.Rows.Count;
            lblThongKe.Text = $"Tổng số tài khoản: {totalAccounts}";
        }

        private void btnMaHoa_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show(
                    "Bạn có chắc chắn muốn mã hóa lại tất cả mật khẩu?\n" +
                    "Lưu ý: Hãy đảm bảo đã sao lưu database trước khi thực hiện!",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (dialog == DialogResult.Yes)
                {
                    int success = 0;
                    int failed = 0;

                    // Lấy danh sách tài khoản
                    foreach (DataGridViewRow row in dgvTaiKhoan.Rows)
                    {
                        string maTaiKhoan = row.Cells["MaTaiKhoan"].Value.ToString();
                        string matKhau = row.Cells["MatKhau"].Value.ToString();

                        try
                        {
                            // Mã hóa mật khẩu
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(matKhau);

                            // Cập nhật vào database
                            string updateQuery = "UPDATE TaiKhoan SET MatKhau = @MatKhau WHERE MaTaiKhoan = @MaTaiKhoan";
                            SqlParameter[] parameters = new SqlParameter[]
                            {
                                new SqlParameter("@MatKhau", hashedPassword),
                                new SqlParameter("@MaTaiKhoan", maTaiKhoan)
                            };

                            int result = db.getNonQuery(updateQuery, parameters);
                            if (result > 0)
                                success++;
                            else
                                failed++;
                        }
                        catch
                        {
                            failed++;
                        }
                    }

                    // Hiển thị kết quả
                    MessageBox.Show(
                        $"Kết quả mã hóa mật khẩu:\n" +
                        $"- Thành công: {success}\n" +
                        $"- Thất bại: {failed}",
                        "Hoàn tất",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Load lại dữ liệu
                    LoadDanhSachTaiKhoan();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            // Form
            this.Text = "Mã hóa mật khẩu";
            this.Size = new System.Drawing.Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Button
            btnMaHoa = new Guna2Button();
            btnMaHoa.Text = "Mã hóa tất cả mật khẩu";
            btnMaHoa.Size = new System.Drawing.Size(200, 45);
            btnMaHoa.Location = new System.Drawing.Point(20, 20);
            btnMaHoa.Click += btnMaHoa_Click;

            // Label thống kê
            lblThongKe = new Guna2HtmlLabel();
            lblThongKe.Location = new System.Drawing.Point(240, 30);
            lblThongKe.AutoSize = true;

            // DataGridView
            dgvTaiKhoan = new Guna2DataGridView();
            dgvTaiKhoan.Location = new System.Drawing.Point(20, 80);
            dgvTaiKhoan.Size = new System.Drawing.Size(740, 350);
            dgvTaiKhoan.AllowUserToAddRows = false;
            dgvTaiKhoan.ReadOnly = true;
            dgvTaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add controls to form
            this.Controls.Add(btnMaHoa);
            this.Controls.Add(lblThongKe);
            this.Controls.Add(dgvTaiKhoan);
        }
    }
}