using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public class ImageResize
{
    public static void Main(string[] args)
    {
        bool continueProcessing = true;

        while (continueProcessing)
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

            int scale;
            do
            {
                Console.Write("Enter resize multiplier (e.g., 2 for doubling the size, 0.5 for halving): ");
                string inputScale = Console.ReadLine();

                if (double.TryParse(inputScale, out double scaleDouble) && scaleDouble > 0)
                {
                    scale = (int)scaleDouble;
                    break;
                }
                else
                {
                    Console.WriteLine("ERR: Enter a positive number for the resize multiplier.");
                }
            } while (true);

            double resizeFactor = scale;

            ProcessImages(inputFolderPath, outputFolderPath, resizeFactor);

            Console.WriteLine("Success! You may exit entering enter. If you wish to resize again enter any key or reopen the application.");

            string resizeAgain = Console.ReadLine();

            if (string.IsNullOrEmpty(resizeAgain))
            {
                continueProcessing = false;
            }
        }
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

    private static void ProcessImages(string inputFolderPath, string outputFolderPath, double resizeFactor)
    {
        foreach (var filename in Directory.EnumerateFiles(inputFolderPath, "*.png", SearchOption.AllDirectories))
        {
            string baseName = Path.GetFileNameWithoutExtension(filename);
            string extension = Path.GetExtension(filename);
            string outputPath = Path.Combine(outputFolderPath, baseName + "_resized" + extension);

            int counter = 1;
            while (File.Exists(outputPath)) // -- check if the image already exists and if so adds a number at the end
            {
                outputPath = Path.Combine(outputFolderPath, $"{baseName}_resized_{counter}{extension}");
                counter++;
            }

            ResizeImage(filename, outputPath, resizeFactor);
        }
    }

    public static void ResizeImage(string inputPath, string outputPath, double resizeFactor)
    {
        using (var image = Image.Load(inputPath))
        {
            int newWidth = (int)(image.Width * resizeFactor);
            int newHeight = (int)(image.Height * resizeFactor);

            if (newWidth <= 0 || newHeight <= 0)
            {
                newWidth = Math.Max(1, newWidth);
                newHeight = Math.Max(1, newHeight);
            }

            image.Mutate(x => x.Resize(newWidth, newHeight));
            image.Save(outputPath);
        }
    }
}
