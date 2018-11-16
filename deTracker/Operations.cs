using CsvHelper;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace deTracker
{
    class Operations
    {
        public static DataTable Logreader(string path)
        {
            {
                DataTable dt = new DataTable("Tracker");
                //DataTable dt2 = dt.Clone();

                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                    {
                        using (var csv = new CsvReader(sr))
                        {
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.HasHeaderRecord = true;
                            csv.Read();
                            csv.ReadHeader();

                            //lista kolumn do zachowania
                            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                            dt.Columns.Add(new DataColumn("Latitude", typeof(String)));
                            dt.Columns.Add(new DataColumn("Longitude", typeof(String)));
                            dt.Columns.Add(new DataColumn("Cgi", typeof(String)));
                            dt.Columns.Add(new DataColumn("Psc", typeof(Int32)));
                            dt.Columns.Add(new DataColumn("NetworkType", typeof(String)));

                            //zapisanie danych do DataTable
                            while (csv.Read())
                            {
                                var row = dt.NewRow();
                                foreach (DataColumn column in dt.Columns)
                                {
                                    row[column.ColumnName] = csv.GetField(column.DataType, column.ColumnName);
                                }
                                dt.Rows.Add(row);
                            }

                            /*DataRow[] dr = new DataRow[0];
                            dr = dt.Select("(Latitude <> '') AND (Longitude <> '') AND (Latitude IS NOT NULL) AND (Longitude IS NOT NULL)");
                            if (dr.Length > 0)
                            {
                                dt2 = dr.CopyToDataTable();
                            }*/
                        }
                    }
                }
                //dt.Clear();
                //dt.Dispose();
                return dt;
            }
        }

        public static DataTable FindBTS_orange(DataTable dt)
        {
            DataTable dt2 = dt.Copy();
            dt2.Columns.Add("lac");
            dt2.Columns.Add("cid");
            dt.Columns.Add("network");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string cgi = dt.Rows[i][3].ToString();
                string nt = dt.Rows[i][5].ToString();
                int iof2 = 0;
                int iof3 = 0;
                int n2 = 0;
                int n3 = 0;

                for (int cnt = 0; cnt < 2; cnt++)
                {
                    iof2 = cgi.IndexOf(@"-", n2);
                    n2 = iof2+1;
                }
                for (int cnt = 0; cnt < 3; cnt++)
                {
                    iof3 = cgi.IndexOf(@"-", n3);
                    n3 = iof3+1;
                }
                string lac = cgi.Substring(iof2+1, iof3-iof2-1);
                string cid = cgi.Substring(iof3+1, cgi.Length-iof3-1);
                Console.WriteLine("lac: " + lac);
                Console.WriteLine("cid: " + cid);
                Console.WriteLine("Network: " + nt);

                DataRow ndr;

                dt2.Rows.Add();
            }


            return dt2;
        }

        public static void PrintToConsole(DataTable dt)
        {
            //wyświetl w konsoli
            {
                string data = string.Empty;
                StringBuilder sb = new StringBuilder();
                int cnt = 0;

                if (null != dt && null != dt.Rows)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        foreach (var item in dataRow.ItemArray)
                        {
                            sb.Append(item);
                            sb.Append(',');
                        }
                        sb.AppendLine();
                        cnt++;
                    }

                    data = sb.ToString();
                    Console.WriteLine(sb);
                    Console.WriteLine(cnt);
                    Console.WriteLine();
                }
                //Console.ReadKey();
                //Console.WriteLine();
            }
        }

        public static void SaveToCSV(DataTable dt, string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            //using (var textWriter = File.CreateText(path))
            using (var textWriter = new StreamWriter(path, false, Encoding.UTF8))
            using (var csv = new CsvWriter(textWriter))
            {
                // Write columns
                csv.Configuration.Delimiter = ";";
                foreach (DataColumn column in dt.Columns)
                {
                    csv.WriteField(column.ColumnName);
                }
                csv.NextRecord();

                // Write row values
                foreach (DataRow row in dt.Rows)
                {
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        csv.WriteField(row[i]);
                    }
                    csv.NextRecord();
                }
            }
        }
    }
}
