using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Services.Interfaces
{
    public interface IPersonService
    {
        Task<string> GetPersonNameById(int personId);
    }
}
