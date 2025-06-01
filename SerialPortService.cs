using System;
using System.IO.Ports;
using System.Collections.Generic;

namespace SerialCommunicationApp
{
    /// <summary>
    /// 串口服务类，负责管理串口检测和刷新
    /// </summary>
    public class SerialPortService
    {
        /// <summary>
        /// 可用串口列表
        /// </summary>
        public List<string> AvailablePorts { get; private set; } = new List<string>();

        /// <summary>
        /// 当前选择的串口
        /// </summary>
        public string SelectedPort { get; set; }

        /// <summary>
        /// 串口列表更新事件
        /// </summary>
        public event EventHandler PortsUpdated;

        /// <summary>
        /// 获取当前系统所有可用串口
        /// </summary>
        /// <returns>串口名称列表</returns>
        public List<string> GetPorts()
        {
            RefreshPorts();
            return new List<string>(AvailablePorts);
        }

        /// <summary>
        /// 刷新串口列表
        /// </summary>
        /// <returns>是否刷新成功</returns>
        public bool RefreshPorts()
        {
            try
            {
                AvailablePorts.Clear();
                string[] ports = SerialPort.GetPortNames();
                AvailablePorts.AddRange(ports);

                // 如果之前选择的端口仍然存在，保持选择
                if (!string.IsNullOrEmpty(SelectedPort) && 
                    AvailablePorts.Contains(SelectedPort))
                {
                    // 保持原选择
                }
                else if (AvailablePorts.Count > 0)
                {
                    SelectedPort = AvailablePorts[0];
                }
                else
                {
                    SelectedPort = null;
                }

                PortsUpdated?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception)
            {
                AvailablePorts.Clear();
                SelectedPort = null;
                return false;
            }
        }
    }
}