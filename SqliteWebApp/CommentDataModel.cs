
namespace AndroidModel.douyin
{
    /// <summary>
    /// 评论页采集评论用模型
    /// </summary>
    public class CommentDataModel
    {
        /// <summary>
        /// 序号
        /// </summary>

        public string Number { get; set; }

        /// <summary>
        /// 短ID
        /// </summary>

        public string ShortId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>

        public string NickName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>

        public string Phone { get; set; }

        /// <summary>
        /// 用户主页Url
        /// </summary>

        public string ShareUrl { get; set; }

        /// <summary>
        /// 性别
        /// </summary>

        public string Gender { get; set; }

        /// <summary>
        /// 签名
        /// </summary>

        public string Signature { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>

        public string CommentText { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>

        public string CreateTime { get; set; }

        /// <summary>
        /// 本条点赞数
        /// </summary>

        public string DiggCount { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>

        public string UpTime { get; set; }
    }
}
