namespace app1.Helper
{
    public static class MyUtil
    {
        public static string UploadHinh(IFormFile Hinh, string folder)
        {
			try
			{
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", folder, Hinh.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Hinh.CopyToAsync(myfile);
                }return Hinh.FileName;
            }
			catch (Exception ex)
			{
                return string.Empty;
			}
        }
    }
}
