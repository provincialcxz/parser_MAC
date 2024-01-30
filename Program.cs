using System.Diagnostics;
using System.Net;
using HtmlAgilityPack;

class Program
{
    public const string network = "yourNetworkConnectionName";
    static void Main()
    {
        string MAC = GetCurrentMacAddress();

        string filePath = "Path to your HTML file";

        string url = "your URL";

        string[] macAddresses = GetMacAddressesFromHtml(filePath);

        foreach (string macAddress in macAddresses)
        {
            string originalMacAddress = GetCurrentMacAddress();

            string newMacAddress = macAddress;

            string powershellCommand = $"Get-NetAdapter | Where-Object {{$_.Name -eq '{network}'}} | Set-NetAdapter -MacAddress {newMacAddress}";

            ExecuteCommand("powershell.exe", powershellCommand);

            string response = SendGetRequest(url);

            if (!string.IsNullOrEmpty(response))
            {
                Console.WriteLine($"MAC address changed from {originalMacAddress} on {newMacAddress}. Received a response from the site: {response}");
            }
            else
            {
                Console.WriteLine($"Could not get a response from the site after changing the MAC address {newMacAddress}.");
            }

            SetMacAddress(network, originalMacAddress);
        }

        SetMacAddress(network, MAC);
    }

    static string[] GetMacAddressesFromHtml(string filePath)
    {
        HtmlDocument htmlDoc = new HtmlDocument();

        htmlDoc.Load(filePath);

        var macAddressNodes = htmlDoc.DocumentNode.SelectNodes("//td[5]");

        if (macAddressNodes == null)
        {
            Console.WriteLine("MAC addresses not found in HTML file.");
            return new string[0];
        }

        string[] macAddresses = macAddressNodes.Select(node => node.InnerText.Trim()).ToArray();

        macAddresses = macAddresses.Where(mac => !string.IsNullOrEmpty(mac)).ToArray();

        return macAddresses;
    }

    static string GetCurrentMacAddress()
    {
        string powershellCommand = $"(Get-NetAdapter | Where-Object {{$_.Name -eq '{network}'}}).MacAddress";
        return ExecuteCommand("powershell.exe", powershellCommand).Trim();
    }

    static void SetMacAddress(string networkConnectionName, string newMacAddress)
    {
        string powershellCommand = $"Get-NetAdapter | Where-Object {{$_.Name -eq '{networkConnectionName}'}} | Set-NetAdapter -MacAddress {newMacAddress}";
        ExecuteCommand("powershell.exe", powershellCommand);
    }

    static string SendGetRequest(string url)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    return reader.ReadToEnd();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending request: " + ex.Message);
            return null;
        }
    }

    static string ExecuteCommand(string command, string arguments)
    {
        Process process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return output;
    }
}