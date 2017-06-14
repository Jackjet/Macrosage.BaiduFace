using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.Face
{
    public class FaceResult
    {
        public FaceResult() { }
        public string error_code { get; set; }
        public string error_msg { get; set; }

        public long log_id { get; set; }

        
        public bool success { get { return string.IsNullOrEmpty(error_code); } }
    }

    public class FaceReturnResult : FaceResult
    {
        public int result_num { get; set; }
    }

    /// <summary>
    /// 单个用户
    /// </summary>
    public class SingleUserInfo
    {
        public string uid { get; set; }
        public string group_id { get; set; }
        public string user_info { get; set; }
    }

    /// <summary>
    /// 用户识别带分数
    /// </summary>
    public class SingleUserScoreInfo : SingleUserInfo
    {
        public List<double> scores { get; set; }
    }

    /// <summary>
    /// 获取单个用户信息结果
    /// </summary>
    public class FaceGetResult : FaceReturnResult
    {
        public SingleUserInfo result { get; set; }
    }

    public class FaceVerifyResult : FaceReturnResult
    {
        public List<double> result { get; set; }
    }

    /// <summary>
    /// 组结果
    /// </summary>
    public class FaceGetGroupResult : FaceReturnResult
    {
        public List<string> result { get; set; }
    }

    /// <summary>
    /// 用户组内信息结果
    /// </summary>
    public class FaceGetGroupUsersResult : FaceReturnResult
    {
        public List<SingleUserInfo> result { get; set; }
    }

    /// <summary>
    /// 用户识别结果
    /// </summary>
    public class FaceGetUserIdentifyResult : FaceReturnResult
    {
        public List<SingleUserScoreInfo> result { get; set; }
    }
}
