using System.Numerics;
using System.Reflection.PortableExecutable;
using Raylib_cs;

struct Bird {
    public KeyboardKey[] jumpKeys = [KeyboardKey.One, KeyboardKey.Two, KeyboardKey.Three, KeyboardKey.Four, KeyboardKey.Five, KeyboardKey.Six, KeyboardKey.Seven, KeyboardKey.Eight, KeyboardKey.Nine, KeyboardKey.Zero];

    public int playerIndex { get; private set; } = -1;
    public int Score { get; private set; } = 0;
    private Vector2 position;
    private Vector2 velocity;
    private float gravity = 13.0f;

    private float jumpForce = -3.0f;
    private float speed = 5;
    private float scoreTime = 0.0f;
    private float scoreRate = 0.75f;
    private float rotation = 0.0f;
    private float rotateCounter = 0.0f;

    public bool p1Used = false;
    public bool p2Used = false;
    public bool p3Used = false;
    public bool p4Used = false;
    public bool p5Used = false;
   public bool p6Used = false;
    public bool p7Used = false;
    public bool p8Used = false;
    public bool p9Used = false;
    public bool p10Used = false;

    private KeyboardKey JumpControl
    {
        get
        {
            if (playerIndex >= 1 && playerIndex <= 10)
                return jumpKeys[playerIndex - 1];
            else
                return KeyboardKey.Space;
        }
    }

    public bool IsGrounded {
        get {
            if (position.Y >= 256) return true;
            if(position.Y <= 0) return true;
            return false;
        }
    }

    public bool IsDead { get; private set; } = false;

    private enum BirdState {
        Idle,
        Flying,
        Falling
    }

    private BirdState state;

    public Bird(Vector2 position, int player) {
        this.position = position;
        state = BirdState.Idle;
        velocity = Vector2.Zero;
        this.playerIndex = player;
    }

    public void Update() {
        if (!IsDead)
        {
            if (Raylib.IsKeyPressed(JumpControl) || (playerIndex == 1 && Raylib.IsKeyPressed(KeyboardKey.Space))) {
                Raylib.PlaySound(Global.wingSound);

                velocity.Y = jumpForce * Global.MULTIPLIER;
                state = BirdState.Flying;
            }

            if (velocity.Y >= 0) {
                state = BirdState.Falling;
            }

            if (velocity == Vector2.Zero) {
                state = BirdState.Idle;
            }
            
            CheckCollisionWithPipes();
            CheckIfBetweenPipes();
        }

        if (IsGrounded) {
            IsDead = true;
            velocity.Y = 0;
        }
        
        velocity.Y += gravity * Global.MULTIPLIER * Global.deltaTime;
        position += velocity * speed * Global.deltaTime;
    }

    public void CheckCollisionWithPipes() {
        foreach (var pipe in Global.pipes) {
            if (Raylib.CheckCollisionRecs(
                    new Rectangle(position.X - Global.idleBirdTexture.Width / 2.0f, position.Y - Global.idleBirdTexture.Height / 2.0f, Global.idleBirdTexture.Width, Global.idleBirdTexture.Height),
                    new Rectangle(pipe.position.X, pipe.position.Y, Global.pipeUpTexture.Width,
                        Global.pipeUpTexture.Height)
                )) {
                Raylib.PlaySound(Global.birdHitSound);
                IsDead = true;
            }
        }
    }

    private void CheckIfBetweenPipes() {
        foreach (var pipe in Global.pipes) {
            if (position.X > pipe.position.X && position.X < pipe.position.X + Global.pipeUpTexture.Width) {
                if (scoreTime > scoreRate) {
                    if (!Raylib.IsSoundPlaying(Global.pointSound))
                        Raylib.PlaySound(Global.pointSound);
                    Score++;
                    scoreTime = 0;
                }
                scoreTime += Global.deltaTime;
            }
        }
    }

    public void Draw() {
        Color color = Color.White;
        if (IsDead) {
            rotateCounter = 180.0f;
            color = Color.Gray;
        }
        else {
            float velocityX = 2.0f * Global.MULTIPLIER;
            rotation = (float)(Math.Atan2(velocity.Y, velocityX) * 180.0 / Math.PI);
            if (rotation < rotateCounter || (rotation > rotateCounter && rotation > 20.0))
                rotateCounter += (float)((rotateCounter < rotation ? 1 : -1) * Math.Sqrt(Math.Abs(rotation - rotateCounter)) * (rotateCounter < rotation ? 0.005 : 0.02));
            rotateCounter %= 360.0f;
            if (rotation < -30.0 && rotateCounter < -30.0f)
                rotateCounter = -30.0f;
        }

        Texture2D drawTexture;
        if(!p1Used)
        {

            drawTexture = Global.idleBirdTexture;
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
        }
        else if(p1Used && !p2Used) 
        
        {
            drawTexture = Global.idleBirdTexture2;
            switch (state)
            {
                case BirdState.Idle:
                    drawTexture = Global.idleBirdTexture2;
                    break;
                case BirdState.Flying:
                    drawTexture = Global.flyBirdTexture2;
                    break;
                case BirdState.Falling:
                    drawTexture = Global.fallBirdTexture2;
                    break;

            }
          
        }
        else if (p1Used && p2Used && !p3Used)

        {
            drawTexture = Global.idleBirdTexture3;
            switch (state)
            {
                case BirdState.Idle:
                    drawTexture = Global.idleBirdTexture3;
                    break;
                case BirdState.Flying:
                    drawTexture = Global.flyBirdTexture3;
                    break;
                case BirdState.Falling:
                    drawTexture = Global.fallBirdTexture3;
                    break;

            }
          
        }
        else if (p1Used && p2Used && p3Used && !p4Used)

        {
            drawTexture = Global.idleBirdTexture4;
            switch (state)
            {
                case BirdState.Idle:
                    drawTexture = Global.idleBirdTexture4;
                    break;
                case BirdState.Flying:
                    drawTexture = Global.flyBirdTexture4;
                    break;
                case BirdState.Falling:
                    drawTexture = Global.fallBirdTexture4;
                    break;

            }
         
        }
        else if (p1Used && p2Used && p3Used && p4Used && !p5Used)

        {
            drawTexture = Global.idleBirdTexture5;
            switch (state)
            {
                case BirdState.Idle:
                    drawTexture = Global.idleBirdTexture5;
                    break;
                case BirdState.Flying:
                    drawTexture = Global.flyBirdTexture5;
                    break;
                case BirdState.Falling:
                    drawTexture = Global.fallBirdTexture5;
                    break;

            }
          
        }
        else if (p1Used && p2Used && p3Used && p4Used && p5Used && !p6Used)

        {
            drawTexture = Global.idleBirdTexture6;
            switch (state)
            {
                case BirdState.Idle:
                    drawTexture = Global.idleBirdTexture6;
                    break;
                case BirdState.Flying:
                    drawTexture = Global.flyBirdTexture6;
                    break;
                case BirdState.Falling:
                    drawTexture = Global.fallBirdTexture6;
                    break;

            }
          
        }
        else if (p1Used && p2Used && p3Used && p4Used && p5Used && p6Used && !p7Used)

        {
            drawTexture = Global.idleBirdTexture7;
            switch (state)
            {
                case BirdState.Idle:
                    drawTexture = Global.idleBirdTexture7;
                    break;
                case BirdState.Flying:
                    drawTexture = Global.flyBirdTexture7;
                    break;
                case BirdState.Falling:
                    drawTexture = Global.fallBirdTexture7;
                    break;

            }
         
        }
        else if (p1Used && p2Used && p3Used && p4Used && p5Used && p6Used && p7Used && !p8Used)

        {
            drawTexture = Global.idleBirdTexture8;
            switch (state)
            {
                case BirdState.Idle:
                    drawTexture = Global.idleBirdTexture8;
                    break;
                case BirdState.Flying:
                    drawTexture = Global.flyBirdTexture8;
                    break;
                case BirdState.Falling:
                    drawTexture = Global.fallBirdTexture8;
                    break;

            }

        }
        else if (p1Used && p2Used && p3Used && p4Used && p5Used && p6Used && p7Used && p8Used && !p9Used)

        {
            drawTexture = Global.idleBirdTexture9;
            switch (state)
            {
                case BirdState.Idle:
                    drawTexture = Global.idleBirdTexture9;
                    break;
                case BirdState.Flying:
                    drawTexture = Global.flyBirdTexture9;
                    break;
                case BirdState.Falling:
                    drawTexture = Global.fallBirdTexture9;
                    break;

            }

        }
        else if (p1Used && p2Used && p3Used && p4Used && p5Used && p6Used && p7Used && p8Used && p9Used && !p10Used)

        {
            drawTexture = Global.idleBirdTexture10;
            switch (state)
            {
                case BirdState.Idle:
                    drawTexture = Global.idleBirdTexture10;
                    break;
                case BirdState.Flying:
                    drawTexture = Global.flyBirdTexture10;
                    break;
                case BirdState.Falling:
                    drawTexture = Global.fallBirdTexture10;
                    break;

            }

        }
        else
        {
            drawTexture = Global.idleBirdTexture2;

        }


        Raylib.DrawTexturePro(drawTexture, new Rectangle(0.0f, 0.0f, drawTexture.Width, drawTexture.Height), new Rectangle(position.X, position.Y, drawTexture.Width, drawTexture.Height), new Vector2(drawTexture.Width / 2.0f, drawTexture.Height / 2.0f), rotateCounter, color);
        DrawAABB();
    }
    
    private void DrawAABB() {
        if (!Global.drawAABB) return;
        Raylib.DrawRectangleLines((int)(position.X - Global.idleBirdTexture.Width / 2.0f), (int)(position.Y - Global.idleBirdTexture.Height / 2.0f), Global.idleBirdTexture.Width, Global.idleBirdTexture.Height, Color.Red);
    }
}