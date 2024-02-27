using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class ImageResize
{
    public static void Main(string[] args)
    {
        string inputFolderPath;
        do
        {
            Console.Write("Enter folder containing images: ");
            inputFolderPath = Console.ReadLine();
        } while (!Directory.Exists(inputFolderPath));

        string outputFolderPath;
        do
        {
            Console.Write("Enter folder where you want to save resized images: ");
            outputFolderPath = Console.ReadLine();
        } while (!Directory.Exists(outputFolderPath) && !CreateDirectory(outputFolderPath));

        int incrementWidth;
        int incrementHeight;
        do
        {
            Console.Write("Enter resize increment (width,height): ");
            string inputIncrement = Console.ReadLine();

            if (int.TryParse(inputIncrement.Split(',')[0], out incrementWidth) && int.TryParse(inputIncrement.Split(',')[1], out incrementHeight))
            {
                break;
            }
            else
            {
                Console.WriteLine("ERR: enter width and height separated by a comma.");
            }
        } while (true);

        ProcessImages(inputFolderPath, outputFolderPath, incrementWidth, incrementHeight);
    }

    private static bool CreateDirectory(string path)
    {
        try
        {
            Directory.CreateDirectory(path);
            return true;
        }
        catch (Exception)
        {
            Console.WriteLine($"ERR: Failed to create directory: {path}");
            return false;
        }
    }

    private static void ProcessImages(string inputFolderPath, string outputFolderPath, int incrementWidth, int incrementHeight)
    {
        foreach (var filename in Directory.EnumerateFiles(inputFolderPath, "*.png", SearchOption.AllDirectories))
        {
            string outputPath = Path.Combine(outputFolderPath, Path.GetFileNameWithoutExtension(filename) + "_resized.png");

            ResizeImage(filename, outputPath, incrementWidth, incrementHeight);
        }
    }

    public static void ResizeImage(string inputPath, string outputPath, int incrementWidth, int incrementHeight)
    {
        using (var image = Image.Load(inputPath))
        {
            int newWidth = image.Width + incrementWidth;
            int newHeight = image.Height + incrementHeight;

            if (newWidth <= 0 || newHeight <= 0)
            {
                throw new ArgumentException("ERR: NEGATIVE NUMBER");
            }

            image.Mutate(x => x.Resize(newWidth, newHeight));
            image.Save(outputPath);
        }
    }
}
