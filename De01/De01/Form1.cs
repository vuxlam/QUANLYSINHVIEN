using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace De01
{
    public partial class Form1 : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;
        private string QuanLySV;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            string connectionString = "Data Source=DESKTOP-GTF9NJ3\\SQLEXPRESS;Initial Catalog=QuanLySV;Integrated Security=True"; 
            connection = new SqlConnection(connectionString);

            dataAdapter = new SqlDataAdapter("SELECT * FROM SinhVien", connection);
            dataSet = new DataSet();

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
            dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
            dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        private void LoadDataIntoDataGridView()
        {
            try
            {
                dataSet.Clear();
                dataAdapter.Fill(dataSet, "SinhVien");
                lvSinhVien.DataSource = dataSet.Tables["SinhVien"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow newRow = dataSet.Tables["Sinhvien"].NewRow();
                newRow["MaSV"] = txtMaSV.Text.Trim();
                newRow["HotenSV"] = txtHoTenSV.Text.Trim();
                newRow["MaLop"] = cboLop.SelectedItem?.ToString();
                dataSet.Tables["Sinhvien"].Rows.Add(newRow);
                newRow["NgaySinh"] = dtNgaySinh.Value;


                int rowsAffected = dataAdapter.Update(dataSet, "Sinhvien");

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Thêm thành công.");
                    LoadDataIntoDataGridView();
                }
                else
                {
                    MessageBox.Show("Thêm không thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (lvSinhVien.SelectedRows.Count > 0)
            {
                try
                {
                    foreach (DataGridViewRow row in lvSinhVien.SelectedRows)
                    {
                        lvSinhVien.Rows.Remove(row);
                    }

                    int rowsAffected = dataAdapter.Update(dataSet, "SinhVien");

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa thành công.");
                        LoadDataIntoDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = lvSinhVien.CurrentCell.RowIndex;

                if (rowIndex >= 0)
                {
                    DataRow rowToUpdate = dataSet.Tables["SinhVien"].Rows[rowIndex];
                    rowToUpdate["MaSV"] = txtMaSV.Text.Trim();
                    rowToUpdate["HoTen"] = txtHoTenSV.Text.Trim();
                    rowToUpdate["NgaySinh"] = dtNgaySinh.Value;
                    rowToUpdate["Lop"] = cboLop.SelectedItem?.ToString();

                    int rowsAffected = dataAdapter.Update(dataSet, "SinhVien");

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thành công.");
                        LoadDataIntoDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật không thành công.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một sinh viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnsua_Click_1(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = lvSinhVien.CurrentCell.RowIndex;

                if (rowIndex >= 0)
                {
                    DataRow rowToUpdate = dataSet.Tables["Sinhvien"].Rows[rowIndex];
                    rowToUpdate["MaSV"] = txtMaSV.Text.Trim();
                    rowToUpdate["HotenSV"] = txtHoTenSV.Text.Trim();
                    rowToUpdate["MaLop"] = cboLop.SelectedItem?.ToString();

                    int rowsAffected = dataAdapter.Update(dataSet, "Sinhvien");

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thành công.");
                        LoadDataIntoDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật không thành công.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một sinh viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string button8 = txtTimKiem.Text.Trim().ToLower();

            foreach (DataGridViewRow row in lvSinhVien.Rows)
            {
                string maSV = row.Cells["MaSV"].Value.ToString().ToLower(); 
                string tenSV = row.Cells["HotenSV"].Value.ToString().ToLower(); 

                if (maSV.Contains(button8) || tenSV.Contains(button8))
                {
                    row.Selected = true;
                    lvSinhVien.CurrentCell = row.Cells["HotenSV"]; 
                    row.Cells["HotenSV"].Selected = true;
                    row.Cells["MaSV"].Selected = true; 
                    row.Cells["NgaySinh"].Selected = true; 
                }
                else
                {
                    row.Selected = false;
                }
            }

            int selectedCount = lvSinhVien.SelectedRows.Count;
            if (selectedCount > 0)
            {
                MessageBox.Show($"Tìm thấy {selectedCount} kết quả tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không tìm thấy kết quả nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}


       

