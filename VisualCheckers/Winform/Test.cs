using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winform
{
    public static class Test
    {
        private enum Case
        {
            blackBlocked, blackGetsEaten, doubleEating
        }
        private static void TestCase(out List<Tile> whitePieces, out List<Tile> blackPieces, Case testInstance)
        {
            switch (testInstance)
            {
                case Case.blackBlocked:
                    whitePieces = new List<Tile>() { Checkers.CreateTile(4, 5), Checkers.CreateTile(5,6), Checkers.CreateTile(4,3) };
                    blackPieces = new List<Tile>() { Checkers.CreateTile(6,7), Checkers.CreateTile(7,4) };
                    break;
                case Case.blackGetsEaten:
                    whitePieces = new List<Tile>() { Checkers.CreateTile(3, 4), Checkers.CreateTile(3, 2), Checkers.CreateTile(2, 5) };
                    blackPieces = new List<Tile>() { Checkers.CreateTile(5, 2), Checkers.CreateTile(5, 4) };
                    break;
                case Case.doubleEating:
                    whitePieces = new List<Tile>() { Checkers.CreateTile(3, 4) };
                    blackPieces = new List<Tile>() { Checkers.CreateTile(4, 3), Checkers.CreateTile(6, 3) };
                    break;
                default:
                    whitePieces = null;
                    blackPieces = null;
                    break;

            }
        }
        public static Piece[,] FabricateBoard()
        {
            Piece[,] mockBoard = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        mockBoard[i, j] = new Empty(i, j);
                    }
                }
            }
            TestCase(out List<Tile> whitePieces, out List<Tile> blackPieces, Case.blackBlocked);
            foreach (Tile piece in whitePieces)
            {
                if ((piece.x + piece.y) % 2 == 0)
                {
                    throw new Exception("Illegal piece position");
                }
                mockBoard[piece.x, piece.y] = new Checker(true, piece.x, piece.y);
            }
            foreach (Tile piece in blackPieces)
            {
                if ((piece.x + piece.y) % 2 == 0)
                {
                    throw new Exception("Illegal piece position");
                }
                mockBoard[piece.x, piece.y] = new Checker(false, piece.x, piece.y);
            }
            return mockBoard;
        }
        public static string[] RuntimeBoardUI(Piece[,] givenBoard)
        {
            string[] boardUI = new string[8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = givenBoard[i, j];
                    if (piece is null)
                    {
                        boardUI[i] += '.';
                    }
                    else if (piece is Empty)
                    {
                        boardUI[i] += '-';
                    }
                    else if (piece.isWhite)
                    {
                        boardUI[i] += 'o';
                    }
                    else
                    {
                        boardUI[i] += 'x';
                    }
                }
            }
            return boardUI;
        }
        public static string GetMoveUI(List<Piece> move)
        {
            string output = "";
            foreach (Piece p in move)
            {
                output += $"|{p.x},{p.y} ";
            }
            return output;
        }
    }
}
