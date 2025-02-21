using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using ReactiveUI;


namespace Dsm.ViewModel;

public   class PlayerViewModel : ReactiveObject
{
    public MusicController Controller { get; } = MusicController.Instance;

    public PlayerViewModel()
    {
        Controller.OnTrackChanged += (_, _) => UpdateCover();
        Controller.OnPlaybackEnded += (_, _) => PlayNext();
        
        PlayPauseCommand = ReactiveCommand.Create(Controller.PlayPause);
        NextCommand = ReactiveCommand.Create(Controller.NextTrack);
        PreviousCommand = ReactiveCommand.Create(Controller.PreviousTrack);
        
        // 启动定时更新
        Observable.Interval(TimeSpan.FromMilliseconds(500))
            .Subscribe(_ => UpdateProgress());
        
    }
    
    private void UpdateProgress()
    {
        this.RaisePropertyChanged(nameof(CurrentTime));
        this.RaisePropertyChanged(nameof(Progress));
    }
    
    public string CurrentTime => Controller.CurrentTime.ToString(@"mm\:ss");
    
    
    public string TotalTime => Controller.TotalTime.ToString(@"mm\:ss");
    public double Progress => Controller.CurrentTime.TotalSeconds / 
                              Controller.TotalTime.TotalSeconds;
    
    private IImage? _coverArt;
    public IImage? CoverArt
    {
        get => _coverArt;
         set => this.RaiseAndSetIfChanged(ref _coverArt, value);
    }
   
    
    public ICommand PlayPauseCommand { get; }
    public ICommand NextCommand { get; }
    public ICommand PreviousCommand { get; }
    private async void UpdateCover()
    {
        var path = Controller.CurrentCover;
        if (path.StartsWith("avares://"))
        {
            using var stream = AssetLoader.Open(new Uri(path));
            CoverArt = new Bitmap(stream);
        }
        else
        {
            CoverArt = await Task.Run(() => new Bitmap(path));
        }
    }

    private void PlayNext() => Dispatcher.UIThread.Post(Controller.NextTrack);
}