using System;
using Cardnell.Chess.Engine;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    class RookTests
    {
        private Game _game;
        private Position _initialPosition;
        private Piece _piece;


        private void Init()
        {
            _game = new Game(new Board(), new ClassicalRules());
            _initialPosition = new Position(2, 3);
            _piece = new Piece(PieceColour.White, PieceType.Rook);
            _game.Board.AddPiece(_piece, _initialPosition);
        }

        [Test]
        public void CantMoveDiagnally()
        {
            Init();
            for (int i = 1; i < 7; i++)
            {
                var upLeft = new Position(_initialPosition.Rank + i, _initialPosition.File - i);
                var upRight = new Position(_initialPosition.Rank + i, _initialPosition.File + i);
                var downLeft = new Position(_initialPosition.Rank - i, _initialPosition.File - i);
                var downRight = new Position(_initialPosition.Rank - i, _initialPosition.File + i);

                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upLeft, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upRight, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downLeft, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downRight, _piece.Colour));
            }
        }

        [Test]
        public void CanMoveAlongFile()
        {
            Init();
            for (int i = 2; i < 7; i++)
            {
                 var upSome = new Position(_initialPosition.Rank, _initialPosition.File + i);
                if (upSome.File  < 8)
                {
                    Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upSome, _piece.Colour));
                }

                var downSome = new Position(_initialPosition.Rank, _initialPosition.File - i);
                if (downSome.File > 8)
                {
                    Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downSome, _piece.Colour));
                }
            }
        }

        [Test]
        public void CanMoveAlongRank()
        {
            Init();
            for (int i = 2; i < 7; i++)
            {
                var upSome = new Position(_initialPosition.Rank + i, _initialPosition.File);
                if (upSome.Rank < 8)
                {
                    Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upSome, _piece.Colour));
                }

                var downSome = new Position(_initialPosition.Rank - i, _initialPosition.File);
                if (downSome.Rank > 8)
                {
                    Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downSome, _piece.Colour));
                }
            }
        }

        [Test]
        public void CantJumpPiece()
        {
            Init();
            var finalPosition = new Position(_initialPosition.Rank + 2, _initialPosition.File);
            var inTheWay = new Position(_initialPosition.Rank + 1, _initialPosition.File);

            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Pawn), inTheWay);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, finalPosition, _piece.Colour));
        }

        [Test]
        public void CanTakeOtherColour()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank + 2, _initialPosition.File);
            var otherPiece = new Piece(PieceColour.Black, PieceType.Knight);
            _game.Board.AddPiece(otherPiece, upOne);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }

        [Test]
        public void CantTakeSameColour()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank + 2, _initialPosition.File);
            var otherPiece = new Piece(PieceColour.White, PieceType.Knight);
            _game.Board.AddPiece(otherPiece, upOne);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }


        [Test]
        public void CantMoveIntoCheck()
        {
            throw new NotImplementedException();
        }


        [Test]
        public void RandomOtherIlligalMoves()
        {
            Init();
            var upLeft = new Position(_initialPosition.Rank + 2, _initialPosition.File - 1);
            var upRight = new Position(_initialPosition.Rank + 2, _initialPosition.File + 1);
            var downLeft = new Position(_initialPosition.Rank - 2, _initialPosition.File - 1);
            var downRight = new Position(_initialPosition.Rank - 2, _initialPosition.File + 1);

            var upLeftMore = new Position(_initialPosition.Rank + 1, _initialPosition.File - 2);
            var upRightMore = new Position(_initialPosition.Rank + 1, _initialPosition.File + 2);
            var downLeftMore = new Position(_initialPosition.Rank - 1, _initialPosition.File - 2);
            var downRightMore = new Position(_initialPosition.Rank - 1, _initialPosition.File + 2);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upLeft, _piece.Colour));
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upRight, _piece.Colour));
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downLeft, _piece.Colour));
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downRight, _piece.Colour));

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upLeftMore, _piece.Colour));
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upRightMore, _piece.Colour));
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downLeftMore, _piece.Colour));
            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downRightMore, _piece.Colour));
        }
    }
}
