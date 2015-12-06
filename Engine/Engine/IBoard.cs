using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Cardnell.Chess.Engine
{
    public interface IBoard
    {
        void AddPiece(Piece piece, Position position);
        Piece GetPieceAt(Position position);
        bool IsPieceAt(Position position);
        Move MovePiece(Move move);


        IEnumerable<Tuple<Piece, Position>> GetPieces(PieceColour colour);

        Position GetKingPosition(PieceColour colour);
        //void ReverseMove(Move move);
        bool IsPositionOnBoard(Position initialPosition);
    }
}