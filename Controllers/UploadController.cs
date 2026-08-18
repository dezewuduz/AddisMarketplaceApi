using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddisMarketplaceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public UploadController(IWebHostEnvironment env)
    {
        _env = env;
    }

    // POST: api/upload/photo
    [HttpPost("photo")]
    [Authorize]
    public async Task<ActionResult<object>> UploadPhoto(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("ፋይል አልተመረጠም።");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(ext))
            return BadRequest("ፎቶ ብቻ ተፈቅዷል (jpg, png, webp)።");

        if (file.Length > 5 * 1024 * 1024)  // 10MB ገደብ
            return BadRequest("ፎቶ ከ10MB መብለጥ የለበትም።");

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
        return Ok(new { url });
    }
}