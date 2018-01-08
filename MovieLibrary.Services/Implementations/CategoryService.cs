using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.Interfaces;

namespace MovieLibrary.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly MovieLibraryDbContext _db;

        public CategoryService(MovieLibraryDbContext db)
        {
            _db = db;
        }

        public async Task AddCategoryAsync(string category)
        {
            await this._db.Categories.AddAsync(new Category()
            {
                Name = category
            });

            await this._db.SaveChangesAsync();
        }

        public async Task<bool> CategoryExistsAsync(string categoryName)
        {
            return await this._db.Categories.AnyAsync(c => c.Name == categoryName);
        }

        public async Task<IList<Category>> GetAllCategories()
        {
            return await this._db.Categories.ToListAsync();
        }

        public async Task<string> GetCategoryNameById(int categoryId)
        {
            var result = await this._db.Categories.FindAsync(categoryId);
            return result.Name;
        }

        public async Task<bool> IsCategoryExistByIdAsync(int categoryId) =>
            await this._db.Categories.AnyAsync(x => x.Id == categoryId);
    }
}
