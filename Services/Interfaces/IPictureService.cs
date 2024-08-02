using Microsoft.AspNetCore.Mvc;

namespace ShopBackgroundSystem.Services.Interfaces
{
    public interface IPictureService
    {
       Task<string?> GetPictureUrlAsync(IFormFile? file,[FromServices] IWebHostEnvironment env);
    }
}
