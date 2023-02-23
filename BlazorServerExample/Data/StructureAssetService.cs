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
                records = records.Where( x => x.Code.ToLower().Contains(filterCode.ToLower()) );

            if (filterType != "")
                records = records.Where(x => x.Type.ToLower().Contains(filterType.ToLower()));

            if (filterLocation != "")
                records = records.Where(x => x.Location.ToLower().Contains(filterLocation.ToLower()));

            if (filterOwner != "")
                records = records.Where(x => x.Owner.ToLower().Contains(filterOwner.ToLower()));

            if (filterCondition != "")
                records = records.Where(x => x.Condition.ToLower().Contains(filterCondition.ToLower()));

            if (filterInspector != "")
                records = records.Where(x => x.Inspector.ToLower().Contains(filterInspector.ToLower()));

            return records.ToArray();
        }
    }
}