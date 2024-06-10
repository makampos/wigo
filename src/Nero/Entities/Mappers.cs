namespace Nero.Entities;

public static class Mappers
{
    public static BalanceDTO ToDTO(this Balance balance)
    {
        return new BalanceDTO(balance.Name, balance.Amount);
    }
}