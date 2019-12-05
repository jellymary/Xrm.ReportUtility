using System;
using System.Collections.Generic;
using System.Linq;
using Xrm.ReportUtility.BuilderPattern;
using Xrm.ReportUtility.Interfaces;
using Xrm.ReportUtility.Models;
using Xrm.ReportUtility.Services;

namespace Xrm.ReportUtility
{
    public static class Program
    {
        // "Files/table.txt" -data -weightSum -costSum -withIndex -withTotalVolume
        public static void Main(string[] args)
        {
//            args = new[] {"Files/table.txt", "-data", "-weightSum", "-costSum"};
            args = new[] {"Files/table.txt", "-data", "-weightSum", "-costSum", "-withIndex", "-withTotalVolume", "-withTotalWeight"};
            var service = GetReportService(args);

            var report = service.CreateReport();

            PrintReport(report);

            Console.WriteLine("");
            Console.WriteLine("Press enter...");
            Console.ReadLine();
        }

        private static IReportService GetReportService(string[] args)
        {
            var filename = args[0];

            if (filename.EndsWith(".txt"))
            {
                return new TxtReportService(args);
            }

            if (filename.EndsWith(".csv"))
            {
                return new CsvReportService(args);
            }

            if (filename.EndsWith(".xlsx"))
            {
                return new XlsxReportService(args);
            }

            throw new NotSupportedException("this extension not supported");
        }

        private static void PrintReport(Report report)
        {
            if (report.Config.WithData && report.Data != null && report.Data.Any())
            {
                var columnNames = new List<string> {"Наименование", "Объём упаковки", "Масса упаковки", "Стоимость", "Количество"};
                var reportBuilder = new ReportBuilder(columnNames);

                if (report.Config.WithIndex)
                {
                    reportBuilder.AddColumn("№", 0);
                }
                if (report.Config.WithTotalVolume)
                {
                    reportBuilder.AddColumn("Суммарный объём", 6);
                }
                if (report.Config.WithTotalWeight)
                {
                    reportBuilder.AddColumn("Суммарный вес", 7);
                }

                Console.WriteLine(reportBuilder.GetHeaderRow());

                for (var i = 0; i < report.Data.Length; i++)
                {
                    var dataRow = report.Data[i];
                    Console.WriteLine(reportBuilder.GetRowTemplate(), i + 1, dataRow.Name, dataRow.Volume, dataRow.Weight, dataRow.Cost, dataRow.Count, dataRow.Volume * dataRow.Count, dataRow.Weight * dataRow.Count);
                }

                Console.WriteLine();
            }

            if (report.Rows != null && report.Rows.Any())
            {
                Console.WriteLine("Итого:");
                foreach (var reportRow in report.Rows)
                {
                    Console.WriteLine(string.Format("  {0,-20}\t{1}", reportRow.Name, reportRow.Value));
                }
            }
        }
    }
}