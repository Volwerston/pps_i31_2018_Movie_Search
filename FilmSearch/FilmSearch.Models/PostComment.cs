namespace FilmSearch.Models
{
    public class PostComment: Comment
    {
        public long PostId { get; set; }
        public Post Post { get; set; }
    }
}