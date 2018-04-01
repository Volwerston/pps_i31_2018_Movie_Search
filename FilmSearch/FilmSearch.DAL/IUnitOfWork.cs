namespace FilmSearch.DAL
{
    public interface IUnitOfWork
    {
        IFileRepository FileRepository { get; }
        
        IPersonRepository PersonRepository { get; }
        
        IFilmRepository FilmRepository { get; }
        
        IGenreRepository GenreRepository { get; }
        
        IFilmRoleRepository FilmRoleRepository { get; }
        
        IPersonRoleRepository PersonRoleRepository { get; }

        IFilmGenreRepository FilmGenreRepository { get; }
        
        IFilmPerformanceRepository FilmPerformanceRepository { get; }
        
        void Save();
    }
}