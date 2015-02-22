using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Cardnell.Chess.Engine
{
    public class Board : IBoard
    {
        private Dictionary<PieceColour, Piece> _kings;
        private Dictionary<PieceColour, List<Piece>> _pieces;
        private Tuple<int, int> boardSize;


        public Board() : this(8, 8)
        {
        }

        public Board(int numberOfRanks, int numberOfFiles)
        {
            boardSize = new Tuple<int, int>(numberOfRanks, numberOfFiles);
            _kings = new Dictionary<PieceColour, Piece>();
            _pieces = new Dictionary<PieceColour, List<Piece>>();
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
            if (piece.PieceType != PieceType.King)
            {
                if (!_pieces.ContainsKey(piece.Colour))
                {
                    _pieces.Add(piece.Colour, new List<Piece>());
                }
                _pieces[piece.Colour].Add(piece);
                piece.Position = position;
                return;
            }
            AddKing(piece, position);
        }

        public Piece GetPieceAt(Position position)
        {
            if (!IsPositionOnBoard(position))
            {
                return null;
            }

            Piece output = _kings.Values.FirstOrDefault(t => t.Position.Equals(position));
            return output ??
                   _pieces.Values.Select(piecesList => piecesList.FirstOrDefault(t => t.Position.Equals(position)))
                       .FirstOrDefault(piece => piece != null);
        }

        public bool IsPieceAt(Position position)
        {
            return GetPieceAt(position) != null;
        }


        //todo Enpassant removal
        public void MovePiece(Move move)
        {
            Piece piece = GetPieceAt(move.InitialPosition);
            if (piece == null)
            {
                throw new ArgumentException("No piece at locattion");
            }
            move.PieceMoved = piece;
            Piece pieceToRemove = GetPieceAt(move.FinalPosition);
            if (pieceToRemove != null)
            {
                move.PieceTaken = pieceToRemove;
                RemovePieceAtLocation(move.FinalPosition);
            }
            piece.Position = move.FinalPosition;
            
        }

        public void ReverseMove(Move move)
        {
            Piece piece = GetPieceAt(move.FinalPosition);
            piece.Position = move.InitialPosition;
            if (move.PieceTaken != null)
            {
                AddPiece(move.PieceTaken, move.FinalPosition);
            }
        }

        public IEnumerable<Piece> GetPieces(PieceColour colour)
        {
            if (_kings.ContainsKey(colour))
            {
                return _pieces.ContainsKey(colour)
                    ? _pieces[colour].Select(t => t).Concat(new List<Piece> {_kings[colour]})
                    : new List<Piece> {_kings[colour]};
            }
            return _pieces.ContainsKey(colour)
                ? _pieces[colour]
                : new List<Piece>();
        }


        public Piece GetKing(PieceColour colour)
        {
            return _kings.ContainsKey(colour) ? _kings[colour] : null;
        }

        public bool IsPositionOnBoard(Position position)
        {
            if (position.Rank < 0 || position.File < 0)
            {
                return false;
            }
            return (position.Rank < boardSize.Item1 && position.File < boardSize.Item2);
        }

        private void AddKing(Piece piece, Position position)
        {
            if (!_kings.ContainsKey(piece.Colour))
            {
                _kings.Add(piece.Colour, null);
            }

                    if (_kings[piece.Colour] != null)
                    {
                        throw new ArgumentException(string.Format("{0} king already present", piece.Colour));
                    }
            _kings[piece.Colour] = piece;
            piece.Position = position;
        }

        private void RemovePieceAtLocation(Position finalPosition)
        {
            if (_kings.Values.Any(t => t.Position.Equals(finalPosition)))
            {
                throw new ArgumentException("Can't remove king");
            }
            foreach (List<Piece> pieces in _pieces.Values)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (!pieces[i].Position.Equals(finalPosition)) continue;
                    pieces.RemoveAt(i);
                    return;
                }
            }
        }
    }
}