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
        public readonly List<Move> Moves;
        private readonly IRulesEngine _rulesEngine;

        public Game(IBoard board, IRulesEngine rulesEngine)
        {
            Board = board;
            _rulesEngine = rulesEngine;
            Moves = new List<Move>();
            GameStatus = GameStatus.InProgress;
        }


        public Game(IBoard board, IRulesEngine rulesEngine, IEnumerable<Tuple<Piece, Position>> pieces)
        {
            Board = board;
            _rulesEngine = rulesEngine;
            Moves = new List<Move>();
            foreach (var piece in pieces)
            {
                board.AddPiece(piece.Item1, piece.Item2);
            }
        }

        public IBoard Board { get; }
        public GameStatus GameStatus { get; set; }


        public bool IsMoveLegal(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            var piece = Board.GetPieceAt(initialPosition);
            if (piece == null)
            {
                return false;
            }
            return piece.Colour == mover &&
                   _rulesEngine.IsMoveLegal(new Move(initialPosition, finalPosition, mover, piece, null), Board, Moves);
        }

    
        public void MakeMove(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            if (!IsMoveLegal(initialPosition, finalPosition, mover))
            {
                throw new ArgumentException("Move not legal");
            }
            var move = new Move(initialPosition, finalPosition, mover, null, null);
            Move updatedMoved = Board.MovePiece(move);
            Moves.Add(updatedMoved);
            updatedMoved.PieceMoved.HasMoved = true;
            PieceColour oppositeColour = mover==PieceColour.White?PieceColour.Black : PieceColour.White;

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
            var pieces = Board.GetPieces(pieceColour);
            return pieces.Any(x => GetPossibleMoves(x.Item2).Count > 0);
        }

        public bool? IsMoveLegal(Move move)
        {
            return IsMoveLegal(move.InitialPosition, move.FinalPosition, move.Mover);
        }

        public void MakeMove(Move move)
        {
            MakeMove(move.InitialPosition, move.FinalPosition, move.Mover);
        }

        public bool IsInCheck(PieceColour pieceColour)
        {
            PieceColour oppositeColour = pieceColour==PieceColour.White?PieceColour.Black:PieceColour.White;
            Position kingPosition = Board.GetKingPosition(pieceColour);
            return Board.GetPieces(oppositeColour).Any(x => IsMoveLegal(x.Item2, kingPosition, oppositeColour));

        }

        public IList<Move> GetPossibleMoves(Position piecePosition)
        {
            Piece piece = Board.GetPieceAt(piecePosition);
            IList < Position > positions = Board.GetPositions();
            List<Move> list = new List<Move>();
            foreach (Position position in positions)
            {
                Move potentialMove = new Move(piecePosition, position, piece.Colour, piece, null);
                if (_rulesEngine.IsMoveLegal(potentialMove, Board, Moves)) list.Add(potentialMove);
            }
            return list;
        }
    }
}