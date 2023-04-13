using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using CsvHelper;
using CsvHelper.Configuration;

public class IPData
{
    public string Country { get; set; }
    public string CountryCode { get; set; }
    public string Reputation { get; set; }
}

public class IPAlert
{
    public string SourceIPAddress { get; set; }
}

class Program
{
    static void Main()
    {
        // Leer el archivo CSV
        using (var reader = new StreamReader("ips-alerts.csv"))
        using (var csv = new CsvReader(reader, new CsvConfiguration { HasHeaderRecord = true }))
        {
            var ipAlerts = csv.GetRecords<IPAlert>();

            // Crear una lista para almacenar los datos enriquecidos
            var ipDataList = new List<IPData>();

            // Consultar la API ip2country para obtener el nombre del país y el código de país
            using (var webClient = new WebClient())
            {
                foreach (var ipAlert in ipAlerts)
                {
                    var ip = ipAlert.SourceIPAddress;

                    try
                    {
                        var ip2CountryResponse = webClient.DownloadString($"https://api.ip2country.info/ip?{ip}");
                        var countryData = JsonConvert.DeserializeObject<Dictionary<string, string>>(ip2CountryResponse);
                        var country = countryData["countryName"];
                        var countryCode = countryData["countryCode"];

                        // Consultar la API de Virus Total para obtener la reputación de la IP
                        // Aquí se debe utilizar la API key obtenida previamente como variable de ambiente
                        var apiKey = "5129efeb375f7434b0ce764faf3d2934e306401974c79fe18e0e026ef5cf5272";
                        var virusTotalResponse = webClient.DownloadString($"https://www.virustotal.com/api/v3/ip_addresses/{ip}");
                        var reputationData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(virusTotalResponse);
                        var reputation = reputationData["data"]["attributes"]["last_analysis_stats"]["malicious"].ToString();

                        // Agregar los nuevos campos a la lista
                        var ipData = new IPData
                        {
                            Country = country,
                            CountryCode = countryCode,
                            Reputation = reputation
                        };
                        ipDataList.Add(ipData);
                    }
                    catch (WebException ex)
                    {
                        // Manejar errores de conexión a la API
                        Console.WriteLine($"Error al consultar API: {ex.Message}");
                    }
                    catch (JsonException ex)
                    {
                        // Manejar errores de deserialización JSON
                        Console.WriteLine($"Error al deserializar respuesta JSON: {ex.Message}");
                    }
                }
            }

            // Escribir los datos enriquecidos en un nuevo archivo CSV
            using (var writer = new StreamWriter("ips-alerts-enriched.csv"))
            using (var csvWriter = new CsvWriter(writer, new CsvConfiguration { HasHeaderRecord = true }))
            {
                csvWriter.WriteRecords(ipDataList);
            }

            Console.WriteLine("Enriquecimiento de CSV completado.");
        }
    }
}