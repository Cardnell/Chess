﻿using System;
using System.Collections.Generic;
using Cardnell.Chess.Engine;
using Cardnell.Chess.Engine.Rules;
using Moq;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    internal class GameTests
    {
        private static string GetPiecePoisitionString(Tuple<Piece, Position> value)
        {
            return value.Item1.PieceType.ToString() + value.Item1.Colour + value.Item2.Rank + value.Item2.File;
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
            Assert.AreEqual(move.PieceMoved, returnedMove.PieceMoved);
            Assert.AreEqual(move.PieceTaken, returnedMove.PieceTaken);
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
                {new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(7, 7)), 1},
            };

            var piecesComparison = new Dictionary<string, int>();
            foreach (var value in piecesToAdd.Keys)
            {
                piecesComparison.Add(GetPiecePoisitionString(value), 1);
            }

            var addCalls = new List<Tuple<Piece, Position>>();
            boardMock.Setup(c => c.AddPiece(It.IsAny<Piece>(), It.IsAny<Position>()))
                .Callback<Piece, Position>(
                    (piece, position) => addCalls.Add(new Tuple<Piece, Position>(piece, position)));

            var game = new Game(boardMock.Object, rulesEngineMock.Object, GameSetup.Classical);

            Assert.AreEqual(32, addCalls.Count);

            foreach (var pieces in addCalls)
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


    }
}