using PINChat.App.Library.Models;
using SkiaSharp;

namespace PINChat.App.Library.Api;

public class GroupEndpoint : IGroupEndpoint
{    
    private readonly IApiHelper _apiHelper;

    public GroupEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }
    
    public async Task<List<GroupModel>> GetAll()
    {
        using var response = await _apiHelper.ApiClient.GetAsync("api/Group/GetAll");

        if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        
        var result = await response.Content.ReadAsAsync<List<GroupModel>>();
        return result;
    }

    public async Task<string> Create(GroupModel group)
    {
        group.Avatar ??= GenerateImageFromInitials($"{group.Name![0]}");
        
        using var response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Group/Insert", group);
        
        if(!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
    
    public async Task<string> Update(GroupModel group)
    {
        using var response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Group/Update", group);
        
        if(!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
    
    private byte[] GenerateImageFromInitials(string? name)
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
                var x = info.Width / 2 - 2;
                var y = (info.Height + paint.TextSize / 2) / 2 + 2;

                // Draw the text on the bitmap
                canvas.DrawText(name, x, y, paint);
            }
        }
        
        using (var image = SKImage.FromBitmap(bitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 300))
        using (var stream = data.AsStream())
        {
            // Get the byte array from the encoded data
            return data.ToArray();
        }
    }
}