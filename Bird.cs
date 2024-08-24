using System.Numerics;
using System.Reflection.PortableExecutable;
using Raylib_cs;

struct Bird {
    private Vector2 position;
    private Vector2 velocity;
    private float gravity = 20.0f;

    private float jumpForce = -10.0f;
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
        velocity = Vector2.Zero;
    }

    private bool IsGrounded() {
        if (position.Y >= 256) return true;
        if(position.Y <= 0) return true;
        return false;
        
    }

    public void Update() {
        if (Raylib.IsKeyPressed(KeyboardKey.Space)) {
            velocity.Y = jumpForce * Global.MULTIPLIER;
            state = BirdState.Flying;
        }

        if (velocity.Y >= 0) {
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
        float velocityX = 2.0f * Global.MULTIPLIER;
        rotation = (float)(Math.Atan2(velocity.Y, velocityX) * 180.0 / Math.PI);
        if (rotation < rotateCounter || (rotation > rotateCounter && rotation > 20.0))
            rotateCounter += (float)((rotateCounter < rotation ? 1 : -1) * Math.Sqrt(Math.Abs(rotation - rotateCounter)) * (rotateCounter < rotation ? 0.005 : 0.02));
        rotateCounter %= 360.0f;
        if (rotation < -30.0 && rotateCounter < -30.0f)
            rotateCounter = -30.0f; 

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