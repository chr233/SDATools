using Newtonsoft.Json;
using SDATools.Data;
using SDATools.Properties;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SDATools;

public partial class FrmMain : Form
{
    public FrmMain()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        tsAuthor.Text = "作者: Chr_";
        var version = Assembly.GetExecutingAssembly().GetName().Version ?? new Version("0.0.0.0");
        tsVersion.Text = $"版本: {version}";
        tsGithub.Text = "获取源码";

        var config = GlobalConfig.Default;

        if (!config.Upgraded)
        {
            config.Upgrade();
            config.Upgraded = true;
            config.Save();
        }

        txtMaFolder.Text = config.MaFolder;
        (config.UseSteamID ? rbSteamID : (config.UseAccountName ? rbSteamIDAccountName : rbAccountName)).Checked = true;
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        var config = GlobalConfig.Default;

        config.MaFolder = txtMaFolder.Text;
        config.UseSteamID = rbSteamID.Checked;
        config.UseAccountName = rbSteamIDAccountName.Checked;
        config.Save();
    }

    private void BtnMaFolder_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            ShowNewFolderButton = true,
            AutoUpgradeEnabled = true,
            InitialDirectory = txtMaFolder.Text,
        };
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            if (!string.IsNullOrEmpty(dialog.SelectedPath))
            {
                txtMaFolder.Text = dialog.SelectedPath;
            }
        }
    }

    private async void BtnSort_Click(object sender, EventArgs e)
    {
        string maFolder = txtMaFolder.Text;
        int mode = rbSteamID.Checked ? 1 : (rbAccountName.Checked ? 2 : 3);

        if (!Path.Exists(maFolder))
        {
            MessageBox.Show("令牌文件夹路径不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtMaFolder.Focus();
            return;
        }

        var filePaths = Directory.EnumerateFiles(maFolder, "*.maFile");
        if (!filePaths.Any())
        {
            MessageBox.Show("令牌文件夹下未找到可用令牌 (需要以 .maFile 结尾)", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // 读取 manifest.json
        string manifestPath = Path.Combine(maFolder, "manifest.json");
        bool exist = File.Exists(manifestPath);
        ManifestData? manifest = null;

        if (exist)
        {
            try
            {
                using var maStream = File.Open(manifestPath, FileMode.Open, FileAccess.Read);
                manifest = await Utils.TryParseJson<ManifestData>(maStream);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(string.Format("读取 {0} 失败, 继续操作会导致旧的内容被覆盖\r\n{1}\r\n确认 - 继续运行, 取消 - 终止操作", manifestPath, ex.Message), ex.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                {
                    return;
                }
            }
        }

        if (manifest == null)
        {
            manifest = new ManifestData();
        }

        // 扫描 mafile 文件, key = account name
        var maFilesDict = new Dictionary<string, List<MaFileData>>();
        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);
            MaFileData? maFile = null;
            try
            {
                using var maStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                maFile = await Utils.TryParseJson<MaFileData>(maStream);

                if (maFile != null)
                {
                    string key = maFile.AccountName?.ToLower() ?? "NULL";
                    if (maFilesDict.TryGetValue(key, out var list))
                    {
                        list.Add(maFile);
                    }
                    else
                    {
                        list = new List<MaFileData> { maFile };
                        maFilesDict.TryAdd(key, list);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("读取 {0} 失败, 该文件可能被加密\r\n{1}", filePath, ex.Message), ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (maFile == null)
                {
                    maFile = new MaFileData
                    {
                        FileName = fileName,
                        FilePath = filePath,
                    };
                }
                else
                {
                    maFile.FileName = fileName;
                    maFile.FilePath = filePath;
                }
            }
        }

        var entries = new Dictionary<ulong, ManifestEntryData>();

        int success = 0, failed = 0, duplicate = 0;
        foreach (var (account, maFiles) in maFilesDict)
        {
            // 跳过空的列表
            if (maFiles.Count == 0)
            {
                continue;
            }

            if (maFiles.Count == 1 && account != "NULL")
            {
                var maFile = maFiles[0];

                // 检查路径字段是否有效
                if (!maFile.FileValid)
                {
                    continue;
                }

                string? newFileName = null;
                if (maFile.SteamIdValid)
                {
                    newFileName = mode switch
                    {
                        1 => $"{maFile.Session?.SteamID}.maFile",
                        2 => $"{maFile.AccountName}.maFile",
                        3 => $"{maFile.AccountName}-{maFile.Session?.SteamID}.maFile",
                        _ => "",
                    };
                }
                else
                {
                    newFileName = $"未登录-{maFile.AccountName}.maFile";
                }

                if (maFile.SteamIdValid)
                {
                    var steamID = maFile.Session?.SteamID;
                    entries.TryAdd(steamID!.Value, new ManifestEntryData
                    {
                        EncryptionIv = null,
                        EncryptionSalt = null,
                        SteamId = steamID.Value,
                        FileName = newFileName,
                    });
                }

                if (newFileName != maFile.FileName)
                {
                    try
                    {
                        var newPath = Path.Combine(maFolder, newFileName);
                        File.Move(maFile.FilePath!, newPath, false);
                        maFile.FilePath = newPath;
                        success++;
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        MessageBox.Show(string.Format("重命名文件 {0} -> {1} 失败, 可能已有相同的文件\r\n{2}", maFile.FileName, newFileName, ex.Message), ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    success++;
                }
            }
            else
            {
                int count = 1;
                foreach (var maFile in maFiles)
                {
                    // 检查路径字段是否有效
                    if (!maFile.FileValid)
                    {
                        continue;
                    }

                    duplicate++;

                    string? newFileName = null;
                    if (maFile.SteamIdValid)
                    {
                        newFileName = $"重复-{maFile.AccountName}-{maFile.Session?.SteamID}-{count++}.maFile";
                    }
                    else
                    {
                        newFileName = $"重复-{count++}-{maFile.AccountName}-未登录.maFile";
                    }

                    if (maFile.SteamIdValid)
                    {
                        var steamID = maFile.Session?.SteamID;
                        entries.TryAdd(steamID!.Value, new ManifestEntryData
                        {
                            EncryptionIv = null,
                            EncryptionSalt = null,
                            SteamId = steamID.Value,
                            FileName = newFileName,
                        });
                    }

                    if (newFileName != maFile.FileName)
                    {
                        try
                        {
                            var newPath = Path.Combine(maFolder, newFileName);
                            File.Move(maFile.FilePath!, newPath, false);
                            maFile.FilePath = newPath;
                            success++;
                        }
                        catch (Exception ex)
                        {
                            failed++;
                            MessageBox.Show(string.Format("重命名文件 {0} -> {1} 失败, 可能已有相同的文件\r\n{2}", maFile.FileName, newFileName, ex.Message), ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        success++;
                    }
                }
            }
        }

        manifest.Entries = entries.Values.ToList();

        using var msStream = File.Open(manifestPath, exist ? FileMode.Truncate : FileMode.CreateNew, FileAccess.Write);
        var json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
        await msStream.WriteAsync(Encoding.UTF8.GetBytes(json));
        await msStream.FlushAsync();

        MessageBox.Show(string.Format("重命名成功 {0} 个, 重命名失败 {1} 个, 重复的令牌 {2} 个", success, failed, duplicate), "操作完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private async void BtnUpdate_Click(object sender, EventArgs e)
    {
        string maFolder = txtMaFolder.Text;

        if (!Path.Exists(maFolder))
        {
            MessageBox.Show("令牌文件夹路径不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtMaFolder.Focus();
            return;
        }

        var filePaths = Directory.EnumerateFiles(maFolder, "*.maFile");
        if (!filePaths.Any())
        {
            MessageBox.Show("令牌文件夹下未找到可用令牌 (需要以 .maFile 结尾)", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // 读取 manifest.json
        string manifestPath = Path.Combine(maFolder, "manifest.json");
        bool exist = File.Exists(manifestPath);
        ManifestData? manifest = null;

        if (exist)
        {
            try
            {
                using var maStream = File.Open(manifestPath, FileMode.Open, FileAccess.Read);
                manifest = await Utils.TryParseJson<ManifestData>(maStream);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(string.Format("读取 {0} 失败, 继续操作会导致旧的内容被覆盖\r\n{1}\r\n确认 - 继续运行, 取消 - 终止操作", manifestPath, ex.Message), ex.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                {
                    return;
                }
            }
        }

        if (manifest == null)
        {
            manifest = new ManifestData();
        }

        // 扫描 mafile 文件, key = account name
        var entries = new Dictionary<ulong, ManifestEntryData>();
        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);
            MaFileData? maFile = null;
            try
            {
                using var maStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                maFile = await Utils.TryParseJson<MaFileData>(maStream);

                if (maFile?.SteamIdValid == true)
                {
                    maFile.FileName = fileName;
                    maFile.FilePath = filePath;

                    var steamID = maFile.Session?.SteamID;
                    entries.TryAdd(steamID!.Value, new ManifestEntryData
                    {
                        EncryptionIv = null,
                        EncryptionSalt = null,
                        SteamId = steamID.Value,
                        FileName = fileName,
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("读取 {0} 失败, 该文件可能被加密\r\n{1}", filePath, ex.Message), ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        manifest.Entries = entries.Values.ToList();

        using var msStream = File.Open(manifestPath, exist ? FileMode.Truncate : FileMode.CreateNew, FileAccess.Write);
        var json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
        await msStream.WriteAsync(Encoding.UTF8.GetBytes(json));
        await msStream.FlushAsync();

        MessageBox.Show(string.Format("共计 {0} 个有效令牌, 更新 manifest.json 完成", manifest.Entries.Count), "操作完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static void OpenLink(string uri)
    {
        Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
    }

    private void TsAuthor_Click(object sender, EventArgs e)
    {
        const string target = "https://steamcommunity.com/id/Chr_/";
        OpenLink(target);
    }

    private void TsGithub_Click(object sender, EventArgs e)
    {
        const string target = "https://github.com/chr233/SDATools";
        OpenLink(target);
    }

    private void TsVersion_Click(object sender, EventArgs e)
    {
        const string target = "https://github.com/chr233/SDATools/releases";
        OpenLink(target);
    }
}