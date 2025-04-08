// SpringBoot_Port_Scanner - ��Ʈ�������� �� �ּ� ����

using System; // .NET�� �⺻ ���ӽ����̽�, �پ��� �ٽ� ��� ����
using System.Collections.Generic; // ���׸� �÷��� ����� ���� ���ӽ����̽�
using System.ComponentModel; // ������Ʈ �� ��� ����
using System.Data; // ������ ó�� ���� ���ӽ����̽�
using System.Drawing; // GDI+ ��� �׷��� ��� ���
using System.Linq; // LINQ ��� ��� -> SQL�� ó�� ���� 
using System.Text; // ���ڿ� ���ڵ� �� ó��
using System.Threading.Tasks; // �񵿱� ���α׷����� ���� Task ���
using System.Windows.Forms; // WinForms UI ��� ���

using System.Net; // ��Ʈ��ũ ���
using System.Net.Sockets; // ���� ���α׷��� ���� ���
using System.IO; // ���� ����� ���
using System.Threading; // ������ ���
using System.Diagnostics; // ���μ��� �� ����� ���� ���
using System.Text.RegularExpressions; // ����ǥ���� ���
using System.Net.WebSockets; // ������ ���� ���

namespace SpringBoot_Port_Scanner // ������Ʈ ���ӽ����̽� ����
{
    public partial class Form1 : Form // Form1 Ŭ������ WinForms�� Form�� ���
    {
        public Form1()
        {
            InitializeComponent(); // �� ������Ʈ �ʱ�ȭ
        }

        private IPAddress scanIp = null; // ��ĵ�� IP �ּ� ���� ����
        private string strFilePath = null; // ��� ���� ���� ���
        private string IPAdrs = ""; // ���ڿ� ������ IP �ּ�
        private int PortStart = 0; // ���� ��Ʈ
        private int PortEnd = 0; // ���� ��Ʈ
        private CancellationTokenSource cts = null; // ��ĵ ��Ҹ� ���� ��ū �ҽ�

        private delegate void OnPortDelegate(string port, string status, string processName); // ��Ʈ ��ĵ ����� UI�� ����� ��������Ʈ
        private OnPortDelegate OnPort = null; // ��������Ʈ �ν��Ͻ�

        private delegate void OnProgressDelegate(int value); // ��ĵ ���൵�� UI�� ǥ���� ��������Ʈ
        private OnProgressDelegate OnProgress = null; // ��������Ʈ �ν��Ͻ�

        private void Form1_Load(object sender, EventArgs e)
        {
            OnPort = new OnPortDelegate(OnPortScan); // ��Ʈ ��� ��������Ʈ �ʱ�ȭ
            OnProgress = new OnProgressDelegate(OnProgressStatus); // ����� ��������Ʈ �ʱ�ȭ
        }

        private void OnPortScan(string port, string status, string processName)
        {
            this.lvScan.Items.Add(new ListViewItem(new string[] { port, status, processName })); // ��ĵ ����� ����Ʈ�信 �߰�
        }

        private void OnProgressStatus(int value)
        {
            this.pgbScan.Value = value; // ����� ���� ������Ʈ
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtIp.Text) && !string.IsNullOrWhiteSpace(txtStart.Text) 
                && !string.IsNullOrWhiteSpace(txtEnd.Text)) // �Է°� ��ȿ�� �˻�
            {
                IPAdrs = this.txtIp.Text; // IP �ּ� ����
                PortStart = Convert.ToInt32(this.txtStart.Text); // ���� ��Ʈ ����
                PortEnd = Convert.ToInt32(this.txtEnd.Text); // ���� ��Ʈ ����
                this.pgbScan.Minimum = PortStart; // ����� ���۰� ����
                this.pgbScan.Maximum = PortEnd; // ����� ���ᰪ ����
                this.btnScan.Enabled = false; // ��ĵ �߿��� ��ư ��Ȱ��ȭ

                cts = new CancellationTokenSource(); // ��ĵ �ߴ��� ���� ��ū ����
                Thread thread = new Thread(() => PortScanner(cts.Token)); // ��׶��� �����忡�� ��ĵ ����
                thread.Start(); // ������ ����
            }
            else
            {
                MessageBox.Show("IP �ּҿ� ��Ʈ ������ �Է��ϼ���.", "����", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                // ��ȿ�� �˻� ���� �� �޽��� ���
            }
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) // ���� ���� ���̾�α� ǥ��
            {
                strFilePath = Path.Combine(folderBrowserDialog.SelectedPath, "��Ʈ��ĵ_���.txt"); // ��� ���� ��� ����
                lblFile.Text = "���� ���: " + strFilePath; // �󺧿� ��� ǥ��
            }
        }

        private void PortScanner(CancellationToken token)
        {
            for (int i = PortStart; i <= PortEnd; i++) // ���� ��Ʈ���� �� ��Ʈ���� �ݺ�
            {
                if (token.IsCancellationRequested) // �ߴ� ��û �� ���� Ż��
                    break;
                int pid = GetProcessIdByPort(i); // �ش� ��Ʈ�� ���� ���� ���μ��� ID Ȯ��
                string processName = "N/A"; // �⺻�� ����
                if (pid > 0) // PID�� ������ ��� ���μ��� �̸� ���� �õ�
                {
                    try
                    {
                        Process process = Process.GetProcessById(pid);
                        processName = process.ProcessName;
                    }
                    catch (Exception)
                    {
                        processName = "Unknown"; // ���� ���� ������ �̹� ����� ���
                    }
                }
                bool isPortInNetstat = IsPortInNetstatOutput(i); // netstat ������� �ش� ��Ʈ�� ���� ���� �ִ��� Ȯ��
                string status = isPortInNetstat ? "Open" : "Mismatch"; // ���� ����

                BeginInvoke(new Action(() => lvScan.Items.Add(new ListViewItem(new string[] { pid.ToString(), status, processName })))); 
                // UI �����忡�� ����Ʈ�信 ��� �߰�

                Thread.Sleep(10); // ������ CPU ���� ����
            }
            SaveNetstatOutputToFile(); // ��ü ����� ���Ϸ� ����
            MessageBox.Show("��ĵ �Ϸ�", "�˸�", MessageBoxButtons.OK, MessageBoxIcon.Information); // �Ϸ� �޽��� ǥ��
        }

        private int GetProcessIdByPort(int port)
        {
            try
            {
                Process netstatProcess = new Process(); // ���ο� ���μ��� ����
                netstatProcess.StartInfo.FileName = "cmd.exe"; // ��� ������Ʈ ����
                netstatProcess.StartInfo.Arguments = $"/c netstat -ano -p TCP | findstr :{port}"; // netstat�� �ش� ��Ʈ ã��
                netstatProcess.StartInfo.RedirectStandardOutput = true; // ��� ���𷺼�
                netstatProcess.StartInfo.UseShellExecute = false; // �� ��� �� ��
                netstatProcess.StartInfo.CreateNoWindow = true; // â ����
                netstatProcess.Start(); // ���μ��� ����

                string output = netstatProcess.StandardOutput.ReadToEnd(); // ��� ��ü �б�
                netstatProcess.WaitForExit(); // ���μ��� ������� ���

                string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); // �� ������ �и�
                foreach (string line in lines)
                {
                    string[] parts = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // ���� ���� ����
                    if (parts.Length > 4 && int.TryParse(parts[^1], out int processId)) // ������ ��Ұ� PID�̸� ��ȯ
                    {
                        return processId;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching process ID: " + ex.Message); // ����� ���
            }
            return 0; // ���� �� 0 ��ȯ
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (cts != null) // ��� ��ū�� ���� ���
            {
                cts.Cancel(); // ��� ��û
                MessageBox.Show("��ĵ�� �ߴܵǾ����ϴ�.", "�˸�", MessageBoxButtons.OK, MessageBoxIcon.Warning); // �˸� ǥ��
                BeginInvoke(new Action(Reset)); // UI �ʱ�ȭ ȣ��
            }
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            if (lvScan.SelectedItems.Count > 0) // ����Ʈ���� ���õ� �׸��� ���� ���(1�� �̻��̹Ƿ�)
            {
                try
                {
                    string pidStr = lvScan.SelectedItems[0].SubItems[0].Text; // ���õ� PID ���ڿ� ����
                    if (int.TryParse(pidStr, out int pid) && pid > 0) // ��ȿ�� �������� Ȯ��
                    {
                        if (IsProcessRunning(pid)) // ���� ���� ������ Ȯ��
                        {
                            try
                            {
                                Process.GetProcessById(pid).Kill(); // ���μ��� ���� �õ�
                                MessageBox.Show($"PID {pid} ���μ����� �����߽��ϴ�.", "�˸�");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("�Ϲ� ���� ����. ���� ���� �õ�.\n" + ex.Message, "����"); // ���� �� ������ ���� ���� �õ�
                                RunTaskKillAsAdmin(pid);
                            }
                            lvScan.SelectedItems[0].Remove(); // ����Ʈ�信�� ����
                        }
                        else
                        {
                            MessageBox.Show("�̹� ����� ���μ����Դϴ�.", "�˸�"); // �̹� ���� ���
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("���μ��� ���� �� ���� �߻�: " + ex.Message, "����"); // ���� ó��
                }
            }
            else
            {
                MessageBox.Show("������ ���μ����� �����ϼ���.", "�˸�"); // ���� �� �� ���
            }
        }

        private void Reset()
        {
            txtIp.Text = ""; // IP �Է� �ʵ� �ʱ�ȭ
            txtStart.Text = ""; // ���� ��Ʈ �Է� �ʵ� �ʱ�ȭ
            txtEnd.Text = ""; // ���� ��Ʈ �Է� �ʵ� �ʱ�ȭ
            lvScan.Items.Clear(); // ����Ʈ�� �ʱ�ȭ
            btnScan.Enabled = true; // ��ĵ ��ư �ٽ� Ȱ��ȭ
            btnFilePath.Enabled = true; // ��� ���� ��ư Ȱ��ȭ
            btnKill.Enabled = true; // ���� ��ư Ȱ��ȭ
        }

        private void SaveNetstatOutputToFile()
        {
            if (string.IsNullOrEmpty(strFilePath)) // ��ΰ� �������� ���� ���
            {
                MessageBox.Show("���� ��θ� ���� �����ϼ���!", "����"); // ����ڿ��� ���
                return;
            }

            try
            {
                Process netstatProcess = new Process(); // �� ���μ��� ��ü ����
                netstatProcess.StartInfo.FileName = "cmd.exe"; // cmd ����
                netstatProcess.StartInfo.Arguments = "/c netstat -ano > \"" + strFilePath + "\""; // ����� ���Ϸ� ����
                netstatProcess.StartInfo.UseShellExecute = false; // �� ��� �� ��
                netstatProcess.StartInfo.CreateNoWindow = true; // â ǥ�� �� ��
                netstatProcess.Start(); // ��� ����
                netstatProcess.WaitForExit(); // ���� ���

                Process.Start("notepad.exe", strFilePath); // �޸������� ���� ����
            }
            catch (Exception ex)
            {
                MessageBox.Show("netstat ��� ���� �� ���� �߻�: " + ex.Message, "����"); // ���� �˸�
            }
        }

        private bool IsPortInNetstatOutput(int port)
        {
            try
            {
                Process netstatProcess = new Process(); // netstat ��� ����� ���μ���
                netstatProcess.StartInfo.FileName = "cmd.exe"; // cmd ����
                netstatProcess.StartInfo.Arguments = "/c netstat -ano -p TCP"; // TCP ��Ʈ ��� ��������
                netstatProcess.StartInfo.RedirectStandardOutput = true; // ��� ���𷺼�
                netstatProcess.StartInfo.UseShellExecute = false; // �� ��� �� ��
                netstatProcess.StartInfo.CreateNoWindow = true; // â ����
                netstatProcess.Start(); // ����

                string output = netstatProcess.StandardOutput.ReadToEnd(); // ��� �б�
                netstatProcess.WaitForExit(); // ���� ���
                return output.Contains(":" + port); // ��Ʈ ��ȣ ���� ���η� �Ǵ�
            }
            catch (Exception)
            {
                return false; // ���� �� false ��ȯ
            }
        }

        private void RunTaskKillAsAdmin(int pid)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(); // ������ ���� ������ ���� ����
                psi.FileName = "cmd.exe"; // cmd ����
                psi.Arguments = $"/c taskkill /PID {pid} /F"; // ���� ���� ��ɾ�
                psi.Verb = "runas"; // ������ ���� ����
                psi.UseShellExecute = true; // �� ���
                Process.Start(psi); // ����
            }
            catch (Exception ex)
            {
                MessageBox.Show("������ ���� ���� ����: " + ex.Message, "����"); // ���� �� �˸�
            }
        }

        private bool IsProcessRunning(int pid)
        {
            try
            {
                return Process.GetProcessById(pid) != null; // ���μ����� �����ϸ� true
            }
            catch (ArgumentException)
            {
                return false; // ���μ����� �������� ���� ��� false
            }
        }
    }
}
