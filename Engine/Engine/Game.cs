using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cardnell.Chess.Engine.Rules;

namespace Cardnell.Chess.Engine
{
    public class Game
    {
        public IBoard Board { get; private set; }
        
        
        private List<Move> _moves;
        private readonly IRulesEngine _rulesEngine;

        public Game(IBoard board, IRulesEngine rulesEngine)
        {
            Board = board;
            _rulesEngine = rulesEngine;
            _moves = new List<Move>();
        }

        public Game(IBoard board, IRulesEngine rulesEngine, List<Tuple<Piece, Position>> pieces )
        {
            Board = board;
            _rulesEngine = rulesEngine;
            _moves = new List<Move>();

        }

        public bool IsMoveLegal(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            Piece piece = Board.GetPieceAt(initialPosition);
            if(piece == null)
            {
                return false;
            }
            if (piece.Colour != mover)
            {
                return false;
            }
            return _rulesEngine.IsMoveLegal(new Move(initialPosition, finalPosition, mover, piece, null), Board, _moves);
        }

        public bool IsMoveLegal(Move move)
        {
            return false;
        }

        public void MakeMove(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            if (!IsMoveLegal(initialPosition, finalPosition, mover))
            {
                throw new ArgumentException("Move not legal");
            }
            Board.MovePiece(new Move(initialPosition, finalPosition, mover, null, null));
        }
    }
}
