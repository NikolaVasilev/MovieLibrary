using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieLibrary.Web.Areas.Uploader.Models
{
    public class EditMovieViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        public int? Year { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        [DefaultValue(1)]
        public int? Duration { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 1)]
        public string Plot { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 1)]
        public string TrailerUrl { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 1)]
        public string ImageUrl { get; set; }

        [Range(1, 18)]
        public int? ParrentControl { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        public IList<SelectListItem> Categories { get; set; }

        [Required]
        public IList<string> SelectedCategories { get; set; }

        public bool IsApproved { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z])([a-zA-Z0-9,.' ]*)$", ErrorMessage = "Names are not filled correctly.")]
        [StringLength(1000, MinimumLength = 5)]
        public string Actiors { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z])([a-zA-Z0-9,.' ]*)$", ErrorMessage = "Names are not filled correctly.")]
        [StringLength(1000, MinimumLength = 5)]
        public string Writers { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z])([a-zA-Z0-9,.' ]*)$", ErrorMessage = "Names are not filled correctly.")]
        [StringLength(1000, MinimumLength = 5)]
        public string Director { get; set; }

        public string UploaderUsername { get; set; }
    }
}
