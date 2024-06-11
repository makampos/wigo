namespace Nero.Entities;

public static class Mappers
{
    public static AccountBalanceDTO ToDTO(this AccountBalance accountBalance)
    {
        return new AccountBalanceDTO(accountBalance.Name, accountBalance.Amount);
    }
}