using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class VisualInfo
    {
        SWFunction swfunctions = new SWFunction();
        string masterPath = @"C:\Users\yue\Desktop\d3test\" + "master\\";
        string libraryPath = @"C:\Users\yue\Desktop\d3test\" + "library\\";
        string pdfPath = @"C:\Users\yue\Dropbox\PDF\";
        public static string getVisualName(string tag, string master, int w, int h, int d, string originalName)
        {
            string output = "";
            Dictionary<int, string> w_dic = new Dictionary<int, string>() { { 12, "12" }, { 15, "15" }, { 18, "1" }, { 21, "21" }, { 24, "2" }, { 30, "30" },
                { 36, "3"}, {42, "42" }, {48, "4" }, {60, "5" } };
            Dictionary<int, string> h_dic1 = new Dictionary<int, string>() { { 35, "20" }, { 32, "23" }, { 29, "30" } };
            Dictionary<int, string> h_dic2 = new Dictionary<int, string>() { { 19, "31" }, { 22, "32" }, { 25, "33" }, { 28, "34" } };
            Dictionary<int, string> h_dic3 = new Dictionary<int, string>() { { 35, "60" }, { 32, "65" }, { 29, "70" } };

            Dictionary<int, string> SIDELEFT1 = new Dictionary<int, string>() { { 35, "1000" }, { 32, "1005" }, { 29, "1060" } };
            Dictionary<int, string> SIDELEFT2 = new Dictionary<int, string>() { { 19, "1080" }, { 22, "1095" }, { 25, "1093" }, { 28, "1195" } };
            Dictionary<int, string> SIDERIGHT1 = new Dictionary<int, string>() { { 35, "1010" }, { 32, "1015" }, { 29, "1070" } };
            Dictionary<int, string> SIDERIGHT2 = new Dictionary<int, string>() { { 19, "1081" }, { 22, "1094" }, { 25, "1092" }, { 28, "1194" } };
            Dictionary<int, string> POSTCENTER1 = new Dictionary<int, string>() { { 3, "1286" }, { 6, "1290" }, { 9, "1287" }, { 12, "1300" }, { 15, "1301" }, { 18, "1302" }, { 21, "1305" }, { 24, "1310" }, { 27, "1325" }, { 30, "1320" } };

            if (tag == "sideleft")
            {
                if (master == "LC10")
                {
                    output += "LCU";
                    output += SIDELEFT1[h];
                }
                else if (master == "H-LC10")
                {
                    output += "LC";
                    output += SIDELEFT1[h];
                    output += "H";
                }
                else if (master == "NM-LC10")
                {
                    output += "LCU";
                    output += SIDELEFT1[h];
                    output += "HNM";
                }
                else if (master == "LC10-SS")
                {
                    output += "LCU";
                    output += SIDELEFT1[h];
                    output += "SS";
                }
                else if (master == "M-LC10")
                {
                    output += "LCU";
                    output += SIDELEFT2[h];
                    output += "M";
                }
                else if (master == "M-LC10-SS")
                {
                    output += "LCU";
                    output += SIDELEFT2[h];
                    output += "MSS";
                }
                else if (master == "MNM-LC10")
                {
                    output += "LCU";
                    output += SIDELEFT2[h];
                    output += "MNM";
                }
            }
            else if (tag == "sideright")
            {
                if (master == "LC10")
                {
                    output += "LCU";
                    output += SIDERIGHT1[h];
                }
                else if (master == "H-LC10")
                {
                    output += "LC";
                    output += SIDERIGHT1[h];
                    output += "H";
                }
                else if (master == "NM-LC10")
                {
                    output += "LCU";
                    output += SIDERIGHT1[h];
                    output += "HNM";
                }
                else if (master == "LC10-SS")
                {
                    output += "LCU";
                    output += SIDERIGHT1[h];
                    output += "SS";
                }
                else if (master == "M-LC10")
                {
                    output += "LCU";
                    output += SIDERIGHT2[h];
                    output += "M";
                }
                else if (master == "M-LC10-SS")
                {
                    output += "LCU";
                    output += SIDERIGHT2[h];
                    output += "MSS";
                }
                else if (master == "MNM-LC10")
                {
                    output += "LCU";
                    output += SIDERIGHT2[h];
                    output += "MNM";
                }
            }
            else if (tag == "back")
            {
                if (master == "LC11")
                {
                    output += "LC11";
                    output += h_dic1[h];
                    output += "-" + w_dic[w];
                }
                else if (master == "LC11-SS")
                {
                    output += "LC11";
                    output += h_dic1[h];
                    output += "-" + w_dic[w] + "SS";
                }
                else if (master == "HT-LC11")
                {
                    output += "LC11";
                    output += "20" + "HT";
                    output += "-" + w_dic[w];
                }
                else if (master == "HB-LC11")
                {
                    output += "LC11";
                    output += "20" + "HB";
                    output += "-" + w_dic[w];
                }
                else if (master == "HC-LC11")
                {
                    output += "LC11";
                    output += h_dic3[h] + "H";
                }
                else if (master == "M-LC11")
                {
                    output += "LC11";
                    output += h_dic2[h] + "M";
                    output += "-" + w_dic[w];
                }
                else if (master == "M-LC11-SS")
                {
                    output += "LC11";
                    output += h_dic2[h] + "M";
                    output += "-" + w_dic[w] + "SS";
                }
            }
            else if (tag == "top")
            {
                if (master == "LC1230")
                {
                    output += "LC1230-";
                    output += w_dic[w];
                }
                else if (master == "LC1230-SS")
                {
                    output += "LC1230-";
                    output += w_dic[w];
                    output += "SS";
                }
                else if (master == "H-LC1230")
                {
                    output += "LC1230H-";
                    output += w_dic[w];
                }
                else if (master == "NM-LC1230")
                {
                    output += "LC1230HNM-";
                    output += w_dic[w];
                }
                else if (master == "M-LC1230")
                {
                    output += "LC1230M-";
                    output += w_dic[w];
                }
                else if (master == "M-LC1230-SS")
                {
                    output += "LC1230M-";
                    output += w_dic[w];
                    output += "SS";
                }
                else if (master == "MNM-LC1230")
                {
                    output += "LC1230MNM-";
                    output += w_dic[w];
                }
                else if (master == "FH-LC1230")
                {
                    output += "LC1230FH-";
                    output += w_dic[w];
                }
            }
            else if (tag == "toe")
            {
                if (master == "LC1260")
                {
                    output += "LC1260-";
                    output += w_dic[w];
                }
                else if (master == "LC1260-SS")
                {
                    output += "LC1260-";
                    output += w_dic[w];
                    output += "SS";
                }
                else if (master == "H-LC1260")
                {
                    output += "LC1260H-";
                    output += w_dic[w];
                }
                else if (master == "NM-LC1260")
                {
                    output += "LC1260HNM-";
                    output += w_dic[w];
                }
            }
            else if (tag == "runner")
            {
                if (h == 3)
                {
                    output = "LC1326";
                }
                else if (h == 6)
                {
                    output = "LC1330";
                }
                else if (h == 9)
                {
                    output = "LC1327";
                }
                else if (h == 12)
                {
                    output = "LC1340";
                }
                else if (h == 15)
                {
                    output = "LC1350";
                }
                else if (h == 18)
                {
                    output = "LC1381";
                }
                else if (h == 21)
                {
                    output = "LC1365";
                }
                else if (h == 24)
                {
                    output = "LC1380";
                }
                else if (h == 27)
                {
                    output = "LC1375";
                }
                else if (h == 30)
                {
                    output = "LC1370";
                }
                if (master == "LC13-SS")
                {
                    output += "SS";
                }
                else if (master == "NM-LC13")
                {
                    output += "NM";
                }

            }
            else if (tag == "gusset")
            {
                output = master;
            }
            else if (tag == "shelf")
            {
                if (master == "LC1630")
                {
                    output += "LC1630-";
                    output += w_dic[w];
                }
                else if (master == "LC1630-SS")
                {
                    output += "LC1630-";
                    output += w_dic[w];
                    output += "SS";
                }
                else if (master == "NM-LC1630")
                {
                    output += "LC1630NM-";
                    output += w_dic[w];
                }
                else if (master == "NM-LC1630-SS")
                {
                    output += "LC1630NM-";
                    output += w_dic[w];
                    output += "SS";
                }
            }
            else if (tag == "removableback")
            {
                if (master == "LC17")
                {
                    output += "LC12";
                    if (h == 29)
                    {
                        output += "20";
                    }
                    else if (h == 32)
                    {
                        output += "15";
                    }
                    else if (h == 35)
                    {
                        output += "10";
                    }
                    output += "H-";
                    output += w_dic[w];
                }
            }
            else if (tag == "bottom")
            {
                if (master == "LC1250")
                {
                    output += "LC1250-";
                    output += w_dic[w];
                }
                else if (master == "LC1250-SS")
                {
                    output += "LC1250-";
                    output += w_dic[w];
                    output += "SS";
                }
                else if (master == "H-LC1250")
                {
                    output += "LC1250H-";
                    output += w_dic[w];
                    output += "FO";
                }
                else if (master == "M-LC1250")
                {
                    output += "LC1250M-";
                    output += w_dic[w];
                }
                else if (master == "M-LC1250-SS")
                {
                    output += "LC1250-";
                    output += w_dic[w];
                    output += "SS";
                }
            }
            else if (tag == "hiddenrail")
            {
                if (master == "LC1280")
                {
                    output += "LC1280-";
                    try
                    {
                        output += w_dic[w];
                    }
                    catch
                    {
                        output += w.ToString();
                    }
                }
                else if (master == "NM-LC1280")
                {
                    output += "LC1280NM-";
                    try
                    {
                        output += w_dic[w];
                    }
                    catch
                    {
                        output += w.ToString();
                    }
                }
            }
            else if (tag == "partition")
            {
                if (master == "LC1400")
                {
                    if (h == 24)
                    {
                        output += "LC1390";
                    }
                    else if (h == 30)
                    {
                        output += "LC1400";
                    }
                    else
                    {
                       
                    }
                }
                else if (master == "LC1400-SS")
                {
                    if (h == 24)
                    {
                        output += "LC1390SS";
                    }
                    else if (h == 30)
                    {
                        output += "LC1400SS";
                    }
                    else
                    {
                        output += "Do not have this information.";
                    }
                }
                else if (master == "HNM-LC1400")
                {
                    if (h == 24)
                    {
                        output += "LC1390HNM";
                    }
                    else if (h == 30)
                    {
                        output += "LC1400HNM";
                    }
                    else
                    {
                        output += "Do not have this information.";
                    }
                }
                else if (master == "H-LC1400")
                {
                    if (h == 24)
                    {
                        output += "LC1390H";
                    }
                    else if (h == 30)
                    {
                        output += "LC1400H";
                    }
                    else
                    {
                        output += "Do not have this information.";
                    }
                }
            }
            else if (tag == "centerpost")
            {
                if ((master == "LC13") || (master == "H-LC13") || (master == "LC13-SS") || (master == "NM-LC13"))
                {
                    if (h == 3)
                    {
                        output = "LC1326";
                    }
                    else if (h == 6)
                    {
                        output = "LC1330";
                    }
                    else if (h == 9)
                    {
                        output = "LC1327";
                    }
                    else if (h == 12)
                    {
                        output = "LC1340";
                    }
                    else if (h == 15)
                    {
                        output = "LC1350";
                    }
                    else if (h == 18)
                    {
                        output = "LC1381";
                    }
                    else if (h == 21)
                    {
                        output = "LC1365";
                    }
                    else if (h == 24)
                    {
                        output = "LC1380";
                    }
                    else if (h == 27)
                    {
                        output = "LC1375";
                    }
                    else if (h == 30)
                    {
                        output = "LC1370";
                    }
                    if (master == "H-LC13")
                    {
                        output += "H";
                    }
                    else if (master == "NM-LC13")
                    {
                        output += "HNM";
                    }
                    else if (master == "LC13-SS")
                    {
                        output += "SS";
                    }
                }
                else if ((master == "LC13-in") || (master == "H-LC13-in") || (master == "LC13-in-SS") || (master == "NM-LC13-in"))
                {

                }
                else if ((master == "LC13-out") || (master == "H-LC13-out") || (master == "LC13-out-SS") || (master == "NM-LC13-out"))
                {
                    output += "LC";
                    output += POSTCENTER1[h];
                    if (master == "H-LC13-out")
                    {
                        output += "H";
                    }
                    else if (master == "NM-LC13-out")
                    {
                        output += "HNM";
                    }
                    else if (master == "LC13-out-SS")
                    {
                        output += "SS";
                    }
                }
            }
            else if (tag == "securitypanel")
            {
                if (master == "LC1460")
                {
                    output += "LC1462-";
                    output += w_dic[w];
                }
            }
            if (output == "")
            {
                output = originalName + "*";
            }
            return output;
        }
        public static string getVisualDescription(string tag, string master, int w, int h, int d)
        {
            string output = "";
            Dictionary<int, string> POSTCENTER_DES = new Dictionary<int, string>() { { 3, "29/32" }, { 6, "13/16" }, { 9, "23/32" }, { 12, "5/8" }, { 15, "17/32" }, { 21, "11/32" } };

            if (tag == "sideleft")
            {
                if (master == "LC10")
                {
                    output = "SIDE LEFT 22X" + h.ToString() + " BASE CABINET";
                }
                else if (master == "H-LC10")
                {
                    output = "SIDE LEFT 22X" + h.ToString() + " B/C HYBRID FLUSH OVERLAP";
                }
                else if (master == "NM-LC10")
                {
                    output = "SIDE LEFT 22X" + h.ToString() + " B/C HYBRID";
                }
                else if (master == "LC10-SS")
                {
                    output = "SIDE LEFT 22X" + h.ToString() + " S/S BASE CABINET";
                }
                else if (master == "M-LC10")
                {
                    output = "SIDE LEFT 22X" + h.ToString() + " MOB CAB";
                }
                else if (master == "M-LC10-SS")
                {
                    output = "SIDE LEFT S/S 22X" + h.ToString() + " MOB CAB";
                }
                else if (master == "MNM-LC10")
                {
                    output = "SIDE LEFT 22X" + h.ToString() + " MOB CAB NON-METAL FRONT";
                }
            }
            else if (tag == "sideright")
            {
                if (master == "LC10")
                {
                    output = "SIDE RIGHT 22X" + h.ToString() + " BASE CABINET";
                }
                else if (master == "H-LC10")
                {
                    output = "SIDE RIGHT 22X" + h.ToString() + " B/C HYBRID FLUSH OVERLAP";
                }
                else if (master == "NM-LC10")
                {
                    output = "SIDE RIGHT 22X" + h.ToString() + " B/C HYBRID";
                }
                else if (master == "LC10-SS")
                {
                    output = "SIDE RIGHT 22X" + h.ToString() + " S/S BASE CABINET";
                }
                else if (master == "M-LC10")
                {
                    output = "SIDE RIGHT 22X" + h.ToString() + " MOB CAB";
                }
                else if (master == "M-LC10-SS")
                {
                    output = "SIDE RIGHT S/S 22X" + h.ToString() + " MOB CAB";
                }
                else if (master == "MNM-LC10")
                {
                    output = "SIDE RIGHT 22X" + h.ToString() + " MOB CAB NON-METAL FRONT";
                }
            }
            else if (tag == "back")
            {
                if (master == "LC11")
                {
                    output = "BACK FULL " + h.ToString() + "X" + w.ToString() + " BACE CABINET";
                }
                else if (master == "LC11-SS")
                {
                    output = "BACK FULL " + h.ToString() + "X" + w.ToString() + " BACE CABINET S/S";
                }
                else if (master == "HT-LC11")
                {
                    output = "TOP REAR RAIL HYBRID " + w.ToString() + "\"";
                }
                else if (master == "HB-LC11")
                {
                    output = "BOTTOM REAR RAIL HYBRID " + w.ToString() + "\"";
                }
                else if (master == "HC-LC11")
                {
                    output = "CHANNEL BACK CENTER HYBRID " + h.ToString() + "\"";
                }
                else if (master == "M-LC11")
                {
                    output = "BACK " + h.ToString() + "X" + w.ToString() + " MOB CAB";
                }
                else if (master == "M-LC11-SS")
                {
                    output = "BACK " + h.ToString() + "X" + w.ToString() + " MOB CAB S/S";
                }
            }
            else if (tag == "top")
            {
                if (master == "LC1230")
                {
                    output = "RAIL FRONT TOP BASE CAB 1X" + w.ToString();
                }
                else if (master == "LC1230-SS")
                {
                    output = "RAIL FRONT TOP BASE CAB 1X" + w.ToString() + "SS";
                }
                else if (master == "H-LC1230")
                {
                    output = "RAIL FRONT TOP HYBRID B/C 1X" + w.ToString();
                }
                else if (master == "NM-LC1230")
                {
                    output = "RAIL FRONT TOP HYBRID B/C 1X" + w.ToString();
                }
                else if (master == "M-LC1230")
                {
                    output = "RAIL/TOP COVER MOBILE B/C " + w.ToString() + "X22";
                }
                else if (master == "M-LC1230-SS")
                {
                    output = "RAIL/TOP COVER MOBILE B/C " + w.ToString() + "X22 S/S";
                }
                else if (master == "MNM-LC1230")
                {
                    output = "RAIL/TOP COVER MOBILE B/C " + w.ToString() + "X22 S/S";
                }
                else if (master == "FH-LC1230")
                {
                    output = "RAIL COUNTER SUPPORT 2X" + w.ToString();
                }
            }
            else if (tag == "toe")
            {
                if (master == "LC1260")
                {
                    output = "TOE SPACE HINGED " + w.ToString() + "\" B/C&S/C";
                }
                else if (master == "LC1260-SS")
                {
                    output = "TOE SPACE HINGED " + w.ToString() + "\" B/C&S/C S/S";
                }
                else if (master == "H-LC1260")
                {
                    output = "TOE SPACE HINGED HYB " + w.ToString() + "\" B/C";
                }
                else if (master == "NM-LC1260")
                {
                    output = "TOE SPACE HINGED HYBRID " + w.ToString() + "\" B/C F/O NON-METAL FRONT";
                }
            }
            else if (tag == "runner")
            {
                if (master == "LC13")
                {
                    output = "RUNNER SUPPORT " + h.ToString() + "\"";
                }
                else if (master == "LC13-SS")
                {
                    output = "RUNNER SUPPORT " + h.ToString() + "\" S/S";
                }
                else if (master == "NM-LC13")
                {
                    output = "RUNNER SUPPORT " + h.ToString() + "\" WOOD CABINET";
                }
            }
            else if (tag == "gusset")
            {
                if (master == "LC1270")
                {
                    output = "GUSSET TO ALL CAB LINES";
                }
                else if (master == "LC1270G")
                {
                    output = "GUSSET TO GLIDE CABINET";
                }
            }
            else if (tag == "shelf")
            {
                if (master == "LC1630")
                {
                    output = "SHELF BASE CABINET 19-3/8X" + (w - 1).ToString() + "-5/8";
                }
                else if (master == "LC1630-SS")
                {
                    output = "SHELF BASE CABINET 19-3/8X" + (w - 1).ToString() + "-5/8 S/S";
                }
                else if (master == "NM-LC1630")
                {
                    output = "SHELF BASE CABINET 17-9/16X" + (w - 1).ToString() + "-5/8";
                }
                else if (master == "NM-LC1630-SS")
                {
                    output = "SHELF BASE CABINET 17-9/16X" + (w - 1).ToString() + "-5/8 S/S";
                }
            }
            else if (tag == "removableback")
            {
                if (master == "LC17")
                {
                    output = "REMOVABLE BACK HYBRID 18\" " + h.ToString() + " HT";
                }
            }
            else if (tag == "bottom")
            {
                if (master == "LC1250")
                {
                    output = "RAIL-BOTTOM HINGED 22X" + w.ToString() + " W/TOP RAIL";
                }
                else if (master == "LC1250-SS")
                {
                    output = "BOTTOM HINGED 22X" + w.ToString() + " B/C&S/C S/S";
                }
                else if (master == "H-LC1250")
                {
                    output = "BOTTOM TRAY HINGED 22X" + w.ToString() + " B/C";
                }
                else if (master == "M-LC1250")
                {
                    output = "BOTTOM W/O HOLES 22X" + w.ToString() + " MOBILE";
                }
                else if (master == "M-LC1250-SS")
                {
                    output = "BOTTOM W/O HOLES 22X" + w.ToString() + " MOBILE S/S";
                }
            }
            else if (tag == "hiddenrail")
            {
                if (master == "LC1280")
                {
                    output = "RAIL HIDDEN " + w.ToString() + "\"";
                }
                else if (master == "NM-LC1280")
                {
                    output = "RAIL HIDDEN " + w.ToString() + "\" NON-METAL FRONT";
                }
            }
            else if (tag == "partition")
            {
                if (master == "LC1400")
                {
                    output = "CENTER PARTITION " + h.ToString() + "\"";
                }
                else if (master == "LC1400-SS")
                {
                    output = "CENTER PARTITION " + h.ToString() + "\" S/S";
                }
                else if (master == "HNM-LC1400")
                {
                    output = "CENTER PARTITION HYBRID WOOD " + h.ToString() + "\"";
                }
                else if (master == "H-LC1400")
                {
                    output = "CENTER PARTITION HYBRID " + h.ToString() + "\"";
                }

            }
            else if (tag == "centerpost")
            {
                if ((master == "LC13") || (master == "H-LC13") || (master == "LC13-SS") || (master == "NM-LC13"))
                {
                    if (master == "LC13")
                    {
                        output = "RUNNER SUPPORT " + h.ToString() + "\"";
                    }
                    else if (master == "LC13-SS")
                    {
                        output = "RUNNER SUPPORT " + h.ToString() + "\" S/S";
                    }
                    else if (master == "NM-LC13")
                    {
                        output = "RUNNER SUPPORT " + h.ToString() + "\" WOOD CABINET";
                    }
                }
                else if ((master == "LC13-in") || (master == "H-LC13-in") || (master == "LC13-in-SS") || (master == "NM-LC13-in"))
                {
                    if (master == "LC13-in")
                    {
                        output = "POST CENTER INNER " + (h - 1).ToString() + "\"";
                    }
                    else if (master == "LC13-in-SS")
                    {
                        output = "POST CENTER INNER " + (h - 1).ToString() + "\" S/S";
                    }
                    else if (master == "NM-LC13-in")
                    {
                        output = "POST CENTER INNER " + (h - 1).ToString() + "\" WOOD CABINET";
                    }
                }
                else if ((master == "LC13-out") || (master == "H-LC13-out") || (master == "LC13-out-SS") || (master == "NM-LC13-out"))
                {
                    if ((master == "LC13-out"))
                    {
                        try
                        {
                            output = "POST CENTER " + (h - 1).ToString() + " " + POSTCENTER_DES[h] + " X 1 1/2";
                        }
                        catch
                        {
                            output = "POST CENTER " + (h).ToString() + " X 1 1/2";
                        }
                    }
                    else if ((master == "LC13-out-SS"))
                    {
                        output = "POST CENTER " + (h - 1).ToString() + " " + POSTCENTER_DES[h] + " X 1 1/2 S/S";
                    }
                    else if ((master == "H-LC13-out"))
                    {
                        output = "POST CENTER HYBRID " + (h).ToString() + "\"";
                    }
                    else if ((master == "NM-LC13-out"))
                    {
                        output = "POST CENTER HYBRID " + (h).ToString() + "\"X1 1/2";
                    }
                }
            }
            else if (tag == "securitypanel")
            {
                if (master == "LC1460")
                {
                    output = "SECURITY PANEL " + w.ToString() + "\"";
                }
            }
            return output;
        }
        public static bool getRemovability(string tag, string master, int w, int h, int d)
        {
            bool output = false;
            if (tag == "sideleft")
            {
                output = false;
            }
            else if (tag == "sideright")
            {
                output = false;
            }
            else if (tag == "back")
            {
                output = false;
            }
            else if (tag == "top")
            {
                output = false;
            }
            else if (tag == "toe")
            {
                output = false;
            }
            else if (tag == "runner")
            {
                output = false;
            }
            else if (tag == "gusset")
            {
                output = false;
            }
            else if (tag == "shelf")
            {
                output = true;
            }
            else if (tag == "removableback")
            {
                output = true;
            }
            else if (tag == "bottom")
            {
                if (master == "LC1250")
                {
                    output = false;
                }
                else if (master == "LC1250-SS")
                {
                    output = false;
                }
                else if (master == "H-LC1250")
                {
                    output = true;
                }
                else if (master == "M-LC1250")
                {
                    output = true;
                }
                else if (master == "M-LC1250-SS")
                {
                    output = true;
                }
            }
            else if (tag == "hiddenrail")
            {
                output = false;
            }
            else if (tag == "partition")
            {
                output = true;
            }
            else if (tag == "centerpost")
            {
                output = false;
            }
            else if (tag == "securitypanel")
            {
                output = true;
            }
            return output;
        }
        public void exportDrawing(string model, string cstyle, int w, int h, int d, string metalTop, string selfClose, string lockPro, double cw,
            List<int> door_h, List<int> door_w, List<int> door_confi, List<int> door_x, List<int> door_y, List<int> door_lock,
            List<int> drawer_w, List<int> drawer_h, List<int> drawer_x, List<int> drawer_y, List<int> drawer_lock, List<int> drawer_load, string cabName, string specialDesign)
        {
            Dictionary<int, string> dicMat = new Dictionary<int, string>() { { 1, "CRS" }, { 2, "SS" }, { 3, "GUSSET" } };
            Dictionary<double, string> dicThickness = new Dictionary<double, string>() { { 0.036, "20" }, { 0.048, "18" }, { 0.06, "16" }, { 0.074, "14" }, { 0.104, "12" } };
            int doorh = 0;
            int door_conf1 = 0;
            int door_conf2 = 0;
            if (door_h.Count > 0)
            {
                doorh = door_h[0];
                if (door_h.Count == 1)
                {
                    door_conf1 = door_confi[0];
                }
                else if (door_h.Count == 2)
                {
                    if (door_y[0] == door_y[1])
                    {
                        if (door_confi[0] > 9)
                        {
                            door_conf1 = 4;
                        }
                        else
                        {
                            door_conf1 = 3;
                        }
                    }
                    else
                    {
                        door_conf1 = door_confi[0];
                        door_conf2 = door_confi[1];
                    }
                }
                else
                {
                    if (door_confi[0] > 9)
                    {
                        door_conf1 = 4;
                    }
                    else
                    {
                        door_conf1 = 3;
                    }
                    if (door_confi[2] > 9)
                    {
                        door_conf2 = 4;
                    }
                    else
                    {
                        door_conf2 = 3;
                    }
                }
            }

            string desshell = "BASE CABINET SEHLL " + w.ToString() + "\"";
            if (cstyle == "PH")
            {
                desshell = "CABINET NEW FLUSH OVERLAY WIDE " + w.ToString() + "\"";
            }

            List<string> partname1 = new List<string>() { "Part ID", w.ToString() + "X" + h.ToString() + " BASE SHELL" };
            List<string> des1 = new List<string>() { "Description", desshell };
            List<string> mat1 = new List<string>() { "Material", "" };
            List<string> num1 = new List<string>() { "QTY.", "1" };

            List<string> partname2 = new List<string>() { "Part ID" };
            List<string> des2 = new List<string>() { "Description" };
            List<string> mat2 = new List<string>() { "Material" };
            List<string> num2 = new List<string>() { "QTY." };

            List<List<string>> shellpart = ShellParts.shellParts(model, cstyle, specialDesign, d, h, w, doorh, door_conf1, door_conf2, metalTop, door_h, drawer_w, drawer_y, door_x, door_w, drawer_lock, drawer_x, drawer_h, "NSC", "");
            List<string> shellPartTag = new List<string>() { "sideleft","back", "top", "toe", "runner", "toespacebox", "gusset", "shelf", "removableback", "bottom", "stileside", "stiletop", "stilebottom",
            "front", "hiddenrail", "partition", "centerpost","securitypanel","selfclosepackage"};
            for (int i = 0; i < shellpart.Count - 1; i++)
            {
                string tag = shellPartTag[i];
                for (int j = 0; j < shellpart[i].Count; j++)
                {
                    //if ((tag != "centerpost") || (j == 0))
                    //{

                        if (tag == "sideleft")
                        {
                            if (j != 0)
                            {
                                tag = "sideright";
                            }
                        }
                        List<string> cat = Price.nametocathwd(shellpart[i][j]);
                        if (shellpart[i][j].Contains("SS"))
                        {
                            cat[0] += "-SS";
                        }
                        string partname = getVisualName(tag, cat[0], Convert.ToInt16(cat[1]), Convert.ToInt16(cat[2]), Convert.ToInt16(cat[3]), shellpart[i][j]);
                        int k = 0;
                        for (k = 0; k < partname1.Count; k++)
                        {
                            if (partname == partname1[k])
                            {
                                break;
                            }
                        }
                        if (k < partname1.Count)
                        {
                            num1[k] = (Convert.ToInt16(num1[k]) + 1).ToString();
                        }
                        else
                        {
                            for (k = 0; k < partname2.Count; k++)
                            {
                                if (partname == partname2[k])
                                {
                                    break;
                                }
                            }
                            if (k < partname2.Count)
                            {
                                num2[k] = (Convert.ToInt16(num2[k]) + 1).ToString();
                            }
                            else
                            {
                                string des = getVisualDescription(tag, cat[0], Convert.ToInt16(cat[1]), Convert.ToInt16(cat[2]), Convert.ToInt16(cat[3]));
                                string mat = dicMat[Price.matindexCSG(shellpart[i][j])] + dicThickness[Price.matgauge(cat[0])];
                                string qt = "1";
                                bool remov = getRemovability(tag, cat[0], Convert.ToInt16(cat[1]), Convert.ToInt16(cat[2]), Convert.ToInt16(cat[3]));
                                if (remov == true)
                                {
                                    partname1.Add(partname);
                                    des1.Add(des);
                                    mat1.Add(mat);
                                    num1.Add(qt);
                                }
                                else
                                {
                                    partname2.Add(partname);
                                    des2.Add(des);
                                    mat2.Add(mat);
                                    num2.Add(qt);
                                }
                            }
                        }
                    //    if ((tag == "centerpost"))
                    //    {
                    //        j = shellpart[i].Count;
                    //    }
                    //}
                }
            }

            partname1.Add("CR#8X.50PT"); des1.Add("#8X1/2\"PHILIPS PAN TECH STZ"); num1.Add("2"); mat1.Add("");
            for (int i = 0; i < door_x.Count; i++)
            {
                string doorname = DoorParts.fromoutdoortoformaldoorname(DoorParts.parts_door(w, model, door_h[i], cstyle, door_w[i], door_confi[i], "P4", door_lock[i], 0, "", 1, false)[1]);
                int o = 0;
                for (o = 0; o < partname1.Count; o++)
                {
                    if (partname1[o] == doorname)
                    { break; }
                }
                if (o == partname1.Count)
                {
                    partname1.Add(doorname);
                    des1.Add("DOOR ASSEMBLY PAINTED " + door_h[i].ToString() + "X" + door_w[i].ToString());
                    mat1.Add("");
                    num1.Add("1");
                }
                else
                {
                    num1[o] = (Convert.ToInt16(num1[o]) + 1).ToString();
                }
            }
            partname1.Add("K428"); des1.Add("LEVEL BOLTS 1/2-13 X 2-1/2 HEX (AL LIEB)"); num1.Add((4).ToString()); mat1.Add("");
            if (door_h.Count > 0)
            {
                
                partname1.Add("SJ-5012"); des1.Add("BUMPER, DOOR"); num1.Add((door_h.Count).ToString()); mat1.Add("Polyurethene".ToUpper());
            }
            partname1.Add("CR#14X.50PS"); des1.Add("#14X1/2 PHIL. PAN S/M/S"); num1.Add(("2").ToString()); mat1.Add("steel".ToUpper());


            //List<string> drawernames = new List<string>();
            for (int i = 0; i < drawer_x.Count; i++)
            {
                string drawername = DrawerParts.drawername(cstyle, drawer_h[i], drawer_w[i], drawer_load[i].ToString(), "0", drawer_lock[i].ToString(), "P4", "0", "0", "1", "-");
                //string drawerheadname = DrawerParts.drawerheadname(cstyle, drawer_h[i], drawer_w[i], "P4", drawer_lock[i], 0, "-", drawer_load[i].ToString());
                //string drawerbodyname = DrawerParts.drawerbodyname(drawer_h[i], drawer_w[i], cstyle, drawer_load[i].ToString(), "T1", 0, "Base", "-");
                //string trackname = DrawerParts.trackname("Base", "T1", drawer_load[i], 0, cstyle, "-");
                //List<string> trackclip = DrawerParts.fromtracktoclip(trackname);
                int o = 0;
                for (o = 0; o < partname1.Count; o++)
                {
                    if (partname1[o] == drawername)
                    {
                        break;
                    }
                }
                if (o == partname1.Count)
                {
                    partname1.Add(drawername);
                    des1.Add("DRAWER ASSEMBLY PAINTED STEEL " + drawer_h[i].ToString() + "X" + drawer_w[i].ToString());
                    mat1.Add("");
                    num1.Add("1");
                }
                else
                {
                    num1[o] = (Convert.ToInt16(num1[o]) + 1).ToString();
                }

                //BomForTable.Add(drawerheadname); desForTable.Add("Drawer Head");
                //BomForTable.Add(drawerbodyname); desForTable.Add("Drawer Body");
                //BomForTable.Add(trackname); desForTable.Add("Track"); BomForTable.Add(trackname); desForTable.Add("Track");
                //BomForTable.Add(trackclip[0]); desForTable.Add("Track Clip"); BomForTable.Add(trackclip[0]); desForTable.Add("Track Clip");
                //BomForTable.Add(trackclip[1]); desForTable.Add("Track Clip"); BomForTable.Add(trackclip[1]); desForTable.Add("Track Clip");
                //drawernames.Add(drawername);
            }


            partname2.Add("SBS43"); des2.Add("1/8 DIA. X 5/16 STEEL POP RIVET"); num2.Add("34"); mat2.Add("STEEL");
            partname2.Add("SBS64"); des2.Add("3/16 DIA.X7/16 STEEL POP RIVET"); num2.Add("2"); mat2.Add("STEEL");


            
            if ((model == "Mobile")||(model == "M"))
            {
                //string myname = CabinetOverview.myname(model, "-", cstyle, metalTop, w, h, d, "0", 0, drawer_x, drawer_y, drawer_w, drawer_h,
                //    drawer_lock, new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, drawer_load, door_x, door_y, door_w, door_h, door_confi,
                //    new List<int>() { 1, 1, 1, 1 }, door_lock, new List<int>() { 0, 0, 0, 0 }, pull, hinge, tracktype, lockPro, "", "", "NSC");
                //MySqlDataReader rd = 
                if (cw > 0)
                {
                    partname1.Add("WTBXM"); num1.Add("1"); des1.Add("CounterWeight Box"); mat1.Add("");
                    cw -= 4.24;
                    int tempnum = Convert.ToInt32(Math.Ceiling(cw / 5));
                    if (tempnum > 0)
                    { partname1.Add("WTBXM-B"); num1.Add(tempnum.ToString()); des1.Add("Brick"); mat1.Add(""); }
                    
                }
            }
            List<string> itemno1 = new List<string>() { "ITEM NO." };
            for (int i = 1; i < partname1.Count; i++)
            {
                itemno1.Add(i.ToString());
            }
            List<string> itemno2 = new List<string>() { "ITEM NO." };
            for (int i = 1; i < partname2.Count; i++)
            {
                itemno2.Add(i.ToString());
            }

            swfunctions.activeApp();
            swfunctions.openDrawingFile(masterPath + "drawing\\1.slddrw");
            swfunctions.activeCurrent("1.slddrw");
            swfunctions.packandgo1(libraryPath + "Assembly\\cabinet\\" + cabName + "\\", cabName, "1.SLDDRW");
            swfunctions.openDrawingFile(libraryPath + "assembly\\cabinet\\" + cabName + "\\" + cabName + "_1.SLDDRW");
            swfunctions.insertGeneralTabel("Sheet1", new List<List<string>>() { itemno1, partname1, des1, mat1, num1 }, 0.0068, 0.2735);
            swfunctions.insertGeneralTabel("Sheet2", new List<List<string>>() { itemno2, partname2, des2, mat2, num2 }, 0.0068, 0.2735);
            //swfunctions.saveDrawingAs(libraryPath + "Assembly\\cabinet\\" + cabName + "\\", cabName + "Drawing");
            
            swfunctions.exportDrawingToPDF(pdfPath, cabName);


            swfunctions.killSW();

            bool STATUS = true;
            while(STATUS == true)
            {
                try
                {
                    File.Delete(masterPath + "drawing\\1.sldasm");
                    STATUS = false;
                }
                catch { swfunctions.killSW(); }
            }
            STATUS = true;
            while (STATUS == true)
            {
                try
                {
                    File.Delete(masterPath + "drawing\\2.sldasm");
                    STATUS = false;
                }
                catch { swfunctions.killSW(); }
            }
            
        }
    }
}
