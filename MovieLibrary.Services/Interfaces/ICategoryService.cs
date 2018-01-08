using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Data.Models;

namespace MovieLibrary.Services.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(string category);
        Task<bool> CategoryExistsAsync(string categoryName);
        Task<IList<Category>> GetAllCategories();
        Task<string> GetCategoryNameById(int categoryId);
        Task<bool> IsCategoryExistByIdAsync(int categoryId);
    }
}
