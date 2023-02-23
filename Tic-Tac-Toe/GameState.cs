using System;

namespace Tic_Tac_Toe
{
    public class GameState
    {
        public GameState()
        {
            GameGrid = new Player[3, 3];
            CurrentPlayer = Player.X;
            TurnsPassed = 0;
            GameOver = false;
        }

        public Player[,] GameGrid { get; private set; }

        public Player CurrentPlayer { get; private set; }

        public int TurnsPassed { get; private set; }

        public bool GameOver { get; private set; }

        public event Action<int, int> MoveMade;

        public event Action<GameResult> GameEnded;

        public event Action GameRestarted;

        private bool CanMakeMove(int row, int col)
        {
            return !GameOver && GameGrid[row, col] == Player.None;
        }

        private bool IsGridFull()
        {
            return TurnsPassed == 9;
        }

        private void SwitchPlayer()
        {
            if (CurrentPlayer == Player.X)
            {
                CurrentPlayer = Player.O;
            }
            else
            {
                CurrentPlayer = Player.X;
            }
        }

        private bool AreSquaresMarked((int, int)[] squares, Player player)
        {
            foreach ((int row, int col) in squares)
            {
                if (GameGrid[row, col] != player)
                {
                    return false;
                }
            }

            return true;
        }

        private bool DidMoveWin(int r, int c, out WinInfo winInfo)
        {
            (int, int)[] row = new[] {(r, 0), (r, 1), (r, 2)};
            (int, int)[] col = new[] { (0, c), (1, c), (2, c) };
            (int, int)[] mainDiagonal = new[] {(0, 0), (1, 1), (2, 2) };
            (int, int)[] antiDiagonal = new[] { (0, 2), (1, 1), (2, 0) };

            if (AreSquaresMarked(row, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.Row, Number = r };
                return true;
            }

            if (AreSquaresMarked(col, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.Column, Number = c };
                return true;
            }

            if (AreSquaresMarked(mainDiagonal, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.MainDiagonal };
                return true;
            }

            if (AreSquaresMarked(antiDiagonal, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.Antidiagonal };
                return true;
            }

            winInfo = null;
            return false;
        }

        private bool DidMoveEndTheGame(int r, int c, out GameResult gameResult)
        {
            if (DidMoveWin())
            {

            }
        }
    }
}
