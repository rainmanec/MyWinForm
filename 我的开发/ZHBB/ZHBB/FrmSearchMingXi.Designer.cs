﻿namespace ZHBB
{
    partial class FrmSearchMingXi
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.paginator1 = new MYUI.Paginator();
            this.cb_kind = new System.Windows.Forms.ComboBox();
            this.dtp_end = new System.Windows.Forms.DateTimePicker();
            this.dtp_begin = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_cp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_submit = new System.Windows.Forms.Button();
            this.tb_chepai = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv_cars = new System.Windows.Forms.DataGridView();
            this.dgv_records = new System.Windows.Forms.DataGridView();
            this.dgv_company = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_cars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_records)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_company)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.paginator1);
            this.panel1.Controls.Add(this.cb_kind);
            this.panel1.Controls.Add(this.dtp_end);
            this.panel1.Controls.Add(this.dtp_begin);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tb_cp);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btn_submit);
            this.panel1.Controls.Add(this.tb_chepai);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(847, 137);
            this.panel1.TabIndex = 0;
            // 
            // paginator1
            // 
            this.paginator1.Location = new System.Drawing.Point(373, 98);
            this.paginator1.Name = "paginator1";
            this.paginator1.Size = new System.Drawing.Size(389, 23);
            this.paginator1.TabIndex = 22;
            this.paginator1.PageChanged += new MYUI.Paginator.PageChangeHandle(this.paginator1_PageChanged);
            this.paginator1.ExportExcel += new MYUI.Paginator.ExportExcelHandle(this.paginator1_ExportExcel);
            // 
            // cb_kind
            // 
            this.cb_kind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_kind.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_kind.FormattingEnabled = true;
            this.cb_kind.Location = new System.Drawing.Point(91, 98);
            this.cb_kind.Name = "cb_kind";
            this.cb_kind.Size = new System.Drawing.Size(257, 20);
            this.cb_kind.TabIndex = 20;
            // 
            // dtp_end
            // 
            this.dtp_end.Location = new System.Drawing.Point(232, 10);
            this.dtp_end.Name = "dtp_end";
            this.dtp_end.Size = new System.Drawing.Size(116, 21);
            this.dtp_end.TabIndex = 11;
            // 
            // dtp_begin
            // 
            this.dtp_begin.Location = new System.Drawing.Point(91, 10);
            this.dtp_begin.Name = "dtp_begin";
            this.dtp_begin.Size = new System.Drawing.Size(120, 21);
            this.dtp_begin.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "时间段：";
            // 
            // tb_cp
            // 
            this.tb_cp.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_cp.Location = new System.Drawing.Point(91, 68);
            this.tb_cp.Name = "tb_cp";
            this.tb_cp.Size = new System.Drawing.Size(257, 21);
            this.tb_cp.TabIndex = 8;
            this.tb_cp.TextChanged += new System.EventHandler(this.tb_cp_TextChanged);
            this.tb_cp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_cp_KeyDown);
            this.tb_cp.Leave += new System.EventHandler(this.tb_cp_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(18, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "采购单位：";
            // 
            // btn_submit
            // 
            this.btn_submit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_submit.Location = new System.Drawing.Point(373, 10);
            this.btn_submit.Name = "btn_submit";
            this.btn_submit.Size = new System.Drawing.Size(141, 79);
            this.btn_submit.TabIndex = 5;
            this.btn_submit.Text = "提交";
            this.btn_submit.UseVisualStyleBackColor = true;
            this.btn_submit.Click += new System.EventHandler(this.btn_submit_Click);
            // 
            // tb_chepai
            // 
            this.tb_chepai.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_chepai.Location = new System.Drawing.Point(91, 39);
            this.tb_chepai.Name = "tb_chepai";
            this.tb_chepai.Size = new System.Drawing.Size(257, 21);
            this.tb_chepai.TabIndex = 2;
            this.tb_chepai.TextChanged += new System.EventHandler(this.tb_chepai_TextChanged);
            this.tb_chepai.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_chepai_KeyDown);
            this.tb_chepai.Leave += new System.EventHandler(this.tb_chepai_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(42, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "料种：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(30, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "车牌号：";
            // 
            // dgv_cars
            // 
            this.dgv_cars.AllowUserToAddRows = false;
            this.dgv_cars.AllowUserToDeleteRows = false;
            this.dgv_cars.AllowUserToResizeColumns = false;
            this.dgv_cars.AllowUserToResizeRows = false;
            this.dgv_cars.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_cars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_cars.Location = new System.Drawing.Point(563, 207);
            this.dgv_cars.MultiSelect = false;
            this.dgv_cars.Name = "dgv_cars";
            this.dgv_cars.ReadOnly = true;
            this.dgv_cars.RowHeadersVisible = false;
            this.dgv_cars.RowTemplate.Height = 23;
            this.dgv_cars.Size = new System.Drawing.Size(257, 246);
            this.dgv_cars.TabIndex = 1;
            this.dgv_cars.Visible = false;
            // 
            // dgv_records
            // 
            this.dgv_records.AllowUserToAddRows = false;
            this.dgv_records.AllowUserToDeleteRows = false;
            this.dgv_records.AllowUserToResizeRows = false;
            this.dgv_records.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_records.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_records.Location = new System.Drawing.Point(0, 137);
            this.dgv_records.Name = "dgv_records";
            this.dgv_records.ReadOnly = true;
            this.dgv_records.RowTemplate.Height = 23;
            this.dgv_records.Size = new System.Drawing.Size(847, 376);
            this.dgv_records.TabIndex = 1;
            this.dgv_records.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_in_records_CellContentClick);
            // 
            // dgv_company
            // 
            this.dgv_company.AllowUserToAddRows = false;
            this.dgv_company.AllowUserToDeleteRows = false;
            this.dgv_company.AllowUserToResizeColumns = false;
            this.dgv_company.AllowUserToResizeRows = false;
            this.dgv_company.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_company.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_company.Location = new System.Drawing.Point(255, 207);
            this.dgv_company.MultiSelect = false;
            this.dgv_company.Name = "dgv_company";
            this.dgv_company.ReadOnly = true;
            this.dgv_company.RowHeadersVisible = false;
            this.dgv_company.RowTemplate.Height = 23;
            this.dgv_company.Size = new System.Drawing.Size(259, 246);
            this.dgv_company.TabIndex = 2;
            this.dgv_company.Visible = false;
            // 
            // FrmSearchMingXi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 513);
            this.Controls.Add(this.dgv_company);
            this.Controls.Add(this.dgv_cars);
            this.Controls.Add(this.dgv_records);
            this.Controls.Add(this.panel1);
            this.Name = "FrmSearchMingXi";
            this.Tag = "FrmSearchMingXi";
            this.Text = "明细查询";
            this.Load += new System.EventHandler(this.FrmRecordAdd_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_cars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_records)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_company)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_submit;
        private System.Windows.Forms.TextBox tb_chepai;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv_cars;
        private System.Windows.Forms.DataGridView dgv_records;
        private System.Windows.Forms.TextBox tb_cp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgv_company;
        private System.Windows.Forms.DateTimePicker dtp_end;
        private System.Windows.Forms.DateTimePicker dtp_begin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_kind;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private MYUI.Paginator paginator1;

    }
}