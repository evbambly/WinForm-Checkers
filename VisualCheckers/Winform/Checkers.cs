using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Winform
{
    public struct Tile
    {
        public int x;
        public int y;
        public static bool operator ==(Tile a, Tile b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Tile a, Tile b)
        {
            return a.x != b.x || a.y != b.y;
        }
    }
    public class Checkers
    {
        private Piece[,] board = new Piece[8, 8];
        private List<Tile> moveTiles = new List<Tile>();
        private readonly bool whiteTurnIsAI = false;
        private readonly bool blackTurnIsAI = true;
        public readonly object playerInputLock = new object();
        public AutoResetEvent playerCommitMove = new AutoResetEvent(false);
        public AutoResetEvent amendBoardListener = new AutoResetEvent(false);
        public readonly bool testMode = false;
        public static bool breakpointsActive = false;
        public Checkers()
        {
            SetBoard();
            board = testMode ? Test.FabricateBoard() : board;
        }
        private void SetBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        if (i > 4)
                        {
                            board[i, j] = new Checker(false, i, j);
                        }
                        else if (i < 3)
                        {
                            board[i, j] = new Checker(true, i, j);
                        }
                        else
                        {
                            board[i, j] = new Empty(i, j);
                        }
                    }
                }
            }
            WhiteTurn = true;
        }
        public void Run()
        {
            bool gameOver = false;
            while (!gameOver)
            {
                if (breakpointsActive)
                {

                }
                if ((whiteTurnIsAI && WhiteTurn) || (blackTurnIsAI && !WhiteTurn))
                    lock (playerInputLock)
                    {
                        moveTiles = Machine.ChooseMove(board, WhiteTurn);
                    }
                else
                    playerCommitMove.WaitOne();
                lock (playerInputLock)
                {
                    if (!(moveTiles is null) && moveTiles.Count > 1)
                    {
                        board = AmendBoard(board, moveTiles);
                        amendBoardListener.Set();
                        WhiteTurn = !WhiteTurn;
                        moveTiles.Clear();
                        gameOver = IsGameOver(board, WhiteTurn);
                    }
                }
            }
            Form2 winForm = new Form2(!WhiteTurn);
            Application.Run(winForm);
            while (!(winForm is null))
            {
                if (winForm.Reset)
                {
                    SetBoard();
                    amendBoardListener.Set();
                    Run();
                }
            }
        }
        private static bool IsMovePossible(List<Tile> moveTiles, Piece[,] givenBoard, bool whiteTurn)
        {
            Piece[,] amendedBoard = CopyBoard(givenBoard);
            bool moveIsPossible = true;
            Piece chosenPiece = amendedBoard[moveTiles[0].x, moveTiles[0].y];
            if (moveTiles.Count > 1)
            {
                for (int i = 0; i < moveTiles.Count - 1 && moveIsPossible; i++)
                {
                    moveIsPossible = false;
                    Tile fromTile = moveTiles[i];
                    Tile toTile = moveTiles[i + 1];
                    if (IsMoveSane(fromTile, toTile, amendedBoard, whiteTurn))
                    {
                        if (chosenPiece.IsMoveLegal(fromTile, toTile))
                        {
                            List<Piece> piecesInBetween = GetPiecesInBetween(fromTile, toTile, amendedBoard);
                            moveIsPossible = chosenPiece.IsPathLegal(piecesInBetween);
                            if (chosenPiece.GetMoveState == Piece.MoveState.hasEaten)
                            {
                                amendedBoard = AmendBoard(amendedBoard, new List<Tile>() { fromTile, toTile });
                            }
                        }
                    }
                }
            }
            chosenPiece.ResetMoveState();
            return moveIsPossible;
        }
        private static bool IsMoveSane(Tile fromTile, Tile toTile, Piece[,] givenBoard, bool whiteTurn)
        {
            Piece.MoveState pieceState = givenBoard[fromTile.x, fromTile.y].GetMoveState;
            bool moveIsSane = pieceState != Piece.MoveState.hasMoved;
            moveIsSane = moveIsSane && givenBoard[fromTile.x, fromTile.y].isWhite == whiteTurn;
            moveIsSane = moveIsSane && givenBoard[toTile.x, toTile.y] is Empty;
            return moveIsSane;
        }
        private static List<Piece> GetPiecesInBetween(Tile fromTile, Tile toTile, Piece[,] givenBoard)
        {
            List<Piece> piecesInBetween = new List<Piece>();
            int xDirection = toTile.x > fromTile.x ? 1 : -1;
            int yDirection = toTile.y > fromTile.y ? 1 : -1;
            if (Math.Abs(toTile.x - fromTile.x) == Math.Abs(toTile.y - fromTile.y))
            {
                Tile pathTile = CreateTile(fromTile.x, fromTile.y);
                for (int i = 0; i < Math.Abs(toTile.x - fromTile.x) - 1; i++)
                {
                    pathTile.x += xDirection;
                    pathTile.y += yDirection;
                    if (!(givenBoard[pathTile.x, pathTile.y] is Empty))
                    {
                        piecesInBetween.Add(givenBoard[pathTile.x, pathTile.y]);
                    }
                }
            }
            return piecesInBetween;
        }
        public static Piece[,] AmendBoard(Piece[,] givenBoard, List<Tile> moveTiles)
        {
            Piece[,] amendedBoard = CopyBoard(givenBoard);
            for (int i = 0; i < moveTiles.Count - 1; i++)
            {
                List<Piece> removePieces = GetPiecesInBetween(moveTiles[i], moveTiles[i + 1], amendedBoard);
                foreach (Piece remove in removePieces)
                {
                    amendedBoard[remove.x, remove.y] = new Empty(remove.x, remove.y);
                }
            }
            Tile prev = moveTiles[0];
            Tile next = moveTiles[moveTiles.Count - 1];
            Piece from = amendedBoard[prev.x, prev.y];
            if (next.x == from.XFinal || from is Queen)
            {
                amendedBoard[next.x, next.y] = new Queen(from.isWhite, next.x, next.y);
            }
            else
            {
                amendedBoard[next.x, next.y] = new Checker(from.isWhite, next.x, next.y);
            }
            amendedBoard[prev.x, prev.y] = new Empty(prev.x, prev.y);
            return amendedBoard;
        }
        private static List<List<Piece>> GetSimpleMoves(Piece[,] givenBoard, bool whiteTurn)
        {
            List<List<Piece>> legalMoves = new List<List<Piece>>();
            List<Piece> playerPieces = GetPlayerPieces(whiteTurn, givenBoard);
            if (playerPieces.Count != 0)
            {
                foreach (Piece playerPiece in playerPieces)
                {
                    foreach (Piece square in givenBoard)
                    {
                        if (square != null && square != playerPiece)
                        {
                            Tile player = CreateTile(playerPiece.x, playerPiece.y);
                            Tile tile = CreateTile(square.x, square.y);
                            if (IsMovePossible(new List<Tile>() { player, tile }, givenBoard, whiteTurn))
                            {
                                legalMoves.Add(new List<Piece>() { playerPiece, square });
                            }
                            playerPiece.ResetMoveState();
                        }
                    }
                }
            }
            return legalMoves;
        }
        public static List<List<Piece>> GetAllMoves(Piece[,] givenBoard, bool whiteTurn)
        {
            List<List<Piece>> legalMoves = GetSimpleMoves(givenBoard, whiteTurn);
            int i = 0;
            while (i < legalMoves.Count)
            {
                foreach (Piece square in givenBoard)
                {
                    if (square != null)
                    {
                        List<Tile> thisMove = CastPieceListAsTileList(legalMoves[i]);
                        thisMove.Add(CreateTile(square.x, square.y));
                        if (IsMovePossible(thisMove, givenBoard, whiteTurn))
                        {
                            legalMoves.Add(legalMoves[i]);
                            legalMoves[legalMoves.Count - 1].Add(square);
                        }
                        legalMoves[i][0].ResetMoveState();
                    }
                }
                i++;
            }
            return legalMoves;
        }
        public bool CanClick(Tile picBox)
        {
            bool tileIsClickable = false;
            if (moveTiles.Count == 0)
            {
                if (board[picBox.x, picBox.y] is Checker || board[picBox.x, picBox.y] is Queen)
                {
                    if (board[picBox.x, picBox.y].isWhite == WhiteTurn)
                    {
                        moveTiles.Add(picBox);
                        tileIsClickable = true;
                    }
                }
            }
            else
            {
                moveTiles.Add(picBox);
                if (IsMovePossible(moveTiles, board, WhiteTurn))
                {
                    tileIsClickable = true;
                }
                else
                {
                    moveTiles.Remove(picBox);
                }
            }
            return tileIsClickable;
        }
        public List<Tile> Unclick(Tile picBox)
        {
            List<Tile> unclickedTiles = new List<Tile>();
            bool removeTiles = false;
            for (int i = 0; i < moveTiles.Count; i++)
            {
                if (moveTiles[i] == picBox)
                {
                    removeTiles = true;
                }
                if (removeTiles)
                {
                    unclickedTiles.Add(moveTiles[i]);
                }
            }
            foreach (Tile tile in unclickedTiles)
            {
                moveTiles.Remove(tile);
            }
            return unclickedTiles;
        }
        #region AssistingMethods
        public static List<Piece> GetPlayerPieces(bool whiteTurn, Piece[,] givenBoard)
        {
            List<Piece> getPieces = new List<Piece>();
            foreach (Piece piece in givenBoard)
            {
                if (piece != null && !(piece is Empty) && piece.isWhite == whiteTurn)
                {
                    getPieces.Add(piece);
                }
            }
            return getPieces;
        }
        public static Tile CreateTile(int x, int y)
        {
            Tile tile;
            tile.x = x;
            tile.y = y;
            return tile;
        }
        public bool WhiteTurn { get; private set; }
        public static List<Tile> CastPieceListAsTileList(List<Piece> givenList)
        {
            List<Tile> castList = null;
            if (givenList is null) { }
            else
            {
                castList = new List<Tile>();
                foreach (Piece p in givenList)
                {
                    castList.Add(CreateTile(p.x, p.y));
                }
            }
            return castList;
        }
        public static Piece[,] CopyBoard(Piece[,] givenBoard)
        {
            Piece[,] copy = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    copy[i, j] = givenBoard[i, j];
                }
            }
            return copy;
        }
        public static bool IsGameOver(Piece[,] givenBoard, bool whiteTurn)
        {
            return !GetSimpleMoves(givenBoard, whiteTurn).Any();
        }
        public Piece GetPiece(int x, int y)
        {
            return board[x, y];
        }
        #endregion
    }
}