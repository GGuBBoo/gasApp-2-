using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Drawing;
using System.Windows.Controls;


namespace gasApp
{
    public partial class Form1 : Form
    {
        public Form1() 
        {
            InitializeComponent();

            pictureBox1.BackgroundImage = Properties.Resources.first_Icon;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox2.BackgroundImage = Properties.Resources.title;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox3.BackgroundImage = Properties.Resources.title1;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;

            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "value";

            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "value";

            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "value";

            addressBindingSource.Add(new address() { Name = "지역을 선택하세요" });
            addressBindingSource.Add(new address() { Name = "대구", value = "14" });
            addressBindingSource.Add(new address() { Name = "서울", value = "01" });

            address2BindingSource.Add(new address2() { Name = "제품을 선택하세요" });
            address2BindingSource.Add(new address2() { Name = "휘발유", value = "B027" });
            address2BindingSource.Add(new address2() { Name = "경유", value = "D047" });
            address2BindingSource.Add(new address2() { Name = "고급휘발유", value = "B034" });
            address2BindingSource.Add(new address2() { Name = "실내등유", value = "C004" });

        }



        string key = "F616190925"; //오피넷 전국 주유소 평균 가격
        string areaNum1; // 지역코드 (시)
        string areaNum2; // 지역코드 (구)
        string proNum; //제품코드

        string keyJP = "guest"; // 일본 API 키값 (guest 밖에 구하지 못했다)



        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox3.Visible = false;
            panel4.Visible = true;
            panel1.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
        }

        private void ButtonPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonPanel1.BackgroundImage = Properties.Resources.homeButton1;
            pictureBox3.Visible = false;
            panel4.Visible = true;
            panel1.Visible = false; 
            panel5.Visible = false;
            panel6.Visible = false;
        }

        private void ButtonPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            ButtonPanel1.BackgroundImage = Properties.Resources.homeButtonDown1;
        }

        private void ButtonPanel2_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonPanel2.BackgroundImage = Properties.Resources.DcmPriceButton;
            pictureBox3.Visible = true;
            panel1.Visible = true;
            panel5.Visible = false;
            panel6.Visible = false;
        }

        private void ButtonPanel2_MouseDown(object sender, MouseEventArgs e)
        {
            ButtonPanel2.BackgroundImage = Properties.Resources.DcmPriceButtonDown;
        }

        private void ButtonPanel3_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonPanel3.BackgroundImage = Properties.Resources.GasStationButton;
            panel5.Visible = true;
            panel1.Visible = false;
            panel4.Visible = false;
            panel6.Visible = false;
        }

        private void ButtonPanel3_MouseDown(object sender, MouseEventArgs e)
        {
            ButtonPanel3.BackgroundImage = Properties.Resources.GasStationButtonDown;
        }

       

        private void ButtonPanel4_MouseUp(object sender, MouseEventArgs e)
        {
            ButtonPanel4.BackgroundImage = Properties.Resources.PriceLogButton;
            panel6.Visible = true;
            panel1.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;

            PriceLogPictureBox.Visible = true;

            webBrowser1.Navigate("about:blank");

        }

        private void ButtonPanel4_MouseDown(object sender, MouseEventArgs e)
        {
            ButtonPanel4.BackgroundImage = Properties.Resources.PriceLogButtonDown;
        }


        private void SearchPanelButton_MouseUp(object sender, MouseEventArgs e)
        {
            SearchPanelButton.BackgroundImage = Properties.Resources.serchButton;
            //gasList1.Items.Clear();
            List<Gas> gass = gasManager.Search(key);
            gasList1.DataSource = gass;
        }

        private void SearchPanelButton_MouseDown(object sender, MouseEventArgs e)
        {
            SearchPanelButton.BackgroundImage = Properties.Resources.serchButtonDown;

        }

        private void StationSearchPanel_Mouse_Up(object sender, MouseEventArgs e)
        {
            StationSearchPanel.BackgroundImage = Properties.Resources.serchButton;
            try
            {
                List<GasSt> gasSt = gasStationManager.Search(key, areaNum1, areaNum2, proNum);
                gasStationlist.DataSource = gasSt;
                areaNum1 = comboBox1.SelectedValue.ToString();
                areaNum2 = comboBox2.SelectedValue.ToString();
                proNum = comboBox3.SelectedValue.ToString();
            }
            catch
            {
                MessageBox.Show("종류를 선택해주세요!");
            }
           
           
        }

        private void StationSearchPanel_Mouse_Down(object sender, MouseEventArgs e)
        {
           StationSearchPanel.BackgroundImage = Properties.Resources.serchButtonDown;
        }


        private void gasList1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Gas gas = gasList1.SelectedItem as Gas;
            TRADE_DT.Text = "일자 : " + gas.TRADE_DT; // 해당일자
            //PRODCD.Text = gas.PRODCD; //제품구분코드
            PRODNM.Text = gas.PRODNM; //제품명
            PRICE.Text = gas.PRICE + "원"; //가격
            DIFF.Text = "전일 대비" + gas.DIFF; //전일대비 등락값
        }

        private void gasStationlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            GasSt gasSt = gasStationlist.SelectedItem as GasSt;
            OS_NM.Text = gasSt.OS_NM; // 해당일자
            VAN_ADR.Text = gasSt.VAN_ADR; //위치
            stPRICE.Text = gasSt.STPRICE + "원" ; //가격
            POLL_DIV.Text = gasSt.POLL_DIV_CO; //

            string logoCheck = gasSt.POLL_DIV_CO; //이미지 체크용
            string ske = "SKE";
            string gsc = "GSC";
            string hdo = "HDO";
            string sol = "SOL";
            string rto = "RTO";
            string e1g = "E1G";
            string skg = "SKG";
            if (gsc.Equals(logoCheck))
            {
                pictureBox5.BackgroundImage = Properties.Resources.gsLogoCut;
            }
            else if (ske.Equals(logoCheck))
            {
                pictureBox5.BackgroundImage = Properties.Resources.sk1logoCut;
            }
            else if (hdo.Equals(logoCheck))
            {
                pictureBox5.BackgroundImage = Properties.Resources.hyunDaeLogoCut;
            }
            else if (sol.Equals(logoCheck))
            {
                pictureBox5.BackgroundImage = Properties.Resources.SoilCut;
            }
            else if (rto.Equals(logoCheck))
            {
                pictureBox5.BackgroundImage = Properties.Resources.alttleCut;
            }
            else if (e1g.Equals(logoCheck))
            {
                pictureBox5.BackgroundImage = Properties.Resources.e1logoCut;
            }
            else if (skg.Equals(logoCheck))
            {
                pictureBox5.BackgroundImage = Properties.Resources.sklogoCut;
            }

        }

        // 콤보박스 부분

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (comboBox1.SelectedIndex == 0)
            {
                comboBox2.ResetText();
            }


            else if (comboBox1.SelectedIndex == 1)
            {
                comboBox2.ResetText();
                address3BindingSource.Add(new address3() { Name = "지역을 선택하세요" });
                address3BindingSource.Add(new address3() { Name = "북구", value = "05" });
                address3BindingSource.Add(new address3() { Name = "수성구", value = "06" });
            }

            else if (comboBox1.SelectedIndex == 2)
            {
                comboBox2.ResetText();
                address3BindingSource.Add(new address3() { Name = "지역을 선택하세요" });
                address3BindingSource.Add(new address3() { Name = "강남구", value = "13" });
                address3BindingSource.Add(new address3() { Name = "중구", value = "02" });
            }
        }
        // 웹뷰 부분

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Text = webBrowser1.DocumentTitle;
        }

        private void ButtonBef_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void ButtonFor_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

       
        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Go_Click(sender, e);
            }
        }

        private void Go_Click(object sender, EventArgs e)
        {
            String url = "http://dlgusgh7870.iptime.org:8090/webpro3/addPage.jsp";
            webBrowser1.Navigate(url);

            PriceLogPictureBox.Visible = false;
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GasSt gasSt = gasStationlist.SelectedItem as GasSt;
            string  rinkAddr = gasSt.VAN_ADR; //링크 라벨용 주소값
            System.Diagnostics.Process.Start(string.Format("https://map.naver.com/v5/search/{0}" , rinkAddr));
        }
        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GasStJP gasStJP = listBox1.SelectedItem as GasStJP;
            string rinkAddr1 = gasStJP.JPAddress; //링크 라벨용 주소값
            System.Diagnostics.Process.Start(string.Format("https://www.google.co.kr/maps/place/{0}", rinkAddr1));
       
         }
       

        //일본 부분

        private void JpSearchPanel_Mouse_Up(object sender, MouseEventArgs e)
        {
            JpSearchPanel.BackgroundImage = Properties.Resources.searchPanelJp;
            List<GasStJP> gasStJP = gasStationManagerJP.Search(keyJP);
            listBox1.DataSource = gasStJP;


        }

        private void JpSearchPanel_Mouse_Down(object sender, MouseEventArgs e)
        {
            JpSearchPanel.BackgroundImage = Properties.Resources.searchPanelJpDown;
        }

        private void jpHomePanel_Mouse_Up(object sender, MouseEventArgs e)
        {
            jpHomePanel.BackgroundImage = Properties.Resources.homePanelJp;
            JpStPanel.Visible = false;
            priceJPpanel.Visible = false;
        }

        private void jpHomePanel_Mouse_Down(object sender, MouseEventArgs e)
        {
            jpHomePanel.BackgroundImage = Properties.Resources.homePanelJpDown;
        }

        //jpGasStPanel

        private void jpGasStPanel_Mouse_Up(object sender, MouseEventArgs e)
        {
            jpGasStPanel.BackgroundImage = Properties.Resources.gasStationJp;
            JpStPanel.Visible = true;
            priceJPpanel.Visible = false;
        }

        private void jpGasStPanel_Mouse_Down(object sender, MouseEventArgs e)
        {
            jpGasStPanel.BackgroundImage = Properties.Resources.gasStationJpDown;
        }

        //jpPriceLog

        private void jpPriceLog_Mouse_Up(object sender, MouseEventArgs e)
        {
            jpPriceLog.BackgroundImage = Properties.Resources.pricelogJp;
            priceJPpanel.Visible = true;
            JpStPanel.Visible = false;

            pictureBox9.Visible = true;
            webBrowser2.Navigate("about:blank");
        }

        private void jpPriceLog_Mouse_Down(object sender, MouseEventArgs e)
        {
            jpPriceLog.BackgroundImage = Properties.Resources.pricelogJpDown;
        }

        //goKoreaSt
        private void goKoreaSt_Mouse_Up(object sender, MouseEventArgs e)
        {
            goKoreaSt.BackgroundImage = Properties.Resources.goKoreaSt;
            panel7.Visible = false;
        }

        private void goKoreaSt_Mouse_Down(object sender, MouseEventArgs e)
        {
            goKoreaSt.BackgroundImage = Properties.Resources.goKoreaStDown;
        }

        //goJapanSt
        private void goJapanSt_Mouse_Up(object sender, MouseEventArgs e)
        {
            goJapanSt.BackgroundImage = Properties.Resources.goJpStation;
            panel7.Visible = true;
        }

        private void goJapanSt_Mouse_Down(object sender, MouseEventArgs e)
        {
            goJapanSt.BackgroundImage = Properties.Resources.goJpStationDown;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GasStJP gasStJP = listBox1.SelectedItem as GasStJP;
            Date.Text = "日 : " + gasStJP.Date; // 해당일자
            ShopName.Text = gasStJP.ShopName; ; //제품명
            JPPrice.Text = gasStJP.JPPrice + "円"; //가격
            JPAddress.Text = gasStJP.JPAddress; //전일대비 등락값
            BrandPanel.Text =  gasStJP.Brand; //전일대비 등락값
        }

        //웹뷰 일본
        private void webBrowser2_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Text = webBrowser2.DocumentTitle;
        }

        private void ButtonBfJP_Click(object sender, EventArgs e)
        {
            webBrowser2.GoBack();
        }

        private void ButtonFrJP_Click(object sender, EventArgs e)
        {
            webBrowser2.GoForward();
        }

        private void txtUlJP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Go_Click(sender, e);
            }
        }

        private void GoJP_Click(object sender, EventArgs e)
        {
            String url = "http://dlgusgh7870.iptime.org:8090/webpro3/addPage.jsp";
            webBrowser2.Navigate(url);

            pictureBox9.Visible = false;
        }
    }
    }

    internal class gasManager
    {
        internal static List<Gas> Search(string key)
        {
            List<Gas> gass = new List<Gas>();

            string queryurl = string.Format("http://www.opinet.co.kr/api/avgAllPrice.do?out=xml&code={0}", key);

            XmlDocument xdoc = new XmlDocument(); //Xml 문서 개체 생성
            xdoc.PreserveWhitespace = true; //원본의 공백 유지
            xdoc.Load(queryurl);            //Xml 문서 개체에 사이트 로딩

            XmlNode cnode = xdoc.SelectSingleNode("RESULT");//RESULT 요소 탐색
            XmlNodeList xnl = cnode.SelectNodes("OIL");//OIL 요소 목록 탐색


            foreach (XmlNode xn in xnl)
            {
                gass.Add(Gas.Parse(xn));
            }
            return gass;
        }
    }



    internal class Gas
    {
        internal string TRADE_DT
        {
            get;
            private set;
        }
        internal string PRODCD
        {
            get;
            private set;
        }
        internal string PRODNM
        {
            get;
            private set;
        }

        internal string PRICE
        {
            get;
            private set;
        }

        internal string DIFF
        {
            get;
            private set;
        }

        static readonly Gas gas = new Gas("", "", "", "", "");
        internal static Gas Parse(XmlNode xn)
        {
            string trade = xn.SelectSingleNode("TRADE_DT").InnerText;

            string prodcd = xn.SelectSingleNode("PRODCD").InnerText;

            string prodnm = xn.SelectSingleNode("PRODNM").InnerText;

            string price = xn.SelectSingleNode("PRICE").InnerText;

            string diff = xn.SelectSingleNode("DIFF").InnerText;

            return new Gas(trade, prodcd, prodnm, price, diff);
        }

        private Gas(string trade, string prodcd, string prodnm, string price, string diff)
        {
            TRADE_DT = trade;
            PRODCD = prodcd;
            PRODNM = prodnm;
            PRICE = price;
            DIFF = diff;
        }

        public override string ToString()
        {
            return PRODNM;
        }



    }


    //http://www.opinet.co.kr/api/lowTop10.do?out=xml&code=XXXXXX&prodcd=B027&area=0101&cnt=2
    internal class gasStationManager
    {
        internal static List<GasSt> Search(string key, string areaNum1, string areaNum2, string proNum)
        {
            List<GasSt> gasSt = new List<GasSt>();

            string queryurl = string.Format("http://www.opinet.co.kr/api/lowTop10.do?out=xml&code={0}&prodcd={3}&area={1}{2}&cnt=10", key, areaNum1, areaNum2, proNum);

            XmlDocument xdoc = new XmlDocument(); //Xml 문서 개체 생성
            xdoc.PreserveWhitespace = true; //원본의 공백 유지
            xdoc.Load(queryurl);            //Xml 문서 개체에 사이트 로딩

            XmlNode cnode = xdoc.SelectSingleNode("RESULT");//RESULT 요소 탐색
            XmlNodeList xnl = cnode.SelectNodes("OIL");//OIL 요소 목록 탐색


            foreach (XmlNode xn in xnl)
            {
                gasSt.Add(GasSt.Parse(xn));
            }
            return gasSt;
        }
    }



    internal class GasSt
    {
        internal string OS_NM
        {
            get;
            private set;
        }
        internal string VAN_ADR
        {
            get;
            private set;
        }
        internal string STPRICE
        {
            get;
            private set;
        }

        internal string POLL_DIV_CO
        {
            get;
            private set;
        }



        static readonly GasSt gasSt = new GasSt("", "", "", "");
        internal static GasSt Parse(XmlNode xn)
        {
            string osnm = xn.SelectSingleNode("OS_NM").InnerText;

            string vanadr = xn.SelectSingleNode("VAN_ADR").InnerText;

            string stprice = xn.SelectSingleNode("PRICE").InnerText;

            string polldivco = xn.SelectSingleNode("POLL_DIV_CO").InnerText;



            return new GasSt(osnm, vanadr, stprice, polldivco);
        }

        private GasSt(string osnm, string vanadr, string stprice, string polldivco)
        {
            OS_NM = osnm;
            VAN_ADR = vanadr;
            STPRICE = stprice;
            POLL_DIV_CO = polldivco;

        }

        public override string ToString()
        {
            return OS_NM;
        }



    }

internal class gasStationManagerJP
{
    internal static List<GasStJP> Search(string keyJP)
    {
        List<GasStJP> gasStJP = new List<GasStJP>();

        string queryurl = string.Format("http://api.gogo.gs/v1.1/?apid={0}", keyJP);

        XmlDocument xdoc = new XmlDocument(); //Xml 문서 개체 생성
        xdoc.PreserveWhitespace = true; //원본의 공백 유지
        xdoc.Load(queryurl);            //Xml 문서 개체에 사이트 로딩

        XmlNode cnode = xdoc.SelectSingleNode("PriceInfo");//RESULT 요소 탐색
        XmlNodeList xnl = cnode.SelectNodes("Item");//OIL 요소 목록 탐색


        foreach (XmlNode xn in xnl)
        {
            gasStJP.Add(GasStJP.Parse(xn));
        }
        return gasStJP;
    }
}



internal class GasStJP
{
    internal string Brand
    {
        get;
        private set;
    }
    internal string ShopName
    {
        get;
        private set;
    }
    internal string JPPrice
    {
        get;
        private set;
    }

    internal string JPAddress
    {
        get;
        private set;
    }

    internal string Date
    {
        get;
        private set;
    }



    static readonly GasStJP gasStJP = new GasStJP("", "", "", "","");
    internal static GasStJP Parse(XmlNode xn)
    {
        string brand = xn.SelectSingleNode("Brand").InnerText;

        string shopname = xn.SelectSingleNode("ShopName").InnerText;

        string jpprice = xn.SelectSingleNode("Price").InnerText;

        string jpaddress = xn.SelectSingleNode("Address").InnerText;

        string date = xn.SelectSingleNode("Date").InnerText;



        return new GasStJP(brand, shopname, jpprice, jpaddress, date);
    }

    private GasStJP(string brand, string shopname, string jpprice,string jpaddress, string date)
    {
        Brand = brand;
        ShopName = shopname;
        JPPrice = jpprice;
        JPAddress = jpaddress;
        Date = date;

    }

    public override string ToString()
    {
        return ShopName;
    }



}





