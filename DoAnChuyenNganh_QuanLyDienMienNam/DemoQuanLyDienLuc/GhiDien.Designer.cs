﻿namespace DemoQuanLyDienLuc
{
    partial class GhiDien
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GhiDien));
            this.Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvTienDien = new Guna.UI2.WinForms.Guna2DataGridView();
            this.Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.btnReset = new Guna.UI2.WinForms.Guna2Button();
            this.txtTimKhachHang = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnTimKiem = new Guna.UI2.WinForms.Guna2Button();
            this.cboThangLoc = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cboNamLoc = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboDongHo = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtChiSoMoi = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtChiSoCu = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPre = new Guna.UI2.WinForms.Guna2Button();
            this.btnNext = new Guna.UI2.WinForms.Guna2Button();
            this.lblNhanVienGhiDien = new System.Windows.Forms.Label();
            this.lblSoMoi = new System.Windows.Forms.Label();
            this.lblSoCu = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblThangNam = new System.Windows.Forms.Label();
            this.cboXa = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cboHuyen = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cboTinh = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnThem = new Guna.UI2.WinForms.Guna2Button();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTienDien)).BeginInit();
            this.Panel2.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel1.BorderColor = System.Drawing.Color.DodgerBlue;
            this.Panel1.BorderRadius = 10;
            this.Panel1.BorderThickness = 1;
            this.Panel1.Controls.Add(this.dgvTienDien);
            this.Panel1.Location = new System.Drawing.Point(28, 483);
            this.Panel1.Margin = new System.Windows.Forms.Padding(4);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(1499, 360);
            this.Panel1.TabIndex = 48;
            // 
            // dgvTienDien
            // 
            this.dgvTienDien.AllowUserToAddRows = false;
            this.dgvTienDien.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvTienDien.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTienDien.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTienDien.ColumnHeadersHeight = 15;
            this.dgvTienDien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTienDien.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTienDien.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvTienDien.Location = new System.Drawing.Point(48, 30);
            this.dgvTienDien.Margin = new System.Windows.Forms.Padding(4);
            this.dgvTienDien.Name = "dgvTienDien";
            this.dgvTienDien.ReadOnly = true;
            this.dgvTienDien.RowHeadersVisible = false;
            this.dgvTienDien.RowHeadersWidth = 51;
            this.dgvTienDien.Size = new System.Drawing.Size(1391, 303);
            this.dgvTienDien.TabIndex = 56;
            this.dgvTienDien.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvTienDien.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvTienDien.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvTienDien.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvTienDien.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvTienDien.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvTienDien.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvTienDien.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvTienDien.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvTienDien.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTienDien.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvTienDien.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvTienDien.ThemeStyle.HeaderStyle.Height = 15;
            this.dgvTienDien.ThemeStyle.ReadOnly = true;
            this.dgvTienDien.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvTienDien.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvTienDien.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTienDien.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvTienDien.ThemeStyle.RowsStyle.Height = 22;
            this.dgvTienDien.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvTienDien.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvTienDien.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTienDien_CellClick);
            // 
            // Panel2
            // 
            this.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel2.BorderColor = System.Drawing.Color.DodgerBlue;
            this.Panel2.BorderRadius = 10;
            this.Panel2.BorderThickness = 1;
            this.Panel2.Controls.Add(this.guna2GroupBox1);
            this.Panel2.Controls.Add(this.label6);
            this.Panel2.Controls.Add(this.label5);
            this.Panel2.Controls.Add(this.label4);
            this.Panel2.Controls.Add(this.cboDongHo);
            this.Panel2.Controls.Add(this.txtChiSoMoi);
            this.Panel2.Controls.Add(this.txtChiSoCu);
            this.Panel2.Controls.Add(this.label2);
            this.Panel2.Controls.Add(this.label1);
            this.Panel2.Controls.Add(this.btnPre);
            this.Panel2.Controls.Add(this.btnNext);
            this.Panel2.Controls.Add(this.lblNhanVienGhiDien);
            this.Panel2.Controls.Add(this.lblSoMoi);
            this.Panel2.Controls.Add(this.lblSoCu);
            this.Panel2.Controls.Add(this.label7);
            this.Panel2.Controls.Add(this.lblThangNam);
            this.Panel2.Controls.Add(this.cboXa);
            this.Panel2.Controls.Add(this.cboHuyen);
            this.Panel2.Controls.Add(this.cboTinh);
            this.Panel2.Controls.Add(this.btnThem);
            this.Panel2.Location = new System.Drawing.Point(28, 25);
            this.Panel2.Margin = new System.Windows.Forms.Padding(4);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(1499, 440);
            this.Panel2.TabIndex = 49;
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox1.Controls.Add(this.btnReset);
            this.guna2GroupBox1.Controls.Add(this.txtTimKhachHang);
            this.guna2GroupBox1.Controls.Add(this.btnTimKiem);
            this.guna2GroupBox1.Controls.Add(this.cboThangLoc);
            this.guna2GroupBox1.Controls.Add(this.cboNamLoc);
            this.guna2GroupBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2GroupBox1.Location = new System.Drawing.Point(1076, 62);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(300, 302);
            this.guna2GroupBox1.TabIndex = 63;
            this.guna2GroupBox1.Text = "Tìm kiếm";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BorderRadius = 20;
            this.btnReset.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(94)))), ((int)(((byte)(121)))));
            this.btnReset.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnReset.ImageSize = new System.Drawing.Size(30, 30);
            this.btnReset.Location = new System.Drawing.Point(153, 214);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(120, 50);
            this.btnReset.TabIndex = 39;
            this.btnReset.Text = "Reset";
            this.btnReset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // txtTimKhachHang
            // 
            this.txtTimKhachHang.BorderRadius = 15;
            this.txtTimKhachHang.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKhachHang.DefaultText = "Nhập tên khách hàng";
            this.txtTimKhachHang.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTimKhachHang.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTimKhachHang.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimKhachHang.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTimKhachHang.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTimKhachHang.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTimKhachHang.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTimKhachHang.Location = new System.Drawing.Point(32, 156);
            this.txtTimKhachHang.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTimKhachHang.Name = "txtTimKhachHang";
            this.txtTimKhachHang.PasswordChar = '\0';
            this.txtTimKhachHang.PlaceholderText = "";
            this.txtTimKhachHang.SelectedText = "";
            this.txtTimKhachHang.Size = new System.Drawing.Size(254, 44);
            this.txtTimKhachHang.TabIndex = 61;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTimKiem.BorderRadius = 20;
            this.btnTimKiem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(94)))), ((int)(((byte)(121)))));
            this.btnTimKiem.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Image = ((System.Drawing.Image)(resources.GetObject("btnTimKiem.Image")));
            this.btnTimKiem.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnTimKiem.ImageSize = new System.Drawing.Size(30, 30);
            this.btnTimKiem.Location = new System.Drawing.Point(32, 214);
            this.btnTimKiem.Margin = new System.Windows.Forms.Padding(4);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(113, 50);
            this.btnTimKiem.TabIndex = 39;
            this.btnTimKiem.Text = "Tìm";
            this.btnTimKiem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // cboThangLoc
            // 
            this.cboThangLoc.BackColor = System.Drawing.Color.Transparent;
            this.cboThangLoc.BorderRadius = 15;
            this.cboThangLoc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboThangLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThangLoc.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboThangLoc.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboThangLoc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboThangLoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboThangLoc.ItemHeight = 30;
            this.cboThangLoc.Location = new System.Drawing.Point(32, 56);
            this.cboThangLoc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboThangLoc.Name = "cboThangLoc";
            this.cboThangLoc.Size = new System.Drawing.Size(254, 36);
            this.cboThangLoc.TabIndex = 60;
            // 
            // cboNamLoc
            // 
            this.cboNamLoc.BackColor = System.Drawing.Color.Transparent;
            this.cboNamLoc.BorderRadius = 15;
            this.cboNamLoc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboNamLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNamLoc.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboNamLoc.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboNamLoc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboNamLoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboNamLoc.ItemHeight = 30;
            this.cboNamLoc.Location = new System.Drawing.Point(32, 104);
            this.cboNamLoc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboNamLoc.Name = "cboNamLoc";
            this.cboNamLoc.Size = new System.Drawing.Size(254, 36);
            this.cboNamLoc.TabIndex = 60;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label6.Location = new System.Drawing.Point(43, 343);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 28);
            this.label6.TabIndex = 59;
            this.label6.Text = "Phường";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.Location = new System.Drawing.Point(43, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 28);
            this.label5.TabIndex = 59;
            this.label5.Text = "Quận";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.Location = new System.Drawing.Point(43, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 28);
            this.label4.TabIndex = 59;
            this.label4.Text = "Thành phố";
            // 
            // cboDongHo
            // 
            this.cboDongHo.BackColor = System.Drawing.Color.Transparent;
            this.cboDongHo.BorderRadius = 20;
            this.cboDongHo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboDongHo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDongHo.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboDongHo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboDongHo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboDongHo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboDongHo.ItemHeight = 30;
            this.cboDongHo.Location = new System.Drawing.Point(554, 110);
            this.cboDongHo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboDongHo.Name = "cboDongHo";
            this.cboDongHo.Size = new System.Drawing.Size(437, 36);
            this.cboDongHo.TabIndex = 58;
            this.cboDongHo.SelectedIndexChanged += new System.EventHandler(this.cboDongHo_SelectedIndexChanged);
            // 
            // txtChiSoMoi
            // 
            this.txtChiSoMoi.BorderRadius = 20;
            this.txtChiSoMoi.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtChiSoMoi.DefaultText = "";
            this.txtChiSoMoi.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtChiSoMoi.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtChiSoMoi.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtChiSoMoi.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtChiSoMoi.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtChiSoMoi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtChiSoMoi.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtChiSoMoi.Location = new System.Drawing.Point(552, 236);
            this.txtChiSoMoi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChiSoMoi.Name = "txtChiSoMoi";
            this.txtChiSoMoi.PasswordChar = '\0';
            this.txtChiSoMoi.PlaceholderText = "";
            this.txtChiSoMoi.SelectedText = "";
            this.txtChiSoMoi.Size = new System.Drawing.Size(437, 48);
            this.txtChiSoMoi.TabIndex = 57;
            // 
            // txtChiSoCu
            // 
            this.txtChiSoCu.AutoSize = true;
            this.txtChiSoCu.Location = new System.Drawing.Point(692, 90);
            this.txtChiSoCu.Name = "txtChiSoCu";
            this.txtChiSoCu.Size = new System.Drawing.Size(10, 16);
            this.txtChiSoCu.TabIndex = 55;
            this.txtChiSoCu.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(547, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 28);
            this.label2.TabIndex = 55;
            this.label2.Text = "Chỉ số mới";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(549, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 28);
            this.label1.TabIndex = 55;
            this.label1.Text = "Mã đồng hồ";
            // 
            // btnPre
            // 
            this.btnPre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPre.BorderRadius = 20;
            this.btnPre.FillColor = System.Drawing.Color.DarkSeaGreen;
            this.btnPre.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPre.ForeColor = System.Drawing.Color.White;
            this.btnPre.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnPre.ImageSize = new System.Drawing.Size(30, 30);
            this.btnPre.Location = new System.Drawing.Point(539, 314);
            this.btnPre.Margin = new System.Windows.Forms.Padding(4);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(61, 50);
            this.btnPre.TabIndex = 54;
            this.btnPre.Text = "<<";
            this.btnPre.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnPre.Click += new System.EventHandler(this.btnPre_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.BorderRadius = 20;
            this.btnNext.FillColor = System.Drawing.Color.DarkSeaGreen;
            this.btnNext.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnNext.ImageSize = new System.Drawing.Size(30, 30);
            this.btnNext.Location = new System.Drawing.Point(838, 314);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(61, 50);
            this.btnNext.TabIndex = 53;
            this.btnNext.Text = ">>";
            this.btnNext.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblNhanVienGhiDien
            // 
            this.lblNhanVienGhiDien.AutoSize = true;
            this.lblNhanVienGhiDien.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblNhanVienGhiDien.Location = new System.Drawing.Point(41, 39);
            this.lblNhanVienGhiDien.Name = "lblNhanVienGhiDien";
            this.lblNhanVienGhiDien.Size = new System.Drawing.Size(129, 28);
            this.lblNhanVienGhiDien.TabIndex = 51;
            this.lblNhanVienGhiDien.Text = "Nhân viên:";
            // 
            // lblSoMoi
            // 
            this.lblSoMoi.AutoSize = true;
            this.lblSoMoi.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblSoMoi.Location = new System.Drawing.Point(748, 158);
            this.lblSoMoi.Name = "lblSoMoi";
            this.lblSoMoi.Size = new System.Drawing.Size(95, 28);
            this.lblSoMoi.TabIndex = 51;
            this.lblSoMoi.Text = "Số mới:";
            // 
            // lblSoCu
            // 
            this.lblSoCu.AutoSize = true;
            this.lblSoCu.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblSoCu.Location = new System.Drawing.Point(547, 158);
            this.lblSoCu.Name = "lblSoCu";
            this.lblSoCu.Size = new System.Drawing.Size(75, 28);
            this.lblSoCu.TabIndex = 51;
            this.lblSoCu.Text = "Số cũ:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.Location = new System.Drawing.Point(41, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 28);
            this.label7.TabIndex = 51;
            this.label7.Text = "Lịch ghi điện:";
            // 
            // lblThangNam
            // 
            this.lblThangNam.AutoSize = true;
            this.lblThangNam.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblThangNam.Location = new System.Drawing.Point(233, 80);
            this.lblThangNam.Name = "lblThangNam";
            this.lblThangNam.Size = new System.Drawing.Size(144, 28);
            this.lblThangNam.TabIndex = 51;
            this.lblThangNam.Text = "tháng / năm";
            // 
            // cboXa
            // 
            this.cboXa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboXa.BackColor = System.Drawing.Color.Transparent;
            this.cboXa.BorderRadius = 20;
            this.cboXa.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboXa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboXa.Enabled = false;
            this.cboXa.FocusedColor = System.Drawing.Color.Empty;
            this.cboXa.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboXa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboXa.FormattingEnabled = true;
            this.cboXa.ItemHeight = 30;
            this.cboXa.Location = new System.Drawing.Point(48, 371);
            this.cboXa.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboXa.Name = "cboXa";
            this.cboXa.Size = new System.Drawing.Size(340, 36);
            this.cboXa.TabIndex = 50;
            this.cboXa.SelectedIndexChanged += new System.EventHandler(this.cboXa_SelectedIndexChanged);
            // 
            // cboHuyen
            // 
            this.cboHuyen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboHuyen.BackColor = System.Drawing.Color.Transparent;
            this.cboHuyen.BorderRadius = 20;
            this.cboHuyen.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboHuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHuyen.Enabled = false;
            this.cboHuyen.FocusedColor = System.Drawing.Color.Empty;
            this.cboHuyen.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboHuyen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboHuyen.FormattingEnabled = true;
            this.cboHuyen.ItemHeight = 30;
            this.cboHuyen.Location = new System.Drawing.Point(48, 290);
            this.cboHuyen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboHuyen.Name = "cboHuyen";
            this.cboHuyen.Size = new System.Drawing.Size(340, 36);
            this.cboHuyen.TabIndex = 50;
            this.cboHuyen.SelectedIndexChanged += new System.EventHandler(this.cboHuyen_SelectedIndexChanged);
            // 
            // cboTinh
            // 
            this.cboTinh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTinh.BackColor = System.Drawing.Color.Transparent;
            this.cboTinh.BorderRadius = 20;
            this.cboTinh.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTinh.Enabled = false;
            this.cboTinh.FocusedColor = System.Drawing.Color.Empty;
            this.cboTinh.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboTinh.FormattingEnabled = true;
            this.cboTinh.ItemHeight = 30;
            this.cboTinh.Location = new System.Drawing.Point(48, 198);
            this.cboTinh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTinh.Name = "cboTinh";
            this.cboTinh.Size = new System.Drawing.Size(340, 36);
            this.cboTinh.TabIndex = 50;
            this.cboTinh.SelectedIndexChanged += new System.EventHandler(this.cboTinh_SelectedIndexChanged);
            // 
            // btnThem
            // 
            this.btnThem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThem.BorderRadius = 20;
            this.btnThem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(94)))), ((int)(((byte)(121)))));
            this.btnThem.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Image = ((System.Drawing.Image)(resources.GetObject("btnThem.Image")));
            this.btnThem.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnThem.ImageSize = new System.Drawing.Size(30, 30);
            this.btnThem.Location = new System.Drawing.Point(645, 314);
            this.btnThem.Margin = new System.Windows.Forms.Padding(4);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(153, 50);
            this.btnThem.TabIndex = 39;
            this.btnThem.Text = "Thêm";
            this.btnThem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // GhiDien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1600, 914);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.Panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GhiDien";
            this.Text = "GhiDien";
            this.Load += new System.EventHandler(this.GhiDien_Load);
            this.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTienDien)).EndInit();
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.guna2GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        internal Guna.UI2.WinForms.Guna2Panel Panel1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvTienDien;
        internal Guna.UI2.WinForms.Guna2Panel Panel2;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKhachHang;
        private Guna.UI2.WinForms.Guna2ComboBox cboNamLoc;
        private Guna.UI2.WinForms.Guna2ComboBox cboThangLoc;
        internal Guna.UI2.WinForms.Guna2Button btnReset;
        internal Guna.UI2.WinForms.Guna2Button btnTimKiem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox cboDongHo;
        private Guna.UI2.WinForms.Guna2TextBox txtChiSoMoi;
        private System.Windows.Forms.Label txtChiSoCu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        internal Guna.UI2.WinForms.Guna2Button btnPre;
        internal Guna.UI2.WinForms.Guna2Button btnNext;
        private System.Windows.Forms.Label lblNhanVienGhiDien;
        private System.Windows.Forms.Label lblSoMoi;
        private System.Windows.Forms.Label lblSoCu;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblThangNam;
        private Guna.UI2.WinForms.Guna2ComboBox cboXa;
        private Guna.UI2.WinForms.Guna2ComboBox cboHuyen;
        private Guna.UI2.WinForms.Guna2ComboBox cboTinh;
        internal Guna.UI2.WinForms.Guna2Button btnThem;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
    }
}