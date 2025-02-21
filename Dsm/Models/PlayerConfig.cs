using System;
using System.Collections.Generic;
using System.IO;

namespace Dsm.Models;

public class PlayerConfig
{
    // ==================== 音乐文件配置 ====================
    /// <summary>
    /// 音乐库根目录
    /// </summary>
    public string MusicRootPath { get; set; } = "Z:\\Github\\OpenSource\\Project\\DownKyi-1.0.16-1.win-x64\\Media";

    /// <summary>
    /// 支持的音频格式（扩展名需小写）
    /// </summary>
    public HashSet<string> SupportedAudioFormats { get; set; } = new()
    {
        ".mp3","flac", ".wav", ".m4a", ".aac"
    };

    // ==================== 封面匹配配置 ====================
    /// <summary>
    /// 匹配模式规则（优先级从高到低）
    /// <para>可用占位符：</para>
    /// <list type="bullet">
    /// <item>{filename}: 音频文件名</item>
    /// <item>{cover}: 自定义封面名称</item>
    /// </list>
    /// </summary>
    public List<string> MatchPatterns { get; set; } = new() 
    {
        "{filename}",      // 同名文件
        "{cover}",         // 自定义封面名
        "folder"           // 文件夹封面
    };
    
    /// <summary>
    /// 自定义封面名称列表（默认包含cover）
    /// </summary>
    public List<string> CustomCoverNames { get; set; } = new() { "cover" };
    
    /// <summary>
    /// 支持的封面图片格式（按推荐优先级排序）
    /// </summary>
    public HashSet<string> SupportedImageFormats { get; set; } = new()
    {
        ".webp",  // 最优先
        ".png",   // 次优先：无损透明
        ".jpg",   // 基础兼容
        ".jpeg",  // 兼容 jpg 的另一种扩展名
    };

    /// <summary>
    /// 默认封面路径（当无匹配封面时使用）
    /// </summary>
    public string DefaultCoverArt { get; set; } = "avares://Dsm/Assets/44.jpg";

    // ==================== 文件夹筛选配置 ====================
    /// <summary>
    /// 包含文件夹的匹配模式（通配符）
    /// </summary>
    public List<string> IncludeFolders { get; set; } = new();
 

    /// <summary>
    /// 最大扫描目录深度（0=仅根目录）
    /// </summary>
    public sbyte MaxScanDepth { get; set; } = 6;

    // ==================== 播放控制配置 ====================
    /// <summary>
    /// 初始音量（0.0~1.0）
    /// </summary>
    public float InitialVolume { get; set; } = 0.8f;

    /// <summary>
    /// 播放模式
    /// </summary>
    public PlaybackMode PlayMode { get; set; } = PlaybackMode.Sequential;

    /// <summary>
    /// 启动时自动播放
    /// </summary>
    public bool AutoPlayOnStart { get; set; } = true;

    // ==================== 界面显示配置 ====================
    /// <summary>
    /// 窗口置顶
    /// </summary>
    public bool AlwaysOnTop { get; set; } = false;

    public WindowCornerPosition WindowStartupLocation { get; set; } = WindowCornerPosition.TopCenter;

    /// <summary>
    /// 背景透明度（0.0~1.0）
    /// </summary>
    public double WindowOpacity { get; set; } = 1;

    /// <summary>
    /// 主题颜色（HEX格式）
    /// </summary>
    public string ThemeColor { get; set; } = "#4A90E2";

    

    /// <summary>
    /// 自动刷新音乐库间隔（分钟，0=禁用）
    /// </summary>
    public int AutoRefreshInterval { get; set; } = 60;
}

public enum PlaybackMode
{
    Sequential,  // 顺序播放
    Shuffle,     // 随机播放
}

public enum WindowCornerPosition
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
    TopCenter,
    BottomCenter
}