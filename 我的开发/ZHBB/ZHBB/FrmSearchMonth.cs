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
    public partial class FrmSearchMonth : Form
    {

        public FrmSearchMonth()
        {
            InitializeComponent();
        }

        private void FrmRecordAdd_Load(object sender, EventArgs e)
        {
            this.lb_month.Text = this.dtp_month.Value.Month.ToString() + "月";
            this.RefreshRecords();
        }


        /// <summary>
        /// 根据搜索条件，返回DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetResultDataTable()
        {
            int year = this.dtp_month.Value.Year;
            int month = this.dtp_month.Value.Month;
            int day = DateTime.DaysInMonth(year, month);
            SqlParameter p_year_begin = Util.NewSqlParameter("@p_year_begin", SqlDbType.DateTime, new DateTime(year, month, 1, 0, 0, 0));
            SqlParameter p_year_end = Util.NewSqlParameter("@p_year_end", SqlDbType.DateTime, new DateTime(year, month, day, 23, 59, 59));
            string sql = string.Format(@"
                            SELECT DAY(OutTime) AS '日期', SUM(NetWeight) AS '净重'
                            FROM Records
                            WHERE IsClose = 1 AND OutTime >= @p_year_begin AND OutTime <= @p_year_end
                            GROUP BY DAY(OutTime)
                            ORDER BY '日期' ASC");
            DataTable table = SqlHelper.GetDataTableBySQL(sql, p_year_begin, p_year_end);
            return table;
        }

        public void RefreshSum()
        {
            int year = this.dtp_month.Value.Year;
            int month = this.dtp_month.Value.Month;
            int day = DateTime.DaysInMonth(year, month);
            SqlParameter p_year_begin = Util.NewSqlParameter("@p_year_begin", SqlDbType.DateTime, new DateTime(year, month, 1, 0, 0, 0));
            SqlParameter p_year_end = Util.NewSqlParameter("@p_year_end", SqlDbType.DateTime, new DateTime(year, month, day, 23, 59, 59));
            string sql = string.Format(@"
                            SELECT ISNULL(SUM(NetWeight), 0)
                            FROM Records 
                            WHERE IsClose = 1 AND OutTime >= @p_year_begin AND OutTime <= @p_year_end");
            this.lb_sum.Text = string.Format("合计：{0}吨", SqlHelper.GetFirstCellStringBySQL(sql, p_year_begin, p_year_end));
        }

        /// <summary>
        /// 显示搜索记录
        /// </summary>
        public void RefreshRecords()
        {
            // 合计
            this.RefreshSum();

            //查询
            DataGridView dgv = dgv_records;
            Util.DgvClear(dgv);
            dgv.DataSource = this.GetResultDataTable();
        }

        /// <summary>
        /// 搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_submit_Click(object sender, EventArgs e)
        {
            this.RefreshRecords();
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_excel_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.Title = "导出数据";
            this.saveFileDialog1.Filter = "Excel工作簿|*.xls";
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 整理数据
                DataTable table = this.GetResultDataTable();
                // 导出数据
                string filename = this.saveFileDialog1.FileName;
                if (Util.ExportExcelWithAspose(table, filename, "汇总"))
                {
                    MessageBox.Show("导出成功！", "提示");
                }
                else
                {
                    MessageBox.Show("导出失败", "提示");
                }
            }
        }


        private void dtp_begin_ValueChanged(object sender, EventArgs e)
        {
            this.lb_month.Text = this.dtp_month.Value.Year.ToString() + "年";
        }

    }
}