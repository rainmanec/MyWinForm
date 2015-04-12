using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
namespace ZHBB
{
    public partial class FrmCompanyList : Form
    {
        public int id;

        public FrmCompanyList()
        {
            InitializeComponent();
        }

        private void FrmListUser_Load(object sender, EventArgs e)
        {
            this.LoadData(tb_serch.Text.Trim());
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            this.LoadData(tb_serch.Text.Trim());
        }
        // 刷新最新录入的10条记录
        public void LoadData(string search)
        {
            search = search.Trim().Replace("'", "");
            string sql;
            if (search == "")
            {
                sql = string.Format(@"
                        SELECT TOP 2000
                            ID,
                            Gsm as '采购单位',
                            owner as '单位负责人',
                            phone as '电话',
                            address as '地址',
                            beizhu as '备注'
                        FROM Company ORDER BY ID DESC");
            }
            else
            {
                sql = string.Format(@"
                        SELECT TOP 2000
                            ID,
                            Gsm as '采购单位',
                            owner as '单位负责人',
                            phone as '电话',
                            address as '地址',
                            beizhu as '备注'
                        FROM Company WHERE Gsm like '%{0}%' OR beizhu like '%{0}%' ORDER BY ID DESC", search);
            }
            DataTable table = SqlHelper.GetDataTableBySQL(sql);

            Util.DgvClear(dgv_main);
            dgv_main.DataSource = table;

            DataGridViewButtonColumn dgv_button_col_edit = new DataGridViewButtonColumn();
            dgv_button_col_edit.Name = "编辑";
            dgv_button_col_edit.UseColumnTextForButtonValue = true;
            dgv_button_col_edit.Text = "编辑";
            dgv_button_col_edit.HeaderText = "编辑";
            dgv_main.Columns.Insert(dgv_main.Columns.Count, dgv_button_col_edit);
            dgv_main.Columns["编辑"].Width = 75;
            dgv_main.Columns["编辑"].DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);

            DataGridViewButtonColumn dgv_button_col_del = new DataGridViewButtonColumn();
            dgv_button_col_del.Name = "删除";
            dgv_button_col_del.UseColumnTextForButtonValue = true;
            dgv_button_col_del.Text = "删除";
            dgv_button_col_del.HeaderText = "删除";
            dgv_main.Columns.Insert(dgv_main.Columns.Count, dgv_button_col_del);
            dgv_main.Columns["删除"].Width = 75;
            dgv_main.Columns["删除"].DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);

        }

        private void dgv_main_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 如果点击的是表头则退出
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dgv_main.Rows[e.RowIndex];
            this.id = Convert.ToInt32(row.Cells["ID"].Value);

            if (dgv_main.Columns[e.ColumnIndex].Name == "编辑")
            {
                FrmCompanyEdit frm = new FrmCompanyEdit();
                frm.id = this.id;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.LoadData(tb_serch.Text.Trim());
                }
            }
            else if (dgv_main.Columns[e.ColumnIndex].Name == "删除")
            {
                if (MessageBox.Show("您确定要删除这条记录吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    SqlParameter p_id = Util.NewSqlParameter("@p_id", SqlDbType.Int, this.id);
                    int affect = SqlHelper.ExecuteNonQuery("DELETE FROM Company WHERE ID = @p_id", p_id);
                    if (affect == 1)
                    {
                        MessageBox.Show("删除成功！");
                        this.LoadData(tb_serch.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show("删除失败");
                    }
                }
            }

        }

        private void tb_serch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.LoadData(tb_serch.Text.Trim());
            }
        }

        private void btn_car_add_Click(object sender, EventArgs e)
        {
            FrmCompanyAdd frm = new FrmCompanyAdd();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.LoadData(tb_serch.Text.Trim());
            }
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.Title = "导出数据";
            this.saveFileDialog1.Filter = "Excel工作簿|*.xls";
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 整理数据
                string sql = string.Format(@"
                        SELECT
                            Gsm as '采购单位',
                            owner as '单位负责人',
                            phone as '电话',
                            address as '地址',
                            beizhu as '备注'
                        FROM Company ORDER BY Gsm ASC");
                DataTable table = SqlHelper.GetDataTableBySQL(sql);
                // 导出数据
                string filename = this.saveFileDialog1.FileName;
                if (Util.ExportExcelWithAspose(table, filename, "采购单位"))
                {
                    MessageBox.Show("导出成功！", "提示");
                }
                else
                {
                    MessageBox.Show("导出失败", "提示");
                }
            }
        }
    }
}
