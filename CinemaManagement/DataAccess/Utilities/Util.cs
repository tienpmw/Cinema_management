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
        public void SaveFile(IFormFile file, string path)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = new FileStream(path, FileMode.Create))
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
    }
}
