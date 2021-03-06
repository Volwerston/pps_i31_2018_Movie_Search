﻿using System;

namespace FilmSearch.DAL.Impl
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly FilmSearchContext _context;

        private IFileRepository _fileRepository;

        private IPersonRepository _personRepository;

        private IFilmRepository _filmRepository;

        private IGenreRepository _genreRepository;

        private IFilmRoleRepository _filmRoleRepository;

        private IPersonRoleRepository _personRoleRepository;

        private IFilmGenreRepository _filmGenreRepository;

        private IFilmPerformanceRepository _filmPerformanceRepository;

        private IPersonPerformanceRepository _personPerformanceRepository;

        private IPersonCommentRepository _personCommentRepository;

        private IPostRepository _postRepository;

        private IPostCommentRepository _postCommentRepository;

        private IAwardRepository _awardRepository;

        private IFilmAwardRepository _filmAwardRepository;

        public UnitOfWork(FilmSearchContext context)
        {
            _context = context;
        }

        public IAwardRepository AwardRepository => _awardRepository ?? (_awardRepository = new AwardRepository(_context));

        public IFilmAwardRepository FilmAwardRepository => _filmAwardRepository ?? (_filmAwardRepository = new FilmAwardRepository(_context));

        public IFileRepository FileRepository => _fileRepository ?? (_fileRepository = new FileRepository(_context));

        public IPersonRepository PersonRepository => _personRepository ?? (_personRepository = new PersonRepository(_context));

        public IFilmRepository FilmRepository => _filmRepository ?? (_filmRepository = new FilmRepository(_context));
        
        public IGenreRepository GenreRepository => _genreRepository ?? (_genreRepository = new GenreRepository(_context));

        public IFilmRoleRepository FilmRoleRepository =>
            _filmRoleRepository ?? (_filmRoleRepository = new FilmRoleRepository(_context));

        public IPersonRoleRepository PersonRoleRepository =>
            _personRoleRepository ?? (_personRoleRepository = new PersonRoleRepository(_context));
        
        public IFilmGenreRepository FilmGenreRepository =>
            _filmGenreRepository ?? (_filmGenreRepository = new FilmGenreRepository(_context));

        public IFilmPerformanceRepository FilmPerformanceRepository =>
            _filmPerformanceRepository ?? (_filmPerformanceRepository = new FilmPerformanceRepository(_context));

        public IPersonPerformanceRepository PersonPerformanceRepository =>
            _personPerformanceRepository ?? (_personPerformanceRepository = new PersonPerformanceRepository(_context));

        public IPersonCommentRepository PersonCommentRepository =>
            _personCommentRepository ?? (_personCommentRepository = new PersonCommentRepository(_context));
        
        public IPostRepository PostRepository =>
            _postRepository ?? (_postRepository = new PostRepository(_context));
        
        public IPostCommentRepository PostCommentRepository =>
            _postCommentRepository ?? (_postCommentRepository = new PostCommentRepository(_context));

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