﻿using System;
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
    public partial class FrmSearchNotOut : Form
    {
        public int id;

        public FrmSearchNotOut()
        {
            InitializeComponent();
        }

        private void FrmListUser_Load(object sender, EventArgs e)
        {
            this.LoadTop5000();
        }

        public void LoadTop5000()
        {
            string sql = string.Format(@"
                            SELECT TOP 5000
                                0 AS '序号',
                                Records.ID,
	                            Records.chepai AS '车牌',
	                            Records.Company AS '采购单位',
	                            Records.InWeight AS '进厂种类',
	                            Records.InTime AS '进厂时间',
	                            USERS.xingming AS '进厂操作员'
                            FROM Records LEFT JOIN Users ON Records.InUname = Users.uname
                            WHERE IsClose <> 1 ORDER BY Records.ID ASC
                            ");
            DataTable table = SqlHelper.GetDataTableBySQL(sql);
            Util.DataTableSetRowNumber(table, "序号", true);
            this.ResetDgv(table);
        }

        public void LoadAll()
        {
            string sql = string.Format(@"
                            SELECT
                                0 AS '序号',
                                Records.ID,
	                            Records.chepai AS '车牌',
	                            Records.Company AS '采购单位',
	                            Records.InWeight AS '进厂种类',
	                            Records.InTime AS '进厂时间',
	                            USERS.xingming AS '进厂操作员'
                            FROM Records LEFT JOIN Users ON Records.InUname = Users.uname
                            WHERE IsClose <> 1 ORDER BY Records.ID ASC
                            ");
            DataTable table = SqlHelper.GetDataTableBySQL(sql);
            Util.DataTableSetRowNumber(table, "序号", true);
            this.ResetDgv(table);


        }

        public void ResetDgv(DataTable table)
        {
            Util.DgvClear(this.dgv_main);
            this.dgv_main.DataSource = table;
            Util.DgvColHide(this.dgv_main, "ID");

            DataGridViewButtonColumn dgv_button_col_del = new DataGridViewButtonColumn();
            dgv_button_col_del.Name = "删除";
            dgv_button_col_del.UseColumnTextForButtonValue = true;
            dgv_button_col_del.Text = "删除";
            dgv_button_col_del.HeaderText = "删除";
            this.dgv_main.Columns.Insert(this.dgv_main.Columns.Count, dgv_button_col_del);
            this.dgv_main.Columns["删除"].Width = 75;
            this.dgv_main.Columns["删除"].DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
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

            if (dgv_main.Columns[e.ColumnIndex].Name == "删除")
            {
                if (MessageBox.Show("您确定要删除这条记录吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (Util.DeleteRecord(this.id) == true)
                    {
                        MessageBox.Show("删除成功！");
                        this.LoadTop5000();
                    }
                    else
                    {
                        MessageBox.Show("删除失败！");
                    }
                }
            }

        }

        private void btn_5000_Click(object sender, EventArgs e)
        {
            this.LoadTop5000();
        }

        private void btn_all_Click(object sender, EventArgs e)
        {
            this.LoadAll();
        }

    }
}
