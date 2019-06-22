using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Winform
{

    static class Machine
    {
        public struct Move
        {
            public List<Piece> path;
            public int score;
        }
        private static readonly int initialDepth = 3;

        public static List<Tile> ChooseMove(Piece[,] board, bool isWhite)
        {
            //AILatency();
            return Checkers.CastPieceListAsTileList(MinMax(initialDepth, isWhite, true, Checkers.CopyBoard(board)).path);
        }
        private static void AILatency()
        {
            Random r = new Random();
            int latency = r.Next(10) * 100;
            latency = latency > 700 ? latency + 200 : latency - 200;
            latency += 200;
            Thread.Sleep(latency);
        }
        public static Move MinMax(int depth, bool whiteTurn, bool maximizing, Piece[,] simulation)
        {
            string[] boardUI = Test.RuntimeBoardUI(simulation);
            List<List<Piece>> possibleMoves = Checkers.GetAllMoves(simulation, whiteTurn);
            Move bestMove = new Move() { score = maximizing ? -15 : 15 };
            if (depth == 0 || Checkers.IsGameOver(simulation, !whiteTurn))
            {
                bestMove.score = EvaluateMove(whiteTurn, maximizing, simulation);
            }
            else
            {
                if (possibleMoves.Count > 0) bestMove.path = possibleMoves[0];
                foreach (List<Piece> thisMove in possibleMoves)
                {
                    string uiMove = Test.GetMoveUI(thisMove);
                    Piece[,] amendedBoard = Checkers.AmendBoard(simulation, Checkers.CastPieceListAsTileList(thisMove));
                    string[] visualizeBoard = Test.RuntimeBoardUI(simulation);
                    int score = MinMax(depth - 1, !whiteTurn, !maximizing, Checkers.CopyBoard(amendedBoard)).score;
                    if (maximizing && score > bestMove.score)
                    {
                        bestMove.score = score;
                        bestMove.path = thisMove;
                    }
                    else if (!maximizing && score < bestMove.score)
                    {
                        bestMove.score = score;
                        bestMove.path = thisMove;
                    }
                }
            }
            return bestMove;
        }
        private static int EvaluateMove(bool whiteTurn, bool maximizing, Piece[,] simulation)
        {
            _ = Test.RuntimeBoardUI(simulation);
            int score;
            if (Checkers.IsGameOver(simulation, !whiteTurn))
            {
                score = maximizing ? 15 : -15;
            }
            else
            {
                bool thisPlayer = maximizing ? whiteTurn : !whiteTurn;
                int playerPieces = Checkers.GetPlayerPieces(thisPlayer, simulation).Count;
                int opponentPieces = Checkers.GetPlayerPieces(!thisPlayer, simulation).Count;
                score = playerPieces - opponentPieces;
            }
            return score;
        }
    }
}
