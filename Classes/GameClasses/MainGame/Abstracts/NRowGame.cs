public abstract class NRowGame : Game
{

    public override void ShowGameInfo()
    {
        board.DisplayBoard();
        ShowGameTurnText();
    }


    // NRow games
    // if there is no winner at the end, it is draw,
    protected override void UpdateGameState()
    {
        isAnyWinner = board.IsThereWinner(lastMove);
        isGameOver = isAnyWinner || board.IsDraw();
    }

    //The game ends when the player makes a winning move
    //The last player to move is the winner.
    protected override Player GetWinnerPlayer()
    {
        return playerList![CurrentTurn];
    }

}
