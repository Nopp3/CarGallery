using ExcelDataReader;
using System.Data;
using System.Text;

namespace CarGalleryAPI.Data
{
    public static class ExcelReader
    {
        private static string file = "CarGalleryDBData.xlsx";
        public static DataSet ImportFromExcel()
        {
            DataSet ds = new DataSet();


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("UTF-8");

            FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);

            var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration()
            {
                FallbackEncoding = encoding
            });

            ds = reader.AsDataSet();
            stream.Close();

            return ds;
        }
    }
}
