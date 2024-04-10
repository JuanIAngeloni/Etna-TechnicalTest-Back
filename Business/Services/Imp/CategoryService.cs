using AutoMapper;
using Task_Manager;
using Task_Manager.Entities;
using Task_Manager.Models;
using Task_Manager.Exceptions;

namespace Task_Manager.Services.Imp
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly TaskManagerDbContext _context;

        public CategoryService(IMapper mapper, TaskManagerDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            try
            {
                List<CategoryEntity> categoriesListEntity = await _context.GetAllCategories();

                List<CategoryModel> categoriesListModel = _mapper.Map<List<CategoryModel>>(categoriesListEntity);

                return categoriesListModel;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error en la obtención de las categorías: {ex.Message}");
            }
        }


    }
}
