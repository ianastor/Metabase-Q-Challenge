using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using dotenv.net; // se solicitan las apikeys desde un .env

class Program
{
    static void Main()
    {
        try
        {
            DotEnv.Load();
            // Variable API key 
            string virustotalApiKey = Environment.GetEnvironmentVariable("VIRUSTOTAL_API_KEY");
            int retryDelay = 1500; //Se agregan estas variables como una solucion un poco primitiva a la pobreza de recursos de geolocationdb, si se remplzara la misma, estas serian obsoletas 
            int retryCount = 3;

            // Leer las IPs desde el archivo CSV
            List<string> ipList = ReadIpListFromCsv("ips-alerts.csv");

            // Crear una lista para almacenar los resultados
            List<Resultado> resultados = new List<Resultado>();

            // Realizar la petición a Geolocation-db
            string geolocationdbApiKey = Environment.GetEnvironmentVariable("GEOLOCATIONDB_API_KEY");
            string geolocationdbUrl = $"https://geolocation-db.com/json/{geolocationdbApiKey}";
            WebClient geolocationdbClient = new WebClient();


            // Realizar la petición a VirusTotal
            string virustotalUrl = $"https://www.virustotal.com/api/v3/ip_addresses";
            WebClient virustotalClient = new WebClient();
            virustotalClient.Headers.Add("x-apikey", virustotalApiKey);

            // Realizar las peticiones para cada IP en la lista
            foreach (string ip in ipList)
            {
                try
                {
                    for (int i = 1; i <= retryCount; i++)
                    {
                        try
                        {
                            string geolocationdbResponse = geolocationdbClient.DownloadString($"{geolocationdbUrl}/{ip}");

                            // Obtener la información del país y el código del país de la respuesta de Geolocation-db
                            JObject geolocationdbJson = JObject.Parse(geolocationdbResponse);
                            string countryName = (string)geolocationdbJson["country_name"];
                            string countryCode = (string)geolocationdbJson["country_code"];


                            string virustotalResponse = virustotalClient.DownloadString($"{virustotalUrl}/{ip}");

                            // Obtener la última reputación de la respuesta de VirusTotal
                            JObject virustotalJson = JObject.Parse(virustotalResponse);
                            int virustotalReputation = int.Parse(virustotalJson["data"]["attributes"]["last_analysis_stats"]["malicious"].ToString());

                            // Se crea un objeto Resultado con los datos obtenidos
                            Resultado resultado = new Resultado
                            {
                                sourceIPAddress = ip,
                                country = countryName,
                                countryCode = countryCode,
                                reputation = virustotalReputation
                            };

                            // Agregar el resultado a la lista de resultados
                            resultados.Add(resultado);
                            Console.WriteLine($"Procesado el IP: {ip}");
                            break;
                        }
                        catch (Exception ex) //Se agregan catches para obtener los errores
                        {
                            Console.WriteLine($"Ocurrio un error procesando la IP {ip}, reintentando en {retryDelay}ms, intento ({i}/{retryCount})");
                            if (i == retryCount) { throw ex; }
                        }
                        System.Threading.Thread.Sleep(retryDelay);

                    }

                }
                catch (WebException ex)
                {
                    String responseFromServer = ex.Message.ToString() + " ";
                    if (ex.Response != null)
                    {
                        Console.WriteLine($"Error consultado el url {ex.Response.ResponseUri}");
                        using (WebResponse response = ex.Response)
                        {
                            Stream dataRs = response.GetResponseStream();
                            using (StreamReader reader = new StreamReader(dataRs))
                            {
                                responseFromServer += reader.ReadToEnd();
                            }
                        }
                    }
                    Console.WriteLine($"Ocurrió un error procesando la IP {ip}: {responseFromServer}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocurrió un error procesando la IP {ip}: {ex.Message}");
                }
            }
            // Escribir los resultados en un archivo CSV
            WriteResultsToCsv(resultados, "ips-alerts.csv"); //Se sobreescribe el archivo original con las nuevas variables 

            Console.WriteLine("Proceso finalizado correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error: {ex.Message}");
        }
    }

    static List<string> ReadIpListFromCsv(string filePath)
    {
        List<string> ipList = new List<string>();
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
        {
            // Leer el encabezado del archivo CSV
            csv.Read();
            csv.ReadHeader();

            // Obtener el índice de la columna "sourceIPAddress" o "IPs"
            int sourceIpIndex = csv.GetFieldIndex("sourceIPAddress") >= 0 ? csv.GetFieldIndex("sourceIPAddress") : csv.GetFieldIndex("IPs");

            while (csv.Read())
            {
                // Obtener el valor del campo "sourceIPAddress" o "IPs"
                string ip = csv.GetField(sourceIpIndex);
                ipList.Add(ip);
            }
            return ipList;
        }
    }

    static void WriteResultsToCsv(List<Resultado> resultados, string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(resultados);
        }
    }

    class Resultado
    {
        public string sourceIPAddress { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public int reputation { get; set; }
    }
}