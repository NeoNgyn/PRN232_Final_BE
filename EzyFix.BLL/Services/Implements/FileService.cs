using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Responses.Assignments;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Implements
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;

        // Add license context for EPPlus
        static FileService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public async Task<byte[]> ExportAssignmentsToExcel(IEnumerable<AssignmentResponse> assignments)
        {
            try
            {
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Assignments");

                // Add header row
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Student Name";
                worksheet.Cells[1, 4].Value = "Student Email";
                worksheet.Cells[1, 5].Value = "Exam Title";
                worksheet.Cells[1, 6].Value = "Status";
                worksheet.Cells[1, 7].Value = "Submission Date";
                worksheet.Cells[1, 8].Value = "Deadline";
                worksheet.Cells[1, 9].Value = "Created At";
                worksheet.Cells[1, 10].Value = "Updated At";

                // Style header row
                using (var range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                // Add data rows
                int row = 2;
                foreach (var assignment in assignments)
                {
                    worksheet.Cells[row, 1].Value = assignment.AssignmentId;
                    worksheet.Cells[row, 2].Value = assignment.Name;
                    worksheet.Cells[row, 3].Value = assignment.StudentName;
                    worksheet.Cells[row, 4].Value = assignment.StudentEmail;
                    worksheet.Cells[row, 5].Value = assignment.ExamTitle;
                    worksheet.Cells[row, 6].Value = assignment.Status;
                    worksheet.Cells[row, 7].Value = assignment.SubmissionDate;
                    worksheet.Cells[row, 7].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    worksheet.Cells[row, 8].Value = assignment.Deadline;
                    worksheet.Cells[row, 8].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    worksheet.Cells[row, 9].Value = assignment.CreatedAt;
                    worksheet.Cells[row, 9].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    worksheet.Cells[row, 10].Value = assignment.UpdatedAt;
                    worksheet.Cells[row, 10].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    
                    row++;
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Return the Excel package as a byte array
                return await package.GetAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting assignments to Excel");
                throw;
            }
        }
    }
}