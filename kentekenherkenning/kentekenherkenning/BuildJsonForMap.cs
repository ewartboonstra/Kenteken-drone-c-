using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace kentekenherkenning
{
    /// <summary>
    /// deze klasse genereert een JSON bestand voor gebruik in de interactieve kaart.
    /// </summary>
    class BuildJsonForMap
    {
        private string firstpart =
            "{" +
            "\"id\": \"points\"," +
            "\"type\": \"symbol\"," +
            "\"source\": {" +
            "\"type\": \"geojson\"," +
            "\"data\": {" +
            "\"type\": \"FeatureCollection\"," +
            "\"features\": [";
        private string lastpart =
            "]" +
            "}" +
            "}," +
            "\"layout\": {" +
            "\"icon-image\": \"{icon}-15\"," +
            "\"text-field\": \"{title}\"," +
            "\"text-font\": [\"Open Sans Semibold\", \"Arial Unicode MS Bold\"]," +
            "\"text-offset\": [0, 0.6]," +
            "\"text-anchor\": \"top\"" +
            "}" +
            "}";

        private string createCoordinates(Dictionary<string, DoublePoint> licensePlatePoints)
        {
            string output = "";
            foreach (KeyValuePair<string, DoublePoint> entry in licensePlatePoints)
            {
                output +=
                    "{" +
                    "\"type\": \"Feature\"," +
                    "\"geometry\": {" +
                    "\"type\": \"Point\"," +
                    "\"coordinates\": [ " + entry.Value.Y.ToString().Replace(',', '.') + ", " + entry.Value.X.ToString().Replace(',', '.') + "]" +
                    "}," +
                    "\"properties\": {" +
                    "\"title\": \"" + entry.Key + "\"," +
                    "\"icon\": \"monument\"" +
                    "}" +
                    "},";
            }
            return output.TrimEnd(',');
        }

        public string createJsonString(Dictionary<string, DoublePoint> licensePlatePoints)
        {
            return firstpart + createCoordinates(licensePlatePoints) + lastpart;
        }


    }
    /// <summary>
    /// Haalt template van HTML pagina op, wijzigt deze om de interactieve map te maken en slaat dit op.
    /// </summary>
    class GenerateHTML {
        
        private string _html;
    
        public GenerateHTML(string templatePath)
        {
            using (StreamReader reader = new StreamReader(templatePath))
                _html = reader.ReadToEnd();
        }
        //deze methode vervangt 'tokens' in de HTML voor variabelen die in de functie worden meegegeven.
        public string Render(object values)
        {
            string output = _html;
            foreach (var p in values.GetType().GetProperties())
                output = output.Replace("[" + p.Name + "]", (p.GetValue(values, null) as string) ?? string.Empty);
            return output;
        }
    }
}


