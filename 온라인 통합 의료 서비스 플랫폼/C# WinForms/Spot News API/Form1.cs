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
        // REST API 전송 방식 정의 (POST, GET, PUT, DELETE)
        enum Method
        {
            POST,
            GET,
            PUT,
            DELETE
        }
        enum LogTrace // 로깅 레벨
        {
            L1,//가장 기본적인 정보 로그 (L1: 가장 낮은 중요도)
            L2,//L1보다 중요한 로그, 디버깅용 상세 정보 포함
            L3,//L2보다 더 중요한 로그, 실행 흐름 확인 가능
            Warning,//심각하지 않지만, 주의해야 할 문제 (예: 성능 저하)
            Error,//프로그램 실행 중 오류 발생 (예: 예외 발생, 실패한 요청)
        }

        public Form1()
        {
            InitializeComponent();// UI 초기화
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 콤보박스에 HTTP Method 항목 추가
            foreach (Method m in Enum.GetValues(typeof(Method)))
            {
                cboxMethod.Items.Add(m.ToString());
                cboxMethod.SelectedIndex = 0;// 기본값 설정 (POST)
            }
        }
        // Method 콤보박스 선택 변경 시 실행
        private void cboxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            Method m = StringToEnum(cboxMethod.Text);
            //post와 put은 json의 삽입 및 수정이므로 Body 박스에
            //입력할 수 있도록 true로 설정되도록 하고 
            //get과 delete는 각 출력과 삭제이므로 Body 박스에 입력할 수 없도록
            //false로 설정해야 한다.
            //C# 윈폼은 MySql에 직접적으로 접근하지 않고 스프링부트 서버에 우선적으로 보낸다.
            //그 후 전송된 데이터는 서버에서 DB로 이동하여 저장되고 전송된 데이터는 메인하면에 표시된다.
            // POST와 PUT은 Body 입력 허용, GET과 DELETE는 비활성화
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

        // 전송 버튼 클릭 시 실행
        private async void btnSend_Click(object sender, EventArgs e)
        {
            Send(StringToEnum(cboxMethod.Text));// 메서드 실행
            // GET 요청인 경우 결과를 리스트에 표시
            if (cboxMethod.SelectedItem.ToString() == "GET")
            {
                await FetchDataToListBox();
            }
        }


        // 문자열을 enum으로 변환 (실패 시 로그)
        private Method StringToEnum(string m)
        {
            Method M;
            if(!Enum.TryParse<Method>(m, out M))
            {
                Log(LogTrace.Error, $"여기에 없는 값은 사용할 수 없습니다.({m}");
            }
            return M;
        }
        // 로그 출력 함수
        private void Log(LogTrace l, string LogDesc)
        {
            this.Invoke(new Action(delegate ()
            {
                if (lboxLog.Items.Count >= 15) 
                {
                    lboxLog.Items.RemoveAt(14); // 로그 15개 초과 시 가장 오래된 로그 삭제
                }
                DateTime d = DateTime.Now;
                string info = $"{d:yyyy-MM-dd HH:mm:ss.fff} [{l.ToString()}] {LogDesc}";

                lboxLog.Items.Insert(0, info); // 최신 로그가 위쪽에 추가되도록 설정
                lboxLog.TopIndex = 0; // 항상 최신 로그를 표시
            }));
        }

        //new Action(delegate{...})를 컴파일하면 컴파일러가 익명 메서드 또는 람다식을 델리게이트 객체로 캡슐화한다.
        //내부적으로는 MulticastDelegate 기반의 Actoin 객체 인스턴스가 생성된다.
        //이 인스턴스는 CLR 힙에 할당되어 Invoke() 메서드를 호출한다.
        //말하자면 다른 코드(스레드)가 열심히 실행되는 중에 UI에 뭔가 해야 할 일이 생기면,
        //UI가 한가할 때 처리하고 메모를 넘기는 방식이 Invoke+Action 패턴이다.

        // REST API 호출 함수
        private async void Send(Method m)
        {
            string Url = tboxURL.Text;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                { 
                    HttpResponseMessage httpResponseMessage = null;
                    string sendingData = tboxBody.Text;// JSON 입력
                    StringContent stringContent = new StringContent(sendingData, System.Text.Encoding.UTF8, "application/json");
                    // URL에서 ID 추출 및 원본 URL 분리
                    string Id = Url.Substring(Url.LastIndexOf("/") + 1).Trim();
                    string originalUrl = Url.Substring(0, Url.LastIndexOf("/"));
                    Console.WriteLine($"Original URL: {originalUrl}");// GET, PUT, DELETE는 ID가 필요
                    if (string.IsNullOrEmpty(Id) && m != Method.POST)
                    {
                        Log(LogTrace.Error, "Id를 입력하세요");
                        return;
                    }                   
                    switch (m) // 전송 방식에 따라 API 호출
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
                        default: Log(LogTrace.Error, "잘못된 요청 타입");
                            break;
                    }

                    // 응답 로그 출력
                    string mType = $"Method = {httpResponseMessage.RequestMessage.Method}, StatusCode = {httpResponseMessage.StatusCode.ToString()}";
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        String responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                        Log(LogTrace.L1, $"데이터를 성공적으로 수신하였습니다.");
                    }
                    else
                    {
                        Log(LogTrace.L1, $"요청에 실패하였습니다. 상태코드: {httpResponseMessage.StatusCode}");
                    }
                    Log(LogTrace.L1, $"{mType}");
                }
                catch(Exception ex)
                {
                    Log(LogTrace.Error, ex.Message);
                }
            }
        }
        // JSON 데이터 불러오기 (tboxBody에 표시용)
        private async Task FetchDataAsync()
        {
            string url = tboxURL.Text; // URL 입력 필드에서 가져오기
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                //여기서 GetAsync()는 실제 요청을 보내고 아직 응답이 오지 않았음에도 프로그램은 중단되지 않고 흘러간다
                string jsonData = await response.Content.ReadAsStringAsync();

                Console.WriteLine("응답 데이터: " + jsonData); // 디버깅 용도
                //await 이후 UI에 반영
                if (response.IsSuccessStatusCode)
                {
                    // JSON 데이터를 객체로 변환 (예제: Dictionary 사용)
                    var parsedData = JsonConvert.DeserializeObject<dynamic>(jsonData);

                    // UI에 JSON 데이터 표시
                    tboxBody.Text = JsonConvert.SerializeObject(parsedData, Formatting.Indented);
                }
                else
                {
                    tboxBody.Text = "오류 발생: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                tboxBody.Text = "예외 발생: " + ex.Message;
            }
        }
        // JSON 데이터 불러와서 리스트박스에 표시 (GET 시 사용)
        private async Task FetchDataToListBox()
        {
            string url = tboxURL.Text; // URL 입력 필드에서 가져오기
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string jsonData = await response.Content.ReadAsStringAsync();

                Console.WriteLine("응답 데이터: " + jsonData); // 디버깅 용도

                if (response.IsSuccessStatusCode)
                {
                    // JSON 데이터를 리스트 형태로 변환 (예제: 리스트 형태의 객체라고 가정)
                    var parsedData = JsonConvert.DeserializeObject<SpotNews>(jsonData);



                    // ListBox 내용 초기화
                    lboxLog.Items.Clear();

                    // ListBox에 단일 객체 추가
                    lboxLog.Items.Add($"ID: {parsedData.Id} | 제목: {parsedData.Title} | 내용: {parsedData.Content}");

                }
                else
                {
                    lboxLog.Items.Add("오류 발생: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                lboxLog.Items.Add("예외 발생: " + ex.Message);
            }
        }
        // 서버에서 받아올 JSON 데이터의 구조 정의
        //await 구문이 걸려있는 로직은 비동기식 처리를 보장하기 위한 곳이다. 
        //말 그대로 현재 다른 작업이 진행중이면 그 작업이 끝나는 대로 실행하기 위해서이다. 
        public class SpotNews
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }


    }
}
