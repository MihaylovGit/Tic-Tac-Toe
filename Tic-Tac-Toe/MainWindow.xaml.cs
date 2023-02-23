﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tic_Tac_Toe
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<Player, ImageSource> imageSources = new()
        {
            { Player.X, new BitmapImage(new Uri("pack://application:,,,/Assets/X15.png")) },
            { Player.O, new BitmapImage(new Uri("pack://application:,,,/Assets/O15.png")) },
        };

        private readonly Image[,] imageControls = new Image[3, 3];
        private readonly GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            SetupGameGrid();

            gameState.MoveMade += OnMoveMade;
            gameState.GameEnded += OnGameEnded;
            gameState.GameRestarted += OnGameRestarted;
        }

        private void SetupGameGrid() 
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    Image imageControl = new Image();
                    GameGrid.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
        }

        private void TransitionToEndScreen(string text, ImageSource winnerImage)
        {
            TurnPanel.Visibility = Visibility.Hidden;
            GameCanvas.Visibility = Visibility.Hidden;

            ResultText.Text = text;

            WinnerImage.Source = winnerImage;
            EndScreen.Visibility = Visibility.Visible;
        }

        private void TransitionToGameScreen()
        {
            EndScreen.Visibility = Visibility.Hidden;
            Line.Visibility = Visibility.Hidden;
            TurnPanel.Visibility = Visibility.Visible;
            GameCanvas.Visibility = Visibility.Visible;
        }

        private (Point, Point) FindLinePoints(WinInfo winInfo)
        {
            double squareSize = GameGrid.Width / 3;
            double margin = squareSize / 2;

            if (winInfo.Type == WinType.Row)
            {
                double y = winInfo.Number * squareSize + margin;
                return (new Point(0, y), new Point(GameGrid.Width, y));
            }

            if (winInfo.Type == WinType.Column)
            {
                double x = winInfo.Number * squareSize + margin;
                return (new Point(x, 0), new Point(x, GameGrid.Height));
            }

            if (winInfo.Type == WinType.MainDiagonal)
            {
                return (new Point(0, 0), new Point(GameGrid.Width, GameGrid.Height));
            }

            return (new Point(GameGrid.Width, 0), new Point(0, GameGrid.Height));
        }

        private void ShowLine(WinInfo winInfo)
        {
            (Point startPoint, Point endPoint) = FindLinePoints(winInfo);

            Line.X1 = startPoint.X;
            Line.Y1 = startPoint.Y;

            Line.X2 = endPoint.X;
            Line.Y2 = endPoint.Y;

            Line.Visibility = Visibility.Visible;   
        }

        private void OnMoveMade(int r, int c)
        {
            Player player = gameState.GameGrid[r, c];
            imageControls[r, c].Source = imageSources[player];
            PlayerImage.Source = imageSources[gameState.CurrentPlayer];
        }

        private async void OnGameEnded(GameResult gameResult)
        {
            await Task.Delay(1000);

            if (gameResult.Winner == Player.None)
            {
                TransitionToEndScreen("It' a tie!", null);
            }
            else
            {
                ShowLine(gameResult.WinInfo);
                await Task.Delay(1000);
                TransitionToEndScreen("The Winner is:", imageSources[gameResult.Winner]);
            }
        }

        private void OnGameRestarted()
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    imageControls[r, c].Source = null;
                }
            }

            PlayerImage.Source = imageSources[gameState.CurrentPlayer];
            TransitionToGameScreen();
        }

        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double squareSize = GameGrid.Width / 3;
            Point clickPosition = e.GetPosition(GameGrid);
            int row = (int)(clickPosition.Y / squareSize);
            int col = (int)(clickPosition.X / squareSize);

            gameState.MakeMove(row, col);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gameState.Reset();
        }
    }
}
