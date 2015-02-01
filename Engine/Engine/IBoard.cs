using System.Security.Cryptography.X509Certificates;

namespace Cardnell.Chess.Engine
{
    public interface IBoard
    {
        void AddPiece(Piece piece, Position position);
        Piece GetPieceAt(Position position);
        bool IsPieceAt(Position position);
    }
}