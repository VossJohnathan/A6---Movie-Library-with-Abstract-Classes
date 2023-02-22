using System.Data;
using System.IO;
using System.Text;

namespace A4___Movie_Library_Assignment
{
    internal class Program
    {
        //Need to point to the file.
        static string movieFile = $"{Environment.CurrentDirectory}/data/movies.csv";

        static void Main(string[] args)
        {

            char csvDelimiter = ',';

            //1-Create Movie Listing Console Application

            
            

            //Just want to make sure it exists.
            if (!File.Exists(movieFile))
            {
                Console.WriteLine("File is missing!");
                return;
            } else
            {
                Console.WriteLine("File is found! Good to continue!\r\n");
            }

            DataTable dt = CSVtoDataTable(movieFile, csvDelimiter);

            //Ask for which option the user wants.
            Console.WriteLine("Welcome to the Movie Library!");
            Console.WriteLine("1 - Add a Movie to the Library.");
            Console.WriteLine("2 - Display all movies from the Library.");
            Console.WriteLine("Any other button to exit.\r\n");


            var userInput = Console.ReadLine();

            if (userInput == "1")
            {
                Console.WriteLine("Adding to Library!\r\n");

                //movieID, title, genres
                int maxID = Convert.ToInt32(dt.Compute("max([movieID])", string.Empty));
                //Asking user for Movie Title.
                Console.WriteLine("Please Input Movie Title: ");
                string titleInput = Console.ReadLine();
                //Asking user for Movie Genres.
                Console.WriteLine("Please Input Movie Genres: ");
                string genresInput = Console.ReadLine();


                dt.Rows.Add(maxID++, titleInput, genresInput);
                SaveToCSV(dt, ',');




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

                DataRow[] result = dt.Select($"title LIKE '%{EscapeLikeValue(userSearch)}%'");
                //Returning number of results found, in case the keyword returns nothing.
                Console.WriteLine("Results found: {0}", result.Length);
                foreach (DataRow row in result)
                {
                    // movieId,title,genres
                    Console.WriteLine($"{row.Field<string>(0)} - {row.Field<string>(1)} - {row.Field<string>(2)}");


                }




            } else
            {
                Console.WriteLine("Exit Program.\r\nThank you for visiting the Library today!");
            }

            
        }

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