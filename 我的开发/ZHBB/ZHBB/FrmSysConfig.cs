using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO.Ports;
namespace ZHBB
{
    public partial class FrmSysConfig : Form
    {

        public int IsSensor = 1;

        public SerialPort comm = new SerialPort();

        public FrmSysConfig()
        {
            InitializeComponent();
        }

        private void FrmPriceEdit_Load(object sender, EventArgs e)
        {
            // IsSensor
            string sensor = Util.GetDbConfig("IsSensor");
            if (sensor == "1")
            {
                this.SwitchIsSensorCheckbox(1);
            }
            else
            {
                this.SwitchIsSensorCheckbox(0);
            }
            if (TransferData.uroleid < 1)
            {
                this.cb_person.Enabled = false;
                this.cb_sensor.Enabled = false;
            }


            // 纸张
            string Paper1 = Util.GetAppConfig("PaperName");
            for(int i = 0, length = printDoc.PrinterSettings.PaperSizes.Count; i < length;i++)
            {
                PaperSize ps = printDoc.PrinterSettings.PaperSizes[i];
                this.cb_paper1.Items.Add(ps.PaperName);
                if (ps.PaperName == Paper1)
                {
                    this.cb_paper1.SelectedIndex = i;
                }
            }

            // 串口
            string PortName = Util.GetAppConfig("PortName");
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            for (int i = 0, length = ports.Length - 1; i < length; i++)
            {
                this.cb_port1.Items.Add(ports[i]);
                if (ports[i] == PortName)
                {
                    this.cb_port1.SelectedIndex = i;
                }
            }
        }





        public void SwitchIsSensorCheckbox(int which)
        {
            if (which == 0)
            {
                this.cb_person.Checked = true;
                this.cb_sensor.Checked = false;
                this.IsSensor = 0;
            }
            else
            {
                this.cb_person.Checked = false;
                this.cb_sensor.Checked = true;
                this.IsSensor = 1;
            }
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            if (this.cb_port1.Text.Trim() != "")
            {
                Util.SetAppConfig("PortName", this.cb_port1.Text.Trim());
            }
            if (this.cb_paper1.Text.Trim() != "")
            {
                Util.SetAppConfig("PaperName", this.cb_paper1.Text.Trim());
            }
            bool result = Util.SetDbConfig("IsSensor", this.IsSensor.ToString());
            if (result)
            {
                MessageBox.Show("提交成功");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("操作失败");
            }
        }


        private void cb_person_Click(object sender, EventArgs e)
        {
            this.SwitchIsSensorCheckbox(0);

        }

        private void cb_sensor_Click(object sender, EventArgs e)
        {
            this.SwitchIsSensorCheckbox(1);
        }

    }
}
