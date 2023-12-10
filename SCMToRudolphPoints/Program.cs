using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Npgsql;


class SCMRP
{
    static void Main()
    {
        
        string connectionString = "Host=localhost;Username=postgres;Password=Mzkwcim181099!;Database=postgres";
        Dictionary<string, double> RudolphTableValues = new Dictionary<string, double>();

        double[] wo = ConvertToDouble(Record());
        string[] distancesarray = GettingDistances();
        List<double> records = new List<double>(wo);
        List<string> distances = new List<string>();
        records.Remove(21.75);
        string pomocnik = "REAL";

        string createTableQueryTest = "CREATE TABLE WorldRecords (" +
                          "ID SERIAL PRIMARY KEY, ";
        for (int i = 0; i < distancesarray.Length; i++)
        {
            if (i < distancesarray.Length - 1)
            {
                createTableQueryTest += $"\"{distancesarray[i]}\" {pomocnik}, ";
            }
            else
            {
                createTableQueryTest += $"\"{distancesarray[i]}\" {pomocnik} )";
            }
        }


        string addValuesQueryTest = "INSERT INTO WorldRecords(";
        for (int i = 0;i < distancesarray.Length; i++)
        {
            if (i < distancesarray.Length - 1)
            {
                addValuesQueryTest += $"\"{distancesarray[i]}\", ";
            }
            else
            {
                addValuesQueryTest += $"\"{distancesarray[i]}\" ) VALUES (";
            }
        }
        for (int i = 0; i < records.Count; i++)
        {
            if (i < records.Count-1)
            {
                addValuesQueryTest += $"{records[i]}, ";
            }
            else
            {
                addValuesQueryTest += $"{records[i]} ";
            }
            
        }
        addValuesQueryTest += ");";

       

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(createTableQueryTest, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Powodzenie");
                }
                using (NpgsqlCommand command2 = new NpgsqlCommand(addValuesQueryTest, connection))
                {
                    command2.ExecuteNonQuery();
                    Console.WriteLine("Powodzenie");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Błąd: {e.Message}");
            }
        }  
    }
    public static string[] Distances()
    {
        string url = "https://www.swimrankings.net/index.php?page=recordDetail&recordListId=50001&genderCourse=SCM_1";
        var httpClient = new HttpClient();
        var html = httpClient.GetStringAsync(url).Result;
        string[] http = new string[2]; 
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var Distance = htmlDocument.DocumentNode.SelectNodes("//tr[@class='rankingList0']//td[@class='swimtime']//a[@class='time']");
        return http;
    }
    public static string[] Record()
    {
        string url = "https://www.swimrankings.net/index.php?page=recordDetail&recordListId=50001&genderCourse=SCM_1";
        var httpClient = new HttpClient();
        var html = httpClient.GetStringAsync(url).Result;
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var times = htmlDocument.DocumentNode.SelectNodes("//tr[@class='rankingList0']//td[@class='swimtime']//a[@class='time']");
        var times1 = htmlDocument.DocumentNode.SelectNodes("//tr[@class='rankingList1']//td[@class='swimtime']//a[@class='time']");
        string[] tab = new string[19];
        for (int i = 0; i < 19; i++)
        {
            if (i < 9)
            {
                if (i % 2 == 0)
                {
                    string dziwak = times[i / 2].InnerText.Trim();
                    tab[i] = dziwak;
                }
                else if (i == 1)
                {
                    string dziwak = times1[0].InnerText.Trim();
                    tab[1] = dziwak;
                }
                else if (i % 2 == 1 && i != 1)
                {
                    string dziwak = times1[i / 2].InnerText.Trim();
                    tab[i] = dziwak;
                }
            }
            else if (i >= 9 && i < 12)
            {
                if (i % 2 == 1)
                {
                    string dziwak = times[(i/2)+1].InnerText.Trim();
                    tab[i] = dziwak;
                }
                else if (i % 2 == 0)
                {
                    string dziwak = times1[(i/2)-1].InnerText.Trim();
                    tab[i] = dziwak;
                }
            }
            else if (i >= 12 && i < 16)
            {
                if (i % 2 == 0)
                {
                    string dziwak = times[(i / 2) + 1].InnerText.Trim();
                    tab[i] = dziwak;
                }
                else if (i % 2 == 1)
                {
                    string dziwak = times1[(i/2) - 1].InnerText.Trim();
                    tab[i] = dziwak;
                }
            }
            else if ( i >= 16 && i < 19)
            {
                if (i % 2 == 0)
                {
                    string dziwak = times[(i / 2) + 1].InnerText.Trim();
                    tab[i] = dziwak;
                }
                else if (i % 2 == 1)
                {
                    string dziwak = times[(i/2)-2].InnerText.Trim();
                    tab[i] = dziwak;
                }
            }
        }
        return tab;
    }
    public static double[] ConvertToDouble(string[] doubles)
    {

        double[] times = new double[doubles.Length];
        double[] bifactorial = new double[2];
        for (int i = 0; i < 19; i++)
        {
            if (doubles[i].Contains(":"))
            {

                bifactorial[0] = Convert.ToDouble(doubles[i].Split(":")[0]);
                bifactorial[1] = Convert.ToDouble(doubles[i].Split(":")[1]);
                for (int j = 0; j < bifactorial[0]; j++)
                {
                    bifactorial[1] += 60;
                }
                times[i] = bifactorial[1];
            }
            else
            {
                times[i] = Convert.ToDouble(doubles[i]);
            }
        }
        return times;
    }

    public static string[] GettingDistances()
    {
        string url = "https://www.swimrankings.net/index.php?page=rankingDetail&club=POL";
        var httpClient = new HttpClient();
        var html = httpClient.GetStringAsync(url).Result;
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var Distance = htmlDocument.DocumentNode.SelectNodes("//tr[@class='rankingList0']//td[@class='swimstyle']//a");
        var Distance1 = htmlDocument.DocumentNode.SelectNodes("//tr[@class='rankingList1']//td[@class='swimstyle']//a");
        string[] tab = new string[18];
        for (int i = 0; i < tab.Length; i++)
        {
            if (i % 2 == 0)
            {
                string dziwak = Distance[i/2].InnerText.Trim();
                tab[i] = dziwak;
            }
            else if (i % 2 == 1)
            {
                string dziwak = Distance1[i / 2].InnerText.Trim();
                tab[i] = dziwak;
            }
        }
        return tab;

    }
}