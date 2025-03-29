namespace SpeakEase.Infrastructure.Filters
{
    public class FileOption
    {
        /// <summary>
        /// 文件最大大小
        /// </summary>
        public int MaxSize { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 文件根目录
        /// </summary>
        public string RootPath { get; set; }
    }
}
