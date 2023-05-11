using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Minigames.Models
{
    public partial class MinesweeperWindow
    {
        private Grid? _gameGrid;
        private Button[,]? _buttons;
        private int _rows, _columns, _mines, _flags, _revealedCells;
        private bool _gameOver;

        public MinesweeperWindow()
        {
            InitializeComponent();
        }

        private void BeginnerButtonClick(object sender, RoutedEventArgs e)
        {
            StartGame(10, 10, 10);
        }

        private void IntermediateButtonClick(object sender, RoutedEventArgs e)
        {
            StartGame(16, 16, 40);
        }

        private void ExpertButtonClick(object sender, RoutedEventArgs e)
        {
            StartGame(16, 30, 99);
        }

        private void StartGame(int rows, int columns, int mines)
        {
            _gameOver = false;
            _rows = rows;
            _columns = columns;
            _mines = mines;
            _flags = 0;
            _revealedCells = 0;

            _gameGrid = new Grid();

            for (var i = 0; i < rows; i++)
            {
                _gameGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (var i = 0; i < columns; i++)
            {
                _gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            _buttons = new Button[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var button = new Button
                    {
                        Tag = new Cell { Row = i, Column = j, IsMine = false, IsFlagged = false, IsRevealed = false },
                        Content = "",
                        FontWeight = FontWeights.Bold,
                        FontSize = 12,
                        Margin = new Thickness(1)
                    };

                    button.Click += ButtonClick;
                    button.MouseRightButtonDown += ButtonRightClick;

                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    _gameGrid.Children.Add(button);
                    _buttons[i, j] = button;
                }
            }

            PlaceMines(mines);
            MinesweeperGrid.Content = _gameGrid;
        }

        private void PlaceMines(int mines)
        {
            var random = new Random();

            for (var i = 0; i < mines; i++)
            {
                int row, column;

                do
                {
                    row = random.Next(0, _rows);
                    column = random.Next(0, _columns);
                } while (((Cell)_buttons![row, column].Tag).IsMine);

                ((Cell)_buttons[row, column].Tag).IsMine = true;
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (_gameOver) return;

            var button = (Button)sender;
            var cell = (Cell)button.Tag;

            if (cell.IsFlagged || cell.IsRevealed) return;

            if (cell.IsMine)
            {
                button.Content = "X";
                button.Background = Brushes.Red;
                GameOver(false);
                return;
            }

            RevealCell(cell.Row, cell.Column);
            CheckWin();
        }

        private void ButtonRightClick(object sender, MouseButtonEventArgs e)
        {
            if (_gameOver) return;

            var button = (Button)sender;
            var cell = (Cell)button.Tag;

            if (cell.IsRevealed) return;

            if (cell.IsFlagged)
            {
                button.Content = "";
                button.Background = Brushes.LightGray;
                _flags--;
            }
            else
            {
                button.Content = "F";
                button.Background = Brushes.Coral;
                _flags++;
            }

            cell.IsFlagged = !cell.IsFlagged;
            FlagsLabel.Content = $"Flags: {_flags}";
        }

        private void RevealCell(int row, int column)
        {
            if (row < 0 || row >= _rows || column < 0 || column >= _columns) return;

            var button = _buttons![row, column];
            var cell = (Cell)button.Tag;

            if (cell.IsRevealed || cell.IsFlagged) return;

            cell.IsRevealed = true;
            _revealedCells++;

            var adjacentMines = CountAdjacentMines(row, column);

            if (adjacentMines > 0)
            {
                button.Content = adjacentMines.ToString();
                // button.Background = Brushes.LightGray;
                button.Background = Brushes.Bisque;
            }
            else
            {
                button.Background = Brushes.WhiteSmoke;
                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        RevealCell(row + i, column + j);
                    }
                }
            }
        }

        private int CountAdjacentMines(int row, int column)
        {
            var count = 0;

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var newRow = row + i;
                    var newColumn = column + j;

                    if (newRow < 0 || newRow >= _rows || newColumn < 0 || newColumn >= _columns) continue;

                    if (((Cell)_buttons![newRow, newColumn].Tag).IsMine) count++;
                }
            }

            return count;
        }

        private void CheckWin()
        {
            if (_revealedCells == (_rows * _columns) - _mines)
            {
                GameOver(true);
            }
        }

        private void GameOver(bool won)
        {
            _gameOver = true;
            MessageBox.Show(won ? "Вы выиграли!" : "Вы проиграли!");
            StartGame(_rows, _columns, _mines);
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private class Cell
        {
            public int Row { get; init; }
            public int Column { get; init; }
            public bool IsMine { get; set; }
            public bool IsFlagged { get; set; }
            public bool IsRevealed { get; set; }
        }
    }
}
