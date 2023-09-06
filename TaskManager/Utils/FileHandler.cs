using System.Text.RegularExpressions;

namespace Utils
{
    public class FileHandler
    {
        private const string PATH_PATTERN = @"^([A-Za-z]:\/{2}|\.\/)(\w+\/)*\w+\.\w+$";
        private string _path;

        public FileHandler(string path)
        {
            if (!Regex.IsMatch(path, PATH_PATTERN))
                throw new ArgumentException("Invalid path provided.");
            this._path = path;
        }

        public void Append(string text)
        {
            File.AppendAllText(this._path, text);
        }

        public void Write(string text)
        {
            File.WriteAllText(this._path, text);
        }

        public string Read()
        {
            return File.ReadAllText(this._path);
        }
    }
}
