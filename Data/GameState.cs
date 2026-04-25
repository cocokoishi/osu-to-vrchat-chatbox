namespace OsuOscVRC.Data
{
    public enum GameState
    {
        NotRunning = -1,
        Menu = 0,
        Idle = 1,
        Playing = 2,
        SongSelect = 5,
        ResultScreen = 7,
        Paused = 10,
        WatchingReplay = 11,
        ReplayResultScreen = 12,
        Editor = 13,
        Unknown = 99
    }
}
