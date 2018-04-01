namespace FilmSearch.Models
{
    public class Opinion
    {
        public long Id { get; set; }
        public bool Approval { get; set; }
        
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}