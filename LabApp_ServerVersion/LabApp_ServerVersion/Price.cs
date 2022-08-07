using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class Price
    {
        public static List<string> nametocathwd(string name)
        {
            name = name.Split('+')[0];
            int d = 0;
            List<string> output = new List<string>();
            string[] namestr_ = name.Split('-');
            List<string> namestr = namestr_.ToList();
            int namestrLen = namestr.Count;

            if ((namestr[namestrLen - 1] == "SS"))
            {
                namestr.Remove(namestr[namestrLen - 1]);
                namestrLen--;
            }
            if ((namestr[namestrLen - 1] == "L") || (namestr[namestrLen - 1] == "R"))
            {
                try
                {
                    d = Convert.ToInt32(namestr[namestrLen - 1]);
                    namestr.Remove(namestr[namestrLen - 1]); namestrLen--;
                }
                catch { }
                namestr.Remove(namestr[namestrLen - 1]); namestrLen--;

            }

            if (namestr[0].Substring(0, 1) == "D")
            {

                output = namereaddrawer(namestr);
            }
            else if ((namestr[0].Count() >= 4) && (namestr[0].Substring(0, 4) == "LC15"))
            {
                output = namereaddoor1(namestr);
            }
            else if ((namestr[0].Count() >= 4) && (namestr[0].Substring(2, 2) == "35"))
            {
                output = namereaddoor2(namestr);
            }
            else if ((namestr[0].Count() >= 6) && (namestr[0].Substring(0, 6) == "SC4180"))
            {
                output = new List<string>() { "SC4180", "84", "0", "0" };
            }
            else
            {
                if (namestr.Count == 1)
                {
                    if ((namestr[0].Substring(2, 1) == "1") &&
                        ((namestr[0].Substring(3, 1) == "0") || (namestr[0].Substring(3, 1) == "3")))
                    {
                        output = nameread1(namestr);
                    }
                    else if (namestr[0].Substring(2, 1) == "2")
                    {
                        if (namestr[0].Substring(3, 1) == "0")
                        {
                            output = nameread1(namestr);
                        }
                        else
                        {
                            output = nameread3(namestr);
                        }

                    }
                    else if ((namestr[0].Count() >= 3) && (namestr[0].Substring(0, 3) == "GCB"))
                    {
                        output.Add(namestr[0].Substring(0, 6));
                        output.Add(namestr[0].Substring(6, namestr[0].Count() - 6));
                    }
                    else
                    {
                        output = nameread4(namestr);
                    }
                }
                else if (namestr.Count == 2)
                {
                    if ((namestr[0].Count() >= 2) && (namestr[0].Substring(0, 2) == "LC"))
                    {
                        if (namestr[0].Substring(2, 1) == "1")
                        {
                            if ((namestr[0].Substring(3, 1) == "1") || (namestr[0].Substring(3, 1) == "7"))
                            {
                                if ((namestr[0].Substring(3, 1) == "1") && (namestr[0].Substring(4, 2) == "50"))
                                {
                                    output = nameread3(namestr);
                                }
                                else
                                {
                                    output = nameread2(namestr);
                                }
                            }
                            else if (namestr[0].Substring(3, 1) == "3")
                            {
                                output = namereadF(namestr);
                            }
                            else if (namestr[0].Substring(3, 2) == "46")
                            {
                                output = nameread3(namestr);
                            }
                            else if (namestr[0].Substring(3, 2) == "40")
                            {
                                output = namereadI(namestr);
                            }
                            else
                            {
                                output = nameread3(namestr);
                            }
                        }
                        else if (namestr[0].Substring(2, 1) == "2")
                        {
                            if (namestr[0].Substring(3, 1) == "0")
                            {
                                output = nameread1(namestr);
                            }
                            else if (namestr[0].Substring(3, 1) == "1")
                            {
                                output = nameread2(namestr);
                            }
                            else
                            {
                                output = nameread3(namestr);
                            }
                        }
                        else if (namestr[0].Substring(2, 1) == "3")
                        {
                            if (namestr[0].Substring(3, 1) == "3")
                            {
                                if ((namestr[0].Substring(4, 1) != "5") && (namestr[0].Substring(4, 1) != "S"))
                                {
                                    output = nameread2(namestr);
                                }
                                else if (namestr[0].Substring(4, 1) == "S")
                                {
                                    output = new List<string>() { "SC-LC33", namestr[1], "0", "0" };
                                }
                                else
                                {
                                    output = nameread3(namestr);
                                }
                            }
                            else if (namestr[0].Substring(3, 1) == "6")
                            {
                                output = nameread3(namestr);
                            }
                        }

                    }
                    else if ((namestr[0].Count() >= 2) && (namestr[0].Substring(0, 2) == "WC"))
                    {
                        if (namestr[0].Substring(2, 1) == "1")
                        {
                            if (namestr[0].Substring(3, 1) == "1")
                            {
                                output = nameread2(namestr);
                            }
                            else if ((namestr[0].Substring(3, 1) == "2") || (namestr[0].Substring(3, 1) == "6"))
                            {
                                output = namereadA(namestr);
                            }
                        }
                        else if (namestr[0].Substring(2, 1) == "3")
                        {
                            if (namestr[0].Substring(3, 1) == "0")
                            {
                                output = nameread9(namestr);
                            }
                            else if (namestr[0].Substring(3, 1) == "1")
                            {
                                if (namestr[0].Substring(4, 1) == "5")
                                {
                                    output = namereadB(namestr);
                                }
                                else
                                {
                                    output = nameread3(namestr);
                                }
                            }
                            else if (namestr[0].Substring(3, 1) == "4")
                            {
                                output = namereadB(namestr);
                            }
                            else
                            {
                                output = nameread3(namestr);
                            }
                        }
                    }
                    else if ((namestr[0].Count() >= 2) && (namestr[0].Substring(0, 2) == "SC"))
                    {
                        if (namestr[0].Substring(2, 1) == "4")
                        {
                            if (namestr[0].Substring(3, 1) == "0")
                            {
                                output = nameread9(namestr);
                            }
                            else if (namestr[0].Substring(3, 1) == "1")
                            {
                                if (namestr[0].Substring(4, 2) == "50")
                                {
                                    output = namereadB(namestr);
                                }
                                else if ((namestr[0].Substring(4, 2) == "81") || (namestr[0].Substring(4, 2) == "80"))
                                {
                                    output = namereadB(namestr);
                                }
                                else
                                {
                                    output = nameread2(namestr);
                                }
                            }
                            else if (namestr[0].Substring(3, 1) == "2")
                            {
                                output = nameread3(namestr);
                            }
                        }
                    }

                    else
                    {
                        output = nameread5(namestr);
                    }
                }
                else if (namestr.Count == 3)
                {
                    if (namestr[0].Substring(0, 1) == "S")
                    {
                        output = namereadC(namestr);
                    }
                    else
                    {
                        if (namestr[1].Substring(0, 2) == "LC")
                        {
                            if (namestr[1].Substring(2, 2) == "11")
                            {
                                output = nameread7(namestr);
                            }
                            else if (namestr[1].Substring(2, 2) == "13")
                            {
                                output = namereadG(namestr);
                            }
                            else if (namestr[1].Substring(2, 4) == "1460")
                            {
                                output = nameread8(namestr);
                            }
                            else if (namestr[1].Substring(2, 4) == "1400")
                            {
                                output = namereadH(namestr);
                            }
                            else if (namestr[1].Substring(2, 2) == "17")
                            {
                                output = nameread7(namestr);
                            }
                            else
                            {
                                output = nameread6(namestr);
                            }
                        }
                        else if (namestr[1].Substring(0, 2) == "WC")
                        {
                            output = nameread8(namestr);
                        }
                        else if (namestr[1].Substring(0, 2) == "SC")
                        {
                            if (namestr[1].Substring(2, 2) == "41")
                            {
                                output = nameread8(namestr);
                            }
                            else if (namestr[1].Substring(2, 2) == "40")
                            {
                                output = namereadD(namestr);
                            }
                        }
                    }
                }
                else if (namestr.Count == 4)
                {
                    output = namereadE(namestr);
                }
            }
            if (d != 0)
            {
                output[3] = d.ToString();
            }
            return output;
        }
        public static List<string> namereaddrawer(List<string> namestr)
        {
            string cat = "";
            int w = 0; int h = 0; int d = 0;
            if (namestr[0].Substring(2, 1) == "H")
            {
                string h_ = "";
                int tem = 0;
                for (int i = namestr[0].Count() - 1; i >= 0; i--)
                {
                    try
                    {
                        tem = Convert.ToInt32(namestr[0].Substring(i, 1));
                        h_ = namestr[0].Substring(i, 1) + h_;
                    }
                    catch
                    { cat = namestr[0].Substring(i, 1) + cat; }

                }
                h = Convert.ToInt32(h_);
                string w_ = "";
                for (int i = 0; i < namestr[1].Count(); i++)
                {
                    try
                    {
                        tem = Convert.ToInt32(namestr[1].Substring(i, 1));
                        w_ += namestr[1].Substring(i, 1);
                    }
                    catch { break; }
                }
                w = Convert.ToInt32(w_);
            }
            else if (namestr[0].Substring(2, 1) == "B")
            {
                cat = "DRBU";
                h = Convert.ToInt32(namestr[0].Substring(4, namestr[0].Count() - 4));
                w = Convert.ToInt32(namestr[1].Split('S')[0]);
            }
            return new List<string>() { cat, w.ToString(), h.ToString(), d.ToString() };
        }
        public static List<string> namereaddoor1(List<string> namestr)
        {
            string cat = "";
            int w = 0; int h = 0; int d = 0;
            cat += namestr[0].Substring(0, 4);
            int t = namestr[0].Count();

            if (namestr[0].Substring(t - 1, 1) == "I")
            {
                cat += "-I";
                h = Convert.ToInt32(namestr[0].Substring(t - 3, 2));
            }
            else
            {
                cat += "-O";
                h = Convert.ToInt32(namestr[0].Substring(4, 2));
            }
            cat += "-" + namestr[1].Substring(2, 1);

            w = Convert.ToInt32(namestr[1].Substring(0, 2));
            return new List<string>() { cat, w.ToString(), h.ToString(), d.ToString() };
        }
        public static List<string> namereaddoor2(List<string> namestr)
        {
            string cat = ""; string w = "0"; string h = "0"; string d = "0";
            cat = namestr[0].Substring(0, 5);
            h = namestr[1];
            w = namestr[2];
            return new List<string>() { cat, w, h, d };
        }
        public static List<string> nameread1(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0].Substring(0, 4);
            h = name[0].Substring(4, 2);
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> nameread2(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0].Substring(0, 4);
            h = name[0].Substring(4, 2);
            w = name[1];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> nameread3(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0];
            w = name[1];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> nameread4(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0];
            
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> nameread5(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0] + "-" + name[1].Substring(0, 4);
            h = name[1].Substring(4, 2);
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> nameread6(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0] + "-" + name[1];
            w = name[2];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> nameread7(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0] + "-" + name[1].Substring(0, 4);
            w = name[2];
            h = name[1].Substring(4, 2);
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> nameread8(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0] + "-" + name[1];
            h = name[2];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> nameread9(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0].Substring(0, 4);
            h = name[0].Substring(4, 2);
            d = name[1];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> namereadA(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0].Substring(0, 4);
            w = name[0].Substring(4, 2);
            d = name[1];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> namereadB(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0];
            h = name[1];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> namereadC(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0];
            w = name[1];
            d = name[2];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> namereadD(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0] + "-" + name[1].Substring(0, 4);
            h = name[1].Substring(4, 2);
            d = name[2];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> namereadE(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0] + "-" + name[1];
            w = name[2];
            d = name[3];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> namereadF(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0].Substring(0, 4) + "-" + name[1];
            h = name[0].Substring(4, 2);
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> namereadG(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0] + "-" + name[1].Substring(0, 4) + "-" + name[2];
            h = name[1].Substring(4, 2);
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> namereadH(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0] + "-" + name[1];
            h = name[2];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }
        public static List<string> namereadI(List<string> name)
        {
            string cat = "";
            string w = "0"; string h = "0"; string d = "0";
            cat = name[0];
            h = name[1];
            List<string> output = new List<string>() { cat, w, h, d };
            return output;
        }

        public static List<double> actualsizeWXL(string cat, int h, int w, int d, int machineamadaornot)
        {
            double aw = 0;
            double al = 0;
            List<double> output = new List<double>();
            if (cat == "DRBU")
            {
                if (h <= 3)
                {
                    al = 21.8177;
                    aw = 15.9642 + (w - 15);
                }
                else if (h <= 6)
                {
                    al = 27.7895;
                    aw = 23.7252 + (w - 15);
                }
                else if (h <= 9)
                {
                    al = 31.4407;
                    aw = 25.2177 + (w - 15);
                }
                else if (h <= 12)
                {
                    al = 34.34;
                    aw = 25.2177 + (w - 15);
                }

            }
            else if (cat == "BP")
            {
                aw = 30;
                al = 30;
            }

            //////SIDE
            else if (cat == "LC10")
            {
                aw = 27.4619;
                al = 35.8811 + (h - 35) * 31 / 32;
            }
            else if (cat == "H-LC10")
            {
                aw = 26.370;
                al = 35.900 + (h - 35) * 31 / 32;
            }
            else if (cat == "NM-LC10")
            {
                aw = 25.031;
                al = 35.868 + (h - 35) * 31 / 32;
            }
            else if (cat == "M-LC10")
            {
                aw = 27.070;
                al = 34.789 + (h - 28);
            }
            else if (cat == "MNM-LC10")
            {
                aw = 25.777;
                al = 34.789 + (h - 28);
            }
            else if (cat == "WC30")
            {
                aw = 26.439 + (h - 25);
                al = 16.898 + (d - 13);
            }
            else if (cat == "A-SC40")
            {
                aw = 85 + (h - 84);
                al = 16.898 + (d - 13);
            }
            else if (cat == "SC40")
            {
                aw = 85 + (h - 84);
                al = 25.896 + (d - 22);
            }
            else if (cat == "LC20")
            {
                aw = 29.052 + (h - 36);
                al = 20.225;
            }
            ///////////BACK
            else if (cat == "LC11")
            {
                aw = 35.95 + w - 36;
                al = 35.8811 + (h - 35);
            }
            else if (cat == "HT-LC11")
            {
                aw = 4.705;
                al = 17.875 + (w - 18);
            }
            else if (cat == "HB-LC11")
            {
                aw = 17.880;
                al = 12.29 + (w - 18);
            }
            else if (cat == "HC-LC11")
            {
                aw = 3.1269;
                al = 5.0620 + (h - 6);
            }
            else if (cat == "M-LC11")
            {
                aw = 11.88 + (w - 12);
                al = 29.355 + (h - 28);
            }
            else if (cat == "WC11")
            {
                aw = 26.315 + (h - 25);
                al = 17.880 + (w - 18);
            }
            else if (cat == "LC1220")
            {
                aw = 3.781;
                al = 17.875 + (w - 18);
            }
            else if (cat == "VAC-LC11")
            {
                aw = 35.935 + (h - 35);
                al = 23.880 + (w - 24);
            }
            else if (cat == "SC41")
            {
                aw = 84.875 + (h - 84);
                al = 17.875 + (w - 18);
            }
            else if (cat == "LC33")
            {
                aw = 35.890 + (h - 35);
                al = 35.880 + (w - 36);
            }
            else if (cat == "A-LC11")
            {
                aw = 0;
                al = 0;
            }
            else if (cat == "LC21")
            {
                aw = (w - 18) + 14.880;
                al = (h - 29) + 42.027;////可能不大对
            }
            else if (cat == "LC1150")
            {
                aw = (w - 18) + 17.875;
                al = 12.218;
            }
            ///TOP
            else if (cat == "LC1230")
            {
                aw = 4.9087;
                al = 35.8811 + (w - 36);
            }
            else if (cat == "H-LC1230")
            {
                aw = 4.930;
                al = 23.880 + (w - 24);
            }
            else if (cat == "NM-LC1230")
            {
                aw = 3.218;
                al = 17.88 + (w - 18);
            }
            else if (cat == "M-LC1230")
            {
                aw = 13.855 + (w - 12);
                al = 25.617;
            }
            else if (cat == "MNM-LC1230")
            {
                aw = 25.6182;
                al = 16.8499 + (w - 15);
            }
            else if (cat == "WC12")
            {
                aw = 11.88 + (w - 12);
                al = 12.95 + (d - 13);
            }
            else if (cat == "FH-LC1230")
            {
                aw = 23.875 + (w - 24);
                al = 2.976;
            }
            else if (cat == "VACTC-LC1240")
            {
                aw = 35.880 + (w - 36);
                al = 19.785;
            }
            else if (cat == "FOHM-SC4230")
            {
                aw = 24.752 + (w - 24);
                al = 23.875 + (d - 22);
            }
            else if (cat == "SC4230")
            {
                aw = 17.880 + (w - 18);
                al = 23.517 + (d - 22);
            }
            else if (cat == "O-SC4230")
            {
                aw = 17.880 + (w - 18);
                al = 24.760 + (d - 22);
            }
            else if (cat == "WC3260")
            {
                aw = 34.38 + (w - 36);
                al = 3.595;
            }
            ///TOE
            else if (cat == "LC1260")
            {
                aw = 10.0612;
                al = 37.3649 + (w - 36);
            }
            else if (cat == "H-LC1260")
            {
                aw = 10.572;
                al = 13.345 + (w - 12);
            }
            else if (cat == "NM-LC1260")
            {
                aw = 9.751;
                al = 19.365 + (w - 18);
            }
            else if (cat == "LC335")
            {
                aw = 37.365 + (w - 36);
                al = 8.243;
            }
            else if (cat == "VAC-LC1260")
            {
                aw = 25.376 + (w - 24);
                al = 10.476;
            }
            else if (cat == "SC4260")
            {
                aw = 19.369 + (w - 18);
                al = 9.711;
            }
            ///SHELF
            else if (cat == "LC1630")
            {
                aw = 21.896;
                al = 36.88 + (w - 36);
            }
            else if (cat == "NM-LC1630")
            {
                aw = 19.70;
                al = 18.88 + (w - 18);
            }
            else if (cat == "WC16")
            {
                aw = 12.880 + (w - 12);
                al = 11.961 + (d - 13);
            }
            else if (cat == "VACB-LC1630")
            {
                aw = 23.009 + (w - 18);
                al = 25.562;
            }
            else if (cat == "SC4631")
            {
                aw = 19.245 + (w - 18);
                al = 21.927 + (d - 22);
            }
            else if (cat == "SC4630")
            {
                aw = 36.880 + (w - 36);
                al = 20.959 + (d - 22);
            }
            else if (cat == "LC3630")
            {
                aw = 21.125 + (w - 18);
                al = 23.375;
            }
            ///REMOVABLEBACK
            else if (cat == "LC17")
            {
                aw = 23.5 + (h - 35);
                al = 20.285 + (w - 24);
            }
            else if (cat == "NM-LC17")
            {
                aw = 23.5 + (h - 35);
                al = 14.285 + (w - 18);
            }
            ///BOTTOM
            else if (cat == "LC1250")
            {
                aw = 23.7644;
                al = 37.2449 + (w - 36);
            }
            else if (cat == "H-LC1250")
            {
                aw = 20.869;
                al = 19.619 + (w - 18);
            }
            else if (cat == "M-LC1250")
            {
                aw = 23.738;
                al = 19.245 + (w - 18);
            }
            else if (cat == "MNM-LC1250")
            {
                aw = 23.7382;
                al = 16.2449 + (w - 15);
            }
            else if (cat == "LC334")
            {
                aw = 17.937;
                al = 5.949;
            }
            else if (cat == "SC-LC33")
            {
                aw = 37.225 + (w - 36);
                al = 26.693;
            }
            else if (cat == "VAC-LC1250")
            {
                aw = 31.256 + (w - 30);
                al = 23.770;
            }
            else if (cat == "SC4250")
            {
                aw = 19.256 + (w - 18);
                al = 14.77 + (d - 13);
            }
            else if (cat == "A-SC4250")
            {
                aw = 19.245 + (w - 30);
                al = 18.541 + (d - 13);
            }
            else if (cat == "LC2250")
            {
                aw = 18.725 + (w - 18);
                al = 24.111;
            }
            ///HIDDEN
            else if (cat == "LC1280")
            {
                aw = 2.588;
                al = 35.88 + (w - 36);
            }
            else if (cat == "NM-LC1280")
            {
                aw = 4.182;
                al = 17.750 + (w - 18);
            }
            ///PARTITION
            else if (cat == "LC1400")
            {
                aw = 22.452;
                al = 23.788 + (w - 24);
            }
            else if (cat == "HNM-LC1400")
            {
                aw = 22.452;
                al = 23.788 + (w - 24);
            }
            ///RUNNER
            else if (cat == "LC13")
            {
                aw = 3.1269;
                al = 5.0620 + (h - 6);
            }
            else if (cat == "LC13-out")
            {
                aw = 5.039;
                al = 8.348 + (h - 6);
            }
            else if (cat == "NM-LC13")
            {
                aw = 4.309;
                al = 28.375 + (h - 35);
            }
            else if (cat == "NM-LC13-out")
            {
                aw = 4.345;
                al = 5.812 + (h - 6);
            }
            else if (cat == "LC13-in")
            {
                aw = 3.1269;
                al = 5.0620 + (h - 6);
            }
            else if (cat == "NM-LC13-in")
            {
                aw = 3.1269;
                al = 5.0620 + (h - 6);
            }
            else if (cat == "VAC-LC13")
            {
                aw = 6.826;
                al = 19.580;
            }
            ///STILESIDE
            else if (cat == "WC3150")
            {
                aw = 5;
                al = 23.310 + (h - 25);
            }
            else if (cat == "NM-WC3150")
            {
                aw = 23.875 + (h - 25);
                al = 4.261 /*+ (d - 13)*/;
            }
            else if (cat == "BNM-WC3150")
            {
                aw = 23.875 + (h - 25);
                al = 4.261 /*+ (d - 13)*/;
            }
            else if (cat == "SC4150")
            {
                aw = 78.120 + (h - 84);
                al = 5.531;
            }
            else if (cat == "Z-SC4150")
            {
                aw = 78.125 + (h - 84);
                al = 5.003;
            }
            ///STILETOP
            else if (cat == "WC3210")
            {
                aw = 5.44;
                al = 11.88 + (w - 12);
            }

            ///STILEBOTTOM
            else if (cat == "WC3130")
            {
                aw = 13.225 + (w - 12);
                al = 13.852 /*+ (d - 13)*/;
            }

            ///FRONT
            else if (cat == "LC1240")
            {
                aw = 17.880 + (w - 18);
                al = 10.810;
            }
            else if (cat == "VAC-LC1240")
            {
                aw = 29.725 + (w - 30);
                al = 8.186;
            }
            else if (cat == "H-LC1240")
            {
                aw = 16.77766 - 18 + w;
                al = 8.321;
            }
            ///GUSSET
            else if (cat == "LC1270")
            {
                aw = 19.036;
                al = 0.5;
            }
            else if (cat == "LC1270G")
            {
                aw = 19.036;
                al = 0.5;
            }
            else if (cat == "LC1270H")
            {
                aw = 19.036;
                al = 0.5;
            }
            else if (cat == "LC1270-FH")
            {
                aw = 19.036;
                al = 0.5;
            }
            ///SECURITYPANEL
            else if (cat == "LC1460")
            {
                aw = 13.13 + (w - 15);
                al = 20.977;
            }
            ///SELFCLOSE


            ///DOOR
            else if (cat == "LC15-I-H")
            {
                aw = 19.212 + h - 24;
                al = 24.272 + (w - 18);
            }
            else if (cat == "LC15-O-H")
            {
                aw = 18.612 + h - 24;
                al = 24.34 + (w - 18);
            }
            else if (cat == "LC15-I-Z")
            {
                aw = 19.212 + h - 24;
                al = 24.272 + (w - 18) + 0.5;
            }
            else if (cat == "LC15-O-Z")
            {
                aw = 18.612 + h - 24;
                al = 24.34 + (w - 18) + 0.5;
            }
            else if (cat == "LC35I")
            {
                aw = 32.095 + (h - 30);
                al = 20.296 + (w - 18);
            }
            else if (cat == "LC35O")
            {
                aw = 24.34 + (h - 24);
                al = 17.83 + (w - 17);
            }
            else if (cat == "LC332RSCHCSS")
            {
                aw = 7;
                al = 6;
            }
            else if (cat == "WC35O")
            {
                aw = 50.035 + (h - 49);
                al = 19.330 + (w - 18);
            }
            else if (cat == "SC35O")
            {
                aw = 81.034 + (h - 80);
                al = 23.828 + (w - 23);
            }
            else if (cat == "WC3460")
            {
                aw = h;
                al = 2.380;
            }
            else if (cat == "WC3490")
            {
                aw = 48.69 + (h - 49);
                al = 4.023;
            }
            else if (cat == "WC3491")
            {
                aw = 48.69 + (h - 49);
                al = 4.023;
            }
            else if (cat == "WC3461")
            {
                aw = 17.938 + (h - 24); ///19.469
                al = 4.205;///may be different for pn/ph  3.268
            }
            else if (cat == "SC4180")
            {
                h = w;
                aw = 77.812 + (h - 80);
                al = 3.276;
            }
            ///DRAWER
            else if (cat == "DRH_IN")
            {
                if (h < 6)
                {
                    aw = 5.0240 + h - 6;
                    al = 19.4740 + (w - 18);
                }
                else
                {
                    aw = 8.0160 + h - 6;
                    al = 19.4269 + (w - 18);
                }

            }
            else if (cat == "DRH_FO")
            {
                aw = 18.53 + (w - 15);
                al = 5.4 + (h - 3);
            }
            else if (cat == "LC1470EFC18")
            {
                aw = 24.344;
                al = 0;
            }
            else if (cat == "LC1470EFCHNM18")
            {
                aw = 24.92;
                al = 0;
            }
            else if (cat == "LC1470EFCH18")
            {
                aw = 20.888;
                al = 0;
            }
            else if (cat == "LC1470ERC18")
            {
                aw = 51.5312;
                al = 0;
            }
            //else if (cat == "LS11")
            //{
            //    aw = 11.875 + w - 12;
            //    al = 12.218 + (h - 35);
            //}



            else if (cat == "LS1230")
            {
                aw = 10.810;
                al = 11.880 + (w - 12);
            }
            else if (cat == "H-LS1230")
            {
                aw = 8.321;
                al = 16.778 + (w - 12);
            }


            else if (cat == "HNM-LC1230")
            {
                aw = 4.9087;
                al = 29.880 + (w - 30);
            }








            else if (cat == "HNM-LC1260")
            {
                aw = 9.7507;
                al = 13.3649 + (w - 12);
            }











            else if (cat == "GCB_S2")
            {
                aw = 19.775;
                al = 5.431;
            }
            else if (cat == "GCB_FR")
            {
                aw = 8.321;
                al = 16.778 + (w - 12);
            }














            /////下面的是瞎编的

            else if (cat == "NM-LC13-in")
            {
                aw = 4.309;
                al = 28.375 + (h - 35);
            }


            ///瞎编的结束了









            ///下面是没用上的










            ////////////////////////////

            if (machineamadaornot == 1)
            {
                al += 1;
                aw += 1;
            }
            else
            {
                al += 0.5;
                aw += 0.5;
            }
            output.Add(aw); output.Add(al);
            return output;
        }
        public static List<double> banzi(double w, double h, int type, double gau, int machineamadaornot)
        {

            List<double> output = new List<double>();
            List<int> www = new List<int>();
            List<double> hhh = new List<double>();
            //if ((type == 2)&&(gau == 0.036))
            //{
            //    www = new List<int>() { 96, 96, 96, 100, 100, 100, 105, 105, 105, 110, 110, 115, 120, 120, 36, 48, 60, 36, 60, 48, 36, 48, 60, 36, 48, 60, 36, 48 };
            //    hhh = new List<double>() { 35.3, 47.3, 59.3, 35.3, 59.3, 47.3, 35.3, 47.3, 59.3, 35.3, 47.3, 59.3, 35.3, 47.3, 95.3, 95.3, 95.3, 99.3, 99.3, 99.3, 104.3, 104.3, 104.3, 109.3, 109.3, 114.3, 119.3, 119.3 };
            //}

            //else
            //{
            //    

            www = new List<int>() { 96, 96, 96, 100, 100, 100, 105, 105, 105, 110, 110, 115, 120, 120, 36, 48, 60, 36, 60, 48, 36, 48, 60, 36, 48, 60, 36, 48 };
            if (machineamadaornot == 1)
            {
                hhh = new List<double>() { 35.3, 47.3, 59.3, 35.3, 59.3, 47.3, 35.3, 47.3, 59.3, 35.3, 47.3, 59.3, 35.3, 47.3, 95.3, 95.3, 95.3, 99.3, 99.3, 99.3, 104.3, 104.3, 104.3, 109.3, 109.3, 114.3, 119.3, 119.3 };
            }
            else
            {
                hhh = new List<double>() { 36, 48, 60, 36, 60, 48, 36, 48, 60, 36, 48, 60, 36, 48, 96, 96, 96, 100, 100, 100, 105, 105, 105, 110, 110, 115, 120, 120 };
            }

            //}

            List<double> are_sheet = new List<double>();
            double jige = 0;
            double shengduoshao = 0;
            int nayige = 0;
            for (int i = 0; i < www.Count(); i++)
            {
                are_sheet.Add(www[i] * hhh[i]);
            }
            for (int i = 0; i < www.Count(); i++)
            {
                double temp = www[i] / w;
                double a1 = Math.Truncate(temp);
                temp = hhh[i] / h;
                double a2 = Math.Truncate(temp);
                double a = a1 * a2;
                double b = are_sheet[i] / a - w * h;
                int c = i;
                if (jige == 0)
                {
                    jige = a;
                    shengduoshao = b;
                    nayige = c;
                }
                else
                {
                    if (b < shengduoshao)
                    {
                        jige = a;
                        shengduoshao = b;
                        nayige = c;
                    }
                }
            }
            output.Add(www[nayige]);
            output.Add(hhh[nayige]);
            output.Add(jige);
            output.Add(shengduoshao);

            return output;

        }
        public static List<double> selectsheet(string cat, int h, int w, int d, int type, double gau, int machineamadaornot)
        {
            List<double> temp = actualsizeWXL(cat, h, w, d, machineamadaornot);
            if ((cat == "LC1270") || (cat == "LC1470EFC18") || (cat == "LC1470EFCHNM18") || (cat == "LC1470EFCH18") || (cat == "LC1470ERC18") || (cat == "LC1270G") || (cat == "LC1270H"))
            {
                List<double> tem = new List<double>();
                tem.Add(temp[0]);
                tem.Add(temp[1]);
                tem.Add(1);
                tem.Add(0);
                tem.Add(0);
                return tem;
            }
            else
            {
                double ww = temp[0]; double hh = temp[1];
                List<double> temp1 = banzi(ww, hh, type, gau, machineamadaornot);
                List<double> temp2 = banzi(hh, ww, type, gau, machineamadaornot);
                if (temp1[3] < temp2[3])
                {
                    temp1.Add(1);
                    return temp1;
                }
                else
                {
                    temp2.Add(2);
                    return temp2;
                }
            }
        }
        public static int matindexCSG(string partname)
        {
            string cat = nametocathwd(partname)[0];
            int type = 0;

            if ((cat == "DRBU"))
            {
                type = 2;
            }
            else if (partname.Contains("SN") == true)
            {
                type = 2;
            }
            else if ((cat == "LC1270") || (cat == "LC1470EFC18") || (cat == "LC1470EFCHNM18") || (cat == "LC1470EFCH18") || (cat == "LC1470ERC18") || (cat == "LC1270G") || (cat == "LC1270H"))
            {
                type = 3;
            }
            else if (partname.Substring(partname.Count() - 2, 2) == "SS")
            {
                type = 2;
            }
            else
            {
                type = 1;
            }


            return type;
        }
        public static double matgauge(string cat)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>()
            {
                /////////////SIDE
                {"LC10", 0.048 },
                { "H-LC10", 0.048 },
                {"NM-LC10", 0.048 },
                { "M-LC10", 0.048 },
                {"MNM-LC10", 0.048 },
                {"WC30", 0.048 },
                {"A-SC40", 0.048 },
                {"SC40", 0.048 },
                {"LC20", 0.048 },
                //////////////BACK
                 {"LC11", 0.036 },
                 {"HT-LC11", 0.048 },
                 {"HB-LC11", 0.048 },
                 {"HC-LC11", 0.048 },
                 {"M-LC11", 0.048 },
                 {"WC11", 0.048 },
                 {"LC1220",  0.06},
                {"VAC-LC11", 0.048 },
                {"SC41", 0.048},
                {"LC33", 0.048 },
                {"A-LC11", 0.048 },
                {"LC21", 0.048 },
                {"LC1150", 0.048 },
                 ///TOP
                 {"LC1230", 0.036 },
                 { "H-LC1230", 0.060 },
                 {"NM-LC1230", 0.06 },
                 {"M-LC1230", 0.048 },
                 {"MNM-LC1230", 0.048 },
                 {"WC12", 0.048 },
                {"FH-LC1230", 0.06 },
                {"VACTC-LC1240", 0.048 },
                {"FOHM-SC4230", 0.048  },
                {"SC4230", 0.048},
                {"O-SC4230", 0.048},
                 {"WC3260", 0.048},
                ////TOE
                 {"LC1260", 0.036 },
                 { "H-LC1260", 0.048 } ,
                 {"NM-LC1260", 0.048 },
                {"LC335", 0.048},
                {"VAC-LC1260", 0.048},
                {"SC4260", 0.048},
                 ///SHELF
                {"LC1630",  0.036 },
                { "NM-LC1630",  0.036 },
                {"WC16", 0.036 },
                {"VACB-LC1630", 0.048 },
                {"SC4630", 0.048 },
                 {"SC4631", 0.048 },
                {"LC3630", 0.06},
                ///REMOVABLEBACK
                {"LC17", 0.048 },
                ///BOTTOM
                {"LC1250", 0.036 },
                {"H-LC1250", 0.048 },
                {"M-LC1250", 0.048 },
                {"MNM-LC1250", 0.048 },
                {"LC334", 0.048},
                {"SC-LC33", 0.048},
                {"VAC-LC1250", 0.048},
                {"SC4250", 0.048},
                {"A-SC4250", 0.048 },
                {"LC2250", 0.048 },
                ///HIDDEN
                {"LC1280", 0.036 },
                { "NM-LC1280", 0.060 },
                ///PARTITION
                {"LC1400", 0.048 },
                {"HNM-LC1400", 0.048 },
                {"H-LC1400", 0.048 },
                ///RUNNER
                {"LC13", 0.048 },
                {"LC13-in", 0.048 },
                {"LC13-out", 0.048 },
                {"NM-LC13", 0.048 },
                {"NM-LC13-in", 0.048 },
                {"NM-LC13-out", 0.048 },
                {"VAC-LC13", 0.06},
                ///STILESIDE
                {"WC3150", 0.048 },
                {"NM-WC3150", 0.048 },
                {"BNM-WC3150", 0.048 },
                {"SC4150", 0.048},
                {"Z-SC4150", 0.048},
                ///STILETOP
                 {"WC3210", 0.048 },
                
                ///STILEBOTTOM
                {"WC3130", 0.048 },

                ///FRONT
                {"LC1240", 0.048 },
                {"VAC-LC1240", 0.048 },
                {"H-LC1240", 0.048 },
                ///GUSSET
                {"LC1270", 0.104 },
                { "LC1270G", 0.104 },
                  { "LC1270H", 0.104 },

                ///SECURITY
                {"LC1460", 0.036},
                ///DOOR
               
                {"LC15-I-Z", 0.036 },
                {"LC15-I-H", 0.036 },
                {"LC15-O-H", 0.048 },
                {"LC15-O-Z", 0.048 },
                {"LC35I", 0.036 },
                {"LC35O", 0.048 },
                {"LC332RSCHCSS", 0.036 },
                {"WC35O", 0.048 },
                {"SC35O", 0.048},
                {"WC3460", 0.036},
                {"WC3490",0.036 },
                {"WC3491",0.036 },
                {"WC3461",0.036 },
                {"SC4180", 0.036 },
                ///
                {"DRBU", 0.036 },
                {"DRH_IN", 0.036 },
                { "DRH_FO", 0.036 },















                { "H-LC13-out", 0.048 },{"H-LC13", 0.048 },

                {"LC1470EFC18", 0.074 },
                {"LC1470EFCHNM18", 0.074 },
                {"LC1470EFCH18", 0.074 },
                {"LC1470ERC18", 0.074 },
               
                
                
                
                
                
               
                
                
            //     else if (cat == "WC30")
            //{
            //    aw = 26.439 + (h - 25);
            //    al = 16.898 + (d - 13);
            //}
            //else if (cat == "WC11")
            //{
            //    aw = 26.439 + (h - 25);
            //    al = 16.898 + (d - 13);
            //}

            //else if (cat == "WC12")
            //{
            //    aw = 26.439 + (h - 25);
            //    al = 16.898 + (d - 13);
            //}
            //else if (cat == "WC16")
            //{
            //    aw = 26.439 + (h - 25);
            //    al = 16.898 + (d - 13);
            //}/////下面的是瞎编的











            { "GCB_S2", 0.048 },
                {"GCB_FR", 0.048 },
                

                //////下面的都是瞎编的
               
                
                  
                  
                {"BP", 0.036 },

            //    else if (cat == "WC3150")
            //{
            //    aw = 5;
            //    al = 23.310 + (h - 25);
            //}
            //else if (cat == "WC3210")
            //{
            //    aw = 5.44;
            //    al = 11.88 + (w - 12);
            //}
            //else if (cat == "WC3130")
            //{
            //    aw = 13.225 + (w - 12);
            //    al = 13.852 /*+ (d - 13)*/;
            //}
            //else if (cat == "NM-WC3150")
            //{
            //    aw = 23.875 + (h-25);
            //    al = 4.261 /*+ (d - 13)*/;
            //}
            //else if (cat == "BNM-WC3150")
            //{
            //    aw = 23.875 + (h - 25);
            //    al = 4.261 /*+ (d - 13)*/;
            //}
    //            
    //<add key="" value="0"/>
    //<add key="" value="0"/>
    //<add key="" value="0"/>
    //<add key="" value="0"/>
    //<add key="" value="0"/>
    //<add key="" value="0"/>
    //<add key="" value="0"/>
    //<add key="" value="0"/>
    //<add key="" value="0"/>
    //<add key="" value="0"/>
    //<add key="" value="0"/>

            /*{"NM-LC10", 0.048 }, */
               
                 {"LS11", 0.036 },
                  {"LS1230", 0.036 },{"H-LS1230", 0.036 },


            };
            return dic[cat];
        }
        public static List<double> matcost(string partname, int machineamadaornot)
        {

            Dictionary<double, double> dic = new Dictionary<double, double>()
            {
                {1, 0.5 },
                {2, 1.5 },
                {3, 0.75 }
            };
            List<double> output = new List<double>();
            try
            {

                List<string> cathwd = nametocathwd(partname);
                string cat = cathwd[0];
                int w = 0;
                int h = 0;
                int d = 0;
                try
                {
                    w = Convert.ToInt32(cathwd[1]);
                    h = Convert.ToInt32(cathwd[2]);
                    d = Convert.ToInt32(cathwd[3]);
                }
                catch { }

                double gau = matgauge(cat);
                double ind = matindexCSG(partname);





                List<double> tem1 = selectsheet(cat, h, w, d, Convert.ToInt32(ind), gau, machineamadaornot);


                List<double> tem2 = actualsizeWXL(cat, h, w, d, machineamadaornot);




                if (machineamadaornot == 0)
                {

                }
                else
                {
                    if ((cat == "LC1270") || (cat == "LC1470EFC18") || (cat == "LC1470EFCHNM18") || (cat == "LC1470EFCH18") || (cat == "LC1470ERC18") || (cat == "LC1270G"))
                    {

                    }
                    else
                    {
                        tem1[1] += 0.7;
                    }
                }


                double cost = tem1[0] * tem1[1] * gau * dic[ind] * 0.324 / tem1[2];

                output.Add(cost);
                output.AddRange(tem1);
                output.AddRange(tem2);
                output.Add(ind);
                output.Add(gau);

            }
            catch
            {
                output.Add(0);
            }

            return output;
        }
    }
}
