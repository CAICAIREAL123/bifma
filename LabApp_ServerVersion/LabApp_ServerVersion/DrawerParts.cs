using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class DrawerParts
    {
        public static List<string> drawerCate(string drawer_load, string track)
        {
            string selfcls = "False";
            string interlock = "False";
            string extsion = "False";

            if ((track == "T1") || (track == "T2") || (track == "T4")
                || (track == "T5") || (track == "T7" || (track == "T8")))
            {
                if (drawer_load != "100")
                {
                    extsion = "True";
                }
            }
            else if ((track == "T2L") || (track == "T8L"))
            {
                extsion = "True";
                interlock = "True";
            }
            else if ((track == "T3") || (track == "T6"))
            {
                selfcls = "True";
                if (drawer_load != "100")
                {
                    extsion = "True";
                }
            }
            else if ((track == "T5L"))
            {
                interlock = "True";
                //if (drawer_load != "100")
                //{
                    extsion = "True";
                //}
            }
            else
            {
                extsion = "True";
                selfcls = "True";
            }

            return new List<string>() { selfcls, interlock, extsion };

        }
        public static string drawername(string c_style, int d_height, int drawerwidth, string d_load, string ch, string lk, string pullstyle, string sefcls, string interlock, string extsion, string specialdesign)
        {
            string output = "";
            string track_type = "";
            string selfclose_code = "";
            string pull_code = "";
            string key_type = "";
            string label_holder = "";

            if (d_load != "0")
            {
                if (extsion == "True")
                {
                    track_type = "F";
                }
                else
                {
                    track_type = "T";
                }
                if ((lk == "False") || (lk == "0"))
                {
                }
                else
                {
                    key_type = "K0";
                }
                if ((ch == "True") || (ch == "1"))
                {
                    label_holder = "L2";
                }
                if ((pullstyle == "Flush ABS") || (pullstyle == "P1"))
                {
                    pull_code = "SP";
                }
                else if ((pullstyle == "Flush Aluminum") || (pullstyle == "P2"))
                {
                    pull_code = "AP";
                }
                else if ((pullstyle == "Raised Wire 96 mm") || (pullstyle == "P3"))
                {
                    pull_code = "378";
                }
                else if ((pullstyle == "Raised Wire 4\"") || (pullstyle == "P4"))
                {
                    pull_code = "400";
                }
                else if ((pullstyle == "Raised Wire 128 mm") || (pullstyle == "P5"))
                {
                    pull_code = "504";
                }
                else
                {
                    pull_code = "";
                }
                if (sefcls == "False")
                {
                    selfclose_code = "";
                }
                else
                {
                    selfclose_code = "SC";
                }
                if (interlock == "True")
                {
                    selfclose_code = "I";
                }


                output = "DRA" + d_height + "-" + drawerwidth + "SS" + c_style + "F" + track_type + d_load + selfclose_code + pull_code + key_type + label_holder;
            }
            else if ((specialdesign != "A") && (specialdesign != "Acid/ Base Corrosive Storage") && (specialdesign != "H") && (specialdesign != "Fume Hood General Storage") && (specialdesign != "S") && (specialdesign != "Sink"))
            {
                output = "BP" + drawerwidth.ToString() + "-" + d_height.ToString();
            }
            return output;
        }
        public static string drawerheadname(string cstyle, int drawerheight, int drawerwidth, string pullstyle, int lockornot, int cardholderornot, string specde, string dload)
        {
            string drawerhead_name = "";
            if ((specde != "Sink") && (specde != "S") && (cstyle != "WH") && (dload != "0") &&
                                             ((specde != "A")) && ((specde != "Acid/ Base Corrosive Storage")) && (specde != "H") && (specde != "Fume Hood General Storage"))
            {

                //cstyle drawerheight drawerwidth pullstyle=pulltype lock cardholder
                string drawerheadname = "";
                string pull_code = "";
                string key_type = "";
                string label_holder = "";

                if (cstyle == "PH")
                {
                    drawerheadname = "_FO";

                }
                else if (cstyle == "PN")
                {
                    drawerheadname = "_IN";

                }
                else if (cstyle == "SN")
                {
                    drawerheadname = "_IN";

                }
                else if (cstyle == "SH")
                {
                    drawerheadname = "_FO";
                }
                else
                {
                    drawerheadname = "_FO_HNM";
                }

                if ((pullstyle == "Flush ABS") || (pullstyle == "P1"))
                {
                    pull_code = "SP";
                }
                else if ((pullstyle == "Flush Aluminum") || (pullstyle == "P2"))
                {
                    pull_code = "AP";
                }
                else if ((pullstyle == "Raised Wire 96 mm") || (pullstyle == "P3"))
                {
                    pull_code = "378";
                }
                else if ((pullstyle == "Raised Wire 4\"") || (pullstyle == "P4"))
                {
                    pull_code = "400";
                }
                else if ((pullstyle == "Raised Wire 128 mm") || (pullstyle == "P5"))
                {
                    pull_code = "504";
                }
                else
                {
                    pull_code = "";
                }
                if (lockornot == 0)
                {
                }
                else
                {
                    key_type = "K0";
                }
                if (cardholderornot == 1)
                {
                    label_holder = "L2";
                }

                drawerhead_name = "DRH" + drawerheadname + drawerheight.ToString() + "-" + drawerwidth + cstyle + pull_code + key_type + label_holder;
            }
            else
            {
                if ((specde == "Sink") || (specde == "S") || (dload == "0") ||
                                            (specde == "A") || (specde == "Acid/ Base Corrosive Storage") || (specde == "H") || (specde == "Fume Hood General Storage"))
                {
                    // drawerhead_name = "BP" + drawerwidth.ToString() + "-" + drawerheight.ToString();
                }
            }
            return drawerhead_name;
        }
        public static string drawerbodyname(int drawerheight, int drawerwidth, string c_style, string d_load1, string track, int tracktype, string ctype, string specde)
        {
            string drawerbody_name = "";

            if (tracktype != 1)
            {
                tracktype = 1;
            }
            if ((specde != "Sink") && (specde != "S") && (d_load1 != "0") &&
                                            ((specde != "A")) && ((specde != "Acid/ Base Corrosive Storage")) && (specde != "H") && (specde != "Fume Hood General Storage"))
            {
                string drawerheadname_code = "";
                string track_index = "";
                int selfclose = 0;
                if (((ctype == "M") || (ctype == "G") || (ctype == "Mobile") || (ctype == "Glide")) && ((track == "T2L") || (track == "T5L") || (track == "T8L")))
                {
                    selfclose = 2;
                }
                else if ((track == "T3") || (track == "T6") || (track == "T9"))
                {
                    selfclose = 1;
                }
                else
                {
                    selfclose = 0;
                }
                if (selfclose == 2)
                {
                    tracktype = 1;
                }
                else
                {
                    if (((ctype == "M") || (ctype == "G") || (ctype == "Mobile") || (ctype == "Glide")) && (d_load1 == "100"))
                    {
                        tracktype = 0;
                    }
                    else
                    {
                        tracktype = 1;
                    }
                }
                if (c_style == "PH")
                {


                }
                else if (c_style == "PN")
                {


                }
                else if (c_style == "SN")
                {


                }
                else
                {
                    drawerheadname_code = "NM";
                }



                if (d_load1 == "100")
                {
                    if (selfclose == 1)
                    {
                        if (tracktype == 1)
                        {
                            track_index = "5001.ECD.450";
                        }
                        else
                        {
                            track_index = "5001.ECD.355";
                        }
                    }
                    else if (selfclose == 2)
                    {
                        if (tracktype == 1)
                        {
                            track_index = "5019";
                        }
                        else
                        {
                            track_index = "";
                        }
                    }
                    else
                    {
                        if (tracktype == 1)
                        {
                            track_index = "5043";
                        }
                        else
                        {
                            track_index = "5005";
                        }
                    }

                }
                else if (d_load1 == "150")
                {
                    track_index = "5000.H";
                }
                else
                {
                    if (selfclose == 1)
                    {
                        track_index = "5210.ECD";
                    }
                    else if (selfclose == 2)
                    {
                        track_index = "5219";
                    }
                    else
                    {
                        track_index = "5210";
                    }
                }
                // drawerheight drawerwidth
                drawerbody_name = "DRBU" + drawerheight + "-" + drawerwidth + "SS" + drawerheadname_code + "-" + track_index;


            }


            return drawerbody_name;
        }
        public static string trackname(string ctype, string track, int d_load1, int tracktype, string c_style, string specde)
        {
            string drawertrack_name = "";
            if ((specde != "Sink") && (specde != "S") && (d_load1 != 0) &&
                                            (specde != "A") && (specde != "Acid/ Base Corrosive Storage") && (specde != "H") && (specde != "Fume Hood General Storage"))
            {
                string drawertrack_extention = "";
                string drawertrack_type = "";
                string selfclose_code = "";
                int selfclose;

                /// tracktype d_load1 track c_style

                if (((ctype == "M") || (ctype == "G") || (ctype == "Mobile") || (ctype == "Glide")) && ((track == "T2L") || (track == "T5L") || (track == "T8L")))
                {
                    selfclose = 2;
                }
                else if ((track == "T3") || (track == "T6") || (track == "T9"))
                {
                    selfclose = 1;
                }
                else
                {
                    selfclose = 0;
                }
                if (selfclose == 2)
                {
                    tracktype = 1;
                }
                else
                {
                    if (((ctype == "M") || (ctype == "G") || (ctype == "Mobile") || (ctype == "Glide")) && (Convert.ToInt32(d_load1) == 100))
                    {
                        tracktype = 0;
                    }
                    else
                    {
                        tracktype = 1;
                    }
                }
                if (tracktype == 1)
                {
                    drawertrack_extention = "F";
                }
                else
                {
                    drawertrack_extention = "T";
                }

                if (c_style == "WH")
                {

                    drawertrack_type = "NM";
                }
                else if (c_style == "PH")
                {

                    drawertrack_type = "HYBRID";
                }
                else if (c_style == "PN")
                {

                    drawertrack_type = "STANDARD";
                }
                else if ((c_style == "SN") || (c_style == "SH"))
                {

                    drawertrack_type = "STANDARD";
                }
                else
                {

                }
                if (selfclose == 0)
                {
                    selfclose_code = "";
                }
                else if (selfclose == 1)
                {
                    selfclose_code = "SC";
                }
                else
                {
                    selfclose_code = "I";
                }

                drawertrack_name = "DT" + drawertrack_extention + d_load1 + selfclose_code + "-" + "ZBFL18-" + drawertrack_type;
            }
            return drawertrack_name;
        }
        public static List<string> fromtracktoclip(string trackname)
        {
            List<string> output = new List<string>();
            try
            {
                string[] ouut = ConfigurationSettings.AppSettings[trackname].Split('-');
                output = ouut.ToList();
            }
            catch { }
            return output;
        }
        public static List<List<string>> drawerParts(string model, string cstyle, int drawer_h, int drawer_w, string drawer_load, int drawer_cardholder, int drawer_lock, string pullstyle, string track,
            string specialdesign)
        {
            List<string> drawercate = drawerCate(drawer_load, track);
            string selfcls = drawercate[0];
            string interlock = drawercate[1];
            string extsion = drawercate[2];

            string drawerhead = drawerheadname(cstyle, drawer_h, drawer_w, pullstyle, drawer_lock, drawer_cardholder, specialdesign, drawer_load);
            string drawerbody = drawerbodyname(drawer_h, drawer_w, cstyle, drawer_load, track, 1, model, specialdesign);
            string drawertrack = trackname(model, track, Convert.ToInt32(drawer_load), 1, cstyle, specialdesign);
            List<string> drawertrackclips = fromtracktoclip(drawertrack);
            try
            {
                List<string> type = new List<string>() { "Drawerhead", "Drawerbody", "DrawerTrackClips", "DrawerTrackClips" };
                List<string> name = new List<string>() { drawerhead, drawerbody, drawertrackclips[0], drawertrackclips[1] };
                List<string> num = new List<string>() { "1", "1", "2", "2" };
                return new List<List<string>>() { name, num, type };
            }
            catch
            {
                return new List<List<string>>();
            }
        }
        public static double powder_drawerhead(int w, int h)
        {
            double output = (w * h + 0.69 * w + 0.69 * h) * 2 * 1.05 / 5760;
            return output;
        }
        public static string wooddrawerhead(int drawerw, int drawerh, string pullstyle, int lockornot, int cardholderornot)
        {
            string output = "DRH_FO";
            output += drawerh;
            output += drawerw;
            output += "WH";
            string pull_code = "";
            if (pullstyle == "P1")
            {
                pull_code = "SP";
            }
            else if (pullstyle == "P2")
            {
                pull_code = "AP";
            }
            else if (pullstyle == "P3")
            {
                pull_code = "378";
            }
            else if (pullstyle == "P4")
            {
                pull_code = "400";
            }
            else if (pullstyle == "P5")
            {
                pull_code = "504";
            }
            output += pull_code;

            string key_type = "";
            if (lockornot == 0)
            {
            }
            else
            {
                key_type = "K0";
            }
            output += key_type;
            string label_holder = "";
            if (cardholderornot == 1)
            {
                label_holder = "L2";
            }
            output += label_holder;
            return output;

        }
        public static List<List<string>> drawerAcc(string trackname, int lockornot, string bywhat, string specde, string cstyle,
            int drawer_w, int drawer_h, string pullstyle, int drawer_l, int drawer_ch)
        {
            List<List<string>> output = new List<List<string>>();

            List<string> name = new List<string>();
            List<int> num = new List<int>();
            List<string> type = new List<string>();
            List<string> num_string = new List<string>();
            if ((cstyle != "WH") && (cstyle.Substring(0, 1) != "S"))
            {
                name.Add("Powder"); num_string.Add(powder_drawerhead(drawer_w, drawer_h).ToString("0.00")); type.Add("Powder");
            }


            if ((specde != "Sink") && (specde != "S") && (trackname != "") &&
                                            (specde != "A") && (specde != "Acid/ Base Corrosive Storage") && (specde != "H") && (specde != "Fume Hood General Storage"))
            {
                int h = drawer_h;
                int w = drawer_w;
                name.Add(trackname); num.Add(2); type.Add("Track");

                name.Add("Foam " + (0.4048 + 0.0152 * w + 0.0241 * h).ToString()); num.Add(1); type.Add("Foam");

                name.Add("CR#8x.50TS"); type.Add("Screw(Truss-head)"); num.Add(8);



                name.Add("P2-51");
                if (w >= 30)
                {
                    num.Add(2);
                }
                else
                {
                    num.Add(1);
                }
                type.Add("Handle");

                if (lockornot == 1)
                {
                    if (bywhat == "Drawer")
                    {
                        name.Add("K17D");
                        num.Add(1);
                        type.Add("Lock");
                    }
                    else if (bywhat == "Hasp")
                    {
                        name.Add("Hasp");
                        num.Add(1);
                        type.Add("Hasp");
                    }
                    else
                    {
                        name.Add("K17");
                        num.Add(1);
                        type.Add("Lock");
                    }
                }

            }
            if (cstyle == "WH")
            {
                name.Add(wooddrawerhead(drawer_w, drawer_h, pullstyle, drawer_l, drawer_ch));
                num.Add(1);
                type.Add("Wood Drawer Head");
            }

            for (int i = 0; i < num.Count; i++)
            {
                num_string.Add(num[i].ToString());
            }

            output.Add(name);
            output.Add(num_string);
            output.Add(type);
            return output;

        }
        public static List<List<string>> drawerAss(string specde, string cstyle)
        {
            string[] name = { "Assembly" };
            string[] num = { "1" };
            string[] num_1 = { "0.5" };
            List<List<string>> output = new List<List<string>>();
            if ((specde != "Sink") && (specde != "S") &&
                                            (specde != "A") && (specde != "Acid/ Base Corrosive Storage") && (specde != "H") && (specde != "Fume Hood General Storage"))
            {
                output.Add(name.ToList());
                if (cstyle != "WH")
                {
                    output.Add(num.ToList());
                }
                else
                {
                    output.Add(num_1.ToList());
                }
            }
            else
            {
                output.Add(new List<string>());
                output.Add(new List<string>());
            }

            return output;
        }
        public static int trackToLoad(string track, int drawerh, int drawerw, int w)
        {
            string load = "";
            if (track == "T1")
            {
                if (drawerh < 12)
                {
                    load = "100";
                }
                else
                {
                    load = "150";
                }
            }
            else if ((track == "T2") || (track == "T2L") || (track == "T3") || (track == "T3L"))
            {
                if (drawerh < 12)
                {
                    load = "100";
                }
                else
                {
                    load = "200";
                }
            }
            else if (track == "T4")
            {
                if ((drawerw != w) || (w <= 30))
                {
                    if (drawerh < 9)
                    {
                        load = "100";
                    }
                    else
                    {
                        load = "150";
                    }
                }
                else
                {
                    load = "150";
                }
            }
            else if ((track == "T5") || (track == "T5L") || (track == "T6") || (track == "T6L"))
            {
                if ((drawerw != w) || (w <= 30))
                {
                    if (drawerh < 9)
                    {
                        load = "100";
                    }
                    else
                    {
                        load = "200";
                    }
                }
                else
                {
                    load = "200";
                }
            }
            else if (track == "T7")
            {
                if ((drawerw != w) || (w <= 30))
                {
                    if (drawerh < 6)
                    {
                        load = "100";
                    }
                    else
                    {
                        load = "150";
                    }
                }
                else
                {
                    load = "150";
                }
            }
            else if ((track == "T9") || (track == "T8") || (track == "T8L") || (track == "T9L"))
            {
                if ((drawerw != w) || (w <= 30))
                {
                    if (drawerh < 6)
                    {
                        load = "100";
                    }
                    else
                    {
                        load = "200";
                    }
                }
                else
                {
                    load = "200";
                }
            }
            return Convert.ToInt32(load);
        }
        public static List<int> index012(int full_w, List<int> drawer_h, List<int> drawer_w)
        {
            List<int> dr_ext = new List<int>();
            if ((full_w < 20) || ((full_w >= 20) && (drawer_h.Count == 1)))
            {
                double big1 = drawer_h[0] * drawer_w[0];
                dr_ext.Add(1);
                int ind = 0;
                for (int i = 1; i < drawer_h.Count; i++)
                {
                    if (drawer_w[i] * drawer_h[i] > big1)
                    {
                        big1 = drawer_w[i] * drawer_h[i];
                        dr_ext[ind] = 0;
                        ind = i;
                        dr_ext.Add(1);
                    }
                    else
                    {
                        dr_ext.Add(0);
                    }
                }
            }
            else
            {
                double big1 = 0;
                int ind1 = 0;
                double big2 = 0;
                int ind2 = 0;
                if (drawer_h[0] * drawer_w[0] >= drawer_h[1] * drawer_w[1])
                {
                    big1 = drawer_h[0] * drawer_w[0];
                    ind1 = 0;
                    dr_ext.Add(1);
                    big2 = drawer_h[1] * drawer_w[1];
                    ind2 = 1;
                    dr_ext.Add(2);
                }
                else
                {
                    big1 = drawer_h[1] * drawer_w[1];
                    ind1 = 1;
                    dr_ext.Add(2);
                    big2 = drawer_h[0] * drawer_w[0];
                    ind2 = 0;
                    dr_ext.Add(1);
                }
                for (int i = 2; i < drawer_h.Count; i++)
                {
                    if (drawer_w[i] * drawer_h[i] > big1)
                    {
                        dr_ext[ind2] = 0;
                        ind2 = ind1;
                        big2 = big1;
                        dr_ext[ind1] = 2;
                        ind1 = i;
                        big1 = drawer_w[i] * drawer_h[i];
                        dr_ext.Add(1);
                    }
                    else if ((drawer_w[i] * drawer_h[i] <= big1) && (drawer_w[i] * drawer_h[i] > big2))
                    {
                        dr_ext[ind2] = 0;
                        ind2 = i;
                        big2 = drawer_w[i] * drawer_h[i];
                        dr_ext.Add(2);
                    }
                    else
                    {
                        dr_ext.Add(0);
                    }
                }
            }
            return dr_ext;
        }
        public static string drawerbodynodrbu(string drawerbodyname)
        {
            string output = "";

            for (int i = 4; i < drawerbodyname.Count(); i++)
            {
                output += drawerbodyname.Substring(i, 1);
            }


            return output;
        }
        public static bool selfclose(string track)
        {
            bool slfcl = false;
            if ((track == "T3") || (track == "T6") || (track == "T9"))
            {
                slfcl = true;
            }
            return slfcl;
        }
        public static bool interlock(string model, string track)
        {
            bool intl = false;
            if ((model != "M") && (model != "G"))
            {

            }
            else
            {
                if ((track == "T2L") || (track == "T5L") || (track == "T8L"))
                {
                    intl = true;
                }
            }
            return intl;
        }
        public static bool fullExtension(string trackPro, int trackLoad)
        {
            bool output = false;
            if ((trackPro != "T2L")&&(trackPro != "T8L")&&(trackPro!="T9"))
            {
                if (trackLoad == 100)
                {
                    output = false;
                }
                else
                {
                    output = true;
                }
            }
            else
            {
                output = true;
            }
            return output;
        }

        ///////////Something New
        public static string drawerName_New(int drawerHeight, int drawerWidth, string drawerStyle, string trackname, string pullPro, int lockN, int chN)
        {
            string output = "";
            Dictionary<int, string> lockDic = new Dictionary<int, string>() { { 0, "" }, { 1, "K0" } };
            Dictionary<int, string> chDic = new Dictionary<int, string>() { { 0, "" }, { 1, "L2" } };

            string selfClose = trackname.Substring(6, 1);
            if (selfClose == "-")
            {
                selfClose = "";
            }
            else if (selfClose == "S")
            {
                selfClose = "SC";
            }
            else if (selfClose == "I")
            {
                selfClose = "I";
            }

            if ((pullPro == "Flush ABS") || (pullPro == "P1"))
            {
                pullPro = "SP";
            }
            else if ((pullPro == "Flush Aluminum") || (pullPro == "P2"))
            {
                pullPro = "AP";
            }
            else if ((pullPro == "Raised Wire 96 mm") || (pullPro == "P3"))
            {
                pullPro = "378";
            }
            else if ((pullPro == "Raised Wire 4\"") || (pullPro == "P4"))
            {
                pullPro = "400";
            }
            else if ((pullPro == "Raised Wire 128 mm") || (pullPro == "P5"))
            {
                pullPro = "504";
            }
            else
            {
                pullPro = "";
            }

            output = "DRA" + drawerHeight.ToString() + "-" + drawerWidth.ToString() + "SS" + drawerStyle + "F" + trackname.Substring(2, 1) + trackname.Substring(3, 3)
                + selfClose + pullPro + lockDic[lockN] + chDic[chN];
            return output;
        }
        public static string drawerHeadName_New(string drawerStyle, string puuPro, int lockN, int chN, int drawerWidth, int drawerHeight)
        {
            string output = "";

            string drawerheadname = "";
            string pull_code = "";
            string key_type = "";
            string label_holder = "";

            if (drawerStyle == "PH")
            {
                drawerheadname = "_FO";

            }
            else if (drawerStyle == "PN")
            {
                drawerheadname = "_IN";

            }
            else if (drawerStyle == "SN")
            {
                drawerheadname = "_IN";

            }
            else if (drawerStyle == "SH")
            {
                drawerheadname = "_FO";
            }
            else
            {
                drawerheadname = "_FO_HNM";
            }

            if ((puuPro == "Flush ABS") || (puuPro == "P1"))
            {
                pull_code = "SP";
            }
            else if ((puuPro == "Flush Aluminum") || (puuPro == "P2"))
            {
                pull_code = "AP";
            }
            else if ((puuPro == "Raised Wire 96 mm") || (puuPro == "P3"))
            {
                pull_code = "378";
            }
            else if ((puuPro == "Raised Wire 4\"") || (puuPro == "P4"))
            {
                pull_code = "400";
            }
            else if ((puuPro == "Raised Wire 128 mm") || (puuPro == "P5"))
            {
                pull_code = "504";
            }
            else
            {
                pull_code = "";
            }
            if (lockN == 0)
            {
            }
            else
            {
                key_type = "K0";
            }
            if (chN == 1)
            {
                label_holder = "L2";
            }

            output = "DRH" + drawerheadname + drawerHeight.ToString() + "-" + drawerWidth + drawerStyle + pull_code + key_type + label_holder;

            return output;
        }
        public static string drawerBodyName_New(int drawerWidth, int drawerHeight, string drawerStyle, string trackName)
        {
            string output = "";
            string track_index = "";
            if (drawerStyle == "WH")
            {
                drawerStyle = "NM";
            }
            else
            {
                drawerStyle = "";
            }

            string trackLoad = trackName.Substring(3, 3);
            string trackType = trackName.Substring(6, 1);
            int selfclose = 0;
            if (trackType == "-")
            {
                selfclose = 0;
            }
            else if (trackType == "S")
            {
                selfclose = 1;
            }
            else
            {
                selfclose = 2;
            }
            trackType = trackName.Substring(2, 1);
            if (trackType == "F")
            {
                trackType = "1";
            }
            else
            {
                trackType = "0";
            }


            if (trackLoad == "100")
            {
                if (selfclose == 1)
                {
                    if (trackType == "1")
                    {
                        track_index = "5001.ECD.450";
                    }
                    else
                    {
                        track_index = "5001.ECD.355";
                    }
                }
                else if (selfclose == 2)
                {
                    if (trackType == "1")
                    {
                        track_index = "5019";
                    }
                    else
                    {
                        track_index = "";
                    }
                }
                else
                {
                    if (trackType == "1")
                    {
                        track_index = "5043";
                    }
                    else
                    {
                        track_index = "5005";
                    }
                }

            }
            else if (trackLoad == "150")
            {
                track_index = "5000.H";
            }
            else
            {
                if (selfclose == 1)
                {
                    track_index = "5210.ECD";
                }
                else if (selfclose == 2)
                {
                    track_index = "5219";
                }
                else
                {
                    track_index = "5210";
                }
            }
            output = "DRBU" + drawerHeight.ToString() + "-" + drawerWidth.ToString() + "SS" + drawerStyle + "-" + track_index;
            return output;
        }

    }
}
