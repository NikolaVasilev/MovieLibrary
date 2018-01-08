using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Data;
using MovieLibrary.Services.Interfaces;

namespace MovieLibrary.Services.Implementations
{
    public class PersonService : IPersonService
    {
        private readonly MovieLibraryDbContext _db;

        public PersonService(MovieLibraryDbContext db)
        {
            _db = db;
        }

        public async Task<string> GetPersonNameById(int personId)
        {
            var result = await this._db.Persons.FindAsync(personId);
            return result.Name;
        }
    }
}
