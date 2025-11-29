using Microsoft.AspNetCore.Mvc;
using GrapheneTrace.Services;
using System;
using System.IO;

namespace GrapheneTrace.Controllers
{
    public class HeatmapController : Controller
    {
        private readonly CsvService _csvService;

        public HeatmapController()
        {
            _csvService = new CsvService();
        }

        // Returns a single 32x32 frame based on frame index
        [HttpGet("/heatmap/data/{fileName}")]
        public IActionResult GetCsvFrame(string fileName, [FromQuery] int frame = 0)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);

            try
            {
                double[,] allData = _csvService.LoadCsv(path);
                int rows = allData.GetLength(0);
                int cols = allData.GetLength(1);
                int frameHeight = 32;
                int totalFrames = rows / frameHeight;

                if (frame < 0 || frame >= totalFrames)
                    return BadRequest($"Frame {frame} out of range (0–{totalFrames - 1})");

                double[,] frameData = new double[frameHeight, cols];
                for (int i = 0; i < frameHeight; i++)
                    for (int j = 0; j < cols; j++)
                        frameData[i, j] = allData[(frame * frameHeight) + i, j];

                double[][] jagged = _csvService.ToJagged(frameData);
                return Json(new { frame, totalFrames, data = jagged });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("/heatmap/view")]
        public IActionResult ViewHeatmap()
        {
            return View();
        }
    }
}

