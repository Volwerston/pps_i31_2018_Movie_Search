namespace FilmSearch.Models
{
    public class FilmGenre
    {
        public long FilmId { get; set; }
        public Film Film { get; set; }
        
        public long GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}