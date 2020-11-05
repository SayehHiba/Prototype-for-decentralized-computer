using Microsoft.AspNetCore.Mvc;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RessourcesHumaines.Models
{
    public class Excel
    {
        public static IWorkbook _workbook;
        private static IWorksheet _worksheet;
        public static ExcelEngine _excelEngine;
        public static async Task<IWorksheet> get()
        {
            await Task.Yield();
            if (_worksheet == null)
            {
                //forcer la methode pour etre asyncrone

                _excelEngine = new ExcelEngine();

                //Instantiate the Excel application object
                IApplication application = _excelEngine.Excel;

                //Assigns default application version
                application.DefaultVersion = ExcelVersion.Excel2016;

                //A existing workbook is opened.              
                string basePath = "C:/Users/sayeh/Downloads/RessourcesHumaines/RH.xlsx";
                FileStream sampleFile = new FileStream(basePath, FileMode.Open);
                _workbook = application.Workbooks.Open(sampleFile);
                _worksheet = _workbook.Worksheets[0];
                //Access first worksheet from the workbook.
            }
            return _worksheet;
        }

        public static async Task save()
        {
            await Task.Yield();

          /*  using (var stream = new MemoryStream())
            {
                string contentType = "Application/msexcel";
                _workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, contentType, "C:/Users/sayeh/Downloads/RessourcesHumaines/RH.xlsx");
            }
          */

        }
    }
}
