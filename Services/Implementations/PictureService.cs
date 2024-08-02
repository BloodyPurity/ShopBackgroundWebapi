using Microsoft.AspNetCore.Mvc;
using ShopBackgroundSystem.Services.Interfaces;

namespace ShopBackgroundSystem.Services.Implementations
{
    public class PictureService : IPictureService
    {
        public async Task<string?> GetPictureUrlAsync(IFormFile? file, [FromServices] IWebHostEnvironment env)
        {
            string? fileName = null;
            //检查文件大小
            if (file != null && file.Length != 0)
            {
                //获取文件的MIME类型
                var mimeType = file.ContentType;
                // 定义一些常见的图片MIME类型  
                var imageMimeTypes = new List<string>
                {
                     "image/jpeg",
                     "image/png",
                     "image/webp"
                     // 可以根据需要添加其他图片MIME类型
                     //,"image/bmp"
                     //,"image/tiff"
                     //, 
                 };
                // 检查文件的MIME类型是否在图片MIME类型列表中
                if (!imageMimeTypes.Contains(mimeType))
                {
                    return null;
                }
                //拼接上传的文件路径
                string fileExt = Path.GetExtension(file.FileName);         //获取文件扩展名
                fileName = Guid.NewGuid().ToString("N") + fileExt;  //生成全球唯一文件名
                string relPath = Path.Combine(@"\uploads", fileName);       //拼接相对路径
                string fullPath = Path.Combine(env.ContentRootPath, "Uploads", fileName);  //拼接绝对路径

                //创建文件
                using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fs); //把上传的文件file拷贝到fs里
                    await fs.FlushAsync();      //刷新fs文件缓存区
                };
            }
            return fileName;
        }
    }
}
