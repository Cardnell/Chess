using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardnell.Chess.Engine;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    class BishopTests
    {
        private Game _game;
        private Position _initialPosition;
        private Piece _piece;


        private void Init()
        {
            _game = new Game(new Board(), new ClassicalRules());
            _initialPosition = new Position(2, 3);
            _piece = new Piece(PieceColour.White, PieceType.Bishop);
            _game.Board.AddPiece(_piece, _initialPosition);
        }
        
        [Test]
        public void CanMoveDiagnally()
        {
            Init();
            for (int i = 1; i < 7; i++)
            {
                var upLeft = new Position(_initialPosition.Rank + i, _initialPosition.File - i);
                if (upLeft.Rank < 8 && upLeft.File > 0)
                {
                    Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upLeft, _piece.Colour));
                }

                var upRight = new Position(_initialPosition.Rank + i, _initialPosition.File + i);
                if (upRight.Rank < 8 && upRight.File > 0)
                {
                    Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upRight, _piece.Colour));
                }

                var downLeft = new Position(_initialPosition.Rank - i, _initialPosition.File - i);
                if (downLeft.Rank < 8 && downLeft.File > 0)
                {
                    Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downLeft, _piece.Colour));
                }

                var downRight = new Position(_initialPosition.Rank - i, _initialPosition.File + i);
                if (downRight.Rank < 8 && downRight.File > 0)
                {
                    Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downRight, _piece.Colour));
                }
            }
        }

        [Test]
        public void CantMoveAlongFile()
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
        public void CantMoveAlongRank()
        {
            Init();
            for (int i = 2; i < 7; i++)
            {
                var upSome = new Position(_initialPosition.Rank + i, _initialPosition.File);
                var downSome = new Position(_initialPosition.Rank - i, _initialPosition.File);

                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upSome, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downSome, _piece.Colour));
            }
        }

        [Test]
        public void CantJumpPiece()
        {
            Init();
            var finalPosition = new Position(_initialPosition.Rank + 2, _initialPosition.File + 2);
            var inTheWay = new Position(_initialPosition.Rank + 1, _initialPosition.File + 1);

            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Pawn), inTheWay);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, finalPosition, _piece.Colour));
        }

        [Test]
        public void CanTakeOtherColour()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank + 2, _initialPosition.File+2);
            var otherPiece = new Piece(PieceColour.Black, PieceType.Knight);
            _game.Board.AddPiece(otherPiece, upOne);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }

        [Test]
        public void CantTakeSameColour()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank + 2, _initialPosition.File+2);
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
