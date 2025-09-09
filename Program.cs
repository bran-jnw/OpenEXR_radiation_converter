using TinyEXR;
using SixLabors.ImageSharp;

namespace OpenEXR_radiation_converter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReadFile();
        }

        private static void ReadFile()
        {
            Console.WriteLine("Type in file directory: ");
            string? path = Console.ReadLine();
            if (path != null)
            {
                float[] rgba;
                int xPixels, yPixels;
                ResultCode result = Exr.LoadEXR(path, out rgba, out xPixels, out yPixels);
                Console.WriteLine(result.ToString());

                if(result == 0)
                {
                    float min = float.MaxValue, max = float.MinValue;
                    for (int y = 0; y < yPixels; ++y)
                    {
                        for (int x = 0; x < xPixels; ++x)
                        {
                            int index = 4 * x + y * xPixels * 4;
                            float r = rgba[index];
                            float g = rgba[index + 1];
                            float b = rgba[index + 2];
                            float a = rgba[index + 3];
                            float pixelValue = (r + g + b) / 3f; //assume grey scale

                            if (pixelValue < min)
                            {
                                min = pixelValue;
                            }
                            if (pixelValue > max)
                            {
                                max = pixelValue;
                            }
                        }
                    }
                    Console.WriteLine("Min value: " + min + ". Max value: " + max);

                    Image<SixLabors.ImageSharp.PixelFormats.Argb32> output = new Image<SixLabors.ImageSharp.PixelFormats.Argb32>(xPixels, yPixels);
                    float range = max - min;
                    for (int y = 0; y < yPixels; ++y)
                    {
                        for (int x = 0; x < xPixels; ++x)
                        {
                            int index = 4 * x + y * xPixels * 4;
                            float r = rgba[index];
                            float g = rgba[index + 1];
                            float b = rgba[index + 2];
                            float a = rgba[index + 3];
                            float pixelValue = (r + g + b) / 3f;
                            float normalizedValue = (pixelValue - min) / range;
                            Color c = GetColorRange(normalizedValue);

                            output[x, y] = c;
                        }
                    }
                    output.SaveAsPng(Path.GetFileNameWithoutExtension(path) + "_output.png");
                }                
            }
            else
            {
                Console.WriteLine("Invalid path, try again");
                ReadFile();
            }
        }

        //based on the kry color map range
        private static Color GetColorRange(float normalizedValue)
        {
            SixLabors.ImageSharp.PixelFormats.Argb32 color = new SixLabors.ImageSharp.PixelFormats.Argb32(0, 0, 0);

            //from black to red
            if(normalizedValue < 0.5f)
            {
                color.R = (byte)(255 * 2f * normalizedValue);
            }
            //from red to yellow
            else
            {
                color.R = 255;
                color.G = (byte)(255 * (2f * normalizedValue - 1f));
            }

            return new Color(color);
        }
    }   
}
