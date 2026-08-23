using SMAPIGameLoader.Launcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SMAPIGameLoader.Tool;
internal static class LogParser
{
    const string SMAPILogFileName = "SMAPI-latest.txt";

    public static void OnClickUploadLog(object sender, EventArgs eventArgs)
    {
        try
        {
            TaskTool.Run(ActivityTool.CurrentActivity, async () =>
            {
                try
                {
                    TaskTool.SetTitle("SMAPI Log Saving...");
                    await TaskSaveLogLocal();
                }
                catch (Exception ex)
                {
                    ErrorDialogTool.Show(ex);
                }
            });
        }
        catch (Exception ex)
        {
            ErrorDialogTool.Show(ex);
        }
    }
    static async Task TaskSaveLogLocal()
    {
        TaskTool.NewLine("starting task save log");

        string logFilePath = Path.Combine(FileTool.ExternalFilesDir, "ErrorLogs", SMAPILogFileName);
        if (File.Exists(logFilePath) is false)
        {
            ErrorDialogTool.Show(new Exception($"Not found file {logFilePath}"), "SMAPI Log Error");
            return;
        }

        TaskTool.NewLine("read log from path: " + logFilePath);
        var logStringContent = File.ReadAllText(logFilePath);
        var fileSize = new FileInfo(logFilePath).Length / 1024f;
        TaskTool.NewLine($"file size: {fileSize:F2}kb");

        TaskTool.NewLine("copying to clipboard..");
        await Clipboard.SetTextAsync(logStringContent);

        DialogTool.Show("SMAPI Log", $"Log has been copied to clipboard and is available locally at:\n{logFilePath}");
    }
}
