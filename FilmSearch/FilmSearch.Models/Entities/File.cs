using System;

namespace FilmSearch.Models
{
    public class File
    {
        /// <summary>
        /// Database id
        /// </summary>
        public long Id { get; set; }
        public string FileName { get; set; }
        /// <summary>
        /// This path is generic.
        /// It could be both some url or a local path
        /// </summary>
        public string Path { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileType { get; set; }
    }
}