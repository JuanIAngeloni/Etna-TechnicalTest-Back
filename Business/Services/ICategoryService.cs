using Task_Manager.Models;

namespace Task_Manager.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetCategories();

}
}
