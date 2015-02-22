using System;
using Cardnell.Chess.Engine;
using Cardnell.Chess.Engine.Rules;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    class KingTests
    {
        private Game _game;
        private Position _initialPosition;
        private Piece _piece;


        private void Init()
        {
            _game = new Game(new Board(), new RefactoredClassicalRules());
            _initialPosition = new Position(2,3);
            _piece = new Piece(PieceColour.White, PieceType.King);
            _game.Board.AddPiece(_piece, _initialPosition);
        }

        [Test]
        public void CanMoveOneSpaceInAFile()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank, _initialPosition.File + 1);
            var downOne = new Position(_initialPosition.Rank, _initialPosition.File - 1);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downOne, _piece.Colour));
        }

        [Test]
        public void CanMoveOneSpaceInARank()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank + 1, _initialPosition.File);
            var downOne = new Position(_initialPosition.Rank - 1, _initialPosition.File);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downOne, _piece.Colour));
        }

        [Test]
        public void CanMoveOneSpaceDiagnally()
        {
            Init();
            var upLeft = new Position(_initialPosition.Rank + 1, _initialPosition.File - 1);
            var upRight = new Position(_initialPosition.Rank + 1, _initialPosition.File + 1);
            var downLeft = new Position(_initialPosition.Rank - 1, _initialPosition.File - 1);
            var downRight = new Position(_initialPosition.Rank - 1, _initialPosition.File + 1);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upLeft, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upRight, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downLeft, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downRight, _piece.Colour));
        }

        [Test]
        public void CantMoveMoreThanOneSpaceInAFile()
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
        public void CantMoveMoreThanOneSpaceInARank()
        {
            Init();
            for (int i = 2; i <7; i++)
            {
                var upSome = new Position(_initialPosition.Rank + i, _initialPosition.File);
                var downSome = new Position(_initialPosition.Rank - i, _initialPosition.File);

                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upSome, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downSome, _piece.Colour));
            }
        }
        [Test]
        public void CantMoveMoreThanOneSpaceDiagnally()
        {
            Init();
            for (int i = 2; i < 7; i++)
            {
                var upLeft = new Position(_initialPosition.Rank + i, _initialPosition.File - i);
                var upRight = new Position(_initialPosition.Rank + i, _initialPosition.File + i);
                var downLeft = new Position(_initialPosition.Rank -i, _initialPosition.File -i);
                var downRight = new Position(_initialPosition.Rank - i, _initialPosition.File +i);

                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upLeft, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upRight, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downLeft, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downRight, _piece.Colour));
            }
        }

        [Test]
        public void CanTakeOtherColour()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank, _initialPosition.File + 1);
            var otherPiece = new Piece(PieceColour.Black, PieceType.Knight);
            _game.Board.AddPiece(otherPiece, upOne);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }

        [Test]
        public void CantTakeSameColour()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank, _initialPosition.File + 1);
            var otherPiece = new Piece(PieceColour.White, PieceType.Knight);
            _game.Board.AddPiece(otherPiece, upOne);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }


        [Test]
        public void CantMoveIntoCheck()
        {
            Init();
            var BishopPosition = new Position(_initialPosition.Rank + 1, _initialPosition.File + 2);
            var newPosition = new Position(_initialPosition.Rank, _initialPosition.File + 1);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Bishop), BishopPosition);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, newPosition, _piece.Colour));
        }

        //[Test]
        //public void RandomOtherIlligalMoves()
        //{
        //    Init();
        //    throw new NotImplementedException();
        //}

    }
}
