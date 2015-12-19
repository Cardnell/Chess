using System;
using System.Collections.Generic;
using System.Linq;
using Cardnell.Chess.Engine.Rules;

namespace Cardnell.Chess.Engine
{
    public enum GameStatus
    {
        InProgress,
        WhiteWon,
        BlackWon,
        Drawn
    }

    public class Game
    {
        public static Game ClassicalGame()
        {
            return new Game(new Board(), new RefactoredClassicalRules(), GameSetup.Classical);
        }

        private readonly IRulesEngine _rulesEngine;
        public readonly List<Move> Moves;
        private readonly PgnMoveParser _parser;

        public Game(IBoard board, IRulesEngine rulesEngine)
        {
            Board = board;
            _rulesEngine = rulesEngine;
            _parser = new PgnMoveParser(_rulesEngine);
            Moves = new List<Move>();
            GameStatus = GameStatus.InProgress;
        }

        public IList<Move> GetPossibleMoves()
        {
            List<Move> output = new List<Move>();
            if (Moves.Count%2 == 1)
            {
                IEnumerable<Tuple<Piece, Position>> pieces = Board.GetPieces(PieceColour.Black);
                foreach (Position position in pieces.Select(x => x.Item2))
                {
                    output.AddRange(GetPossibleMoves(position));
                }
                return output;
            }
            IEnumerable<Tuple<Piece, Position>> whitePieces = Board.GetPieces(PieceColour.White);
            foreach (Position position in whitePieces.Select(x => x.Item2))
            {
                output.AddRange(GetPossibleMoves(position));
            }
            return output;
        }

        public Game(IBoard board, IRulesEngine rulesEngine, IEnumerable<Tuple<Piece, Position>> pieces)
            :this(board, rulesEngine)
        {
            foreach (Tuple<Piece, Position> piece in pieces)
            {
                board.AddPiece(piece.Item1, piece.Item2);
            }
        }

        public Game(IBoard board, IRulesEngine rulesEngine, List<Move> newMoves, GameStatus status)
        {
            Board = board;
            _rulesEngine = rulesEngine;
            _parser = new PgnMoveParser(_rulesEngine);
            Moves = newMoves;
            GameStatus = status;
        }

        public IBoard Board { get; }
        public GameStatus GameStatus { get; set; }


        public bool IsMoveLegal(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            if (Moves.Count % 2 == 0 && mover == PieceColour.Black)
            {
                return false;
            }
            if (Moves.Count % 2 == 1 && mover == PieceColour.White)
            {
                return false;
            }
            return _rulesEngine.IsMoveLegal(new Move(initialPosition, finalPosition, mover, null, null), Board, Moves);
        }


        public void MakeMove(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            if (!IsMoveLegal(initialPosition, finalPosition, mover))
            {
                throw new ArgumentException("Move not legal");
            }
            Piece pieceToMove = Board.GetPieceAt(initialPosition);
            var move = new Move(initialPosition, finalPosition, mover, pieceToMove, null);
            Move updatedMoved = Board.MovePiece(move);
            Moves.Add(updatedMoved);
            updatedMoved.PieceMoved.HasMoved = true;
            PieceColour oppositeColour = mover == PieceColour.White ? PieceColour.Black : PieceColour.White;

            if (!CanMove(oppositeColour))
            {
                if (IsInCheck(oppositeColour))
                {
                    GameStatus = mover == PieceColour.White ? GameStatus.WhiteWon : GameStatus.BlackWon;
                }
                else
                {
                    GameStatus = GameStatus.Drawn;
                }
            }
        }

        private bool CanMove(PieceColour pieceColour)
        {
            IEnumerable<Tuple<Piece, Position>> pieces = Board.GetPieces(pieceColour);
            return pieces.Any(x => GetPossibleMoves(x.Item2).Count > 0);
        }

        public bool IsMoveLegal(Move move)
        {
            return IsMoveLegal(move.InitialPosition, move.FinalPosition, move.Mover);
        }

        public void MakeMove(Move move)
        {
            MakeMove(move.InitialPosition, move.FinalPosition, move.Mover);
        }

        public bool IsInCheck(PieceColour pieceColour)
        {
            PieceColour oppositeColour = pieceColour == PieceColour.White ? PieceColour.Black : PieceColour.White;
            Position kingPosition = Board.GetKingPosition(pieceColour);
            return Board.GetPieces(oppositeColour).Any(x => _rulesEngine.IsMoveLegal(new Move(x.Item2, kingPosition, oppositeColour, null, null),Board, Moves));
        }

        public IList<Move> GetPossibleMoves(Position piecePosition)
        {
            Piece piece = Board.GetPieceAt(piecePosition);
            IList<Position> positions = Board.GetPositions();
            return
                positions.Select(position => new Move(piecePosition, position, piece.Colour, piece, null))
                    .Where(potentialMove => _rulesEngine.IsMoveLegal(potentialMove, Board, Moves))
                    .ToList();
        }

        public void MakeMove(string move, PieceColour pieceColour)
        {
            Move parsedMove = _parser.ParseMove(move, pieceColour, Board, Moves);

            MakeMove(parsedMove);
        }


        public Game Copy()
        {
            Board newBoard = Board.Copy();
            var newMoves = new List<Move>();
            newMoves.AddRange(Moves.Select(x=>x.Copy()).ToList());
            return new Game(newBoard, _rulesEngine, newMoves, GameStatus);
        }
    }
}