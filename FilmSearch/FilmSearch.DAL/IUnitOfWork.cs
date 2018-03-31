namespace FilmSearch.DAL
{
    public interface IUnitOfWork
    {
        IFileRepository FileRepository { get; }
        
        IPersonRepository PersonRepository { get; }
        
        IFilmRepository FilmRepository { get; }

        void Save();
    }
}