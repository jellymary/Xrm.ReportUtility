using System;
using System.Collections.Generic;
using System.Linq;
using Xrm.ReportUtility.Models;

namespace Xrm.ReportUtility.BuilderPattern
{
    public class ReportBuilder
    {
        private readonly List<string> _columns;
        private readonly string _separator;
        private readonly List<string> _rowTemplate;

        public ReportBuilder(List<string> columns, string separator = "\t")
        {
            _columns = columns;
            _separator = separator;
            _rowTemplate = columns.Select((c, i) => $"{{{i + 1}, {c.Length}}}").ToList();
        }

        public void AddColumn(string name, int columnIndex)
        {
            _columns.Insert(columnIndex, name);
            _rowTemplate.Insert(columnIndex, $"{{{columnIndex}, {name.Length}}}");
        }

        public string GetHeaderRow() => string.Join(_separator, _columns);

        public string GetRowTemplate() => string.Join(_separator, _rowTemplate);
    }
}