using CsvHelper;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Reflection;

namespace BlazorServerExample.Data
{
    public class StructureAssetService
    {
        public StructureAsset[] GetStructures(string filterCode, string filterType, string filterLocation, string filterOwner, string filterCondition, string filterInspector)
        {
            using var reader = new StreamReader( "./Data/Structure Assets.csv" );
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<StructureAsset>();

            if ( filterCode != "" )
                records = records.Where( x => x.Code.Contains(filterCode) );

            if (filterType != "")
                records = records.Where(x => x.Code.Contains(filterType));

            if (filterLocation != "")
                records = records.Where(x => x.Code.Contains(filterLocation));

            if (filterOwner != "")
                records = records.Where(x => x.Code.Contains(filterOwner));

            if (filterCondition != "")
                records = records.Where(x => x.Code.Contains(filterCondition));

            if (filterInspector != "")
                records = records.Where(x => x.Code.Contains(filterInspector));

            return records.ToArray();
        }
    }
}