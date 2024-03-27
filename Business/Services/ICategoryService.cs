using Etna_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etna_Business.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetCategories();

}
}
