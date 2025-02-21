using Guna.UI2.WinForms;
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
    public partial class MaHoaMatKhau : Form
    {
        private DBConnect db;
        public MaHoaMatKhau()
        {
            InitializeComponent();
            db = new DBConnect();
            guna2ShadowForm1 = new Guna2ShadowForm();
        }

      

        private void LoadDanhSachTaiKhoan()
        {
            string query = @"SELECT MaTaiKhoan, TenDangNhap, MatKhau, ChucVu 
                       FROM TaiKhoan 
                       ORDER BY MaTaiKhoan";
            dgvTaiKhoan.DataSource = db.getDataTable(query);
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

                    MessageBox.Show(
                        $"Kết quả mã hóa mật khẩu:\n" +
                        $"- Thành công: {success}\n" +
                        $"- Thất bại: {failed}",
                        "Hoàn tất",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadDanhSachTaiKhoan();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MaHoaMatKhau_Load(object sender, EventArgs e)
        {
           
            LoadDanhSachTaiKhoan();
        }
    }
}
