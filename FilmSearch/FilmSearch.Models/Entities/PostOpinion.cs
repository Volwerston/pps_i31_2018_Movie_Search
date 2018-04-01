namespace FilmSearch.Models
{
    public class PostOpinion: Opinion
    {
        public long PostId { get; set; }
        public Post Post { get; set; }
    }
}