using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectName.Models.Base;
using System.Collections.Generic;

namespace ProjectName.HtmlHelpers
{
    public static class GridExtensions
    {
        public static IHtmlContent GetGridViewSchemaConfigData(this IHtmlHelper htmlHelper, GridViewModel viewModel)
        {
            var gridColumns = new HtmlContentBuilder();
            var schemas = new List<string>();

            gridColumns.Append("{");

            schemas.Add("\"Id\": { \"editable\": false }");
            foreach (var column in viewModel.ViewColumns)
            {
                var dataType = "";
                if (!string.IsNullOrWhiteSpace(column.ColumnFormat) && (column.ColumnFormat.Contains("yyyy") || column.ColumnFormat.ToLower().Contains("date")))
                {
                    dataType = ", \"type\":\"date\"";
                }
                if (column.UtcFormat == true)
                {
                    dataType = ", \"utc\":\"true\"";
                }
                schemas.Add("\"" + column.Name + "\": { \"editable\": false " + dataType + " }");
            }
            gridColumns.Append(string.Join(", \n", schemas.ToArray()));
            gridColumns.Append("}");
            return gridColumns;
        }

        public static IHtmlContent GetGridColumnsConfigData(this IHtmlHelper htmlHelper, GridViewModel viewModel)
        {
            var gridColumns = new HtmlContentBuilder();
            var columns = new List<string>();
            // Check if user manager grid => add command reset pass and active
            if (viewModel.UseActionDefaultColumn)
            {
                // Add delete column
                string delColumnString = "{\"field\": \"Command\", \"template\":\"commandTemplate\", \"title\": \"&nbsp;\", \"width\": \"" + (viewModel.ActionDefaultWidthColumn != null ? viewModel.ActionDefaultWidthColumn.ToString() : "150") + "px\",\"mandatory\":true,\"sortable\":false}";
                columns.Add(delColumnString);
            }
            foreach (var column in viewModel.ViewColumns)
            {
                var columnWidthString = column.ColumnWidth == 0 ? "" : string.Format(", \"width\": {0}", column.ColumnWidth);
                var columnString = "{  \"field\": \"" + column.Name + "\", \"title\": \"" + column.Text + "\"" +
                                columnWidthString +
                                ", \"attributes\":{\"style\":\"text-align:" + column.ColumnJustification + ";\"}, \"sortable\": " + column.Sortable.ToString().ToLower()
                                + ", \"filterable\": " + column.Filterable.ToString().ToLower()
                                + ", \"hidden\": " + column.HideColumn.ToString().ToLower()
                                + ", \"mandatory\": " + column.Mandatory.ToString().ToLower();
                if (!string.IsNullOrWhiteSpace(column.CustomTemplate))
                {
                    columnString += ",\"template\":\"" + column.CustomTemplate + "\"";
                }
                if (!string.IsNullOrWhiteSpace(column.ColumnFormat))
                {
                    if (!column.ColumnFormat.ToLower().Contains("date"))
                    {
                        columnString += ", \"format\": \"{0: " + column.ColumnFormat + "}\"";
                    }
                    else
                    {
                        columnString += ", \"format\": \"{0: MM/dd/yyyy hh:mm tt}\"";
                    }
                }
                columnString += "}";
                columns.Add(columnString);
            }

            gridColumns.Append("[");
            gridColumns.Append(string.Join(", ", columns.ToArray()));
            gridColumns.Append("]");

            return gridColumns;
        }
    }
}
