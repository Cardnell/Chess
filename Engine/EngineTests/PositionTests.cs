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

        [Test]
        public void AlgebraicNotation()
        {
            Assert.AreEqual(2, new Position("D3").Rank);
            Assert.AreEqual(3, new Position("D3").File);
            Assert.AreEqual(5, new Position("E6").Rank);
            Assert.AreEqual(4, new Position("E6").File);
        }
    }
}