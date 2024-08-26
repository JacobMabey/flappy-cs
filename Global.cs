using System.Reflection;
using Raylib_cs;

class Global {
    public static Bird[] birds = [];

    public const int OG_WIDTH = 144;
    public const int OG_HEIGHT = 312;
    public const int SCALE = 3;
    public static Texture2D backgroundTexture;
    public static Texture2D groundTexture;
    public static Texture2D titleTexture;
    public static Texture2D readyTexture;
    public static Texture2D idleBirdTexture;
    public static Texture2D flyBirdTexture;
    public static Texture2D fallBirdTexture;
    public static Texture2D pipeUpTexture;
    public static Texture2D pipeDownTexture;
    public static Texture2D gameOverTexture;
    public static Texture2D silverMedalTexture;
    public static Texture2D goldMedalTexture;
    public static Texture2D scoreBoardTexture;
    public static Texture2D animatedBackgorund;
    public static Sound birdDieSound;
    public static Sound birdHitSound;
    public static Sound pointSound;
    public static Sound swooshSound;
    public static Sound wingSound;
    public static float MULTIPLIER = 10.0f;
    public static float GRAVITY { get => 10.0f + DifficultyMultiplier; }
    public static List<Pipe> pipes = new List<Pipe>();
    public static float pipeTimerStart = 2.0f;
    public static float pipeTimer = pipeTimerStart;
    public static GameState gameState = GameState.TitleScreen;
    public static int Score {
        get {
            return birds.Select(b => b.Score).Max();
        }
    }
    public static float deltaTime = 0.0f;
    public static bool drawAABB = false;
    public static Enum difficulty = DIFFICULTY.EASY;
    public static int playerNum = 1;

    public static float DifficultyMultiplier
    {
        get {
            switch (difficulty)
            {
                default:
                case DIFFICULTY.EASY: return 1.0f;
                case DIFFICULTY.MEDIUM: return 2.0f;
                case DIFFICULTY.HARD: return 3.0f;
            }
        }
    }

    private static Texture2D LoadTexture(string path) {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(path);
        if (stream == null) return new Texture2D();
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        var data = ms.ToArray();
        if (data.Length == 0) return new Texture2D();
        return Raylib.LoadTextureFromImage(Raylib.LoadImageFromMemory(".png", data));
    }
    
    private static Sound LoadSound(string path) {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(path);
        if (stream == null) return new Sound();
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        var data = ms.ToArray();
        if (data.Length == 0) return new Sound();
        return Raylib.LoadSoundFromWave(Raylib.LoadWaveFromMemory(".wav", data));
    }

    public static void Initialize() {
        backgroundTexture = LoadTexture("Flappy.assets.bg.png");
        groundTexture = LoadTexture("Flappy.assets.ground.png");
        titleTexture = LoadTexture("Flappy.assets.title.png");
        readyTexture = LoadTexture("Flappy.assets.ready.png");
        idleBirdTexture = LoadTexture("Flappy.assets.idle.png");
        flyBirdTexture = LoadTexture("Flappy.assets.jump.png");
        fallBirdTexture = LoadTexture("Flappy.assets.fall.png");
        pipeUpTexture = LoadTexture("Flappy.assets.pipe_up.png");
        pipeDownTexture = LoadTexture("Flappy.assets.pipe_down.png");
        gameOverTexture = LoadTexture("Flappy.assets.game_over.png");
        silverMedalTexture = LoadTexture("Flappy.assets.silver_medal.png");
        goldMedalTexture = LoadTexture("Flappy.assets.gold_medal.png");
        scoreBoardTexture = LoadTexture("Flappy.assets.scoreboard_dead.png");
        animatedBackgorund = LoadTexture("Flappy.assets.background.gif");

        birdDieSound = LoadSound("Flappy.assets.sfx_die.wav");
        birdHitSound = LoadSound("Flappy.assets.sfx_hit.wav");
        pointSound = LoadSound("Flappy.assets.sfx_point.wav");
        swooshSound = LoadSound("Flappy.assets.sfx_swooshing.wav");
        wingSound = LoadSound("Flappy.assets.sfx_wing.wav");
    }
}