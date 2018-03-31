using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models
{
    public class FileViewModelConverter : IViewModelConverter<IFormFile, File>
    {
        public File Convert(IFormFile source)
        {
            File toReturn = new File();

            toReturn.FileName = source.FileName;
            toReturn.Path = string.Empty;
            toReturn.FileType = source.ContentType;
            toReturn.UploadDate = DateTime.Now;

            return toReturn;
        }
    }
}
