namespace FilmSearch.DAL
{
    public interface IUnitOfWork
    {
        IFileRepository FileRepository { get; }
        
        IPersonRepository PersonRepository { get; }

        void Save();
    }
}