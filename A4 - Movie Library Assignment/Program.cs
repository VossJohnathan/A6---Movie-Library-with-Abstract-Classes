using System.Data;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.Collections;
using System.Runtime.InteropServices;

namespace A4___Movie_Library_Assignment
{
    internal class Program
    {
        //Need to point to the file.
        static string movieFile = $"{Environment.CurrentDirectory}/data/movies.json";
        //Point to the Show.csv
        static string showFile = $"{Environment.CurrentDirectory}/data/shows.csv";
        //point to the video.csv
        static string videoFile = $"{Environment.CurrentDirectory}/data/videos.csv";

        static void Main(string[] args)
        {

            char csvDelimiter = ',';

            //1-Create Movie Listing Console Application




            //Just want to make sure it exists.
            if (!File.Exists(movieFile) | !File.Exists(showFile) | !File.Exists(videoFile))
            {
                Console.WriteLine("File is missing!");
                return;
            } else
            {
                Console.WriteLine("File is found! Good to continue!\r\n");
            }

            List<Movie> movies = GetMovies(movieFile);
            DataTable dtV = CSVtoDataTable(videoFile, csvDelimiter);
            DataTable dtS = CSVtoDataTable(showFile, csvDelimiter);

            //Ask for which option the user wants.
            Console.WriteLine("Welcome to the Movie Library!");
            Console.WriteLine("1 - Add a Movie to the Library.");
            Console.WriteLine("2 - Display all movies from the Library.");
            Console.WriteLine("3 (NEW! - A6) - Display movies or shows or videos!.");
            Console.WriteLine("Any other button to exit.\r\n");


            var userInput = Console.ReadLine();

            if (userInput == "1")
            {
                Console.WriteLine("Adding to Library!\r\n");
                var movie = new Movie();
                //movieID, title, genres
                
                //Asking user for Movie Title.
                Console.WriteLine("Please Input Movie Title: ");
                movie.Title = Console.ReadLine();
                //Asking user for Movie Genres.
                Console.WriteLine("Please Input Movie Genres: ");
                movie.Genres.Add(Console.ReadLine());

                long newID = 1;
                if (movies.Count > 0)
                {
                    newID = movies.MaxBy(x => x.Id).Id;
                    newID++;
                }

                movie.Id = newID;

                movies.Add(movie);
                SaveMovies(movieFile, movies);
                




            } else if (userInput == "2")
            {
                Console.WriteLine("Displaying Library!\r\n");


                /*
                //This block of code output EVERY line in the movies.csv file.
                int counter = 0;

                // Read the file and display it line by line.  
                foreach (string line in File.ReadLines($"{Environment.CurrentDirectory}/data/movies.csv"))
                {
                    Console.WriteLine(line);
                    counter++;
                }

                Console.WriteLine("There were {0} lines.", counter);
                */

                //Asking user for a keyword for their movie title
                //Using LIKE to get results "Toy" = Toy Story.
                Console.WriteLine("Enter a Movie Title Keyword to search");
                string userSearch = Console.ReadLine();

                /*
                DataRow[] result = movies.Select($"title LIKE '%{EscapeLikeValue(userSearch)}%'");
                //Returning number of results found, in case the keyword returns nothing.
                Console.WriteLine("Results found: {0}", result.Length);
                foreach (DataRow row in result)
                {
                    // movieId,title,genres
                    Console.WriteLine($"{row.Field<string>(0)} - {row.Field<string>(1)} - {row.Field<string>(2)}");


                }
                */



            }
            else if (userInput == "3")
            {

                // ===========================================================
                // This is where the A6 Assignment should begin
                // ===========================================================

                /* Look at the assignment closely.
                 * 1- Create shows and videos.csv files using the provided format 
                 * 1A- Added through the use of abstract classes
                 * https://github.com/mcarthey/Class06-20024/blob/master/Context/MediaManager.cs
                 * 
                 * 2- Add an abstract Display() method to each class to output the content.
                 * 
                 * 3- Ask user which TYPE of media (Movies, shows, videos)
                 * 
                 * 4- Display the specific contents of that file.
                 
                */

                // ===========================================================

                Console.WriteLine("Thank you for trying out our new show and video libraries!");
                Console.WriteLine("1 - Display the new Video library!");
                Console.WriteLine("2 - Display the new Show Library!");
                Console.WriteLine("Please select an option.");
                userInput = Console.ReadLine();

                if (userInput == "1")
                {

                    DataRow[] result = dtV.Select();
                    //Returning number of results found, in case the keyword returns nothing.
                    Console.WriteLine("Results found: {0}", result.Length);
                    foreach (DataRow row in result)
                    {
                        // videoID,title,format,length,regions
                        Console.WriteLine($"{row.Field<string>(0)} - {row.Field<string>(1)} - {row.Field<string>(2)} - {row.Field<string>(3)} - {row.Field<string>(4)}");

                    }

                }
                else if (userInput == "2")
                {
                    DataRow[] result = dtS.Select();
                    //Returning number of results found, in case the keyword returns nothing.
                    Console.WriteLine("Results found: {0}", result.Length);
                    foreach (DataRow row in result)
                    {
                        // showId,title,season,episode,writer
                        Console.WriteLine($"{row.Field<string>(0)} - {row.Field<string>(1)} - {row.Field<string>(2)} - {row.Field<string>(3)} - {row.Field<string>(4)}");

                    }
                }
                else
                {
                    Console.WriteLine("Thank you for trying our new libraries!\r\nExiting Program.");
                }
            }

            else
            {
                Console.WriteLine("Exit Program.\r\nThank you for visiting the Library today!");
            }


        }



        // ==========================================================



        //All the stuff to read / write to the datatables are below.

        public static List<Movie> GetMovies(string strFilePath)
        {
            var movies = new List<Movie>();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                movies = JsonSerializer.Deserialize<List<Movie>>(sr.ReadToEnd());
            }
            return movies;
        }

        public static void SaveMovies(string strFilePath, List<Movie> movies)
        {
            //File.WriteAllLines(movieFile, output, Encoding.UTF8);
            using (StreamWriter sw = new StreamWriter(strFilePath))
            {
                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                options.WriteIndented = true;
                
                sw.Write(JsonSerializer.Serialize(movies, options));
            }
        }



        //static string movieFile = $"{Environment.CurrentDirectory}/data/movies.json";

        public static DataTable CSVtoDataTable(string strFilePath, char csvDelimiter)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(csvDelimiter);
                foreach (string header in headers)
                {
                    try
                    {
                        dt.Columns.Add(header);
                    }
                    catch { }
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(csvDelimiter);
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }

        public static void SaveToCSV(DataTable dt, char csvDelimiter)
        {
            
        
                // code block for writing headers of data table

                int columnCount = dt.Columns.Count;
                string columnNames = "";
                string[] output = new string[dt.Rows.Count + 1];
                for (int i = 0; i < columnCount; i++)
                {
                    columnNames += dt.Columns[i].ToString() + csvDelimiter;
                }
                output[0] += columnNames;

                // code block for writing rows of data table
                for (int i = 1; (i - 1) < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        output[i] += dt.Rows[i - 1][j].ToString() + csvDelimiter;
                    }
                }

                File.WriteAllLines(movieFile, output, Encoding.UTF8);
         
         }

        public static string EscapeLikeValue(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

    }
}