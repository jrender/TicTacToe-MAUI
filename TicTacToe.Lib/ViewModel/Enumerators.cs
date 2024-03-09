using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Lib.ViewModel
{
    public static class Enumerators
    {
        public enum CellState
        {
            Empty,
            Filled,
            Winner
        }
        public enum CellValue
        {
            X,
            O,
            Empty
        }
        public enum PlayerTurn
        {
            PlayerX,
            PlayerO
        }
        public enum GameState
        {
            PlayerXWins,
            PlayerOWins,
            Draw,
            InProgress
        }
    }
}
