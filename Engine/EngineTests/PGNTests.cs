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
        public void KingSideCastleWhite()
        {

            var parser = new PgnMoveParser(new RefactoredClassicalRules());
            Move move = parser.ParseMove("0-0", PieceColour.White, _game.Board, _game.Moves);
            Assert.AreEqual(new Position("e1"), move.InitialPosition);
            Assert.AreEqual(new Position("g1"), move.FinalPosition);
        }
        [Test]
        public void KingSideCastleBlack()
        {

            var parser = new PgnMoveParser(new RefactoredClassicalRules());
            Move move = parser.ParseMove("0-0", PieceColour.Black, _game.Board, _game.Moves);
            Assert.AreEqual(new Position("e8"), move.InitialPosition);
            Assert.AreEqual(new Position("g8"), move.FinalPosition);
        }
        [Test]
        public void QueenSideCastleWhite()
        {

            var parser = new PgnMoveParser(new RefactoredClassicalRules());
            Move move = parser.ParseMove("0-0-0", PieceColour.White, _game.Board, _game.Moves);
            Assert.AreEqual(new Position("e1"), move.InitialPosition);
            Assert.AreEqual(new Position("c1"), move.FinalPosition);
        }
        [Test]
        public void QueenSideCastleBlack()
        {

            var parser = new PgnMoveParser(new RefactoredClassicalRules());
            Move move = parser.ParseMove("0-0-0", PieceColour.Black, _game.Board, _game.Moves);
            Assert.AreEqual(new Position("e8"), move.InitialPosition);
            Assert.AreEqual(new Position("c8"), move.FinalPosition);
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
        public void TwoPossiblePiecesWithTheSameRank()
        {
            var initialPosition = new Position("a1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), initialPosition);
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), new Position("c1"));

            _game.MakeMove("Rcb1", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("c1"), actualMove.InitialPosition);
        }

        [Test]
        public void TwoPossiblePiecesWithTheSameRankOtherSide()
        {
            var initialPosition = new Position("a1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), initialPosition);
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), new Position("c1"));

            _game.MakeMove("Rab1", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(initialPosition, actualMove.InitialPosition);
        }
        [Test]
        public void TwoPossiblePiecesWithTheSameFile()
        {
            var initialPosition = new Position("a1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), initialPosition);
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), new Position("a3"));

            _game.MakeMove("R1a2", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("a1"), actualMove.InitialPosition);
        }

        [Test]
        public void TwoPossiblePiecesWithTheSameFileOtherSide()
        {
            var initialPosition = new Position("a1");
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), initialPosition);
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), new Position("a3"));

            _game.MakeMove("R3a2", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(new Position("a3"), actualMove.InitialPosition);
        }

        [Test]
        public void PawnTakesPiece()
        {
            Piece pieceToTake = new Piece(PieceColour.Black, PieceType.Rook);

            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Pawn), new Position("e2"));
            _game.Board.AddPiece(pieceToTake, new Position("f3"));

            _game.MakeMove("xf3", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(pieceToTake, actualMove.PieceTaken);
        }


        [Test]
        public void PieceTakesPiece()
        {
            Piece pieceToTake = new Piece(PieceColour.Black, PieceType.Rook);

            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Bishop), new Position("e2"));
            _game.Board.AddPiece(pieceToTake, new Position("f3"));

            _game.MakeMove("Bxf3", PieceColour.White);
            Move actualMove = _game.Moves[0];
            Assert.AreEqual(pieceToTake, actualMove.PieceTaken);
        }
    }
}