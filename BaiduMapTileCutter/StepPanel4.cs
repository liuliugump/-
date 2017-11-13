using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiduMapTileCutter
{   
    /*
     *  坐标配准（非常重要）
     */ 
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class StepPanel4 : UserControl
    {
        public StepPanel4()
        {
            InitializeComponent();
        }

        public void LoadPage()
        {
            string htmlString = "<!DOCTYPE html>"
                + "<html>"
                + "<head>"
                + "<meta charset=\"utf-8\">"
                + "<title>Get Point</title>"
                + "<script type=\"text/javascript\" src=\"http://webapi.amap.com/maps?v=1.4.0&key=6583e8ddd4f2fcc57e487cd43ab6da5a\" ></script>"
                + "<style>"
                + "html, body {margin: 0;padding: 0;}"
                + "</style>"
                + "</head>"
                + "<body>"
                + "<div id=\"container\" style=\"width:492px;height:236px;background:#000;\"></div>"
                + "<script type=\"text/javascript\">"
                + "var map = new AMap.Map('container',{"
                + "resizeEnable: 0,"
                + "zoom: 21,"
                + "center: [104.147257, 30.674912]"
                + "});"
                + "map.on('click', function(e) {"
                + "window.external.SetCenter(e.lnglat.getLng(),e.lnglat.getLat());"
                + " });"
                + "</script>"
                + "</body>"
                + "</html>";
            webviewMap.DocumentText = htmlString;
            webviewMap.ObjectForScripting = this;
        }

        public void SetCenter(string lng, string lat)
        {
            tbxLng.Text = lng;
            tbxLat.Text = lat;
        }

        public LatLng getCenter()
        {
            float lat = Convert.ToSingle(tbxLat.Text);
            float lng = Convert.ToSingle(tbxLng.Text);
            return new LatLng(lat, lng);
        }
    }
}
