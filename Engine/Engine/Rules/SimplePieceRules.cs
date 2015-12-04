using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class SimplePieceRules : IRulesEngine
    {
        private readonly Dictionary<PieceType, List<IMoveRule>> _moveLegality;

        public SimplePieceRules()
        {
            _moveLegality = new Dictionary<PieceType, List<IMoveRule>>
            {
                {
                    PieceType.Bishop, new List<IMoveRule> {new BishopMoveRule()}
                },
                {PieceType.King, new List<IMoveRule> {new KingMoveRule()}},
                {PieceType.Knight, new List<IMoveRule> {new KnightMoveRule()}},
                {PieceType.Queen, new List<IMoveRule> {new QueenMoveRule()}},
                {PieceType.Rook, new List<IMoveRule> {new RookMoveRule()}},
                {PieceType.Pawn, new List<IMoveRule> {new PawnMoveRule()}}
            };

            foreach (var ruleList in _moveLegality.Values)
            {
                ruleList.Add(new CantTakeOwnPieceRule());
            }
        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            var pieceToMove = board.GetPieceAt(move.InitialPosition);
            return pieceToMove != null && _moveLegality[pieceToMove.PieceType].All(rule => rule.IsMoveLegal(move, board, moves));
        }
    }
}