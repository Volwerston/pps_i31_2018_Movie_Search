namespace FilmSearch.Analytics


module DbContext =  
    open FilmSearch.DAL 
                                 
    let context () = new FilmSearchContext(Constants.connectionString)
        