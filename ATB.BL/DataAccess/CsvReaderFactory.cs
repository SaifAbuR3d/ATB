using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ATB.DataAccess
{
    public static class CsvReaderFactory
    {
        public static CsvReader CreateCsvReader(string csvFilePath, bool hasHeaderRecord)
        {
            using var streamReader = File.OpenText(csvFilePath);
            var csvConfiguration = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = hasHeaderRecord
            }; 
            return new CsvReader(streamReader, csvConfiguration);
        }
    }
}
