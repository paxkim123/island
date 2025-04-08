// SpringBoot_Port_Scanner - 포트폴리오용 상세 주석 버전

using System; // .NET의 기본 네임스페이스, 다양한 핵심 기능 제공
using System.Collections.Generic; // 제네릭 컬렉션 사용을 위한 네임스페이스
using System.ComponentModel; // 컴포넌트 모델 기능 제공
using System.Data; // 데이터 처리 관련 네임스페이스
using System.Drawing; // GDI+ 기반 그래픽 기능 사용
using System.Linq; // LINQ 기능 사용 -> SQL문 처리 관련 
using System.Text; // 문자열 인코딩 및 처리
using System.Threading.Tasks; // 비동기 프로그래밍을 위한 Task 사용
using System.Windows.Forms; // WinForms UI 기능 사용

using System.Net; // 네트워크 기능
using System.Net.Sockets; // 소켓 프로그래밍 관련 기능
using System.IO; // 파일 입출력 기능
using System.Threading; // 스레딩 기능
using System.Diagnostics; // 프로세스 및 디버깅 관련 기능
using System.Text.RegularExpressions; // 정규표현식 사용
using System.Net.WebSockets; // 웹소켓 관련 기능

namespace SpringBoot_Port_Scanner // 프로젝트 네임스페이스 정의
{
    public partial class Form1 : Form // Form1 클래스는 WinForms의 Form을 상속
    {
        public Form1()
        {
            InitializeComponent(); // 폼 컴포넌트 초기화
        }

        private IPAddress scanIp = null; // 스캔할 IP 주소 저장 변수
        private string strFilePath = null; // 결과 파일 저장 경로
        private string IPAdrs = ""; // 문자열 형태의 IP 주소
        private int PortStart = 0; // 시작 포트
        private int PortEnd = 0; // 종료 포트
        private CancellationTokenSource cts = null; // 스캔 취소를 위한 토큰 소스

        private delegate void OnPortDelegate(string port, string status, string processName); // 포트 스캔 결과를 UI에 출력할 델리게이트
        private OnPortDelegate OnPort = null; // 델리게이트 인스턴스

        private delegate void OnProgressDelegate(int value); // 스캔 진행도를 UI에 표시할 델리게이트
        private OnProgressDelegate OnProgress = null; // 델리게이트 인스턴스

        private void Form1_Load(object sender, EventArgs e)
        {
            OnPort = new OnPortDelegate(OnPortScan); // 포트 출력 델리게이트 초기화
            OnProgress = new OnProgressDelegate(OnProgressStatus); // 진행률 델리게이트 초기화
        }

        private void OnPortScan(string port, string status, string processName)
        {
            this.lvScan.Items.Add(new ListViewItem(new string[] { port, status, processName })); // 스캔 결과를 리스트뷰에 추가
        }

        private void OnProgressStatus(int value)
        {
            this.pgbScan.Value = value; // 진행바 값을 업데이트
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtIp.Text) && !string.IsNullOrWhiteSpace(txtStart.Text) 
                && !string.IsNullOrWhiteSpace(txtEnd.Text)) // 입력값 유효성 검사
            {
                IPAdrs = this.txtIp.Text; // IP 주소 저장
                PortStart = Convert.ToInt32(this.txtStart.Text); // 시작 포트 저장
                PortEnd = Convert.ToInt32(this.txtEnd.Text); // 종료 포트 저장
                this.pgbScan.Minimum = PortStart; // 진행바 시작값 설정
                this.pgbScan.Maximum = PortEnd; // 진행바 종료값 설정
                this.btnScan.Enabled = false; // 스캔 중에는 버튼 비활성화

                cts = new CancellationTokenSource(); // 스캔 중단을 위한 토큰 생성
                Thread thread = new Thread(() => PortScanner(cts.Token)); // 백그라운드 스레드에서 스캔 실행
                thread.Start(); // 스레드 시작
            }
            else
            {
                MessageBox.Show("IP 주소와 포트 범위를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                // 유효성 검사 실패 시 메시지 출력
            }
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) // 폴더 선택 다이얼로그 표시
            {
                strFilePath = Path.Combine(folderBrowserDialog.SelectedPath, "포트스캔_결과.txt"); // 결과 파일 경로 생성
                lblFile.Text = "파일 경로: " + strFilePath; // 라벨에 경로 표시
            }
        }

        private void PortScanner(CancellationToken token)
        {
            for (int i = PortStart; i <= PortEnd; i++) // 시작 포트부터 끝 포트까지 반복
            {
                if (token.IsCancellationRequested) // 중단 요청 시 루프 탈출
                    break;
                int pid = GetProcessIdByPort(i); // 해당 포트를 점유 중인 프로세스 ID 확인
                string processName = "N/A"; // 기본값 설정
                if (pid > 0) // PID가 존재할 경우 프로세스 이름 추출 시도
                {
                    try
                    {
                        Process process = Process.GetProcessById(pid);
                        processName = process.ProcessName;
                    }
                    catch (Exception)
                    {
                        processName = "Unknown"; // 접근 권한 문제나 이미 종료된 경우
                    }
                }
                bool isPortInNetstat = IsPortInNetstatOutput(i); // netstat 결과에서 해당 포트가 실제 열려 있는지 확인
                string status = isPortInNetstat ? "Open" : "Mismatch"; // 상태 설정

                BeginInvoke(new Action(() => lvScan.Items.Add(new ListViewItem(new string[] { pid.ToString(), status, processName })))); 
                // UI 스레드에서 리스트뷰에 결과 추가

                Thread.Sleep(10); // 과도한 CPU 점유 방지
            }
            SaveNetstatOutputToFile(); // 전체 결과를 파일로 저장
            MessageBox.Show("스캔 완료", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information); // 완료 메시지 표시
        }

        private int GetProcessIdByPort(int port)
        {
            try
            {
                Process netstatProcess = new Process(); // 새로운 프로세스 생성
                netstatProcess.StartInfo.FileName = "cmd.exe"; // 명령 프롬프트 실행
                netstatProcess.StartInfo.Arguments = $"/c netstat -ano -p TCP | findstr :{port}"; // netstat로 해당 포트 찾기
                netstatProcess.StartInfo.RedirectStandardOutput = true; // 출력 리디렉션
                netstatProcess.StartInfo.UseShellExecute = false; // 셸 사용 안 함
                netstatProcess.StartInfo.CreateNoWindow = true; // 창 숨김
                netstatProcess.Start(); // 프로세스 시작

                string output = netstatProcess.StandardOutput.ReadToEnd(); // 출력 전체 읽기
                netstatProcess.WaitForExit(); // 프로세스 종료까지 대기

                string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); // 줄 단위로 분리
                foreach (string line in lines)
                {
                    string[] parts = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // 공백 기준 분할
                    if (parts.Length > 4 && int.TryParse(parts[^1], out int processId)) // 마지막 요소가 PID이면 반환
                    {
                        return processId;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching process ID: " + ex.Message); // 디버깅 출력
            }
            return 0; // 실패 시 0 반환
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (cts != null) // 취소 토큰이 있을 경우
            {
                cts.Cancel(); // 취소 요청
                MessageBox.Show("스캔이 중단되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning); // 알림 표시
                BeginInvoke(new Action(Reset)); // UI 초기화 호출
            }
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            if (lvScan.SelectedItems.Count > 0) // 리스트에서 선택된 항목이 있을 경우(1개 이상이므로)
            {
                try
                {
                    string pidStr = lvScan.SelectedItems[0].SubItems[0].Text; // 선택된 PID 문자열 추출
                    if (int.TryParse(pidStr, out int pid) && pid > 0) // 유효한 정수인지 확인
                    {
                        if (IsProcessRunning(pid)) // 실제 실행 중인지 확인
                        {
                            try
                            {
                                Process.GetProcessById(pid).Kill(); // 프로세스 종료 시도
                                MessageBox.Show($"PID {pid} 프로세스를 종료했습니다.", "알림");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("일반 종료 실패. 강제 종료 시도.\n" + ex.Message, "오류"); // 실패 시 관리자 권한 종료 시도
                                RunTaskKillAsAdmin(pid);
                            }
                            lvScan.SelectedItems[0].Remove(); // 리스트뷰에서 제거
                        }
                        else
                        {
                            MessageBox.Show("이미 종료된 프로세스입니다.", "알림"); // 이미 꺼진 경우
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("프로세스 종료 중 오류 발생: " + ex.Message, "오류"); // 예외 처리
                }
            }
            else
            {
                MessageBox.Show("종료할 프로세스를 선택하세요.", "알림"); // 선택 안 된 경우
            }
        }

        private void Reset()
        {
            txtIp.Text = ""; // IP 입력 필드 초기화
            txtStart.Text = ""; // 시작 포트 입력 필드 초기화
            txtEnd.Text = ""; // 종료 포트 입력 필드 초기화
            lvScan.Items.Clear(); // 리스트뷰 초기화
            btnScan.Enabled = true; // 스캔 버튼 다시 활성화
            btnFilePath.Enabled = true; // 경로 설정 버튼 활성화
            btnKill.Enabled = true; // 종료 버튼 활성화
        }

        private void SaveNetstatOutputToFile()
        {
            if (string.IsNullOrEmpty(strFilePath)) // 경로가 설정되지 않은 경우
            {
                MessageBox.Show("파일 경로를 먼저 설정하세요!", "오류"); // 사용자에게 경고
                return;
            }

            try
            {
                Process netstatProcess = new Process(); // 새 프로세스 객체 생성
                netstatProcess.StartInfo.FileName = "cmd.exe"; // cmd 실행
                netstatProcess.StartInfo.Arguments = "/c netstat -ano > \"" + strFilePath + "\""; // 결과를 파일로 저장
                netstatProcess.StartInfo.UseShellExecute = false; // 셸 사용 안 함
                netstatProcess.StartInfo.CreateNoWindow = true; // 창 표시 안 함
                netstatProcess.Start(); // 명령 실행
                netstatProcess.WaitForExit(); // 종료 대기

                Process.Start("notepad.exe", strFilePath); // 메모장으로 파일 열기
            }
            catch (Exception ex)
            {
                MessageBox.Show("netstat 결과 저장 중 오류 발생: " + ex.Message, "오류"); // 에러 알림
            }
        }

        private bool IsPortInNetstatOutput(int port)
        {
            try
            {
                Process netstatProcess = new Process(); // netstat 명령 실행용 프로세스
                netstatProcess.StartInfo.FileName = "cmd.exe"; // cmd 실행
                netstatProcess.StartInfo.Arguments = "/c netstat -ano -p TCP"; // TCP 포트 목록 가져오기
                netstatProcess.StartInfo.RedirectStandardOutput = true; // 출력 리디렉션
                netstatProcess.StartInfo.UseShellExecute = false; // 셸 사용 안 함
                netstatProcess.StartInfo.CreateNoWindow = true; // 창 숨김
                netstatProcess.Start(); // 실행

                string output = netstatProcess.StandardOutput.ReadToEnd(); // 출력 읽기
                netstatProcess.WaitForExit(); // 종료 대기
                return output.Contains(":" + port); // 포트 번호 포함 여부로 판단
            }
            catch (Exception)
            {
                return false; // 실패 시 false 반환
            }
        }

        private void RunTaskKillAsAdmin(int pid)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(); // 관리자 권한 실행을 위한 설정
                psi.FileName = "cmd.exe"; // cmd 실행
                psi.Arguments = $"/c taskkill /PID {pid} /F"; // 강제 종료 명령어
                psi.Verb = "runas"; // 관리자 권한 실행
                psi.UseShellExecute = true; // 셸 사용
                Process.Start(psi); // 실행
            }
            catch (Exception ex)
            {
                MessageBox.Show("관리자 권한 종료 실패: " + ex.Message, "오류"); // 실패 시 알림
            }
        }

        private bool IsProcessRunning(int pid)
        {
            try
            {
                return Process.GetProcessById(pid) != null; // 프로세스가 존재하면 true
            }
            catch (ArgumentException)
            {
                return false; // 프로세스가 존재하지 않을 경우 false
            }
        }
    }
}
