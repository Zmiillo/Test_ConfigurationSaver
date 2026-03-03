using CS.DataLibrary.Models.Dictionaries;
using System.Threading.Tasks;

namespace CS.LogicLayerLibrary.Interfaces
{
    public interface IDictionaryService
    {
        Task<int> AddRecordAsync(CSFont model);

        Task<int> AddRecordAsync(CSColor model);

        Task<int> EditRecordAsync(CSFont model);

        Task<int> EditRecordAsync(CSColor model);
    }
}
