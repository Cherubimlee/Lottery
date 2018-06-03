using LottoryUWP.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoryUWP.Utils
{
    public class HTMLPageUtil
    {

        public static string DataToHTMLCode(DataModel.DrawData drawData, DataModel.SettingData settingData)
        {
            StringBuilder sb = new StringBuilder();

            String tables = "";

            foreach(var group in drawData.DrawHistory)
            {
                tables = buildWinnerTable(group, drawData.ColumnTitles[0], drawData.ColumnTitles[1]) + tables;
            }

            var totalWinner = drawData.DrawHistory.Sum((x) => x.Items.Count);

            sb.Append("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">" +
                "<head>" +
                "   <meta charset=\"utf-8\" />" +
                "   <title>Report</title>" +
                "</head>" +
                "<body>" +
                " <h1>" + settingData.EventTitle + "</h1>" +
                "<p>Total Audiance:" + drawData.OrignalDrawItems.Count + "</p>" +
                "<p>Total Round:" + drawData.DrawHistory.Count + "</p>" +
                "<p>Total Winner:"+ totalWinner + "</p>" +
                "<p>Winner Ratio:" + (int)((double)totalWinner/drawData.OrignalDrawItems.Count * 100.0) + "%</p>" +
                "<p>Report Data:" + DateTime.Now.ToString() + "</p>" +
                " <br />" +
                tables +
                "</body>" +
                "</html>");

            return sb.ToString();
        }

        private static string buildWinnerTable(DrawItemGroup group, string col1Name, string col2Name)
        {
            StringBuilder sb = new StringBuilder();

            String columns = "";

            for(int i=0;i<group.Items.Count;i++)
            {        
                columns = buildTableColumn(group.Items[i] as WinnerItem, group.Items.Count - i) + columns;
            }

            sb.Append(" <TABLE style=\"BORDER-COLLAPSE: collapse\" borderColor=#ffffff cellSpacing=0 bgColor=#ffffff border=1>" +
                "<TBODY>" +
                " <TR>" +
                "<th colSpan=4 style=\"background-color:#000000;color:#ffffff\">" +
                " <DIV align=center>"+ group.GroupTitle +"</DIV>" +
                " </th>" +
                "  </TR>" +
                " <TR>" +
                " <th width=200>" +
                " <DIV align=center>"+ "Index" + "</DIV>" +
                "</th>" +
                " <th width=200>" +
                " <DIV align=center>" + col1Name + "</DIV>" +
                "</th>" +
                " <th width=200>" +
                " <DIV align=center>" + col2Name + "</DIV>" +
                "</th>" +
                " <th width=200>" +
                " <DIV align=center>" + "Time Stamp" + "</DIV>" +
                "</th>" +
                "</TR>" +
                columns +
                " </TBODY>" +
                "</TABLE>" +
                "<br/>");

            return sb.ToString();
        }

        private static string buildTableColumn(WinnerItem item, int index)
        {
            if (item == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append( (index % 2 == 0 ? "<tr>" :  "<tr style=\"background-color:#7f7f7f; color:#e5e5e5\">") +
                " <TD>" +
                "<DIV align=center>"+ index +"</DIV>" +
                " </TD>" +
                " <TD>" +
                "<DIV align=center>" + item.MajorColumnValue + "</DIV>" +
                " </TD>" +
                " <TD>" +
                "<DIV align=center>" + item.SecondaryColumnValue + "</DIV>" +
                " </TD>" +
                " <TD>" +
                "<DIV align=center>" + item.DrawnTimeStamp.ToString() + "</DIV>" +
                " <TD>" +
                "</tr>");

            return sb.ToString();
  

        }
    }
}
