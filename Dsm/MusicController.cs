using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dsm.Loaders;
using Dsm.Models;
using NAudio.Wave; // 需要安装NAudio包

public sealed class MusicController : IDisposable
{
    #region 单例模式
    public static readonly MusicController Instance = new();
    private MusicController() => Initialize();
    #endregion

    #region 核心字段
    private readonly PlayerConfig _config = new();
    private readonly MusicLoader _musicLoader = new(new PlayerConfig());
    private readonly CoverArtLoader _coverLoader = new(new PlayerConfig());
    private IWavePlayer? _waveOut;
    private AudioFileReader? _audioStream;
    private List<string> _playlist = new();
    private int _currentTrackIndex;
    private bool _isDisposed;
    #endregion

    #region 播放状态属性
    public string CurrentSong => _playlist.ElementAtOrDefault(_currentTrackIndex) ?? string.Empty;
    public string CurrentCover => _coverLoader.FindCoverArt(CurrentSong);
    public TimeSpan CurrentTime => _audioStream?.CurrentTime ?? TimeSpan.Zero;
    public TimeSpan TotalTime => _audioStream?.TotalTime ?? TimeSpan.Zero;
    public float Volume
    {
        get => _waveOut?.Volume ?? _config.InitialVolume;
        set => _waveOut.Volume = Math.Clamp(value, 0, 1);
    }
    public bool IsPlaying => _waveOut?.PlaybackState == PlaybackState.Playing;
    #endregion

    #region 初始化
    private void Initialize()
    {
        ReloadPlaylist();
        _waveOut = new WaveOutEvent {
            Volume = _config.InitialVolume
        };
    }
    #endregion

    #region 播放控制
    public void PlayPause()
    {
        if (_waveOut == null || _playlist.Count == 0) return;

        if (IsPlaying)
        {
            _waveOut.Pause();
        }
        else
        {
            if (_audioStream == null) LoadTrack(_currentTrackIndex);
            _waveOut.Play();
        }
    }

    public void NextTrack() => ChangeTrack(1);
    public void PreviousTrack() => ChangeTrack(-1);

    private void ChangeTrack(int offset)
    {
        _currentTrackIndex = (_currentTrackIndex + offset + _playlist.Count) % _playlist.Count;
        LoadTrack(_currentTrackIndex);
        if (IsPlaying) _waveOut?.Play();
    }
    #endregion

    #region 播放列表管理
    public void ReloadPlaylist()
    {
        _playlist = _musicLoader.ScanMusicFiles()
            .OrderBy(Path.GetFileName)
            .ToList();
        
        if (_config.PlayMode == PlaybackMode.Shuffle)
        {
            ShufflePlaylist();
        }
    }

    private void ShufflePlaylist()
    {
        var rng = new Random();
        _playlist = _playlist.OrderBy(_ => rng.Next()).ToList();
    }
    #endregion

    #region 音频加载
    private void LoadTrack(int index)
    {
        _audioStream?.Dispose();
        
        try
        {
            _audioStream = new AudioFileReader(_playlist[index]);
            _waveOut?.Init(_audioStream);
            OnTrackChanged?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            OnPlaybackError?.Invoke(this, ex.Message);
            NextTrack(); // 自动跳过损坏文件
        }
    }
    #endregion

    #region 事件定义
    public event EventHandler? OnTrackChanged;
    public event EventHandler<string>? OnPlaybackError;
    public event EventHandler? OnPlaybackEnded;
    #endregion

    #region 资源释放
    public void Dispose()
    {
        if (_isDisposed) return;
        
        _waveOut?.Dispose();
        _audioStream?.Dispose();
        _isDisposed = true;
    }
    #endregion
}