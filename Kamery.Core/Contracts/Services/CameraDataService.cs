using Kamery.Core.Models;

namespace Kamery.Core.Contracts.Services;

// Remove this class once your pages/features are using your data.
public interface ICameraDataService
{
    Task<IEnumerable<Camera>> GetGridDataAsync();

    Task<IEnumerable<Camera>> GetListDetailsDataAsync();
}
