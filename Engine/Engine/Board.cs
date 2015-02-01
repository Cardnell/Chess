using System;
using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine
{

    public class Board: IBoard
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
            //iterates piece lsit twice, not ideal, look to rewrite if performance issue
            return !IsPieceAt(position) ? null : _pieceLocations.First(t => t.Item1.Equals(position)).Item2;
        }

        public bool IsPieceAt(Position position)
        {
            return _pieceLocations.Exists(t => t.Item1.Equals(position));
        }

    }
}