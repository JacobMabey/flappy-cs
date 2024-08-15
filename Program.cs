using System.Numerics;
using System.Reflection;
using Raylib_cs;

Raylib.InitWindow(Global.OG_WIDTH * Global.SCALE, Global.OG_HEIGHT * Global.SCALE, "Flappy");
Global.Initialize();

var renderTexture = Raylib.LoadRenderTexture(Global.OG_WIDTH, Global.OG_HEIGHT);
Raylib.SetTextureFilter(renderTexture.Texture, TextureFilter.Point);

var bird = new Bird(new Vector2(Global.OG_WIDTH / 2 - Global.idleBirdTexture.Width / 2,
    Global.OG_HEIGHT / 2 - Global.idleBirdTexture.Height / 2));

void GeneratePipes() {
    for (var i = 0; i < 5; i++) {
        var offset = Raylib.GetRandomValue(150, 316);
        var downY = Raylib.GetRandomValue(180, 200);
        var upY = Raylib.GetRandomValue(-100, 0);
        var x = Global.OG_WIDTH + (i * 100) + offset;
        Global.pipes.Add(new Pipe(new Vector2(x, upY), Pipe.PipeType.Up));
        Global.pipes.Add(new Pipe(new Vector2(x, downY), Pipe.PipeType.Down));
    }
}

void SetPipesPosition() {
    var offPipes = Global.pipes.Where(pipe => pipe.position.X < 0).ToList();
    for (var i = 0; i < offPipes.Count; i += 2) {
        var offset = Raylib.GetRandomValue(150, 316);
        var downY = Raylib.GetRandomValue(180, 200);
        var upY = Raylib.GetRandomValue(-100, 0);
        var x = Global.OG_WIDTH + (i * 100) + offset; 
        offPipes[i].position = new Vector2(x, upY);
        offPipes[i + 1].position = new Vector2(x, downY);
    }
}

void DrawBackground() {
    Raylib.DrawTexture(Global.backgroundTexture, 0, 0, Color.White);
    Raylib.DrawTexture(Global.groundTexture, 0, 256, Color.White);
}

void DrawTitleScreen() {
    Raylib.DrawTexture(Global.titleTexture, Global.OG_WIDTH / 2 - Global.titleTexture.Width / 2, 50, Color.White);
    Raylib.DrawTexture(Global.readyTexture, Global.OG_WIDTH / 2 - Global.readyTexture.Width / 2, 75, Color.White);
    Raylib.DrawText("Press SPACE to start", Global.OG_WIDTH / 2 - 50, 110, 10, Color.White);
}

void UpdateGame() {
    Global.pipes.ForEach(pipe => pipe.Update());
    SetPipesPosition();
    bird.Update();
}

void DrawGame() {
    Global.pipes.ForEach(pipe => pipe.Draw());
    bird.Draw();
    Raylib.DrawText(Global.score.ToString(), Global.OG_WIDTH / 2 - 10, 10, 20, Color.White);
}

void DrawGameOver() {
    Raylib.DrawTexture(Global.gameOverTexture, Global.OG_WIDTH / 2 - Global.gameOverTexture.Width / 2, 50, Color.White);
    Raylib.DrawTexture(Global.scoreBoardTexture, Global.OG_WIDTH / 2 - Global.scoreBoardTexture.Width / 2, 100, Color.White);
    Raylib.DrawTexture(Global.score < 100 ? Global.silverMedalTexture : Global.goldMedalTexture, 30, 122, Color.White);
    Raylib.DrawText(Global.score.ToString(), 98, 116, 10, Color.White);
}

void UpdateTitleScreen() {
    if (Raylib.IsKeyPressed(KeyboardKey.Space)) {
        GeneratePipes();
        Global.gameState = GameState.Playing;
    }
}

void UpdateGameOver() {
    if (Raylib.IsKeyPressed(KeyboardKey.Space)) {
        Global.gameState = GameState.TitleScreen;
        Global.score = 0;
        bird = new Bird(new Vector2(Global.OG_WIDTH / 2 - Global.idleBirdTexture.Width / 2,
            Global.OG_HEIGHT / 2 - Global.idleBirdTexture.Height / 2));
        Global.pipes.Clear();
    }
}

while (!Raylib.WindowShouldClose()) {
    Global.deltaTime = Raylib.GetFrameTime();
    switch (Global.gameState) {
        case GameState.TitleScreen:
            UpdateTitleScreen();
            break;
        case GameState.Playing:
            UpdateGame();
            break;
        case GameState.GameOver:
            UpdateGameOver();
            break;
    }

    if (Raylib.IsKeyPressed(KeyboardKey.F1)) {
        Global.drawAABB = !Global.drawAABB;
    }
    Raylib.BeginDrawing();
    Raylib.BeginTextureMode(renderTexture);
    DrawBackground();
    switch (Global.gameState) {
        case GameState.TitleScreen:
            DrawTitleScreen();
            break;
        case GameState.Playing:
            DrawGame();
            break;
        case GameState.GameOver:
            DrawGameOver();
            break;
    }

    Raylib.EndTextureMode();

    Raylib.DrawTexturePro(renderTexture.Texture, new Rectangle(0, 0, Global.OG_WIDTH, -Global.OG_HEIGHT),
        new Rectangle(0, 0, Global.OG_WIDTH * Global.SCALE, Global.OG_HEIGHT * Global.SCALE), new Vector2(0, 0), 0,
        Color.White);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();

enum GameState {
    TitleScreen,
    Playing,
    GameOver
}

struct Bird {
    private Vector2 position;
    private Vector2 velocity;
    private float gravity = 10.0f;

    private float jumpForce = -5.0f;
    private float scoreTime = 0.0f;
    private float scoreRate = 0.75f;
    private float rotation = 0.0f;
    private float rotateCounter = 0.0f;

    private enum BirdState {
        Idle,
        Flying,
        Falling
    }

    private BirdState state;

    public Bird(Vector2 position) {
        this.position = position;
        state = BirdState.Idle;
        velocity = new Vector2(0, 0);
    }

    private bool IsGrounded() {
        return position.Y >= 256;
    }

    public void Update() {
        if (Raylib.IsKeyPressed(KeyboardKey.Space)) {
            velocity.Y = jumpForce * Global.MULTIPLIER;
            state = BirdState.Flying;
        }

        if (velocity.Y > 0) {
            state = BirdState.Falling;
        }

        if (velocity == Vector2.Zero) {
            state = BirdState.Idle;
        }

        if (IsGrounded()) {
            velocity.Y = 0;
            Global.gameState = GameState.GameOver;
        }
        
        CheckCollisionWithPipes();
        CheckIfBetweenPipes();

        velocity.Y += gravity * Global.MULTIPLIER * Global.deltaTime;
        position += velocity * Global.deltaTime;
    }

    private void CheckCollisionWithPipes() {
        foreach (var pipe in Global.pipes) {
            if (Raylib.CheckCollisionRecs(
                    new Rectangle(position.X, position.Y, Global.idleBirdTexture.Width, Global.idleBirdTexture.Height),
                    new Rectangle(pipe.position.X, pipe.position.Y, Global.pipeUpTexture.Width,
                        Global.pipeUpTexture.Height)
                )) {
                Global.gameState = GameState.GameOver;
            }
        }
    }

    private void CheckIfBetweenPipes() {
        foreach (var pipe in Global.pipes) {
            if (position.X > pipe.position.X && position.X < pipe.position.X + Global.pipeUpTexture.Width) {
                if (scoreTime > scoreRate) {
                    Global.score++;
                    scoreTime = 0;
                }
                scoreTime += Global.deltaTime;
            }
        }
    }

    public void Draw() {
        float velocityX = 5.0f * Global.MULTIPLIER;
        rotation = (float)(Math.Atan2(velocity.Y, velocityX) * 180.0 / Math.PI);
        rotateCounter += (float)((rotateCounter < rotation ? 1 : (rotateCounter > rotation ? -1 : 0)) * Math.Sqrt(Math.Abs(rotation - rotateCounter)) * 0.1);
        rotateCounter %= 360.0f;

        Texture2D drawTexture = Global.idleBirdTexture;
        switch (state) {
            case BirdState.Idle:
                drawTexture = Global.idleBirdTexture;
                break;
            case BirdState.Flying:
                drawTexture = Global.flyBirdTexture;
                break;
            case BirdState.Falling:
                drawTexture = Global.fallBirdTexture;
                break;
        }
        Raylib.DrawTexturePro(drawTexture, new Rectangle(0.0f, 0.0f, drawTexture.Width, drawTexture.Height), new Rectangle(position.X, position.Y, drawTexture.Width, drawTexture.Height), new Vector2(drawTexture.Width / 2.0f, drawTexture.Height / 2.0f), rotateCounter, Color.White);
        DrawAABB();
    }
    
    private void DrawAABB() {
        if (!Global.drawAABB) return;
        Raylib.DrawRectangleLines((int)position.X, (int)position.Y, Global.idleBirdTexture.Width, Global.idleBirdTexture.Height, Color.Red);
    }
}

class Pipe {
    public Vector2 position;
    private float speed = 5.0f;

    public enum PipeType {
        Up,
        Down
    }

    private PipeType type;

    public Pipe(Vector2 position, PipeType type) {
        this.position = position;
        this.type = type;
    }

    public void Update() {
        if (position.X < 0) {
            position.X = Global.OG_WIDTH + Global.pipeUpTexture.Width;
        }

        position.X -= speed * Global.deltaTime * Global.MULTIPLIER;
    }

    public void Draw() {
        switch (type) {
            case PipeType.Up:
                Raylib.DrawTexture(Global.pipeUpTexture, (int)position.X, (int)position.Y, Color.White);
                break;
            case PipeType.Down:
                Raylib.DrawTexture(Global.pipeDownTexture, (int)position.X, (int)position.Y, Color.White);
                break;
        }
        DrawAABB();
    }
    
    private void DrawAABB() {
        if (!Global.drawAABB) return;
        Raylib.DrawRectangleLines((int)position.X, (int)position.Y, Global.pipeUpTexture.Width, Global.pipeUpTexture.Height, Color.Red);
    }
}

class Global {
    public const int OG_WIDTH = 144;
    public const int OG_HEIGHT = 312;
    public const int SCALE = 2;
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
    public static float MULTIPLIER = 10.0f;
    public static float GRAVITY = 10.0f;
    public static List<Pipe> pipes = new List<Pipe>();
    public static GameState gameState = GameState.TitleScreen;
    public static int score = 0;
    public static float deltaTime = 0.0f;
    public static bool drawAABB = false;

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
    }
}