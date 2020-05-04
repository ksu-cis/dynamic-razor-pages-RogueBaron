using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// The movies to display on the index page 
        /// </summary>
        public IEnumerable<Movie> Movies { get; protected set; }

        /// <summary>
        /// The current search terms 
        /// </summary>
        [BindProperty]
        public string SearchTerms { get; set; } = "";

        /// <summary>
        /// The filtered MPAA Ratings
        /// </summary>
        [BindProperty]
        public string[] MPAARatings { get; set; }

        /// <summary>
        /// The filtered genres
        /// </summary>
        [BindProperty]
        public string[] Genres { get; set; }

        /// <summary>
        /// The minimum IMDB Rating
        /// </summary>
        [BindProperty]
        public double? IMDBMin { get; set; }

        /// <summary>
        /// The maximum IMDB Rating
        /// </summary>
        [BindProperty]
        public double? IMDBMax { get; set; }

        /// <summary>
        /// The minimum IMDB Rating
        /// </summary>
        [BindProperty]
        public double? RottenMin { get; set; }

        /// <summary>
        /// The maximum IMDB Rating
        /// </summary>
        [BindProperty]
        public double? RottenMax { get; set; }

        /// <summary>
        /// Gets the search results for display on the page
        /// </summary>
        public void OnGet(double? IMDBMin, double? IMDBMax, double? RottenMin, double? RottenMax)
        {
            // Nullable conversion workaround
            SearchTerms = Request.Query["SearchTerms"];
            MPAARatings = Request.Query["MPAARatings"];
            Genres = Request.Query["Genres"];
            this.IMDBMin = IMDBMin;
            this.IMDBMax = IMDBMax;
            this.RottenMin = RottenMin;
            this.RottenMax = RottenMax;

            Movies = MovieDatabase.All;
            // Search movie titles for the SearchTerms
            if (SearchTerms != null)
            {
                Movies = Movies.Where(movie =>
                movie.Title != null &&
                movie.Title.Contains(SearchTerms,
                StringComparison.InvariantCultureIgnoreCase)
                );
            }
            // Filter by MPAA Rating 
            if (MPAARatings != null && MPAARatings.Length != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MPAARating != null &&
                    MPAARatings.Contains(movie.MPAARating)
                    );
            }
            if (Genres != null && Genres.Length != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MajorGenre != null &&
                    Genres.Contains(movie.MajorGenre)
                    );
            }
            if (IMDBMin != null && IMDBMax != null)
            {
                Movies = Movies.Where(movie =>
                    movie.IMDBRating <= IMDBMax &&
                    movie.IMDBRating >= IMDBMin
                    );
            }
            if (RottenMax != null && RottenMin != null)
            {
                Movies = Movies.Where(movie =>
                    movie.RottenTomatoesRating <= RottenMax &&
                    movie.RottenTomatoesRating >= RottenMin
                    );
            }
        }
    }
}
