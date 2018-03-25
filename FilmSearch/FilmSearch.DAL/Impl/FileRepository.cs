using FilmSearch.Models;

namespace FilmSearch.DAL.Impl
{
    public class FileRepository: GenericRepository<File>, IFileRepository
    {
        public FileRepository(FilmSearchContext ctx) :
            base(ctx)
        {
            
        }
    }
}