using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaWebClient.Pages.Admin.Show
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ShowCreateDTO Show { get; set; }
        [ViewData]
        public SelectList Rooms { get; set; }
        public void OnGet()
        {
        }
    }
}
