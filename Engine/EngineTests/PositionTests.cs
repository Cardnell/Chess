using Cardnell.Chess.Engine;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    public class PositionTests
    {
        [Test]
        public void NonReferentialEquality()
        {
            Assert.IsTrue(new Position(1, 4).Equals(new Position(1, 4)));
        }
    }
}