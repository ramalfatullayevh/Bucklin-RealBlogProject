namespace Bucklin.Helpers
{
    public static class FileExtension
    {
        //Check file Type
        public static bool CheckFileType(this IFormFile file, string type) => file.ContentType.Contains(type);


        //Check File Size
        public static bool CheckFileSize(this IFormFile file, int kb) => kb * 1024 > file.Length;


        //Change File Name
        static string ChangeFileName(string oldName)
        {
            string extension = oldName.Substring(oldName.LastIndexOf('.'));
            if (oldName.Length < 32)
            {
                oldName = oldName.Substring(0, oldName.IndexOf('.'));
            }
            else
            {
                oldName = oldName.Substring(0, 31);
            }
            string newName = Guid.NewGuid() + oldName + extension;
            return newName;
        }


        //Save File
        public static string SaveFile(this IFormFile file, string path)
        {
            string fileName = ChangeFileName(file.FileName);
            using (FileStream fs = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return fileName;
        }


        //Delete File
        public static void DeleteFile(this string file, string root, string folder)
        {
            string path = Path.Combine(root, folder, file);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }


        //Check Validate Extension Method
        public static string CheckValidate(this IFormFile file, string type, int kb)
        {
            string result = "";
            if (!CheckFileType(file, type))
            {
                result += $"{file.FileName} file type must be {type}.";
            }
            if (!CheckFileSize(file, kb))
            {
                result += $"{file.FileName} file memory must be {kb} kilobayt";
            }
            return result;
        }
    }
}
