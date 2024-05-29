﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports; //COMポートを使うためです。
using System.Text;     
using Ivi.Visa.Interop; //USBとGPIBを使うためです。
using Aspose.Cells; //Excelの出力に使います。
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.Common;
using System.Security.Cryptography;


namespace MO_test9
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());



        }
    }

    public class Data
    {
        public double mag;
        public double faraday_deg;
    }

    //実働する関数を書きます
    class Function
    {
        public void Nonmedia_measure()
        {
            Device device = new Device("COM3", "USB::0x0B3E::0x104A::CP002893::INSTR", "GPIB::6::INSTR");
            Result_data result_Data = new Result_data();
            Measurment measurment = new Measurment(device.Ref_serialport(), device.Ref_multi());
            Electromagnet electromagnet = new Electromagnet(device.Ref_mag());

            device.Open();
            result_Data.Workbook_set();

            double nonmedia_theta = measurment.Ref_nonmediatheta();
            nonmedia_theta = measurment.Measurement_main(nonmedia_theta);
            measurment.Nonmedia_theta_write(nonmedia_theta);

            //データをワークシートに格納します。
            var datalist_theta = measurment.Ref_datalist_theta();
            var datalist_intensity = measurment.Ref_datalist_intensity();

            int column = 0, row = 2;

            foreach(double data in datalist_theta)
            {
                result_Data.Workbook_write(row, column, data);
                row++;
            }

            column = 1;  row = 2;

            foreach (double data in datalist_intensity)
            {
                result_Data.Workbook_write(row, column, data);
                row++;
            }

            result_Data.Workbook_save();
            device.Close();
        }

        public void Faraday_measure(double h_max, double dh)
        {
            Device device = new Device("COM3", "USB::0x0B3E::0x104A::CP002893::INSTR", "GPIB::6::INSTR");
            Result_data result_Data = new Result_data();
            Measurment measurment = new Measurment(device.Ref_serialport(), device.Ref_multi());
            Electromagnet electromagnet = new Electromagnet(device.Ref_mag());

            device.Open();
            //メディア無し角度を持って来ます。
            double nonmedia_theta = measurment.Ref_nonmediatheta();
            int column = 4;
            Hold_data hold_data = new Hold_data();

            //第１象限
            hold_data = Faraday_measure_sub(measurment, electromagnet, result_Data, 0,
                dh, false, hold_data.datas_mag, hold_data.datas_faradaydeg, nonmedia_theta, column);

            //第２象限
            hold_data = Faraday_measure_sub(measurment, electromagnet, result_Data, h_max,
                dh, true, hold_data.datas_mag, hold_data.datas_faradaydeg, hold_data.start_theta, hold_data.column);

            //第３象限
            hold_data = Faraday_measure_sub(measurment, electromagnet, result_Data, 0,
                dh, true, hold_data.datas_mag, hold_data.datas_faradaydeg, hold_data.start_theta, hold_data.column);

            //第４象限
            hold_data = Faraday_measure_sub(measurment, electromagnet, result_Data, h_max,
               dh, false, hold_data.datas_mag, hold_data.datas_faradaydeg, hold_data.start_theta, hold_data.column);

            //最終的なデータを格納します。
            int row = 2;
            foreach (double data in hold_data.datas_mag)
            {
                result_Data.Workbook_write(row, 2, data);
                row++;
            }
            row = 2;
            foreach (double data in hold_data.datas_faradaydeg)
            {
                result_Data.Workbook_write(row, 3, data);
                row++;
            }

            result_Data.Workbook_save();
            device.Close();
        }

        public Hold_data Faraday_measure_sub(Measurment measurment, Electromagnet electromagnet, Result_data result_Data,
            double target_h,double dh,Boolean flag_reverse,List<double> datas_mag, List<double> datas_faradaydeg, double start_theta, int column)
        {
            int row = 2;

            if (target_h == 0)
            {

                for (double current_h = target_h; current_h > 0; current_h -= dh)
                {
                    datas_mag.Add(electromagnet.Mag_output(current_h, flag_reverse));
                    start_theta = measurment.Measurement_main(start_theta);
                    datas_faradaydeg.Add(start_theta);

                    var datalist_theta = measurment.Ref_datalist_theta();
                    var datalist_intensity = measurment.Ref_datalist_intensity();

                    result_Data.Workbook_writestr(1, column, "Theta[deg]");
                    result_Data.Workbook_writestr(1, column + 1, "Light intensity[V]");

                    foreach (double data in datalist_theta)
                    {
                        result_Data.Workbook_write(row, column, data);
                        row++;
                    }

                    column += 1; row = 2;

                    foreach (double data in datalist_intensity)
                    {
                        result_Data.Workbook_write(row, column, data);
                        row++;
                    }

                    column += 1; row = 2;

                    if (current_h <= dh)
                    {
                        current_h = 0;
                    }
                }

            } else
            {
                for (double current_h = 0; current_h < target_h; current_h += dh)
                {
                    datas_mag.Add(electromagnet.Mag_output(current_h, flag_reverse));
                    start_theta = measurment.Measurement_main(start_theta);
                    datas_faradaydeg.Add(start_theta);

                    var datalist_theta = measurment.Ref_datalist_theta();
                    var datalist_intensity = measurment.Ref_datalist_intensity();

                    result_Data.Workbook_writestr(1, column, "Theta[deg]");
                    result_Data.Workbook_writestr(1, column + 1, "Light intensity[V]");

                    foreach (double data in datalist_theta)
                    {
                        result_Data.Workbook_write(row, column, data);
                        row++;
                    }

                    column += 1; row = 2;

                    foreach (double data in datalist_intensity)
                    {
                        result_Data.Workbook_write(row, column, data);
                        row++;
                    }

                    column += 1; row = 2;

                    if (current_h <= dh)
                    {
                        current_h = 0;
                    }
                }
            }

            Hold_data hold_data = new Hold_data();
            hold_data.start_theta = start_theta;
            hold_data.column = column;
            hold_data.datas_mag = datas_mag;
            hold_data.datas_faradaydeg = datas_faradaydeg;

            return hold_data;

        }
    }

    class Hold_data
    {
        public double start_theta;
        public int column;
        public List<double> datas_mag = new List<double>();
        public List<double> datas_faradaydeg = new List<double>();
    }


    //デバイスの接続をします。
    class Device
    {
        string comNum; //COMポートの番号です。
        string pInst_magpow; //電磁石電源のインスタンスパスです。
        string pInst_multi; //マルチメータのインスタンスパスです。

        SerialPort serialPort;
        ResourceManager rm_magpow = new ResourceManager();
        FormattedIO488 msg_magpow = new FormattedIO488();
        ResourceManager rm_multi = new ResourceManager();
        FormattedIO488 msg_multi = new FormattedIO488();

        public Device(string comNum, string pInst_magpow, string pInst_multi) 
        {
            //コンストラクタを書くところです。
            this.comNum = comNum;
            this.pInst_magpow = pInst_magpow;
            this.pInst_multi = pInst_multi;

            //  COMポートの設定をします。
            string portName = comNum; // 使用するCOMポート名を指定してください"COM3"
            int baudRate = 9600;
            Parity parity = Parity.None;
            int dataBits = 8;
            StopBits stopBits = StopBits.One;
            // シリアルポートのインスタンスを作成
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            serialPort.ReadTimeout = 1500;
            serialPort.WriteTimeout = 1500;
            serialPort.Handshake = Handshake.None;  //ハンドシェイク
            serialPort.Encoding = Encoding.UTF8;          //エンコード
            serialPort.NewLine = "\r";                   //改行コード指定
        }

        public void Open()
        {
            //  COMポートを開きます。
            serialPort.Open();

            //IDN?ここは消しても問題ないです。
            serialPort.WriteLine("*IDN?");

            string response = serialPort.ReadLine();

            Console.WriteLine(response);
            //

            //インスタンスパスで開く機器を開きます。
            //電磁石電源です。"USB::0x0B3E::0x104A::CP002893::INSTR"
            msg_magpow.IO = (IMessage)rm_magpow.Open(pInst_magpow, AccessMode.NO_LOCK, 0, "");

            //IDN?
            msg_magpow.WriteString("*IDN?");

            response = msg_magpow.ReadString();

            Console.WriteLine(response);
            //

            //マルチメータです。"GPIB::6::INSTR"
            msg_multi.IO = (IMessage)rm_multi.Open(pInst_multi, AccessMode.NO_LOCK, 0, "");

            //IDN?
            msg_multi.WriteString("*IDN?");

            response = msg_multi.ReadString();

            Console.WriteLine(response);
        }

        public void Close()
        {
            serialPort.Close();
            msg_magpow.IO.Close();
            msg_multi.IO.Close();
        }

        public SerialPort Ref_serialport()
        {
            return serialPort;
        }

        public FormattedIO488 Ref_mag()
        {
            return msg_magpow;
        }

        public FormattedIO488 Ref_multi()
        {
            return msg_multi;
        }
    }


    class Result_data
    {
        Workbook workbook = new Workbook();
        Worksheet worksheet1;
        Worksheet worksheet2;

        public void Workbook_set()
        {
            worksheet1 = workbook.Worksheets[0];
            worksheet2 = workbook.Worksheets[workbook.Worksheets.Add()];
            worksheet1.Name = "Data";
            worksheet2.Name = "Graph";

            worksheet1.Cells[0, 0].PutValue("Non-media collect datas");
            worksheet1.Cells[1, 0].PutValue("Theta[deg]");
            worksheet1.Cells[1, 1].PutValue("Light intensity[V]");
            worksheet1.Cells[0, 2].PutValue("Fraday rotation");
            worksheet1.Cells[1, 2].PutValue("H[mT]");
            worksheet1.Cells[1, 3].PutValue("ratation angle[deg]");
            worksheet1.Cells[0, 4].PutValue("Faraday rotation cllect datas");

            workbook.Save("MO_data.xlsx");
        }

        //あとで書き込み関数書きます
        public void Workbook_write(int row,int column,double value)
        {
            worksheet1.Cells[row, column].PutValue(value);
        }

        public void Workbook_writestr(int row, int column, string value)
        {
            worksheet1.Cells[row, column].PutValue(value);
        }

        public void Workbook_save()
        {
            workbook.Save("MO_data.xlsx");
        }
    }

    class Measurment
    {
        double current_theta;
        double nonmedia_theta;

        List<double> datalist_intensity = new List<double>();
        List<double> datalist_theta = new List<double>();

        SerialPort serialPort;
        FormattedIO488 msg_multi;

        public Measurment(SerialPort serialPort, FormattedIO488 msg_mult)
        {
            this.serialPort = serialPort;
            this.msg_multi = msg_mult;
        }

        public void Theta_read()
        {
            //textからThetaを読み込みます

            string path = @"C:\Users\yohei\Downloads\MO_test9\MO_test9\datalist.txt";

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("Shift_JIS"));

            current_theta = double.Parse(sr.ReadLine());   //doubleにstringから変換します
            nonmedia_theta = double.Parse(sr.ReadLine());

            sr.Close();
        }

        public void Theta_write()
        {
            string path = @"C:\Users\yohei\Downloads\MO_test9\MO_test9\datalist.txt";

            StreamWriter sw = new StreamWriter(path, false);

            sw.WriteLine(current_theta);
            sw.WriteLine(nonmedia_theta);

            sw.Close();
        }

        //測定関数です。
        public double Measurement_main(double start_theta) //引数は、測定の開始角度を入れます
        {// 最下点のファラデー回転角を磁界とともに返すメイン測定関数です。

            Theta_read();

            datalist_intensity = new List<double>();
            datalist_theta = new List<double>();

            double dtheta = 250;
            int data_collectflag = 0;

            Boolean start_flag = false;
            while (start_flag == false)
            {
                //スタート地点から上下をはかり、どちらが減少傾向にあるか確かめます。
                //もしも同じ値が出たら、dthetaを小さくしてもう一回します。
                current_theta = start_theta;

                datalist_intensity.Add(Moveing_and_reading(0));
                datalist_theta.Add(current_theta);

                datalist_intensity.Add(Moveing_and_reading(dtheta));
                datalist_theta.Add(current_theta);

                current_theta = start_theta;

                datalist_intensity.Add(Moveing_and_reading(-dtheta));
                datalist_theta.Add(current_theta);

                if (datalist_intensity[1] - datalist_intensity[2] != 0)
                {
                    if (datalist_intensity[1] - datalist_intensity[2] > 0)
                    {
                        dtheta = -dtheta;
                    }
                    start_flag = true;
                }
                else
                {
                    dtheta /= 2;
                }
            }

            //決めた方向に向けて測定をします。
            Boolean revflag = false;
            int datanum = 0;
            int pre_datanum = 0;
            while (data_collectflag < 5 || datalist_intensity.Count < 20)
            {
                datalist_intensity.Add(Moveing_and_reading(dtheta));
                datalist_theta.Add(current_theta);

                datanum = datalist_intensity.Count - 1;
                pre_datanum = datanum - 1;

                if (data_collectflag < 5 && datalist_intensity[datanum] - datalist_intensity[pre_datanum] > 0)
                {
                    data_collectflag++;
                }

                if (data_collectflag >= 5 && revflag == false)
                {
                    current_theta += dtheta / 2;
                    dtheta = -dtheta;
                    revflag = true;
                }
            }

            //測定した中で、最小の点を探します。
            double data_min = datalist_intensity[0];
            datanum = 0;
            int i = 0;
            foreach (double data_compare in datalist_intensity)
            {
                if (data_min >= data_compare)
                {
                    data_min = data_compare;
                    datanum = i;
                }
                i++;
            }

            //最小の点から左右に20点ずつ細かくとります。
            current_theta = datalist_theta[datanum];
            dtheta = 25;

            for (i = 0; i <= 39; i++)
            {
                datalist_intensity.Add(Moveing_and_reading(dtheta));
                datalist_theta.Add(current_theta);

                if (i == 19)
                {
                    current_theta = datalist_theta[datanum];
                    dtheta = -dtheta;
                }
            }

            //データを取り終わりました。近似曲線を出して、最下点を推定します。
            double[,] datamatrix_intensity = new double[datalist_intensity.Count, 3];
            double[,] datamatrix_theta = new double[datalist_theta.Count, 1];

            for (int j = 0; j <= datalist_intensity.Count; j++)
            {
                datamatrix_intensity[j, 0] = datalist_theta[j] * datalist_theta[j];
                datamatrix_intensity[j, 1] = datalist_theta[j];
                datamatrix_intensity[j, 2] = 1;

                datamatrix_theta[j, 0] = datalist_theta[j];
            }

            Theta_write();

            Calculation calculation = new Calculation();

            double data = calculation.approximation(datamatrix_intensity, datamatrix_theta);

            return data;
        }


        public double Moveing_and_reading(double dtheta)//動かしたい角度を要求して、回して、その先のマルチメータの電圧を返します。
        {
            //角度を足します。
            current_theta += dtheta;
            serialPort.WriteLine("AXI1:GOABS " + current_theta.ToString());
            Delay();

            //マルチメータに値を尋ねます。
            msg_multi.WriteString("*IDN?");
            string response = msg_multi.ReadString();
            string[] response_arry = response.Split('V');

            return (double.Parse(response_arry[0]));
        }

        public void Delay()
        {
            double getpos = 0, pre_getpos = 1;

            while (getpos - pre_getpos != 0)
            {

                pre_getpos = getpos;
                serialPort.WriteLine("AXI1:POS?");
                string response = serialPort.ReadLine();
                getpos = double.Parse(response);

                if (getpos - pre_getpos == 1)
                {
                    pre_getpos = 0;
                }
            }
        }

        public List<double> Ref_datalist_intensity()
        {
            return datalist_intensity;
        }

        public List<double> Ref_datalist_theta()
        {
            return datalist_theta;
        }

        public double Ref_nonmediatheta()
        {
            return nonmedia_theta;
        }

        public void Nonmedia_theta_write(double new_nonmedia_theta)
        {
            nonmedia_theta = new_nonmedia_theta;
            Theta_write();
        }
    }


    class Electromagnet
    {
        FormattedIO488 msg_magpow;
        public Electromagnet(FormattedIO488 msg_magpow)
        {   
            this.msg_magpow = msg_magpow;
        }

        //電磁石を動かすやつです。
        public double Mag_output(double h_target, Boolean flag_reverse)
        {
            double cur = 0.00378515 * h_target - 0.02470835;
            cur = Math.Round(cur, 3, MidpointRounding.AwayFromZero);

            Convert.ToString(cur);

            msg_magpow.WriteString("CURR " + cur);
            msg_magpow.WriteString("OUTP 1");

            if (flag_reverse == false)
            {
                return (264.19 * cur + 6.5277);
            }
            else
            {
                return (-264.19 * cur + 6.5277);
            }
        }


    }
}

