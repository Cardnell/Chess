using System;
using System.Collections.Generic;
using System.Linq;
using Cardnell.Chess.Engine;
using Cardnell.Chess.Engine.Rules;
using Moq;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    internal class GameTests
    {
        [SetUp]
        public void SetUp()
        {
            _board = new Board();
            _game = new Game(_board, _rulesEngine);
        }

        private IBoard _board;
        private readonly IRulesEngine _rulesEngine = new RefactoredClassicalRules();
        private Game _game;

        private static string GetPiecePoisitionString(Tuple<Piece, Position> value)
        {
            return value.Item1.PieceType.ToString() + value.Item1.Colour + value.Item2.Rank + value.Item2.File;
        }

        [Test]
        public void CheckMate()
        {
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("e1"));
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Queen), new Position("a2"));
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.King), new Position("e3"));

            Assert.AreEqual(GameStatus.InProgress, _game.GameStatus);
            _game.MakeMove(new Position("a2"), new Position("e2"), PieceColour.White);
            Assert.AreEqual(GameStatus.WhiteWon, _game.GameStatus);
        }

        [Test]
        public void GetListOfPossibleMovesForKing()
        {
            var whiteKingPosition = new Position("e1");
            var whiteKing = new Piece(PieceColour.White, PieceType.King);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("e8"));
            _game.Board.AddPiece(whiteKing, whiteKingPosition);

            IList<Move> _actualPossibleMoves = _game.GetPossibleMoves(whiteKingPosition);

            IList<Move> _expectedMoves = new List<Move>
            {
                new Move(whiteKingPosition, new Position("d1"), whiteKing.Colour, whiteKing, null),
                new Move(whiteKingPosition, new Position("d2"), whiteKing.Colour, whiteKing, null),
                new Move(whiteKingPosition, new Position("e2"), whiteKing.Colour, whiteKing, null),
                new Move(whiteKingPosition, new Position("f2"), whiteKing.Colour, whiteKing, null),
                new Move(whiteKingPosition, new Position("f1"), whiteKing.Colour, whiteKing, null)
            };

            Assert.AreEqual(_expectedMoves.Count, _actualPossibleMoves.Count);
            foreach (var move in _expectedMoves)
            {
                Assert.AreEqual(1,
                    _actualPossibleMoves.Count(x => x.FinalPosition.Equals(move.FinalPosition) && x.InitialPosition.Equals(move.InitialPosition)
                            &&
                            x.Mover == move.Mover && x.PieceMoved == move.PieceMoved));
            }
        }

        [Test]
        public void IllegalMoveWhenIsMoveNotLegal()
        {
            var boardMock = new Mock<IBoard>();
            var rulesEngineMock = new Mock<IRulesEngine>();

            var game = new Game(boardMock.Object, rulesEngineMock.Object);

            var initialPosition = new Position(2, 1);
            var finalPosition = new Position(4, 2);

            rulesEngineMock.Setup(x => x.IsMoveLegal(It.IsAny<Move>(), It.IsAny<Board>(), It.IsAny<IList<Move>>()))
                .Returns(false);
            var piece = new Piece(PieceColour.White, PieceType.Bishop);

            boardMock.Setup(x => x.GetPieceAt(It.IsAny<Position>())).Returns(piece);


            Assert.Throws<ArgumentException>(() => game.MakeMove(initialPosition, finalPosition, PieceColour.White));
        }

        [Test]
        public void IllegalMoveWhenNoPieceThere()
        {
            var boardMock = new Mock<IBoard>();
            var rulesEngineMock = new Mock<IRulesEngine>();

            var game = new Game(boardMock.Object, rulesEngineMock.Object);

            var initialPosition = new Position(2, 1);
            var finalPosition = new Position(4, 2);

            rulesEngineMock.Setup(x => x.IsMoveLegal(It.IsAny<Move>(), It.IsAny<Board>(), It.IsAny<IList<Move>>()))
                .Returns(true);

            boardMock.Setup(x => x.GetPieceAt(It.IsAny<Position>())).Returns((Piece) null);


            Assert.Throws<ArgumentException>(() => game.MakeMove(initialPosition, finalPosition, PieceColour.White));
        }

        [Test]
        public void IsInCheck()
        {
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.King), new Position("e1"));
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Queen), new Position("e4"));
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("e8"));

            Assert.IsTrue(_game.IsInCheck(PieceColour.White));
            Assert.IsFalse(_game.IsInCheck(PieceColour.Black));
        }

        [Test]
        public void IsMoveLegalInitialPieceWrongColour()
        {
            var boardMock = new Mock<IBoard>();
            var rulesEngineMock = new Mock<IRulesEngine>();


            var game = new Game(boardMock.Object, rulesEngineMock.Object);
            var initialPosition = new Position(1, 2);
            var finalPosition = new Position(3, 4);
            const PieceColour MOVER_COLOUR = PieceColour.White;


            boardMock.Setup(x => x.GetPieceAt(initialPosition)).Returns(new Piece(PieceColour.Black, PieceType.King));

            Assert.IsFalse(game.IsMoveLegal(initialPosition, finalPosition, MOVER_COLOUR));
        }

        [Test]
        public void IsMoveLegalNoPiece()
        {
            var boardMock = new Mock<IBoard>();
            var rulesEngineMock = new Mock<IRulesEngine>();


            var game = new Game(boardMock.Object, rulesEngineMock.Object);
            var initialPosition = new Position(1, 2);
            var finalPosition = new Position(3, 4);
            const PieceColour MOVER_COLOUR = PieceColour.White;


            boardMock.Setup(x => x.GetPieceAt(initialPosition)).Returns((Piece) null);

            Assert.IsFalse(game.IsMoveLegal(initialPosition, finalPosition, MOVER_COLOUR));
        }

        [Test]
        public void LegalMovePassedInAsMove()
        {
            var boardMock = new Mock<IBoard>();
            var rulesEngineMock = new Mock<IRulesEngine>();


            var game = new Game(boardMock.Object, rulesEngineMock.Object);
            var initialPosition = new Position(1, 2);
            var finalPosition = new Position(3, 4);
            var moverColour = PieceColour.White;
            var piece = new Piece(moverColour, PieceType.King);
            var move = new Move(initialPosition, finalPosition, moverColour, piece, null);


            boardMock.Setup(x => x.GetPieceAt(initialPosition)).Returns(piece);

            Move returnedMove = null;
            // rulesEngineMock.Verify(x => x.IsMoveLegal(It.IsAny<Move>(), It.IsAny<IBoard>()));
            rulesEngineMock.Setup(x => x.IsMoveLegal(It.IsAny<Move>(), It.IsAny<IBoard>(), It.IsAny<IList<Move>>()))
                .Returns(true)
                .Callback<Move, IBoard, IList<Move>>((m, b, l) => returnedMove = m);

            // Assert.IsFalse(game.IsMoveLegal(initialPosition, finalPosition, moverColour));


            game.IsMoveLegal(initialPosition, finalPosition, moverColour);

            Assert.AreEqual(move.InitialPosition, returnedMove.InitialPosition);
            Assert.AreEqual(move.FinalPosition, returnedMove.FinalPosition);
            Assert.AreEqual(move.Mover, returnedMove.Mover);
        }

        [Test]
        public void MakeMoveAddsToMoveList()
        {
            var boardMock = new Mock<IBoard>();
            var rulesEngineMock = new Mock<IRulesEngine>();

            var move = new Move(new Position(1, 1),
                new Position(2, 2),
                PieceColour.White,
                new Piece(PieceColour.White, PieceType.Knight),
                null);
            rulesEngineMock.Setup(x => x.IsMoveLegal(It.IsAny<Move>(), boardMock.Object, It.IsAny<List<Move>>()))
                .Returns(true);
            var piece = new Piece(PieceColour.White, PieceType.Knight);
            boardMock.Setup(x => x.GetPieceAt(move.InitialPosition)).Returns(piece);
            boardMock.Setup(x => x.MovePiece(It.IsAny<Move>())).Returns(move);

            var game = new Game(boardMock.Object, rulesEngineMock.Object);


            game.MakeMove(move.InitialPosition, move.FinalPosition, PieceColour.White);

            Assert.AreEqual(1, game.Moves.Count);
            Assert.AreEqual(move.InitialPosition, game.Moves[0].InitialPosition);
            Assert.AreEqual(move.FinalPosition, game.Moves[0].FinalPosition);
            Assert.AreEqual(move.Mover, game.Moves[0].Mover);
            //Assert.AreEqual(piece, game.Moves[0].PieceMoved);
            rulesEngineMock.VerifyAll();
        }

        [Test]
        public void SetupBoardWithMultiplePieces()
        {
            var rulesEngineMock = new Mock<IRulesEngine>();
            var boardMock = new Mock<IBoard>();

            var pieceOne = new Piece(PieceColour.White, PieceType.Knight);
            var postitionOne = new Position(0, 0);
            var pieceTwo = new Piece(PieceColour.White, PieceType.Knight);
            var postitionTwo = new Position(1, 1);

            var piecesToAdd = new List<Tuple<Piece, Position>>
            {
                new Tuple<Piece, Position>(pieceOne, postitionOne),
                new Tuple<Piece, Position>(pieceTwo, postitionTwo)
            };

            var game = new Game(boardMock.Object, rulesEngineMock.Object, piecesToAdd);
            boardMock.Verify(m => m.AddPiece(pieceOne, postitionOne));
            boardMock.Verify(m => m.AddPiece(pieceTwo, postitionTwo));
        }

        [Test]
        public void SetupBoardWithOnePiece()
        {
            var rulesEngineMock = new Mock<IRulesEngine>();
            var boardMock = new Mock<IBoard>();

            var piece = new Piece(PieceColour.White, PieceType.Knight);
            var postition = new Position(0, 0);

            var piecesToAdd = new List<Tuple<Piece, Position>> {new Tuple<Piece, Position>(piece, postition)};

            var game = new Game(boardMock.Object, rulesEngineMock.Object, piecesToAdd);
            boardMock.Verify(m => m.AddPiece(piece, postition));
        }

        [Test]
        public void SetupClassicBoard()
        {
            var rulesEngineMock = new Mock<IRulesEngine>();
            var boardMock = new Mock<IBoard>();


            var piecesToAdd = new Dictionary<Tuple<Piece, Position>, int>
            {
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.King), new Position(0, 4)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.King), new Position(7, 4)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Rook), new Position(0, 0)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Knight), new Position(0, 1)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Bishop), new Position(0, 2)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Queen), new Position(0, 3)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Bishop), new Position(0, 5)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Knight), new Position(0, 6)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Rook), new Position(0, 7)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 0)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 1)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 2)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 3)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 4)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(0, 5)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(0, 6)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(0, 7)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Rook), new Position(7, 0)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Knight), new Position(7, 1)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Bishop), new Position(7, 2)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Queen), new Position(7, 3)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Bishop), new Position(7, 5)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Knight), new Position(7, 6)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Rook), new Position(7, 7)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 0)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 1)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 2)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 3)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 4)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(7, 5)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(7, 6)), 1},
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(7, 7)), 1}
            };

            Dictionary<string, int> piecesComparison = piecesToAdd.Keys.ToDictionary(GetPiecePoisitionString, value => 1);

            var addCalls = new List<Tuple<Piece, Position>>();
            boardMock.Setup(c => c.AddPiece(It.IsAny<Piece>(), It.IsAny<Position>()))
                .Callback<Piece, Position>(
                    (piece, position) => addCalls.Add(new Tuple<Piece, Position>(piece, position)));

            var game = new Game(boardMock.Object, rulesEngineMock.Object, GameSetup.Classical);

            Assert.AreEqual(32, addCalls.Count);

            foreach (Tuple<Piece, Position> pieces in addCalls)
            {
                Assert.IsTrue(piecesComparison.ContainsKey(GetPiecePoisitionString(pieces)),
                    pieces.Item1.ToString() + pieces.Item2);
            }
        }

        [Test]
        public void SetupEmptyBoard()
        {
            var boardMock = new Mock<IBoard>();
            var rulesEngineMock = new Mock<IRulesEngine>();


            var game = new Game(boardMock.Object, rulesEngineMock.Object);

            boardMock.Verify(m => m.AddPiece(It.IsAny<Piece>(), It.IsAny<Position>()), Times.Never());
        }

        [Test]
        public void StaleMate()
        {
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("e1"));
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Queen), new Position("a3"));
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.King), new Position("f3"));

            Assert.AreEqual(GameStatus.InProgress, _game.GameStatus);
            _game.MakeMove(new Position("a3"), new Position("d3"), PieceColour.White);
            Assert.AreEqual(GameStatus.Drawn, _game.GameStatus);
        }
    }
}