using System;
using System.IO.Ports;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// HID键盘模拟器类(兼容CH340芯片)
/// 通过串口模拟USB HID键盘输入，支持标准键码和CH340协议
/// </summary>
public class HIDKeyboard : IDisposable
{
    private SerialPort? _serialPort;  // 串口通信对象
    private byte _modifierState;      // 当前修饰键状态(Ctrl/Shift/Alt等)
    private byte[] _keyStates = new byte[6]; // 当前按下的普通键状态(最多6键)

    public bool IsOpen => _serialPort?.IsOpen ?? false;



    /// <summary>
    /// 初始化串口
    /// </summary>
    /// <param name="port"></param>
    /// <param name="baudrate"></param>
    public HIDKeyboard(string port = "COM3", int baudrate = 9600)
    {
        // 初始化串口
        try
        {
            _serialPort = new SerialPort(port, baudrate);
            _serialPort.Open();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"无法打开串口 {port}: {ex.Message}");
        }
    }

    /// <summary>
    /// 这个没有参数是怎么运行的
    /// 这个是由ai生成的python再由ai转成csharp
    /// </summary>
    /// <returns></returns>
    private bool SendReport()
    {
        // 检查串口状态
        if (_serialPort == null || !_serialPort.IsOpen)
            return false;

        // CH340CH9329协议格式: 
        // [0x57, 0xAB, 0x00, 0x02, 0x08] + [修饰键, 0] + 6个键码 + 校验和
        byte[] cmdHeader = { 0x57, 0xAB, 0x00, 0x02, 0x08 };
        byte[] data = { _modifierState, 0 };

        // 合并所有数据
        byte[] fullData = new byte[cmdHeader.Length + data.Length + _keyStates.Length + 1];
        Array.Copy(cmdHeader, 0, fullData, 0, cmdHeader.Length);
        Array.Copy(data, 0, fullData, cmdHeader.Length, data.Length);
        Array.Copy(_keyStates, 0, fullData, cmdHeader.Length + data.Length, _keyStates.Length);

        // 计算校验和
        int sum = 0;
        foreach (byte b in cmdHeader) sum += b;
        foreach (byte b in data) sum += b;
        foreach (byte b in _keyStates) sum += b;
        fullData[fullData.Length - 1] = (byte)(sum & 0xFF);

        try
        {
            _serialPort.Write(fullData, 0, fullData.Length);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发送CH340报告失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 基于ai生成的endReport  我自己改改学习与理解以及实现目标
    /// 直接实现发送按键释放报告   
    /// </summary>
    /// <returns></returns>
    private bool SendReporZero()
    {
        // 检查串口状态
        if (_serialPort == null || !_serialPort.IsOpen)
            return false;

        // CH340CH9329协议格式: 
        // [0x57, 0xAB, 0x00, 0x02, 0x08] + [修饰键, 0] + 6个键码 + 校验和
        byte[] cmdHeader = { 0x57, 0xAB, 0x00, 0x02, 0x08 };
        byte[] data = { 0x00, 0 };
        byte[] keys = new byte[6]; // 全零键码

        // 合并所有数据
        byte[] fullData = new byte[cmdHeader.Length + data.Length + keys.Length + 1];
        Array.Copy(cmdHeader, 0, fullData, 0, cmdHeader.Length);
        Array.Copy(data, 0, fullData, cmdHeader.Length, data.Length);
        Array.Copy(keys, 0, fullData, cmdHeader.Length + data.Length, keys.Length);

        // 计算校验和
        int sum = 0;
        foreach (byte b in cmdHeader) sum += b;
        foreach (byte b in data) sum += b;
        foreach (byte b in keys) sum += b;
        fullData[fullData.Length - 1] = (byte)(sum & 0xFF);

        try
        {
            _serialPort.Write(fullData, 0, fullData.Length);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发送CH340报告失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 发送按键按下事件（支持组合键）
    /// </summary>
    /// <param name="charOrKey">需要按下的字符或键名</param>
    /// <remarks>
    /// 工作机制：
    /// 1. 获取字符对应的HID键码（包含Shift修饰状态）
    /// 2. 更新修饰键状态（Shift/Ctrl/Alt等）
    /// 3. 填充按键缓冲区（支持最大6键按下）
    /// 4. 发送HID报告
    /// </remarks>
    public void PressKey(string charOrKey)
    {
        // 处理修饰键（Shift/Ctrl/Alt等）
        if (KeyCodes.ModifierKeys.TryGetValue(charOrKey.ToUpper(), out byte modifier))
        {
            _modifierState |= modifier; // 添加修饰键状态
            SendReport();
            return;
        }

        // 处理单个字符输入
        if (charOrKey.Length == 1)
        {
            char c = charOrKey[0];
            byte hidKey = 0;
            bool shiftNeeded = false;

            // 查找基础键码
            if (KeyCodes.AsciiToHid.TryGetValue(c, out hidKey))
            {
                // 直接使用基础键码
            }
            else if (KeyCodes.ShiftedChars.TryGetValue(c, out char baseChar))
            {
                // 需要Shift修饰的字符
                shiftNeeded = true;
                hidKey = KeyCodes.AsciiToHid[baseChar];
            }

            if (hidKey != 0)
            {
                // 设置Shift状态（如果需要）
                if (shiftNeeded)
                {
                    _modifierState |= KeyCodes.ModifierKeys["LeftShift"];
                }

                // 寻找空位填充键码
                for (int i = 0; i < _keyStates.Length; i++)
                {
                    if (_keyStates[i] == 0)
                    {
                        _keyStates[i] = hidKey;
                        break;
                    }
                }

                SendReport();
                return;
            }
        }

        // 处理功能键（F1-F12等）
        if (KeyCodes.FunctionKeys.TryGetValue(charOrKey, out byte funcKey))
        {
            AddKeyToState(funcKey);
            return;
        }

        // 处理特殊键（Enter/Esc等）
        if (KeyCodes.SpecialKeys.TryGetValue(charOrKey, out byte specialKey))
        {
            AddKeyToState(specialKey);
            return;
        }

        Console.WriteLine($"警告: 未识别的键 '{charOrKey}'");
    }


    /// <summary>
    /// 基于PressKey    
    /// 想实现 ctrl 加c组合键
    /// </remarks>
    public void PressKeyCtelC(string charOrKey="c")
    {
        // 处理修饰键（Shift/Ctrl/Alt等）
        if (KeyCodes.ModifierKeys.TryGetValue(charOrKey.ToUpper(), out byte modifier))
        {
            _modifierState |= modifier; // 添加修饰键状态
            SendReport();
            return;
        }

        // 处理单个字符输入
        if (charOrKey.Length == 1)
        {
            char c = charOrKey[0];
            byte hidKey = 0;


            // 查找基础键码
            if (KeyCodes.AsciiToHid.TryGetValue(c, out hidKey))
            {
                // 直接使用基础键码
            }
            else if (KeyCodes.ShiftedChars.TryGetValue(c, out char baseChar))
            {
 
                hidKey = KeyCodes.AsciiToHid[baseChar];
            }

            if (hidKey != 0)
            {
                // 设置Ctrl状态
                    _modifierState |= KeyCodes.ModifierKeys["LeftCtrl"];
                

                // 寻找空位填充键码
                for (int i = 0; i < _keyStates.Length; i++)
                {
                    if (_keyStates[i] == 0)
                    {
                        _keyStates[i] = hidKey;
                        break;
                    }
                }

                SendReport();
                return;
            }
        }



        Console.WriteLine($"警告: 未识别的键 '{charOrKey}'");
    }





    /// <summary>
    /// 发送按键释放事件（支持组合键）
    /// </summary>
    /// <param name="charOrKey"></param>

    public void ReleaseKey(string charOrKey)
    {
        // 处理修饰键
        if (KeyCodes.ModifierKeys.ContainsKey(charOrKey))
        {
            _modifierState &= (byte)~KeyCodes.ModifierKeys[charOrKey];
            SendReport();
            return;
        }

        // 处理单个字符
        if (charOrKey.Length == 1)
        {
            char c = charOrKey[0];
            byte hidKey = 0;
            bool wasShifted = false;

            if (KeyCodes.AsciiToHid.ContainsKey(c))
            {
                hidKey = KeyCodes.AsciiToHid[c];
            }
            else if (KeyCodes.ShiftedChars.ContainsKey(c))
            {
                char baseChar = KeyCodes.ShiftedChars[c];
                hidKey = KeyCodes.AsciiToHid[baseChar];
                wasShifted = true;
            }

            if (hidKey != 0)
            {
                RemoveKeyFromState(hidKey);

                // 如果释放的是Shift键修饰的字符，也释放Shift
                if (wasShifted)
                {
                    _modifierState &= unchecked((byte)~KeyCodes.ModifierKeys["LeftShift"]);
                }

                SendReport();
                return;
            }
        }

        // 处理功能键和特殊键
        if (KeyCodes.FunctionKeys.ContainsKey(charOrKey))
        {
            byte hidKey = KeyCodes.FunctionKeys[charOrKey];
            RemoveKeyFromState(hidKey);
        }
        else if (KeyCodes.SpecialKeys.ContainsKey(charOrKey))
        {
            byte hidKey = KeyCodes.SpecialKeys[charOrKey];
            RemoveKeyFromState(hidKey);
        }
    }


    /// <summary>
    /// 基于ReleaseKey  释放按键不是度一样吗?
    /// </summary>

    public void ReleaseKeyCtrlC(string charOrKey)
    {
        // 处理修饰键
        if (KeyCodes.ModifierKeys.ContainsKey(charOrKey))
        {
            _modifierState &= (byte)~KeyCodes.ModifierKeys[charOrKey];
            SendReport();
            return;
        }

        // 处理单个字符
        if (charOrKey.Length == 1)
        {
            char c = charOrKey[0];
            byte hidKey = 0;


            if (KeyCodes.AsciiToHid.ContainsKey(c))
            {
                hidKey = KeyCodes.AsciiToHid[c];
            }
            else if (KeyCodes.ShiftedChars.ContainsKey(c))
            {
                char baseChar = KeyCodes.ShiftedChars[c];
                hidKey = KeyCodes.AsciiToHid[baseChar];
            }

            if (hidKey != 0)
            {
                RemoveKeyFromState(hidKey);


               _modifierState &= unchecked((byte)~KeyCodes.ModifierKeys["LeftCtrl"]);
                

                SendReport();
                return;
            }
        }




    }
    /// <summary>单键码处理
    /// 模拟一个按键按下
    /// </summary>
    /// <param name="c"></param>
    public void HidKeyPress(string c)
    {
        PressKey(c);
        ReleaseKey(c);
    }

    /// <summary>
    /// 依据接收的数据发送对应单键码
    /// </summary>
    /// <param name="c"></param>
    ///Console.WriteLine("\n(输入EN 输出回车)(输入ES 输出ESC)(输入BA 输出Backspace):");
    ///Console.WriteLine("\n(输入TA 输出Tab):");
    ///Console.WriteLine("\n(输入W 输出↑)(输入S 输出↓)(输入A 输出←)(输入D 输出→):");
    public void Single_key_code(string c)
    {
        // 统一转换为大写处理
        string command = c.ToUpper().Trim();

        switch (command)
        {
            case "EN":  // 回车键
                HidKeyPress("Enter");
                break;
            case "ES":  // ESC键
                HidKeyPress("Escape");
                break;
            case "BA":  // Backspace
                HidKeyPress("Backspace");
                break;
            case "W":   // 上方向键
                HidKeyPress("UpArrow");
                break;
            case "S":   // 下方向键
                HidKeyPress("DownArrow");
                break;
            case "A":   // 左方向键
                HidKeyPress("LeftArrow");
                break;
            case "D":   // 右方向键
                HidKeyPress("RightArrow");
                break;
            case "TA":   // tab键
                HidKeyPress("Tab");
                break;
            default:
                // 处理未知指令或直接发送字符
                if (command.Length == 1)
                {
                    // 如果是单个字符直接发送
                    HidKeyPress(command);
                }
                else
                {
                    Console.WriteLine($"警告: 无法识别的指令代码 '{c}'");
                    Console.WriteLine("可用指令: EN,ES,DE,W,S,A,D");
                }
                break;
        }
    }








    /// <summary>串键码处理
    /// 字符串单个字符键按下
    /// </summary>
    /// <param name="c"></param>
    public void TypeChar(char c)
    {
        PressKey(c.ToString());
        ReleaseKey(c.ToString());
    }
    /// <summary>
    /// 字符串接收发送单个键
    /// </summary>
    /// <param name="text"></param>

    public void Serial_key_codes(string text)
    {
        foreach (char c in text)
        {
            TypeChar(c);
        }
    }



    /// <summary>
    /// 添加键码到按键缓冲区
    /// </summary>
    /// <param name="key"></param>
    private void AddKeyToState(byte key)
    {
        for (int i = 0; i < _keyStates.Length; i++)
        {
            if (_keyStates[i] == 0)
            {
                _keyStates[i] = key;
                break;
            }
        }
        SendReport();
    }


    /// <summary>
    /// 从按键缓冲区移除键码
    /// </summary>
    /// <param name="key"></param>

    private void RemoveKeyFromState(byte key)
    {
        for (int i = 0; i < _keyStates.Length; i++)
        {
            if (_keyStates[i] == key)
            {
                _keyStates[i] = 0;
                break;
            }
        }
        SendReport();
    }

    /// <summary>
    /// 释放所有按键
    /// </summary>

    public void Dispose()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }
    }


}
