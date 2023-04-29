using System.Xml.Serialization;

namespace Isbm2Client.Test;

[XmlInclude(typeof(Record)), XmlInclude(typeof(Record[]))]
public class TestObject
{
    // TODO: Needs to be set back to int[] after issue on server has been resolved
    public double[] Numbers { get; set; } = null!;
    public string Text { get; set; } = "";
    [XmlIgnore]
    public Dictionary<string, double> Weather { get; set; } = new Dictionary<string, double>();

    [XmlArray(ElementName = "Weather")]
    [XmlArrayItem(ElementName = "Record", Type = typeof(Record))]
    public Record[] SerializableWeather
    {
        get
        {
            return (
                from e in Weather
                select new Record() {Location = e.Key, Temperature = e.Value}
            ).ToArray();
        }
        set
        {
            Weather = new Dictionary<string, double>(
                from e in value
                select KeyValuePair.Create(e.Location, e.Temperature)
            );
        }
    }

    [Serializable]
    public class Record
    {
        [XmlAttribute("Location", DataType = "string")]
        public string Location { get; set; } = "";
        [XmlAttribute("Temperature", DataType = "double")]
        public double Temperature { get; set; }
    }
}
