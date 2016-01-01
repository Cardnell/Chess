using System;
using System.Collections.Generic;
using System.Linq;
using Cardnell.Chess.Engine;
using Cardnell.Chess.Engine.Rules;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    class KnightTests
    {
        private Game _game;
        private Position _initialPosition;
        private Piece _piece;


        private void Init()
        {
            InitWithoutKings();
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.King), new Position(0, 0));
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position(7, 7));
        }
        private void InitWithoutKings()
        {
            _game = new Game(new Board(), new RefactoredClassicalRules());
            _initialPosition = new Position(4, 5);
            _piece = new Piece(PieceColour.White, PieceType.Knight);
            _game.Board.AddPiece(_piece, _initialPosition);
        }

        [Test]
        public void GetCorrectListOfPossibleMoves()
        {
            Init();

            IList<Move> possibleMoves= _game.GetPossibleMoves(_initialPosition);

            IList<Position> expectedFinalPositions = new List<Position>
            {
                new Position(_initialPosition.Rank + 1, _initialPosition.File + 2),
                new Position(_initialPosition.Rank + 1, _initialPosition.File - 2),

                new Position(_initialPosition.Rank - 1, _initialPosition.File + 2),
                new Position(_initialPosition.Rank - 1, _initialPosition.File - 2),

                new Position(_initialPosition.Rank + 2, _initialPosition.File + 1),
                new Position(_initialPosition.Rank + 2, _initialPosition.File - 1),

                new Position(_initialPosition.Rank - 2, _initialPosition.File + 1),
                new Position(_initialPosition.Rank - 2, _initialPosition.File - 1)
            };

            CollectionAssert.AreEquivalent(expectedFinalPositions, possibleMoves.Select(x=>x.FinalPosition));
        }

        [Test]
        public void CanMoveInLShape()
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

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upLeft, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upRight, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downLeft, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downRight, _piece.Colour));

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upLeftMore, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upRightMore, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downLeftMore, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downRightMore, _piece.Colour));
        }

        [Test]
        public void CantMoveInStrightLine()
        {
            Init();
            for (int i = 1; i < 7; i++)
            {
                var upSome = new Position(_initialPosition.Rank + i, _initialPosition.File);
                var downSome = new Position(_initialPosition.Rank - i, _initialPosition.File);
                var leftSome = new Position(_initialPosition.Rank, _initialPosition.File - i);
                var rightSome = new Position(_initialPosition.Rank, _initialPosition.File - i);

                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upSome, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, downSome, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, leftSome, _piece.Colour));
                Assert.IsFalse(_game.IsMoveLegal(_initialPosition, rightSome, _piece.Colour));
            }
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
        public void CanTakeOtherColour()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank + 2, _initialPosition.File + 1);
            var otherPiece = new Piece(PieceColour.Black, PieceType.Knight);
            _game.Board.AddPiece(otherPiece, upOne);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }

        [Test]
        public void CantTakeSameColour()
        {
            Init();
            var upOne = new Position(_initialPosition.Rank + 2, _initialPosition.File + 1);
            var otherPiece = new Piece(PieceColour.White, PieceType.Knight);
            _game.Board.AddPiece(otherPiece, upOne);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }

        [Test]
        public void CantMoveIntoCheck()
        {
            InitWithoutKings();
            var KingPosition = new Position(_initialPosition.Rank - 1, _initialPosition.File - 1);
            var BishopPosition = new Position(_initialPosition.Rank + 1, _initialPosition.File + 1);
            var newPosition = new Position(_initialPosition.Rank + 2, _initialPosition.File +1);
            _game.Board.AddPiece(new Piece(_piece.Colour, PieceType.King), KingPosition);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Bishop), BishopPosition);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, newPosition, _piece.Colour));
        }


    }
}
