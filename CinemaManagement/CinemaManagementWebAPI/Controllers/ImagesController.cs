using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpGet("{path}")]
        public IActionResult GetImage(string path)
        {
            try
            {
                string folder = "Data/Images";
                string imagePath = Path.Combine(folder, path);
                if (!System.IO.File.Exists(imagePath))
                {
                    return NotFound();
                }

                // Read the image file into a byte array
                var imageData = System.IO.File.ReadAllBytes(imagePath);

                // Determine the content type of the image based on the file extension
                var contentType = "";
                if(path.Contains(".jpg") || path.Contains(".jpeg"))
                {
                    contentType = "image/jpeg";
                }else if (path.Contains(".png"))
                {
                    contentType = "image/png";
                }else if (path.Contains(".gif"))
                {
                    contentType = "image/gif";
                }
                else
                {
                    contentType = "application/octet-stream"; // Fallback MIME type
                }

                // Return the image data as the API response with appropriate content type
                return File(imageData, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Return 500 Internal Server Error with the exception
            }
        }
    }
}
