using System.Numerics;
using Raylib_cs;

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