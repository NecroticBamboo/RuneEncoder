// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");


using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

class RuneEncoder
{

    private const string chaosPath = @"C:\Users\andre\VSProjects\RuneEncoder\Chaos_Runes";
    private const string dawiPath = @"C:\Users\andre\VSProjects\RuneEncoder\Khazalid_Runes";
    private const string resultsPath = @"C:\Users\andre\VSProjects\RuneEncoder\Results\";



    private static void Main(string[] args)
    {
        var language = false;
        var word = "amogus".ToLower();

        var pathNumber = language ? 50 : 56;
        var folderPath = language ? dawiPath : chaosPath;

        var fileNames = Directory
            .GetFiles(folderPath)
            .SelectMany(x=>Path.GetFileNameWithoutExtension(x).ToLower().Split('_').Select(y=> (Rune: y, FileName: x)))
            .ToDictionary(x=>x.Rune,x=>x.FileName)
            ;

        // foreach (var val in fileNames)
        // {
        //     Console.WriteLine("Key: "+val.Key+" Value: "+val.Value);
        // }

        Image canvas = language ? new Image<Rgba32>(800, 100) : new Image<Rgba32>(800,50);

        var offset = 0;

        for (var i=0; i < word.Length; i++)
        {
            string? fileName = null;

            // Console.WriteLine(word[i] + "" + word[i + 1] + "" + word[i + 2]);
            if ( i <= word.Length-3 && fileNames.TryGetValue(word[i] + "" + word[i + 1] + "" + word[i + 2], out fileName))
            {
                i += 2;
            }
            
            if (fileName == null && i <= word.Length - 2 && fileNames.TryGetValue(word[i] + "" + word[i + 1], out fileName))
            {
                i += 1;
            }
            
            if (fileName == null && !fileNames.TryGetValue(word[i].ToString(), out fileName))
            {
                // throw new ApplicationException("No such rune found");
                fileNames.TryGetValue("a", out fileName);
            }
            
            Console.WriteLine(Path.Combine(folderPath, fileName));
            var img = Image.Load(fileName);
        
            canvas.Mutate(x => x
                .DrawImage(img, new Point(offset, 0), 1f)
            );
            offset += img.Width;
        }
        canvas.Save(resultsPath+word+".png");

    }
}