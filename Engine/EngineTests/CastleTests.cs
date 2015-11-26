using System.Collections.Generic;
using Cardnell.Chess.Engine;
using Cardnell.Chess.Engine.Rules;
using Moq;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    public class CastleTests
    {
        [Test]
        public void KingsideCastle()
        {
            var board = new Board();

            Position kingStartPosition = new Position("E1");
            Position rookStartPosition = new Position("H1");
            Position kingEndPosition = new Position("G1");
            Position rookEndPosition = new Position("F1");

            var rules = new Mock<IRulesEngine>();

            var game= new Game(board, rules.Object);

            rules.Setup(x => x.IsMoveLegal(It.IsAny<Move>(), board, new List<Move>())).Returns(true);

            board.AddPiece(new Piece(PieceColour.White, PieceType.King), kingStartPosition);
            board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), rookStartPosition);

            game.MakeMove(kingStartPosition, kingEndPosition, PieceColour.White);

            Assert.AreEqual(PieceType.King, board.GetPieceAt(kingEndPosition).PieceType);
            Assert.AreEqual(PieceType.Rook, board.GetPieceAt(rookEndPosition).PieceType);
        }

        [Test]
        public void QueensideCastle()
        {
            var board = new Board();

            Position kingStartPosition = new Position("E1");
            Position rookStartPosition = new Position("A1");
            Position kingEndPosition = new Position("C1");
            Position rookEndPosition = new Position("D1");

            var rules = new Mock<IRulesEngine>();

            var game = new Game(board, rules.Object);

            rules.Setup(x => x.IsMoveLegal(It.IsAny<Move>(), board, new List<Move>())).Returns(true);

            board.AddPiece(new Piece(PieceColour.White, PieceType.King), kingStartPosition);
            board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), rookStartPosition);

            game.MakeMove(kingStartPosition, kingEndPosition, PieceColour.White);

            Assert.AreEqual(PieceType.King, board.GetPieceAt(kingEndPosition).PieceType);
            Assert.AreEqual(PieceType.Rook, board.GetPieceAt(rookEndPosition).PieceType);
        }

    }
}