using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform
{
    public abstract class Piece
    {
        public enum MoveState
        {
            start, hasEaten, hasMoved
        }
        protected MoveState moveState;
        public MoveState GetMoveState
        {
            get => moveState;
        }
        public void ResetMoveState()
        {
            moveState = MoveState.start;
        }
        public readonly bool isWhite;
        protected bool moveIsLegal;
        protected int xAdvance, yAdvance;
        protected List<Piece> path;
        protected int xFinal;
        public readonly int x;
        public readonly int y;
        public Piece(bool isWhite, int x, int y)
        {
            this.isWhite = isWhite;
            this.x = x;
            this.y = y;
            moveState = MoveState.start;
            xFinal = -1;
        }
        public virtual bool IsMoveLegal(Tile fromTile, Tile toTile)
        {
            xAdvance = toTile.x - fromTile.x;
            yAdvance = Math.Abs(toTile.y - fromTile.y);
            return Math.Abs(xAdvance) == yAdvance;
        }
        public abstract bool IsPathLegal(List<Piece> piecesInBetween);
        public int XFinal
        {
            get => xFinal;
        }
    }
    public class Checker : Piece
    {
        readonly int xDirection;
        public Checker(bool isWhite, int x, int y) : base(isWhite, x, y)
        {
            xDirection = isWhite ? 1 : -1;
            xFinal = isWhite ? 7 : 0;
        }
        public override bool IsMoveLegal(Tile fromTile, Tile toTile)
        {
            moveIsLegal = false;
            if (moveState != MoveState.hasMoved && base.IsMoveLegal(fromTile, toTile))
            {
                if (moveState == MoveState.start)
                {
                    if (yAdvance == 1 && toTile.x - fromTile.x == xDirection)
                    {
                        moveIsLegal = true;
                        moveState = MoveState.hasMoved;
                    }
                    if (yAdvance == 2 && xAdvance == xDirection * 2)
                    {
                        moveIsLegal = true;
                        moveState = MoveState.hasEaten;
                    }
                }
                else
                {
                    if (yAdvance == 2 && Math.Abs(xAdvance) == 2)
                        moveIsLegal = true;
                }
            }
            return moveIsLegal;
        }
        public override bool IsPathLegal(List<Piece> piecesInBetween)
        {
            bool pathIslegal = false;
            if (moveState == MoveState.hasEaten)
            {
                if (piecesInBetween.Count == 1)
                {
                    pathIslegal = piecesInBetween[0].isWhite != this.isWhite;
                }
            }
            if (moveState == MoveState.hasMoved)
                pathIslegal = piecesInBetween.Count == 0;
            return pathIslegal;
        }
    }
    public class Queen : Piece
    {
        public Queen(bool isWhite, int x, int y) : base(isWhite, x, y) { }

        public override bool IsMoveLegal(Tile fromTile, Tile toTile)
        {
            return base.IsMoveLegal(fromTile, toTile);
        }
        public override bool IsPathLegal(List<Piece> piecesInBetween)
        {
            bool pathIsLegal = false;
            if (moveState != MoveState.hasMoved)
            {
                if (piecesInBetween.Count == 0)
                {
                    pathIsLegal = moveState == MoveState.start;
                    moveState = MoveState.hasMoved;
                }
                if (piecesInBetween.Count == 1)
                {
                    pathIsLegal = piecesInBetween[0].isWhite != this.isWhite;
                    moveState = MoveState.hasEaten;
                }
            }
            return pathIsLegal;
        }
    }
    public class Empty : Piece
    {
        public Empty(int x, int y) : base(false, x, y) { }
        public override bool IsPathLegal(List<Piece> path) { return false; }
    }
}
