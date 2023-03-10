using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A4___Movie_Library_Assignment
{
    public class Movie
    {
        //movieId,title,genres
        
        public long Id { get; set; }
        public string Title { get; set; }
        public List<string> Genres { get; set; }

        public Movie()
        {
            Genres = new List<string>();
        }

    }
}
