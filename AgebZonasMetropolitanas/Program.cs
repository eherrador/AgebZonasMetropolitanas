using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using GeoJSON.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AgebZonasMetropolitanas
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string geoJsonText;
			string cveAgeb;
			int agebs = 0;

			string originalsGeoJsonPath = @"/Users/eherrador/Desktop/GFK/GeoJSON/Originales/";
			string newGeoJsonPath = @"/Users/eherrador/Desktop/GFK/GeoJSON/Originales y Filtrados/";
			string clavesAgebsPath = @"/Users/eherrador/Desktop/GFK/GeoJSON/Claves Agebs ZM/";

			//string clavesAgebs = @"AGS.txt";
			//string clavesAgebs = @"BC.txt";
			//string clavesAgebs = @"DF.txt";  //Tiene que usarse tanto con DFUrbAgeb.json como con MEXUrbAgeb.json. Contiene la clave de ambos.
			//string clavesAgebs = @"GTO.txt";
			//string clavesAgebs = @"JL.txt";
			//string clavesAgebs = @"MOR.txt";
			//string clavesAgebs = @"NL.txt";
			//string clavesAgebs = @"QRO.txt";
			//string clavesAgebs = @"SIN.txt";
			//string clavesAgebs = @"SLP.txt";
			string clavesAgebs = @"SON.txt";

			//string geoJsonFileName = @"AGSUrbAgeb.json";
			//string geoJsonFileName = @"BCUrbAgeb.json";
			//string geoJsonFileName = @"DFUrbAgeb.json";
			//string geoJsonFileName = @"MEXUrbAgeb.json";
			//string geoJsonFileName = @"GTOUrbAgeb.json";
			//string geoJsonFileName = @"JLUrbAgeb.json";
			//string geoJsonFileName = @"MORUrbAgeb.json";
			//string geoJsonFileName = @"NLUrbAgeb.json";
			//string geoJsonFileName = @"QROUrbAgeb.json";
			//string geoJsonFileName = @"SINUrbAgeb.json";
			//string geoJsonFileName = @"SLPUrbAgeb.json";
			string geoJsonFileName = @"SONUrbAgeb.json";


			System.Collections.Generic.List<GeoJSON.Net.Feature.Feature> featuresList = new List<GeoJSON.Net.Feature.Feature> ();

			using (StreamReader streamGeoJSON = new StreamReader (originalsGeoJsonPath + geoJsonFileName)) {
				geoJsonText = streamGeoJSON.ReadToEnd ();
				Console.WriteLine("GeoJSON Original leído...");
				Console.WriteLine(geoJsonText);
			}

			var features = JsonConvert.DeserializeObject<GeoJSON.Net.Feature.FeatureCollection>(geoJsonText);

			Console.WriteLine ("GeoJson Deserializado...");
			Console.WriteLine ("Se inicia la lectura del filtrado de agebs del archivo original... ");
			using (StreamReader streamClavesAgeb = new StreamReader (clavesAgebsPath + clavesAgebs)) {
				while ((cveAgeb = streamClavesAgeb.ReadLine ()) != null) {
					Console.WriteLine ("Buscando la clave ageb: " + cveAgeb);
					foreach (GeoJSON.Net.Feature.Feature f in features.Features) {
						if (f.Properties.ContainsValue (cveAgeb)) {
							Console.WriteLine ("Se ha encontrado la clave ageb");
							featuresList.Add (f);
							//featuresList.Features.Add(f);
							agebs++;
						}
					}
				}
			}

			GeoJSON.Net.Feature.FeatureCollection fc = new GeoJSON.Net.Feature.FeatureCollection (featuresList);
			string featuresSerialized = JsonConvert.SerializeObject (fc);
			Console.WriteLine ("Se ha serializado el objeto lista de agebs...");

			using (StreamWriter outfile = new StreamWriter(newGeoJsonPath + geoJsonFileName))
			{

				outfile.Write (featuresSerialized);
			}
			Console.WriteLine("Se ha generado un nuevo archivo GeoJSON...");
			Console.WriteLine("El nuevo archivo GeoJson contiene: " + agebs + " agebs" );
		}
	}
}
