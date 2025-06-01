using System;
using System.Collections.Generic;

/// <summary>
/// 键码映射工具类
/// 提供ASCII到HID键码的映射关系
/// </summary>
public static class KeyCodes
{
    /// <summary>
    /// 修饰键位掩码字典
    /// </summary>
    public static Dictionary<string, byte> ModifierKeys { get; private set; }

    /// <summary>
    /// ASCII到HID键码的基础映射
    /// </summary>
    public static Dictionary<char, byte> AsciiToHid { get; private set; }

    /// <summary>
    /// 需要Shift键的字符映射
    /// </summary>
    public static Dictionary<char, char> ShiftedChars { get; private set; }

    /// <summary>
    /// 功能键(F1-F12)映射
    /// </summary>
    public static Dictionary<string, byte> FunctionKeys { get; private set; }

    /// <summary>
    /// 特殊键映射
    /// </summary>
    public static Dictionary<string, byte> SpecialKeys { get; private set; }

    /// <summary>
    /// 静态构造函数，初始化所有键码映射字典
    /// </summary>
    static KeyCodes()
    {
        try
        {
            // 初始化修饰键位掩码
            ModifierKeys = new Dictionary<string, byte>
            {
                { "LeftCtrl", 0x01 },   // 左Ctrl键
                { "LeftShift", 0x02 },   // 左Shift键
                { "LeftAlt", 0x04 },     // 左Alt键
                { "LeftGui", 0x08 },     // 左Windows键
                { "RightCtrl", 0x10 },   // 右Ctrl键
                { "RightShift", 0x20 },  // 右Shift键
                { "RightAlt", 0x40 },    // 右Alt键
                { "RightGui", 0x80 }     // 右Windows键
            };

            // 初始化ASCII到HID键码的基础映射
            AsciiToHid = new Dictionary<char, byte>
            {
                // 小写字母a-z映射
                { 'a', 0x04 }, { 'b', 0x05 }, { 'c', 0x06 }, { 'd', 0x07 },
                { 'e', 0x08 }, { 'f', 0x09 }, { 'g', 0x0A }, { 'h', 0x0B },
                { 'i', 0x0C }, { 'j', 0x0D }, { 'k', 0x0E }, { 'l', 0x0F },
                { 'm', 0x10 }, { 'n', 0x11 }, { 'o', 0x12 }, { 'p', 0x13 },
                { 'q', 0x14 }, { 'r', 0x15 }, { 's', 0x16 }, { 't', 0x17 },
                { 'u', 0x18 }, { 'v', 0x19 }, { 'w', 0x1A }, { 'x', 0x1B },
                { 'y', 0x1C }, { 'z', 0x1D }, 
                
                // 数字0-9映射
                { '1', 0x1E }, { '2', 0x1F }, { '3', 0x20 }, { '4', 0x21 },
                { '5', 0x22 }, { '6', 0x23 }, { '7', 0x24 }, { '8', 0x25 },
                { '9', 0x26 }, { '0', 0x27 },
                
                // 特殊字符映射
                { ' ', 0x2C },  // 空格键
                { '\n', 0x28 },  // 回车键(换行符)
                { '\r', 0x28 },   // 回车键(回车符)

                //符号映射
                   // ========== 新增符号映射 ==========
                { '-',  0x2D },  // 减号（对应物理键 -_）
                { '=',  0x2E },  // 等号（对应物理键 =+）
                { '[',  0x2F },  // 左方括号（物理键 [{）
                { ']',  0x30 },  // 右方括号（物理键 ]}）
                { '\\', 0x31 },  // 反斜杠（物理键 \|）
                { ';',  0x33 },  // 分号（物理键 ;:）
                { '\'', 0x34 },  // 单引号（物理键 '"）
                { '`',  0x35 },  // 反引号（物理键 `~）
                { ',',  0x36 },  // 逗号（物理键 ,<）
                { '.',  0x37 },  // 句号（物理键 .>）
                { '/',  0x38 }   // 斜杠（物理键 /?）
            };

            // 初始化需要Shift键的字符映射
            ShiftedChars = new Dictionary<char, char>
            {
                // 上档符号映射到对应键位
                { '~', '`' }, { '!', '1' }, { '@', '2' }, { '#', '3' }, { '$', '4' },
                { '%', '5' }, { '^', '6' }, { '&', '7' }, { '*', '8' }, { '(', '9' },
                { ')', '0' }, { '_', '-' }, { '+', '=' }, { '{', '[' }, { '}', ']' },
                { '|', '\\' }, { ':', ';' }, { '"', '\'' }, { '<', ',' }, { '>', '.' },
                { '?', '/' }, 
                
                // 大写字母映射到对应小写字母
                { 'A', 'a' }, { 'B', 'b' }, { 'C', 'c' }, { 'D', 'd' },
                { 'E', 'e' }, { 'F', 'f' }, { 'G', 'g' }, { 'H', 'h' }, { 'I', 'i' },
                { 'J', 'j' }, { 'K', 'k' }, { 'L', 'l' }, { 'M', 'm' }, { 'N', 'n' },
                { 'O', 'o' }, { 'P', 'p' }, { 'Q', 'q' }, { 'R', 'r' }, { 'S', 's' },
                { 'T', 't' }, { 'U', 'u' }, { 'V', 'v' }, { 'W', 'w' }, { 'X', 'x' },
                { 'Y', 'y' }, { 'Z', 'z' }
            };

            // 初始化功能键映射(F1-F12)
            FunctionKeys = new Dictionary<string, byte>
            {
                { "F1", 0x3A }, { "F2", 0x3B }, { "F3", 0x3C }, { "F4", 0x3D },
                { "F5", 0x3E }, { "F6", 0x3F }, { "F7", 0x40 }, { "F8", 0x41 },
                { "F9", 0x42 }, { "F10", 0x43 }, { "F11", 0x44 }, { "F12", 0x45 }
            };

            // 初始化特殊键映射
            SpecialKeys = new Dictionary<string, byte>
            {
                { "Insert", 0x49 },    // 插入键
                { "Home", 0x4A },      // Home键(回到行首/文档开头)
                { "PageUp", 0x4B },    // PageUp键(向上翻页)
                { "Delete", 0x4C },    // Delete键(删除键)
                { "End", 0x4D },       // End键(跳到行尾/文档结尾)
                { "PageDown", 0x4E },  // PageDown键(向下翻页)
                { "RightArrow", 0x4F }, // 右方向键
                { "LeftArrow", 0x50 },  // 左方向键
                { "DownArrow", 0x51 },  // 下方向键
                { "UpArrow", 0x52 },     // 上方向键
                { "Enter", 0x28 },       // 回车键(换行符)
                { "Escape", 0x29 },  // ← 新增的Esc键定义
                { "Backspace", 0x2A },  // HID规范中Backspace键的标准键码
                { "Tab", 0x2B },      // ← 新增 Tab 键映射（HID Usage ID 0x2B）
            };
        }
        catch (Exception ex)
        {
            // 键码映射初始化失败时记录错误并抛出
            Console.Error.WriteLine($"键码映射初始化失败: {ex}");
            throw;
        }
    }
}