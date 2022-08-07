using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class CabinetOverview
    {
        public static List<double> getPartLocation(double partX, double partY, double partZ, double cabW, double cabH, double cabD, 
            string indexstr, string transStr, List<bool> toespacebool, int i, List<bool> uppontoespacebool)
        {
            double mToInch = 39.3700787;
            List<double> output = new List<double>();
            if (indexstr == "Customize")
            {
                output = new List<double>() { 0, 0, 0 };
            }
            else
            {
                if (transStr == "")
                {
                    transStr = "0/0/0";
                }
                List<string> indexes = indexstr.Split('/').ToList();
                List<string> transEs = transStr.Split('/').ToList();

                double x = 0;
                if (transEs[0] != "")
                {
                   x = Convert.ToDouble(transEs[0]);
                }
                double y = 0;
                if (transEs[1] != "")
                {
                    y = Convert.ToDouble(transEs[1]);
                }
                double z = 0;
                if (transEs[2] != "")
                {
                    z = Convert.ToDouble(transEs[2]);
                }
                
                if ((indexes[0] == "Left")|| (indexes[0] == "L"))
                {
                    x += -cabW / 2 + partX / 2;
                }
                else if ((indexes[0] == "Middle")|| (indexes[0] == "M"))
                {
                    x += 0;
                }
                else
                {
                    x += cabW / 2 - partX / 2;
                }
                if ((indexes[1] == "Left")||(indexes[1] == "B"))
                {
                    y += -cabH / 2 + partY / 2;
                }
                else if ((indexes[1] == "Middle") || (indexes[1] == "M"))
                {
                    y += 0;
                }
                else
                {
                    y += cabH / 2 - partY / 2;
                }
                if ((indexes[2] == "Left")|| (indexes[2] == "B"))
                {
                    z += -cabD / 2 + partZ / 2;
                }
                else if ((indexes[2] == "Middle")|| (indexes[2] == "M"))
                {
                    z += 0;
                }
                else
                {
                    z += cabD / 2 - partZ / 2;
                }
                if ((uppontoespacebool[i] == true) && (toespacebool.Contains(true)) && (indexes[1] == "Left"))
                {
                    y += 3.71;
                    //z += 0.048;
                }
                output = new List<double>() { x, y, z };
            }
            return output;
        }
        public static List<double> getHidLocation(List<int> index, double partX, double partY, double partZ, double cabW, double framH, double cabD, string cstyle, double realCabHeight)
        {
            double mToInch = 39.3700787;
            List<double> output = new List<double>();
            double x = 0;
            double y = 0;
            double z = 0;
            if (index[1] == 0)
            {
                if (index[2] == cabW)
                {
                    x = 0;
                }
                else
                {
                    x = -cabW / 2 + partX / 2;
                }
            }
            else
            {
                x = partX / 2;
            }
            x /= mToInch;

            y = framH / 2 - Convert.ToDouble(index[0]) * 31 / 32;
            //y = (cabH / 2 - Convert.ToDouble(index[0]));
            //y = y * 31 / 32;

            //y -= partY / 2;
            //y -= 0.23232990 / 2;
            //y -= 0.36573911;
            y /= mToInch;

            //double ind = (realCabHeight / 2 - index[0])*31/32;
            //y = ind ;
            //y /= mToInch;
            if ((cstyle == "PN")||(cstyle == "SN"))
            {
                z = (0.56 - 2.0262 / 2) / mToInch;
            }
            else
            {
                z = (0.6 - 1.16800 / 2) / mToInch;
            }
            //z = -0.962533 / mToInch - 0.41936357 / mToInch/*cabD / 2 - partZ / 2*/;

            output = new List<double>() { x, y, z };
            return output;
        }
        public static List<double> getcenterpostLocation(List<int> index, double partX, double partY, double partZ, double cabW, double framH, double cabD, string cstyle, double realCabHeight)
        {
            double mToInch = 39.3700787;
            List<double> output = new List<double>();
            double x = 0;
            double y = 0;
            double z = 0;
            x = 0;
            //y = cabH / 2 - Convert.ToDouble(index[1]);
            //y = y * 31 / 32;
            //y -= partY / 2;
            //y -= 0.23232990 / 2;
            //y /= mToInch;

            //double ind = (realCabHeight / 2 - index[1] - index[0]) * 31 / 32;
            //y = ind + partY / 2;
            //y -= 0.23232990 / 2;
            //y -= 0.37500000;

            //y -= 0.36573911;
            //y /= mToInch;
            y = framH / 2 - Convert.ToDouble(index[1]) * 31 / 32 - partY /2;
            y /= mToInch;

            //z = 0/*cabD / 2 - partZ / 2*/;
            if ((cstyle == "PN") || (cstyle == "SN"))
            {
                z = (2.0262 / 2-1.53133/2) / mToInch;
            }
            else
            {
                z = (1.16800 / 2-0.67718/2) / mToInch;
            }

            output = new List<double>() { x, y, z };
            return output;
        }
        public static int fromVarToPartClassNameRegulation(string variables)
        {
            int output = 0;
            if (variables == "")
            {

            }
            else if (variables == "w")
            {
                output = 1;
            }
            else if (variables == "h")
            {
                output = 2;
            }
            else if (variables == "d")
            {
                output = 3;
            }
            else if (variables == "wh")
            {
                output = 4;
            }
            else if (variables == "wd")
            {
                output = 5;
            }
            else if (variables == "hd")
            {
                output = 6;
            }
            else if (variables == "whd")
            {
                output = 7;
            }
            return output;
        }
        public static string showPartClassFormat(int index, string partClassName)
        {
            string output = "";

            string kong = " ___ ";
            string dash = " - ";

            if (index == 0)
            {
                output = kong + dash + kong + "C" + partClassName + dash + kong;
            }
            else if (index == 1)
            {
                output = kong + dash + kong + "C" + partClassName + dash + "W" + dash + kong;
            }
            else if (index == 2)
            {
                output = kong + dash + kong + "C" + partClassName + dash + "H" + dash + kong;
            }
            else if (index == 3)
            {
                output = kong + dash + kong + "C" + partClassName + dash + "D" + dash + kong;
            }
            else if (index == 4)
            {
                output = kong + dash + kong + "C" + partClassName + "W" + dash + "H" + dash + kong;
            }
            else if (index == 5)
            {
                output = kong + dash + kong + "C" + partClassName + "W" + dash + "D" + dash + kong;
            }
            else if (index == 6)
            {
                output = kong + dash + kong + "C" + partClassName + "H" + dash + "D" + dash + kong;
            }
            else if (index == 7)
            {
                output = kong + dash + kong + "C" + partClassName + "W" + dash + "H" + dash + "D" + dash + kong;
            }
            return output;
        }
        public static List<int> whd(string partClass, string partMaster, string partName, string variables)
        {
            Database database = new Database();
            List<int> output = new List<int>();
            int index = fromVarToPartClassNameRegulation(variables);
            string[] partMasterArray = partMaster.Split('-');
            string[] partNameArray = partName.Split('-');
            int w = 0; int h = 0; int d = 0;
            if (index == 0)
            {

            }
            else if (index == 1)
            {
                w = Convert.ToInt16(partNameArray[partNameArray.Count() - 1]);
            }
            else if (index == 2)
            {
                h = Convert.ToInt16(partNameArray[partNameArray.Count() - 1]);
            }
            else if (index == 3)
            {
                d = Convert.ToInt16(partNameArray[partNameArray.Count() - 1]);
            }
            else if (index == 4)
            {
                string[] partNameC = partName.Split('C');
                string[] partNameD = partNameC[1].Split('-');
                w = Convert.ToInt16(partNameD[0].Substring(partClass.Count(), partNameD[0].Count() - partClass.Count()));
                h = Convert.ToInt16(partNameD[1]);
            }
            else if (index == 5)
            {
                string[] partNameC = partName.Split('C');
                string[] partNameD = partNameC[1].Split('-');
                w = Convert.ToInt16(partNameD[0].Substring(partClass.Count(), partNameD[0].Count() - partClass.Count()));
                d = Convert.ToInt16(partNameD[1]);
            }
            else if (index == 6)
            {
                string[] partNameC = partName.Split('C');
                string[] partNameD = partNameC[1].Split('-');
                h = Convert.ToInt16(partNameD[0].Substring(partClass.Count(), partNameD[0].Count() - partClass.Count()));
                d = Convert.ToInt16(partNameD[1]);
            }
            else if (index == 7)
            {
                string[] partNameC = partName.Split('C');
                string[] partNameD = partNameC[1].Split('-');
                w = Convert.ToInt16(partNameD[0].Substring(partClass.Count(), partNameD[0].Count() - partClass.Count()));
                h = Convert.ToInt16(partNameD[1]);
                d = Convert.ToInt16(partNameD[2]);
            }
            output = new List<int>() { w, h, d };
            return output;
        }

        public static List<string> getPartTipsByPartname(string partname)
        {
            Database database = new Database();

            List<string> output = new List<string>();
            string tipString = "";
            string sql = "select * from masterpart_shell where parthead = '" + partname + "'";
            MySqlDataReader rd = database.getMySqlReader_rd1(sql);
            if (rd.Read())
            {
                try
                {
                    tipString = rd.GetString("tips");
                }
                catch { }
            }
            database.CloseConnection_rd1();
            if (tipString != "")
            {
                string[] tipArray = tipString.Split('|');
                for (int i = 0; i < tipArray.Count(); i++)
                {
                    output.Add(tipArray[i]);
                }
            }
            return output;
        }
        public static string getPartVarString(string partname, string partClass)
        {
            Database database = new Database();
            string output = "";
            List<string> tipList = getPartTipsByPartname(partname);
            if ((tipList.Count > 0) && (tipList[0] != ""))
            {
                output = tipList[0];
            }
            if (output == "")
            {
                MySqlDataReader rd = database.getMySqlReader_rd1("Select * from partclass where name = '" + partClass + "'");
                if (rd.Read())
                {
                    output = rd.GetString("variables");
                }
                database.CloseConnection_rd1();
            }
            return output;
        }
        public static List<string> getPartLocString(string partname, string partClass)
        {
            Database database = new Database();
            List<string> output = new List<string>();
            List<string> tipList = getPartTipsByPartname(partname);
            string locStr = "";
            string transtr = "";
            if ((tipList.Count > 1) && (tipList[1] != ""))
            {
                string[] temp = tipList[1].Split('/');
                for (int i = 0; i < 3; i++)
                {
                    if (temp[i]!="")
                    {
                        while ((temp[i].Substring(0,1)!="+")&& (temp[i].Substring(0, 1) != "-"))
                        {
                            locStr += temp[i].Substring(0, 1);
                            temp[i] = temp[i].Substring(1, temp[i].Count() - 1);
                        }
                        if (temp[i]!="")
                        {
                            transtr += temp[i];
                        }
                    }
                    if (i!=2)
                    {
                        locStr += "/";
                        transtr += "/";
                    }
                }
            }
            if ((locStr == "//")||(locStr == ""))
            {
                MySqlDataReader rd = database.getMySqlReader_rd1("Select * from partclass where name = '" + partClass + "'");
                if (rd.Read())
                {
                    locStr = rd.GetString("locations");
                }
                database.CloseConnection_rd1();
            }
            return new List<string>() { locStr, transtr };
        }
        public static List<List<string>> getPartInsString(string partname)
        {
            Database database = new Database();
            List<List<string>> output = new List<List<string>>();
            List<string> tipList = getPartTipsByPartname(partname);
            if ((tipList.Count > 2) && (tipList[2] != ""))
            {
                tipList.Remove(tipList[0]);
                tipList.Remove(tipList[0]);
                for (int i = 0; i < tipList.Count; i++)
                {
                    if (tipList[i] != "")
                    {
                        string ifString = "";
                        string action = "";
                        string variable = "";
                        string symb = "";
                        string value = "";

                        string currentString = tipList[i].ToLower();
                        ifString = currentString.Split('(')[0];
                        currentString = currentString.Split('(')[1].Split(')')[0];
                        action = currentString.Split(';')[0];
                        currentString = currentString.Split(';')[1];
                        string[] currentArray = currentString.Split('&');
                        for (int j = 0; j < currentArray.Count(); j++)
                        {
                            if (currentArray[j] != "")
                            {
                                variable += currentArray[j].Substring(0, 1);
                                if (currentArray[j].Contains("<"))
                                {
                                    if (currentArray[j].Contains("="))
                                    {
                                        symb += "<=";
                                    }
                                    else
                                    {
                                        symb += "<";
                                    }
                                }
                                else if (currentArray[j].Contains(">"))
                                {
                                    if (currentArray[j].Contains("="))
                                    {
                                        symb += ">=";
                                    }
                                    else
                                    {
                                        symb += ">";
                                    }
                                }
                                else
                                {
                                    symb += "==";
                                }
                                string tempValue = "";
                                for (int k = currentArray[j].Count() - 1; k >= 0; k--)
                                {
                                    try
                                    {
                                        int temp = Convert.ToInt16(currentArray[j].Substring(k, 1));
                                        tempValue = temp + tempValue;
                                    }
                                    catch
                                    {
                                        break;
                                    }
                                }
                                value += tempValue;
                                if ((j != currentArray.Count() - 1) && (currentArray[j + 1] != ""))
                                {
                                    variable += "-";
                                    symb += "-";
                                    value += "-";
                                }
                            }
                        }
                        output.Add(new List<string>() { ifString, action, variable, symb, value });
                    }
                }
            }
            return output;
        }
        public static List<bool> tipConmmendBool(string partMaster, int w, int h, int d)
        {
            //                            Default
            //             true             false
            //0 suppress  (hide)           (show)

            List<bool> output = new List<bool>();
            for (int i = 0; i < 1; i++)
            {
                output.Add(false);
            }

            List<List<string>> insListString = getPartInsString(partMaster);
            for (int i = 0; i < insListString.Count; i++)
            {
                if (insListString[i][1] == "suppress")
                {
                    string[] variables = insListString[i][2].Split('-');
                    string[] symbs = insListString[i][3].Split('-');
                    string[] values = insListString[i][4].Split('-');
                    bool flag = true;
                    for (int j = 0; j < variables.Count(); j++)
                    {
                        if (variables[j] != "")
                        {
                            if (variables[j] == "w")
                            {
                                if (symbs[j] == ">")
                                {
                                    if (w > Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == "<")
                                {
                                    if (w < Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == "==")
                                {
                                    if (w == Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == ">=")
                                {
                                    if (w >= Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == "<=")
                                {
                                    if (w <= Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                            }
                            else if (variables[j] == "h")
                            {
                                if (symbs[j] == ">")
                                {
                                    if (h > Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == "<")
                                {
                                    if (h < Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == "==")
                                {
                                    if (h == Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == ">=")
                                {
                                    if (h >= Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == "<=")
                                {
                                    if (h <= Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                            }
                            else if (variables[j] == "d")
                            {
                                if (symbs[j] == ">")
                                {
                                    if (d > Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == "<")
                                {
                                    if (d < Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == "==")
                                {
                                    if (d == Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == ">=")
                                {
                                    if (d >= Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                                else if (symbs[j] == "<=")
                                {
                                    if (d <= Convert.ToInt16(values[j]))
                                    {

                                    }
                                    else
                                    {
                                        flag = false;
                                    }
                                }
                            }
                        }
                    }
                    if (flag == true)
                    {
                        output[0] = true;
                    }
                }
            }


            return output;
        }
        public static string genPartNameByMaster(int _w, int _h, int _d, string masterPart, int regIndex)
        {
            string output = "";
            string w = "";
            string h = "";
            string d = "";
            if (_w < 10)
            {
                w = "0" + _w.ToString();
            }
            else
            {
                w = _w.ToString();
            }
            if (_h < 10)
            {
                h = "0" + _h.ToString();
            }
            else
            {
                h = _h.ToString();
            }
            if (_d < 10)
            {
                d = "0" + _d.ToString();
            }
            else
            {
                d = _d.ToString();
            }
            List<string> masterPartList = masterPart.Split('-').ToList();
            int i = 0;
            bool cAppe = false;
            for (i = 0; i < masterPartList.Count; i++)
            {
                if (cAppe == false)
                {
                    output += masterPartList[i] + "-";
                    if ((masterPartList[i].Contains("c")) || (masterPartList[i].Contains("C")))
                    {
                        cAppe = true;
                    }
                }
                else
                {
                    break;
                }
            }
            output = output.Substring(0, output.Count() - 1);
            Dictionary<int, string> dic = new Dictionary<int, string>()
            { {0,""}, {1, "-" + w + "" }, {2, "-" + h  + ""},{3, "-" + d + "" }, {4, w + "-" + h+"" },
             {5, w + "-" + d+"" },  {6, h + "-" + d+"" }, {7, w + "-" + h + "-" + d + "" } };
            output += dic[regIndex];
            for (int j = i; j < masterPartList.Count; j++)
            {
                output += "-" + masterPartList[j];
            }

            return output;
        }
        public static int adjustHforFont(string model, int h)
        {
            if ((model == "Base") || (model == "Special Base") || (model == "B") || (model == "SB"))
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
            return h;
        }
        public static string myname(string model, string specialdesign, string cstyle, string metaltop, int w, int h, int d, string pulloutshelf, int secpanel,
          List<int> drawerx, List<int> drawery, List<int> drawerw, List<int> drawerh, List<int> drawerl, List<int> drawerch, List<int> drawerload,
          List<int> doorx, List<int> doory, List<int> doorw, List<int> doorh, List<int> doorcon, List<int> doormat, List<int> doorl, List<int> doorch,
          string pullstyle, string hingestyle, string tracktype, string lockprofile, string cardprofile, string lockgroupby, string selfcloseforsb, List<int> heihei)
        {
            Dictionary<string, string> dic_mod = new Dictionary<string, string>()
            { {"Base", "B"}, {"Glide", "G"}, {"Suspended", "C"}, {"Storage", "S"}, {"Mobile", "M"}, {"Wall", "W"} , {"Special Base", "SB" } };

            Dictionary<string, string> dic_spec = new Dictionary<string, string>()
            { {"Open Face", "OF"}, {"Sink", "S"}, {" ", " "}, {"Corner Cabinet", "C"}, {"Fume Hood General Storage", "H"}, {"Flammable Liquid Storage", "F" },
            { "Acid/ Base Corrosive Storage","A" }, {"Vacuum Pump Storage", "VP" }, {"Vacuum Pump Open Bottom with Dolly","VD"  },
                    {"Vented Dry Chemical Storage", "E" },{""," " } };

            Dictionary<string, string> dic_met = new Dictionary<string, string>()
            {{"Metal Top", "M"}, {"Metal Top + Pull Handle", "MP"}, {"No Metal Top", "N"}, {"No Metal Top + Pull Handle", "NP"}, {" ", " " }, {"-", " " } , {"M", "M" } };

            Dictionary<string, string> dic_pullstyle = new Dictionary<string, string>()
             { {"Flush ABS", "P1"},{"Flush Aluminum","P2" }, {"Raised Wire 96 mm","P3" },
                {"Raised Wire 4\"", "P4" }, {"Raised Wire 128 mm", "P5" }, {"Flush Stainless Steel", "P6" },
                {"P1", "P1"},{"P2","P2" }, {"P3","P3" }, {"P4", "P4" }, {"P5", "P5" }, {"P6", "P6" }};

            Dictionary<string, string> dic_hinge = new Dictionary<string, string>()
            { {"LC Pivot Stainless", "H1"}, {"5-Knuckle Stainless", "H2" }, {"3-Knuckle Self-close", "H3" }, {"H1", "H1"}, {"H2", "H2" }, {"H3", "H3" } };

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
                { "", " "},  { "A", "A"},
                {"B","B" },
                { "C","C" },
                {"D","D" },
                { "E", "E" },
                {"F","F" } ,
                {"G", "G" },
                {"H","H" } ,
                 };

            Dictionary<string, string> dic_ch = new Dictionary<string, string>()
            {
                {"No Cardholder"," " },
                {"Single Card Holder Riveted Stainless Steel","1" },
                {"Card Holder on all File Drawers Riveted Stainless Steel","2" },
                { "Card Holder on all Doors and Drawers Riveted Stainless Steel", "3" }, {"", " "}
            };

            Dictionary<string, string> dic_pulloutshelf = new Dictionary<string, string>()
            {
                {"-1", "0" },
                {"0", "0" }, {"1", "1"}, {"2", "2"}, {"3", "3"}
            };

            string output1 = "";
            string output2 = "";
            string output3 = "";
            string output4 = "";

            output1 += dic_mod[model] + "_";
            output1 += dic_spec[specialdesign] + "_";
            output1 += cstyle + "_";
            output1 += dic_met[metaltop] + "_";
            output1 += w.ToString() + "_";
            output1 += h.ToString() + "_";
            output1 += d.ToString() + "_";
            string pullOutShelfString = pulloutshelf + ",";
            int y_ = 0;
            for (int i = 0; i < Convert.ToInt16(pulloutshelf) - 1; i++)
            {
                y_ += heihei[i];
                pullOutShelfString += y_.ToString();
                if (i != Convert.ToInt16(pulloutshelf) - 2)
                {
                    pullOutShelfString += ",";

                }
            }
            output1 += pullOutShelfString + "_";
            output1 += secpanel.ToString() + "_";
            output1 += selfcloseforsb;

            for (int i = 0; i < drawerx.Count; i++)
            {
                output2 += drawerx[i].ToString() + "," + drawery[i].ToString() + "," + drawerw[i].ToString() + "," + drawerh[i].ToString() + "," + drawerl[i] + "," + drawerch[i] + "," + drawerload[i] + ",";
                if (specialdesign == "Open Face")
                {
                    output2 += "2";
                }
                else
                {
                    output2 += "1";

                }
                output2 += "_";
            }
            try
            {
                output2 = output2.Substring(0, output2.Count() - 1);
            }
            catch { }
            for (int i = 0; i < doorx.Count; i++)
            {
                output3 += doorcon[i] + "," + doormat[i] + "," + doorx[i].ToString() + "," + doory[i].ToString() + "," + doorw[i].ToString() + "," + doorh[i].ToString() + "," + doorl[i] + "," + doorch[i] + "," + selfcloseforsb + ",";
                if (specialdesign == "Open Face")
                {
                    output3 += "2";
                }
                else
                {
                    output3 += "1";

                }
                output3 += "_";
            }
            try
            {
                output3 = output3.Substring(0, output3.Count() - 1);
            }
            catch { }


            output4 += dic_pullstyle[pullstyle] + "_";
            output4 += dic_hinge[hingestyle] + "_";
            output4 += tracktype + "_";
            output4 += dic_loc[lockprofile] + "_";
            output4 += dic_ch[cardprofile] + "_";
            output4 += lockgroupby;


            string output = output1 + "-" + output2 + "-" + output3 + "-" + output4;
            return output;
        }

        public static List<string> fromFormalNameToCabDetail(string formalName)
        {
            Dictionary<string, string> modelDic = new Dictionary<string, string>()
            { {"B", "Base" }, {"M", "Mobile" }, {"G", "Glide" }, {"C", "Suspended"}, {"W", "Wall" },  {"S", "Storage" } };
            Dictionary<string, string> widthDic_B = new Dictionary<string, string>()
            { {"2", "29"}, {"H", "32"}, {"3", "35"}};
            Dictionary<string, string> widthDic_M = new Dictionary<string, string>()
            { {"L", "19"}, {"1", "22"}, {"2", "25"}, {"H", "28"}};
            List<string> output = new List<string>();
            string model = "";
            string cstyle = "";
            string specialDesign = "";
            string drawerstring = "";
            string doorstring = "";
            string w = "";
            string h = "";
            string d = "";
            model = modelDic[formalName.Substring(0, 1)];
           
            if ((model == "Base")||(model == "Mobile")||(model == "Glide")||(model == "Suspended"))
            {
                for (int i = 1; i < formalName.Count(); i++)
                {
                    try
                    {
                        int temp = Convert.ToInt32(formalName.Substring(i, 1));
                        break;
                    }
                    catch
                    {
                        specialDesign += formalName.Substring(i, 1);
                    }
                }
                if (specialDesign == "")
                {
                    specialDesign = "-";
                }
                if (model == "Base")
                {
                    if (new List<string>() { "F", "A", "VP", "VD", "E" }.Contains(specialDesign))
                    {
                        model = "Special Base";
                    }
                }
                string[] formalNameArray = formalName.Split('-');
                string tempstring = formalNameArray[0].Substring(formalNameArray[0].Count() - 5, 5);
                w = tempstring.Substring(0, 2);
                h = tempstring.Substring(2, 1);
                if ((model == "Base")|| (model == "Suspended"))
                {
                    h = widthDic_B[h];
                }
                else
                {
                    h = widthDic_M[h];
                }
                cstyle = tempstring.Substring(3, 2);
                d = "22";
            }



            output = new List<string>() { model, cstyle, specialDesign, w, h, d };
            return output;
        }
        public static double cabinetFullHeight(int h, string model)
        {
            double output = h;
            if ((model == "B")||(model == "Base"))
            {
                output = (Convert.ToDouble(h) - 32) * 31 / 32 + 31.91;
            }
            else if ((model == "M")||(model == "Mobile"))
            {
                output = (Convert.ToDouble(h) - 28) * 31 / 32 + 28.05984252;
            }
            else if ((model == "W")||(model == "Wall"))
            {
                if (h >= 31)
                {
                    output = (Convert.ToDouble(h));
                }
                else
                {
                    output = (31 - Convert.ToDouble(h))/32 + Convert.ToDouble(h);
                }
            }
            return output;
        }
        public static double cabinetInsideHeight(double fullHeight, string model, string cstyle)
        {
            double output = 0;

            double toe = 0;
            double b = 0;//bottom frame thickness
            double c = 0;//top frame thickness
            //bool toespaceofnot = false;

            if ((model == "B")||(model == "Base"))
            {
                //toespaceofnot = true;
                toe = -3.69015748-0.1;
                b = -0.94015748;
                c = 0.94015748+0.06;
            }
            else if ((model == "M") || (model == "Mobile"))
            {
                toe = 0;
                b = -0.94015748;
                c = 0.94015748+0.06;
            }
            else if ((model == "W")||(model == "Wall"))
            {
                toe = 0;
                b = -0.94015748;
                if (cstyle == "PH")
                {
                    c = 0;
                }
                else
                {
                    c = 0.94015748;
                }
            }

            double insidehight = fullHeight - c + b + toe;
            
            return insidehight;
        }
        public static double transitInsideToOutside(string model, string cstyle)
        {
            double output = 0;
            double toe = 0;
            double b = 0;//bottom frame thickness
            double c = 0;//top frame thickness
            //bool toespaceofnot = false;

            if ((model == "B") || (model == "Base"))
            {
                //toespaceofnot = true;
                toe = -3.69015748-0.1;
                b = -0.94015748;
                c = 0.94015748+0.06;
            }
            else if ((model == "M") || (model == "Mobile"))
            {
                toe = 0;
                b = -0.94015748;
                c = 0.94015748+0.06;
            }
            else if ((model == "W") || (model == "Wall"))
            {
                toe = 0;
                b = -0.94015748;
                if (cstyle == "PH")
                {
                    c = 0;
                }
                else
                {
                    c = 0.94015748;
                }
            }
            output = -(c + b + toe) / 2;
            
            return output;
        }
        public static string formalCabName(string model, string cstyle, string specialdesign, string metop, int w, int h, int d, List<int> drawer_w, List<int> drawer_h, List<int> drawer_x,
            List<int> door_x, List<int> door_y, List<int> door_face, List<int> door_conf, List<int> door_mat, string pullstyle, string hinge, string track, string lockprofile, string cardholder,
            string selfclosestring, string pulloutShelfNum)
        {
            string nameofcabinet = "";
            Dictionary<string, string> dic_specialdesign = new Dictionary<string, string>()
            { {"Open Face", "OF"},
                {"Sink", "S" },
                {"Corner Cabinet", "C" },
                {"Fume Hood General Storage", "H" },
                {"-", "" } ,
                {" ", "" },
                    {"Flammable Liquid Storage", "F" },
                    { "Acid/ Base Corrosive Storage","A" },
                    { "Vacuum Pump Storage", "VP" },
                    { "Vacuum Pump Open Bottom with Dolly","VD"  },
                    {"E", "E" },
                    {"OF", "OF"},
                {"S", "S" },
                {"C", "C" },
                {"H", "H" }, {"", ""},
                    {"F", "F" },
                    { "A","A" },
                    { "VP", "VP" },
                    { "VD","VD"  },

            };
            Dictionary<int, string> dic_doorconfiguration = new Dictionary<int, string>()
            {{0,"" }, {1, "L"},{2, "R"}, {3, "LR"}, {4, "S" } };
            Dictionary<string, string> dic_pullstyle = new Dictionary<string, string>()
            { {"Flush ABS", "P1"},{"Flush Aluminum","P2" }, {"Raised Wire 96 mm","P3" },
                {"Raised Wire 4\"", "P4" }, {"Raised Wire 128 mm", "P5" }, {"Flush Stainless Steel", "P6" },
                    {"Paddle Latch", "P7" } };
            Dictionary<string, string> dic_hinge = new Dictionary<string, string>()
            { {"LC Pivot Stainless", "H1"}, {"5-Knuckle Stainless", "H2" }, {"3-Knuckle Self-close", "H3" } };
            Dictionary<string, string> dic_loc = new Dictionary<string, string>()
            { { "No Lock",""},{ " ",""},
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
                { "Lock for Bottom Doors Only (Astrigal included if required)", "C" } };
            Dictionary<string, string> dic_ch = new Dictionary<string, string>()
            {
                {"No Cardholder","" },
                {"Single Card Holder Riveted Stainless Steel","1" },
                {"Card Holder on all File Drawers Riveted Stainless Steel","2" },
                { "Card Holder on all Doors and Drawers Riveted Stainless Steel", "3" }
            };
            Dictionary<string, string> dic_pulloutshelf = new Dictionary<string, string>()
            {
                {"0", "" }, {"1", "J"}, {"2", "K"}, {"3", "L"}
            };
            Dictionary<string, string> dic_metaltop = new Dictionary<string, string>()
            { {"Metal Top", "M"}, {"No Metal Top", "N"},
                {"Metal Top + Pull Handle", "MP" }, {"No Metal Top + Pull Handle", "NP"},
            {"M", "M"}, {"N", "N"},
                {"MP", "MP" }, {"NP", "NP"}};

            
            int doorindex1 = 0;
            int doorindex2 = 0;
            int doormat1 = 0;
            int doormat2 = 0;
            if (door_conf.Count == 0)
            { }
            else if (door_conf.Count == 1)
            {
                doorindex1 = door_conf[0];
                doormat1 = door_mat[0];
            }
            else if (door_conf.Count == 2)
            {
                if (door_y[0] == door_y[1])
                {
                    if (door_conf[0] > 3)
                    {
                        doorindex1 = 4;

                    }
                    else
                    {
                        doorindex1 = 3;
                    }
                    doormat1 = door_mat[0];
                }
                else
                {
                    doorindex1 = door_conf[0];
                    doorindex2 = door_conf[1];
                    doormat1 = door_mat[0];
                    doormat2 = door_mat[1];
                }
            }
            else if (door_conf.Count == 4)
            {
                if (door_conf[0] > 3)
                {
                    doorindex1 = 4;
                }
                else
                {
                    doorindex1 = 3;
                }
                if (door_conf[2] > 3)
                {
                    doorindex2 = 4;
                }
                else
                {
                    doorindex2 = 3;
                }
                doormat1 = door_mat[0];
                doormat2 = door_mat[2];
            }
            int hInside = h;
            if ((model == "B") || (model == "SB"))
            {
                hInside -= 5;
            }
            else if ((model == "Storage") || (model == "S"))
            {
                hInside -= 4;
            }
            else
            {
                hInside -= 1;
            }
            if ((model == "Base") || (model == "B"))
            {
                Dictionary<int, string> dic_h = new Dictionary<int, string>()
                { {29, "2"}, {32, "H"}, {35, "3"}};
                nameofcabinet += "B";
                nameofcabinet += dic_specialdesign[specialdesign];
                int drawernum = 0;
                int widedrawernum = 0;
                int filedrawernum = 0;
                int drawer1overdoor1 = 0;
                if ((specialdesign != "Sink") && (specialdesign != "S") &&
                                             (((specialdesign != "A") && (specialdesign != "Acid/ Base Corrosive Storage")) || (h == 29)) && (specialdesign != "H") && (specialdesign != "Fume Hood General Storage"))
                {
                    for (int i = 0; i < drawer_w.Count; i++)
                    {
                        drawernum++;

                        if (w >= 300)
                        {
                            if (drawer_w[i] == w)
                            {
                                widedrawernum++;
                            }
                        }
                        if (drawer_h[i] >= 120)
                        {
                            filedrawernum++;
                        }
                        if ((door_x.Count == 1) && (door_x[0] == drawer_x[i]))
                        {
                            drawer1overdoor1++;
                        }
                    }

                }
                if (drawernum > 0)
                {
                    nameofcabinet += drawernum.ToString();
                }
                if (drawer1overdoor1 == 1)
                {
                    nameofcabinet += "D";
                }
                if ((specialdesign != "Sink") && (specialdesign != "S") &&
                                              (((specialdesign != "A") && (specialdesign != "Acid/ Base Corrosive Storage")) || (h == 29)) && (specialdesign != "H") && (specialdesign != "Fume Hood General Storage"))
                {
                    if (drawer_h.Count > 0)
                    {
                        if (drawer_h[0] == 30)
                        {
                            nameofcabinet += "S";
                        }
                    }
                }
                if (widedrawernum > 0)
                {
                    nameofcabinet += "W" + widedrawernum.ToString();
                }
                if (filedrawernum > 0)
                {
                    nameofcabinet += "F" + filedrawernum.ToString();
                }
                //if (checkBox1.Checked == true)
                //{
                //    nameofcabinet += "P";
                //}
                if (door_x.Count > 0)
                {
                    bool blankornot = false;
                    for (int i = 0; i < door_x.Count; i++)
                    {
                        if (door_face[i] == 0)
                        {
                            blankornot = true;
                            break;
                        }
                    }

                    if (blankornot == false)
                    {
                        nameofcabinet += dic_doorconfiguration[doorindex1];
                    }
                    else
                    {
                        nameofcabinet += "B";
                    }
                }

                nameofcabinet += w.ToString();//2
                nameofcabinet += dic_h[h];//1
                nameofcabinet += cstyle;//2
                try
                {
                    string ex1 = "";

                    ex1 += pullstyle;
                    ex1 += hinge;
                    ex1 += track;
                    if (ex1 != "")
                    {
                        nameofcabinet += "-" + ex1;
                    }
                    string ex2 = "";

                    ex2 += lockprofile;
                    ex2 += cardholder;
                    ex2 += dic_pulloutshelf[pulloutShelfNum];
                    if (ex2 != "")
                    {
                        nameofcabinet += "-" + ex2;
                    }
                }
                catch { }


            }
            if ((model == "Special Base") || (model == "SB"))
            {
                Dictionary<int, string> dic_h = new Dictionary<int, string>()
                { {29, "2"}, {35, "3"}};
                nameofcabinet += "B";
                nameofcabinet += dic_specialdesign[specialdesign];
                int drawernum = 0;
                int widedrawernum = 0;
                int filedrawernum = 0;
                Dictionary<int, string> doordicforsb = new Dictionary<int, string>()
                { { 0, "B" }, {1, "L" }, {2, "R" }, {3, "LR" } };
                nameofcabinet += doordicforsb[doorindex1];
                if (selfclosestring == "SC")
                {
                    nameofcabinet += "SC";
                }
                nameofcabinet += w.ToString();//2
                nameofcabinet += dic_h[h];//1
                nameofcabinet += cstyle;//2
                try
                {
                    string ex1 = "";

                    ex1 += pullstyle;
                    ex1 += hinge;
                    ex1 += track;
                    if (ex1 != "")
                    {
                        nameofcabinet += "-" + ex1;
                    }
                    string ex2 = "";
                    if (lockprofile != "F")
                    {
                        lockprofile = "No Lock";
                    }
                    if ((lockprofile != "3") && (lockprofile != "1"))
                    {
                        lockprofile = "No Cardholder";
                    }
                    ex2 += lockprofile;
                    ex2 += cardholder;
                    ex2 += dic_pulloutshelf[pulloutShelfNum];
                    if (ex2 != "")
                    {
                        nameofcabinet += "-" + ex2;
                    }
                }
                catch { }


            }
            else if ((model == "Suspended") || (model == "C"))
            {
                Dictionary<int, string> dic_h = new Dictionary<int, string>()
                { {19, "L"}, {22, "1"}, {25, "2"}, {28, "H"}};
                nameofcabinet += "C";
                nameofcabinet += dic_specialdesign[specialdesign];
                int drawernum = 0;
                int widedrawernum = 0;
                int filedrawernum = 0;
                int drawer1overdoor1 = 0;
                for (int i = 0; i < drawer_h.Count; i++)
                {
                    if (drawer_w[i] != 0)
                    {
                        drawernum++;
                    }
                    if (w >= 300)
                    {
                        if (drawer_w[i] == w)
                        {
                            widedrawernum++;
                        }
                    }
                    if (drawer_h[i] >= 120)
                    {
                        filedrawernum++;
                    }
                    if ((door_x.Count == 1) && (door_x[0] == drawer_x[i]))
                    {
                        drawer1overdoor1++;
                    }
                }

                if (drawernum > 0)
                {
                    nameofcabinet += drawernum.ToString();
                }
                if (drawer1overdoor1 == 1)
                {
                    nameofcabinet += "D";
                }

                if (drawer_h[0] == 30)
                {
                    nameofcabinet += "S";
                }

                if (widedrawernum > 0)
                {
                    nameofcabinet += "W" + widedrawernum.ToString();
                }
                if (filedrawernum > 0)
                {
                    nameofcabinet += "F" + filedrawernum.ToString();
                }
                //if (checkBox1.Checked == true)
                //{
                //    nameofcabinet += "P";
                //}
                nameofcabinet += dic_doorconfiguration[doorindex1];
                nameofcabinet += (w).ToString();
                nameofcabinet += dic_h[h];
                nameofcabinet += cstyle;

                string ex1 = "";
                try
                {
                    ex1 += pullstyle;
                }
                catch { }
                try
                {
                    ex1 += hinge;
                }
                catch { }
                ex1 += track;
                nameofcabinet += "-" + ex1;

                string ex2 = "";
                ex2 += lockprofile;
                ex2 += cardholder;
                ex2 += dic_pulloutshelf[pulloutShelfNum];
                if (ex2 != "")
                {
                    nameofcabinet += "-" + ex2;
                }

            }
            else if ((model == "Storage") || (model == "S"))
            {
                Dictionary<string, string> dic_doormaterial = new Dictionary<string, string>()
                { {"000", "OF"}, {"010", "ST"}, {"020", "GT"}, {"111", "SS"}, {"112", "SG"},
                    {"122", "GG" }, {"121", "GS" }, {"101", "OS" }, {"102", "OG" } };
                Dictionary<string, string> dic_depth = new Dictionary<string, string>()
                { {"13", "3"}, {"16", "6"}, {"22", "2"}};
                nameofcabinet += "S";
                string doomat = "";
                if (doorindex2 != 0)
                {
                    doomat += "1";
                    doomat += doormat1.ToString();
                    doomat += doormat2.ToString();
                }
                else
                {
                    doomat += "0";
                    doomat += doormat1.ToString();
                    doomat += "0";
                }
                nameofcabinet += dic_doormaterial[doomat];
                nameofcabinet += dic_doorconfiguration[doorindex1];
                if (doorindex2 != doorindex1)
                {
                    nameofcabinet += dic_doorconfiguration[doorindex2];
                }
                nameofcabinet += (w).ToString();
                nameofcabinet += (hInside).ToString();
                nameofcabinet += dic_depth[d.ToString()];
                nameofcabinet += cstyle;

                string ex1 = "";
                ex1 += pullstyle;
                ex1 += hinge;
                nameofcabinet += "-" + ex1;

                string ex2 = "";
                ex2 += lockprofile;
                ex2 += cardholder;
                ex2 += dic_pulloutshelf[pulloutShelfNum];

                if (ex2 != "")
                {
                    nameofcabinet += "-" + ex2;
                }
            }
            else if ((model == "Wall") || (model == "W"))
            {
                Dictionary<string, string> dic_doormaterial = new Dictionary<string, string>()
                { {"1", "S"}, {"3", "G1"}, {"4", "G2"}};
                Dictionary<string, string> dic_depth = new Dictionary<string, string>()
                { {"13", "3"}, {"16", "6"}, {"22", "2"}};
                nameofcabinet += "W";
                nameofcabinet += dic_specialdesign[specialdesign];
                nameofcabinet += dic_doormaterial[doormat1.ToString()];

                nameofcabinet += dic_doorconfiguration[doorindex1];
                nameofcabinet += (w).ToString();
                nameofcabinet += hInside.ToString();
                try
                {
                    nameofcabinet += dic_depth[d.ToString()];
                }
                catch
                {
                    nameofcabinet += dic_depth[d.ToString()];
                }
                nameofcabinet += cstyle;

                string ex1 = "";
                try
                {
                    ex1 += pullstyle;
                }
                catch { ex1 += pullstyle; }
                try
                {
                    ex1 += hinge;
                }
                catch { ex1 += hinge; }
                nameofcabinet += "-" + ex1;

                string ex2 = "";
                try
                {
                    ex2 += lockprofile;
                }
                catch { ex2 += lockprofile; }
                ex2 += cardholder;

                if (ex2 != "")
                {
                    nameofcabinet += "-" + ex2;
                }
            }
            else if ((model == "Mobile") || (model == "M"))
            {
                Dictionary<string, string> dic_h = new Dictionary<string, string>()
                {
                    {"19/22", "L" }, {"22/25", "1" }, {"25/28", "2" }, {"28/31", "H" },
                    {"19", "L" }, {"22", "1" }, {"25", "2" }, {"28", "H" }
                };

                nameofcabinet += "M";
                try
                {
                    nameofcabinet += dic_metaltop[metop];
                }
                catch { nameofcabinet += metop; }
                nameofcabinet += dic_specialdesign[specialdesign];
                int drawernum = 0;
                int widedrawernum = 0;
                int filedrawernum = 0;
                int drawer1overdoor1 = 0;
                for (int i = 0; i < drawer_h.Count; i++)
                {
                    if (drawer_w[i] != 0)
                    {
                        drawernum++;
                        if (drawer_h[i] >= 120)
                        {
                            filedrawernum++;
                        }
                    }
                    if (w >= 300)
                    {
                        if (drawer_w[i] == w)
                        {
                            widedrawernum++;
                        }
                    }
                    if ((door_x.Count == 1) && (door_x[0] == drawer_x[i]))
                    {
                        drawer1overdoor1++;
                    }
                }
                if (drawernum != 0)
                {
                    nameofcabinet += drawernum.ToString();
                }
                //if (checkBox2.Checked == true)
                //{
                //    nameofcabinet += "A";
                //}
                if (drawer1overdoor1 == 1)
                {
                    nameofcabinet += "D";
                }
                if (drawer_h.Count > 0)
                {
                    if (drawer_h[0] == 30)
                    {
                        nameofcabinet += "S";
                    }
                }

                if (widedrawernum > 0)
                {
                    nameofcabinet += "W" + widedrawernum.ToString();
                }
                if (filedrawernum > 0)
                {
                    nameofcabinet += "F" + filedrawernum.ToString();
                }
                //if (checkBox1.Checked == true)
                //{
                //    nameofcabinet += "P";
                //}
                nameofcabinet += dic_doorconfiguration[doorindex1];
                nameofcabinet += (w).ToString();
                try
                {
                    nameofcabinet += dic_h[h.ToString()];
                }
                catch { nameofcabinet += dic_h[h.ToString()]; }
                nameofcabinet += cstyle;

                string ex1 = "";
                try
                {
                    ex1 += pullstyle;
                }
                catch
                {
                    ex1 += pullstyle;
                }
                try
                {
                    ex1 += hinge;
                }
                catch { ex1 += hinge; }
                ex1 += track;

                nameofcabinet += "-" + ex1;

                string ex2 = "";
                ex2 += lockprofile;
                ex2 += cardholder;
                ex2 += dic_pulloutshelf[pulloutShelfNum];
                if (ex2 != "")
                {
                    nameofcabinet += "-" + ex2;
                }
            }
            else if ((model == "Glide") || (model == "G"))
            {
                Dictionary<string, string> dic_h = new Dictionary<string, string>()
                {
                    {"19/24", "L" }, {"22/27", "1" }, {"25/30", "2" }, {"28/33", "H" },
                        {"19", "L" }, {"22", "1" }, {"25", "2" }, {"28", "H" }
                };

                nameofcabinet += "G";
                nameofcabinet += dic_metaltop[metop];
                nameofcabinet += dic_specialdesign[specialdesign];
                int drawernum = 0;
                int widedrawernum = 0;
                int filedrawernum = 0;
                int drawer1overdoor1 = 0;
                for (int i = 0; i < drawer_h.Count; i++)
                {
                    if (drawer_w[i] != 0)
                    {
                        drawernum++;
                        if (drawer_h[i] >= 120)
                        {
                            filedrawernum++;
                        }
                    }
                    if (w >= 300)
                    {
                        if (drawer_w[i] == w)
                        {
                            widedrawernum++;
                        }
                    }
                    if ((door_x.Count == 1) && (door_x[0] == drawer_x[i]))
                    {
                        drawer1overdoor1++;
                    }
                }
                nameofcabinet += drawernum.ToString();
                //if (checkBox2.Checked == true)
                //{
                //    nameofcabinet += "A";
                //}
                if (drawer1overdoor1 == 1)
                {
                    nameofcabinet += "D";
                }
                if (drawer_h[0] == 30)
                {
                    nameofcabinet += "S";
                }

                if (widedrawernum > 0)
                {
                    nameofcabinet += "W" + widedrawernum.ToString();
                }
                if (filedrawernum > 0)
                {
                    nameofcabinet += "F" + filedrawernum.ToString();
                }
                //if (checkBox1.Checked == true)
                //{
                //    nameofcabinet += "P";
                //}
                nameofcabinet += dic_doorconfiguration[doorindex1];
                nameofcabinet += (w).ToString();
                try
                {
                    nameofcabinet += dic_h[h.ToString()];
                }
                catch
                {
                    nameofcabinet += dic_h[h.ToString()];
                }
                nameofcabinet += cstyle;

                string ex1 = ""; try
                {
                    ex1 += pullstyle;
                }
                catch { ex1 += pullstyle; }
                try
                {
                    ex1 += hinge;
                }
                catch { ex1 += hinge; }
                ex1 += track;
                nameofcabinet += "-" + ex1;

                string ex2 = "";
                ex2 += lockprofile;
                ex2 += cardholder;
                ex2 += dic_pulloutshelf[pulloutShelfNum];
                if (ex2 != "")
                {
                    nameofcabinet += "-" + ex2;
                }
            }
            return nameofcabinet;
        }

        public static double counterWeigh(List<double> drawerCenter_z, List<double> drawerCenter_y, List<double> drawerG,
            List<double> pulloutshelfCenter_z, List<double> pulloutshelfCenter_y, List<double> pulloutShelfG,
            double shell_z, double shell_y, double shellG,
            int w, int h_orginal, bool levelontop, int styleNum, string specialDesign, string cwkey /*1:back  2:bottom*/)
        {

            double cw = 0;
            for (int i = 0; i < drawerCenter_z.Count; i++)
            {
                cw += (-1) * drawerCenter_z[i] * drawerG[i];
            }
            for (int i = 0; i < pulloutshelfCenter_z.Count; i++)
            {
                cw += (-1) * pulloutshelfCenter_z[i] * pulloutShelfG[i];
            }

            double testTopPoint_y = (28 - (28 - Convert.ToDouble(h_orginal)) * 31 / 32) - 1 - 1.37 + 2;
            if (specialDesign == "OF")
            {
                double y1 = Convert.ToDouble(cwkey.Split('-')[0].Split('_')[7].Split(',')[1]);
                testTopPoint_y = 6 + ((h_orginal - 4) - y1) * 31 / 32;
            }

            cw += (-1) * shell_z * shellG;

            if ((w >= 20) && (levelontop == true))
            {
                cw += 44 * (testTopPoint_y) / 4.45;
            }

            if (styleNum == 1)
            {
                cw /= 17.5412;
            }
            else if (styleNum == 2)
            {
                cw /= 12.7277666;
            }


            return cw;
        }
        public static double readCounterWeight(string cabname, List<int> drawer_y, int width, int height)
        {
            Database database = new Database();
            double output = 0;
            MySqlDataReader rd = database.getMySqlReader("select * from counterweightinfo where cabname = '" + cabname + "'");
            if (rd.Read())
            {
                string cwdrcenter = rd.GetString("dr_cent");
                string cwdrg = rd.GetString("dr_g");
                string cwshcenter = rd.GetString("sh_cent").Split('_')[0];
                string cwshg = rd.GetString("sh_g").Split('_')[0];
                string[] cwdrcen = cwdrcenter.Split('_');
                string[] cwdrgg = cwdrg.Split('_');
                List<double> dr_cen_z = new List<double>();
                List<double> dr_cen_y = new List<double>();
                List<double> dr_g = new List<double>();
                List<double> ps_cen_z = new List<double>();
                List<double> ps_cen_y = new List<double>();
                List<double> ps_g = new List<double>();
                double sh_cen_z = 0;
                double sh_cen_y = 0;
                double sh_g = 0;
                for (int i = 0; i < cwdrcen.Count(); i++)
                {
                    string[] temp = cwdrcen[i].Split(',');
                    dr_cen_z.Add(Convert.ToDouble(temp[0]));
                    dr_cen_y.Add(Convert.ToDouble(temp[1]));
                    dr_g.Add(Convert.ToDouble(cwdrgg[i]));
                }
                sh_cen_z = Convert.ToDouble(cwshcenter.Split(',')[0]);
                sh_cen_y = Convert.ToDouble(cwshcenter.Split(',')[1]);
                sh_g = Convert.ToDouble(cwshg);
                bool levelontop = false;
                if (drawer_y.Contains(0))
                {
                    levelontop = true;
                }
                double counterWeight = CabinetOverview.counterWeigh(dr_cen_z, dr_cen_y, dr_g, ps_cen_z, ps_cen_y, ps_g, sh_cen_z, sh_cen_y, sh_g,
                    width, height, levelontop, 1, cabname.Split('-')[0].Split('_')[1], cabname);
                output = counterWeight;
            }
            database.CloseConnection();
            return output;
        }
    }
}
