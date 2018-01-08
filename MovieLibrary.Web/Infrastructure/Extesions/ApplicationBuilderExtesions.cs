using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieLibrary.Data;
using MovieLibrary.Data.Models;
using MovieLibrary.Services.Implementations;
using MovieLibrary.Services.Interfaces;
using MovieLibrary.Web.WebConstants;

namespace MovieLibrary.Web.Infrastructure.Extesions
{
    public static class ApplicationBuilderExtesions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<MovieLibraryDbContext>().Database.Migrate();
            }
            return app;
        }

        public static IApplicationBuilder SeedRoles(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                Task.Run(async () =>
                {

                    var roleNames = new[]
                    {
                        RoleConstants.AdministratorRole,
                        RoleConstants.UploaderRole,
                        RoleConstants.ModeratorRole,
                        RoleConstants.AuthenticatedUserRole,
                        RoleConstants.BannedUserRole
                    };

                    foreach (var roleName in roleNames)
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = roleName
                            });
                        }
                    }

                    var adminEmail = "admin@movielib.com";

                    var adminUser = await userManager.FindByEmailAsync(adminEmail);
                    if (adminUser == null)
                    {
                        User admin = new User()
                        {
                            Email = adminEmail,
                            UserName = "Admin",
                            FirstName = "Nikola",
                            LastName = "Vasilev",
                            BirthDate = Convert.ToDateTime("1984-03-16")
                        };

                        await userManager.CreateAsync(admin, "test123");
                        await userManager.AddToRoleAsync(admin, RoleConstants.AdministratorRole);
                    }
                }).Wait();
            }
            return app;
        }
        public static IApplicationBuilder SeedCategories(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var categoryManager = serviceScope.ServiceProvider.GetService<ICategoryService>();

                Task.Run(async () =>
                {

                    var categoryNames = new[]
                    {
                      "Action",
                      "Adventure",
                      "Animation",
                      "Biography",
                      "Comedy",
                      "Crime",
                      "Documentary",
                      "Drama",
                      "Family",
                      "Fantasy",
                      "Film Noir",
                      "History",
                      "Horror",
                      "Music",
                      "Musical",
                      "Mystery",
                      "Romance",
                      "Sci-Fi",
                      "Short",
                      "Sport",
                      "Superhero",
                      "Thriller",
                      "War",
                      "Western"
                    };

                    foreach (var categoryName in categoryNames)
                    {
                        if (!await categoryManager.CategoryExistsAsync(categoryName))
                        {
                            await categoryManager.AddCategoryAsync(categoryName);
                        }
                    }

                  
                }).Wait();
            }

            return app;
        }
    }
}

