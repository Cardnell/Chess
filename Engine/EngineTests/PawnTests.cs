using Cardnell.Chess.Engine;
using Cardnell.Chess.Engine.Rules;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    internal class PawnTests
    {
        private Game _game;
        private Position _initialPosition;
        private Piece _piece;


        private void Init()
        {
            _game = new Game(new Board(), new RefactoredClassicalRules());
            _initialPosition = new Position(2, 3);
            _piece = new Piece(PieceColour.White, PieceType.Pawn);
            _game.Board.AddPiece(_piece, _initialPosition);
        }

        [Test]
        public void CanMoveDiagnallyOneAndCapture()
        {
            Init();
            var upRight = new Position(_initialPosition.Rank + 1, _initialPosition.File + 1);
            var upLeft = new Position(_initialPosition.Rank + 1, _initialPosition.File - 1);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Bishop), upRight);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Bishop), upLeft);
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upRight, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upLeft, _piece.Colour));
        }

        [Test]
        public void CanMoveOneSpaceForward()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank + 1, _initialPosition.File);
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }

        [Test]
        public void CanMoveTwoSpacesForwardOnFirstMove()
        {
            Init();
            var upTwo = new Position(_initialPosition.Rank + 2, _initialPosition.File);
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upTwo, _piece.Colour));
        }

        [Test]
        public void CantCaptureOwnColour()
        {
            Init();
            var upRight = new Position(_initialPosition.Rank + 1, _initialPosition.File + 1);
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.Bishop), upRight);
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upRight, _piece.Colour));
        }

        [Test]
        public void CantEnpassantAfterMoveTest()
        {
            Init();

            var pawnPosition = new Position(4, 4);
            var thePiece = new Piece(PieceColour.Black, PieceType.Pawn);
            var pawnMoveTo = new Position(_initialPosition.Rank + 2, _initialPosition.File);


            var whiteBishop = new Piece(PieceColour.White, PieceType.Bishop);
            var whiteBishopPosition = new Position(7, 7);
            var blackBishop = new Piece(PieceColour.Black, PieceType.Bishop);
            var blackBishopPosition = new Position(7, 6);

            _game.Board.AddPiece(thePiece, pawnPosition);
            _game.Board.AddPiece(whiteBishop, whiteBishopPosition);
            _game.Board.AddPiece(blackBishop, blackBishopPosition);

            _game.MakeMove(_initialPosition, pawnMoveTo, PieceColour.White);
            _game.MakeMove(blackBishopPosition, new Position(6, 5), PieceColour.Black);
            _game.MakeMove(whiteBishopPosition, new Position(6, 6), PieceColour.White);

            Assert.IsTrue(_game.IsMoveLegal(pawnPosition, pawnMoveTo, PieceColour.Black));
        }


        [Test]
        public void CantMoveAlongRank()
        {
            Init();
            for (int i = 2; i < 7; i++)
            {
                var upSome = new Position(_initialPosition.Rank, _initialPosition.File + i);
                var downSome = new Position(_initialPosition.Rank, _initialPosition.File - i);

                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upSome, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downSome, _piece.Colour));
            }
        }

        [Test]
        public void CantMoveDiagnallyWithoutCapture()
        {
            Init();
            var upRight = new Position(_initialPosition.Rank + 1, _initialPosition.File + 1);
            var upLeft = new Position(_initialPosition.Rank + 1, _initialPosition.File - 1);
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upRight, _piece.Colour));
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upLeft, _piece.Colour));
        }

        [Test]
        public void CantMoveIntoCheck()
        {
            Init();
            var KingPosition = new Position(_initialPosition.Rank - 1, _initialPosition.File - 1);
            var BishopPosition = new Position(_initialPosition.Rank + 1, _initialPosition.File + 1);
            var newPosition = new Position(_initialPosition.Rank + 1, _initialPosition.File);
            _game.Board.AddPiece(new Piece(_piece.Colour, PieceType.King), KingPosition);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Bishop), BishopPosition);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, newPosition, _piece.Colour));
        }

        [Test]
        public void CantMoveOneStepFowardAndCapture()
        {
            Init();

            var upOne = new Position(_initialPosition.Rank + 1, _initialPosition.File);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Bishop), upOne);
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }

        [Test]
        public void CantMoveTwoSpacesForwardAndCapture()
        {
            Init();

            var upTwo = new Position(_initialPosition.Rank + 2, _initialPosition.File);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Bishop), upTwo);
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upTwo, _piece.Colour));
        }

        [Test]
        public void CantMoveTwoSpacesForwardOnFutureMoves()
        {
            Init();

            var upOne = new Position(_initialPosition.Rank + 1, _initialPosition.File);
            var upTwo = new Position(_initialPosition.Rank + 2, _initialPosition.File);
            _game.MakeMove(_initialPosition, upOne, _piece.Colour);
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upTwo, _piece.Colour));
        }

        [Test]
        public void EnpassantTest()
        {
            Init();

            var newPosition = new Position(4, 4);
            var thePiece = new Piece(PieceColour.Black, PieceType.Pawn);
            var pawnMoveTo = new Position(_initialPosition.Rank + 2, _initialPosition.File);

            _game.Board.AddPiece(thePiece, newPosition);

            _game.MakeMove(_initialPosition, pawnMoveTo, PieceColour.White);

            Assert.IsTrue(_game.IsMoveLegal(newPosition, new Position(_initialPosition.Rank + 1, _initialPosition.File),
                PieceColour.Black));
        }

        [Test]
        public void RandomOtherIlligalMoves()
        {
            Init();
            for (int i = 3; i < 7; i++)
            {
                var upSome = new Position(_initialPosition.Rank, _initialPosition.File + i);
                var downSome = new Position(_initialPosition.Rank, _initialPosition.File - i);

                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upSome, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downSome, _piece.Colour));
            }
        }
    }
}