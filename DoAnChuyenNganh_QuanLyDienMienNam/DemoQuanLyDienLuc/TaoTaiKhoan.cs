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
    public partial class TaoTaiKhoan : Form
    {
        private DBConnect db;
        private KhachHang khachHangForm;
        private NhanVien nhanVienForm;

        public TaoTaiKhoan(KhachHang formKhachHang)
        {
            InitializeComponent();
            db = new DBConnect();
            this.khachHangForm = formKhachHang;

            // Thiết lập vai trò cho ComboBox
            cmbVaiTro.Items.Add("Khách Hàng");
            cmbVaiTro.SelectedIndex = 0;
        }
        public TaoTaiKhoan(NhanVien nhanVien)
        {
            InitializeComponent();
            db = new DBConnect();
            this.nhanVienForm = nhanVien;

            // Thiết lập vai trò cho ComboBox
           
            cmbVaiTro.Items.Add("Quản lý kỹ thuật");
            cmbVaiTro.Items.Add("Nhân viên tiếp nhận");
            cmbVaiTro.Items.Add("Nhân viên kỹ thuật");
            cmbVaiTro.Items.Add("Nhân viên ghi điện");
            cmbVaiTro.Items.Add("Nhân viên thu ngân ");
            cmbVaiTro.SelectedIndex = 0;
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();
            string ChucVu = cmbVaiTro.SelectedItem.ToString();

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin.");
                return;
            }

            // Kiểm tra độ dài mật khẩu tối thiểu
            if (matKhau.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!");
                return;
            }


            // Chuẩn bị truy vấn INSERT
            string query = @"INSERT INTO TaiKhoan (TenDangNhap, MatKhau, ChucVu) 
                VALUES (@TenDangNhap, CONVERT(VARBINARY(256), HASHBYTES('SHA2_256', @MatKhau)), @ChucVu)";


            // Tạo các tham số với mật khẩu gốc (không cần hash trước)
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenDangNhap", tenDangNhap),
                new SqlParameter("@MatKhau", matKhau),  // Truyền mật khẩu gốc
                new SqlParameter("@ChucVu", ChucVu)
            };

            try
            {
                int rowsAffected = db.getNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Tạo tài khoản thành công!");
                    // Xóa trắng các trường sau khi tạo thành công
                    txtTenDangNhap.Clear();
                    txtMatKhau.Clear();
                    cmbVaiTro.SelectedIndex = 0;

                    // Lấy MaTaiKhoan vừa được tạo
                    string query1 = "SELECT TOP 1 MaTaiKhoan FROM TaiKhoan ORDER BY MaTaiKhoan DESC";
                    DataTable dt = db.getDataTable(query1);

                    if (dt.Rows.Count > 0)
                    {
                        string maTaiKhoanMoi = dt.Rows[0]["MaTaiKhoan"].ToString();

                        // Cập nhật MaTaiKhoan cho form tương ứng
                        if (khachHangForm != null)
                        {
                            khachHangForm.SetMaTaiKhoan(maTaiKhoanMoi);
                        }
                        if (nhanVienForm != null)
                        {
                            nhanVienForm.SetMaTaiKhoan(maTaiKhoanMoi);
                        }
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tạo tài khoản thất bại.");
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                }
                else
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void TaoTaiKhoan_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            txtMatKhau.UseSystemPasswordChar = true;
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtTenDangNhap_TextChanged(object sender, EventArgs e)
        {

        }



        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {
            this.txtMatKhau.UseSystemPasswordChar = true;
            this.txtMatKhau.PasswordChar = '\0';
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !guna2CheckBox1.Checked;
        }
    }
}
