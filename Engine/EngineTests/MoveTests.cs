using Cardnell.Chess.Engine;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    public class MoveTests
    {
        [Test]
        public void AlgebraicCastling()
        {
            var whiteKingSide = new Move("0-0", PieceColour.White);
            var whiteQueenSide = new Move("0-0-0", PieceColour.White);
            var blackKingSide = new Move("0-0", PieceColour.Black);
            var blackQueenSide = new Move("0-0-0", PieceColour.Black);

            Assert.AreEqual(new Position("e1"), whiteKingSide.InitialPosition);
            Assert.AreEqual(new Position("g1"), whiteKingSide.FinalPosition);

            Assert.AreEqual(new Position("e1"), whiteQueenSide.InitialPosition);
            Assert.AreEqual(new Position("c1"), whiteQueenSide.FinalPosition);

            Assert.AreEqual(new Position("e8"), blackKingSide.InitialPosition);
            Assert.AreEqual(new Position("g8"), blackKingSide.FinalPosition);

            Assert.AreEqual(new Position("e8"), blackQueenSide.InitialPosition);
            Assert.AreEqual(new Position("c8"), blackQueenSide.FinalPosition);
        }

    }
}