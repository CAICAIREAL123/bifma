using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class ShellParts
    {
        public static List<string> sideleft(string t, int w, int h, int d)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC10" + h.ToString() + "-L");
            }
            else if (t == "2")
            {
                output.Add("H-LC10" + h.ToString() + "-L");
            }
            else if (t == "3")
            {
                output.Add("NM-LC10" + h.ToString() + "-L");
            }
            else if (t == "4")
            {
                output.Add("M-LC10" + h.ToString() + "-L");
            }
            else if (t == "5")
            {
                output.Add("MNM-LC10" + h.ToString() + "-L");
            }
            else if (t == "6")
            {
                output.Add("WC30" + h.ToString() + "-" + d.ToString() + "-L");
            }
            else if (t == "7")
            {
                output.Add("SC40" + h.ToString() + "-" + d.ToString() + "-L");
            }
            else if (t == "8")
            {
                output.Add("A-SC40" + h.ToString() + "-" + d.ToString() + "-L");
            }
            else if (t == "9")
            {
                output.Add("LC20" + h.ToString() + "-" + "L");
            }
            return output;
        }
        public static List<string> sideright(string t, int w, int h, int d)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC10" + h.ToString() + "-R");
            }
            else if (t == "2")
            {
                output.Add("H-LC10" + h.ToString() + "-R");
            }
            else if (t == "3")
            {
                output.Add("NM-LC10" + h.ToString() + "-R");
            }
            else if (t == "4")
            {
                output.Add("M-LC10" + h.ToString() + "-R");
            }
            else if (t == "5")
            {
                output.Add("MNM-LC10" + h.ToString() + "-R");
            }
            else if (t == "6")
            {
                output.Add("WC30" + h.ToString() + "-" + d.ToString() + "-R");
            }
            else if (t == "7")
            {
                output.Add("SC40" + h.ToString() + "-" + d.ToString() + "-R");
            }
            else if (t == "8")
            {
                output.Add("A-SC40" + h.ToString() + "-" + d.ToString() + "-R");
            }
            else if (t == "9")
            {
                output.Add("LC20" + h.ToString() + "-" + "R");
            }
            return output;
        }
        public static List<string> backpanel(string t, int w, int h, int d)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC11" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "2")
            {
                output.Add("HT-LC11" + h.ToString() + "-" + w.ToString());
                //output.Add("HB-LC11" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "3")
            {
                output.Add("HB-LC11" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "4")
            {
                output.Add("M-LC11" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "5")
            {
                output.Add("HC-LC11" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "6")
            {
                output.Add("WC11" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "7")
            {
                output.Add("A-LC11" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "8")
            {
                output.Add("LC11" + (h - 6).ToString() + "-" + w.ToString());
            }
            else if (t == "9")
            {
                output.Add("VAC-LC11" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "B")
            {
                output.Add("LC1220-" + w.ToString());
            }
            else if (t == "C")
            {
                output.Add("SC41" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "D")
            {
                output.Add("LC33" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "E")
            {
                output.Add("LC334");
            }
            else if (t == "F")
            {
                output.Add("LC21" + h.ToString() + "-" + w.ToString());
            }
            else if (t == "G")
            {
                output.Add("LC1150-" + w.ToString());
            }
            return output;
        }
        public static List<string> top(string t, int w, int h, int d)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC1230-" + w.ToString());
            }
            else if (t == "2")
            {
                output.Add("H-LC1230-" + w.ToString());
            }
            else if (t == "3")
            {
                output.Add("NM-LC1230-" + w.ToString());
            }
            else if (t == "4")
            {
                output.Add("M-LC1230-" + w.ToString());
            }
            else if (t == "5")
            {
                output.Add("MNM-LC1230-" + w.ToString());
            }
            else if (t == "6")
            {
                //output="WC12" + w.ToString() + "-" + d.ToString();
                //output="WC12" + w.ToString() + "-" + d.ToString();
                output.Add("WC12" + w.ToString() + "-" + d.ToString());
            }
            else if (t == "8")
            {
                output.Add("VACTC-LC1240-" + w.ToString());
            }
            else if (t == "9")
            {
                output.Add("FH-LC1230-" + w.ToString());
            }
            else if (t == "A")
            {
                output.Add("SC4230-" + w.ToString() + "-" + d.ToString());
            }
            else if (t == "B")
            {
                output.Add("FOHM-SC4230-" + w.ToString() + "-" + d.ToString());
            }
            else if (t == "C")
            {
                output.Add("O-SC4230-" + w.ToString() + "-" + d.ToString());
            }
            else if (t == "D")
            {
                output.Add("WC3260-" + w.ToString());
            }

            return output;
        }
        public static List<string> toe(string t, int w, int h)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC1260-" + w.ToString());
            }
            else if (t == "2")
            {
                output.Add("H-LC1260-" + w.ToString());
            }
            else if (t == "3")
            {
                output.Add("NM-LC1260-" + w.ToString());
            }
            else if (t == "4")
            {
                output.Add("VAC-LC1260-" + w.ToString());
            }
            else if (t == "5")
            {
                output.Add("SC4260-" + w.ToString());
            }
            else if (t == "6")
            {
                output.Add("LC335-" + w.ToString());
            }
            return output;
        }
        public static List<string> runnersupport(string t, int h)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC13" + h.ToString());
                output.Add("LC13" + h.ToString());
                //output.Add("LC13" + h.ToString());
            }
            else if (t == "3")
            {
                output.Add("NM-LC13" + h.ToString());
                output.Add("NM-LC13" + h.ToString());
                //output.Add("NM-LC13" + h.ToString());
            }
            else if (t == "2")
            {
                output.Add("VAC-LC13" + h.ToString());
                output.Add("VAC-LC13" + h.ToString());
            }

            return output;
        }
        public static List<string> gusset(string t)
        {
            List<string> output = new List<string>();
            if (t == "1")
            {
                output.Add("LC1270");
                output.Add("LC1270");
                output.Add("LC1270");
                output.Add("LC1270");
            }
            else if (t == "2")
            {
                output.Add("LC1270G");
                output.Add("LC1270G");
                output.Add("LC1270G");
                output.Add("LC1270G");
            }
            else if (t == "3")
            {
                output.Add("LC1270H");
                output.Add("LC1270H");
                output.Add("LC1270H");
                output.Add("LC1270H");
            }
            return output;
        }
        public static List<string> shelf(string t, int dow, int d)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC1630-" + dow.ToString());
            }
            else if (t == "2")
            {
                output.Add("LC3630-" + dow.ToString());
            }
            else if (t == "3")
            {
                output.Add("NM-LC1630-" + dow.ToString());
            }
            else if (t == "4")
            {
                output.Add("VACB-LC1630-" + dow.ToString());
            }
            else if (t == "6")
            {
                output.Add("WC16" + dow.ToString() + "-" + d.ToString());
            }
            else if (t == "7")
            {
                output.Add("SC4630-" + dow.ToString() + "-" + d.ToString());
            }
            else if (t == "8")
            {
                output.Add("SC4631-" + dow.ToString() + "-" + d.ToString());
            }
            return output;
        }
        public static List<string> intermedshelf(int dow, int d)
        {
            return shelf("8", dow, d);
        }
        public static List<string> removableback(string t, int w, int h)
        {
            List<string> output = new List<string>();

            if (t == "1")
            {
                if (w >= 30)
                {
                    output.Add("LC17" + h.ToString() + "-" + (w / 2).ToString());
                    output.Add("LC17" + h.ToString() + "-" + (w / 2).ToString());
                }
                else
                {
                    output.Add("LC17" + h.ToString() + "-" + (w).ToString());
                }
            }
            else if (t == "3")
            {
                output.Add("NM-LC17" + h.ToString() + "-" + w.ToString());
            }
            return output;
        }
        public static List<string> bottom(string t, int dow, int doh)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC1250-" + dow.ToString());
            }
            else if (t == "2")
            {
                output.Add("H-LC1250-" + dow.ToString());
            }
            else if (t == "3")
            {
                output.Add("NM-LC1250-" + dow.ToString());
            }
            else if (t == "4")
            {
                output.Add("M-LC1250-" + dow.ToString());
            }
            else if (t == "5")
            {
                output.Add("MNM-LC1250-" + dow.ToString());
            }
            else if (t == "6")
            {
                output.Add("SC4250-" + dow.ToString() + "-" + dow.ToString());
            }
            else if (t == "7")
            {
                output.Add("SC4250-" + dow.ToString() + "-" + dow.ToString() + "-A");
            }
            else if (t == "8")
            {
                output.Add("LC334");
            }
            else if (t == "9")
            {
                output.Add("VAC-LC1250-" + dow.ToString());
            }
            else if (t == "A")
            {
                output.Add("LC33SC-" + dow.ToString());
            }
            else if (t == "B")
            {
                output.Add("LC2250-" + dow.ToString());
            }
            return output;
        }
        public static List<string> stileside(string t, int h, int doorindex)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("WC3150-" + h.ToString());
                output.Add("WC3150-" + h.ToString());
            }
            else if (t == "2")
            {
                if ((doorindex == 1) || (doorindex == 2))
                {
                    output.Add("NM-WC3150-" + h.ToString());
                    output.Add("BNM-WC3150-" + h.ToString());
                }
                else
                {
                    output.Add("NM-WC3150-" + h.ToString());
                    output.Add("NM-WC3150-" + h.ToString());
                }
            }
            else if (t == "3")
            {
                output.Add("SC4150-" + h.ToString());
                output.Add("SC4150-" + h.ToString());
            }
            else if (t == "4")
            {
                output.Add("Z-SC4150-" + h.ToString());
                output.Add("Z-SC4150-" + h.ToString());
            }
            else if (t == "6")
            {
                if ((doorindex == 1) || (doorindex == 2))
                {
                    output.Add("FO-SC4150-" + h.ToString());
                    output.Add("FOB-SC4150-" + h.ToString());
                }
                else
                {
                    output.Add("FO-SC4150-" + h.ToString());
                    output.Add("FO-SC4150-" + h.ToString());
                }
            }
            return output;

        }
        public static List<string> stiletop(string t, int w)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("WC3210-" + w.ToString());
            }
            return output;
        }
        public static List<string> stilebottom(string t, int w)
        {
            List<string> output = new List<string>();
            if (t == "0") { }
            else if (t == "1")
            {
                output.Add("WC3130-" + w.ToString());

            }
            return output;
        }
        public static List<string> front(string t, int w)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC1240-" + w.ToString());
            }
            else if (t == "2")
            {
                output.Add("VAC-LC1240-" + w.ToString());
            }
            else if (t == "3")
            {
                output.Add("H-LC1240-" + w.ToString());
            }
            return output;
        }
        public static List<string> hiddenrail(string t, int dow, int doh)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC1280-" + dow.ToString());
            }
            else if (t == "3")
            {
                output.Add("NM-LC1280-" + dow.ToString());
            }


            return output;
        }
        public static List<string> partition(string t, int dow, int doh)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC1400-" + doh.ToString());
            }
            else if (t == "2")
            {
                output.Add("HNM-LC1400-" + doh.ToString());
            }
            else if (t == "3")
            {
                output.Add("H-LC1400-" + doh.ToString());
            }

            return output;
        }
        public static List<string> centerpost(string t, int length)
        {
            string length1 = "";
            if (length < 10)
            {
                length1 = "0" + length.ToString();
            }
            else
            {
                length1 = length.ToString();
            }
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                if ((length == 24) || (length == 27) || (length == 30))
                {
                    output.Add("LC13" + length1);
                    output.Add("LC13" + length1);
                }
                else
                {
                    output.Add("LC13" + length1 + "-in");
                }
                output.Add("LC13" + length1 + "-out");
            }
            else if (t == "2")
            {
                output.Add("H-LC13" + length1 + "-out");
            }
            else if (t == "3")
            {
                if ((length == 24) || (length == 27) || (length == 30))
                {
                    output.Add("NM-LC13" + length1);
                    output.Add("NM-LC13" + length1);
                }
                else
                {
                    output.Add("NM-LC13" + length1 + "-in");
                }
                output.Add("NM-LC13" + length1 + "-out");
            }
            return output;
        }
        public static List<string> securitypanel(string t, int length)
        {
            List<string> output = new List<string>();
            if (t == "0")
            {

            }
            else if (t == "1")
            {
                output.Add("LC1460-" + length.ToString());
            }
            return output;
        }
        public static List<string> selfclosepackage(string t, int w)
        {
            List<string> output = new List<string>();
            //if (t == "0")
            //{ }
            //else if (t == "1")
            //{
            //    if (w >= 30)
            //    {
            //        output.Add("LC338SC-E");
            //        output.Add("LC338SC-E");
            //        output.Add("LC338SC-C");
            //        output.Add("LC338SC-C");
            //        output.Add("LC338SC-D1");
            //        output.Add("LC338SC-D1");
            //        output.Add("LC338SC-H_G");
            //        output.Add("LC338SC-F");
            //        output.Add("LC338SC-D");
            //        output.Add("LC338SC-D");
            //        output.Add("LC338SC-CSS");
            //        output.Add("LC338SC-CSS");
            //        output.Add("LC338SC-A");
            //        output.Add("LC338SC-A");
            //    }
            //    else
            //    {
            //        output.Add("LC338SC-E");
            //        output.Add("LC338SC-C");
            //        output.Add("LC338SC-D1");
            //        output.Add("LC338SC-D");
            //        output.Add("LC338SC-A");
            //    }
            //}
            return output;
        }

        public static double powder_sec(int w, int d)
        {
            return w * d * 2 * 1.05 / 5760;
        }
        public static double powder_part(int h, int d)
        {
            return h * d * 2 * 1.05 / 5760;
        }
        public static double powder_shell(int w, int h, int d)
        {
            return 2 * (w * h + 2 * w * d + 2 * d * h) * 1.05 / 5760;
        }
        public static double powder_shelf(int w, int d)
        {
            return w * d * 2 * 1.05 / 5760;
        }

        public static List<List<int>> hidrailindex(List<int> doorlist_h, List<int> doorlist_x, List<int> doorlist_w, int h_, List<int> drawer_w, List<int> drawer_y, string model, int w, List<int> drawer_l, List<int> drawer_x, List<int> drawer_h)
        {
            List<List<int>> output = new List<List<int>>();

            int h = 0;
            h += h_;

            for (int i = 0; i < doorlist_h.Count; i++)
            {
                if (doorlist_h[i] != h)
                {
                    int tempy = h - doorlist_h[i];
                    int tempx = doorlist_x[i];
                    int templ = doorlist_w[i];
                    int j = 0;
                    for (j = 0; j < output.Count; j++)
                    {
                        if (output[j][0] == tempy)
                        {
                            break;
                        }
                    }
                    if (j == output.Count)
                    {
                        j = 0;
                        for (j = 0; j < output.Count; j++)
                        {
                            if (tempy < output[j][0])
                            {
                                break;
                            }
                        }
                        output.Insert(j, new List<int>() { tempy, tempx, templ });
                    }
                    else
                    {
                        if (tempx != output[j][1])
                        {
                            output[j][1] = 0;
                            output[j][2] = w;
                        }
                    }
                }
            }

            for (int i = drawer_y.Count - 1; i > 0; i--)
            {
                if (drawer_w[i] == w)
                {
                    if (drawer_w[i - 1] != w)
                    {
                        if (drawer_y[i] != 0)
                        {
                            int tempy = drawer_y[i];
                            int tempx = 0;
                            int templ = w;
                            int j = 0;
                            for (j = 0; j < output.Count; j++)
                            {
                                if (output[j][0] == tempy)
                                {
                                    break;
                                }
                            }
                            if (j == output.Count)
                            {
                                j = 0;
                                for (j = 0; j < output.Count; j++)
                                {
                                    if (tempy < output[j][0])
                                    {
                                        break;
                                    }
                                }
                                output.Insert(j, new List<int>() { tempy, tempx, templ });
                            }
                            else
                            {
                                if (tempx != output[j][1])
                                {
                                    if ((templ == w) || (output[j][2] == w))
                                    {
                                        output[j][1] = 0;
                                        output[j][2] = w;
                                    }
                                    else
                                    {
                                        j = 0;
                                        for (j = 0; j < output.Count; j++)
                                        {
                                            if (tempy < output[j][0])
                                            {
                                                break;
                                            }
                                        }
                                        output.Insert(j, new List<int>() { tempy, tempx, templ });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (drawer_l[i] != 0)
                        {
                            if (drawer_y[i] != 0)
                            {
                                int tempy = drawer_y[i];
                                int tempx = 0;
                                int templ = w;
                                int j = 0;
                                for (j = 0; j < output.Count; j++)
                                {
                                    if (output[j][0] == tempy)
                                    {
                                        break;
                                    }
                                }
                                if (j == output.Count)
                                {
                                    j = 0;
                                    for (j = 0; j < output.Count; j++)
                                    {
                                        if (tempy < output[j][0])
                                        {
                                            break;
                                        }
                                    }
                                    output.Insert(j, new List<int>() { tempy, tempx, templ });
                                }
                                else
                                {
                                    if (tempx != output[j][1])
                                    {
                                        if ((templ == w) || (output[j][2] == w))
                                        {
                                            output[j][1] = 0;
                                            output[j][2] = w;
                                        }
                                        else
                                        {
                                            j = 0;
                                            for (j = 0; j < output.Count; j++)
                                            {
                                                if (tempy < output[j][0])
                                                {
                                                    break;
                                                }
                                            }
                                            output.Insert(j, new List<int>() { tempy, tempx, templ });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (i != drawer_y.Count - 1)
                    {
                        bool flag = false;
                        for (int k = 0; k < output.Count; k++)
                        {
                            if (output[k][0] == drawer_y[i] + drawer_h[i])
                            {
                                flag = true;
                                output.Remove(output[k]);
                            }
                        }
                        if (flag == true)
                        {
                            int tempy = drawer_y[i] + drawer_h[i];
                            int tempx = 0;
                            int templ = w;
                            int j = 0;
                            for (j = 0; j < output.Count; j++)
                            {
                                if (output[j][0] == tempy)
                                {
                                    break;
                                }
                            }
                            if (j == output.Count)
                            {
                                j = 0;
                                for (j = 0; j < output.Count; j++)
                                {
                                    if (tempy < output[j][0])
                                    {
                                        break;
                                    }
                                }
                                output.Insert(j, new List<int>() { tempy, tempx, templ });
                            }
                            else
                            {
                                if (tempx != output[j][1])
                                {
                                    if ((templ == w) || (output[j][2] == w))
                                    {
                                        output[j][1] = 0;
                                        output[j][2] = w;
                                    }
                                    else
                                    {
                                        j = 0;
                                        for (j = 0; j < output.Count; j++)
                                        {
                                            if (tempy < output[j][0])
                                            {
                                                break;
                                            }
                                        }
                                        output.Insert(j, new List<int>() { tempy, tempx, templ });
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (drawer_w[i - 1] == w)
                    {
                        if (drawer_y[i] != 0)
                        {
                            int tempy = drawer_y[i];
                            int tempx = drawer_x[i];
                            int templ = w / 2;
                            int j = 0;
                            for (j = 0; j < output.Count; j++)
                            {
                                if (output[j][0] == tempy)
                                {
                                    break;
                                }
                            }
                            if (j == output.Count)
                            {
                                j = 0;
                                for (j = 0; j < output.Count; j++)
                                {
                                    if (tempy < output[j][0])
                                    {
                                        break;
                                    }
                                }
                                output.Insert(j, new List<int>() { tempy, tempx, templ });
                            }
                            else
                            {
                                if (tempx != output[j][1])
                                {
                                    if ((templ == w) || (output[j][2] == w))
                                    {
                                        output[j][1] = 0;
                                        output[j][2] = w;
                                    }
                                    else
                                    {
                                        if ((output[j][1] + output[j][2] == tempx) && (tempx + templ == w))
                                        {
                                            output[j][1] = 0;
                                            output[j][2] = w;
                                        }
                                        else
                                        {
                                            j = 0;
                                            for (j = 0; j < output.Count; j++)
                                            {
                                                if (tempy < output[j][0])
                                                {
                                                    break;
                                                }
                                            }
                                            output.Insert(j, new List<int>() { tempy, tempx, templ });
                                        }
                                    }
                                }
                            }
                        }
                    }/////////////////////////////////////////////////////////////
                    else
                    {
                        if (drawer_y[i] != 0)
                        {
                            if (drawer_l[i] != 0)
                            {
                                int tempy = drawer_y[i];
                                int tempx = drawer_x[i];
                                int templ = w / 2;
                                int j = 0;
                                for (j = 0; j < output.Count; j++)
                                {
                                    if (output[j][0] == tempy)
                                    {
                                        break;
                                    }
                                }
                                if (j == output.Count)
                                {
                                    j = 0;
                                    for (j = 0; j < output.Count; j++)
                                    {
                                        if (tempy < output[j][0])
                                        {
                                            break;
                                        }
                                    }
                                    output.Insert(j, new List<int>() { tempy, tempx, templ });
                                }
                                else
                                {
                                    if (tempx != output[j][1])
                                    {
                                        if ((templ == w) || (output[j][2] == w))
                                        {
                                            output[j][1] = 0;
                                            output[j][2] = w;
                                        }
                                        else
                                        {
                                            j = 0;
                                            for (j = 0; j < output.Count; j++)
                                            {
                                                if (tempy < output[j][0])
                                                {
                                                    break;
                                                }
                                            }
                                            output.Insert(j, new List<int>() { tempy, tempx, templ });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }

            return output;
        }
        public static List<List<int>> centerpoindex(List<List<int>> hidind, int h_, List<int> drawer_y, List<int> drawer_w, int w, string model)
        {

            List<List<int>> output = new List<List<int>>();
            List<int> hidind_new = new List<int>();
            for (int i = 0; i < hidind.Count; i++)
            {
                if (hidind[i][2] == w)
                {
                    hidind_new.Add(hidind[i][0]);
                }

            }

            int h = 0;
            h += h_;
            if ((model == "Base") || (model == "B") || (model == "Special Base") || (model == "SB"))
            {
                h -= 5;
            }
            else if ((model == "Storage") || (model == "S"))
            {
                h -= 4;
            }
            else
            {
                h -= 1;
            }


            hidind_new.Add(h);
            for (int i = 0; i < hidind_new.Count; i++)
            {
                for (int j = 0; j < drawer_y.Count; j++)
                {
                    if (i == 0)
                    {
                        if ((drawer_y[j] < hidind_new[i]) && (drawer_y[i] >= 0))
                        {
                            if (drawer_w[j] < w)
                            {
                                output.Add(new List<int>() { hidind_new[i], 0 });

                            }
                            break;
                        }

                    }
                    else
                    {

                        if ((drawer_y[j] < hidind_new[i]) && (drawer_y[j] >= hidind_new[i - 1]))
                        {
                            if (drawer_w[j] < w)
                            {
                                output.Add(new List<int>() { hidind_new[i] - hidind_new[i - 1], hidind_new[i - 1] });

                            }
                            break;
                        }
                    }
                }
            }



            return output;
        }
        public static int partitionindex(string model, List<int> door_x, List<int> door_y, int w)
        {
            int output = -1;
            if ((model != "S") && (model != "W"))
            {
                if ((door_x.Count == 1) && (w >= 30))
                {
                    output = door_y[0];
                }
            }

            return output;
        }
        public static List<List<int>> securitypanelindex(string locklist, List<int> door_x, List<int> drawer_y, int w, List<int> drawer_x
            , List<int> drawer_w)
        {
            List<List<int>> output = new List<List<int>>();

            string[] lockstring = locklist.Split('_');

            List<int> sec_x = new List<int>();
            List<int> sec_y = new List<int>();
            List<int> sec_l = new List<int>();
            for (int o = lockstring.Count() - 1 - door_x.Count; o > 0; o--)
            {
                int dif = Convert.ToInt32(lockstring[o]) - Convert.ToInt32(lockstring[o - 1]);

                if ((dif != 0) && (drawer_y[o] != 0))
                {
                    int m = 0;
                    for (m = 0; m < sec_x.Count; m++)
                    {
                        if (sec_l[m] != w)
                        {
                            if ((sec_y[m] == drawer_y[o]) && (drawer_x[o] != sec_x[m]))
                            {
                                break;
                            }
                        }
                    }
                    //if (m == sec_x.Count)
                    //{
                    sec_x.Add(drawer_x[o]);
                    sec_y.Add(drawer_y[o]);
                    sec_l.Add(drawer_w[o]);
                    //}
                    //else
                    //{
                    //    sec_x[m] = 0;
                    //    sec_y[m] = drawer_y[o];
                    //    sec_l[m] = w;
                    //}
                }

            }
            for (int o = 0; o < sec_x.Count; o++)
            {
                output.Add(new List<int>() { sec_x[o], sec_y[o], sec_l[o] });
            }
            return output;
        }

        public static List<string> locknumList(string lockprof, List<int> drawer_l)
        {
            List<string> locklistfordrawer = new List<string>();
            if ((lockprof == "Locks for all File Drawers Keyed Differently W/Security Panels") || (lockprof == "Locks for all File Drawers Keyed Differently W/Security Panels")
                || (lockprof == "Locks for all Doors and Drawers Keyed Differently (W/Security Panels if Applicable)") || (lockprof == "C") || (lockprof == "E") || (lockprof == "G"))
            {
                int lon = 6001;
                for (int j = 0; j < drawer_l.Count; j++)
                {
                    if (drawer_l[j] == 1)
                    {
                        locklistfordrawer.Add(lon.ToString());
                        lon++;
                    }
                    else
                    {
                        locklistfordrawer.Add("0");
                    }
                }
            }
            else if ((lockprof == "Hasps for all Drawers with Security Panels") || (lockprof == "H"))
            {
                int lon = 3001;
                for (int j = 0; j < drawer_l.Count; j++)
                {
                    if (drawer_l[j] == 1)
                    {
                        locklistfordrawer.Add(lon.ToString());
                        lon++;
                    }
                    else
                    {
                        locklistfordrawer.Add("0");
                    }
                }
            }
            else
            {
                int lon = 6001;
                for (int j = 0; j < drawer_l.Count; j++)
                {
                    if (drawer_l[j] == 1)
                    {
                        locklistfordrawer.Add(lon.ToString());
                        lon++;
                    }
                    else
                    {
                        locklistfordrawer.Add("0");
                    }
                }
            }
            return locklistfordrawer;
        }
        public static List<List<string>> shellParts(string model, string cstyle, string specialdesign, int d, int h,
            int w, int doorheight1, int doorindex1, int doorindex2, string metaltop, List<int> door_h, List<int> drawer_w, List<int> drawer_y, List<int> door_x, List<int> door_w,
            List<int> drawer_l, List<int> drawer_x, List<int> drawer_h, string selfcloseforsb, string lockpro)
        {
            Database database = new Database();
            double powder = 0;
            List<List<string>> output = new List<List<string>>();
            Dictionary<string, string> dic_spd = new Dictionary<string, string>()
            { {"-", "-"}, {"Sink", "S"}, {"Corner Cabinet", "CC"}, {"Fume Hood General Storage", "H"}, {"Open Face", "OF"}, {" ", "-" } ,{"", "-" } ,
            {"S", "S"}, {"CC", "CC"}, {"F", "F"}, {"OF", "OF" }, {"Flammable Liquid Storage", "F" },
                {"Acid/ Base Corrosive Storage", "A" }, {"Vacuum Pump Storage", "VP" }, {"Vacuum Pump Open Bottom with Dolly", "VD" },
                { "Vented Dry Chemical Storage", "E"}, {"A", "A" }, {"VP", "VP" }, {"VD", "VD" }, {"E", "E" } };


            Dictionary<string, string> dic_m = new Dictionary<string, string>()
            {
                {"B", "Base" }, {"C", "Suspended"}, {"S", "Storage"}, {"W", "Wall"}, {"M", "Mobile"}, {"G", "Glide"},
                {"Base", "Base" }, {"Suspended", "Suspended"}, {"Storage", "Storage"}, {"Wall", "Wall"}, {"Mobile", "Mobile"}, {"Glide", "Glide"},
                {"SB", "Special Base" }, {"Special Base", "Special Base" }
            };
            Dictionary<string, string> dic_loc = new Dictionary<string, string>()
            { { "No Lock"," "},
                { "Single Lock for Top Drawer(s)", "A"},
                {"Locks for all File Drawers Keyed Alike","B" },
                { "Locks for all File Drawers Keyed Differently W/Security Panels","C" },
                {"Locks for all Drawers Keyed Alike","D" },
                { "Locks for all Drawers Keyed Differently W/Security Panels", "E" },
                {"Locks for all Doors and Drawers Keyed Alike","F" } ,
                {"Locks for all Doors and Drawers Keyed Differently (W/Security Panels if Applicable)", "G" },
                {"Hasps for all Drawers with Security Panels","H" } ,
                {"Lock for Doors (Astrigal included if required)", "A" },
                { "Lock for Top Doors Only (Astrigal included if required)", "B" },
                { "Lock for Bottom Doors Only (Astrigal included if required)", "C" },
                { "", " "},{ " "," "},{ "A", "A"},{"B","B" },{ "C","C" },{"D","D" },{ "E", "E" },
                {"F","F" } ,{"G", "G" },{"H","H" } };

            List<string> locknumlist = locknumList(lockpro, drawer_l);
            string keyword = dic_m[model] + "_";
            keyword += cstyle + "_";
            keyword += dic_spd[specialdesign];
            if (((model == "SB") || (model == "Special Base")) && ((specialdesign == "F") || (specialdesign == "Flammable Liquid Storage")))
            {
                keyword += "_" + selfcloseforsb;
            }
            string _sa = "";
            //MySqlDataReader rd = database.getMySqlReader($"select * from shellbom where keyvalue = '{keyword}'");
            //if (rd.Read())
            //{
            //    _sa = rd.GetString("bom");
            //}
            //
            //database.CloseConnection();
            _sa = System.Configuration.ConfigurationSettings.AppSettings[keyword];
            string[] sa = _sa.Split('_');
            List<List<List<string>>> sas = new List<List<List<string>>>() { };
            for (int i = 0; i < 3; i++)
            {
                sas.Add(new List<List<string>>());
                string[] tempsa = sa[i].Split(',');
                for (int j = 0; j < tempsa.Count(); j++)
                {
                    sas[i].Add(new List<string>());
                    for (int k = 0; k < tempsa[j].Count(); k++)
                    {
                        sas[i][j].Add(tempsa[j].Substring(k, 1));
                    }
                }
            }
            ////////////////////////////////////////////////////////
            //output_shell.Add(sideleft(sas[0][0][0], w, h, d));//////////////////////////////////////sideleft
            output.Add(new List<string>());
            if ((model != "Storage") && (model != "S"))
            {
                for (int i = 0; i < sas[0][0].Count; i++)
                {
                    output[0].AddRange(sideleft(sas[0][0][i], w, h, d));
                    output[0].AddRange(sideright(sas[0][0][i], w, h, d));
                }
            }
            else
            {
                if ((h >= 35) || (d == 22))
                {
                    output[0].AddRange(sideleft(sas[0][0][0], w, h, d));
                    output[0].AddRange(sideright(sas[0][0][0], w, h, d));
                }
                else
                {
                    output[0].AddRange(sideleft(sas[0][0][1], w, h, d));
                    output[0].AddRange(sideright(sas[0][0][1], w, h, d));
                }
            }
            //output_shell.Add(backpanel(sa1[2], w, h, d));//////////////////////////////////////////backpanel
            output.Add(new List<string>());
            for (int i = 0; i < sas[0][1].Count; i++)
            {
                output[1].AddRange(backpanel(sas[0][1][i], w, h, d));
            }
            ////////////////////////////////////////////////////////////////////////////////////////top

            output.Add(new List<string>());
            if (metaltop.Substring(0, 1) != "N")
            {
                if ((specialdesign != "A") && (specialdesign != "Acid/ Base Corrosive Storage"))
                {
                    for (int i = 0; i < sas[0][2].Count; i++)
                    {
                        output[2].AddRange(top(sas[0][2][i], w, h, d));
                    }
                }
                else
                {
                    if (h < 35)
                    {
                        for (int i = 0; i < sas[0][2].Count; i++)
                        {
                            output[2].AddRange(top(sas[0][2][i], w, h, d));
                        }
                    }
                    else
                    {
                        output[2].AddRange(top("9", w, h, d));
                    }
                }
            }
            else
            {
                if ((sas[0][2][0] == "4") || (sas[0][2][0] == "2"))
                {
                    output[2].AddRange(top("2", w, h, d));
                }
                else
                {
                    output[2].AddRange(top("3", w, h, d));
                }
            }
            if (doorindex1 == 4)
            {
                output[2].AddRange(top("D", w, h, d));
            }
            //output_shell.Add(toe(sa1[4], w, h));//////////////////////////////////////////////////////////toe
            output.Add(new List<string>());
            if ((model != "Storage") && (model != "S"))
            {
                for (int i = 0; i < sas[0][3].Count; i++)
                {
                    output[3].AddRange(toe(sas[0][3][i], w, h));
                }
            }
            else
            {
                if ((h >= 35) || (d == 22))
                {
                    for (int i = 0; i < sas[0][3].Count; i++)
                    {
                        output[3].AddRange(toe(sas[0][3][i], w, h));
                    }
                }

            }

            //if ((doorindex1 == 1) || (doorindex1 == 2))/////////////////////////////////////////////////runner

            output.Add(new List<string>());
            int temph = h;
            temph = CabinetOverview.adjustHforFont(model, temph);
            for (int i = 0; i < sas[0][4].Count; i++)
            {
                output[4].AddRange(runnersupport(sas[0][4][i], temph));

            }
            //if (doorindex1 == 0)
            //{

            //}
            //else if ((doorindex1 == 1) || (doorindex1 == 2))
            //{
            //    for (int i = 0; i < sas[0][4].Count; i++)
            //    {
            //        output[4].AddRange(runnersupport(sas[0][4][i], doorheight1));
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < sas[0][4].Count; i++)
            //    {
            //        output[4].AddRange(runnersupport(sas[0][4][i], doorheight1));
            //        output[4].AddRange(runnersupport(sas[0][4][i], doorheight1));
            //    }
            //}
            //if ((model == "Glide")||(model == "G"))
            //{
            //    List<string> temp = new List<string>();
            //    temp.Add("GCB_S2"); temp.Add("GCB_S2");
            //    temp.Add("GCB_FR" + w.ToString()); temp.Add("GCB_FR" + w.ToString());
            //    output_shell.Add(temp);
            //}//////////////////////////////////////////////////////////////////////////////////////////////toespacebox
            output.Add(new List<string>());
            if ((model == "Glide") || (model == "G"))
            {
                output[5].Add("GCB_S2");
                output[5].Add("GCB_S2");
                output[5].Add("GCB_FR" + w.ToString());
                output[5].Add("GCB_FR" + w.ToString());
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////gusset
            output.Add(new List<string>());
            output[6].AddRange(gusset(sas[0][6][0]));
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            output.Add(new List<string>());
            //if (doorindex1 == 0)//////////////////////////////////////////////////////////////////////////////////////shelf

            int doorwidth = w;
            int doh1 = 0;
            int doh2 = 0;
            if ((model != "S") && (model != "Storage") && (model != "W") && (model != "Wall"))
            {
                if (doorindex1 != 0)
                {
                    if ((w >= 30) && ((doorindex1 == 1) || (doorindex1 == 2)))
                    {
                        doorwidth /= 2;
                    }
                    for (int i = 0; i < sas[1][0].Count; i++)
                    {
                        output[7].AddRange(shelf(sas[1][0][i], doorwidth, d));
                        powder += powder_shelf(doorwidth, d);
                    }
                }
                else
                {
                    doorwidth = 0;
                }
            }
            else
            {
                doh1 = h;
                //}
                if ((model == "S") || (model == "Storage"))
                {
                    doh1 -= 4;
                }
                if (doh1 <= 31)
                {
                    for (int i = 0; i < sas[1][0].Count; i++)
                    {
                        output[7].AddRange(shelf(sas[1][0][i], doorwidth, d));
                        powder += powder_shelf(doorwidth, d);
                    }
                }
                else if ((doh1 > 31) && (doh1 < 80))
                {
                    for (int i = 0; i < sas[1][0].Count; i++)
                    {
                        output[7].AddRange(shelf(sas[1][0][i], doorwidth, d));
                        output[7].AddRange(shelf(sas[1][0][i], doorwidth, d));
                        if (doh2 != 0)
                        {
                            output[7].AddRange(shelf(sas[1][0][i], doorwidth, d));
                            powder += powder_shelf(doorwidth, d);
                        }
                        powder += powder_shelf(doorwidth, d);
                        powder += powder_shelf(doorwidth, d);
                    }
                }
                else
                {
                    for (int i = 0; i < sas[1][0].Count; i++)
                    {
                        output[7].AddRange(shelf(sas[1][0][i], doorwidth, d));
                        output[7].AddRange(shelf(sas[1][0][i], doorwidth, d));
                        output[7].AddRange(shelf(sas[1][0][i], doorwidth, d));
                        output[7].AddRange(shelf(sas[1][0][i], doorwidth, d));
                        powder += powder_shelf(doorwidth, d);
                        powder += powder_shelf(doorwidth, d);
                        powder += powder_shelf(doorwidth, d);
                        powder += powder_shelf(doorwidth, d);
                    }
                    output[7].AddRange(intermedshelf(doorwidth, d));
                    powder += powder_shelf(doorwidth, d);
                }
            }
            //output_door.Add(removableback(sa2[1], w, h));////////////////////////////////////////////////////////////removableback
            output.Add(new List<string>());

            for (int i = 0; i < sas[1][1].Count; i++)
            {
                output[8].AddRange(removableback(sas[1][1][i], w, h));

            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////bottom
            output.Add(new List<string>());
            if ((model != "S") && (model != "Storage"))
            {
                if ((cstyle == "PN") || (cstyle == "SN"))
                {
                    doorwidth = w;
                }
                if (doorwidth > 0)
                {
                    for (int i = 0; i < sas[1][2].Count; i++)
                    {
                        output[9].AddRange(bottom(sas[1][2][i], doorwidth, h));
                    }
                }
                else
                {

                }
            }
            else if ((specialdesign == "F") || (specialdesign == "Flammable Liquid Storage"))
            {
                output[9].AddRange(bottom("8", doorwidth, h));
            }
            else
            {
                if ((h >= 35) || (d == 22))
                {
                    output[9].AddRange(bottom(sas[1][2][0], w, h));
                }
                else
                {
                    output[9].AddRange(bottom(sas[1][2][1], w, h));
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////stileside
            output.Add(new List<string>());
            if (((model == "S") || (model == "Storage") || (model == "W") || (model == "Wall")) &&
                        (cstyle == "PN") && (doorindex1 < 40))
            {
                output[10].AddRange(stileside("4", doh1, doorindex1));
            }
            else
            {
                for (int i = 0; i < sas[1][3].Count; i++)
                {
                    output[10].AddRange(stileside(sas[1][3][i], doh1, doorindex1));
                }
            }
            if (doh2 != 0)
            {
                for (int i = 0; i < sas[1][3].Count; i++)
                {
                    output[10].AddRange(stileside(sas[1][3][i], doh2, doorindex2));
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////stiletop
            output.Add(new List<string>());
            for (int i = 0; i < sas[1][4].Count; i++)
            {
                output[11].AddRange(stiletop(sas[1][4][i], w));
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////stilebottom
            output.Add(new List<string>());
            for (int i = 0; i < sas[1][5].Count; i++)
            {
                output[12].AddRange(stilebottom(sas[1][5][i], w));
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////front
            output.Add(new List<string>());
            if (((model == "B") || (model == "Base")) && ((specialdesign.Substring(0, 1) == "A") && (h < 35)))
            {

            }
            else
            {
                for (int i = 0; i < sas[1][6].Count; i++)
                {
                    output[13].AddRange(front(sas[1][6][i], w));
                }
            }

            ////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////hidden
            output.Add(new List<string>());
            int h1 = h;
            if ((model == "Base") || (model == "B") || (model == "Special Base") || (model == "SB"))
            {
                h1 -= 5;
            }
            else if ((model == "Storage") || (model == "S"))
            {
                h1 -= 4;
            }
            else
            {
                h1 -= 1;
            }
            List<List<int>> hidd_temp = hidrailindex(door_h, door_x, door_w, h1, drawer_w, drawer_y, model, w, drawer_l, drawer_x, drawer_h);
            List<string> hidd = new List<string>();
            for (int i = 0; i < hidd_temp.Count; i++)
            {
                hidd.AddRange(hiddenrail(sas[2][0][0], hidd_temp[i][2], h));
            }
            output[14].AddRange(hidd);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////partition
            ///dayibayingzhengzaixiang
            output.Add(new List<string>());
            if ((w >= 30) && ((doorindex1 == 1) || (doorindex1 == 2)))
            {
                for (int i = 0; i < sas[2][1].Count; i++)
                {
                    output[15].AddRange(partition(sas[2][1][i], w, door_h[0]));
                    powder += powder_part(door_h[0], w);
                }
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////centerpost
            output.Add(new List<string>());
            List<List<int>> center_temp = centerpoindex(hidd_temp, h, drawer_y, drawer_w, w, model);
            List<string> center = new List<string>();
            for (int i = 0; i < center_temp.Count; i++)
            {
                for (int j = 0; j < sas[2][2].Count; j++)
                {
                    output[16].AddRange(centerpost(sas[2][2][j], center_temp[i][0]));
                }
                //center.AddRange(centerpost(model, cstyle, sa3[0], w, center_temp[i][0], h));
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////security
            ///
            output.Add(new List<string>());
            List<int> sec_x = new List<int>();
            List<int> sec_y = new List<int>();
            List<int> sec_l = new List<int>();
            for (int i = locknumlist.Count - 1; i > 0; i--)
            {
                int dif = Convert.ToInt32(locknumlist[i]) - Convert.ToInt32(locknumlist[i - 1]);

                if ((dif != 0) && (drawer_y[i] != 0))
                {
                    int j = 0;
                    for (j = 0; j < sec_x.Count; j++)
                    {
                        if (sec_l[j] != w)
                        {
                            if ((sec_y[j] == drawer_y[i]) && (drawer_x[i] != sec_x[j]))
                            {
                                break;
                            }
                        }
                    }
                    //if (j == sec_x.Count)
                    //{
                    sec_x.Add(drawer_x[i]);
                    sec_y.Add(drawer_y[i]);
                    sec_l.Add(drawer_w[i]);
                    //}
                    //else
                    //{
                    //    sec_x[j] = 0;
                    //    sec_y[j] = drawer_y[i];
                    //    sec_l[j] = w;
                    //}
                }

            }
            for (int i = 0; i < sec_l.Count; i++)
            {
                for (int j = 0; j < sas[2][3].Count; j++)
                {
                    output[17].AddRange(securitypanel(sas[2][3][j], sec_l[i]));
                    powder += powder_sec(sec_l[i], 22);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////selfclose
            output.Add(new List<string>());

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            powder += powder_shell(w, h, d);
            //output_drawer.Add(center);


            if ((cstyle == "SH") || (cstyle == "SN"))
            {
                for (int i = 0; i < output.Count; i++)
                {
                    for (int j = 0; j < output[i].Count; j++)
                    {
                        if ((i == 6))
                        {

                        }
                        else
                        {
                            output[i][j] += "-SS";
                        }

                    }
                }
            }

            output.Add(new List<string>() { powder.ToString("0.00") });
            return output;

        }
        public static List<List<string>> acc_shell(string model, int w, int drawernum, int h, string cstyle, string specialdesign, string selfcloseforbf, string Counterweight, double powder)
        {
            List<string> name = new List<string>();
            List<string> num = new List<string>();
            List<string> shigesha = new List<string>();
            if ((model == "B") || (model == "Base") || (model == "C") || (model == "Suspended") || (model == "SB") || (model == "Special Base"))
            {
                if ((specialdesign != "F") && (specialdesign != "A") && (specialdesign != "VP")
                    && (specialdesign != "Flammable Liquid Storage") && (specialdesign != "Acid/ Base Corrosive Storage") && (specialdesign != "Vacuum Pump Storage"))
                {
                    name = new List<string>() { "SBS43", "SBS64", "K428", "2703" };
                    num = new List<string>() { "43", "6", "4", "4" };
                    shigesha = new List<string>() { "Rivet", "Rivet", "Bolt", "Plug" };
                    name.Add("CR#8EXT"); if (w >= 30) { num.Add("4"); } else { num.Add("2"); }
                    shigesha.Add("Pan Tech STZ");
                    name.Add("CR8-32X.1"); num.Add("2"); shigesha.Add("Truss");
                    if (w >= 30)
                    { name.Add("CR8-32X.75"); num.Add("2"); shigesha.Add("LockWasher"); }
                    name.Add("CR#8X.50TS"); if (w >= 30) { num.Add("8"); } else { num.Add("4"); }
                    shigesha.Add("Truss");
                    name.Add("SJ-5012"); num.Add("4"); shigesha.Add("Bumpon");
                    name.Add("FHSMQ82"); num.Add("10"); shigesha.Add("Flat Head SHT");
                    name.Add("SS8-32X.50FMU"); if (w >= 30) { num.Add("8"); } else { num.Add("4"); }
                    shigesha.Add("Patch");
                    if (w >= 40) { name.Add("SS8-32X.62TM"); num.Add("4"); shigesha.Add("Patch"); }
                    name.Add("FHSDP82-N8-.50ZNUC"); num.Add("4"); shigesha.Add("SLF");
                    name.Add("CR#14X.50PS"); num.Add("2"); shigesha.Add("Truss");
                }
                else if ((specialdesign == "F") || (specialdesign == "Flammable Liquid Storage"))
                {
                    name.Add("SBS43"); num.Add("48"); shigesha.Add("RIVET");
                    name.Add("LC3318W"); num.Add("2"); shigesha.Add("Vent Collar");
                    name.Add("HP81048300"); num.Add(((44 + w + 4) / 3).ToString("0.00")); shigesha.Add("Insulation");
                    if (w >= 30)
                    {
                        name.Add("SBS64"); num.Add("14"); shigesha.Add("RIVET");
                        name.Add("C.06X.41X.81F"); num.Add("1"); shigesha.Add("FLAT WASHER");
                        name.Add("90533A113"); num.Add((Convert.ToInt32(w / 4)).ToString()); shigesha.Add("NUT");
                        name.Add("SS10-24KN"); num.Add("4"); shigesha.Add("NUT");



                    }
                    if (selfcloseforbf == "SC")
                    {
                        name.Add("312-C-12-FS-12"); num.Add("1"); shigesha.Add("RIVET");
                        name.Add("#138NN-5327"); num.Add("5"); shigesha.Add("BUMPER");
                        name.Add("P.03X.20X.45F"); num.Add("1"); shigesha.Add("WASHER NYLON");
                        name.Add("90295A160"); num.Add("1"); shigesha.Add("WASHER NYLON");
                        name.Add("P.06X.17X.37F"); num.Add("2"); shigesha.Add("WASHER PLASTIC");
                        name.Add("759"); num.Add("2"); shigesha.Add("RUBBER GROMMET");
                        name.Add("9654K111"); num.Add("1"); shigesha.Add("WIRE SPRING");
                        name.Add("LC338SC-H_G"); num.Add("1"); shigesha.Add("");
                        name.Add("LC338SC-F"); num.Add("1"); shigesha.Add("");
                        name.Add("LC338SC-CSS"); num.Add("1"); shigesha.Add("");
                        name.Add("LC3315SCB"); num.Add("1"); shigesha.Add("");
                        name.Add("LC3315SCT"); num.Add("1"); shigesha.Add("");

                    }
                    else
                    {
                        //name.Add("743"); num.Add("1"); shigesha.Add("RUBBER GROMMET");
                    }
                    if (cstyle.Substring(0, 1) == "S")
                    {
                        name.Add("SS#8X.50PT");
                    }
                    else
                    {
                        name.Add("CR#8X.50PT");
                    }
                    if (selfcloseforbf == "SC")
                    { num.Add("16"); }
                    else { num.Add("4"); }
                    shigesha.Add("TRUSS");
                    if (cstyle.Substring(0, 1) == "S")
                    {
                        name.Add("SS#10X.75TS");
                    }
                    else
                    {
                        name.Add("CR#10X.75TS");
                    }

                    num.Add("6"); shigesha.Add("TRUSS");
                    name.Add("CR37-16HN"); num.Add("1"); shigesha.Add("NUT");
                    name.Add("232000200"); num.Add("2"); shigesha.Add("TRI-SURE PLUG");
                    name.Add("SS8-32X.37FMU/NL");
                    if (w >= 30) { num.Add("8"); } else { num.Add("4"); }
                    shigesha.Add("PATCH");
                    name.Add("SS6-32X.25PM/NL"); num.Add("8"); shigesha.Add("PATCH");
                    name.Add("K428"); num.Add("4"); shigesha.Add("BOLTS");
                    name.Add("3-2631-BS-Z"); num.Add("1"); shigesha.Add("PADDLE LATCH");
                    name.Add("480"); num.Add("3"); shigesha.Add("PADDLE LATCH");
                    name.Add("GC500-0750"); num.Add((w / 400).ToString("0.00")); shigesha.Add("TAPE");
                    name.Add("HP81048300"); num.Add("0.17"); shigesha.Add("CER FIBER BLNK");
                    name.Add("FILLER CLIP"); num.Add("9"); shigesha.Add("FILLER CLIP");
                    name.Add("23519"); num.Add("4"); shigesha.Add("SHELF CLIP");
                    name.Add("90533A133"); num.Add("3"); shigesha.Add("SPEED FLANGE");
                    name.Add("98338A130"); num.Add("3"); shigesha.Add("COTTER PIN");
                }
                else if ((specialdesign == "A") || (specialdesign == "Acid/ Base Corrosive Storage"))
                {

                    name.Add("SBS43"); num.Add("47"); shigesha.Add("RIVET");
                    name.Add("HNR6-10-12-2"); num.Add("8"); shigesha.Add("RIVET");
                    if (cstyle.Substring(0, 1) == "S")
                    {
                        name.Add("SS#8X.50TS"); num.Add("6"); shigesha.Add("TRUSS");
                        //name.Add("SS8-32X1.50PM"); num.Add("4"); shigesha.Add("PAD HEAD");
                        //name.Add("SS8-32X37PM"); num.Add("4"); shigesha.Add("PAD HEAD");
                        name.Add("SS#8EXT.TL/W"); num.Add("4"); shigesha.Add("EXTERNAL TOOTH LOCKWASHER");
                    }
                    else
                    {
                        name.Add("CR#8X.50TS"); num.Add("6"); shigesha.Add("TRUSS");
                        //name.Add("CR8-32X1.50PM"); num.Add("4"); shigesha.Add("PAD HEAD");
                        //name.Add("CR8-32X37PM"); num.Add("4"); shigesha.Add("PAD HEAD");
                        name.Add("CR#8EXT.TL/W"); num.Add("4"); shigesha.Add("EXTERNAL TOOTH LOCKWASHER");
                    }
                    if (w >= 30)
                    {
                        if (cstyle.Substring(0, 1) == "S")
                        {
                            name.Add("SS8-32KN");
                        }
                        else
                        {
                            name.Add("CR8-32KN");
                        }
                        num.Add("4"); shigesha.Add("PAD HEAD");
                        name.Add("95000A148"); num.Add("8"); shigesha.Add("NYLON");
                        name.Add("785"); num.Add("4"); shigesha.Add("BUMPER");
                        name.Add("SS6-32X.25PM/NL"); num.Add("8"); shigesha.Add("PATCH");
                        name.Add("SS8-32X.37FMU/NL"); num.Add("8"); shigesha.Add("PATCH");
                    }
                    else
                    {
                        name.Add("95000A148"); num.Add("4"); shigesha.Add("NYLON");
                        name.Add("785"); num.Add("2"); shigesha.Add("BUMPER");
                        name.Add("SS6-32X.25PM/NL"); num.Add("4"); shigesha.Add("PATCH");
                        name.Add("SS8-32X.37FMU/NL"); num.Add("4"); shigesha.Add("PATCH");
                    }
                    name.Add("K428"); num.Add("4"); shigesha.Add("BOLT");
                    name.Add("FILLER CLIP"); num.Add("8"); shigesha.Add("FILLER CLIP");
                    name.Add(".625 TW 86X102"); num.Add((0.0284 * w / 18).ToString("0.00")); shigesha.Add("TRIWALL");
                    name.Add("H_700_TOP"); num.Add(((w - 48 + 45.625) * (20.25)).ToString("0.00")); shigesha.Add("ACID TOP");
                    name.Add("H_700_BOTTOM"); num.Add(((w - 48 + 45.625) * (20.25)).ToString("0.00")); shigesha.Add("ACID BOTTOM");
                    if (w >= 30)
                    {
                        name.Add("H_700_BACK"); num.Add(((w / 2 - 24 + 22.6) * (20.25)).ToString("0.00")); shigesha.Add("ACID BACK");
                        name.Add("H_700_BACK"); num.Add(((w / 2 - 24 + 22.6) * (20.25)).ToString("0.00")); shigesha.Add("ACID BACK");
                    }
                    else { name.Add("H_700_BACK"); num.Add(((w - 24 + 22.6) * (20.25)).ToString("0.00")); shigesha.Add("ACID BACK"); }
                    name.Add("H_700_SIDE"); num.Add(((22) * (20.25)).ToString("0.00")); shigesha.Add("ACID SIDE");
                    name.Add("H_700_SIDE"); num.Add(((22) * (20.25)).ToString("0.00")); shigesha.Add("ACID SIDE");
                    name.Add("H_700_SUPPORT"); num.Add(((1.25) * (9)).ToString("0.00")); shigesha.Add("ACID SUPPORT");
                    name.Add("H_700_SUPPORT"); num.Add(((1.25) * (9)).ToString("0.00")); shigesha.Add("ACID SUPPORT");
                    name.Add("H_700_SHELF"); num.Add(((10) * (w - 48 + 45.625)).ToString("0.00")); shigesha.Add("ACID SHELF");
                    name.Add("SC3-810WH"); num.Add("14"); shigesha.Add("HINGE SCREW COVER");
                }
                else if ((specialdesign == "VP") || (specialdesign == "Vacuum Pump Storage"))
                {
                    if (w >= 30)
                    {
                        name.Add("SBS43"); num.Add("35"); shigesha.Add("RIVET");
                        name.Add("SBS52"); num.Add("6"); shigesha.Add("RIVET");
                        name.Add("SS6-32X.25PM/NL"); num.Add("8"); shigesha.Add("PATCH");
                        name.Add("SS8-32X.37FMU/NL"); num.Add("8"); shigesha.Add("PATCH");
                        name.Add("CR#8X.50TS"); num.Add("4"); shigesha.Add("TRUSS");
                        name.Add(".625 TW 86X102"); num.Add("0.0568"); shigesha.Add("TRIWALL");
                    }
                    else
                    {
                        name.Add("SBS43"); num.Add("31"); shigesha.Add("RIVET");
                        name.Add("SBS52"); num.Add("12"); shigesha.Add("RIVET");
                        name.Add("SS6-32X.25PM/NL"); num.Add("4"); shigesha.Add("PATCH");
                        name.Add("SS8-32X.37FMU/NL"); num.Add("4"); shigesha.Add("PATCH");
                        name.Add("CR#8X.50TS"); num.Add("2"); shigesha.Add("TRUSS");
                        name.Add(".625 TW 86X102"); num.Add("0.021"); shigesha.Add("TRIWALL");
                    }
                    name.Add("FF40AHP4"); num.Add((4 * w / 12).ToString("0.00")); shigesha.Add("FOAM");
                    name.Add("CR10-24X.75TM"); num.Add("2"); shigesha.Add("TRUSS");
                    name.Add("CR#6X.37PF"); num.Add("4"); shigesha.Add("TRUSS");
                    name.Add("M4518"); num.Add("1"); shigesha.Add("CORD GRIP");
                    name.Add("BP 560-DC2"); num.Add("2"); shigesha.Add("SCREW");
                    name.Add("CR8-32X.37PM"); num.Add("4"); shigesha.Add("SCREW");
                    name.Add("CR10-24KN"); num.Add("2"); shigesha.Add("NUT");
                    name.Add("CR10-32X.37GMS"); num.Add("1"); shigesha.Add("NUT");
                    name.Add("LC1221-PLC"); num.Add("1"); shigesha.Add("SWITCH");
                    name.Add("SS-11"); num.Add("1"); shigesha.Add("FACE PLATE");
                    name.Add("MCAP12/3SO"); num.Add((w / 4).ToString("0.00")); shigesha.Add("CABLE");
                    name.Add("12GTHHNSOWHT"); num.Add("3.73"); shigesha.Add("WIRE");
                    name.Add("12GTHHNSOGRN"); num.Add("3.73"); shigesha.Add("WIRE");
                    name.Add("12GTHHNSOBLK"); num.Add("3.73"); shigesha.Add("WIRE");
                    name.Add("12GTHHNSORED"); num.Add("3.73"); shigesha.Add("WIRE");
                    name.Add("K428"); num.Add("4"); shigesha.Add("BOLT");
                    name.Add("DTF150-ZBFL20"); num.Add("2"); shigesha.Add("TRACK");
                    name.Add("4715FS-12T-B40-D00"); num.Add("1"); shigesha.Add("FAN");
                    name.Add("MC32668"); num.Add("1"); shigesha.Add("FAN");
                    name.Add("RA 232"); num.Add("1"); shigesha.Add("JUNCTION BOX");
                    name.Add("LCCR20"); num.Add("1"); shigesha.Add("JUNCTION BOX");
                    name.Add("RS12"); num.Add("1"); shigesha.Add("JUNCTION BOX");
                    name.Add("RA 600"); num.Add("1"); shigesha.Add("SWITCH BOX");
                    name.Add("RA 600"); num.Add("1"); shigesha.Add("SWITCH BOX");
                    //name.Add("EMT50"); num.Add("0.87"); shigesha.Add("LOCKWASHER");
                    name.Add("EMT16"); num.Add("2"); shigesha.Add("LOCKWASHER");
                    name.Add("TOP SOUNDFOAM"); num.Add("1"); shigesha.Add("TOP SOUNDFOAM");
                    name.Add("BACK SOUNDFOAM"); num.Add("1"); shigesha.Add("BACK SOUNDFOAM");
                    name.Add("SIDE SOUNDFOAM"); num.Add("2"); shigesha.Add("SIDE SOUNDFOAM");
                    if (w >= 30)
                    {
                        name.Add("FRONT SOUNDFOAM"); num.Add("2"); shigesha.Add("FRONT SOUNDFOAM");
                    }
                    else
                    {
                        name.Add("FRONT SOUNDFOAM"); num.Add("1"); shigesha.Add("FRONT SOUNDFOAM");
                    }
                }
                if ((model == "C") || (model == "Suspended"))
                {
                    name.Add("CCSH-010716-1");
                    num.Add("2");
                    shigesha.Add("Front Top Hanger");
                    name.Add("FHMSP82-10-32-3.00TPSS");
                    num.Add("4");
                    shigesha.Add("Flat Head");


                }
            }
            else if ((model == "G") || (model == "Glide"))
            {
                name = new List<string>() { "SBS43", "SBS64", "K428", "2703" };
                num = new List<string>() { "43", "6", "4", "4" };
                shigesha = new List<string>() { "Rivet", "Rivet", "Bolt", "Plug" };
                name.Add("CR#8EXT"); if (w >= 30) { num.Add("4"); } else { num.Add("2"); }
                shigesha.Add("Pan Tech STZ");
                name.Add("CR8-32X.1"); num.Add("2"); shigesha.Add("Truss");
                if (w >= 30)
                { name.Add("CR8-32X.75"); num.Add("2"); shigesha.Add("LockWasher"); }
                name.Add("CR#8X.50TS"); if (w >= 30) { num.Add("8"); } else { num.Add("4"); }
                shigesha.Add("Truss");
                name.Add("SJ-5012"); num.Add("4"); shigesha.Add("Bumpon");
                name.Add("FHSMQ82"); num.Add("10"); shigesha.Add("Flat Head SHT");
                name.Add("SS8-32X.50FMU"); if (w >= 30) { num.Add("8"); } else { num.Add("4"); }
                shigesha.Add("Patch");
                if (w >= 40) { name.Add("SS8-32X.62TM"); num.Add("4"); shigesha.Add("Patch"); }
                name.Add("FHSDP82-N8-.50ZNUC"); num.Add("4"); shigesha.Add("SLF");
                name.Add("CR#14X.50PS"); num.Add("2"); shigesha.Add("Truss");
                name.Add("SCS42"); name.Add("SCS43"); name.Add("1167");
                num.Add("8"); num.Add("16"); num.Add("4");
                shigesha.Add("Rivet"); shigesha.Add("Rivet"); shigesha.Add("Glide");
            }
            else if ((model == "M") || (model == "Mobile"))
            {
                name = new List<string>() { "SBS43", "SBS64", "K428", "2703" };
                num = new List<string>() { "43", "6", "4", "4" };
                shigesha = new List<string>() { "Rivet", "Rivet", "Bolt", "Plug" };
                name.Add("CR#8EXT"); if (w >= 30) { num.Add("4"); } else { num.Add("2"); }
                shigesha.Add("Pan Tech STZ");
                name.Add("CR8-32X.1"); num.Add("2"); shigesha.Add("Truss");
                if (w >= 30)
                { name.Add("CR8-32X.75"); num.Add("2"); shigesha.Add("LockWasher"); }
                name.Add("CR#8X.50TS"); if (w >= 30) { num.Add("8"); } else { num.Add("4"); }
                shigesha.Add("Truss");
                name.Add("SJ-5012"); num.Add("4"); shigesha.Add("Bumpon");
                name.Add("FHSMQ82"); num.Add("10"); shigesha.Add("Flat Head SHT");
                name.Add("SS8-32X.50FMU"); if (w >= 30) { num.Add("8"); } else { num.Add("4"); }
                shigesha.Add("Patch");
                if (w >= 40) { name.Add("SS8-32X.62TM"); num.Add("4"); shigesha.Add("Patch"); }
                name.Add("FHSDP82-N8-.50ZNUC"); num.Add("4"); shigesha.Add("SLF");
                name.Add("CR#14X.50PS"); num.Add("2"); shigesha.Add("Truss");
                name.Add("DW-02GRP-100-SW-TP11"); name.Add("DW-02GRP-100-TL-TP11");
                num.Add("2"); num.Add("2");
                shigesha.Add("Caster"); shigesha.Add("Caster");

                //if ((w > 24) || (drawernum > 1))
                //{
                //    name.Add("WTBXM"); name.Add("WTBXM-B");
                //    num.Add("1"); num.Add("2");
                //    shigesha.Add("CounterWeight"); shigesha.Add("Brick");
                //}
                try
                {
                    double counterweight = 0;
                    counterweight = Convert.ToInt32(Counterweight);
                    if (counterweight > 0)
                    {
                        name.Add("WTBXM"); num.Add("1"); shigesha.Add("CounterWeight Box");
                        counterweight -= 4.24;
                        int tempnum = Convert.ToInt32(Math.Ceiling(counterweight / 5));
                        name.Add("WTBXM-B"); num.Add(tempnum.ToString()); shigesha.Add("Brick");
                    }
                }
                catch { }
            }
            else if ((model == "S") || (model == "Storage") || (model == "W") || (model == "Wall"))
            {
                name = new List<string>();
                num = new List<string>();
                shigesha = new List<string>();
                name.Add("SBS43");
                if (w >= 30)
                {
                    num.Add("47");
                }
                else
                {
                    num.Add("33");
                }
                shigesha.Add("RIVET");
                name.Add("SBS62"); num.Add("4"); shigesha.Add("RIVET");
                name.Add("SCS42"); num.Add("10"); shigesha.Add("RIVET");
                name.Add("785");
                if (w >= 30)
                {
                    num.Add("8");
                }
                else
                {
                    num.Add("2");
                }
                shigesha.Add("BUMPER");
                name.Add("23519");
                if (h > 49)
                {
                    num.Add("16");
                }
                else if (h >= 35)
                {
                    num.Add("8");
                }
                else
                {
                    num.Add("4");
                }

                shigesha.Add("SHELF CLIP");
                name.Add("2703"); num.Add("4"); shigesha.Add("DOME PLUG");
                name.Add("CR#14X.50PS"); num.Add("2"); shigesha.Add("PAN HEAD");
                name.Add("CR#6X.37PT"); num.Add("16"); shigesha.Add("PAN HEAD");
                if (cstyle == "SN")
                {
                    name.Add("SS8-32X.37PM");
                    name.Add("SS8-32X.50TM");
                }
                else
                {
                    name.Add("CR8-32X.37PM");
                    name.Add("CR8-32X.50TM");
                }
                num.Add("4"); shigesha.Add("TRUSS");
                num.Add("6"); shigesha.Add("TRUSS");
                name.Add("CR#8X.50PT"); num.Add("2"); shigesha.Add("PAN HEAD");
                name.Add("CR#8EXT.TL/W");
                if (w >= 30)
                {
                    num.Add("8");
                }
                else
                {
                    num.Add("2");
                }
                shigesha.Add("TOOTH LOCKWASHER");
                name.Add("SJ-5012"); num.Add("2"); shigesha.Add("BUMPON");
                //name.Add("HLDNCLP"); num.Add("2"); shigesha.Add("PACKING CLIP");
                name.Add("K428"); num.Add("4"); shigesha.Add("BOLT");
            }
            if ((cstyle != "WH") && (cstyle.Substring(0, 1) != "S"))
            {
                name.Add("Powder"); num.Add(powder.ToString("0.00")); shigesha.Add("Powder");
            }
            List<List<string>> output = new List<List<string>>();
            output.Add(name);
            output.Add(num);
            output.Add(shigesha);
            return output;
        }
        public static List<List<string>> ass_shell(int w, int h, int d, string specde, string cstyle)
        {
            string[] name = { "Grind", "Weld", "Assembly", "Paint" };
            double shellPaint = 6;
            double[] diFF = { w - 18, h - 35, d - 22 };
            List<double> difAbs = new List<double>() { Math.Abs(w - 18), Math.Abs(h - 35), Math.Abs(d - 22) };
            difAbs.Sort();
            difAbs.Reverse();
            int i = 0;
            for (i = 0; i < 3; i++)
            {
                if (Math.Abs(diFF[i]) == difAbs[0])
                {
                    break;
                }
            }
            for (int j = 0; j < 3; j++)
            {
                if (j == i)
                {
                    shellPaint += diFF[j] / 3;
                }
                else
                {
                    shellPaint += diFF[j] / 30;
                }
            }
            List<string> num = new List<string>();
            num.Add("6");
            num.Add("10");
            num.Add("10");
            num.Add(shellPaint.ToString("0.00"));

            if ((specde == "A") || (specde == "Acid/ Base Corrosive Storage"))
            {
                if (w >= 30)
                {
                    num[2] = (Convert.ToDouble(num[2]) + 55).ToString("0.00");
                }
                else
                {
                    num[2] = (Convert.ToDouble(num[2]) + 55).ToString("0.00");
                }
            }
            if ((cstyle == "WH") || (cstyle.Substring(0, 1) == "S"))
            {
                num[3] = "0";
            }
            List<List<string>> output = new List<List<string>>();
            output.Add(name.ToList());
            output.Add(num);
            return output;
        }
    }
}
