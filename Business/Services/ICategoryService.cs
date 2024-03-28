using Etna_Data.Models;

namespace Etna_Business.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetCategories();

}
}
