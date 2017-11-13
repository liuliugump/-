using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiduMapTileCutter
{
    // 输出文件类型的枚举
    public enum OutputFileTypes : byte
    {
        TileAndCode, //切片和代码
        TileOnly  //仅切片
    }

    // 独立地图类型还是叠加图层
    public enum OutputLayerTypes : byte
    {
        MapType,
        NormalLayer
    }

    // 级别信息结构体
    public struct ZoomInfo
    {
        public int MinZoom; //最大级别
        public int MaxZoom; //最小级别
        public int ImageZoom; //原图级别
    }

    public partial class mainForm : Form
    {
   
        // 当前执行的步骤
        private short currentStep = 0;
        private string imageFilePath = ""; //图片路径
        private string outputPath = ""; //输出路径
        private OutputFileTypes outputFileType = OutputFileTypes.TileAndCode;
        private LatLng center = new LatLng();
        private ZoomInfo zoomInfo;
        private OutputLayerTypes outputLayerType = OutputLayerTypes.MapType;
        private string mapTypeName = "MyMap";
        private UserControl[] stepPanels = new UserControl[7];
        //右侧步骤提示条
        //private Label[] Labelname = new Label[7] {label};

        private StepPanel1 stepPanel1 = new StepPanel1(); //第一步 选择图片路径
        private StepPanel2 stepPanel2 = new StepPanel2(); //第二步 设置输出目录
        private StepPanel3 stepPanel3 = new StepPanel3(); //第三步 选择输出类型
        private StepPanel4 stepPanel4 = new StepPanel4(); //第四步 坐标配置（最重要）
        private StepPanel5 stepPanel5 = new StepPanel5(); //第五步 切片级别设置
        private StepPanel6 stepPanel6 = new StepPanel6(); //第六步 图层设置
        private StepPanel7 stepPanel7 = new StepPanel7(); //第七步 开始切图

        public mainForm()
        {
            InitializeComponent();
        }

        //更新UI btnPrev-上一步按钮 btnNext-下一步按钮 
        private void updateUI()
        {      
            if (currentStep == 0)
            {
                btnPrev.Enabled = false;
            }
            else if (currentStep == 1)
            {
                btnPrev.Enabled = true;
            }

            if (currentStep == 3)
            {
                stepPanel4.LoadPage();
            }

            if (currentStep == 6)
            {
                btnNext.Text = "开始切图";
                stepPanel7.SetInfo(imageFilePath, outputPath, outputFileType, center, zoomInfo, outputLayerType, mapTypeName);
            }else{
                btnNext.Text = "下一步";
            }
            //点亮当前项的步骤条
            TypeCode labCode = Convert.GetTypeCode(label1);
    
            label1.ForeColor = Color.Coral;
             
            for (int i = 0; i < stepPanels.Length; i++)
            {
                if (i == currentStep)
                {
                    stepPanels[i].Visible = true;
                }
                else
                {
                    stepPanels[i].Visible = false;
                }
            }
        }

        //切片开始时锁定上、下一步
        private void OnCutStart(object sender, EventArgs e)
        {
            btnPrev.Enabled = false;
            btnNext.Enabled = false;
        }
        //切片完成时解锁上、下一步
        private void OnCutEnd(object sender, EventArgs e)
        {
            btnPrev.Enabled = true;
            btnNext.Enabled = true;
        }

        //右侧选项框加载
        private void mainForm_Load(object sender, EventArgs e)
        {
            stepPanels[0] = stepPanel1;
            stepPanels[1] = stepPanel2;
            stepPanels[2] = stepPanel3;
            stepPanels[3] = stepPanel4;
            stepPanels[4] = stepPanel5;
            stepPanels[5] = stepPanel6;
            stepPanels[6] = stepPanel7;

            stepPanel7.CutStart += new CutStartEventHandler(OnCutStart);
            stepPanel7.CutEnd += new CutEndEventHandler(OnCutEnd);

            for (int i = 0; i < stepPanels.Length; i++)
            {
                this.Controls.Add(stepPanels[i]);
                stepPanels[i].Location = new System.Drawing.Point(120, 3);
            }
            updateUI();
        }
        //下一步按钮点击事件，并判断错误
        private void btnNext_Click(object sender, EventArgs e)
        {
            switch (currentStep)
            {
                case 0:
                    // 检查图片路径是否有效
                    if (stepPanel1.GetImageFilePath() == "")
                    {
                        showErrorInfo("请选择图片");
                        return;
                    }
                    imageFilePath = stepPanel1.GetImageFilePath();
                    break;
                case 1:
                    if (stepPanel2.getOutputPath() == "")
                    {
                        showErrorInfo("请设置输出路径");
                        return;
                    }
                    outputPath = stepPanel2.getOutputPath();
                    break;
                case 2:
                    outputFileType = stepPanel3.getOutputFileType();
                    break;
                case 3:
                    center = stepPanel4.getCenter();
                    break;
                case 4:
                    stepPanel5.SetImagePath(imageFilePath);
                    if (stepPanel5.CheckZoomValidation() == false)
                    {
                        showErrorInfo("级别设置不正确，请检查");
                        return;
                    }
                    zoomInfo = stepPanel5.GetZoomInfo();
                    break;
                case 5:
                    outputLayerType = stepPanel6.GetOutputLayerType();
                    mapTypeName = stepPanel6.GetMapTypeName();
                    if (outputLayerType == OutputLayerTypes.MapType && mapTypeName == "")
                    {
                        showErrorInfo("请填写地图类型名称");
                        return;
                    }
                    break;
                case 6:
                    stepPanel7.StartCut();
                    break;
            }
            if (currentStep < 6)
            {
                currentStep++;
            }
            updateUI();
        }

        //错误信息模板
        private void showErrorInfo(string errorInfo)
        {
            MessageBoxButtons button = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBox.Show(errorInfo, "错误", button, icon);
        }

        //返回上一步
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentStep > 0)
            {
                currentStep--;
            }
            updateUI();
        }

        //作者点击按钮
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Simon Li制作");
        }
    }
}
