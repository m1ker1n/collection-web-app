using CollectionWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CollectionWebApp.ViewModels
{
    public class CollectionCreateModel
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Required]
        public int ThemeId { get; set; }
        public ICollection<SelectListItem>? AvailableThemes { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int UserId { get; set; }
   }
}
