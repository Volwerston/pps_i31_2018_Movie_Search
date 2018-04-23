using System;
using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.DTO;
using FilmSearch.Models.Entities;

namespace FilmSearch.Services
{
    public class PersonService
    {
        public const int PageSize = 10;
        
        private IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public (IEnumerable<Person>, int) GetPersonsByNamePaginated(string name, int page)
        {
            name = name ?? "";
            var persons = _unitOfWork.PersonRepository.PersonsByName(name);
            return (persons.Skip(PageSize * (page - 1)).Take(PageSize).ToList(), persons.Count());
        }

        public (Person, string) GetPersonData(long id)
        {
            Person person = _unitOfWork.PersonRepository.GetByKey(id);

            File img = _unitOfWork.FileRepository.GetByKey(person.PhotoId);

            return (person, $"data:{img.FileType};base64,{FileManager.GetBase64File(img.Path)}");
        }

        public IEnumerable<PersonCommentChartDTO> GetCommentChartList(IEnumerable<PersonComment> comments, string userEmail)
        {
            List<PersonCommentChartDTO> toReturn = new List<PersonCommentChartDTO>();

            foreach (var comment in comments)
            {
                Person estimated = _unitOfWork.PersonRepository.GetByKey(comment.PersonId);
                PersonCommentChartDTO toAdd = new PersonCommentChartDTO()
                { 
                    Text = comment.Text,
                    Date = comment.CreationDate,
                    Author =  userEmail,
                    Person = estimated.FullName
                };

                toReturn.Add(toAdd);
            }

            return toReturn;
        }

        public double RatePersonRole(long personRole, string userId, long performance)
        {
            IEnumerable<PersonPerformance> allVotes = _unitOfWork.PersonPerformanceRepository.GetAll();

            PersonPerformance pp = allVotes.Where(x => x.UserId == userId && x.PersonRoleId == personRole)
                .FirstOrDefault();

            if(pp != null)
            {
                return -1;
            }
            else
            {
                pp = new PersonPerformance()
                {
                    Performance = performance,
                    PersonRoleId = personRole,
                    UserId = userId
                };

                _unitOfWork.PersonPerformanceRepository.Add(pp);
                _unitOfWork.Save();

                PersonRole pr = _unitOfWork.PersonRoleRepository.GetByKey(personRole);

                long voteSum = allVotes.Where(x => x.PersonRoleId == personRole)
                                         .Select(x => x.Performance)
                                         .Sum();

                int usersVoted = allVotes.Where(x => x.PersonRoleId == personRole).Count();

                usersVoted++;
                voteSum += performance;

                pr.Performance = voteSum / (double)usersVoted;

                _unitOfWork.PersonRoleRepository.Update(pr);

                _unitOfWork.Save();

                return pr.Performance;
            }
        }
    }
}