using Wigo.Domain.Entities;

namespace Wigo.Domain.Interfaces;

public interface ITopUpRepository
{
    Task<IEnumerable<TopUpOption>> GetTopUpOptionsAsync();
}