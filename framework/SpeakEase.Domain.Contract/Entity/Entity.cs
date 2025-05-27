namespace SpeakEase.Domain.Contract.Entity
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Entity<T>
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public T Id { get; set; }
    }
}
