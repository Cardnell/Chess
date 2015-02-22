using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cardnell.Chess.Engine
{
    public static class GameSetup
    {
        public static List<Tuple<Piece, Position>> Classical
        {
            get {  return                     new List<Tuple<Piece, Position>>
                    {

                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.King), new Position(0, 4)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.King), new Position(7, 4)),

                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Rook), new Position(0, 0)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Knight), new Position(0, 1)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Bishop), new Position(0, 2)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Queen), new Position(0, 3)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Bishop), new Position(0, 5)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Knight), new Position(0, 6)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Rook), new Position(0, 7)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 0)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 1)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 2)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 3)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(1, 4)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(0, 5)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(0, 6)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.White, PieceType.Pawn), new Position(0, 7)),

                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Rook), new Position(7, 0)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Knight), new Position(7, 1)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Bishop), new Position(7, 2)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Queen), new Position(7, 3)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Bishop), new Position(7, 5)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Knight), new Position(7, 6)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Rook), new Position(7, 7)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 0)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 1)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 2)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 3)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(1, 4)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(7, 5)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(7, 6)),
                        new Tuple<Piece, Position>(new Piece(PieceColour.Black, PieceType.Pawn), new Position(7, 7))

                    };}
        }
    }
}