using System;
using System.Collections.Generic;

namespace Cardnell.Chess.Engine
{

    public class Board
    {
        private List<Tuple<Position, Piece>> _pieceLocations;

        public Board()
        {
            _pieceLocations = new List<Tuple<Position, Piece>>();
        }

        public void AddPiece(Piece piece, Position position)
        {
            if (IsPieceAt(position))
            {
                throw new ArgumentException("Piece already at position" + position);
            }
            _pieceLocations.Add(new Tuple<Position, Piece>(position, piece));
        }

        public Piece GetPieceAt(Position position)
        {
            throw new NotImplementedException();
        }

        public bool IsPieceAt(Position position)
        {
            return _pieceLocations.Exists(t => t.Item1.Equals(position));
        }

    }
}