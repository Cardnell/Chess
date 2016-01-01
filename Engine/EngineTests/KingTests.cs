using System;
using System.Collections.Generic;
using System.Linq;
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


        [SetUp]
        public void Setup()
        {
            _game = new Game(new Board(), new RefactoredClassicalRules());
            _initialPosition = new Position(2,3);
            _piece = new Piece(PieceColour.White, PieceType.King);
            _game.Board.AddPiece(_piece, _initialPosition);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("a8"));
        }

        [Test]
        public void CanMoveOneSpaceInAFile()
        {
            var upOne = new Position(_initialPosition.Rank, _initialPosition.File + 1);
            var downOne = new Position(_initialPosition.Rank, _initialPosition.File - 1);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downOne, _piece.Colour));
        }

        [Test]
        public void CanMoveOneSpaceInARank()
        {
            var upOne = new Position(_initialPosition.Rank + 1, _initialPosition.File);
            var downOne = new Position(_initialPosition.Rank - 1, _initialPosition.File);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, downOne, _piece.Colour));
        }

        [Test]
        public void CanMoveOneSpaceDiagnally()
        {
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
            var upOne = new Position(_initialPosition.Rank, _initialPosition.File + 1);
            var otherPiece = new Piece(PieceColour.Black, PieceType.Knight);
            _game.Board.AddPiece(otherPiece, upOne);

            Assert.IsTrue(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }

        [Test]
        public void CantTakeSameColour()
        {
            var upOne = new Position(_initialPosition.Rank, _initialPosition.File + 1);
            var otherPiece = new Piece(PieceColour.White, PieceType.Knight);
            _game.Board.AddPiece(otherPiece, upOne);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, upOne, _piece.Colour));
        }


        [Test]
        public void CantMoveIntoCheck()
        {
            var bishopPosition = new Position(_initialPosition.Rank + 1, _initialPosition.File + 2);
            var newPosition = new Position(_initialPosition.Rank, _initialPosition.File + 1);
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Bishop), bishopPosition);

            Assert.IsFalse(_game.IsMoveLegal(_initialPosition, newPosition, _piece.Colour));
        }

        [Test]
        public void CantMoveInStandardMatePosition()
        {   
            _game.Board.AddPiece(new Piece(PieceColour.White, PieceType.King), new Position("e1"));
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.Queen), new Position("e2"));
            _game.Board.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("e3"));

            Assert.IsFalse(
                _game.IsMoveLegal(new Move(new Position("e1"), new Position("d1"), PieceColour.White, null, null)));
        }

        [Test]
        public void GetCorrectListOfPossibleMoves()
        {
            IList<Move> possibleMoves = _game.GetPossibleMoves(_initialPosition);

            IList<Position> expectedFinalPositions = new List<Position>
            {

                new Position(1,2),
                new Position(1, 3),
                new Position(1, 4),
                
                new Position(2, 2),
                new Position(2, 4),

                new Position(3,2),
                new Position(3, 3),
                new Position(3, 4)


            };

            CollectionAssert.AreEquivalent(expectedFinalPositions, possibleMoves.Select(x => x.FinalPosition));
        }

        //[Test]
        //public void RandomOtherIlligalMoves()
        //{
        //    Init();
        //    throw new NotImplementedException();
        //}

    }
}
