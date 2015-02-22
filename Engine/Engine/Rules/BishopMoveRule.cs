using System;
using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class BishopMoveRule : IMoveRule
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            if (Math.Abs(move.InitialPosition.Rank - move.FinalPosition.Rank) !=
                Math.Abs(move.InitialPosition.File - move.FinalPosition.File))
            {
                return false;
            }
            if (move.InitialPosition.Rank < move.FinalPosition.Rank)
            {
                if (move.InitialPosition.File > move.FinalPosition.File)
                {
                    for (int i = 1; i < (move.InitialPosition.File - move.FinalPosition.File); i++)
                    {
                        if (board.IsPieceAt(new Position(move.InitialPosition.Rank + 1, move.InitialPosition.File - i)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                for (int i = 1; i < (move.FinalPosition.File - move.InitialPosition.File); i++)
                {
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank + 1, move.InitialPosition.File + i)))
                    {
                        return false;
                    }
                }
                return true;
            }
            if (move.InitialPosition.File > move.FinalPosition.File)
            {
                for (int i = 1; i < (move.InitialPosition.File - move.FinalPosition.File); i++)
                {
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank - 1, move.InitialPosition.File - i)))
                    {
                        return false;
                    }
                }
                return true;
            }
            for (int i = 1; i < (move.FinalPosition.File - move.InitialPosition.File); i++)
            {
                if (board.IsPieceAt(new Position(move.InitialPosition.Rank - 1, move.InitialPosition.File + i)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}