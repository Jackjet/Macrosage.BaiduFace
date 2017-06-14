using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.Face
{
    public class UserFaceRegister
    {
        public UserFaceRegister()
        {
            ActionType = ActionType.Replace;
        }
        public string UserId { get; set; }
        public string UserInfo { get; set; }
        public string GroupId { get; set; }
        public string Image { get; set; }

        public string ImageBase64 { get; internal set; }

        public ActionType ActionType { get; set; }

        internal Dictionary<string, object> ToRequestParameter()
        {
            return new Dictionary<string, object>
            {
                { "uid",UserId},
                { "user_info",UserInfo},
                { "group_id",GroupId},
                { "image",ImageBase64},
                { "action_type",ActionType == ActionType.Append ? "append" : "replace" }
            };
        }
    }

    public enum ActionType
    {
        Append,
        Replace
    }
}
