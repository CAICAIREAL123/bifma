using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class three_prep
    {
        public static List<string> drawerlist(bool intl, int drawer_l, int drawer_ch, string model, int drawer_load, int drawer_h, int drawer_w,
            bool slfcl, int drawer_x, int drawer_y, string cstyle, string pullsty, string specialdesign, string track, int w, int h, int dr_ext)
        {
            List<string> drawerlist = new List<string>();

            bool ext = false;

            bool lk = false; bool ch = false;
            if (drawer_l == 1)
            {
                lk = true;
            }
            if (drawer_ch == 1)
            {
                ch = true;
            }
            if (((model == "M") || (model == "G")) && (drawer_load == 100))
            {
                ext = false;
            }
            else
            {
                ext = true;
            }
            if (intl == true)
            {
                ext = true;
            }

            drawerlist.Add(drawer_h.ToString());
            drawerlist.Add(drawer_w.ToString());
            drawerlist.Add(lk.ToString());
            drawerlist.Add(ch.ToString());
            drawerlist.Add(drawer_load.ToString());
            drawerlist.Add(slfcl.ToString());
            drawerlist.Add(intl.ToString());
            drawerlist.Add(ext.ToString());
            drawerlist.Add(drawer_x.ToString());
            drawerlist.Add(drawer_y.ToString());
            drawerlist.Add(DrawerParts.drawername(cstyle, drawer_h, drawer_w, drawer_load.ToString(), drawer_ch.ToString(), drawer_l.ToString(), pullsty, slfcl.ToString(), intl.ToString(), ext.ToString(), specialdesign));
            string drawerhead_temp = three_prep.drawerheadname(cstyle, drawer_h, drawer_w, pullsty, drawer_l, drawer_ch, specialdesign, drawer_load.ToString());
            drawerlist.Add(drawerhead_temp);

            string drawerbody_temp = DrawerParts.drawerbodyname(drawer_h, drawer_w, cstyle, drawer_load.ToString(), track, 1, model, specialdesign);
            drawerlist.Add(drawerbody_temp);
            drawerlist.Add(DrawerParts.drawerbodynodrbu(drawerbody_temp));
            drawerlist.Add(DrawerParts.trackname(model, track, drawer_load, 1, cstyle, specialdesign));
            drawerlist.Add(drawerhead_temp.Substring(3, drawerhead_temp.Count() - 3));


            int v = 0;
            for (v = 0; v < drawerhead_temp.Count(); v++)
            {
                try
                {
                    int temp = Convert.ToInt32(drawerhead_temp.Substring(v, 1));
                    break;
                }
                catch { }
            }
            drawerlist.Add(drawerhead_temp.Substring(v, drawerhead_temp.Count() - v));
            if ((w >= 30) && (drawer_w == w))
            {
                drawerlist.Add(true.ToString());
            }
            else
            {
                drawerlist.Add(false.ToString());
            }

            // dr_ext = 
            drawerlist.Add(dr_ext.ToString());
            return drawerlist;
        }
        public static string drawerheadname(string cstyle, int drawerheight, int drawerwidth, string pullstyle, int lockornot, int cardholderornot, string specde, string dload)
        {
            string drawerhead_name = "";
            if ((specde != "Sink") && (specde != "S") /*&& (cstyle != "WH")*/ && (dload != "0") &&
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
        public static List<string> parts_door(int w, string model, int doorheight, string cstyle, int doorwidth, int zuoyou, string pullstyle, int lockornot, int cardholderornot,
            string specialdesign, int doormat, bool selfcloseforf, string hinge)
        {

            List<string> output = new List<string>();
            string pull_code = "";
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
            string key_type = "";
            if (lockornot == 0)
            {
            }
            else
            {
                key_type = "K0";
            }
            string label_holder = "";
            if (cardholderornot == 1)
            {
                label_holder = "L2";
            }
            if ((specialdesign == "F") || (specialdesign == "Flammable Liquid Storage"))
            {
                if ((cstyle == "PN") || (cstyle == "SN"))
                {
                    if (doorwidth == w)
                    {
                        doorwidth--;
                    }
                    doorwidth--;
                }
                string temp = "LC35I";
                if (zuoyou == 1)
                {
                    temp += "L";
                }
                else if (zuoyou == 2)
                {
                    temp += "R";
                }
                temp += "-" + doorheight + "-" + doorwidth;
                temp += "-" + pull_code + key_type + label_holder;
                output.Add(temp);
                temp = "LC35O";
                if (zuoyou == 1)
                {
                    temp += "L";
                }
                else if (zuoyou == 2)
                {
                    temp += "R";
                }
                temp += "-" + doorheight + "-" + doorwidth;
                temp += "-" + pull_code + key_type + label_holder;
                output.Add(temp);
                output.Add("LC332RSCHCSS");
                if (selfcloseforf == true)
                {
                    output.Add("LC338SC-A");
                }
            }
            else if (((model == "S") || (model == "Storage") || (model == "W") || (model == "Wall")) && (doormat > 1))
            {
                if ((cstyle == "PN") || (cstyle == "SN"))
                {
                    if (doorwidth == w)
                    {
                        doorwidth--;
                    }
                    doorwidth--;
                }
                string outdoor = "";
                List<string> inner = new List<string>();
                if (doorheight > 49)
                {
                    outdoor = "SC35O";
                    if (zuoyou == 1)
                    {
                        outdoor += "L";
                    }
                    else if (zuoyou == 2)
                    {
                        outdoor += "R";
                    }
                    else
                    {
                        outdoor += "S";
                    }
                    outdoor += "-" + doorheight + "-" + doorwidth;
                    outdoor += "-" + pull_code + key_type + label_holder;
                    int l = doorheight;
                    inner.Add("SC4180-I4AF");
                    inner.Add("SC4180-I4A1F");
                    bool ifcon = true;
                    int times = 1;
                    while (ifcon == true)
                    {
                        if (l <= 17)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                inner.Add("WC3460-12");
                            }
                            ifcon = false;
                        }
                        else if (l <= 20)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                inner.Add("WC3460-15");
                            }
                            ifcon = false;
                        }
                        else if (l <= 26)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                inner.Add("WC346-21");
                            }
                            ifcon = false;
                        }
                        else if (l <= 35)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                inner.Add("WC346-30");
                            }
                            ifcon = false;
                        }
                        else if (l <= 70)
                        {
                            l /= 2;
                            times = 2;
                        }
                        else
                        {
                            while (l >= 30)
                            {
                                inner.Add("WC3460-30");
                                l -= 30;
                            }
                            if (l > 0)
                            {
                                ifcon = true;
                            }
                            else
                            {
                                ifcon = false;
                            }
                        }
                    }
                    l = doorwidth;
                    inner.Add("WC3461-" + l);
                    inner.Add("WC3461-" + l);
                    if (l <= 17)
                    {
                        inner.Add("WC3460-12");
                    }
                    else if (l <= 20)
                    {
                        inner.Add("WC3460-15");
                    }
                    else if (l <= 26)
                    {
                        inner.Add("WC3460-21");
                    }
                    else if (l <= 35)
                    {
                        inner.Add("WC3460-30");
                    }
                }
                else
                {
                    model = "W";
                }
                if ((model == "Wall") || (model == "W"))
                {
                    outdoor = "WC35O";
                    if (zuoyou == 1)
                    {
                        outdoor += "L";
                    }
                    else if (zuoyou == 2)
                    {
                        outdoor += "R";
                    }
                    else
                    {
                        outdoor += "S";
                    }
                    outdoor += "-" + doorheight + "-" + doorwidth;
                    outdoor += "-" + pull_code + key_type + label_holder;
                    int l = doorheight;
                    inner.Add("WC3490-" + l);
                    inner.Add("WC3491-" + l);
                    int times = 1;
                    bool ifcon = true;
                    while (ifcon == true)
                    {
                        if (l <= 17)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                inner.Add("WC3460-12");
                            }
                            ifcon = false;
                        }
                        else if (l <= 20)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                inner.Add("WC3460-15");
                            }
                            ifcon = false;
                        }
                        else if (l <= 26)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                inner.Add("WC3460-21");
                            }
                            ifcon = false;
                        }
                        else if (l <= 35)
                        {
                            for (int i = 0; i < times; i++)
                            {
                                inner.Add("WC3460-30");
                            }
                            ifcon = false;
                        }
                        else if (l <= 70)
                        {
                            l /= 2;
                            times = 2;
                        }
                    }
                    l = doorwidth;
                    inner.Add("WC3461-" + l);
                    if (l <= 17)
                    {
                        inner.Add("WC3460-12");
                    }
                    else if (l <= 20)
                    {
                        inner.Add("WC3460-15");
                    }
                    else if (l <= 26)
                    {
                        inner.Add("WC3460-21");
                    }
                    else if (l <= 35)
                    {
                        inner.Add("WC3460-30");
                    }
                    inner.Add("WC3461-" + l);
                    inner.Add("WC3461-" + l);
                }
                output.Add(outdoor);
                output.AddRange(inner);
            }
            else
            {
                if (zuoyou != 9)
                {
                    if (cstyle != "WH")
                    {
                        if ((cstyle == "PN") || (cstyle == "SN"))
                        {
                            if (doorwidth == w)
                            {
                                doorwidth--;
                            }
                            doorwidth--;

                        }
                        string wonengshuoshenmene = "Z";
                        if ((cstyle != "PN") && (cstyle != "SN"))
                        {
                            wonengshuoshenmene = "H";
                        }

                        string tem1 = "LC15" + doorheight.ToString();
                        string tem2 = "-" + doorwidth.ToString() + wonengshuoshenmene + "-R";



                        if (zuoyou == 1)
                        {
                            output.Add(tem1 + "I" + tem2);
                            output.Add(tem1 + "LO" + tem2 + pull_code + key_type + label_holder);
                        }
                        else
                        {
                            output.Add(tem1 + "I" + tem2);
                            output.Add(tem1 + "RO" + tem2 + pull_code + key_type + label_holder);
                        }

                    }
                    else
                    {
                        output.Add(DoorParts.wooddoorname(hinge, zuoyou, doormat, doorwidth, doorheight, pullstyle, lockornot, cardholderornot));
                    }
                }
                else
                {
                    string name = "BP" + w.ToString() + "-" + doorheight.ToString();
                    output.Add(name);
                }
            }

            return output;
        }
        public static List<string> doorlist(int door_h, int door_w, string cstyle, int door_l, int door_ch, int door_con, int door_mat, int door_x, int door_y, int w, string model,
            string pullsty, string specialdesign, string selfclosee, string hinge)
        {
            ///doorlis:
            ///0: h
            ///1: w
            ///2: lock(true/false)
            ///3: cardholder(true/false)
            ///4: doorconfiguration(1:left 2: right 41:sliding left 42:sliding right)
            ///5: doormaterial: {{"Solid", 1 },{"Framed Glass", 2 },{"Open Face", 0 },
            ///               { "Framed 1/8\" Glass", 3 }, { "Framed 1/4\" Glass", 4 }
            ///6: x
            ///7: y
            ///8: doorname
            ///9: innerdoorname
            ///10: outdoorname
            ///11：short doorname
            ///12: short outdoorname
            ///

            Dictionary<int, bool> dic_inttobool = new Dictionary<int, bool>() { { 0, false }, { 1, true } };
            Dictionary<string, bool> dictransstringtobool = new Dictionary<string, bool>() { { "NSC", false }, { "SC", true } };
            List<string> output = new List<string>();

            output.Add(door_h.ToString());//0
            int door_w_ = door_w;
            if ((cstyle == "PN") || (cstyle == "SN"))
            {
                if (door_w_ == w)
                {
                    door_w_ -= 2;
                }
                else
                {
                    door_w_--;
                }
            }
            output.Add(door_w_.ToString());//1
            output.Add(dic_inttobool[door_l].ToString());//2
            output.Add(dic_inttobool[door_ch].ToString());//3
            output.Add(door_con.ToString());//4
            output.Add(door_mat.ToString());//5
            output.Add(door_x.ToString());//6
            output.Add(door_y.ToString());//7
            List<string> doorparts_temp = three_prep.parts_door(w, model, door_h, cstyle, door_w, door_con, pullsty, door_l, door_ch,
                specialdesign, door_mat, dictransstringtobool[selfclosee], hinge);
            string doortempname = "";
            if (cstyle != "WH")
            {
                doortempname = DoorParts.fromoutdoortoformaldoorname(doorparts_temp[1]);
                output.Add(doortempname);//8
                output.AddRange(doorparts_temp);//9, 10
                output.Add(doortempname.Substring(4, doortempname.Count() - 4));//11
                output.Add(doorparts_temp[1].Substring(4, doorparts_temp[1].Count() - 4));//12
            }
            else
            {
                doortempname = doorparts_temp[0];
                output.Add(doortempname);//8
                output.Add(doortempname);//9
                output.Add("");//10
                output.Add(doortempname.Split('H')[1]);//11
                output.Add("");//12
            }



            return output;
        }
    }
}
