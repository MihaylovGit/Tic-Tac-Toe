﻿using System;

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
    }
}