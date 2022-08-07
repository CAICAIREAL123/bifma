using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class DoorParts
    {
        public static List<string> parts_door(int w, string model, int doorheight, string cstyle, int doorwidth, int zuoyou, string pullstyle, int lockornot, int cardholderornot,
            string specialdesign, int doormat, bool selfcloseforf)
        {

            List<string> output = new List<string>();
            if (cstyle != "WH")
            {
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
                    //if (selfcloseforf == true)
                    //{
                    //    output.Add("LC338SC-A");
                    //}
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
                    }
                    else
                    {
                        string name = "BP" + w.ToString() + "-" + doorheight.ToString();
                        output.Add(name);
                    }
                }
            }
            if (cstyle == "SN")
            {
                for (int i = 0; i < output.Count; i++)
                {
                    output[i] += "-SS";
                }
            }
            return output;
        }
        public static string wooddoorname(string hingestyle, int doorindex, int doormat, int doorw, int doorh,
           string pullstyle, int doorlock, int doorch)
        {
            Dictionary<string, string> dic_hinge = new Dictionary<string, string>()
            {
                { "LC Pivot Stainless", "H1" },
                {"5-Knuckle Stainless", "H2" },
                {"3-Knuckle Self-close", "H3" }
            };
            Dictionary<int, string> dic_doorcon = new Dictionary<int, string>()
            { {1,"L" }, {2, "R" } };
            Dictionary<string, string> dic_pullstyle = new Dictionary<string, string>()
            {
                {"Flush ABS", "P1" }, {"Flush Aluminum", "P2" }, {"Raised Wire 96 mm", "P3" },
                {"Raised Wire 4\"", "P4" }, {"Raised Wire 128 mm", "P5" }, {"Flush Stainless Steel", "P6" }
            };
            string output = "";
            try
            {
                output += dic_hinge[hingestyle];
            }
            catch
            {
                output += hingestyle;
            }
            output += dic_doorcon[doorindex];
            if (doormat == 1)
            {
                output += "S";
            }
            else
            {
                output += "G";
            }
            output += doorw.ToString();
            output += "X";
            output += doorh.ToString();
            try
            {
                output += dic_pullstyle[pullstyle];
            }
            catch
            {
                output += pullstyle;
            }
            if (doorlock == 1)
            {
                output += "K";
            }
            if (doorch == 1)
            {
                output += "L1";
            }

            return output;
        }
        public static double powder_door(int w, int h)
        {
            Double output = (w * h + 0.69 * w + 0.69 * h) * 4 * 1.05 / 5760;
            return output;
        }
        public static List<List<string>> acc_door(int lockornot, int cardholderornot, string bywhat, string hingestyle, int doorconfi, string specialdesign, int doorheight, int doorwidth, int doormat, string cstyle, string pullsty, string selfclosestring)
        {
            Dictionary<string, string> dic_hin = new Dictionary<string, string>()
            {
                {"LC Pivot Stainless", "H1" }, { "5-Knuckle Stainless","H2" }, { "3-Knuckle Self-close", "H3" },
                {"H1", "H1" }, { "H2","H2" }, { "H3", "H3" },
            };

            List<string> name = new List<string>();
            List<string> num = new List<string>();
            List<string> shigesha = new List<string>();


            string cat = "";
            int w = doorwidth;
            int h = doorheight;
            if ((doormat == 1) && ((specialdesign != "F") && (specialdesign != "Flammable Liquid Storage")))
            {
                cat = "LC15";
            }
            else if ((specialdesign == "F") || (specialdesign == "Flammable Liquid Storage"))
            {
                cat = "LC35";
            }

            //         < add key = "LC35O" value = "528-1_12_12_17_17_0.38_0.01_0-0.4" />

            //            < add key = "LC35I" value = "1040-1_12_12_29_29_0.24_0.015-0.5" />
            if ((cat == "LC15") || (cat == "LC35"))
            {
                name = new List<string>() { "785", "#270", /*"P2-51",*/ "SS6-3  x.25PM/NL", "SS8-32x.25MU/N", "CR#8X.50TS", ".625 TW 86x102", dic_hin[hingestyle] };
                num = new List<string>() { "2", "1", /*"1",*/ "4", "4", "2", "1", "2" };
                shigesha = new List<string>() { "Bumper", "Roller Catch", /*"Handle", */"Screw(Pan-head)", "Screw(Pan-head)", "Screw(Truss-head)", "Tri-wall", "Hinge" };
                if (cat == "LC15")
                {
                    name.Add("P2-51");
                    num.Add("1");
                    shigesha.Add("Handle");
                }
                else
                {
                    if (selfclosestring == "SC")
                    {
                        name.Add("LC338SC-E"); num.Add("1"); shigesha.Add("");
                        name.Add("LC338SC-C"); num.Add("1"); shigesha.Add("");
                        name.Add("LC338SC-D1"); num.Add("1"); shigesha.Add("");
                        name.Add("LC338SC-D"); num.Add("1"); shigesha.Add("");
                        name.Add("LC338SC-A"); num.Add("1"); shigesha.Add("");
                    }
                }
            }
            else
            {
                double glassw = w - 6.31;
                double glassh = h - 6.69;
                name.Add("#141-CL"); num.Add(((glassh + glassw) * 2 - 2.5).ToString("0.00")); shigesha.Add("PVC CHANNEL");
                if (doorconfi < 4)
                {
                    name.Add("#270"); num.Add("1"); shigesha.Add("ROLLER CATCH");
                    name.Add("FOLDC-01"); num.Add("1"); shigesha.Add("FLUSH OVERLAY CATCH/ROLLER BRKT");
                }
                name.Add("H1"); if (h > 50) { num.Add("3"); } else { num.Add("2"); }
                shigesha.Add("HINGE");
                name.Add("785"); num.Add("2"); shigesha.Add("BUMPER");
                name.Add("25GL"); num.Add((glassw * glassh).ToString("0.00")); shigesha.Add("1/4\"GLASS");
                name.Add("P2-51"); num.Add("1"); shigesha.Add("Handle");
            }
            if (specialdesign == "A")
            {
                name.Add("H_700_DOOR"); num.Add(((w - 24 + 21.875) * 22.4375).ToString("0.00")); shigesha.Add("ACID DOOR");

            }
            else if (specialdesign == "VP")
            {
                name.Add("DOOR SOUNDFOAM"); num.Add("1"); shigesha.Add("DOOR SOUNDFOAM");
            }
            if (lockornot == 1)
            {
                //if (bywhat == "Drawer")
                //{
                //    name.Add("K17D");
                //    num.Add("1");
                //    shigesha.Add("Lock");
                //}
                //else 
                if (bywhat == "Hasp")
                {
                    name.Add("Hasp");
                    num.Add("1");
                    shigesha.Add("Hasp");
                }
                else
                {
                    name.Add("K17");
                    num.Add("1");
                    shigesha.Add("Lock");
                }
            }
            if (doorconfi == 9)
            {
                name = new List<string>();
                num = new List<string>();
                shigesha = new List<string>();

            }
            if ((cstyle != "WH") && (cstyle.Substring(0, 1) != "S"))
            {
                name.Add("Powder");
                num.Add(powder_door(doorwidth, doorheight).ToString("0.00"));
                shigesha.Add("Powder");
            }
            else if (cstyle == "WH")
            {
                name.Add(wooddoorname(hingestyle, doorconfi, doormat, doorwidth, doorheight, pullsty, lockornot, cardholderornot));
                num.Add("1");
                shigesha.Add("WoodDoor");
            }

            List<List<string>> output = new List<List<string>>();
            output.Add(name);
            output.Add(num);
            output.Add(shigesha);
            return output;
        }
        public static List<List<string>> ass_door(string cstyle, string spedes, int doorw, int doorh, int doormat)
        {
            List<string> name = new List<string>() { "Assembly" };
            List<string> num = new List<string>() { "1" };
            List<string> num_1 = new List<string>() { "0.5" };
            List<string> num_0 = new List<string>() { "0" };
            double paintTime = 1 + (doorw + doorh - 42) / 25;
            name.Add("Paint");
            num.Add(paintTime.ToString("0.00"));
            num_1.Add(paintTime.ToString("0.00"));
            num_0.Add(paintTime.ToString("0.00"));
            List<List<string>> output = new List<List<string>>();
            output.Add(name.ToList());
            if ((spedes != "F") && (spedes != "Flammable Liquid Storage"))
            {
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
                output.Add(num_0.ToList());
            }
            if ((spedes == "A") || (spedes == "Acid/ Base Corrosive Storage"))
            {
                output[1][0] = (Convert.ToDouble(output[1][0]) + 1).ToString("0.00");
            }
            if (doormat > 1)
            {
                output[1][0] = "10";
            }
            if ((cstyle == "WH") || (cstyle.Substring(0, 1) == "S"))
            {
                output[1][1] = "0";
            }

            return output;
        }
        public static string fromoutdoortoformaldoorname(string outdoorname)
        {
            string output = "";
            string[] temp = outdoorname.Split('+')[0].Split('-');
            output += temp[0].Substring(0, 7);
            output += "-";
            output += temp[1] + "-" + temp[2];
            return output;
        }
        /// <summary>
        /// ///////////////////somethingnew
        /// </summary>
        public static List<string> doorList_New_3D(string doorKey, int confi, double doorWidth, double doorHeight, int lockN, int chN, string pullPro, string hingePro)
        {
            Database database = new Database();
            List<string> output = new List<string>();

            Dictionary<string, string> dic_pullcode = new Dictionary<string, string>()
            { {"Flush ABS", "SP"},{"Flush Aluminum","AP" }, {"Raised Wire 96 mm","378" },
                {"Raised Wire 4\"", "400" }, {"Raised Wire 128 mm", "504" },
                { "P1", "SP"},{"P2","AP" }, {"P3","378" },{"P4", "400" }, {"P5", "504" }};
            Dictionary<string, string> dic_pullstyle = new Dictionary<string, string>()
            { {"Flush ABS", "P1"},{"Flush Aluminum","P2" }, {"Raised Wire 96 mm","P3" },
                {"Raised Wire 4\"", "P4" }, {"Raised Wire 128 mm", "P5" },
                {"P1", "P1"},{"P2","P2" }, {"P3","P3" },{"P4", "P4" }, {"P5", "P5" }};
            Dictionary<string, string> dic_hinge = new Dictionary<string, string>()
            { {"LC Pivot Stainless", "H1"}, {"5-Knuckle Stainless", "H2" }, {"3-Knuckle Self-close", "H3" }
            , {"H1", "H1"}, {"H2", "H2" }, {"H3", "H3" }, {"", "" } };
            Dictionary<int, string> dic_conf = new Dictionary<int, string>() { { 1, "L" }, { 2, "R" } };

            string material = "P";
            double actualWidth = doorWidth;
            double actualHeight = doorHeight;
            double nameWidth = doorWidth;
            double nameHeight = doorHeight;
            string pullCode = dic_pullcode[pullPro];
            pullPro = dic_pullstyle[pullPro];
            string keyCode = "";
            string chCode = "";
            string nameCode = "";
            string outDoorName = "";
            string innerDoorName = "";
            string doorName = "";

            string doorType = "";
            string difh = "";
            string difw = "";
            MySqlDataReader rd = database.getMySqlReader_rd1("select * from detail_door where doormaster = '" + doorKey + "'");
            if (rd.Read())
            {
                doorType = rd.GetString("type");
                difh = rd.GetString("dif_h");
                difw = rd.GetString("dif_w");
            }
            database.CloseConnection_rd1();

            if (doorType == "Classic")
            {
                actualHeight -= Convert.ToDouble(difh);
                actualWidth -= Convert.ToDouble(difw);
                //nameHeight -= Math.Round(Convert.ToDouble(difh));
                //nameWidth -= Math.Round(Convert.ToDouble(difw));
                if (lockN == 1)
                {
                    keyCode = "K0";
                }
                if (chN == 1)
                {
                    chCode = "L2";
                }
                nameCode = doorKey;
                string[] doorKeyAray = doorKey.Split('-');
                outDoorName = doorKeyAray[0] + dic_conf[confi] + "O-" + nameHeight.ToString("0") + "-" + nameWidth.ToString("0") + "-" + doorKeyAray[1] + "-" + pullCode + keyCode+chCode;
                innerDoorName = doorKeyAray[0] + "I-" + nameHeight.ToString("0") + "-" + nameWidth.ToString("0") + "-" + doorKeyAray[1] + "-" + pullCode + keyCode + chCode;
                doorName = doorKeyAray[0] + dic_conf[confi] + "-" + nameHeight.ToString("0") + "-" + nameWidth.ToString("0") + "-" + doorKeyAray[1] + "-" + pullCode + keyCode + chCode;
            }
            else if (doorType == "Glass")
            {
                actualHeight -= Convert.ToDouble(difh);
                actualWidth -= Convert.ToDouble(difw);
                if (lockN == 1)
                {
                    keyCode = "K0";
                }
                if (chN == 1)
                {
                    chCode = "L2";
                }
                nameCode = doorKey;
                string[] doorKeyAray = doorKey.Split('-');
                outDoorName = doorKeyAray[0] + dic_conf[confi] + "O-" + nameHeight.ToString("0") + "-" + nameWidth.ToString("0") + "-" + doorKeyAray[1] + "-" + pullCode + keyCode + chCode;
                innerDoorName = "WC3461";
                doorName = doorKeyAray[0] + dic_conf[confi] + "-" + nameHeight.ToString("0") + "-" + nameWidth.ToString("0") + "-" + doorKeyAray[1] + "-" + pullCode + keyCode + chCode;
            }
            else
            {
                actualHeight -= Convert.ToDouble(difh);
                actualWidth -= Convert.ToDouble(difw);
                if (lockN == 1)
                {
                    keyCode = "K0";
                }
                if (chN == 1)
                {
                    chCode = "L2";
                }
                ///////////////////TypeHere for doorname
                string[] doorKeyAray = doorKey.Split('-');
                doorName = doorKeyAray[0] + dic_conf[confi] + "-" + nameHeight.ToString("0") + "-" + nameWidth.ToString("0") + "-" + doorKeyAray[1] + "-" + pullCode + keyCode + chCode;
            }

            string d3Name = material + "_" + confi + "_" + actualWidth.ToString("0.000000") + "_" + actualHeight.ToString("0.000000") + "_" + lockN.ToString() + "_" + chN.ToString()
                + "_" + pullPro + "_" + dic_hinge[hingePro];



            output = new List<string>() { material, dic_conf[confi].ToString(), actualWidth.ToString("0.000000"), actualHeight.ToString("0.000000"), lockN.ToString(), chN.ToString(), pullPro, dic_hinge[hingePro],
                d3Name, outDoorName, innerDoorName, doorName, nameWidth.ToString(), nameHeight.ToString(), doorType };
            return output;
            ///0:  material
            ///1:  confi
            ///2:  actualWidth.ToString("0.000000")
            ///3:  actualHeight.ToString("0.000000")
            ///4:  lockN.ToString()
            ///5:  chN.ToString()
            ///6:  pullPro
            ///7:  dic_hinge[hingePro]
            ///8:  d3Name
            ///9:  outDoorName
            ///10: innerDoorName
            ///11: doorName
            ///12: nameWidth
            ///13: nameHeight
            ///14: doorType
        }

    }
}
