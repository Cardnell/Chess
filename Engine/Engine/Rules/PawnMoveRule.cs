using System;
using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class PawnMoveRule : IMoveRule
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            Piece pieceOnFinalSquare = board.GetPieceAt(move.FinalPosition);
            if (pieceOnFinalSquare != null)
                return (Math.Abs(move.InitialPosition.File - move.FinalPosition.File) == 1 &&
                        (move.InitialPosition.Rank + 1 == move.FinalPosition.Rank));
            if (EnpassantTest(move, board, moves))
            {
                return true;
            }
            if (move.InitialPosition.File != move.FinalPosition.File)
            {
                return false;
            }
            if (move.InitialPosition.Rank + 1 == move.FinalPosition.Rank)
            {
                return true;
            }
            return (move.InitialPosition.Rank + 2 == move.FinalPosition.Rank)
                   && board.GetPieceAt(move.InitialPosition).HasMoved == false;
        }

        private bool EnpassantTest(Move move, IBoard board, IList<Move> moves)
        {
            if (moves.Count == 0)
            {
                return false;
            }
            if (Math.Abs(move.InitialPosition.File - move.FinalPosition.File) != 1)
            {
                return false;
            }
            int direction = GetDirection(move.PieceMoved.Colour);
            if (direction*(move.FinalPosition.Rank - move.InitialPosition.Rank) != 1)
            {
                return false;
            }

            Move lastMove = moves[moves.Count - 1];
            Piece lastPiece = lastMove.PieceMoved;;
            if (lastPiece.PieceType != PieceType.Pawn)
            {
                return false;
            }
            if (lastPiece.Colour == move.PieceMoved.Colour)
            {
                return false;
            }
            if (lastMove.FinalPosition.File != move.FinalPosition.File)
            {
                return false;
            }
            if (Math.Abs(lastMove.FinalPosition.Rank - move.FinalPosition.Rank) == 1 && Math.Abs(lastMove.FinalPosition.Rank - move.FinalPosition.Rank) == 1)
            {
                return true;
            }

            return false;
        }

        private static int GetDirection(PieceColour colour)
        {
            if (colour == PieceColour.White)
            {
                return 1;
            }
            return -1;
        
        }
    }
}