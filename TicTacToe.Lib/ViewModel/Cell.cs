using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static TicTacToe.Lib.ViewModel.Enumerators;

namespace TicTacToe.Lib.ViewModel
{
    public class Cell : INotifyPropertyChanged
    {
        #region Properties

        public int XPosition { get; private set; }
        public int YPosition { get; private set; }

        public string Name
        {
            get
            {
                return GetAccessibleName();
            }
        }


        private CellValue _cellValue;
        private string _textValue;

        public CellValue Value
        {
            get
            {
                return _cellValue;
            }
        }
        public string TextValue
        {
            set
            {
                SetProperty(ref _textValue, value);
            }
            get
            {
                return _textValue;
            }
        }
        private bool _isStillValid;
        public bool IsStillValid
        {
            set { SetProperty(ref _isStillValid, value); }
            get { return _isStillValid; }
        }
        public bool IsHorizontalLineVisible { get; set; }
        public bool IsVerticalLineVisible { get; set; }
        private CellState _cellState;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CellState State 
        { 
            get { return _cellState; }
            set { if (_cellState == CellState.Empty) { SetProperty(ref _cellState, value); } }
        }
        #endregion
        #region Constructors
        public Cell(int xPosition, int yPosition)
        {
            _textValue = "";
            XPosition = xPosition;
            YPosition = yPosition;
            IsHorizontalLineVisible = xPosition == 1;
            IsVerticalLineVisible = yPosition == 1;
            _cellState = CellState.Empty;
            _cellValue = CellValue.Empty;
            TextValue = SetTextValue();
            IsStillValid = !IsWinner();
        }
        #endregion
        #region Public Methods
        public bool IsWinner()
        {
            return _cellState == CellState.Winner;
        }
        public void SetCellValue(PlayerTurn playerTurn)
        {
            if(State != CellState.Empty)
            {
                return;
            }
            _cellValue = playerTurn switch
            {
                PlayerTurn.PlayerX => CellValue.X,
                PlayerTurn.PlayerO => CellValue.O,
                _ => throw new ArgumentOutOfRangeException("Invalid option for playerTurn"),
            };
            TextValue = SetTextValue();
            _cellState = CellState.Filled;
        }

        //SHIT!
        private string SetTextValue()
        {
            switch (_cellValue)
            {
                case CellValue.X:
                    return "X";
                case CellValue.O:
                    return "O";
                default:
                    return "";
            }
        }

        public void ClearCell()
        {
            _cellValue = CellValue.Empty;
            _cellState = CellState.Empty;
            TextValue = SetTextValue();
            IsStillValid = !IsWinner();

        }
        public void SetWinner()
        {
            if (_cellState == CellState.Filled )
                _cellState = CellState.Winner;
            IsStillValid = !IsWinner();
        }

        #endregion
        #region Private Methods

        private string GetAccessibleName()
        {
            var XName = XPosition switch
            {
                0 => "Top",
                1 => "Middle",
                2 => "Bottom",
                _ => throw new ArgumentOutOfRangeException("Invalid option for XPosition")
            };
            var YName = YPosition switch
            {
                0 => "Left",
                1 => "Middle",
                2 => "Right",
                _ => throw new ArgumentOutOfRangeException("Invalid option for YPosition")
            };

            return XPosition == 1 && YPosition == 1 ? "Center" : $"{XName} {YName}";

        }

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
