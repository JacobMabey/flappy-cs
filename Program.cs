using System.Numerics;
using Raylib_cs;


Raylib.InitWindow(Global.OG_WIDTH * Global.SCALE, Global.OG_HEIGHT * Global.SCALE, "Flappy");
Global.Initialize();

var renderTexture = Raylib.LoadRenderTexture(Global.OG_WIDTH, Global.OG_HEIGHT);
Raylib.SetTextureFilter(renderTexture.Texture, TextureFilter.Point);

//Define Player Bird
var bird = new Bird(new Vector2(Global.OG_WIDTH / 2 - Global.idleBirdTexture.Width / 2,
    Global.OG_HEIGHT / 2 - Global.idleBirdTexture.Height / 2));



#region UtilFunctions

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

#endregion UtilFunctions

#region DrawFunctions

void DrawGame() {
    Raylib.DrawText((Global.score).ToString(), Global.OG_WIDTH / 2 - 10, 10, 20, Color.White);
    Global.pipes.ForEach(pipe => pipe.Draw());
    Raylib.DrawTexture(Global.groundTexture, 0, 256, Color.White);
    bird.Draw();
}

void DrawTitleScreen() {
    Raylib.DrawTexture(Global.groundTexture, 0, 256, Color.White);
    Raylib.DrawTexture(Global.titleTexture, Global.OG_WIDTH / 2 - Global.titleTexture.Width / 2, 50, Color.White);
    Raylib.DrawTexture(Global.readyTexture, Global.OG_WIDTH / 2 - Global.readyTexture.Width / 2, 75, Color.White);
    Raylib.DrawText("Press SPACE to start", Global.OG_WIDTH / 2 - 60, 110, 10, Color.Black);
    Raylib.DrawText("Current Difficult:" + Global.difficulty.ToString(), Global.OG_WIDTH / 2 - 60, 130, 10, Color.Black);
    Raylib.DrawText("Current Players:" + Global.playerNum, Global.OG_WIDTH / 2 - 60, 150, 10, Color.Black);
    Raylib.DrawText("Press 1 for Easy", Global.OG_WIDTH / 2 - 50, 170, 8, Color.Black);
    Raylib.DrawText("Press 2 for Medium", Global.OG_WIDTH / 2 - 50, 190, 8, Color.Black);
    Raylib.DrawText("Press 3 for Hard", Global.OG_WIDTH / 2 - 50, 210, 8, Color.Black);
    Raylib.DrawText("Press 1-10 for players", Global.OG_WIDTH / 2 - 50, 230, 1, Color.Black);
}

void DrawGameOver() {
    Raylib.DrawTexture(Global.gameOverTexture, Global.OG_WIDTH / 2 - Global.gameOverTexture.Width / 2, 50, Color.White);
    Raylib.DrawTexture(Global.scoreBoardTexture, Global.OG_WIDTH / 2 - Global.scoreBoardTexture.Width / 2, 100, Color.White);
    Raylib.DrawTexture(Global.score < 100 ? Global.silverMedalTexture : Global.goldMedalTexture, 30, 122, Color.White);
    Raylib.DrawText(Global.score.ToString(), 98, 116, 10, Color.White);
}

void DrawBackground() {
    Raylib.DrawTexture(Global.backgroundTexture, 0, 0, Color.White);
}

#endregion DrawFunctions


#region UpdateFunctions

void UpdateGame() {
    Global.pipes.ForEach(pipe => pipe.Update());
    SetPipesPosition();
    bird.Update();
}

void UpdateTitleScreen() {
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        GeneratePipes();
        Global.gameState = GameState.Playing;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.One)) {
        Global.playerNum = 1;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Two))
    {
        Global.playerNum = 2;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Three))
    {
        Global.playerNum = 3;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Four))
    {
        Global.playerNum = 4;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Five))
    {
        Global.playerNum = 5;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Six))
    {
        Global.playerNum = 6;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Seven))
    {
        Global.playerNum = 7;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Eight))
    {
        Global.playerNum = 8;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Nine))
    {
        Global.playerNum = 9;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Zero))
    {
        Global.playerNum = 10;
    }
    else if (Raylib.IsKeyPressed(KeyboardKey.One))
    {
        Global.difficulty = DIFFICULTY.EASY;
    }
    else if (Raylib.IsKeyPressed(KeyboardKey.Two))
    {
        Global.difficulty = DIFFICULTY.MEDIUM;
    }
    else if (Raylib.IsKeyPressed(KeyboardKey.Three))
    {
        Global.difficulty = DIFFICULTY.HARD;
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

#endregion UpdateFunctions


#region GameLoop

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

#endregion GameLoop

Raylib.CloseWindow();



enum GameState {
    TitleScreen,
    Playing,
    GameOver
}

enum DIFFICULTY{ 
    EASY,
    MEDIUM,
    HARD
}