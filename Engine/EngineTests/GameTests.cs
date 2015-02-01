using System;
using System.Runtime.InteropServices;
using Cardnell.Chess.Engine;
using Moq;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    class GameTests
    {
        #region BoardSetup
        [Test]
        public void SetupEmptyBoard()
        {
            throw new NotImplementedException();
        }

        public void SetupClassicBoard()
        {
            throw new NotImplementedException();
        }

        public void SetupBoardWithOnePiece()
        {
            throw new NotImplementedException();
        }

        public void SetupBoardWithMultiplePieces()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TryToAddPieceOutOfRange()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IsMoveLegal
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
            var move = new Move(initialPosition, finalPosition, moverColour, piece, false);


            boardMock.Setup(x => x.GetPieceAt(initialPosition)).Returns(piece);

            Move returnedMove = null;
           // rulesEngineMock.Verify(x => x.IsMoveLegal(It.IsAny<Move>(), It.IsAny<IBoard>()));
            rulesEngineMock.Setup(x => x.IsMoveLegal(It.IsAny<Move>(), It.IsAny<IBoard>()))
                .Returns(true)
                .Callback<Move, IBoard>((m, b) => returnedMove =m);

           // Assert.IsFalse(game.IsMoveLegal(initialPosition, finalPosition, moverColour));



            game.IsMoveLegal(initialPosition, finalPosition, moverColour);

            Assert.AreEqual(move.InitialPosition, returnedMove.InitialPosition);
            Assert.AreEqual(move.FinalPosition, returnedMove.FinalPosition);
            Assert.AreEqual(move.Mover, returnedMove.Mover);
            Assert.AreEqual(move.PieceMoved, returnedMove.PieceMoved);
            Assert.AreEqual(move.TookPeice, returnedMove.TookPeice);

        }

        [Test]
        public void IsMoveLegalNoPiece()
        {
            var boardMock = new Mock<IBoard>();
            var rulesEngineMock = new Mock<IRulesEngine>();


            var game = new Game(boardMock.Object, rulesEngineMock.Object);
            var initialPosition = new Position(1, 2);
            var finalPosition = new Position(3, 4);
            var moverColour = PieceColour.White;


            boardMock.Setup(x => x.GetPieceAt(initialPosition)).Returns((Piece)null);

            Assert.IsFalse(game.IsMoveLegal(initialPosition, finalPosition, moverColour));
        }

        [Test]
        public void IsMoveLegalInitialPieceWrongColour()
        {
            var boardMock = new Mock<IBoard>();
            var rulesEngineMock = new Mock<IRulesEngine>();


            var game = new Game(boardMock.Object, rulesEngineMock.Object);
            var initialPosition = new Position(1,2);
            var finalPosition = new Position(3, 4);
            var moverColour = PieceColour.White;


            boardMock.Setup(x => x.GetPieceAt(initialPosition)).Returns(new Piece(PieceColour.Black, PieceType.King));

            Assert.IsFalse(game.IsMoveLegal(initialPosition, finalPosition, moverColour));
            

        }
        #endregion


    }
}
