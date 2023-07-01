using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    public class Util
    {
        public void SaveFile(IFormFile file, string path, string fileName)
        {
            
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = new FileStream(path +"/" + fileName, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Cannot save file!");
            }

        }

        public void DeleteFile(IFormFile file, string path, string fileName)
        {
            var filePath = Path.Combine(path, fileName);    
            try
            {
                // Check if the file exists before attempting to delete it
                File.Delete(filePath);
            }
            catch (IOException ex)
            {
                throw new Exception("File was not existed!");
            }

        }


    }
}
