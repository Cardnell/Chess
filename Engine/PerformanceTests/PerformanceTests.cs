using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Cardnell.Chess.Engine;
using NUnit.Framework;
using NUnit.Framework.Compatibility;

namespace PerformanceTests
{

    public class PerformanceTests
    {
        private static log4net.ILog _log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static int _numberOfMoves;
        private static object thisLock = new object();
        public static void Main(string[] args)

        {
            log4net.Config.XmlConfigurator.Configure();
            int numberOfLevels = 5;
            string runType = "Single thread, no piece based 'GetMoves'";
            _log.Info($"Starting peformance run for {numberOfLevels} levels");
            var watch = new Stopwatch();
            try
            {
                Game game = Game.ClassicalGame();
                watch.Start();
                FirstIteration(game, numberOfLevels);
                watch.Stop();
            }
            catch (Exception e)
            { 
                watch.Stop();
                _log.Error($"Performance run {runType} ended after {watch.Elapsed.ToString("g")} with error: {e.Message}, stacktrace: {e.StackTrace}");
            }
            _log.Info($"Performance run {runType} finished after {watch.Elapsed.ToString("g")}, total moves: {_numberOfMoves}");
        }

        private static void FirstIteration(Game game, int totalLevels)
        {
            IList<Move> moves = game.GetPossibleMoves();
            foreach (Move newMove in moves)
            {
                IterateMoves(game, newMove, 1, totalLevels);
            }
        }

        private static void IterateMoves(Game game, Move move, int level, int totalLevels)
        {
            if (level >= totalLevels)
            {
                return;
            }
            Game newGame = game.Copy();
            level++;
            newGame.MakeMove(move);
            IList<Move> moves = newGame.GetPossibleMoves();
            SequentialRun(level, totalLevels, moves, newGame);
        }

        private static void SequentialRun(int level, int totalLevels, IList<Move> moves, Game newGame)
        {
            foreach (Move newMove in moves)
            {
                _numberOfMoves++;
                IterateMoves(newGame, newMove, level, totalLevels);

            }
        }

        private static void ParallelRun(int level, int totalLevels, IList<Move> moves, Game newGame)
        {
            Parallel.ForEach(moves,
                newMove =>
                {
                    lock (thisLock)
                    {
                        _numberOfMoves++;
                    }
                    IterateMoves(newGame, newMove, level, totalLevels);
                });
        }
    }
}
