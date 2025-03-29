using SpeakEase.Domain.Shared;

namespace SpeakEase.Domain.Users
{
    /// <summary>
    /// 文件存储实体
    /// </summary>
    public class FileEntity : ICreation, IModify
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }    
        /// <summary>
        /// 是否是文件夹
        /// </summary>
        public bool Folder { get; private set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 存储名称
        /// </summary>
        public string StorageName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize { get; private set; }

        /// <summary>
        /// 父级目录id
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long ModifyById { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string ModifyByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ModifyAt { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateById { get ; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateByName { get; set; }

        protected FileEntity() { }  

        public FileEntity(long id, bool folder, string path, string name, string storageName, string type, int fileSize)
        {
            Id = id;
            Folder = folder;
            Path = path;
            Name = name;
            StorageName = storageName;
            Type = type;
            FileSize = fileSize;
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="name"></param>
        public void ReName(string name)=> this.StorageName = name;
    }
}
