using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace ConsoleGraphics
{
    enum DrawingMethod
    {
        One,
        Two,
        Three,
        Gif
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            string option = "";
            
            while (option != "9") try
            {
                    Menu();
            }
            catch { }
        }

        static void DisplayTitle()
        {
            var title = "Bitmap To Console";

            foreach (var character in title)
            {
                Console.Write('-');
            }

            Console.WriteLine();

            Console.WriteLine(title);

            foreach (var character in title)
            {
                Console.Write('-');
            }

            Console.WriteLine();
        }

        static void Menu()
        {
            string option = " ";

            while (option != "1" && option != "2" && option != "3" && option != "4" && option != "9")
            {
                Console.Clear();

                DisplayTitle();

                option = GetMenuOption();
            }

            Console.Clear();

            switch(option)
            {
                case "1": DrawImage(DrawingMethod.One);
                        break;
                case "2": DrawImage(DrawingMethod.Two);
                    break;
                case "3": DrawImage(DrawingMethod.Three);
                    break;
                case "4": DrawImage(DrawingMethod.Gif);
                    break;
                case "9": Environment.Exit(0);
                    break;
            }
        }

        static string GetMenuOption()
        {
            Console.WriteLine("1. Convert Image using Method 1 (with dithering).");
            Console.WriteLine("2. Convert Image using Method 2 (without dithering).");
            Console.WriteLine("3. Convert Image using Method 3 (pixel dithering).");
            Console.WriteLine("4. Convert GIF.");
            Console.WriteLine("9. Quit.");

            Console.Write("Enter an option: ");

            return Console.ReadLine();
        }

        static string GetStringInput()
        {
            DisplayTitle();

            Console.Write("Enter the address of the image: " );

            var address = Console.ReadLine();

            Console.Clear();

            return address;
        }

        static void DrawImage(DrawingMethod drawingMethod)
        {
            Console.SetCursorPosition(0, 0);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            var imageAddress = GetStringInput();
            

            Console.Clear();

            switch(drawingMethod)
            {
                case DrawingMethod.One: ConsoleImage.ConsoleWriteImage(new Bitmap(imageAddress));
                    break;
                case DrawingMethod.Two: ConsoleImage.ConsoleWriteImage2(new Bitmap(imageAddress));
                    break;
                case DrawingMethod.Three: ConsoleImage.ConsoleWriteImage3(new Bitmap(imageAddress));
                    break;
                case DrawingMethod.Gif: ConsoleGif.ConsoleWriteAnimatedGif(imageAddress);
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadLine();
        }




    }



      
}
