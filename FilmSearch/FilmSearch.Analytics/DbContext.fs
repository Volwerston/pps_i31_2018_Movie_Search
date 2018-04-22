namespace FilmSearch.Analytics


module DbContext =  
    open FilmSearch.Models
    open Microsoft.EntityFrameworkCore
    open System.Linq
    open System.Linq.Expressions
    open System
    
    
        
    type FilmSearchContext() =
        inherit DbContext()
        
        override this.OnConfiguring optionsBuilder =
           optionsBuilder.UseNpgsql Constants.connectionString |> ignore
           
        [<DefaultValue>]
        val mutable _users: DbSet<AppUser>        
        member public this.Users with get() = this._users
                                 and set v = this._users <- v
                                 
        [<DefaultValue>]
        val mutable _films: DbSet<Film>        
        member public this.Films with get() = this._films
                                 and set v = this._films <- v
                                 
        [<DefaultValue>]
        val mutable _filmGenres: DbSet<FilmGenre>        
        member public this.FilmGenres with get() = this._filmGenres
                                      and set v = this._filmGenres <- v
                                 
        [<DefaultValue>]
        val mutable _genres: DbSet<Genre>        
        member public this.Genres with get() = this._genres
                                  and set v = this._genres <- v                              
                                 
        
    let context = new FilmSearchContext()
        