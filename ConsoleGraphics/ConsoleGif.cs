using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleGraphics
{
    class ConsoleGif
    {
        static int[] colourArray = { 0x000000, 0x000080, 0x008000, 0x008080, 0x800000, 0x800080, 0x808000, 0xC0C0C0, 0x808080, 0x0000FF, 0x00FF00, 0x00FFFF, 0xFF0000, 0xFF00FF, 0xFFFF00, 0xFFFFFF };

        public static void ConsoleWriteAnimatedGif(string source)
        {
            var gif = Image.FromFile(source);
            var dimension = new FrameDimension(gif.FrameDimensionsList[0]);
            var frameCount = gif.GetFrameCount(dimension);

            decimal percent = Math.Min(decimal.Divide(Console.LargestWindowWidth, gif.Width), decimal.Divide(Console.LargestWindowHeight, gif.Height));
            Size size = new Size((int)(gif.Width * percent), (int)(gif.Height * percent));

            List<ConsolePixel[,]> ConsoleImages = new List<ConsolePixel[,]>();

            Console.WriteLine($"Frame {0} of {frameCount} processed.");
            for (int i = 0; i < frameCount; i++)
            {
                gif.SelectActiveFrame(dimension, i);

                Bitmap bitmapMax = new Bitmap(gif, size.Width * 2, size.Height);

                ConsolePixel[,] consolePixelArray = new ConsolePixel[size.Width * 2, size.Height];

                for (int j = 0; j < size.Height; j++)
                {
                    for (int k = 0; k < size.Width; k++)
                    {
                        ConsoleGetPixel(bitmapMax.GetPixel(k * 2, j), ref consolePixelArray[k * 2, j]);
                        ConsoleGetPixel(bitmapMax.GetPixel(k * 2 + 1, j), ref consolePixelArray[k * 2 + 1, j]);

                    }
                    //Console.WriteLine();
                }

                Console.Clear();
                ConsoleImages.Add(consolePixelArray);
                Console.WriteLine($"Frame {i + 1} of {frameCount} processed.");

            }
            Console.ResetColor();
            Console.WriteLine("Image processed!");

            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();

            DrawGif(ConsoleImages, size.Height, size.Width);
        }

        private static void DrawGif(List<ConsolePixel[,]> consoleImage, int height, int width)
        {
            while (true)
            {
                for (int i = 0; i < consoleImage.Count; i++)
                {

                    for (int j = 0; j < height; j++)
                    {
                        for (int k = 0; k < width; k++)
                        {
                            Console.ForegroundColor = consoleImage[i][k * 2, j].ForeColour;
                            Console.BackgroundColor = consoleImage[i][k * 2, j].BackColour;
                            Console.Write(consoleImage[i][k * 2, j].Character);

                            Console.ForegroundColor = consoleImage[i][k * 2 + 1, j].ForeColour;
                            Console.BackgroundColor = consoleImage[i][k * 2 + 1, j].BackColour;
                            Console.Write(consoleImage[i][k * 2 + 1, j].Character);
                        }
                        System.Console.WriteLine();
                    }
                    Console.Clear();
                }
            }
        }

        private static void ConsoleGetPixel(Color Value, ref ConsolePixel consolePixel)
        {
            Color[] colourTable = colourArray.Select(x => Color.FromArgb(x)).ToArray();
            char[] CharList = new char[] { (char)9617, (char)9618, (char)9619, (char)9608 };
            int[] trackScore = new int[] { 0, 0, 4, int.MaxValue };

            for (int Character = CharList.Length; Character > 0; Character--)
            {
                for (int Foreground = 0; Foreground < colourTable.Length; Foreground++)
                {
                    for (int Background = 0; Background < colourTable.Length; Background++)
                    {
                        int R = (colourTable[Foreground].R * Character + colourTable[Background].R * (CharList.Length - Character)) / CharList.Length;
                        int G = (colourTable[Foreground].G * Character + colourTable[Background].G * (CharList.Length - Character)) / CharList.Length;
                        int B = (colourTable[Foreground].B * Character + colourTable[Background].B * (CharList.Length - Character)) / CharList.Length;
                        int Score = (Value.R - R) * (Value.R - R) + (Value.G - G) * (Value.G - G) + (Value.B - B) * (Value.B - B);

                        if (!(Character > 1 && Character < 4 && Score > 10000))
                        {
                            if (Score < trackScore[3])
                            {
                                trackScore[3] = Score;
                                trackScore[0] = Foreground;
                                trackScore[1] = Background;
                                trackScore[2] = Character;
                            }
                        }
                    }
                }
            }

            consolePixel.BackColour = (ConsoleColor)trackScore[1];
            consolePixel.ForeColour = (ConsoleColor)trackScore[0];
            consolePixel.Character = CharList[trackScore[2] - 1];
        }

    }
}
