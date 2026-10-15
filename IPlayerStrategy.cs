namespace Kunskapskontroll3
{

public interface IPlayerStrategy
{
    Move ChooseMove(Hand hand, Card dealerUpcard);
}
}