using System;
using System.Globalization;
using System.IO;

namespace GrapheneTrace.Services
{
    public class CsvService
    {
        public double[,] LoadCsv(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"CSV file not found: {path}");

            string[] lines = File.ReadAllLines(path);
            int rows = lines.Length;
            int cols = lines[0].Split(',').Length;

            double[,] data = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                string[] parts = lines[i].Split(',');
                for (int j = 0; j < cols; j++)
                    double.TryParse(parts[j], NumberStyles.Any, CultureInfo.InvariantCulture, out data[i, j]);
            }

            return data;
        }

        public double[][] ToJagged(double[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            double[][] jagged = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                jagged[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                    jagged[i][j] = array[i, j];
            }

            return jagged;
        }
    }
}

