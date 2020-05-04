
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;


namespace ConsoleGraphics
{
    class ConsoleImage
    {
        static int[] colourArray = { 0x000000, 0x000080, 0x008000, 0x008080, 0x800000, 0x800080, 0x808000, 0xC0C0C0, 0x808080, 0x0000FF, 0x00FF00, 0x00FFFF, 0xFF0000, 0xFF00FF, 0xFFFF00, 0xFFFFFF };
        static int[] grayScaleColourArray = { 0x000000, 0xFFFFFF };
        public static void ConsoleWritePixels(Color Value, Color Value2)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Color[] colourTable = colourArray.Select(x => Color.FromArgb(x)).ToArray();
            //char[] rList = new char[] { (char)9617, (char)9618, (char)9619, (char)9608 }; // 1/4, 2/4, 3/4, 4/4
            int[] trackScore = new int[] { 0, 0, int.MaxValue, int.MaxValue }; //ForeColor, BackColor, Score, Score2


            for (int Background = 0; Background < colourTable.Length; Background++)
            {
                int R = colourTable[Background].R;
                int G = colourTable[Background].G;
                int B = colourTable[Background].B;

                int Score = (Value2.R - R) * (Value2.R - R) + (Value2.G - G) * (Value2.G - G) + (Value2.B - B) * (Value2.B - B);


                if (Score < trackScore[3])
                {
                        trackScore[3] = Score; 
                        trackScore[1] = Background;  
                }
                
                
            }

            for (int Foreground = 0; Foreground < colourTable.Length; Foreground++)
            {
                int R = colourTable[Foreground].R;
                int G = colourTable[Foreground].G;
                int B = colourTable[Foreground].B;

                int Score = (Value.R - R) * (Value.R - R) + (Value.G - G) * (Value.G - G) + (Value.B - B) * (Value.B - B);


                if (Score < trackScore[2])
                {
                    trackScore[2] = Score;
                    trackScore[0] = Foreground;  
                }


            }
            Console.BackgroundColor = (ConsoleColor)trackScore[1];
            Console.ForegroundColor = (ConsoleColor)trackScore[0];

            Console.Write("\u2580");
        }

        static void SetPixel(ref Bitmap currentWorkingBitMap, int x, int y, float eR, float eG, float eB, float ditherConstant)
        {
            Color c = currentWorkingBitMap.GetPixel(x, y);
            float r = c.R;
            float g = c.G;
            float b = c.B;
            r += eR * ditherConstant;
            g += eG * ditherConstant;
            b += eB * ditherConstant;

            if (r > 255)
                r = 255;

            if (g > 255)
                g = 255;

            if (b > 255)
                b = 255;

            if (r < 0)
                r = 0;

            if (g < 0)
                g = 0;

            if (b < 0)
                b = 0;

            currentWorkingBitMap.SetPixel(x, y, Color.FromArgb((int)r, (int)g, (int)b));
        }

        public static void ConsoleWritePixels2(Bitmap bmp)
        {
            var currentWorkingBitmap = bmp;

            for (int y = 0; y < bmp.Height - 1; y++)
                for(int x = 1; x < bmp.Width - 1; x++)
                {
                    Color pixel = currentWorkingBitmap.GetPixel(x, y);
                    float oldR = pixel.R;
                    float oldG = pixel.G;
                    float oldB = pixel.B;
                    int factor = 1;
                    int newR = (int)Math.Round(factor * oldR / 255) * (255/ factor);
                    int newG = (int)Math.Round(factor * oldG / 255) * (255 / factor);
                    int newB = (int)Math.Round(factor * oldB / 255) * (255 / factor);
                    Color newPixel = Color.FromArgb(newR, newG, newB);

                    currentWorkingBitmap.SetPixel(x, y, newPixel);

                    float errorR = oldR - newR;
                    float errorG = oldG - newG;
                    float errorB = oldB - newB;

                    SetPixel(ref currentWorkingBitmap, x + 1, y, errorR, errorG, errorB, 7 / 16f);

                    SetPixel(ref currentWorkingBitmap, x - 1, y + 1, errorR, errorG, errorB, 3 / 16f);

                    SetPixel(ref currentWorkingBitmap, x, y + 1, errorR, errorG, errorB, 5 / 16f);

                    SetPixel(ref currentWorkingBitmap, x + 1, y + 1, errorR, errorG, errorB, 1 / 16f);

                }

            for (int j = 0; j < bmp.Height; j += 2)
            {
                for (int i = 0; i < bmp.Width; i++)
                {

                    Console.OutputEncoding = System.Text.Encoding.UTF8;
                    Color[] colourTable = grayScaleColourArray.Select(x => Color.FromArgb(x)).ToArray();
                    //char[] rList = new char[] { (char)9617, (char)9618, (char)9619, (char)9608 }; // 1/4, 2/4, 3/4, 4/4
                    int[] trackScore = new int[] { 0, 0, int.MaxValue, int.MaxValue }; //ForeColor, BackColor, Score, Score2

                    var Value = currentWorkingBitmap.GetPixel(i, j);
                    var Value2 = currentWorkingBitmap.GetPixel(i, j + 1);

                    for (int Background = 0; Background < colourTable.Length; Background++)
                    {
                        int R = colourTable[Background].R;
                        int G = colourTable[Background].G;
                        int B = colourTable[Background].B;

                        int Score = (Value2.R - R) * (Value2.R - R) + (Value2.G - G) * (Value2.G - G) + (Value2.B - B) * (Value2.B - B);


                        if (Score < trackScore[3])
                        {
                            trackScore[3] = Score;
                            trackScore[1] = Background;
                        }


                    }

                    for (int Foreground = 0; Foreground < colourTable.Length; Foreground++)
                    {
                        int R = colourTable[Foreground].R;
                        int G = colourTable[Foreground].G;
                        int B = colourTable[Foreground].B;

                        int Score = (Value.R - R) * (Value.R - R) + (Value.G - G) * (Value.G - G) + (Value.B - B) * (Value.B - B);


                        if (Score < trackScore[2])
                        {
                            trackScore[2] = Score;
                            trackScore[0] = Foreground;
                        }


                    }

                    switch(trackScore[1])
                    {
                        case 0: Console.BackgroundColor = (ConsoleColor)0;
                            break;
                        case 1: Console.BackgroundColor = (ConsoleColor)15;
                            break;
                    }

                    switch (trackScore[0])
                    {
                        case 0:
                            Console.ForegroundColor = (ConsoleColor)0;
                            break;
                        case 1:
                            Console.ForegroundColor = (ConsoleColor)15;
                            break;
                    }

                    Console.Write("\u2580");


                }
                Console.WriteLine();
            }




        }

        public static void ConsoleWriteImage3(Bitmap source)
        {
            decimal percent = Math.Min(decimal.Divide(Console.LargestWindowWidth * 2, source.Width), decimal.Divide(Console.LargestWindowHeight * 2, source.Height));
            Size size = new Size((int)(source.Width * percent), (int)(source.Height * percent));

            if (size.Width % 2 != 0)
                size.Width--;

            if (size.Height % 2 != 0)
                size.Height--;


            Bitmap bmpMax = new Bitmap(source, size.Width, size.Height);

            ConsoleWritePixels2(bmpMax);

            Console.ResetColor();
        }


        public static void ConsoleWriteImage2(Bitmap source)
        {
            decimal percent = Math.Min(decimal.Divide(Console.LargestWindowWidth * 2, source.Width), decimal.Divide(Console.LargestWindowHeight * 2, source.Height));
            Size size = new Size((int)(source.Width * percent), (int)(source.Height * percent));

            if (size.Width % 2 != 0)
                size.Width--;

            if (size.Height % 2 != 0)
                size.Height--;


            Bitmap bmpMax = new Bitmap(source, size.Width, size.Height);
            for (int i = 0; i < size.Height; i+=2)
            {
                for (int j = 0; j < size.Width; j++)
                {
                    ConsoleWritePixels(bmpMax.GetPixel(j, i), bmpMax.GetPixel(j, i + 1));
                }
                System.Console.WriteLine();
            }

            Console.ResetColor();
        }        

        public static void ConsoleWritePixel(Color Value)
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

            Console.ForegroundColor = (ConsoleColor)trackScore[0];
            Console.BackgroundColor = (ConsoleColor)trackScore[1];

            Console.Write(CharList[trackScore[2] - 1]);
        }


        public static void ConsoleWriteImage(Bitmap source)
        {
            decimal percent = Math.Min(decimal.Divide(Console.LargestWindowWidth, source.Width), decimal.Divide(Console.LargestWindowHeight, source.Height));
            Size size = new Size((int)(source.Width * percent), (int)(source.Height * percent));
            Bitmap bitmapMax = new Bitmap(source, size.Width * 2, size.Height);

            for (int i = 0; i < size.Height; i++)
            {
                for (int j = 0; j < size.Width; j++)
                {
                    ConsoleWritePixel(bitmapMax.GetPixel(j * 2, i));
                    ConsoleWritePixel(bitmapMax.GetPixel(j * 2 + 1, i));
                }
                System.Console.WriteLine();
            }

            Console.ResetColor();
        }


    }
}

