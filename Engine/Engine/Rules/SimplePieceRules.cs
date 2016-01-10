using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class SimplePieceRules : IRulesEngine
    {
        private readonly Dictionary<PieceType, List<IPieceMoveDeterminer>> _moveLegality;

        public SimplePieceRules()
        {
            _moveLegality = new Dictionary<PieceType, List<IPieceMoveDeterminer>>
            {
                {
                    PieceType.Bishop, new List<IPieceMoveDeterminer> {new BishopMoveLegalityRuleChecker()}
                },
                {PieceType.King, new List<IPieceMoveDeterminer> {new KingMoveLegalityRuleChecker()}},
                {PieceType.Knight, new List<IPieceMoveDeterminer> {new KnightMoveLegalityRuleChecker()}},
                {PieceType.Queen, new List<IPieceMoveDeterminer> {new QueenMoveLegalityRuleChecker()}},
                {PieceType.Rook, new List<IPieceMoveDeterminer> {new RookMoveLegalityRuleChecker()}},
                {PieceType.Pawn, new List<IPieceMoveDeterminer> {new PawnMoveLegalityRuleChecker()}}
            };

        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            var pieceToMove = board.GetPieceAt(move.InitialPosition);
            return pieceToMove != null && _moveLegality[pieceToMove.PieceType].All(rule => rule.IsMoveLegal(move, board, moves));
        }

        public IList<Move> GetLegalMoves(Position startingPositiong, IBoard board, IList<Move> moves)
        {
            var pieceToMove = board.GetPieceAt(startingPositiong);
            var output = new List<Move>();
            if (pieceToMove != null)
            {
                foreach (
                    IList<Move> theMoves in
                        _moveLegality[pieceToMove.PieceType].Select(
                            x => x.GetLegalMoves(startingPositiong, pieceToMove.Colour, board, moves)))
                {
                    output.AddRange(theMoves);
                }
            }
            return output;
        }
    }
}