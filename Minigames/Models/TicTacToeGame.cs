using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Minigames.Models;

public partial class TicTacToeWindow
{
    private const string PlayerSymbol = "X";
    private const string BotSymbol = "O";
    private readonly Button[,] _buttons = new Button[3, 3];

    public TicTacToeWindow()
    {
        InitializeComponent();
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var button = (Button)FindName($"Button{i}{j}")!;
                _buttons[i, j] = button;
                button.Click += Button_Click;
            }
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || !string.IsNullOrEmpty(button.Content as string))
        {
            return;
        }

        button.Content = PlayerSymbol;
        if (CheckWin(PlayerSymbol))
        {
            MessageBox.Show($"Вы выиграли!");
            ResetBoard();
            return;
        }

        if (!IsBoardFull())
        {
            BotMove();
        }
        else
        {
            MessageBox.Show("Ничья!");
            ResetBoard();
            return;
        }

        if (!CheckWin(BotSymbol)) return;
        MessageBox.Show($"Вы проиграли!");
        ResetBoard();
    }

    private bool IsBoardFull()
    {
        return _buttons.Cast<Button>().All(button => !string.IsNullOrEmpty(button.Content as string));
    }

    private void BotMove()
    {
        var random = new Random();

        for (var i = 0; i < 9; i++)
        {
            var row = random.Next(0, 3);
            var col = random.Next(0, 3);

            if (!string.IsNullOrEmpty(_buttons[row, col].Content as string)) continue;
            var selectedButton = _buttons[row, col];
            selectedButton.Content = BotSymbol;
            break;
        }
    }

    private bool CheckWin(string symbol)
    {
        for (var i = 0; i < 3; i++)
        {
            if (_buttons[i, 0].Content as string == symbol && _buttons[i, 1].Content as string == symbol && _buttons[i, 2].Content as string == symbol)
                return true;

            if (_buttons[0, i].Content as string == symbol && _buttons[1, i].Content as string == symbol && _buttons[2, i].Content as string == symbol)
                return true;
        }

        if (_buttons[0, 0].Content as string == symbol && _buttons[1, 1].Content as string == symbol && _buttons[2, 2].Content as string == symbol)
            return true;

        return _buttons[0, 2].Content as string == symbol && _buttons[1, 1].Content as string == symbol && _buttons[2, 0].Content as string == symbol;
    }

    private void ResetBoard()
    {
        foreach (var button in _buttons)
        {
            button.Content = null;
        }
    }
    
    private void CloseButtonClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

}