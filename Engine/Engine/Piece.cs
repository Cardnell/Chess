using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardnell.Chess.Engine
{
    public enum PieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King

    }

    public enum PieceColour
    {
        White,
        Black
    }

    public class Piece
    {
        public Piece(PieceColour colour, PieceType pieceType)
        {
            Colour = colour;
            PieceType = pieceType;
            HasMoved = false;

        }

        public Piece Copy()
        {
            return new Piece(Colour, PieceType) {HasMoved = HasMoved};
        }

        public PieceType PieceType { get; private set; }

        public PieceColour Colour { get; private set; }
        public bool HasMoved { get; set; }
      //  public Position Position { get; set; }

        public override string ToString()
        {
            return $"{Colour} {PieceType}";
        }
    }
}
