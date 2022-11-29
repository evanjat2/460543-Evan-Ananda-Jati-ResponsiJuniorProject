using Npgsql;
using System.Data;

namespace Responsi
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection conn;
        string connstring = "Host=localhost;Port=2022;Username=postgres;Password=informatika;Database=Responsi;";
        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql = null;
        private DataGridViewRow r;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from st_insert(:_nama,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", tbNama.Text);
                cmd.Parameters.AddWithValue("_id_dep", cbDepartemen.SelectedItem);
                if((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Karyawan berhasil ditambah!" , "Yeay", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conn.Close();
                LoadData();
                tbNama.Text = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "FAIL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                conn.Open();
                dgvData.DataSource = null;
                sql = "select * from st_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message, "FAIL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>= 0)
            {
                r = dgvData.Rows[e.RowIndex];
                tbNama.Text = r.Cells["_nama"].Value.ToString();
                cbDepartemen.SelectedItem = r.Cells["_id_dep"].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Pilih data yang mau diupdate!", "Hmm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from st_update(:_id_karyawan,:_nama,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama", tbNama.Text);
                cmd.Parameters.AddWithValue("_id_dep", cbDepartemen.SelectedItem);
                if((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Karyawan berhasil diupdate!", "Yeay", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    LoadData();
                    tbNama.Text  = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "FAIL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Pilih data yang mau dihapus!", "Hmm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from st_delete(:_id_karyawan)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Karyawan berhasil dihapus!", "Yeay", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    LoadData();
                    tbNama.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "FAIL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}