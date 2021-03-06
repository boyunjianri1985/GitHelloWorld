﻿using System;
using System.Collections.Generic;
using System.Linq;
using ModBus4;
using System.Threading;
using Modbus.Device;

namespace ModbusTest
{
    /// <summary>
    /// 测试程序入口
    /// </summary>
    static class Program
    {
        static void Main(string[] args)
        {
            //NewTestSerial();
            //NewTestTcp();
            NewTestUdp();
            while (true)
            {
                var input = Console.ReadLine();
                if (input.ToLower() == "exit")
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 串口测试
        /// </summary>
        static void NewTestSerial()
        {
            var serialc = new SerialConfig()
            {
                BaudRate = 9600,
                DataBits = 8,
                Parity = System.IO.Ports.Parity.None,
                StopBits = System.IO.Ports.StopBits.One,
                PortName = "com3"
            };
            Func(serialc);
        }

        /// <summary>
        /// tcp测试
        /// </summary>
        static void NewTestTcp()
        {
            var tcp = new IPConfig()
            {
                Address = "127.0.0.1",
                Port = 5250,
                Mode = IPMode.Tcp,
            };
            Func(tcp);
        }

        /// <summary>
        /// UDP测试
        /// </summary>
        static void NewTestUdp()
        {
            var conf = new IPConfig()
            {
                Address = "127.0.0.1",
                Port = 5250,
                Mode = IPMode.Udp,
            };
            Func(conf);
        }

        /// <summary>
        /// 主要测试函数
        /// </summary>
        /// <param name="config"></param>
        static void Func(MConfig config)
        {
            var master = ModBus4.ModBusMasterFactor.CreateMaster(config);
            ushort addr = 6;
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var res = master.ReadHoldingRegisters(1, addr, 1);
                        if (res != null && res.Length >= 1)
                        {
                            Console.WriteLine(string.Format("Read:{0},Value:{1}", addr, res[0]));
                        }
                        var addr2 = (ushort)(addr + 4);
                        var v = new Random().Next(1, 500);
                        master.WriteSingleRegister(1, addr2, (ushort)v);
                        Console.WriteLine(string.Format("Write:{0},Value:{1}", addr2, v));
                    }
                    catch (Exception ex)
                    {
                    }
                    Thread.Sleep(500);
                }

            })
            { IsBackground = true }.Start();
        }

    }
}
