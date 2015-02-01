using System;
using Cardnell.Chess.Engine;
using NUnit.Framework;


namespace EngineTests
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void EmptyBoard()
        {
            var board = new Board();
            var position = new Position(3, 5);
            Assert.IsFalse(board.IsPieceAt(position));
        }

        [Test]
        public void OnePiece()
        {
            var board = new Board();
            var position = new Position(3, 5);
            var otherPosition = new Position(5, 3);

            board.AddPiece(new Piece(PieceColour.White, PieceType.King), position);

            Assert.IsFalse(board.IsPieceAt(otherPosition));
            Assert.IsTrue(board.IsPieceAt(position));
        }

        [Test]
        public void TestCheckIsntReference()
        {
            var board = new Board();
            var position = new Position(3, 5);
            var pieceLocation = new Position(3, 5);

            board.AddPiece(new Piece(PieceColour.White, PieceType.King), position);

            Assert.IsTrue(board.IsPieceAt(pieceLocation));
        }

        [Test]
        public void TwoPieces()
        {
            var board = new Board();
            var position = new Position(3, 5);
            var secondPosition = new Position(2, 2);
            var otherPosition = new Position(5, 3);

            board.AddPiece(new Piece(PieceColour.White, PieceType.King), position);
            board.AddPiece(new Piece(PieceColour.White, PieceType.King), secondPosition);
            Assert.IsFalse(board.IsPieceAt(otherPosition));
            Assert.IsTrue(board.IsPieceAt(position));
            Assert.IsTrue(board.IsPieceAt(secondPosition));
        }

        [Test]
        public void CantAddPieceToSameLocation()
        {
            var board = new Board();
            var position = new Position(3, 5);

            board.AddPiece(new Piece(PieceColour.White, PieceType.King), position);

            Assert.Throws<ArgumentException>(() => board.AddPiece(new Piece(), position));
        }
    }
}