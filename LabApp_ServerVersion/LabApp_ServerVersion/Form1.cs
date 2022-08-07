using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabApp_ServerVersion
{
    public partial class Form1 : Form
    {
        SWFunction swfunction = new SWFunction();
        partIndexes partindex = null;
        Database database = new Database();
        System.Timers.Timer timer;
        public Form1()
        {
            InitializeComponent();
            //swfunction.test1();
            //swfunction.test();
            //swfunction.BuildSubAss_PullOutShelf(21, "SP-LC1630", "SP-LC1630-21");
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += searchanddo;
            timer.Start();

            //searchanddo();
        }

        public void action(string myname, string type, string detailinfo, List<string> users)
        {
            Dictionary<string, string> dic_Style = new Dictionary<string, string>()
            { {"IN", "PN"}, {"IN & SS", "SN" }, {"FO", "PH"}, {"FO & SS", "SH"}, {"FO & WOOD", "WH" } };
            Dictionary<bool, string> booltostring = new Dictionary<bool, string>()
            { {true, "True"}, {false, "False" } };
            Dictionary<string, string> cabModelSwitch = new Dictionary<string, string>()
            { {"B", "Base"}, {"G", "Glide" }, {"C", "Suspended" },
              {"S", "Storage" }, {"M", "Mobile" },  {"W", "Wall" }, {"SB", "Special Base" } };
            Dictionary<int, bool> doorPair = new Dictionary<int, bool>() { { 0, false }, { 1, false }, { 2, true }, { 4, true } };
            Dictionary<string, int> doorDirection = new Dictionary<string, int>() { { "L", 1 }, { "R", 2 } };
            bool boolstatus = false;
            string[] typeArray = type.Split('-');
            if (typeArray[0] == "part")
            {
                string[] detailArray = detailinfo.Split(';');
                boolstatus = swfunction.BuildPart_Shell(detailArray[0], Convert.ToInt16(detailArray[1]), Convert.ToInt16(detailArray[2]), Convert.ToInt16(detailArray[3]), detailArray[4]);
            }
            else if (typeArray[0] == "subassembly")
            {
                if (typeArray[1] == "shell")
                {
                    string[] detailArray = detailinfo.Split(';');
                    int w = Convert.ToInt16(detailArray[0]); int h = Convert.ToInt16(detailArray[1]); int d = Convert.ToInt16(detailArray[2]);
                    List<string> shellBomList = new List<string>();
                    string[] shellbomArray = detailArray[3].Split('_');
                    List<string> partClassList = new List<string>();
                    string[] partlistArray = detailArray[4].Split('_');
                    string shellName = detailArray[5];
                    List<bool> toespace = new List<bool>();
                    string[] toeArray = detailArray[6].Split('_');
                    List<bool> uptoespace = new List<bool>();
                    string[] uptoeArray = detailArray[7].Split('_');
                    string model = detailArray[8];
                    string cstyle = detailArray[9];
                    string metalTop = "M";
                    for (int i = 0; i < shellbomArray.Count(); i++)
                    {
                        if (shellbomArray[i] != "")
                        {
                            shellBomList.Add(shellbomArray[i]);
                            partClassList.Add(partlistArray[i]);
                            toespace.Add(Convert.ToBoolean(toeArray[i]));
                            uptoespace.Add(Convert.ToBoolean(uptoeArray[i]));
                        }
                    }

                    boolstatus = swfunction.BuildSubAss_Shell(w, h, d, shellBomList, partClassList, shellName, toespace, uptoespace, model, cstyle, metalTop, "", "");
                }
                else if (typeArray[1] == "drawer")
                {
                    string[] detailArray = detailinfo.Split(';');
                    int drawerWidth = Convert.ToInt16(detailArray[0]);
                    int drawerHeight = Convert.ToInt16(detailArray[1]);
                    string drawerHeadName = detailArray[2];
                    string drawerStyle = detailArray[3];
                    string drawerBodyName = detailArray[4];
                    string drawerTrackName = detailArray[5];
                    string pullP = detailArray[6];
                    int lockN = Convert.ToInt16(detailArray[7]);
                    int chN = Convert.ToInt16(detailArray[8]);
                    string drawerName = detailArray[9];

                    boolstatus = swfunction.BuildSubAss_Drawer(drawerWidth, drawerHeight, drawerHeadName, drawerStyle, drawerBodyName, drawerTrackName, pullP,
                        lockN, chN, drawerName);
                }
                else if (typeArray[1] == "door")
                {
                    
                    string[] detailInfo = detailinfo.Split('_');
                    string doorMaster = detailInfo[0];
                    double doorWidth = Convert.ToDouble(detailInfo[1]);
                    double doorHight = Convert.ToDouble(detailInfo[2]);
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

                    List<string> doorList = DoorParts.doorList_New_3D(doorMaster, doorDirection[detailInfo[3]], doorWidth, doorHight, 0, 0, detailInfo[4], "");
                    boolstatus = swfunction.BuildSubAss_Door(doorList, doorMaster);
                }
            }
            else if (typeArray[0] == "assembly")
            {
                if (typeArray[1] == "cabinet")
                {
                    //B_ _PN_M_30_29_22_0,6,12,18_0_NSC-
                    //0,0,15,6,1,0,100,1_0,6,15,6,0,0,100,1_0,12,15,6,0,0,100,1_0,18,15,6,0,0,100,1_15,0,15,6,1,0,100,1-
                    //2,1,15,6,15,18,0,0,NSC,1-
                    //P1_H1_T1_A_ _Unit
                    string[] detailArray = detailinfo.Split('-');
                    string[] detail1 = detailArray[0].Split('_');
                    string[] detail2 = detailArray[1].Split('_');
                    string[] detail3 = detailArray[2].Split('_');
                    string[] detail4 = detailArray[3].Split('_');
                    if (detail1[1] == " ")
                    {
                        detail1[1] = "-";
                    }
                    string model = cabModelSwitch[detail1[0]];
                    int w = Convert.ToInt16(detail1[4]);
                    int h = Convert.ToInt16(detail1[5]);
                    int d = Convert.ToInt16(detail1[6]);
                    string[] pullOutShelfStrArray = detail1[7].Split(',');
                    string lockstring = detail4[3];
                    string trackpro = detail4[2];
                    string pullStyle = detail4[0];
                    string hingeStyle = detail4[1];
                    string cardHolderString = detail4[4];

                    string shellKey = "";
                    string drawerKey = "";
                    string doorKey = "";
                    string doorKeyS = "";
                    string doorKeyH = "";
                    string confKey = "";
                    string pulloutShelfKey = "";
                    List<string> otherhead = new List<string>();
                    List<string> othersKey = new List<string>();
                    string sql1 = "select * from partclass";
                    DataTable dt = database.getMySqlDatatable_rd1(sql1);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["subass"].ToString() == "Others")
                        {
                            otherhead.Add(dr["name"].ToString());
                        }
                    }
                    database.CloseConnection_rd1();
                    sql1 = $"select * from condition_cabinet where model = '{model}' and cstyle = '{detail1[2]}' and " +
                        $"specialdesign = '{detail1[1]}'";
                    MySqlDataReader rd = database.getMySqlReader_rd1(sql1);
                    if (rd.Read())
                    {
                        
                        shellKey = rd.GetString("shell");
                        if (rd.GetString("drawer") != "")
                        {
                            drawerKey = dic_Style[rd.GetString("drawer")];
                        }
                        if (rd.GetString("doors") != "")
                        {
                            doorKeyS = rd.GetString("doors");
                        }
                        if (rd.GetString("doorh") != "")
                        {
                            doorKeyH = rd.GetString("doorh");
                        }
                        if (rd.GetString("conf") != "")
                        {
                            confKey = rd.GetString("conf");
                        }
                        for (int i = 0; i < otherhead.Count; i++)
                        {

                            othersKey.Add(rd.GetString(otherhead[i]));
                        }
                    }
                    database.CloseConnection_rd1();

                    string shellname = shellKey + "_" + w.ToString() + "_" + h.ToString() + "_" + d.ToString();

                    List<int> drawerx = new List<int>();
                    List<int> drawery = new List<int>();
                    List<int> drawerw = new List<int>();
                    List<int> drawerh = new List<int>();
                    List<int> drawerlock = new List<int>();
                    List<int> drawerch = new List<int>();
                    List<int> drawerload = new List<int>();
                    List<int> drawerface = new List<int>();
                    List<string> drawername = new List<string>();
                    List<string> drawerHead = new List<string>();
                    List<string> drawerBody = new List<string>();
                    List<string> drawerTrack = new List<string>();
                    for (int i = 0; i < detail2.Count(); i++)
                    {
                        //0,18,15,6,0,0,100,1
                        if (detail2[i] != "")
                        {
                            string[] temp = detail2[i].Split(',');
                            drawerx.Add(Convert.ToInt16(temp[0]));
                            drawery.Add(Convert.ToInt16(temp[1]));
                            drawerw.Add(Convert.ToInt16(temp[2]));
                            drawerh.Add(Convert.ToInt16(temp[3]));
                            drawerlock.Add(Convert.ToInt16(temp[4]));
                            drawerch.Add(Convert.ToInt16(temp[5]));
                            drawerload.Add(Convert.ToInt16(temp[6]));
                            drawerface.Add(Convert.ToInt16(temp[7]));
                            string trackname = DrawerParts.trackname(model, trackpro, Convert.ToInt16(temp[6]), 1, drawerKey, detail1[1]);
                            drawername.Add(DrawerParts.drawerName_New(Convert.ToInt16(temp[3]), Convert.ToInt16(temp[2]), drawerKey, trackname,
                                pullStyle, Convert.ToInt16(temp[4]), Convert.ToInt16(temp[5])));
                            drawerHead.Add(DrawerParts.drawerheadname(drawerKey, Convert.ToInt16(temp[3]), Convert.ToInt16(temp[2]), pullStyle,
                                Convert.ToInt16(temp[4]), Convert.ToInt16(temp[5]), detail1[1], temp[6]));
                            drawerTrack.Add(trackname);
                            drawerBody.Add(DrawerParts.drawerBodyName_New(Convert.ToInt16(temp[2]), Convert.ToInt16(temp[3]), drawerKey, trackname));
                        }
                    }

                    List<int> doorconf = new List<int>();
                    List<int> doormat = new List<int>();
                    List<int> doorx = new List<int>();
                    List<int> doory = new List<int>();
                    List<int> doorw = new List<int>();
                    List<int> doorh = new List<int>();
                    List<int> doorlock = new List<int>();
                    List<int> doorch = new List<int>();
                    List<string> doorselfclose = new List<string>();
                    List<int> doorFace = new List<int>();
                    List<string> doorName = new List<string>();
                    
                    for (int i = 0; i < detail3.Count(); i++)
                    {
                        //2,1,15,6,15,18,0,0,NSC,1
                        if (detail3[i] != "")
                        {
                            string[] temp = detail3[i].Split(',');
                            doorconf.Add(Convert.ToInt16(temp[0]));
                            doormat.Add(Convert.ToInt16(temp[1]));
                            doorx.Add(Convert.ToInt16(temp[2]));
                            doory.Add(Convert.ToInt16(temp[3]));
                            doorw.Add(Convert.ToInt16(temp[4]));
                            doorh.Add(Convert.ToInt16(temp[5]));
                            doorlock.Add(Convert.ToInt16(temp[6]));
                            doorch.Add(Convert.ToInt16(temp[7]));
                            doorselfclose.Add(temp[8]);
                            doorFace.Add(Convert.ToInt16(temp[9]));
                            if (detail3.Count() >= 2)
                            {
                                doorKey = doorKeyH;
                            }
                            else
                            {
                                doorKey = doorKeyS;
                            }
                            List<string> doorList = DoorParts.doorList_New_3D(doorKey, Convert.ToInt16(temp[0]), Convert.ToInt16(temp[4]), Convert.ToInt16(temp[5]),
                                 Convert.ToInt16(temp[6]), Convert.ToInt16(temp[7]), pullStyle, hingeStyle);
                            doorName.Add(doorList[11]);
                        }
                    }
                    bool pairOf = doorPair[doorconf.Count()];

                    List<List<int>> hidrailindex = ShellParts.hidrailindex(doorh, doorx, doorw, h, drawerw, drawery, model, w, drawerlock, drawerx, drawerh);
                    List<List<int>> centerpoindex = ShellParts.centerpoindex(hidrailindex, h, drawery, drawerw, w, model);
                    int partitionindex = ShellParts.partitionindex(model, doorx, doory, w);
                    string locklist = "";
                    for (int i = 0; i < drawerlock.Count; i++)
                    {
                        if (drawerface[i] == 1)
                        {
                            locklist += drawerlock[i].ToString() + "_";
                        }
                    }
                    for (int i = 0; i < doorlock.Count; i++)
                    {
                        locklist += doorlock[i].ToString() + "_";
                    }
                    if (locklist != "")
                    {
                        locklist = locklist.Substring(0, locklist.Count() - 1);
                    }
                    List<List<int>> securitypanelindex = ShellParts.securitypanelindex(locklist, doorx, drawery, w, drawerx, drawerw);
                    string flx = "";
                    for (int i = 0; i < hidrailindex.Count; i++)
                    {
                        flx += hidrailindex[i][0] + "_" + hidrailindex[i][1] + "_" + hidrailindex[i][2];
                        if (i == hidrailindex.Count - 1)
                        {
                            flx += "+";
                        }
                        else
                        {
                            flx += "-";
                        }
                    }
                    for (int i = 0; i < centerpoindex.Count; i++)
                    {
                        flx += centerpoindex[i][0] + "_" + centerpoindex[i][1];
                        if (i == centerpoindex.Count - 1)
                        {
                            flx += "+";
                        }
                        else
                        {
                            flx += "-";
                        }
                    }
                    if (partitionindex != -1)
                    {
                        flx += partitionindex + "+";
                    }
                    for (int i = 0; i < securitypanelindex.Count; i++)
                    {
                        flx += securitypanelindex[i][0] + "_" + securitypanelindex[i][1] + "_" + securitypanelindex[i][2];
                        if (i == securitypanelindex.Count - 1)
                        {
                            flx += "+";
                        }
                        else
                        {
                            flx += "-";
                        }
                    }
                    string confName = "";
                    if (flx != "")
                    {
                        confName = confKey + "_" + w.ToString() + "_" + h.ToString() + "_" + d.ToString() + "-" + flx;
                    }
                    if (lockstring == " ")
                    {
                        lockstring = "";
                    }
                    if (cardHolderString == " ")
                    {
                        cardHolderString = "";
                    }
                    string cabname = CabinetOverview.formalCabName(model, detail1[2], detail1[1], "M", w, h, d, 
                        drawerw, drawerh, drawerx, doorx, doory, doorFace, doorconf, doormat, pullStyle, hingeStyle, trackpro, lockstring,
                        cardHolderString, "NSC", pullOutShelfStrArray[0]);
                    string pull = detail4[0];
                    //string drnamestring = detailArray[7];

                    //string[] drx = detailArray[8].Split('|');
                    //string[] dry = detailArray[9].Split('|');
                    //string drawerStyle = detailArray[10];
                    //string[] drw = detailArray[11].Split('|');
                    //string[] drlock = detailArray[12].Split('|');
                    //string[] drh = detailArray[13].Split('|');
                    //string[] drheadname = detailArray[14].Split('|');
                    //string[] drbdname = detailArray[15].Split('|');
                    //string[] drtrack = detailArray[16].Split('|');
                    //string[] drload = detailArray[17].Split('|');

                    //for (int i = 0; i < drx.Count(); i++)
                    //{
                    //    if (drx[i] != "")
                    //    {
                    //        drawerx.Add(Convert.ToInt16(drx[i]));
                    //        drawery.Add(Convert.ToInt16(dry[i]));
                    //        drawerw.Add(Convert.ToInt16(drw[i]));
                    //        drawerlock.Add(Convert.ToInt16(drlock[i]));
                    //        drawerHeadName.Add(drheadname[i]);
                    //        drawerBodyName.Add(drbdname[i]);
                    //        drawerTrackName.Add(drtrack[i]);
                    //        drawerh.Add(Convert.ToInt16(drh[i]));
                    //        drawerload.Add(Convert.ToInt16(drload[i]));
                    //    }
                    //}

                    //string[] donamestring = detailArray[17 + 1].Split('|');
                    //string[] dox = detailArray[18 + 1].Split('|');
                    //string[] doy = detailArray[19 + 1].Split('|');
                    //string[] doconf = detailArray[20 + 1].Split('|');
                    //string doorStyle = detailArray[21 + 1];
                    //string[] doh = detailArray[22 + 1].Split('|');
                    //string[] dow = detailArray[23 + 1].Split('|');
                    //string[] dolock = detailArray[24 + 1].Split('|');
                    //bool pairof = Convert.ToBoolean(detailArray[25 + 1]);
                    //string hinge = detailArray[26 + 1];
                    //List<string> doorName = new List<string>();


                    //for (int i = 0; i < donamestring.Count(); i++)
                    //{
                    //    if (donamestring[i] != "")
                    //    {
                    //        doorName.Add(donamestring[i]);
                    //        doorx.Add(Convert.ToInt32(dox[i]));
                    //        doory.Add(Convert.ToInt32(doy[i]));
                    //        doorconf.Add(Convert.ToInt32(doconf[i]));
                    //        doorh.Add(Convert.ToInt32(doh[i]));
                    //        doorw.Add(Convert.ToInt32(dow[i]));
                    //        doorlock.Add(Convert.ToInt32(dolock[i]));
                    //    }
                    //}


                    //string[] otherArray = detailArray[31].Split('|');
                    //List<string> otherList = otherArray.ToList();

                    boolstatus = swfunction.BuildAss_Cabinet(w, h, d, shellname, confName, cabname,
                    new List<List<string>>(), pull, detail4[2], drawername, drawerx, drawery, drawerKey, drawerw, drawerlock, drawerh, drawerHead, drawerBody,
                    drawerTrack, drawerload, doorName, doorx, doory, doorconf, doorKey, doorh, doorw, doorlock, pairOf, hingeStyle, lockstring, detail1[7], othersKey);
                }
            }
            else if (typeArray[0] == "CW")
            {
                swfunction.counterWeightInfo(detailinfo, myname);
                boolstatus = true;
            }
            else if (typeArray[0] == "UPDATE")
            {
                string partMastername = myname;
                string partClass = "";
                string sql1 = "select * from masterpart_shell where parthead = '" + partMastername + "'";
                MySqlDataReader rd = database.getMySqlReader_rd1(sql1);
                if (rd.Read())
                {
                    partClass = rd.GetString("partclass");
                }
                database.CloseConnection_rd1();
                swfunction.updatePart_Shell(partClass, partMastername);
                if (typeArray[1] == "DRAWING")
                {
                    swfunction.updateDrawing_Shell(partClass, partMastername);
                }
            }
            else if (typeArray[0] == "PARTINDEX_NEW")
            {
                partindex = new partIndexes();
                string[] details = detailinfo.Split('|');
                partindex.newPart(details[0], details[1], details[2], details[3]);
            }
            else if (typeArray[0] == "PARTINDEX_UPDATE")
            {
                partindex = new partIndexes();
                string[] details = detailinfo.Split('-');
                partindex.updatePart(details[0], details[1], details[2], details[3]);
            }
            else if (typeArray[0] == "BUILD_SINGLE")
            {
                string[] detailArray = detailinfo.Split(';');
                //swfunction.activeTempo();
                bool status = swfunction.buildSingle(detailArray[0], detailArray[1], detailArray[2], Convert.ToDouble(detailArray[3]),
                    Convert.ToDouble(detailArray[4]), Convert.ToDouble(detailArray[5]), detailArray[6], detailArray[7]);
            }
            //if (boolstatus == false)
            //{
            //    string sql = "";
            //    for (int i = 0; i < users.Count; i++)
            //    {
            //        sql = $"insert into errorlist(myname, type, detailinfo, users) valuse('{myname}', '{type}', '{detailinfo}', '{users[i]}')";
            //        database.mySqlExecuteQuery_rd2(sql);
            //        database.CloseConnection_rd2();
            //    }
            //    sql = "set sql_safe_updates = 0; delete from waitlist where myname = '" + myname + "' and type = '" + type + "';";
            //    database.mySqlExecuteQuery_rd2(sql);
            //    database.CloseConnection_rd2();
            //}
            //else
            //{
            string sql = "set sql_safe_updates = 0; delete from waitlist where myname = '" + myname + "' and type = '" + type + "';";
            database.mySqlExecuteQuery_rd2(sql);
            database.CloseConnection_rd2();
            //}

        }
        public void searchanddo(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            DataTable dt = database.getMySqlDatatable_rd1("select * from waitlist group by myname");
            List<string> myname = new List<string>();
            List<string> type = new List<string>();
            List<string> detail = new List<string>();
            List<List<string>> users = new List<List<string>>();
            foreach (DataRow dr in dt.Rows)
            {
                myname.Add(dr["myname"].ToString());
                type.Add(dr["type"].ToString());
                detail.Add(dr["detailinfo"].ToString());
                List<string> user = new List<string>();
                DataTable dt1 = database.getMySqlDatatable_rd3("select * from waitlist where myname = '" + dr["myname"].ToString() + "'");
                foreach(DataRow dr1 in dt1.Rows)
                {
                    user.Add(dr1["users"].ToString());
                }
                database.CloseConnection_rd3();
                users.Add(user);
            }
            database.CloseConnection_rd1();
            if (myname.Count>0)
            {
                
                for (int i = 0; i < myname.Count; i++)
                {

                    
                    action(myname[i], type[i], detail[i], users[i]);
                }
                timer.Start();
            }
            else
            {
                timer.Start();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
