using BusinessObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DTOs
{
    public class FilmDTO
    {
        public long FilmId { get; set; }
        public long GenreId { get; set; }
        [Required(ErrorMessage = "Film's cuountry must be not empty!")]
        public string? CountryCode { get; set; }
        [Required(ErrorMessage = "Film's Title must be not empty!")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Film's Description must be not empty!")]
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Film's Duration must be not empty!")]
        [Range(1, long.MaxValue, ErrorMessage = "Film's Duration must be greater than 0!")]
        public long FilmDuration { get; set; }
    }
}
