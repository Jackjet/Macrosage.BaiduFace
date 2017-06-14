using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Macrosage.Face;

namespace Macrosage.Face.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            BaiduFace client = new BaiduFace("appkey","appsecret");

            var file1 = AppDomain.CurrentDomain.BaseDirectory + "panzi.jpg";
            var file2 = AppDomain.CurrentDomain.BaseDirectory + "zhouxingchi2.jpg";

            var fileAdd = AppDomain.CurrentDomain.BaseDirectory + "yujingying1.png";


            //检测人脸
            //{"result_num":1,"result":[{"location":{"left":33,"top":45,"width":39,"height":30},"face_probability":0.95188879966736,"rotation_angle":-2,"yaw":-12.481206893921,"pitch":-1.5474016666412,"roll":-3.5593690872192}],"log_id":573119120}
            //var token = client.FaceCheck(file);


            //人脸对比
            //var result = client.FaceCompare(file1, file2);

            //人脸注册
            //result.success = true;
            //var result = client.FaceAdd(new UserFaceRegister
            //{
            //    GroupId = "macrosage1",
            //    Image = fileAdd,
            //    UserId = "131276",
            //    UserInfo = "余晶莹"
            //});

            //人脸删除
            //var result = client.FaceDelete("131277", "macrosage1");

            //人脸获取
            //var result = client.FaceGet("131276", "macrosage1");

            //组获取
            //var result = client.FaceGetGroup();

            //获取群组下的用户
            //var result = client.FaceGetGroupUsers("macrosage2", 0, 1000);

            //删除某个群组下的某个用户
            //var result = client.FaceDeleteGroupUser("131276", "macrosage1");

            //复制一个组的用户去其他组
            //var result = client.FaceAddGroupUserToOther("macrosage1", "131276", "macrosage2");

            //人脸识别
            //var result = client.FaceIdentify("macrosage1", fileAdd);

            //人脸验证
            var result = client.FaceVerify("131276", fileAdd, "macrosage1", "macrosage2");
            Console.Read();
        }
    }
}
