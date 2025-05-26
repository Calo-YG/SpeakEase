namespace SpeakEase.Study.Domain.Shared;

public interface ISoftDeleted
{
      /// <summary>
      /// 删除人
      /// </summary>
      public string DeleteUserId { get; set; }
      
      /// <summary>
      /// 获取或设置删除人名称
      /// </summary>
      public string DeleteUserName { get; set; }
     
      /// <summary>
      /// 是否删除
      /// </summary>
      public bool IsDeleted { get; set; }
      
      /// <summary>
      /// 当前删除时间
      /// </summary>
      public DateTime DeleteTime { get; set; }
}