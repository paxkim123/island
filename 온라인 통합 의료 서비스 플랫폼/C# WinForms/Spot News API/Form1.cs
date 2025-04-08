using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Spot_News_API
{
    public partial class Form1 : Form
    {
        // REST API ���� ��� ���� (POST, GET, PUT, DELETE)
        enum Method
        {
            POST,
            GET,
            PUT,
            DELETE
        }
        enum LogTrace // �α� ����
        {
            L1,//���� �⺻���� ���� �α� (L1: ���� ���� �߿䵵)
            L2,//L1���� �߿��� �α�, ������ �� ���� ����
            L3,//L2���� �� �߿��� �α�, ���� �帧 Ȯ�� ����
            Warning,//�ɰ����� ������, �����ؾ� �� ���� (��: ���� ����)
            Error,//���α׷� ���� �� ���� �߻� (��: ���� �߻�, ������ ��û)
        }

        public Form1()
        {
            InitializeComponent();// UI �ʱ�ȭ
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // �޺��ڽ��� HTTP Method �׸� �߰�
            foreach (Method m in Enum.GetValues(typeof(Method)))
            {
                cboxMethod.Items.Add(m.ToString());
                cboxMethod.SelectedIndex = 0;// �⺻�� ���� (POST)
            }
        }
        // Method �޺��ڽ� ���� ���� �� ����
        private void cboxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            Method m = StringToEnum(cboxMethod.Text);
            //post�� put�� json�� ���� �� �����̹Ƿ� Body �ڽ���
            //�Է��� �� �ֵ��� true�� �����ǵ��� �ϰ� 
            //get�� delete�� �� ��°� �����̹Ƿ� Body �ڽ��� �Է��� �� ������
            //false�� �����ؾ� �Ѵ�.
            //C# ������ MySql�� ���������� �������� �ʰ� ��������Ʈ ������ �켱������ ������.
            //�� �� ���۵� �����ʹ� �������� DB�� �̵��Ͽ� ����ǰ� ���۵� �����ʹ� �����ϸ鿡 ǥ�õȴ�.
            // POST�� PUT�� Body �Է� ���, GET�� DELETE�� ��Ȱ��ȭ
            switch (m)
            {
                case Method.POST:
                    tboxBody.Enabled = true;
                    break;
                case Method.PUT:
                    tboxBody.Enabled = true;
                    break;
                case Method.GET:
                    tboxBody.Enabled = false;
                    break;
                case Method.DELETE:
                    tboxBody.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        // ���� ��ư Ŭ�� �� ����
        private async void btnSend_Click(object sender, EventArgs e)
        {
            Send(StringToEnum(cboxMethod.Text));// �޼��� ����
            // GET ��û�� ��� ����� ����Ʈ�� ǥ��
            if (cboxMethod.SelectedItem.ToString() == "GET")
            {
                await FetchDataToListBox();
            }
        }


        // ���ڿ��� enum���� ��ȯ (���� �� �α�)
        private Method StringToEnum(string m)
        {
            Method M;
            if(!Enum.TryParse<Method>(m, out M))
            {
                Log(LogTrace.Error, $"���⿡ ���� ���� ����� �� �����ϴ�.({m}");
            }
            return M;
        }
        // �α� ��� �Լ�
        private void Log(LogTrace l, string LogDesc)
        {
            this.Invoke(new Action(delegate ()
            {
                if (lboxLog.Items.Count >= 15) 
                {
                    lboxLog.Items.RemoveAt(14); // �α� 15�� �ʰ� �� ���� ������ �α� ����
                }
                DateTime d = DateTime.Now;
                string info = $"{d:yyyy-MM-dd HH:mm:ss.fff} [{l.ToString()}] {LogDesc}";

                lboxLog.Items.Insert(0, info); // �ֽ� �αװ� ���ʿ� �߰��ǵ��� ����
                lboxLog.TopIndex = 0; // �׻� �ֽ� �α׸� ǥ��
            }));
        }

        //new Action(delegate{...})�� �������ϸ� �����Ϸ��� �͸� �޼��� �Ǵ� ���ٽ��� ��������Ʈ ��ü�� ĸ��ȭ�Ѵ�.
        //���������δ� MulticastDelegate ����� Actoin ��ü �ν��Ͻ��� �����ȴ�.
        //�� �ν��Ͻ��� CLR ���� �Ҵ�Ǿ� Invoke() �޼��带 ȣ���Ѵ�.
        //�����ڸ� �ٸ� �ڵ�(������)�� ������ ����Ǵ� �߿� UI�� ���� �ؾ� �� ���� �����,
        //UI�� �Ѱ��� �� ó���ϰ� �޸� �ѱ�� ����� Invoke+Action �����̴�.

        // REST API ȣ�� �Լ�
        private async void Send(Method m)
        {
            string Url = tboxURL.Text;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                { 
                    HttpResponseMessage httpResponseMessage = null;
                    string sendingData = tboxBody.Text;// JSON �Է�
                    StringContent stringContent = new StringContent(sendingData, System.Text.Encoding.UTF8, "application/json");
                    // URL���� ID ���� �� ���� URL �и�
                    string Id = Url.Substring(Url.LastIndexOf("/") + 1).Trim();
                    string originalUrl = Url.Substring(0, Url.LastIndexOf("/"));
                    Console.WriteLine($"Original URL: {originalUrl}");// GET, PUT, DELETE�� ID�� �ʿ�
                    if (string.IsNullOrEmpty(Id) && m != Method.POST)
                    {
                        Log(LogTrace.Error, "Id�� �Է��ϼ���");
                        return;
                    }                   
                    switch (m) // ���� ��Ŀ� ���� API ȣ��
                    {
                        case Method.POST:
                            httpResponseMessage = await httpClient.PostAsync(Url, stringContent);
                            break;
                        case Method.PUT:
                            httpResponseMessage = await httpClient.PutAsync($"{originalUrl}/{Id}", stringContent);
                            break;
                        case Method.DELETE:
                            httpResponseMessage = await httpClient.DeleteAsync($"{originalUrl}/{Id}");
                            break;
                        case Method.GET:
                            httpResponseMessage = await httpClient.GetAsync($"{originalUrl}/{Id}");
                            break;
                        default: Log(LogTrace.Error, "�߸��� ��û Ÿ��");
                            break;
                    }

                    // ���� �α� ���
                    string mType = $"Method = {httpResponseMessage.RequestMessage.Method}, StatusCode = {httpResponseMessage.StatusCode.ToString()}";
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        String responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                        Log(LogTrace.L1, $"�����͸� ���������� �����Ͽ����ϴ�.");
                    }
                    else
                    {
                        Log(LogTrace.L1, $"��û�� �����Ͽ����ϴ�. �����ڵ�: {httpResponseMessage.StatusCode}");
                    }
                    Log(LogTrace.L1, $"{mType}");
                }
                catch(Exception ex)
                {
                    Log(LogTrace.Error, ex.Message);
                }
            }
        }
        // JSON ������ �ҷ����� (tboxBody�� ǥ�ÿ�)
        private async Task FetchDataAsync()
        {
            string url = tboxURL.Text; // URL �Է� �ʵ忡�� ��������
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                //���⼭ GetAsync()�� ���� ��û�� ������ ���� ������ ���� �ʾ������� ���α׷��� �ߴܵ��� �ʰ� �귯����
                string jsonData = await response.Content.ReadAsStringAsync();

                Console.WriteLine("���� ������: " + jsonData); // ����� �뵵
                //await ���� UI�� �ݿ�
                if (response.IsSuccessStatusCode)
                {
                    // JSON �����͸� ��ü�� ��ȯ (����: Dictionary ���)
                    var parsedData = JsonConvert.DeserializeObject<dynamic>(jsonData);

                    // UI�� JSON ������ ǥ��
                    tboxBody.Text = JsonConvert.SerializeObject(parsedData, Formatting.Indented);
                }
                else
                {
                    tboxBody.Text = "���� �߻�: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                tboxBody.Text = "���� �߻�: " + ex.Message;
            }
        }
        // JSON ������ �ҷ��ͼ� ����Ʈ�ڽ��� ǥ�� (GET �� ���)
        private async Task FetchDataToListBox()
        {
            string url = tboxURL.Text; // URL �Է� �ʵ忡�� ��������
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string jsonData = await response.Content.ReadAsStringAsync();

                Console.WriteLine("���� ������: " + jsonData); // ����� �뵵

                if (response.IsSuccessStatusCode)
                {
                    // JSON �����͸� ����Ʈ ���·� ��ȯ (����: ����Ʈ ������ ��ü��� ����)
                    var parsedData = JsonConvert.DeserializeObject<SpotNews>(jsonData);



                    // ListBox ���� �ʱ�ȭ
                    lboxLog.Items.Clear();

                    // ListBox�� ���� ��ü �߰�
                    lboxLog.Items.Add($"ID: {parsedData.Id} | ����: {parsedData.Title} | ����: {parsedData.Content}");

                }
                else
                {
                    lboxLog.Items.Add("���� �߻�: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                lboxLog.Items.Add("���� �߻�: " + ex.Message);
            }
        }
        // �������� �޾ƿ� JSON �������� ���� ����
        //await ������ �ɷ��ִ� ������ �񵿱�� ó���� �����ϱ� ���� ���̴�. 
        //�� �״�� ���� �ٸ� �۾��� �������̸� �� �۾��� ������ ��� �����ϱ� ���ؼ��̴�. 
        public class SpotNews
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }


    }
}
