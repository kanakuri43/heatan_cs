using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace heatan
{
    internal class Program
    {


        static void Main(string[] args)
        {
            SerialPort port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            port.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            try
            {
                port.Open();
                port.DtrEnable = true;
                port.RtsEnable = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
            Console.ReadLine();
            port.Close();
            port.Dispose();
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            string data = port.ReadExisting();   // ポートから文字列を受信する
            if (!string.IsNullOrEmpty(data))
            {
                string[] receivedDataArray = data.Split('\u002C');

                Console.Write(receivedDataArray[0], receivedDataArray[1]);
                //    Invoke((MethodInvoker)(() =>    // 受信用スレッドから切り替えてデータを書き込む
                //    {
                //        label2.Text = data; // ラベルを受信した文字列へ変更
                //        Thread.Sleep(1);
                //        button1.Enabled = true; // ボタンを押せるようにしておく
                //    }));
                //}

            }
        }

        private void DataInsert()
        {
            var dc = new DatabaseController();
            using (SqlCommand command = new SqlCommand("usp_insert_room_status", dc.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@arg_sense_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@arg_temperature", 12.34);
                command.Parameters.AddWithValue("@arg_heating_status", 1);

                command.Parameters.Add("ReturnValue", SqlDbType.Int);
                command.Parameters["ReturnValue"].Direction = ParameterDirection.ReturnValue;

                command.ExecuteNonQuery();

            }
        }

    }
}
