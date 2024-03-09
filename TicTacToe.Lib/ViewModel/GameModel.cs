using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static TicTacToe.Lib.ViewModel.Enumerators;

namespace TicTacToe.Lib.ViewModel
{
    public class GameModel : INotifyPropertyChanged
    {
        #region Properties
        private int _xScore;

		public int XScore
		{
			get { return _xScore; }
			set { SetProperty(ref _xScore, value); }
		}

        private int _oScore;
		public int OScore
		{
            get { return _oScore; }
            set { SetProperty(ref _oScore, value); }
        }

		public ObservableCollection<Cell> Cells { get; private set; }

		private string _message;
		public string Message
		{
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private GameState _gameState;
        public GameState State
        {
            get { return _gameState; }
            set { SetProperty(ref _gameState, value); }
        }

		private PlayerTurn _playerTurn;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand OnCellPressedCommand { get; set; }
        public ICommand OnResetClickedCommand { get; set; }
        public ICommand OnNewGameClickedCommand { get; set; }
        #endregion

        #region Constructors
        public GameModel()
		{
            _message = "";
            XScore = 0;
            OScore = 0;
            Message = "X Player's turn";
            _playerTurn = PlayerTurn.PlayerX;
            _gameState = GameState.InProgress;
            Cells = new ObservableCollection<Cell>();
            for (int i = 0; i < 3; i++)
			{
                for (int j = 0; j < 3; j++)
				{
                    Cells.Add(new Cell(i, j));
                }
            }
            OnCellPressedCommand = new Command<Cell>((Cell arg) => { CellPressed(arg); });
            OnResetClickedCommand = new Command(ResetScores);
            OnNewGameClickedCommand = new Command(NewGame);
        }

       
        #endregion

        #region Public Methods
        public void InitializeGame() 
        {
            ResetScores();
        }
        public void ResetGame() 
        {
            Message = "X Player's turn";
            _playerTurn = PlayerTurn.PlayerX;
            _gameState = GameState.InProgress;
            foreach (var cell in Cells)
            {
                cell.ClearCell();
            }   
        }
        public void ResetScores() 
        {
            XScore = 0;
            OScore = 0;
            ResetGame();
        }
        public void NewGame() 
        {
            //for now, just reset the game.
            // at some point we'll add a way to save old games
            ResetGame();
        }

        public void CellPressed(Cell cell)
        {
            
            if (State != GameState.InProgress || cell is null || cell.State != CellState.Empty)
            {
                return;
            }

            cell.SetCellValue(_playerTurn);
            if (IsGameOver())
            {
                SetWinningMessage();
                return;
            }
            ChangePlayerTurn();

        }

        #endregion

        #region Private Methods
        private void ChangePlayerTurn()
        {
            if (_playerTurn == PlayerTurn.PlayerX)
            {
                _playerTurn = PlayerTurn.PlayerO;
                Message = "O Player's turn";
            }
            else
            {
                _playerTurn = PlayerTurn.PlayerX;
                Message = "X Player's turn";
            }
        }

        private void SetWinningMessage()
        {
            Message = _gameState switch
            {
                GameState.PlayerXWins => "X Player Wins",
                GameState.PlayerOWins => "O Player Wins",
                GameState.Draw => "It's a Draw",
                _ => throw new ArgumentOutOfRangeException("Invalid option for GameState")
            };  
        }
        private bool IsGameOver() 
        {
            for (int i=0; i<3; i++)
            {
                List<Cell> row = Cells.Where(c=> c.XPosition == i).ToList();
                if (MatchThrees(row[0].Value, row[1].Value, row[2].Value))
                {
                    return DeclareWinner(row);
                }
                List<Cell> column = Cells.Where(c => c.YPosition == i).ToList();
                if (MatchThrees(column[0].Value, column[1].Value, column[2].Value))
                {
                    return DeclareWinner(column);
                }
            }
            List<Cell> downRight = Cells.Where(c => c.XPosition == c.YPosition).ToList();
            if (MatchThrees(downRight[0].Value, downRight[1].Value, downRight[2].Value))
            {
                return DeclareWinner(downRight);
            }
            List<Cell> downLeft = Cells.Where(c => c.XPosition + c.YPosition == 2).ToList();
            if (MatchThrees(downLeft[0].Value, downLeft[1].Value, downLeft[2].Value))
            {
                return DeclareWinner(downLeft);
            }
            if (Cells.All(c => c.State != CellState.Empty))
            {
                _gameState = GameState.Draw;
                return true;
            }
            return false;

        }

        private bool DeclareWinner(List<Cell> cells)
        {
            _gameState = cells[0].Value == CellValue.X ? GameState.PlayerXWins : GameState.PlayerOWins;
            if (_gameState == GameState.PlayerXWins)
            {
                XScore++;
            }
            else
            {
                OScore++;
            }
            SetCellWinners(cells);
            return true;
        }

        private static void SetCellWinners(List<Cell> cells)
        {
            cells.ForEach(c => c.SetWinner());
        }

        private bool MatchThrees(CellValue v1, CellValue v2, CellValue v3)
        {
            if (v1 != CellValue.Empty && v1 == v2 && v2 == v3)
            {
                return true;
            }
            return false;
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
