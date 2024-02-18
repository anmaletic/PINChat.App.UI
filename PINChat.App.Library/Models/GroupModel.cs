using Avalonia.Media.Imaging;
using SkiaSharp;

namespace PINChat.App.Library.Models;

public class GroupModel : TargetModel
{
    public string? Name { get; set; }
    public List<UserModel> Contacts { get; set; } = new ();
    public byte[]? Avatar { get; set; }  
    
    public Bitmap? AvatarBitmap
    {
        get
        {
            if (Avatar is null) return CreateInitialsBitmap($"{Name![0]}");
            using var memory = new MemoryStream(Avatar);
            return new Bitmap(memory);
        }
    }
    
    private Bitmap CreateInitialsBitmap(string initials)
    {
        var info = new SKImageInfo(100, 100); // Set the size of the bitmap
        var bitmap = new SKBitmap(info);
    
        using (var surface = SKSurface.Create(info, bitmap.GetPixels(), info.RowBytes))
        {
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.LightSlateGray); // Set background color
        
            // Create a paint object for the text
            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.White; // Set text color
                paint.TextAlign = SKTextAlign.Center;
                paint.TextSize = 36;
                paint.FakeBoldText = true;

                // Calculate the position to center the text on the bitmap
                var x = info.Width / 2;
                var y = (info.Height + paint.TextSize / 2) / 2;

                // Draw the text on the bitmap
                canvas.DrawText(initials, x, y, paint);
            }
        }
        
        using (var image = SKImage.FromBitmap(bitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 300))
        using (var stream = data.AsStream())
        {
            // Create an Avalonia Bitmap from the stream
            return new Bitmap(stream);
        }
    }
}