using System;
using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class BishopMoveLegalityRuleChecker : PieceMoveLegalityChecker
    {
        public override bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
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
                        if (board.IsPieceAt(new Position(move.InitialPosition.Rank + i, move.InitialPosition.File - i)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                for (int i = 1; i < (move.FinalPosition.File - move.InitialPosition.File); i++)
                {
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank + i, move.InitialPosition.File + i)))
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
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank - i, move.InitialPosition.File - i)))
                    {
                        return false;
                    }
                }
                return true;
            }
            for (int i = 1; i < (move.FinalPosition.File - move.InitialPosition.File); i++)
            {
                if (board.IsPieceAt(new Position(move.InitialPosition.Rank - i, move.InitialPosition.File + i)))
                {
                    return false;
                }
            }
            return true;
        }

        public override IList<Move> GetLegalMoves(Position startingPosition, PieceColour colour, IBoard board, IList<Move> moves)
        {
            var output = new List<Move>();
            output.AddRange(IterateMovesInOneDirection(startingPosition, colour, board, x => new Position(x.Rank + 1, x.File + 1)));
            output.AddRange(IterateMovesInOneDirection(startingPosition, colour, board, x => new Position(x.Rank + 1, x.File - 1)));
            output.AddRange(IterateMovesInOneDirection(startingPosition, colour, board, x => new Position(x.Rank - 1, x.File + 1)));
            output.AddRange(IterateMovesInOneDirection(startingPosition, colour, board, x => new Position(x.Rank - 1, x.File - 1)));

            return output;
        }

    }
}