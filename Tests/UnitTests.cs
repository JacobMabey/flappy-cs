using System.Numerics;
using NUnit.Framework;

namespace BankTests
{
    [TestFixture]
    public class BankAccountTests
    {
        [Test]
        public void TestMethod1()
        {
            Bird bird = new Bird(new Vector2(Global.OG_WIDTH / 2 - Global.idleBirdTexture.Width / 2 + 1 * 3,
                Global.OG_HEIGHT + 30), 1);

            Assert.That(bird.IsGrounded, Is.True);
        }

        [Test]
        public void TestMethod2()
        {
            Pipe pipe = new Pipe(new Vector2(Global.OG_WIDTH / 2 - Global.idleBirdTexture.Width / 2 + 1 * 3,
                Global.OG_HEIGHT / 2 - Global.idleBirdTexture.Height / 2), Pipe.PipeType.Up);

            Bird bird = new Bird(new Vector2(Global.OG_WIDTH / 2 - Global.idleBirdTexture.Width / 2 + 1 * 3,
                Global.OG_HEIGHT / 2 - Global.idleBirdTexture.Height / 2), 1);

            bird.CheckCollisionWithPipes();
            Assert.That(bird.IsDead, Is.False);
        }
    }
}