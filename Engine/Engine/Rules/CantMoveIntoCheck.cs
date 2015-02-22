using System;
using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class CantMoveIntoCheck : IMoveRule
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            board.MovePiece(move);
            try
            {
                
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                board.ReverseMove(move);
            }
            
            return true;
        }
    }
}