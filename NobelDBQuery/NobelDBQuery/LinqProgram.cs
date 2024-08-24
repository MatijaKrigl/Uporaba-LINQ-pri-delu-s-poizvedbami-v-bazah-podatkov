using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

class LinqProgram
{
    static void Main()
    {
        string povezavaNiz = "Data Source=nobelDB.db;Version=3;";
        using (var povezava = new SQLiteConnection(povezavaNiz))
        {
            povezava.Open();

            var ukaz = new SQLiteCommand("SELECT * FROM Nobel", povezava);
            var bralec = ukaz.ExecuteReader();

            List<Nobel> nobelNagrade = new List<Nobel>();

            while (bralec.Read())
            {
                nobelNagrade.Add(new Nobel
                {
                    Leto = Convert.ToInt32(bralec["yr"]),
                    Podrocje = bralec["subject"].ToString(),
                    Zmagovalec = bralec["winner"].ToString()
                });
            }
            // Uporaba operatorja select 
            var imenaZmagovalcev = from nagrada in nobelNagrade
                                   select nagrada.Zmagovalec;
            //// Izpis 
            //Console.WriteLine("Imena zmagovalcev:");
            //foreach (var zmagovalec in imenaZmagovalcev)
            //{
            //    Console.WriteLine(zmagovalec);
            //}


            // Primer uporabe operatorja Where: filtriranje nagrad za kemijo
            var kemijskeNagrade = from nagrada in nobelNagrade
                                  where nagrada.Podrocje == "Chemistry"
                                  select nagrada;
            //// Izpis
            //foreach (var nagrada in kemijskeNagrade)
            //{
            //    Console.WriteLine($"Leto: {nagrada.Leto}, Področje: {nagrada.Podrocje}, Zmagovalec: {nagrada.Zmagovalec}");
            //}


            // Uporaba operatorja order by 
            var razvrsceneNagrade = from nagrada in nobelNagrade
                                    orderby nagrada.Leto
                                    select nagrada;
            //// Izpis 
            //Console.WriteLine("Razvrščene nagrade po letu:");
            //foreach (var nagrada in razvrsceneNagrade)
            //{
            //    Console.WriteLine($"Leto: {nagrada.Leto}, Področje: {nagrada.Podrocje}, Zmagovalec: {nagrada.Zmagovalec}");
            //}


            // Razvrščamo najprej po letu, nato po imenu zmagovalca (abecedno)
            var razvrsceneNagrade2 = nobelNagrade
                .OrderBy(nagrada => nagrada.Leto)         // po letu
                .ThenBy(nagrada => nagrada.Zmagovalec);   // po imenu zmagovalca
            //// Izpis 
            //Console.WriteLine("Razvrščene Nobelove nagrade (po letu in imenu zmagovalca):");
            //foreach (var nagrada in razvrsceneNagrade2)
            //{
            //    Console.WriteLine($"Leto: {nagrada.Leto}, Področje: {nagrada.Podrocje}, Zmagovalec: {nagrada.Zmagovalec}");
            //}


            // Uporaba operatorja group by: združevanje po področju
            var nagradePoPodrocju = from nagrada in nobelNagrade
                                    group nagrada by nagrada.Podrocje into skupina
                                    select skupina;
            //// Izpis 
            //foreach (var skupina in nagradePoPodrocju)
            //{
            //    Console.WriteLine($"Področje: {skupina.Key}");
            //    foreach (var nagrada in skupina)
            //    {
            //        Console.WriteLine($"\tZmagovalec: {nagrada.Zmagovalec}, Leto: {nagrada.Leto}");
            //    }

            //}S

            // operator Distinct
            var unikatnaPodrocja = (from nagrada in nobelNagrade
                                    select nagrada.Podrocje).Distinct();

            // Izpis
            Console.WriteLine("Unikatna podrocja:");
            foreach (var podrocje in unikatnaPodrocja)
            {
                Console.WriteLine(podrocje);
            }

            Console.WriteLine("Pritisni tipko za izhod :)");
            Console.ReadKey();
        }

    }
}

// definiramo enostaven objekt
public class Nobel
{
    public int Leto { get; set; }
    public string Podrocje { get; set; }
    public string Zmagovalec { get; set; }
}
