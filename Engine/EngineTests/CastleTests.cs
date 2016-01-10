﻿using System.Collections.Generic;
using Cardnell.Chess.Engine;
using Cardnell.Chess.Engine.Rules;
using Moq;
using NUnit.Framework;

namespace EngineTests
{
    [TestFixture]
    public class CastleTests
    {
        private IMoveLegalityRuleChecker _legalityRulesCheckerEngine;
        private Board _board;

        [SetUp]
        public void Setup()
        {
            //rules enginge is used to check on checks for castling, except where testing this, we want to 
            //return false to show nothing can move to that position

            _board = new Board();
            _legalityRulesCheckerEngine = new CastlingLegalityRuleChecker(new SimplePieceRules());
            _board.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("e8"));
        }

        private Board GetSimpleCorrectBoard()
        {
            _board.AddPiece(new Piece(PieceColour.White, PieceType.King), new Position("e1"));
            _board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), new Position("h1"));
            return _board;
        }

        [Test]
        public void CantCastleIntoCheck()
        {
            var rulesEngineMock = new Mock<IRulesEngine>();
            IMoveLegalityRuleChecker castlingLegalityRulesCheckerEngine = new CastlingLegalityRuleChecker(rulesEngineMock.Object);
            var board = GetSimpleCorrectBoard();
            board.AddPiece(new Piece(PieceColour.Black, PieceType.Queen), new Position("e2"));

            rulesEngineMock.Setup(
                x =>
                    x.IsMoveLegal(
                        It.Is<Move>(
                            y =>
                                y.InitialPosition.Equals(new Position("g2")) &&
                                y.FinalPosition.Equals(new Position("e1"))), board, It.IsAny<IList<Move>>()))
                .Returns(false);

            rulesEngineMock.Setup(
                x =>
                    x.IsMoveLegal(
                        It.Is<Move>(
                            y =>
                                y.InitialPosition.Equals(new Position("g2")) &&
                                y.FinalPosition.Equals(new Position("g1"))), board, It.IsAny<IList<Move>>()))
                .Returns(true);
            board.AddPiece(new Piece(PieceColour.Black, PieceType.Rook), new Position("g2"));
            Assert.IsFalse(castlingLegalityRulesCheckerEngine.IsMoveLegal(new Move("0-0", PieceColour.White), board, new List<Move>()));
            rulesEngineMock.VerifyAll();

        }

        [Test]
        public void CantCastleThroughPieces()
        {
            var board = GetSimpleCorrectBoard();
            board.AddPiece(new Piece(PieceColour.White, PieceType.Queen), new Position("f1"));
            Assert.IsFalse(_legalityRulesCheckerEngine.IsMoveLegal(new Move("0-0", PieceColour.White), board, new List<Move>()));
        }


        [Test]
        public void CantCastletThroughCheck()
        {
            var rulesEngineMock = new Mock<IRulesEngine>();
            IMoveLegalityRuleChecker castlingLegalityRulesCheckerEngine = new CastlingLegalityRuleChecker(rulesEngineMock.Object);
            var board = GetSimpleCorrectBoard();
            board.AddPiece(new Piece(PieceColour.Black, PieceType.Queen), new Position("e2"));

            rulesEngineMock.Setup(
                x =>
                    x.IsMoveLegal(
                        It.Is<Move>(
                            y =>
                                y.InitialPosition.Equals(new Position("f2")) &&
                                y.FinalPosition.Equals(new Position("e1"))), board, It.IsAny<IList<Move>>()))
                .Returns(false);


            rulesEngineMock.Setup(
                x =>
                    x.IsMoveLegal(
                        It.Is<Move>(
                            y =>
                                y.InitialPosition.Equals(new Position("f2")) &&
                                y.FinalPosition.Equals(new Position("f1"))), board, It.IsAny<IList<Move>>()))
                .Returns(true);

            board.AddPiece(new Piece(PieceColour.Black, PieceType.Rook), new Position("f2"));
            Assert.IsFalse(castlingLegalityRulesCheckerEngine.IsMoveLegal(new Move("0-0", PieceColour.White), board, new List<Move>()));
            rulesEngineMock.VerifyAll();
        }

        [Test]
        public void CantCastleWhenInCheck()
        {
            var rulesEngineMock = new Mock<IRulesEngine>();
            IMoveLegalityRuleChecker castlingLegalityRulesCheckerEngine = new CastlingLegalityRuleChecker(rulesEngineMock.Object);
            var board = GetSimpleCorrectBoard();
            board.AddPiece(new Piece(PieceColour.Black, PieceType.Queen), new Position("e2"));

            rulesEngineMock.Setup(
                x =>
                    x.IsMoveLegal(
                        It.Is<Move>(
                            y =>
                                y.InitialPosition.Equals(new Position("e2")) &&
                                y.FinalPosition.Equals(new Position("e1"))), board, It.IsAny<IList<Move>>()))
                .Returns(true);

            Assert.IsFalse(castlingLegalityRulesCheckerEngine.IsMoveLegal(new Move("0-0", PieceColour.White), board, new List<Move>()));
            rulesEngineMock.VerifyAll();
        }


        [Test]
        public void CastlingInGameSetting()
        {
            var board = GetSimpleCorrectBoard();

            var game = new Game(board, new RefactoredClassicalRules());

            Assert.IsTrue(game.IsMoveLegal(new Move("0-0", PieceColour.White)));

        }

        [Test]
        public void CantCastleWhenPiecesMoves()
        {
            var board = GetSimpleCorrectBoard();

            var game = new Game(board, new RefactoredClassicalRules());

            game.MakeMove(new Move(new Position("h1"), new Position("g1"), PieceColour.White, null, null));
            game.MakeMove(new Move(new Position("e8"), new Position("f8"), PieceColour.Black, null, null));
            game.MakeMove(new Move(new Position("g1"), new Position("h1"), PieceColour.White, null, null));
            game.MakeMove(new Move(new Position("f8"), new Position("e8"), PieceColour.Black, null, null));

            Assert.IsFalse(game.IsMoveLegal(new Move("0-0", PieceColour.White)));
        }

        [Test]
        public void KingsideCastle()
        {
            var board = _board;
            var kingStartPosition = new Position("E1");
            var rookStartPosition = new Position("H1");
            var kingEndPosition = new Position("G1");
            var rookEndPosition = new Position("F1");

            var rules = new Mock<IRulesEngine>();

            var game = new Game(board, new RefactoredClassicalRules());

            rules.Setup(x => x.IsMoveLegal(It.IsAny<Move>(), board, new List<Move>())).Returns(true);

            board.AddPiece(new Piece(PieceColour.White, PieceType.King), kingStartPosition);
            board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), rookStartPosition);

            game.MakeMove(kingStartPosition, kingEndPosition, PieceColour.White);

            Assert.AreEqual(PieceType.King, board.GetPieceAt(kingEndPosition).PieceType);
            Assert.AreEqual(PieceType.Rook, board.GetPieceAt(rookEndPosition).PieceType);
        }

        [Test]
        public void PieceMissing()
        {
            var kingsBoard = new Board();
            var rooksBoard = new Board();

            var king = new Piece(PieceColour.White, PieceType.King);
            var rook = new Piece(PieceColour.White, PieceType.Rook);
            var blackKing = new Piece(PieceColour.Black, PieceType.King);
            var kingPosition = new Position("E1");
            var kingSideRook = new Position("H1");
            var blackKingPosition = new Position("E8");

            kingsBoard.AddPiece(king, kingPosition);
            kingsBoard.AddPiece(blackKing, blackKingPosition);
            rooksBoard.AddPiece(rook, kingSideRook);
            rooksBoard.AddPiece(blackKing, blackKingPosition);

            var kingSideCastle = new Move("0-0", PieceColour.White);

            Assert.IsFalse(_legalityRulesCheckerEngine.IsMoveLegal(kingSideCastle, kingsBoard, new List<Move>()));
            Assert.IsFalse(_legalityRulesCheckerEngine.IsMoveLegal(kingSideCastle, rooksBoard, new List<Move>()));
        }

        [Test]
        public void PiecesInCorrectLocation()
        {
            var queenSideBoard = new Board();
            var blackKingSideBoard = new Board();
            var blackQueenSideBoard = new Board();

            var king = new Piece(PieceColour.White, PieceType.King);
            var rook = new Piece(PieceColour.White, PieceType.Rook);
            var kingPosition = new Position("E1");
            var queenSideRook = new Position("A1");

            var blackKing = new Piece(PieceColour.Black, PieceType.King);
            var blackRook = new Piece(PieceColour.Black, PieceType.Rook);
            var blackKingPosition = new Position("E8");
            var blackKingSideRook = new Position("H8");
            var blackQueenSideRook = new Position("A8");


            queenSideBoard.AddPiece(king, kingPosition);
            queenSideBoard.AddPiece(rook, queenSideRook);
            queenSideBoard.AddPiece(blackKing, blackKingPosition);

            blackKingSideBoard.AddPiece(blackKing, blackKingPosition);
            blackKingSideBoard.AddPiece(blackRook, blackKingSideRook);
            blackKingSideBoard.AddPiece(king, kingPosition);

            blackQueenSideBoard.AddPiece(blackKing, blackKingPosition);
            blackQueenSideBoard.AddPiece(blackRook, blackQueenSideRook);
            blackQueenSideBoard.AddPiece(king, kingPosition);

            var kingSideCastle = new Move("0-0", PieceColour.White);
            var queenSideCastle = new Move("0-0-0", PieceColour.White);
            var blackKingSideCastle = new Move("0-0", PieceColour.Black);
            var blackQueenSideCastle = new Move("0-0-0", PieceColour.Black);

            Assert.IsTrue(_legalityRulesCheckerEngine.IsMoveLegal(kingSideCastle, GetSimpleCorrectBoard(), new List<Move>()),
                "Kingside");
            Assert.IsTrue(_legalityRulesCheckerEngine.IsMoveLegal(queenSideCastle, queenSideBoard, new List<Move>()), "Queenside");

            Assert.IsTrue(_legalityRulesCheckerEngine.IsMoveLegal(blackKingSideCastle, blackKingSideBoard, new List<Move>()),
                "black Kingside");
            Assert.IsTrue(_legalityRulesCheckerEngine.IsMoveLegal(blackQueenSideCastle, blackQueenSideBoard, new List<Move>()),
                "black Queenside");
        }


        [Test]
        public void QueensideCastle()
        {
            var board = _board;
            var kingStartPosition = new Position("E1");
            var rookStartPosition = new Position("A1");
            var kingEndPosition = new Position("C1");
            var rookEndPosition = new Position("D1");

            var rules = new Mock<IRulesEngine>();

            var game = new Game(board, new RefactoredClassicalRules());

            rules.Setup(x => x.IsMoveLegal(It.IsAny<Move>(), board, new List<Move>())).Returns(true);

            board.AddPiece(new Piece(PieceColour.White, PieceType.King), kingStartPosition);
            board.AddPiece(new Piece(PieceColour.White, PieceType.Rook), rookStartPosition);

            game.MakeMove(kingStartPosition, kingEndPosition, PieceColour.White);

            Assert.AreEqual(PieceType.King, board.GetPieceAt(kingEndPosition).PieceType);
            Assert.AreEqual(PieceType.Rook, board.GetPieceAt(rookEndPosition).PieceType);
        }

        [Test]
        public void WrongColourPieces()
        {
            var kingSideBoard = new Board();

            var king = new Piece(PieceColour.Black, PieceType.King);
            var rook = new Piece(PieceColour.Black, PieceType.Rook);
            var kingPosition = new Position("E1");
            var kingSideRook = new Position("H1");


            kingSideBoard.AddPiece(king, kingPosition);
            kingSideBoard.AddPiece(rook, kingSideRook);
            kingSideBoard.AddPiece(new Piece(PieceColour.White, PieceType.King), new Position("e8"));

            var kingSideCastle = new Move("0-0", PieceColour.Black);

            Assert.IsFalse(_legalityRulesCheckerEngine.IsMoveLegal(kingSideCastle, kingSideBoard, new List<Move>()), "Kingside");
        }

        [Test]
        public void WrongPieces()
        {

            var kingsBoard = new Board();
            var rooksBoard = new Board();

            var king = new Piece(PieceColour.White, PieceType.King);
            var queen = new Piece(PieceColour.White, PieceType.Queen);
            var rook = new Piece(PieceColour.White, PieceType.Rook);
            var kingPosition = new Position("E1");
            var kingSideRook = new Position("H1");

            kingsBoard.AddPiece(king, kingPosition);
            kingsBoard.AddPiece(queen, kingSideRook);
            kingsBoard.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("e8"));
            rooksBoard.AddPiece(rook, kingSideRook);
            rooksBoard.AddPiece(queen, kingPosition);
            rooksBoard.AddPiece(new Piece(PieceColour.Black, PieceType.King), new Position("e8"));

            var kingSideCastle = new Move("0-0", PieceColour.White);

            Assert.IsFalse(_legalityRulesCheckerEngine.IsMoveLegal(kingSideCastle, kingsBoard, new List<Move>()));
            Assert.IsFalse(_legalityRulesCheckerEngine.IsMoveLegal(kingSideCastle, rooksBoard, new List<Move>()));
        }

        [Test]
        public void WrongTypeOfCastle()
        {
            var board = GetSimpleCorrectBoard();


            Assert.IsFalse(_legalityRulesCheckerEngine.IsMoveLegal(new Move("0-0-0", PieceColour.White), board, new List<Move>()));
        }
    }
}