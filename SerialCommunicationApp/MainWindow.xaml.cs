using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SerialCommunicationApp
{
    public partial class MainWindow : Window
    {
        private HIDKeyboard? hidKeyboard;
        private bool isConnected = false;


        public MainWindow()
        {
            InitializeComponent();
            RefreshPortList();
        }


        /// <summary>
        /// 实现cmbPorts下拉窗口刷新
        /// </summary>
        /// <param name="param1">[参数1描述]</param>
        /// <param name="param2">[参数2描述]</param>
        /// <returns>[返回值描述]</returns>
        /// <exception cref="ExceptionType">[触发异常的条件说明]</exception>
        /// <example>
        /// [示例代码（可选）]
        /// <code>
        /// var result = MethodName(5, "test");
        /// </code>
        /// </example>
        /// <remarks>
        /// [补充说明或注意事项（可选）]
        /// </remarks>

        private void RefreshPortList()
        {
            if (cmbPorts == null) return;

            cmbPorts.Items.Clear();

            try
            {
                string[] ports = System.IO.Ports.SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    cmbPorts.Items.Add(port);
                }

                if (ports.Length > 0)
                {
                    cmbPorts.SelectedIndex = 0;
                }
            }
            catch
            {
                MessageBox.Show("获取串口列表失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshPortList();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtInput?.Text))
            {
                MessageBox.Show("请输入要发送的内容", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

  
            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }



            // 发送字符串
            hidKeyboard.Serial_key_codes(txtInput.Text);


        }
        

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {

            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///

            hidKeyboard?.Single_key_code("EN");


        }//BtnEnter_Click

        private void btnEsc_Click(object sender, RoutedEventArgs e)
        {
            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///

            hidKeyboard?.Single_key_code("ES");

        }

        private void btnTab_Click(object sender, RoutedEventArgs e)
        {
            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///

            hidKeyboard?.Single_key_code("TA");


        }

        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///

            hidKeyboard?.Single_key_code("BA");

        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///

            hidKeyboard?.Single_key_code("W");

        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///

            hidKeyboard?.Single_key_code("S");

        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///

            hidKeyboard?.Single_key_code("A");

        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///

            hidKeyboard?.Single_key_code("D");

        }

        private void btnClearC_Click(object sender, RoutedEventArgs e)
        {
            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///
            hidKeyboard?.PressKeyCtelC("c");
            hidKeyboard?.ReleaseKeyCtrlC("c");


        }

        private void btnClearO_Click(object sender, RoutedEventArgs e)
        {
            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///
            hidKeyboard?.PressKeyCtelC("o");
            hidKeyboard?.ReleaseKeyCtrlC("o");

        }

        private void btnClearX_Click(object sender, RoutedEventArgs e)
        {
            ///这什么作用啊？
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }
            ///
            hidKeyboard?.PressKeyCtelC("x");
            hidKeyboard?.ReleaseKeyCtrlC("x");

        }

        private void btnSendEnrty_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPorts?.SelectedItem == null)
            {
                MessageBox.Show("请先选择串口", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtInput?.Text))
            {
                MessageBox.Show("请输入要发送的内容", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string portName = cmbPorts.SelectedItem.ToString()!;
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("无效的串口名称", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 初始化或重用HIDKeyboard实例
            if (hidKeyboard == null)
            {
                hidKeyboard = new HIDKeyboard(portName);
            }



            // 发送字符串
            hidKeyboard.Serial_key_codes(txtInput.Text);

            //PressKey('\n'.ToString());
            //ReleaseKey('\n'.ToString());
            hidKeyboard?.PressKey('\n'.ToString());
            hidKeyboard?.ReleaseKey('\n'.ToString());


        }

        private void btenkj1_Click(object sender, RoutedEventArgs e)
        {

        }



        private void btenkj2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btenkj3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void kjjsehezhi_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}