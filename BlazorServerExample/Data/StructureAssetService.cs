using CsvHelper;
using System.Globalization;
using System.Reflection;

namespace BlazorServerExample.Data
{
    public class StructureAssetService
    {
        public StructureAsset[] GetStructures()
        {
            using var reader = new StreamReader( "./Data/Structure Assets.csv" );
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<StructureAsset>();

            return records.ToArray();
        }
    }
}