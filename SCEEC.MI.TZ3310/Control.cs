﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace SCEEC.MI.TZ3310
{
    //public class Control
    //{
    //    private SerialPort uart;
    //    const int timeout = 200;

    //    private static bool __uart_transmit_cmd(SerialPort port, byte[] cmd)
    //    {
    //        if (port == null) return false;
    //        byte[] sendBuffer = new byte[cmd.Length + 1];
    //        byte crc = 0;
    //        for (int i = 0; i < cmd.Length; i++)
    //        {
    //            crc += cmd[i];
    //            sendBuffer[i] = cmd[i];
    //        }
    //        sendBuffer[cmd.Length] = crc;
    //        try
    //        {
    //            if (!port.IsOpen) port.Open();
    //            port.Write(sendBuffer, 0, sendBuffer.Length);
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    //private static bool __uart_receive_data(SerialPort port, int timeout, out byte[] dat)
    //    //{
    //    //    const int inner_timeout = 10;
    //    //    int count = timeout;
    //    //    int bytes = 0;
    //    //    while (count > 0)
    //    //    {
    //    //        if (port.BytesToRead > bytes)
    //    //        {
    //    //            bytes = port.BytesToRead;
    //    //            count = inner_timeout;
    //    //        }
    //    //        else
    //    //        {
    //    //            count--;
    //    //            System.Threading.Thread.Sleep(1);
    //    //        }
    //    //    }

    //    //}

    //    //private static bool __uart_transceive_cmd(SerialPort port, byte[] cmd, int timeout, out byte[] dat)
    //    //{
            
    //    //}

    //    public static Control getSerialPortConnection()
    //    {
    //        SerialPort sp = new SerialPort();
    //        foreach (string spn in SerialPort.GetPortNames())
    //        {
    //            bool effective = false;
    //            try
    //            {
    //                sp = new SerialPort(spn, 115200, Parity.None, 8, StopBits.None);
    //                __uart_transmit_cmd(sp, new byte[] { 0x01, 0x02 });
    //                System.Threading.Thread.Sleep(200);
                    
    //            }
    //            catch (Exception)
    //            {
    //                effective = false;
    //            }
    //            if (effective)
    //            {
    //                return new Control() { uart = sp };
    //            }
    //        }
    //        return null;
    //    }
        
    //}
}
