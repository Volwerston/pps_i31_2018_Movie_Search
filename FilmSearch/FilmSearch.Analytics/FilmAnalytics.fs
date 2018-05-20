namespace FilmSearch.Analytics

module FilmAnalytics = 
    open FilmSearch.Models
    open System.Linq
    open Microsoft.EntityFrameworkCore
    open DbContext

    
    let topRatedFilms n =
        let context = context()
        query { 
            for film in context.Films do
            sortByDescending film.Performance
            select film
            take n 
        } |> Seq.toList
                
    let worstRatedFilms n = 
        let context = context()
        query { 
            for film in context.Films do
            sortBy film.Performance
            select film
            take n
         } |> Seq.toList
                
    
    let averageFilmRate () =
        let context = context()
        query { 
             for film in context.Films do
             averageBy film.Performance
        }
    
    let medianFilmRate () = 
        let context = context()
        let films = context.Films;
        let filmsCount = films.Count()
        query { 
            for film in films do
            sortBy film.Performance
            select film.Performance
            skip (filmsCount / 2)
            take 1
        } |> Seq.head
    
    let averageRateByGenres () =
        let context = context()
        query { 
            for film in context.Films do
            join filmGenre in context.FilmGenres on (film.Id = filmGenre.FilmId)
            join genre in context.Genres on (filmGenre.GenreId = genre.Id)
            groupValBy film genre.Name into fg
            select (fg.Key, fg |> Seq.averageBy (fun f -> f.Performance))
        } |> dict
    