using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Minigames.Models;

public partial class SudokuWindow
{
    private readonly Button[,] _buttonsDesk = new Button[9, 9];
        private readonly int[,] _numsForButtons = {{3,1,5,8,2,7,9,4,6},{4,6,8,9,1,5,7,3,2},
        {7,2,9,3,4,6,5,1,8},{9,4,6,5,3,8,1,2,7},{5,7,1,6,9,2,4,8,3},{8,3,2,1,7,4,6,9,5},
        {6,9,3,2,5,1,8,7,4},{2,5,7,4,8,9,3,6,1},{1,8,4,7,6,3,2,5,9}};
        private int[,] _solutions = new int[9, 9];
        private int[,] _changeDesk = new int[9, 9];
        private readonly Button[] _buttonsNum = new Button[9];
        private string? _box;
        private int _taps;
        private int _flag = 1;
        private Button? _buttonForColor;
        private int[,] _clearColor = new int[9, 9];


        public SudokuWindow()
        {
            InitializeComponent();
            InitializeButtons();
        } 
        private void GenerateButtons()
        {
            int[,]? desk;
            switch (_flag)
            {
                case 0:
                    PreGen();
                    desk = DeleteCells(35);
                    _clearColor = desk;
                    _changeDesk = desk;
                    break;
                case 1:
                    PreGen();
                    desk = DeleteCells(45);
                    _clearColor = desk;
                    _changeDesk = desk;
                    break;
                default:
                    PreGen();
                    desk = DeleteCells(55);
                    _clearColor = desk;
                    _changeDesk = desk;
                    break;
            }
        }
        private void PreGen()
        {
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    _changeDesk[i, j] = _numsForButtons[i, j];
                }
            }
            Transposing();
            var random = new Random();
            var timesOfSwaps = random.Next(25, 31);
            var swaps = new List<Action>() {Transposing, ChangeBigPiecesRow, ChangeBigPiecesColumn,
            ChangeLittlePiecesRow, ChangeLittlePiecesColumn};
            for (var i = 0; i < timesOfSwaps; i++)
            {
                var randonNumber = random.Next(0, 5);
                swaps[randonNumber].Invoke();
            }
        }
        private void InitializeButtons()
        {
            GenerateButtons();
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    var button = (Button)FindName($"Button{i}{j}")!;
                    _buttonsDesk[i, j] = button;
                    button.Click -= Button_Click;
                    button.Click += Button_Click;
                    button.Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("AliceBlue")!;
                    if (_changeDesk[i,j] == 0)
                    {
                        button.Content = null;
                    } else
                    {
                        button.Content = _changeDesk[i, j];
                    }
                }
            }
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (_buttonsDesk[i, j].Content != null)
                    {
                        _buttonsDesk[i, j].Click -= Button_Click;
                    }
                }
            }
            for (var i = 1; i < 10; i++)
            {
                var button = (Button)FindName($"Button{i}")!;
                _buttonsNum[i - 1] = button;
                button.Click -= Button_Click_Num;
                button.Click += Button_Click_Num;

            }
            var buttonClear = (Button)FindName($"ButtonClear")!;
            buttonClear.Click -= Button_Clear;
            buttonClear.Click += Button_Clear;
            var buttonRules = (Button)FindName($"ButtonRules")!;
            buttonRules.Click -= Button_Rules;
            buttonRules.Click += Button_Rules;
            var buttonAnswer = (Button)FindName($"ButtonAnswer")!;
            buttonAnswer.Click -= Button_Answer;
            buttonAnswer.Click += Button_Answer;
            var buttonDiff = (Button)FindName($"ButtonDiff")!;
            buttonDiff.Click -= Button_Diff;
            buttonDiff.Click += Button_Diff;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }
            

            if (_box != null)
            {
                if (_buttonForColor != null)
                {
                    _buttonForColor.Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("AliceBlue")!;
                }
                button.Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("LightPink")!;
                _buttonForColor = button;
                button.Content = Convert.ToInt32(_box);
               
            }
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (_buttonsDesk[i, j].Content == null) continue;
                    if (_buttonsDesk[i, j].Content.ToString() == _box)
                    {
                        _buttonsDesk[i, j].Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("LightPink")!;
                    }

                }
            }
            if (_taps != 0 && _box == null)
            {
                button.Content = null;
                button.Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("AliceBlue")!;
                for (var i = 0; i < 9; i++)
                {
                    for (var j = 0; j < 9; j++)
                    {
                        if (_clearColor[i, j] == 0)
                        {
                            _buttonsDesk[i, j].Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("LightPink")!;
                        }
                    }
                }
            }

            if (!CheckForWin()) return;
            var res = MessageBox.Show("Ты выиграл(-а).\r\nЕсли хотите поиграть ещё раз, " +
                                      "то нажмите 'Yes/Да', иначе игра выключится.", "Победа!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
            {
                Close();
            } else
            {
                InitializeComponent();
                InitializeButtons();
            }
        }
        private void Button_Diff(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }
            var strings = new List<string>() { "Easy", "Normal", "Hard" };
            _flag++;
            if(_flag == 3)
            {
                _flag = 0;
                button.Content = strings[_flag];
            } else
            {
                button.Content = strings[_flag];
            }

            InitializeButtons();

        }
        private void Button_Click_Num(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            if (button.Content == null) return;
            _box = button.Content.ToString()!;
            for (var i = 0; i< 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    _buttonsDesk[i,j].Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("AliceBlue")!;
                }
            }
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (_buttonsDesk[i, j].Content == null) continue;
                    if (_buttonsDesk[i, j].Content.ToString() == _box)
                    {
                        _buttonsDesk[i,j].Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("LightPink")!;
                    }

                }
            }

        }
        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
            {
                return;
            }
            _taps++;
            _box = null;
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    _buttonsDesk[i, j].Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("AliceBlue")!;
                }
            }
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (_clearColor[i, j] == 0)
                    {
                        _buttonsDesk[i, j].Background = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("LightPink")!;
                    }
                }
            }

        }
        private static void Button_Rules(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
            {
                return;
            }
            const string text = "Как играть в судоку\r\nЦель судоку – заполнить сетку 9×9 цифрами, чтобы в каждом столбце, " +
                                "строке и сетке 3×3 были цифры от 1 до 9. В начале игры некоторые ячейки сетки 9×9 будут заполнены." +
                                " Ваша задача – вписать недостающие цифры и заполнить всю сетку при помощи логики. " +
                                "Не забудьте, ход будет неверным, если: \r\n\r\nЛюбая строка содержит дублирующиеся" +
                                " цифры от 1 до 9\r\nЛюбой столбец содержит дублирующиеся цифры от 1 до 9\r\nЛюбая сетка 3×3 " +
                                "содержит дублирующиеся цифры от 1 до 9" +
                                "\r\n\r\nЧтобы поставить цифру, нажмите на кнопку справа, чтобы выбрать, что именно вы будете использовать, а потом на одну из свободных" +
                                " клеток поля. Для удаления - нажмите кнопку 'Clear', а потом на клетку, где вы хотите стереть цифру (цифры, стоящие изначально, не удаляются).\r\n" +
                                "Игра начинается с сложности 'Normal', изменение сложности приведёт к пересозданию стола!";
            
            MessageBox.Show(text, "Rules and control", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Button_Answer(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
            {
                return;
            }
            var text = "";
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    text += _solutions[i, j] + " ";
                }
                text += "\n";
            }
            MessageBox.Show(text, "Answer", MessageBoxButton.OK, MessageBoxImage.Information );
        }
        private void Transposing()
        {
            var temp = new int[9, 9];
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    temp[j,i] = _numsForButtons[i,j];
                }
            }
            for (var i = 0;i < 9; i++)
            {
                for (var j = 0;j < 9; j++)
                {
                    _changeDesk[i,j] = temp[i,j];
                }
            }
        }
        private void ChangeBigPiecesRow()
        {
            var random = new Random();
            var pos1 = random.Next(3);
            var pos2 = random.Next(3);
            int[] poses = { 0, 3, 6 };
            var temp = new int[3, 9];
            while (pos1 == pos2)
            {
                pos2 = random.Next(3);
            }
            for (var i = 0;i < 3;i++)
            {
                for(var j = 0;j < 9;j++)
                {
                    temp[i,j] = _changeDesk[poses[pos1] + i, j];
                    _changeDesk[poses[pos1] + i, j] = _changeDesk[poses[pos2] + i, j];
                    _changeDesk[poses[pos2] + i, j] = temp[i,j];
                }
            }
        }
        private void ChangeBigPiecesColumn()
        {
            var random = new Random();
            var pos1 = random.Next(3);
            var pos2 = random.Next(3);
            int[] poses = { 0, 3, 6 };
            var temp = new int[9, 3];
            while (pos1 == pos2)
            {
                pos2 = random.Next(3);
            }
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    temp[i, j] = _changeDesk[i, poses[pos1] + j];
                    _changeDesk[i, poses[pos1] + j] = _changeDesk[i, poses[pos2] + j];
                    _changeDesk[i, poses[pos2] + j] = temp[i, j];
                }
            }
        }
        private void ChangeLittlePiecesRow()
        {
            var random = new Random();
            var pos1 = random.Next(3);
            var pos2 = random.Next(3);
            int[] poses = { 0, 3, 6 };
            var temp = new int[3, 9];
            while (pos1 == pos2)
            {
                pos2 = random.Next(3);
            }
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    temp[i, j] = _changeDesk[poses[pos1] + i, j];
                }
            }
            var pos3 = random.Next(3);
            var pos4 = random.Next(3);
            int[] poses1 = { 0, 1, 2 };
            var temp1 = new int[9];
            while (pos3 == pos4)
            {
                pos4 = random.Next(3);
            }
            for (var i = 0; i < 1; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    temp1[j] = temp[poses1[pos3], j];
                    temp[poses1[pos3],j] = temp[poses1[pos4], j];
                    temp[poses1[pos4], j] = temp1[j];
                }
            }
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    _changeDesk[poses[pos1] + i, j] = temp[i, j];
                }
            }

        }
        private void ChangeLittlePiecesColumn()
        {
            var random = new Random();
            var pos1 = random.Next(3);
            var pos2 = random.Next(3);
            int[] poses = { 0, 3, 6 };
            var temp = new int[9, 3];
            while (pos1 == pos2)
            {
                pos2 = random.Next(3);
            }
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    temp[i, j] = _changeDesk[i, poses[pos1] + j];
                }
            }
            var pos3 = random.Next(3);
            var pos4 = random.Next(3);
            int[] poses1 = { 0, 1, 2 };
            var temp1 = new int[9];
            while (pos3 == pos4)
            {
                pos4 = random.Next(3);
            }
            for (var i = 0; i < 1; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    temp1[j] = temp[j, poses1[pos3]];
                    temp[j, poses1[pos3]] = temp[j, poses1[pos4]];
                    temp[j, poses1[pos4]] = temp1[j];
                }
            }
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    _changeDesk[i, poses[pos1] + j] = temp[i, j];
                }
            }
        }
        private int[,] DeleteCells(int space)
        {
            var random = new Random();
            var changeDesk = (int[,])_changeDesk.Clone();
            var spaces = 0;

            while (spaces < space)
            {
                var r = random.Next(0, 9);
                var c = random.Next(0, 9);
                if (changeDesk[r, c] == 0) continue;
                changeDesk[r, c] = 0;
                spaces++;
            }

            while (Solve((int[,])changeDesk.Clone()) != 1)
            {
                spaces = 0;
                changeDesk = (int[,])_changeDesk.Clone();
                while (spaces < space)
                {
                    var r = random.Next(0, 9);
                    var c = random.Next(0, 9);
                    if (changeDesk[r, c] == 0) continue;
                    changeDesk[r, c] = 0;
                    spaces++;
                }
            }

            return changeDesk;
        }
        private static bool Helper(int[,] changeDesk, ref int count)
        {
            var r = -1;
            var c = -1;
            var empty = true;

            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (changeDesk[i, j] != 0) continue;
                    r = i;
                    c = j;
                    empty = false;
                    break;
                }
                if (!empty)
                {
                    break;
                }
            }

            if (empty)
            {
                count++;
                return true;
            }

            for (var num = 1; num <= 9; num++)
            {
                if (!IsValid(changeDesk, r, c, num)) continue;
                changeDesk[r, c] = num;
                if (Helper(changeDesk, ref count))
                {
                    if (count == 1)
                    {
                        return true;
                    }
                }
                changeDesk[r, c] = 0;
            }

            return false;
        }
        private static bool IsValid(int[,] changeDesk, int r, int c, int number)
        {
            for (var i = 0; i < 9; i++)
            {
                if (changeDesk[r, i] == number)
                {
                    return false;
                }
                if (changeDesk[i, c] == number)
                {
                    return false;
                }
                var boxRow = 3 * (r / 3) + i / 3;
                var boxCol = 3 * (c / 3) + i % 3;
                if (changeDesk[boxRow, boxCol] == number)
                {
                    return false;
                }
            }
            return true;
        }

        private int Solve(int[,] changeDesk)
        {
            var count = 0;
            Helper(changeDesk, ref count);
            _solutions = (int[,])changeDesk.Clone();
            return count;
        }
        private bool CheckForWin()
        {
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (_buttonsDesk[i, j].Content == null)
                    {
                        return false;
                    }
                }
            }
            return CheckInSquareForWin() && CheckForRowsForWin() && CheckForColumnForWin();
        }
        
        private bool CheckInSquareForWin()
        {
            var buttons = new List<string>();
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
            }
            if (!CheckForDupli(buttons))
            {
                return false;
            }
            buttons.Clear();
            for (var i = 0; i < 3; i++)
            {
                for (var j = 3; j < 6; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
            }
            if (!CheckForDupli(buttons))
            {
                return false;
            }
            buttons.Clear();
            for (var i = 0; i < 3; i++)
            {
                for (var j = 6; j < 9; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
            }
            if (!CheckForDupli(buttons))
            {
                return false;
            }
            buttons.Clear();
            for (var i = 3; i < 6; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
            }
            if (!CheckForDupli(buttons))
            {
                return false;
            }
            buttons.Clear();
            for (var i = 3; i < 6; i++)
            {
                for (var j = 3; j < 6; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
            }
            if (!CheckForDupli(buttons))
            {
                return false;
            }
            buttons.Clear();
            for (var i = 3; i < 6; i++)
            {
                for (var j = 6; j < 9; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
            }
            if (!CheckForDupli(buttons))
            {
                return false;
            }
            buttons.Clear();
            for (var i = 6; i < 9; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
            }
            if (!CheckForDupli(buttons))
            {
                return false;
            }
            buttons.Clear();
            for (var i = 6; i < 9; i++)
            {
                for (var j = 3; j < 6; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
            }
            if (!CheckForDupli(buttons))
            {
                return false;
            }
            buttons.Clear();
            for (var i = 6; i < 9; i++)
            {
                for (var j = 6; j < 9; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
            }
            if (!CheckForDupli(buttons))
            {
                return false;
            }
            buttons.Clear();
            return true;
        }
        private bool CheckForRowsForWin()
        {
            var buttons = new List<string>();
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    buttons.Add(_buttonsDesk[i, j].Content.ToString()!);
                }
                if (!CheckForDupli(buttons))
                {
                    return false;
                }
                buttons.Clear();
            }
            return true;
        }
        private bool CheckForColumnForWin()
        {
            var buttons = new List<string>();
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    buttons.Add(_buttonsDesk[j, i].Content.ToString()!);
                }
                if (!CheckForDupli(buttons))
                {
                    return false;
                }
                buttons.Clear();
            }
            return true;
        }
        private static bool CheckForDupli(List<string> buttons)
        {
            buttons = buttons.Distinct().ToList();
            return buttons.Count == 9;
        }
}