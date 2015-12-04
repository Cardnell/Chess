using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class CastlingRule: IRulesEngine
    {
        readonly Dictionary<PieceColour, Position[]> _positions = new Dictionary<PieceColour, Position[]>();
        IRulesEngine _rulesEngineForChecks;
        const int KING_POSITION= 0;
        const int KINGS_ROOK_POSITION = 1;
        const int QUEENS_ROOK_POSITION = 2;

        public CastlingRule(IRulesEngine rulesEngineForChecks)
        {
            _rulesEngineForChecks = rulesEngineForChecks;
            _positions.Add(PieceColour.White, new Position[3]);
            _positions.Add(PieceColour.Black, new Position[3]);

            _positions[PieceColour.White][KING_POSITION] = new Position("e1");
            _positions[PieceColour.White][KINGS_ROOK_POSITION] = new Position("h1");
            _positions[PieceColour.White][QUEENS_ROOK_POSITION] = new Position("a1");

            _positions[PieceColour.Black][KING_POSITION] = new Position("e8");
            _positions[PieceColour.Black][KINGS_ROOK_POSITION] = new Position("h8");
            _positions[PieceColour.Black][QUEENS_ROOK_POSITION] = new Position("a8");
        }


        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            if (!PiecePositionCheck(move, board))
            {
                return false;
            }

            if (move.InitialPosition.File > move.FinalPosition.File)
            {
                for (int i = move.FinalPosition.File; i <= move.InitialPosition.File; i++)
                {
                    if (WouldBeInCheckAt(new Position(move.InitialPosition.Rank, i), move.Mover, board, moves))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = move.InitialPosition.File; i <= move.FinalPosition.File; i++)
                {
                    if (WouldBeInCheckAt(new Position(move.InitialPosition.Rank, i), move.Mover, board, moves))
                    {
                        return false;
                    }
                }
            }

            //if (WouldBeInCheckAt(move.InitialPosition, move.Mover, board, moves))
            //{
            //    return false;
            //}
            //if (WouldBeInCheckAt(move.FinalPosition, move.Mover, board, moves))
            //{
            //    return false;
            //}
            return true;
        }

        private bool WouldBeInCheckAt(Position position, PieceColour mover, IBoard board, IList<Move> moves)
        {
            PieceColour oppositeColour = mover == PieceColour.White ? PieceColour.Black : PieceColour.White;
            IEnumerable<Tuple<Piece, Position>> pieces = board.GetPieces(oppositeColour);
            return pieces.Any(piecePosition => _rulesEngineForChecks.IsMoveLegal(new Move(piecePosition.Item2, position, oppositeColour, null, null), board, moves));
        }

        private bool PiecePositionCheck(Move move, IBoard board)
        {
            Position[] positions = _positions[move.Mover];
            if (!board.IsPieceAt(positions[KING_POSITION]))
            {
                return false;
            }

            Piece king = board.GetPieceAt(positions[KING_POSITION]);
            if (king.PieceType != PieceType.King || king.HasMoved)
            {
                return false;
            }

            if (move.FinalPosition.File > move.InitialPosition.File)
            {
                if (!board.IsPieceAt(positions[KINGS_ROOK_POSITION]))
                {
                    return false;
                }
                for (int i = positions[KING_POSITION].File + 1; i < positions[KINGS_ROOK_POSITION].File; i++)
                {
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank, i)))
                    {
                        return false;
                    }
                }
                Piece kingsRook = board.GetPieceAt(positions[KINGS_ROOK_POSITION]);
                return kingsRook.PieceType == PieceType.Rook && !kingsRook.HasMoved;
            }

            if (!board.IsPieceAt(positions[QUEENS_ROOK_POSITION]))
            {
                return false;
            }
            for (int i = positions[QUEENS_ROOK_POSITION].File + 1; i < positions[KING_POSITION].File; i++)
            {
                if (board.IsPieceAt(new Position(move.InitialPosition.Rank, i)))
                {
                    return false;
                }
            }

            Piece queensRook = board.GetPieceAt(positions[QUEENS_ROOK_POSITION]);
            return queensRook.PieceType == PieceType.Rook && !queensRook.HasMoved;
        }
    }
}