using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.Face
{
    //
    public class FaceOptions
    {

        public FaceOptions()
        {
            //默认 年龄，面貌，性别，表情 打开
            Age = true;
            Beauty = true;
            Gender = true;
            Expression = true;
        }

        public FaceOptions All()
        {
            FaceShape = true;
            Glasses = true;
            LandMark = true;
            Race = true;
            Qualities = true;
            return this;
        }
        public bool Age { get; set; }
        public bool Beauty { get; set; }
        public bool Expression { get; set; }
        public bool FaceShape { get; set; }
        public bool Gender { get; set; }
        public bool Glasses { get; set; }
        public bool LandMark { get; set; }
        public bool Race { get; set; }
        public bool Qualities { get; set; }


        public override string ToString()
        {
            List<string> fields = new List<string>();
            //age,beauty,expression,faceshape,gender,glasses,landmark,race,qualitie

            if (Age) {
                fields.Add("age");
            }
            if (Beauty)
            {
                fields.Add("beauty");
            }
            if (Expression)
            {
                fields.Add("expression");
            }
            if (FaceShape)
            {
                fields.Add("faceshape");
            }
            if (Gender)
            {
                fields.Add("gender");
            }
            if (Glasses)
            {
                fields.Add("glasses");
            }
            if (LandMark)
            {
                fields.Add("landmark");
            }
            if (Race)
            {
                fields.Add("race");
            }
            if (Qualities)
            {
                fields.Add("qualities");
            }
            return string.Join(",", fields.ToArray());
        }
    }
}
