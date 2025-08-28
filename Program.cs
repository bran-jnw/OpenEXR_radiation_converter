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

                float min = float.MaxValue, max = float.MinValue;
                for (int i = 0; i < rgba.Length; ++i)
                {
                    float pixelValue = rgba[i]; 
                    if(pixelValue < min)
                    {
                        min = pixelValue;
                    }
                    if(pixelValue > max)
                    {
                        max = pixelValue;
                    }
                }
                Console.WriteLine("Min value: " + min + ". Max value: " + max);

                for (int i = 0; i < rgba.Length; ++i)
                {
                    float pixelValue = rgba[i];
                    rgba[i] = 
                }

            }
            else
            {
                Console.WriteLine("Invalid path, try again");
                ReadFile();
            }
        }

        private static Color TemperatureRange(double BlueToRed)
        {
            double r, g, b;

            // blue to cyan
            if (BlueToRed < -0.5)
            {
                r = 0;
                g = 2 + BlueToRed * 2;
                b = 1;
            }

            // cyan to green
            else if (BlueToRed < 0)
            {
                r = 0;
                g = 1;
                b = -BlueToRed * 2;
            }

            // green to yellow
            else if (BlueToRed < 0.5)
            {
                r = BlueToRed * 2;
                g = 1;
                b = 0;
            }

            // yellow to red
            else
            {
                r = 1;
                g = 2 - BlueToRed * 2;
                b = 0;
            }

            return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
    }   
}
