using AutoMapper;
using Etna_Data;
using Etna_Data.Entities;
using Etna_Data.Models;
using gringotts_application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Etna_Business.Services.Imp
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly EtnaDbContext _context;

        public CategoryService(IMapper mapper, EtnaDbContext context)
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
