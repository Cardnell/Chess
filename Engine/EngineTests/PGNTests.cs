using System.Collections.Generic;
using Cardnell.Chess.Engine;
using Cardnell.Chess.Engine.Rules;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace EngineTests
{
    [TestFixture]
    public class PGNTests
    {
        Game _game;

        [SetUp]
        public void Setup()
        {
            Board board = new Board();
            IRulesEngine rulesEngine = new RefactoredClassicalRules();
            _game = new Game(board, rulesEngine);
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.King), new Position("e1"));
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("e8"));
        }

        [Test]
        public void PawnMovesOneSpace()
        {
            var initialPosition = new Position("e2");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Pawn), initialPosition);

            _game.MakeMove("e3", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("e3"), actualMove.FinalPosition);
            Assert.AreEqual(new Position("e2"), actualMove.InitialPosition);
        }


        [Test]
        public void PawnMovesTwoSpaces()
        {
            var initialPosition = new Position("e2");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Pawn), initialPosition);

            _game.MakeMove("e4", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("e4"), actualMove.FinalPosition);
            Assert.AreEqual(new Position("e2"), actualMove.InitialPosition);
        }

        [Test]
        public void KnightMoves()
        {
            var initialPosition = new Position("g1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Knight), initialPosition);

            _game.MakeMove("Nf3", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("f3"), actualMove.FinalPosition);
            Assert.AreEqual(initialPosition, actualMove.InitialPosition);
        }

        [Test]
        public void BishopMoves()
        {
            var initialPosition = new Position("g1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Bishop), initialPosition);

            _game.MakeMove("Be3", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("e3"), actualMove.FinalPosition);
            Assert.AreEqual(initialPosition, actualMove.InitialPosition);
        }

        [Test]
        public void RookMoves()
        {
            var initialPosition = new Position("g1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), initialPosition);

            _game.MakeMove("Rg3", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("g3"), actualMove.FinalPosition);
            Assert.AreEqual(initialPosition, actualMove.InitialPosition);
        }


        [Test]
        public void QueenMoves()
        {
            var initialPosition = new Position("g1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Queen), initialPosition);

            _game.MakeMove("Qg3", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("g3"), actualMove.FinalPosition);
            Assert.AreEqual(initialPosition, actualMove.InitialPosition);
        }

        [Test]
        public void KingMoves()
        {
            _game.MakeMove("Kd1", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("d1"), actualMove.FinalPosition);
            Assert.AreEqual(new Position("e1"), actualMove.InitialPosition);
        }

        [Test]
        public void TwoPossiblePieces()
        {
            var initialPosition = new Position("a1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), initialPosition);
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), new Position("c1"));

            _game.MakeMove("Rcb1", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("c1"), actualMove.InitialPosition);
        }

        [Test]
        public void TwoPossiblePiecesOtherSide()
        {
            var initialPosition = new Position("a1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), initialPosition);
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), new Position("c1"));

            _game.MakeMove("Rab1", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(initialPosition, actualMove.InitialPosition);
        }
    }
}