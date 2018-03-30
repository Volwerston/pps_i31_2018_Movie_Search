namespace FilmSearch.Models
{
    public class CommentOpinion: Opinion
    {
        public long CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}