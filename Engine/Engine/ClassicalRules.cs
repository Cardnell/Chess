using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Cardnell.Chess.Engine
{
    public class ClassicalRules : IRulesEngine
    {
        public bool IsMoveLegal(Move move, IBoard board)
        {
            var pieceOnFinalSquare = board.GetPieceAt(move.FinalPosition);
            if (pieceOnFinalSquare != null)
            {
                if (pieceOnFinalSquare.Colour == move.Mover)
                {
                    return false;
                }               
            }

            switch (move.PieceMoved.PieceType)
            {
                case PieceType.King:
                    return IsKingMoveLegal(move);
                case PieceType.Knight:
                    return IsKnightMoveLegal(move);
                default:
                    return false;

            }
        }

        private bool IsKnightMoveLegal(Move move)
        {
            int rankMove = Math.Abs(move.InitialPosition.Rank - move.FinalPosition.Rank);
            int fileMove = Math.Abs(move.InitialPosition.File - move.FinalPosition.File);

            if (rankMove == 0 || rankMove > 2 || fileMove == 0 || fileMove > 2)
            {
                return false;
            }

            return (rankMove + fileMove) == 3;
        }

        private bool IsKingMoveLegal(Move move)
        {
             return Math.Abs(move.InitialPosition.Rank - move.FinalPosition.Rank) <= 1
                            && Math.Abs(  move.InitialPosition.File - move.FinalPosition.File) <= 1;
        }
    }
}
