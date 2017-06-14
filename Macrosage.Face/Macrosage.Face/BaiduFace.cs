
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.Face
{
    /*
     * 百度人脸识别api接口请求
     * 文档地址：http://ai.baidu.com/docs#/Face-API/top
     */
    public class BaiduFace
    {
        private string _appKey;
        private string _appSecret;

        private readonly string _paramGroupId = "group_id";
        private readonly string _paramUserId = "uid";
        private readonly string _paramImage = "image";
        public BaiduFace()
        {
            _appKey = ApiConfig.APIKey;
            _appSecret = ApiConfig.SecretKey;
        }

        public BaiduFace(string appKey, string appSecret)
        {
            _appKey = appKey;
            _appSecret = appSecret;
        }

        #region 根据Token请求api
        /// <summary>
        /// 将带Token的请求封装， url?access_token=token
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tokenFunc"></param>
        /// <returns></returns>
        private string TryApiWithAccessToken(string url,Func<string, string> tokenFunc)
        {
            var tokenModel = GetAccessToken();
            if (tokenModel.error == null && tokenModel.access_token.Length > 0)
            {
                url += "?access_token=" + tokenModel.access_token;
                return tokenFunc(url);
            }
            else
            {
                return null;
            }
        }

        private T TryApiWithAccessToken<T>(string url, Func<string, T> tokenFunc) where T : class, new()
        {
            var tokenModel = GetAccessToken();
            if (tokenModel.error == null && tokenModel.access_token.Length > 0)
            {
                url += "?access_token=" + tokenModel.access_token;
                return tokenFunc(url);
            }
            else
            {
                return default(T);
            }
        }

        private string TryApiWithAccessToken(string url, Dictionary<string, object> parameter)
        {
            return TryApiWithAccessToken(url, tokenUrl =>
            {
                return RequestUtility.ExecutePost(tokenUrl, parameter);
            });
        }

        private T TryApiWithAccessToken<T>(string url, Dictionary<string, object> parameter) where T:class,new()
        {
            return TryApiWithAccessToken(url, tokenUrl =>
            {
                return RequestUtility.ExecutePost<T>(tokenUrl, parameter);
            });
        }
        #endregion

        #region 获取Token方法
        internal Token GetAccessToken(bool test=true)
        {
            if (test)
            {
                return new Token { access_token = "24.10a945dd544d86ffb41b13c03ffb8bd5.2592000.1500010731.282335-9765064", expires_in = 259200 };
            }
            var token = RequestUtility.ExecutePost<Token>("oauth/2.0/token", new Dictionary<string, object> {
                { "grant_type","client_credentials"},
                { "client_id",_appKey },
                { "client_secret",_appSecret}
            });
            return token;
        }
        #endregion

        #region 图像信息转换
        private string ToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            string base64 = Convert.ToBase64String(imageBytes);

            return base64;
        }
        #endregion

        #region 人脸信息检测
        public string FaceCheck(string imagePath, FaceOptions options = null)
        {
            if (options == null) { options = new FaceOptions(); }

            string url = "/rest/2.0/face/v1/detect";

            string base64 = ToBase64(imagePath);

            return TryApiWithAccessToken(url, new Dictionary<string, object>
            {
                { _paramImage,base64},
                { "face_fields",options.ToString()}
            });
        }
        #endregion

        #region 人脸比对
        /// <summary>
        /// 人脸比对
        /// </summary>
        /// <param name="imagePath1">第一张图</param>
        /// <param name="imagePath2">第二张图</param>
        /// <returns></returns>
        public string FaceCompare(string imagePath1, string imagePath2,bool returnExt=false)
        {
            var url = "/rest/2.0/face/v2/match";

            string base641 = ToBase64(imagePath1);
            string base642 = ToBase64(imagePath2);

            return TryApiWithAccessToken(url, new Dictionary<string, object>
            {
                { "images",$"{base641},{base642}"},
                { "ext_fields",returnExt?"qualities":""},
                { "image_liveness",""}
            });
        }
        #endregion

        #region 人脸识别
        public FaceGetUserIdentifyResult FaceIdentify(string groupId, string image)
        {
            var url = "/rest/2.0/face/v2/identify";

            return TryApiWithAccessToken<FaceGetUserIdentifyResult>(url, new Dictionary<string, object> {
                { _paramGroupId,groupId },
                { _paramImage,ToBase64(image) }
            });
        }
        #endregion

        #region 人脸认证
        public FaceVerifyResult FaceVerify(string userId, string image, params string[] groupIds)
        {
            var url = "/rest/2.0/face/v2/verify";

            return TryApiWithAccessToken<FaceVerifyResult>(url, new Dictionary<string, object>
            {
                { _paramUserId,userId },
                { _paramImage,ToBase64(image) },
                { _paramGroupId,string.Join(",",groupIds) }
            });
        }
        #endregion

        #region 人脸注册或者更新
        private FaceResult FaceAddOrUpdate(UserFaceRegister userInfo, bool isAdd)
        {
            var url = $"/rest/2.0/face/v2/faceset/user/{(isAdd ? "add" : "update")}";

            if (string.IsNullOrEmpty(userInfo.Image))
            {
                return new FaceResult
                {
                    error_code = "-1",
                    error_msg = "无图片信息"
                };
            }

            userInfo.ImageBase64 = ToBase64(userInfo.Image);

            if (!isAdd)
            {
                userInfo.ActionType = ActionType.Replace;
            }

            FaceResult result = TryApiWithAccessToken<FaceResult>(url, userInfo.ToRequestParameter());

            return result;
        }

        public FaceResult FaceAdd(UserFaceRegister userInfo)
        {
            return FaceAddOrUpdate(userInfo, true);
        }

        public FaceResult FaceUpdate(UserFaceRegister userInfo)
        {
            return FaceAddOrUpdate(userInfo, false);
        }

        public FaceResult FaceAdd(string userId, string image,string groupId, string userInfo = "none")
        {
            return FaceAdd(new UserFaceRegister
            {
                Image = image,
                UserId = userId,
                GroupId = groupId,
                UserInfo = userInfo
            });
        }

        public FaceResult FaceUpdate(string userId, string image, string groupId, string userInfo = "none")
        {
            return FaceUpdate(new UserFaceRegister
            {
                Image = image,
                UserId = userId,
                GroupId = groupId,
                UserInfo = userInfo
            });
        }
        #endregion

        #region 人脸删除
        public FaceResult FaceDelete(string userId,string groupId)
        {
            var url = $"/rest/2.0/face/v2/faceset/user/delete";

            FaceResult result = TryApiWithAccessToken<FaceResult>(url, new Dictionary<string, object>
            {
                { _paramUserId,userId},
                { _paramGroupId,groupId}
            });

            return result;
        }
        #endregion

        #region 获取用户信息
        public FaceGetResult FaceGet(string userId, string groupId)
        {
            var url = "/rest/2.0/face/v2/faceset/user/get";

            return TryApiWithAccessToken<FaceGetResult>(url, new Dictionary<string, object>
            {
                {_paramUserId,userId },
                { _paramGroupId,groupId }
            });
        }
        #endregion

        #region 获取组信息

        public FaceGetGroupResult FaceGetGroup()
        {
            return FaceGetGroup(0, 1000);
        }
        public FaceGetGroupResult FaceGetGroup(uint start, uint num)
        { 
            var url = "/rest/2.0/face/v2/faceset/group/getlist";

            return TryApiWithAccessToken<FaceGetGroupResult>(url, new Dictionary<string, object>
            {
                { "start",start},
                { "num",num}
            });
        }
        #endregion

        #region 获取群组用户信息

        public FaceGetGroupUsersResult FaceGetGroupUsers(string groupId, uint start, uint num)
        {
            var url = "/rest/2.0/face/v2/faceset/group/getusers";

            return TryApiWithAccessToken<FaceGetGroupUsersResult>(url, new Dictionary<string, object> {
                { _paramGroupId,groupId},
                { "start",start},
                { "num",num}
            });
        }
        #endregion

        #region 删除某个组内的用户信息
        public FaceResult FaceDeleteGroupUser(string userId, string groupId)
        {
            var url = "/rest/2.0/face/v2/faceset/group/deleteuser";

            return TryApiWithAccessToken<FaceResult>(url, new Dictionary<string, object> {
                { _paramGroupId,groupId},
                { _paramUserId,userId}
            });
        }
        #endregion

        #region 复制一个组的用户到其他组
        public FaceResult FaceAddGroupUserToOther(string fromGroupId, string userId, params string[] groupIds)
        {
            var url = "/rest/2.0/face/v2/faceset/group/adduser";

            return TryApiWithAccessToken<FaceResult>(url, new Dictionary<string, object>
            {
                { _paramGroupId,string.Join(",",groupIds)},
                { _paramUserId,userId},
                { "src_group_id",fromGroupId}
            });
        }
        #endregion

        
    }
}
