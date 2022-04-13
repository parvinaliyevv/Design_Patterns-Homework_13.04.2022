namespace StrategyState;

public abstract class PlayerState
{
    private AudioPlayer _player { get; set; }
    public AudioPlayer Player
    {
        get => _player;
        set => _player = value;
    }


    public abstract void ClickLock();
    public abstract void ClickPlay();
    public abstract void ClickPrev();
    public abstract void ClickNext();
}

public class PlayerReadyState : PlayerState
{
    public override void ClickLock()
        => Player.ChangeState(new PlayerLockedState());

    public override void ClickPlay()
    {
        Player.StartPlayback();
        Player.ChangeState(new PlayerPlayingState());
    }

    public override void ClickNext()
        => Player.NextMusic();

    public override void ClickPrev()
        => Player.PrevMusic();
}

public class PlayerPlayingState : PlayerState
{
    public override void ClickLock()
       => Player.ChangeState(new PlayerLockedState());

    public override void ClickPlay()
    {
        Player.StopPlayback();
        Player.ChangeState(new PlayerReadyState());
    }

    public override void ClickNext()
    {
        if (Player.Event == "DoubleClick") Player.NextMusic();
        else Player.Forward(5);
    }

    public override void ClickPrev()
    {
        if (Player.Event == "DoubleClick") Player.PrevMusic();
        else Player.Rewind(5);
    }
}

public class PlayerLockedState : PlayerState
{
    public override void ClickLock()
       => Player.ChangeState(Player.Playing ? new PlayerPlayingState() : new PlayerReadyState());

    public override void ClickPlay() { }

    public override void ClickNext() { }

    public override void ClickPrev() { }
}

public class Audio { }

public class AudioPlayer
{
    private PlayerState _playerState;
    public PlayerState MyProperty
    {
        get => _playerState;
        set => _playerState = value;
    }

    public string Event { get; set; }
    public bool Playing { get; set; }

    public float Volume { get; set; }
    public float Position { get; set; }

    public Audio CurrentAudio { get; set; }
    public List<Audio> Audios { get; set; }


    public AudioPlayer()
    {
        Audios = new() { new Audio(), new Audio(), new Audio(), new Audio(), new Audio() };

        Event = string.Empty;
        Playing = false;

        Volume = 2.5F;
        Position = 0;

        CurrentAudio = Audios[0];
        _playerState = new PlayerReadyState();
    }


    public void ClickLock()
    {
        _playerState.ClickLock();
    }

    public void ClickPlay()
    {
        _playerState.ClickPlay();
    }

    public void ClickPrev()
    {
        Event = "DoubleClick";
        _playerState.ClickPrev();
    }

    public void ClickNext()
    {
        Event = "Click";
        _playerState.ClickNext();
    }


    public void StopPlayback() => Playing = false;
    public void StartPlayback() => Playing = true;

    public void Rewind(byte second) => Position -= second;
    public void Forward(byte second) => Position += second;

    public void NextMusic()
        => CurrentAudio = Audios[Audios.FindIndex(music => object.ReferenceEquals(music, CurrentAudio)) + 1];

    public void PrevMusic()
        => CurrentAudio = Audios[Audios.FindIndex(music => object.ReferenceEquals(music, CurrentAudio)) - 1];


    public void ChangeState(PlayerState playerState)
    {
        ArgumentNullException.ThrowIfNull(playerState);

        _playerState = playerState;
        _playerState.Player = this;
    }
}
