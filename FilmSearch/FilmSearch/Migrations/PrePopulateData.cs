using FilmSearch.DAL;

namespace FilmSearch.Migrations
{
    public class PrePopulateData
    {
        private IUnitOfWork _unitOfWork;
        
        public PrePopulateData(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void PrePopulate()
        {
            
        }
    }
}