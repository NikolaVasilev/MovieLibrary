using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MovieLibrary.Web.Infrastructure.Extesions
{
    public static class ImagesByUserExtesions
    {
        public static async Task SaveProfileImage(IFormFile imageFile, string username, string path)
        {
            
            Directory.CreateDirectory($"wwwroot/images/users/{username}/Profile");

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
        }
    }
}
