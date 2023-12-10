using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Npgsql;


class SCMRP
{
    static void Main()
    {
        string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        Dictionary<string, double> RudolphTableValues = new Dictionary<string, double>();

        double[] wo = ConvertToDouble(Record());
        string[] distancesarray = new string[] { "50mfreestyle", "100mFreestyle", "200mFreestyle", "400mFreestyle", "800mFreestyle", "1500mFreestyle", "50mBackstroke", "100mBackstroke", "200mBackstroke", "50mBreaststroke", "100mBreaststroke", "200mBreaststroke", "50mButterfly", "100mButterfly", "200mButterfly", "100mMedley", "200mMedley", "400mMedley" };
        List<double> records = new List<double>(wo);
        List<string> distances = new List<string>();
        records.Remove(21.75);
        string pomocnik = "REAL";

        string createTableQuery = "CREATE TABLE WorldRecords (" +
                          "ID SERIAL PRIMARY KEY, " +
                          $"\"{distancesarray[0]}\" {pomocnik}, " +
                          $"\"{distancesarray[1]}\" {pomocnik}, " +
                          $"\"{distancesarray[2]}\" {pomocnik}, " +
                          $"\"{distancesarray[3]}\" {pomocnik}, " +
                          $"\"{distancesarray[4]}\" {pomocnik}, " +
                          $"\"{distancesarray[5]}\" {pomocnik}, " +
                          $"\"{distancesarray[6]}\" {pomocnik}, " +
                          $"\"{distancesarray[7]}\" {pomocnik}," +
                          $"\"{distancesarray[8]}\" {pomocnik}, " +
                          $"\"{distancesarray[9]}\" {pomocnik}, " +
                          $"\"{distancesarray[10]}\" {pomocnik}, " +
                          $"\"{distancesarray[11]}\" {pomocnik}," +
                          $"\"{distancesarray[12]}\" {pomocnik}, " +
                          $"\"{distancesarray[13]}\" {pomocnik}, " +
                          $"\"{distancesarray[14]}\" {pomocnik}, " +
                          $"\"{distancesarray[15]}\" {pomocnik}, " +
                          $"\"{distancesarray[16]}\" {pomocnik}, " +
                          $"\"{distancesarray[17]}\" {pomocnik});";

        string addValuesQuery = $"INSERT INTO WorldRecords(\"{distancesarray[0]}\", \"{distancesarray[1]}\", \"{distancesarray[2]}\", \"{distancesarray[3]}\", \"{distancesarray[4]}\", \"{distancesarray[5]}\", \"{distancesarray[6]}\", \"{distancesarray[7]}\", \"{distancesarray[8]}\", \"{distancesarray[9]}\", \"{distancesarray[10]}\", \"{distancesarray[11]}\", \"{distancesarray[12]}\", \"{distancesarray[13]}\", \"{distancesarray[14]}\", \"{distancesarray[15]}\", \"{distancesarray[16]}\", \"{distancesarray[17]}\")" +
            $"VALUES ({records[0]}, {records[1]}, {records[2]}, {records[3]}, {records[4]}, {records[5]}, {records[6]}, {records[7]}, {records[8]}, {records[9]}, {records[10]}, {records[11]}, {records[12]}, {records[13]}, {records[14]}, {records[15]}, {records[16]}, {records[17]});";

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Powodzenie");
                }
                using (NpgsqlCommand command2 = new NpgsqlCommand(addValuesQuery, connection))
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
}