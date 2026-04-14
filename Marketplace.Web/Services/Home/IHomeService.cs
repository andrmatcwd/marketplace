using Marketplace.Web.Models.Home;

namespace Marketplace.Web.Services.Home;

public interface IHomeService
{
    Task<HomePageVm> GetHomePageAsync(string culture, CancellationToken cancellationToken);
}