using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Winform
{
    public partial class Form1 : Form
    {
        private readonly Checkers checkers;
        private PictureBox[,] UIBoard;
        private readonly Color marked = Color.Green;
        private readonly Thread b, c;

        public Form1()
        {
            InitializeComponent();
            checkers = new Checkers();
            b = new Thread(checkers.Run) { IsBackground = true };
            c = new Thread(AmendUIBoard) { IsBackground = true };
            b.Start();
            c.Start();
            InitializeUI();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void InitializeUI()
        {
            const int tileSize = 50;
            const int gridSize = 8;
            UIBoard = new PictureBox[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    PictureBox newTile = new PictureBox
                    {
                        Size = new Size(tileSize, tileSize),
                        Location = new Point(tileSize * j, tileSize * i)
                    };
                    Controls.Add(newTile);
                    if ((i + j) % 2 == 1)
                    {
                        newTile.Click += new EventHandler(Board_Click);
                    }
                    FillColor(newTile, i, j);
                    newTile.Name = $"T{i}{j}";
                    UIBoard[i, j] = newTile;
                }
            }
            checkers.amendBoardListener.Set();
            if (!checkers.testMode) Clutch.Dispose();
        }
        private void Commit_Click(object sender, EventArgs e)
        {
            checkers.playerCommitMove.Set();
        }
        private void AmendUIBoard()
        {
            while (true)
            {
                checkers.amendBoardListener.WaitOne();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        FillColor(UIBoard[i, j], i, j);
                        InputPiece(UIBoard[i, j], checkers.GetPiece(i, j));
                    }
                }
            }
        }
        private void Board_Click(object sender, EventArgs e)
        {
            lock (checkers.playerInputLock)
            {
                PictureBox box = sender as PictureBox;
                Tile boxTile = Checkers.CreateTile(int.Parse(box.Name[1].ToString()), int.Parse(box.Name[2].ToString()));
                List<Tile> unclickedTiles = new List<Tile>();
                if (box.BackColor == marked)
                {
                    unclickedTiles = checkers.Unclick(boxTile);
                    foreach (Tile tile in unclickedTiles)
                    {
                        FillColor(UIBoard[tile.x, tile.y], tile.x, tile.y);
                    }
                }
                else
                {
                    if (checkers.CanClick(boxTile))
                    {
                        box.BackColor = marked;
                    }
                }
            }
        }
        private void FillColor(PictureBox tile, int x, int y)
        {
            tile.BackColor = (x + y) % 2 == 0 ? Color.Beige : Color.DarkRed;
        }

        private void Clutch_Click(object sender, EventArgs e)
        {
            Checkers.breakpointsActive = !Checkers.breakpointsActive;
        }

        private void InputPiece(PictureBox box, Piece piece)
        {
            string path = @"C:\Users\Ethan\source\repos\VisualCheckers\Winform\assets\";
            string file = "";

            if (piece is Checker)
            {
                file = piece.isWhite ? "WhiteChecker" : "BlackChecker";
            }
            if (piece is Queen)
            {
                file = piece.isWhite ? "WhiteQueen" : "BlackQueen";
            }
            box.Image = piece is Empty | piece is null ? null : Image.FromFile(path + file + ".png");
        }
    }
}