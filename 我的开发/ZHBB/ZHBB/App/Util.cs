using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO.Ports;
using System.Configuration;
using System.Web.Script.Serialization;
namespace ZHBB
{
    partial class Util
    {

        #region 项目函数库

        #endregion

        #region 据库操作-常用表
        /// <summary>
        /// 添加一条日志
        /// </summary>
        /// <param name="uname">登录名</param>
        /// <param name="xingming">姓名</param>
        /// <param name="cnt">内容</param>
        public static void console_log(string uname, string xingming, string cnt)
        {
            SqlParameter[] paras = new SqlParameter[] {
                NewSqlParameter("@p_uname", SqlDbType.VarChar, uname, 20),
                NewSqlParameter("@p_xingming", SqlDbType.VarChar, xingming, 20),
                NewSqlParameter("@p_time", SqlDbType.DateTime, DateTime.Now),
                NewSqlParameter("@p_cnt", SqlDbType.VarChar, cnt, 200)
            };
            string sql = "INSERT INTO 日志(uname, xingming, 时间, 内容) VALUES(@p_uname, @p_xingming, @p_time, @p_cnt)";
            int affect = SqlHelper.ExecuteNonQuery(sql, paras);
        }

        /// <summary>
        /// 添加一条日志
        /// </summary>
        /// <param name="cnt">内容</param>
        public static void console_log(string cnt)
        {
            //Util.console_log(TransferData.uname, TransferData.xingming, cnt);
        }

        /// <summary>
        /// 如果车辆已存在，则不添加车辆；否则添加车辆；返回chepai的模糊搜索值
        /// </summary>
        /// <param name="chepai"></param>
        /// <returns></returns>
        public static string AddCar(string chepai)
        {
            if (chepai.Trim() == "")
            {
                return "";
            }

            // 车牌号
            chepai = chepai.ToUpper();
            string chepai2 = Util.GetLikeValue(chepai);
            SqlParameter p_chepai = Util.NewSqlParameter("@p_chepai", SqlDbType.VarChar, chepai, 50);
            SqlParameter p_chepai2 = Util.NewSqlParameter("@p_chepai2", SqlDbType.VarChar, chepai2, 50);

            string sql_count = "SELECT COUNT(*) FROM Cars WHERE chepai = @p_chepai";
            string total = SqlHelper.GetFirstCellStringBySQL(sql_count, p_chepai);
            if (total == "0")
            {
                string sql = @"insert into Cars(chepai, chepai2, owner, phone, address, beizhu) values (@p_chepai, @p_chepai2, '', '', '', '')";
                SqlHelper.ExecuteNonQuery(sql, p_chepai, p_chepai2);
            }
            return chepai2;
        }

        #endregion


        #region 数据库操作-Record表

        /// <summary>
        /// 撤销记录的出厂状态，即将出厂时间、出厂重量等清空
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CancelRecordOut(int id)
        {
            SqlParameter p_id = Util.NewSqlParameter("@p_id", SqlDbType.Int, id);
            string sql = string.Format(@"
                                UPDATE Records
                                SET
	                                OutTime = NULL, OutWeight = NULL, NetWeight = NULL,
	                                other = '', Kind = '', OutUname = '', IsClose = 0
                                WHERE ID = @p_id");
            return (SqlHelper.ExecuteNonQuery(sql, p_id) == 1);
        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteRecord(int id)
        {
            SqlParameter p_id = Util.NewSqlParameter("@p_id", SqlDbType.Int, id);
            int affect = SqlHelper.ExecuteNonQuery("DELETE FROM Records WHERE ID = @p_id", p_id);
            return (affect == 1);
        }
        #endregion


        #region 项目常用函数

        /// <summary>
        /// 获取传感器的重量
        /// </summary>
        /// <returns></returns>
        public static decimal GetSensorWeight()
        {
            string PortName = Util.GetAppConfig("PortName");
            string BaudRate = Util.GetAppConfig("BaudRate");
            if (PortName == "" || BaudRate == "")
            {
                MessageBox.Show("请选择串口号");
                return 0;
            }

            SerialPort comm = new SerialPort();

            comm.PortName = PortName;
            comm.BaudRate = Util.IntTryParse(BaudRate);
            comm.ReadTimeout = 1000;
            comm.WriteTimeout = 1000;
            comm.Open();
            comm.NewLine = "/r/n";
            comm.RtsEnable = true;//根据实际情况吧。  

            /*
            byte[] data = Encoding.Unicode.GetBytes(textBox1.Text);
            string str = Convert.ToBase64String(data);
            this.comm.WriteLine(str);
            */
            try
            {
                byte[] data = Convert.FromBase64String(comm.ReadLine());
                string result = Encoding.Unicode.GetString(data);
                return Util.DecimalTryParse(result);
            }
            catch (Exception ep)
            {
                MessageBox.Show(ep.ToString());
            }
            finally
            {
                comm.Close();
            }
            return 0;
        }

        /// <summary>
        /// 根据数据中的table初始化ComboBox
        /// </summary>
        /// <param name="cb">ComboBox对象</param>
        /// <param name="tb">表格名称</param>
        /// <param name="col">列名</param>
        /// <param name="flag">是否需</param>
        public static void InitComboBox(ComboBox cb, string tb, string col, bool flag)
        {
            DataTable table = SqlHelper.GetDataTableBySQL(string.Format("select * from {0} order by {1} ASC", tb, col));
            if (flag)
            {
                cb.Items.Add("");
            }
            foreach (DataRow row in table.Rows)
            {
                cb.Items.Add(row[col].ToString());
            }
        }

        public static void InitComboBox(ComboBox cb, string tb, string col)
        {
            Util.InitComboBox(cb, tb, col, false);
        }

        public static string GetDbConfig(string key)
        {
            string sql = string.Format("SELECT V FROM Config WHERE K = '{0}'", key);
            string value = SqlHelper.GetFirstCellStringBySQL(sql);
            return value;
        }
        public static bool SetDbConfig(string key, string value)
        {
            SqlParameter p_key = Util.NewSqlParameter("@p_key", SqlDbType.VarChar, key, 50);
            SqlParameter p_val = Util.NewSqlParameter("@p_val", SqlDbType.VarChar, value, 50);
            string sql = string.Format(@"UPDATE Config SET V = @p_val WHERE K = @p_key");
            int affect = SqlHelper.ExecuteNonQuery(sql, p_key, p_val);
            return (affect == 1);
        }


        /// <summary>
        /// 获取App.config
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppConfig(string key)
        {
            var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            if (!config.HasFile)
            {
                return null;
            }
            System.Configuration.KeyValueConfigurationElement k = config.AppSettings.Settings[key];
            if (k == null)
            {
                return null;
            }
            else
            {
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
        }

        /// <summary>
        /// 设置App.config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetAppConfig(string key, string value)
        {
            var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            if (!config.HasFile)
            {
                return false;   // throw new ArgumentException("程序配置文件缺失！");
            }
            System.Configuration.KeyValueConfigurationElement k = config.AppSettings.Settings[key];
            if (k == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(System.Configuration.ConfigurationSaveMode.Modified);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            return true;
        }

        /// <summary>
        /// 百分之一英寸单位，转化为厘米
        /// </summary>
        /// <param name="inch"></param>
        /// <returns></returns>
        public static string PrintMgToInch(string length)
        {
            decimal d = Util.DecimalTryParse(length);
            return Util.DecimalToString(d / 100);
        }

        /// <summary>
        /// 将CM转化为百分之一英寸单位
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public static int InchToPrintMg(string length)
        {
            decimal d = Util.DecimalTryParse(length);
            return Convert.ToInt32(d * 100);
        }


        /// <summary>
        /// Decimal转化为String
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n">位数</param>
        /// <returns></returns>
        public static string DecimalToString(decimal d, int n)
        {
            return Math.Round(d, n).ToString();
        }

        /// <summary>
        /// Decimal转化为String
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string DecimalToString(decimal d)
        {
            return Math.Round(d, 2).ToString();
        }

        /// <summary>
        /// 根据配置文件重新设置PrintDocument对象
        /// </summary>
        /// <param name="pd"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool SetPrintDocumentByConfig(System.Drawing.Printing.PrintDocument pd, string key)
        {
            string setting = Util.GetAppConfig(key);
            if (setting == null)
            {
                setting = "";
            }
            Hashtable hash = new Hashtable();
            hash = new JavaScriptSerializer().Deserialize<Hashtable>(setting);
            if (hash != null)
            {
                //// 边距
                //int top = (hash["MarginTop"] == null) ? 0 : Util.CmToPrintMg(hash["MarginRight"].ToString());
                //int right = (hash["MarginRight"] == null) ? 0 : Util.CmToPrintMg(hash["MarginRight"].ToString());
                //int bottom = (hash["MarginBottom"] == null) ? 0 : Util.CmToPrintMg(hash["MarginBottom"].ToString());
                //int left = (hash["MarginRight"] == null) ? 0 : Util.CmToPrintMg(hash["MarginRight"].ToString()) ;

                int left = (hash["MarginLeft"] == null) ? 0 : Util.InchToPrintMg(hash["MarginLeft"].ToString());
                int right = (hash["MarginRight"] == null) ? 0 : Util.InchToPrintMg(hash["MarginRight"].ToString());
                int top = (hash["MarginTop"] == null) ? 0 : Util.InchToPrintMg(hash["MarginTop"].ToString());
                int bottom = (hash["MarginBottom"] == null) ? 0 : Util.InchToPrintMg(hash["MarginBottom"].ToString());

                System.Drawing.Printing.Margins mg = new System.Drawing.Printing.Margins(left, right, top, bottom);
                pd.DefaultPageSettings.Margins = mg;

                // 打印机和纸张
                if (hash["Printer"] != null)
                {
                    foreach (String printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (hash["Printer"].ToString() == printer)
                        {
                            pd.PrinterSettings.PrinterName = printer;
                            if (hash["Paper"] != null)
                            {
                                foreach (System.Drawing.Printing.PaperSize ps in pd.PrinterSettings.PaperSizes)
                                {
                                    if (ps.PaperName == hash["Paper"].ToString())
                                    {
                                        pd.PrinterSettings.DefaultPageSettings.PaperSize = ps;
                                        pd.DefaultPageSettings.PaperSize = ps;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }


                // 横纵向
                if (hash["LandScape"] != null)
                {
                    pd.DefaultPageSettings.Landscape = (hash["LandScape"].ToString().ToUpper() == "H") ? true : false;
                }

            }            
            return true;
        }

    }
}
