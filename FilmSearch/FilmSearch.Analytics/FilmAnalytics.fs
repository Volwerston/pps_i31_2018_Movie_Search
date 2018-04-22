namespace FilmSearch.Analytics

module FilmAnalytics = 
    open FilmSearch.Models
    open System.Linq
    open Microsoft.EntityFrameworkCore
 
    let context = DbContext.context
    let films = context.Films
    let filmGenres = context.FilmGenres
    let genres = context.Genres
    
    let topRatedFilms n =
        query { 
            for film in films do
            sortByDescending film.Performance
            select film
            take n 
        }
                
    let worstRatedFilms n = 
        query { 
            for film in films do
            sortBy film.Performance
            select film
            take n
         }
                
    
    let averageFilmRate =
        query { 
             for film in films do
             averageBy film.Performance
        }
    
    let medianFilmRate = 
        let filmsCount = films.Count()
        query { 
            for film in films do
            sortBy film.Performance
            select film.Performance
            skip (filmsCount / 2)
        }
    
    let averageRateByGenres = 
        query { 
            for film in films do
            join filmGenre in filmGenres on (film.Id = filmGenre.FilmId)
            join genre in genres on (filmGenre.GenreId = genre.Id)
            groupValBy film genre.Name into fg
            select (fg.Key, fg |> Seq.averageBy (fun f -> f.Performance))
        } |> dict
    