﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;


using Microsoft.Reporting.WinForms;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;
namespace ZHBB
{
    public partial class FrmSearchMingXi : Form
    {
        public Point p_car; // 车牌号的位置
        public Point p_cp;  // 采购单位的位置

        public int id;      // datagridview中行的ID

        public string sql_query;
        public string sql_count;
        public string sql_where = "";       // 搜索条件
        public SqlParameter[] sql_parms;    // 搜索参数


        public FrmSearchMingXi()
        {
            InitializeComponent();
        }

        private void FrmRecordAdd_Load(object sender, EventArgs e)
        {
            this.p_car = new Point(tb_chepai.Left, tb_chepai.Top + tb_chepai.Height);   // 车辆文本框Location
            this.p_cp = new Point(tb_cp.Left, tb_cp.Top + tb_cp.Height);                // 单位文本框Location
            dgv_cars.BorderStyle = BorderStyle.None;
            dtp_begin.Value = DateTime.Now.AddDays(-1);                                 // 开始日期
            Util.InitComboBox(cb_kind, "Kinds", "Title", true);                         // 料种
            this.LoadData();
        }


        /// <summary>
        /// 根据搜索条件设置sql_where和sql_parms两个变量
        /// </summary>
        public void SearchCdnInit()
        {
            DateTime time_begin = Util.DateTimeToDayBegin(this.dtp_begin.Value);
            DateTime time_end = Util.DateTimeToDayEnd(this.dtp_end.Value);
            string chepai = this.tb_chepai.Text.Trim();
            string kind = this.cb_kind.Text.Trim();
            string company = this.tb_cp.Text.Trim();

            List<string> Condition = new List<string>();
            List<SqlParameter> Parms = new List<SqlParameter>();
            if (chepai != "")
            {
                Condition.Add("chepai = @p_chepai");
                Parms.Add(Util.NewSqlParameter("@p_chepai", SqlDbType.VarChar, chepai, 50));
            }
            if (kind != "")
            {
                Condition.Add("kind = @p_kind");
                Parms.Add(Util.NewSqlParameter("@p_kind", SqlDbType.VarChar, kind, 50));
            }
            if (company != "")
            {
                Condition.Add("company = @p_company");
                Parms.Add(Util.NewSqlParameter("@p_company", SqlDbType.VarChar, company, 50));
            }
            Condition.Add("OutTime >= @p_time_begin AND OutTime <= @p_time_end");
            Parms.Add(Util.NewSqlParameter("@p_time_begin", SqlDbType.DateTime, time_begin));
            Parms.Add(Util.NewSqlParameter("@p_time_end", SqlDbType.DateTime, time_end));

            this.sql_parms = Util.ListToArray(Parms);
            this.sql_where = (Condition.Count > 0) ? " AND " + Util.ListJoin(" AND ", Condition) : "";
        }

        /// <summary>
        /// 显示搜索记录
        /// </summary>
        public void LoadData()
        {
            this.SearchCdnInit();
            this.sql_query = string.Format(@"
                                    SELECT 
                                        ROW_NUMBER() OVER(ORDER BY TA.OutTime DESC) AS  '序号',
	                                    TA.ID AS 'ID', 
	                                    TA.Company AS '采购单位',
	                                    TA.chepai AS '车牌',
	                                    TA.Kind AS '料种',
	                                    TA.NetWeight AS '净重量',
	                                    TA.InWeight AS '进厂重量',
	                                    TA.OutTime AS '出厂重量',
	                                    TA.InTime AS '进厂时间',
	                                    TA.OutTime AS '出厂时间',
	                                    TA.other AS '其他',
	                                    TB.xingming AS '进厂操作员', 
	                                    TC.xingming AS '出厂操作员'
                                    FROM 
	                                    Records AS TA 
	                                    LEFT JOIN Users AS TB ON TA.InUname = TB.uname 
	                                    LEFT JOIN Users AS TC ON TA.OutUname = TC.uname
                                    WHERE IsClose = 1 {0}
                                ", this.sql_where);
            this.sql_count = string.Format(@"
                                    SELECT ISNULL(COUNT(*), 0)
                                    FROM 
	                                    Records AS TA 
	                                    LEFT JOIN Users AS TB ON TA.InUname = TB.uname 
	                                    LEFT JOIN Users AS TC ON TA.OutUname = TC.uname
                                    WHERE IsClose = 1 {0}
                                ", this.sql_where);
            this.paginator1.Init(Util.IntTryParse(SqlHelper.GetFirstCellStringBySQL(this.sql_count, this.sql_parms)), 100);
        }

        /// <summary>
        /// 车牌号文本框：根据车牌号搜索
        /// </summary>
        private void tb_chepai_TextChanged(object sender, EventArgs e)
        {
            bool IsResetDgv = true;
            string search = tb_chepai.Text.Trim().Replace("'", "");
            if (search.Length > 1)
            {
                string sql = string.Format(@"SELECT TOP 10  chepai AS '车牌号', owner as '车主' FROM Cars WHERE chepai2 LIKE '%{0}%'", search);
                DataTable table = SqlHelper.GetDataTableBySQL(sql);
                if (table.Rows.Count > 0)
                {
                    dgv_cars.DataSource = table;
                    dgv_cars.Location = this.p_car;
                    dgv_cars.BringToFront();
                    dgv_cars.Visible = true;
                    dgv_cars.Height = dgv_cars.Rows.Count * dgv_cars.RowTemplate.Height + dgv_cars.ColumnHeadersHeight + 10;
                    IsResetDgv = false;
                }
            }
            if (IsResetDgv)
            {
                dgv_cars.DataSource = null;
                dgv_cars.Visible = false;
            }
        }

        /// <summary>
        /// 车牌号文本框：Enter键
        /// </summary>
        private void tb_chepai_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            DataGridView dgv = this.dgv_cars;
            // 上下键
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                int rowsCount = dgv.Rows.Count;
                if (rowsCount > 0 && dgv.CurrentCell != null)
                {
                    int rowIndex = dgv.CurrentCell.RowIndex;
                    if (e.KeyCode == System.Windows.Forms.Keys.Down)
                    {
                        rowIndex = (rowIndex < rowsCount - 1) ? rowIndex + 1 : 0;
                    }
                    else if (e.KeyCode == System.Windows.Forms.Keys.Up)
                    {
                        rowIndex = (rowIndex == 0) ? rowsCount - 1 : rowIndex - 1;
                    }
                    dgv.CurrentCell = dgv.Rows[rowIndex].Cells[0];
                }
            }
            // Enter键
            if (e.KeyCode == Keys.Enter)
            {
                if (dgv.Rows.Count > 0)
                {
                    int rowIndex = dgv.CurrentCell.RowIndex;
                    if (rowIndex >= 0)
                    {
                        tb_chepai.Text = dgv.Rows[rowIndex].Cells[0].Value.ToString().Trim();
                        tb_chepai.Select(tb_chepai.Text.Length, 0);
                        dgv.DataSource = null;
                        dgv.Visible = false;
                    }
                }
                tb_cp.Focus();
            }
        }
        
        /// <summary>
        /// 车牌号：焦点离开事件
        /// </summary>
        private void tb_chepai_Leave(object sender, EventArgs e)
        {
            dgv_cars.DataSource = null;
            dgv_cars.Visible = false;
        }


        /// <summary>
        /// 采购单位：Value变化事件
        /// </summary>
        private void tb_cp_TextChanged(object sender, EventArgs e)
        {
            bool IsResetDgv = true;
            string search = tb_cp.Text.Trim().Replace("'", "");
            if (search.Length > 1)
            {
                string sql = string.Format(@"SELECT TOP 10 Gsm AS '单位名称', Owner as '负责人'  FROM Company WHERE Gsm LIKE '%{0}%' OR beizhu LIKE '%{0}%'", search);
                DataTable table = SqlHelper.GetDataTableBySQL(sql);
                if (table.Rows.Count > 0)
                {
                    dgv_company.DataSource = table;
                    dgv_company.Location = this.p_cp;
                    dgv_company.BringToFront();
                    dgv_company.Visible = true;
                    dgv_company.Height = dgv_company.Rows.Count * dgv_company.RowTemplate.Height + dgv_company.ColumnHeadersHeight + 5;
                    IsResetDgv = false;
                }
            }
            if (IsResetDgv)
            {
                dgv_company.DataSource = null;
                dgv_company.Visible = false;
            }

        }
        /// <summary>
        /// 采购单位：键盘事件
        /// </summary>
        private void tb_cp_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dgv = this.dgv_company;
            // 上下键
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                int rowsCount = dgv.Rows.Count;
                if (rowsCount > 0 && dgv.CurrentCell != null)
                {
                    int rowIndex = dgv.CurrentCell.RowIndex;
                    if (e.KeyCode == System.Windows.Forms.Keys.Down)
                    {
                        rowIndex = (rowIndex < rowsCount - 1) ? rowIndex + 1 : 0;
                    }
                    else if (e.KeyCode == System.Windows.Forms.Keys.Up)
                    {
                        rowIndex = (rowIndex == 0) ? rowsCount - 1 : rowIndex - 1;
                    }
                    dgv.CurrentCell = dgv.Rows[rowIndex].Cells[0];
                }
            }
            // Enter键
            if (e.KeyCode == Keys.Enter)
            {
                if (dgv.Rows.Count > 0)
                {
                    int rowIndex = dgv.CurrentCell.RowIndex;
                    if (rowIndex >= 0)
                    {
                        tb_cp.Text = dgv.Rows[rowIndex].Cells[0].Value.ToString().Trim();
                        tb_cp.Select(tb_cp.Text.Length, 0);
                        dgv.DataSource = null;
                        dgv.Visible = false;
                    }
                }
            }
        }
        /// <summary>
        /// 采购单位：失去焦点事件
        /// </summary>
        private void tb_cp_Leave(object sender, EventArgs e)
        {
            dgv_company.DataSource = null;
            dgv_company.Visible = false;
        }

        /// <summary>
        /// 搜索按钮
        /// </summary>
        private void btn_submit_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        /// <summary>
        /// 删除，撤销，打印
        /// </summary>
        private void dgv_in_records_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 如果点击的是表头则退出
            if (e.RowIndex < 0)
            {
                return;
            }

            this.id = Convert.ToInt32(dgv_records.Rows[e.RowIndex].Cells["ID"].Value);

            if (dgv_records.Columns[e.ColumnIndex].Name == "删除")
            {
                if (MessageBox.Show("您确定要删除这条记录吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (Util.DeleteRecord(this.id) == true)
                    {
                        MessageBox.Show("删除成功！");
                        this.paginator1.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("删除失败！");
                    }
                }
            }
            else if (dgv_records.Columns[e.ColumnIndex].Name == "撤销")
            {
                if (Util.CancelRecordOut(this.id) == true)
                {
                    MessageBox.Show("撤销成功！");
                    this.paginator1.Refresh();
                }
                else
                {
                    MessageBox.Show("撤销失败！");
                }
            }
            else if (dgv_records.Columns[e.ColumnIndex].Name == "打印")
            {
                // 整理Report Data Source
                string sql = string.Format(@"SELECT *, Users.xingming as 'outXingming' FROM Records LEFT JOIN Users ON Records.OutUname = Users.uname  where Records.ID = {0}", this.id);
                DataTable table = SqlHelper.GetDataTableBySQL(sql);
                DataRow row = table.Rows[0];
                DataTable tb = new DataTable("DataTable1");
                tb.Columns.Add(new DataColumn() { ColumnName = "col1", DataType = typeof(string) });
                tb.Columns.Add(new DataColumn() { ColumnName = "col2", DataType = typeof(string) });
                tb.Columns.Add(new DataColumn() { ColumnName = "col3", DataType = typeof(string) });
                tb.Rows.Add(tb.NewRow());
                tb.Rows.Add(tb.NewRow());
                tb.Rows.Add(tb.NewRow());
                tb.Rows.Add(tb.NewRow());
                tb.Rows[0][0] = "车牌";
                tb.Rows[0][1] = "种类";
                tb.Rows[0][2] = "其他";
                tb.Rows[1][0] = row["chepai"].ToString();
                tb.Rows[1][1] = row["Kind"].ToString();
                tb.Rows[1][2] = "";
                tb.Rows[2][0] = "出厂重量";
                tb.Rows[2][1] = "进厂重量";
                tb.Rows[2][2] = "净重量";
                tb.Rows[3][0] = row["InWeight"].ToString();
                tb.Rows[3][1] = row["OutWeight"].ToString();
                tb.Rows[3][2] = row["NetWeight"].ToString();

                ReportParameter p_company = new ReportParameter("p_company", row["Company"].ToString() + "采购单");
                ReportParameter p_outuser = new ReportParameter("p_outuser", "操作员：" + row["outXingming"].ToString());
                ReportParameter p_outtime = new ReportParameter("p_outtime", Convert.IsDBNull(row["OutTime"]) ? "" : "时间：" + Convert.ToDateTime(row["OutTime"]).ToString("yyyy-MM-dd hh:mm"));
                ReportParameter[] parms = new ReportParameter[] { p_company, p_outuser, p_outtime };
                ReportDataSource source = new ReportDataSource();
                source.Name = "DataRecord";
                source.Value = tb;

                MyUIReportViwer frmRPT = new MyUIReportViwer();
                frmRPT.MainDataSource = source;
                frmRPT.ReportParms = parms;
                frmRPT.ReportID = "PrintSetRecordOut";
                frmRPT.ReportPath = System.Windows.Forms.Application.StartupPath + @"\RdlcRecord.rdlc";
                frmRPT.ShowDialog();
            }
        }

        private void paginator1_PageChanged(object sender, int total, int cur, int per)
        {
            DataTable table = SqlHelper.GetDataTableBySQL(Util.PaginatorSQL(sql_query, cur, per), this.sql_parms);

            DataGridView dgv = this.dgv_records;
            Util.DgvClear(dgv);
            dgv.DataSource = table;
            Util.DgvColWidth(dgv, "序号", 60);
            Util.DgvColWidth(dgv, "料种", 60);
            Util.DgvColHide(dgv, "ID");

            DataGridViewButtonColumn dgv_button_col3 = new DataGridViewButtonColumn();
            dgv_button_col3.Name = "打印";
            dgv_button_col3.UseColumnTextForButtonValue = true;
            dgv_button_col3.Text = "打印";
            dgv_button_col3.HeaderText = "打印";
            dgv.Columns.Insert(0, dgv_button_col3);
            dgv.Columns["打印"].Width = 60;
            dgv.Columns["打印"].DefaultCellStyle.Padding = new Padding(2, 0, 2, 0);

            DataGridViewButtonColumn dgv_button_col2 = new DataGridViewButtonColumn();
            dgv_button_col2.Name = "撤销";
            dgv_button_col2.UseColumnTextForButtonValue = true;
            dgv_button_col2.Text = "撤销";
            dgv_button_col2.HeaderText = "撤销";
            dgv.Columns.Insert(0, dgv_button_col2);
            dgv.Columns["撤销"].Width = 60;
            dgv.Columns["撤销"].DefaultCellStyle.Padding = new Padding(2, 0, 2, 0);

            DataGridViewButtonColumn dgv_button_col1 = new DataGridViewButtonColumn();
            dgv_button_col1.Name = "删除";
            dgv_button_col1.UseColumnTextForButtonValue = true;
            dgv_button_col1.Text = "删除";
            dgv_button_col1.HeaderText = "删除";
            dgv.Columns.Insert(0, dgv_button_col1);
            dgv.Columns["删除"].Width = 60;
            dgv.Columns["删除"].DefaultCellStyle.Padding = new Padding(2, 0, 2, 0);

        }

        private void paginator1_ExportExcel(object sender, int total, int cur, int per)
        {

            this.saveFileDialog1.Title = "导出数据";
            this.saveFileDialog1.Filter = "Excel工作簿|*.xls";
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 整理数据
                DataTable table = SqlHelper.GetDataTableBySQL(this.sql_query, this.sql_parms);
                Util.DataTableRemoveCol(table, "ID");
                // 数量过大
                if (table.Rows.Count > 50000)
                {
                    if (MessageBox.Show("记录数大于5万条，确定要继续吗？", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    {
                        return;
                    }
                }
                // 导出数据
                string filename = this.saveFileDialog1.FileName;
                if (Util.ExportExcelWithAspose(table, filename, "明细"))
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