using System;

namespace FilmSearch.DAL.Impl
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly FilmSearchContext _context;

        private IFileRepository _fileRepository;

        private IPersonRepository _personRepository;

        private IFilmRepository _filmRepository;

        public UnitOfWork(FilmSearchContext context)
        {
            _context = context;
        }

        public IFileRepository FileRepository => _fileRepository ?? (_fileRepository = new FileRepository(_context));

        public IPersonRepository PersonRepository => _personRepository ?? (_personRepository = new PersonRepository(_context));

        public IFilmRepository FilmRepository => _filmRepository ?? (_filmRepository = new FilmRepository(_context));
       
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}