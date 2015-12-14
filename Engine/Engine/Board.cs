using System;
using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine
{
    public class Board : IBoard
    {
        private readonly Tuple<int, int> _boardSize;
        private Piece[,] _squares;
        private Dictionary<PieceColour, Position> _kingPosition;


        public Board() : this(8, 8)
        {
        }

        public Board(int numberOfRanks, int numberOfFiles)
        {
            _squares = new Piece[numberOfRanks, numberOfFiles];
            _boardSize = new Tuple<int, int>(numberOfRanks, numberOfFiles);
        }

        public void AddPiece(Piece piece, Position position)
        {
            if (!IsPositionOnBoard(position))
            {
                throw new ArgumentException("Position given off board");
            }
            if (IsPieceAt(position))
            {
                throw new ArgumentException("Piece already at position" + position);
            }
            _squares[position.Rank, position.File] = piece;
        }

        public Piece GetPieceAt(Position position)
        {
            if (position.Rank < _squares.GetLength(0) && position.File < _squares.GetLength(1))
            {
                return _squares[position.Rank, position.File];
            }
            return null;
        }

        public bool IsPieceAt(Position position)
        {
            return GetPieceAt(position) != null;
        }


        public Move MovePiece(Move move)
        {
            Piece piece = GetPieceAt(move.InitialPosition);
            if (piece == null)
            {
                throw new ArgumentException("No piece at locattion");
            }
            move.PieceMoved = piece;
            if (IsCastling(move))
            {
                Castle(move);
                return move;
            }
            if (IsEnpassant(move))
            {
                Enpassant(move);
                return move;
            }
            Piece pieceToRemove = GetPieceAt(move.FinalPosition);
            if (pieceToRemove != null)
            {
                move.PieceTaken = pieceToRemove;
            }

            _squares[move.FinalPosition.Rank, move.FinalPosition.File] =
                _squares[move.InitialPosition.Rank, move.InitialPosition.File];

            _squares[move.InitialPosition.Rank, move.InitialPosition.File] = null;
            return move;

        }

        private void Enpassant(Move move)
        {

            _squares[move.FinalPosition.Rank, move.FinalPosition.File] =
                _squares[move.InitialPosition.Rank, move.InitialPosition.File];

            _squares[move.InitialPosition.Rank, move.InitialPosition.File] = null;
            move.PieceTaken = _squares[move.InitialPosition.Rank, move.FinalPosition.File];

            _squares[move.InitialPosition.Rank, move.FinalPosition.File] = null;
        }

        private bool IsEnpassant(Move move)
        {
            if (GetPieceAt(move.InitialPosition).PieceType != PieceType.Pawn)
            {
                return false;
            }
            return !IsPieceAt(move.FinalPosition);
        }

        private void Castle(Move move)
        {
            _squares[move.FinalPosition.Rank, move.FinalPosition.File] =
                _squares[move.InitialPosition.Rank, move.InitialPosition.File];

            _squares[move.InitialPosition.Rank, move.InitialPosition.File] = null;
            if (move.FinalPosition.File > move.InitialPosition.File)
            {
                _squares[move.InitialPosition.Rank, move.InitialPosition.File + 1] =
                    _squares[move.InitialPosition.Rank, move.InitialPosition.File + 3];
                _squares[move.InitialPosition.Rank, move.InitialPosition.File + 3] = null;
            }
            else
            {
                _squares[move.InitialPosition.Rank, move.InitialPosition.File - 1] =
                    _squares[move.InitialPosition.Rank, move.InitialPosition.File - 4];
                _squares[move.InitialPosition.Rank, move.InitialPosition.File - 4] = null;

            }
        }

        private bool IsCastling(Move move)
        {
            return GetPieceAt(move.InitialPosition).PieceType == PieceType.King
                   && Math.Abs(move.FinalPosition.File - move.InitialPosition.File) == 2;
        }

        public IEnumerable<Tuple<Piece, Position>> GetPieces(PieceColour colour)
        {
            return GetPieceSelection(x=>x.Colour == colour);
        }

        private IEnumerable<Tuple<Piece, Position>> GetPieceSelection(Func<Piece, bool> condition)
        {
            var pieces = new List<Tuple<Piece, Position>>();
            for (int i = 0; i < _squares.GetLength(0); i++)
            {
                for (int j = 0; j < _squares.GetLength(0); j++)
                {
                    if (_squares[i, j] == null)
                    {
                        continue;
                    }
                    if (condition(_squares[i,j]))
                    {
                        pieces.Add(new Tuple<Piece, Position>(_squares[i, j], new Position(i, j)));
                    }
                }
            }
            return pieces;
        }


        public Position GetKingPosition(PieceColour colour)
        {
            for (int i = 0; i < _squares.GetLength(0); i++)
            {
                for (int j = 0; j < _squares.GetLength(1); j++)
                {
                    if (_squares[i, j] == null)
                    {
                        continue;
                    }
                    if (_squares[i, j].PieceType == PieceType.King && _squares[i, j].Colour == colour)
                    {
                        return new Position(i, j);
                    }
                }
            }
            throw new ArgumentException("No king of colour " + colour);
        }

        public bool IsPositionOnBoard(Position position)
        {
            if (position.Rank < 0 || position.File < 0)
            {
                return false;
            }
            return (position.Rank < _boardSize.Item1 && position.File < _boardSize.Item2);
        }

        public IList<Position> GetPositions()
        {
            var output = new List<Position>();
            for (int i = 0; i < _squares.GetLength(0); i++)
            {
                for (int j = 0; j < _squares.GetLength(1); j++)
                {
                    output.Add(new Position(i, j));
                }
            }

            return output;
        }

        public Board Copy()
        {
            var output = new Board(_boardSize.Item1, _boardSize.Item2);
            for (int i = 0; i < _squares.GetLength(0); i++)
            {
                for (int j = 0; j < _squares.GetLength(1); j++)
                {
                    Position position = new Position(i, j);
                    Piece piece = GetPieceAt(position);
                    if (piece != null)
                    {
                        output.AddPiece(piece, position);
                    }
                }
            }
            return output;
        }

        public IEnumerable<Tuple<Piece, Position>> GetPieces(PieceColour colour, PieceType pieceType)
        {
            return GetPieceSelection(x => x.Colour == colour  && x.PieceType == pieceType);
        }
    }
}