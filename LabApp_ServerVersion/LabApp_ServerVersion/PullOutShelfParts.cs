using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class PullOutShelfParts
    {
        public static string pulloutshelfName(int w)
        {
            string output = "";
            string w_ = w.ToString();
            if (w_.Count() == 1)
            {
                w_ = "0" + w_;
            }
            output = "SP-LC1630-" + w_;
            return output;
        }
        public static string pulloutshelfTrack(int w)
        {
            string output = "";
            if (w >= 30)
            {
                output = "DTF200-ZBFL18-STANDARD";
            }
            else
            {
                output = "DTF100-ZBFL18-STANDARD";
            }
            return output;
        }
        public static List<string> clipsandscrewsforTrack(string trackname)
        {
            List<string> output = new List<string>();
            string[] ouut = ConfigurationSettings.AppSettings[trackname].Split('-');
            output = ouut.ToList();
            return output;
        }
        public static double pulloutshelfPowder(int w, int d)
        {
            double output = w * d * 1.05 / 5760;
            return output;
        }
        public static List<List<string>> pulloutshelfParts(int w, int d)
        {
            List<List<string>> output = new List<List<string>>();

            List<string> name = new List<string>();
            List<string> num = new List<string>();
            List<string> type = new List<string>();
            name.Add(pulloutshelfName(w)); num.Add("1"); type.Add("PullOutShlef");
            string trackname = pulloutshelfTrack(w);
            List<string> clipsandScrew = clipsandscrewsforTrack(trackname);
            name.Add(clipsandScrew[0]); num.Add("2"); type.Add("Front Clip");
            name.Add(clipsandScrew[1]); num.Add("2"); type.Add("Back Clip");
            output = new List<List<string>>() { name, num, type };
            return output;
        }
        public static List<List<string>> pulloutshelfAcc(int w, int d)
        {
            List<List<string>> output = new List<List<string>>();
            List<string> name = new List<string>();
            List<string> num = new List<string>();
            List<string> type = new List<string>();

            string trackname = pulloutshelfTrack(w);
            List<string> clipsandScrew = clipsandscrewsforTrack(trackname);
            name.Add(trackname); num.Add("2"); type.Add("Track");
            name.Add("Powder"); num.Add(pulloutshelfPowder(w, d).ToString("0.00")); type.Add("Powder");

            output = new List<List<string>>() { name, num, type };
            return output;
        }
        public static List<List<string>> pulloutshelfAss()
        {
            return new List<List<string>>() {
                new List<string>(){ "Assembly" },
                new List<string>(){"2"}};
        }
        public static List<int> index012_pulloutshelf(int num, int full_w)
        {
            //index for pullout shelf pullout/load
            List<int> output = new List<int>();

            if (num > 0)
            {
                if ((full_w < 20) || ((full_w >= 20) && (num == 1)))
                {
                    output.Add(1);
                    for (int i = 1; i < num; i++)
                    {
                        output.Add(0);
                    }
                }
                else
                {
                    output.Add(1);
                    output.Add(2);
                    for (int i = 2; i < num; i++)
                    {
                        output.Add(0);
                    }
                }
            }

            return output;
        }
    }
}
