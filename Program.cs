﻿using System.Collections;
using System.Numerics;
using Raylib_cs;


Raylib.InitWindow(Global.OG_WIDTH * Global.SCALE, Global.OG_HEIGHT * Global.SCALE, "Flappy");
Raylib.InitAudioDevice();
Global.Initialize();

var renderTexture = Raylib.LoadRenderTexture(Global.OG_WIDTH, Global.OG_HEIGHT);
Raylib.SetTextureFilter(renderTexture.Texture, TextureFilter.Point);

#region UtilFunctions

void GeneratePipe() {
    var offset = Raylib.GetRandomValue(20, 60);
    var pipeSpace = Raylib.GetRandomValue(200, 270);
    var upY = Raylib.GetRandomValue(-120, -50);
    var downY = upY + pipeSpace;
    var x = Global.OG_WIDTH + offset;
    Global.pipes.Add(new Pipe(new Vector2(x, upY), Pipe.PipeType.Up));
    Global.pipes.Add(new Pipe(new Vector2(x, downY), Pipe.PipeType.Down));
}

#endregion UtilFunctions

#region DrawFunctions

void DrawGame() {

    Raylib.DrawText(Global.Score.ToString(), Global.OG_WIDTH / 2 - 10, 10, 20, Color.White);
    Global.pipes.ForEach(pipe => pipe.Draw());
    Raylib.DrawTexture(Global.groundTexture, 0, 256, Color.White);
    for (int i = 0; i < Global.birds.Length; i++)
    {
       if (i >= 0)
       {
            Global.birds[i].p1Used = false;
        }
        
            if(i >= 1 )
            {
                Global.birds[i].p1Used = true;
               
            }
        if (i >= 2)
        {
            Global.birds[i].p1Used = true;
            Global.birds[i].p2Used = true;
        }
        if (i >= 3)
        {
            Global.birds[i].p1Used = true;
            Global.birds[i].p2Used = true;
            Global.birds[i].p3Used = true;
        }
        if (i >= 4)
        {
            Global.birds[i].p1Used = true;
            Global.birds[i].p2Used = true;
            Global.birds[i].p3Used = true;
            Global.birds[i].p4Used = true;
        }
        if (i >= 5)
        {
            Global.birds[i].p1Used = true;
            Global.birds[i].p2Used = true;
            Global.birds[i].p3Used = true;
            Global.birds[i].p4Used = true;
            Global.birds[i].p5Used = true;
        }
        if (i >= 6)
        {
            Global.birds[i].p1Used = true;
            Global.birds[i].p2Used = true;
            Global.birds[i].p3Used = true;
            Global.birds[i].p4Used = true;
            Global.birds[i].p5Used = true;
            Global.birds[i].p6Used = true;
        }
        if (i >= 7)
        {
            Global.birds[i].p1Used = true;
            Global.birds[i].p2Used = true;
            Global.birds[i].p3Used = true;
            Global.birds[i].p4Used = true;
            Global.birds[i].p5Used = true;
            Global.birds[i].p6Used = true;
            Global.birds[i].p7Used = true;
        }
        if (i >= 8)
        {
            Global.birds[i].p1Used = true;
            Global.birds[i].p2Used = true;
            Global.birds[i].p3Used = true;
            Global.birds[i].p4Used = true;
            Global.birds[i].p5Used = true;
            Global.birds[i].p6Used = true;
            Global.birds[i].p7Used = true;
            Global.birds[i].p8Used = true;
        }
        if (i >= 9)
        {
            Global.birds[i].p1Used = true;
            Global.birds[i].p2Used = true;
            Global.birds[i].p3Used = true;
            Global.birds[i].p4Used = true;
            Global.birds[i].p5Used = true;
            Global.birds[i].p6Used = true;
            Global.birds[i].p7Used = true;
            Global.birds[i].p8Used = true;
            Global.birds[i].p9Used = true;
        }
        Global.birds[i].Draw();

    }
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
    Raylib.DrawTexture(Global.Score < 100 ? Global.silverMedalTexture : Global.goldMedalTexture, 30, 122, Color.White);
    Raylib.DrawText(Global.Score.ToString(), 98, 116, 10, Color.White);

    if (Global.playerNum > 1)
    {
        int winnerIndex = 1;
        int maxScore = -1;
        foreach (Bird bird in Global.birds)
        {
            if (bird.Score > maxScore)
            {
                maxScore = bird.Score;
                winnerIndex = bird.playerIndex;
            }
        }
        Raylib.DrawText(winnerIndex.ToString(), 98, 140, 10, Color.White);
    }
}

void DrawBackground() {
    Raylib.DrawTexture(Global.backgroundTexture, 0, 0, Color.White);
}

#endregion DrawFunctions


#region UpdateFunctions

void UpdateGame() {
    Global.pipes.ForEach(pipe => pipe.Update());
    Global.pipes.RemoveAll(pipe => pipe.position.X < -20);
    Global.pipeTimer -= Global.deltaTime;
    if (Global.pipeTimer < 0.0f) {
        Global.pipeTimer = Global.pipeTimerStart;
        GeneratePipe();
    }

    bool birdAlive = false;
    for (int i = 0; i < Global.birds.Length; i++)
    {
        Global.birds[i].Update();
        if (!Global.birds[i].IsDead && !Global.birds[i].IsGrounded)
            birdAlive = true;
    }
    if (!birdAlive) {
        Global.gameState = GameState.GameOver;
        Raylib.PlaySound(Global.birdDieSound);
    }
    
}

void UpdateTitleScreen() {
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        Raylib.PlaySound(Global.swooshSound);

        //Intitialize player count
        Global.birds = new Bird[Global.playerNum];
        for (int i = 0; i < Global.playerNum; i++)
            Global.birds[i] = GetNewBird(i + 1);

        switch(Global.difficulty) {
            default:
            case DIFFICULTY.EASY: Global.pipeTimerStart = 2.0f;
                break;
            case DIFFICULTY.MEDIUM: Global.pipeTimerStart = 1.5f;
                break;
            case DIFFICULTY.HARD: Global.pipeTimerStart = 1.25f;
                break;
        }
        
        Global.gameState = GameState.Playing;
    }
    else if (Raylib.IsKeyPressed(KeyboardKey.Up)) {
        Global.playerNum++;
        if (Global.playerNum > 10)
            Global.playerNum = 10;
    }
    else if (Raylib.IsKeyPressed(KeyboardKey.Down)) {
        Global.playerNum--;
        if (Global.playerNum < 1)
            Global.playerNum = 1;
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
        Raylib.PlaySound(Global.swooshSound);
        
        Global.gameState = GameState.TitleScreen;
        Global.birds = [];
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

Raylib.CloseAudioDevice();
Raylib.CloseWindow();

Bird GetNewBird(int playerIndex) {
    //Define Player Bird
    return new Bird(new Vector2(Global.OG_WIDTH / 2 - Global.idleBirdTexture.Width / 2 + playerIndex * 3,
        Global.OG_HEIGHT / 2 - Global.idleBirdTexture.Height / 2), playerIndex);
}

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