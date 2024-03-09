
using System.Windows.Input;

namespace TicTacToe
{
    public partial class GamePage : ContentPage
    {
        public GamePage(Lib.ViewModel.GameModel model)
        {
            InitializeComponent();


            BindingContext = model;
            model.InitializeGame();
           

        }

        
        
    }
}