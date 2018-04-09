using FilmSearch.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.DAL
{
    public interface IPersonCommentRepository : IRepository<PersonComment>
    {
        IEnumerable<PersonComment> GetByPersonId(long id);
    }
}
