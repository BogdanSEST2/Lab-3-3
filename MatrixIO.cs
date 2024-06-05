using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class Matrix
{
    private double[][] data;

    public Matrix(double[][] data)
    {
        this.data = data;
    }
    public async Task WriteTextAsync(Stream stream, string sep)
    {
        using (StreamWriter writer = new StreamWriter(stream))
        {
            await writer.WriteLineAsync($"{data.Length} {data[0].Length}");

            foreach (var row in data)
            {
                await writer.WriteLineAsync(string.Join(sep, row));
            }
        }
    }

    public async Task ReadTextAsync(Stream stream, string sep)
    {
        using (StreamReader reader = new StreamReader(stream))
        {
            string[] dimensions = (await reader.ReadLineAsync()).Split(' ');
            int rows = int.Parse(dimensions[0]);
            int cols = int.Parse(dimensions[1]);

            data = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                string[] values = (await reader.ReadLineAsync()).Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
                data[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    data[i][j] = double.Parse(values[j]);
                }
            }
        }
    }

    public void WriteBinary(Stream stream)
    {
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(data.Length);
            writer.Write(data[0].Length);
            foreach (var row in data)
            {
                foreach (var val in row)
                {
                    writer.Write(val);
                }
            }
        }
    }

    public void ReadBinary(Stream stream)
    {
        using (BinaryReader reader = new BinaryReader(stream))
        {
            int rows = reader.ReadInt32();
            int cols = reader.ReadInt32();

            data = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                data[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    data[i][j] = reader.ReadDouble();
                }
            }
        }
    }
    public async Task WriteJsonAsync(Stream stream)
    {
        double[][] dataArray = data;
        await JsonSerializer.SerializeAsync(stream, dataArray);
    }

    public async Task ReadJsonAsync(Stream stream)
    {
        data = await JsonSerializer.DeserializeAsync<double[][]>(stream);
    }

    public void WriteToFile(string directory, string filename, Action<Matrix, Stream> writeMethod)
    {
        string filePath = Path.Combine(directory, filename);
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            writeMethod(this, fs);
        }
    }

    public async Task WriteToFileAsync(string directory, string filename, Func<Matrix, Stream, Task> writeMethod)
    {
        string filePath = Path.Combine(directory, filename);
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            await writeMethod(this, fs);
        }
    }

    public static Matrix ReadFromFile(string file, Func<Stream, Matrix> readMethod)
    {
        using (FileStream fs = new FileStream(file, FileMode.Open))
        {
            return readMethod(fs);
        }
    }

    public static async Task<Matrix> ReadFromFileAsync(string file, Func<Stream, Task<Matrix>> readMethod)
    {
        using (FileStream fs = new FileStream(file, FileMode.Open))
        {
            return await readMethod(fs);
        }
    }
}
