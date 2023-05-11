using System.Windows;
using Minigames.Models;

namespace Minigames
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void TicTacToeButtonClick(object sender, RoutedEventArgs e)
        {
            new TicTacToeWindow().Show();
        }

        private void MinesweeperButtonClick(object sender, RoutedEventArgs e)
        {
            new MinesweeperWindow().Show();
        }

        private void SudokuButtonClick(object sender, RoutedEventArgs e)
        {
            new SudokuWindow().Show();
        }
        
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}