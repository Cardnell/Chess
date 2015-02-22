using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Cardnell.Chess.Engine
{
    public interface IBoard
    {
        void AddPiece(Piece piece, Position position);
        Piece GetPieceAt(Position position);
        bool IsPieceAt(Position position);
        void MovePiece(Move move);
        IEnumerable<Piece> GetPieces(PieceColour colour);
        Piece GetKing(PieceColour colour);
        void ReverseMove(Move move);
        bool IsPositionOnBoard(Position initialPosition);
    }
}