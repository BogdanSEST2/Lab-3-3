using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string textFormatDir = "Bogdan's folder";
            string jsonFormatDir = "Bogdan's JSON";
            string binaryFormatDir = "Bogdan's binary dir";

            if (Directory.Exists(textFormatDir))
                Directory.Delete(textFormatDir, true);
            if (Directory.Exists(jsonFormatDir))
                Directory.Delete(jsonFormatDir, true);
            if (Directory.Exists(binaryFormatDir))
                Directory.Delete(binaryFormatDir, true);

            Directory.CreateDirectory(textFormatDir);
            Directory.CreateDirectory(jsonFormatDir);
            Directory.CreateDirectory(binaryFormatDir);

            Matrix[] matricesA = new Matrix[50];
            Matrix[] matricesB = new Matrix[50];

            Random rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                double[,] valuesA = new double[500, 100];
                double[,] valuesB = new double[100, 500];

                for (int j = 0; j < 500; j++)
                {
                    for (int k = 0; k < 100; k++)
                    {
                        valuesA[j, k] = rand.NextDouble();
                        valuesB[k, j] = rand.NextDouble();
                    }
                }

                matricesA[i] = new Matrix(valuesA);
                matricesB[i] = new Matrix(valuesB);
            }
            Task t1 = Task.Run(async () =>
            {
                for (int i = 0; i < 50; i++)
                {
                    string filePath = Path.Combine(textFormatDir, $"Result{i}.tsv");
                    await WriteProductToFileAsync(matricesA[i], matricesB[i], filePath);
                }
            });

            Task t2 = Task.Run(async () =>
            {
                for (int i = 0; i < 50; i++)
                {
                    string filePath = Path.Combine(jsonFormatDir, $"Result{i}.json");
                    await WriteProductToFileAsync(matricesB[i], matricesA[i], filePath);
                }
            });

            Task t3 = Task.Run(async () =>
            {
                for (int i = 0; i < 50; i++)
                {
                    string filePath = Path.Combine(binaryFormatDir, $"Result{i}.dat");
                    await WriteProductToFileBinaryAsync(matricesA[i], matricesB[i], filePath);
                }
            });

            await Task.WhenAll(t1, t2, t3);

            for (int i = 0; i < 50; i++)
            {
                string textFilePathA = Path.Combine(textFormatDir, $"Result{i}.tsv");
                string textFilePathB = Path.Combine(jsonFormatDir, $"Result{i}.json");

                Task<Matrix> readTextTaskA = ReadMatrixFromFileAsync(textFilePathA);
                Task<Matrix> readTextTaskB = ReadMatrixFromFileAsync(textFilePathB);

                Matrix matrixA = await readTextTaskA;
                Matrix matrixB = await readTextTaskB;

                bool isEqual = matrixA.Equals(matrixB);
                Console.WriteLine($"Are matrices {i} equal? {isEqual}");
            }

            Console.ReadKey();
        }
        public static async Task WriteProductToFileAsync(Matrix a, Matrix b, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync("Matrix A:");
                await writer.WriteLineAsync(a.ToString());
                await writer.WriteLineAsync("Matrix B:");
                await writer.WriteLineAsync(b.ToString());
                await writer.WriteLineAsync("Product of A and B:");
                await writer.WriteLineAsync(MatrixOperations.Multiply(a, b).ToString());
            }
        }
        public static async Task WriteProductToFileBinaryAsync(Matrix a, Matrix b, string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(a.Rows);
                writer.Write(a.Columns);
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < a.Columns; j++)
                    {
                        writer.Write(a[i, j]);
                    }
                }

                writer.Write(b.Rows);
                writer.Write(b.Columns);
                for (int i = 0; i < b.Rows; i++)
                {
                    for (int j = 0; j < b.Columns; j++)
                    {
                        writer.Write(b[i, j]);
                    }
                }
            }
        }
        public static async Task<Matrix> ReadMatrixFromFileAsync(string filePath)
        {
            Matrix matrix;
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Read matrix size
                string[] dimensions = (await reader.ReadLineAsync()).Split(' ');
                int rows = int.Parse(dimensions[0]);
                int cols = int.Parse(dimensions[1]);

                double[,] values = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    string[] line = (await reader.ReadLineAsync()).Split('\t');
                    for (int j = 0; j < cols; j++)
                    {
                        values[i, j] = double.Parse(line[j]);
                    }
                }
                matrix = new Matrix(values);
            }
            return matrix;
        }
    }
}