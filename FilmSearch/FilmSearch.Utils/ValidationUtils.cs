using System;
using FilmSearch.Utils.Exceptions;

namespace FilmSearch.Utils {
    public static class ValidationUtils
    {
        public static void RequireNonNull(object obj, string reason)
        {
            if (obj == null)
            {
                throw new ValidationException(reason);
            }
        }
    }
}