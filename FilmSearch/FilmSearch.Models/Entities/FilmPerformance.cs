namespace FilmSearch.Models
{
    public class FilmPerformance
    {
        public long Id { get; set; }
        
        public string UserId { get; set; }
        public AppUser User { get; set; }
        
        public long Performance { get; set; }
        
        public long FilmId { get; set; }
        public Film Film { get; set; }
    }
}