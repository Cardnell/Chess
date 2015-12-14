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
            return positions.Select(position => new Move(piecePosition, position, piece.Colour, piece, null)).Where(potentialMove => _rulesEngine.IsMoveLegal(potentialMove, Board, Moves)).ToList();
        }

        public void MakeMove(string move, PieceColour pieceColour)
        {
            var finalPosition = GetPGNMoveLocation(move);
            PieceType pieceType = GetPieceType(move);
            Position initalPostion = GetInitialPosition(finalPosition, pieceType, pieceColour);
            MakeMove(initalPostion, finalPosition, pieceColour);
        }

        private Position GetPGNMoveLocation(string move)
        {
            return new Position(move[move.Length - 2].ToString() + move[move.Length - 1]);
        }

        private PieceType GetPieceType(string move)
        {
            if (move.Length == 2)
            {
                return PieceType.Pawn;
            }
            string pieceTypeString = move[0].ToString();
            if (pieceTypeString == "N")
            {
                return PieceType.Knight;
            }
            if (pieceTypeString == "B")
            {
                return PieceType.Bishop;
            }
            if (pieceTypeString == "R")
            {
                return PieceType.Rook;
            }
            if (pieceTypeString == "Q")
            {
                return PieceType.Queen;
            }
            if (pieceTypeString == "K")
            {
                return PieceType.King;
            }
            throw new ArgumentException("Piece type not recognised");
        }

        private Position GetInitialPosition(Position finalPosition, PieceType pieceType, PieceColour pieceColour)
        {
            var piecePositions = Board.GetPieces(PieceColour.White, pieceType);
            return piecePositions.First(x => IsMoveLegal(x.Item2, finalPosition, pieceColour)).Item2;
        }
    }
}