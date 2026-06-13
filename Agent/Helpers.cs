using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace DefaultNamespace;

public static class Helpers
{ 
    public static string GetSha256(string filePath)
    {
        using var sha = SHA256.Create();
        using var stream = File.OpenRead(filePath);

        var hash = sha.ComputeHash(stream);

        return Convert.ToHexString(hash);
    }
    
    public static int GetSubdirectoriesCount(string path, bool recursive = true)
    {
        try
        {
            return Directory.EnumerateDirectories(
                path,
                "*",
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly
            ).Count();
        }
        catch
        {
            return 0;
        }
    }
    
    public static string GetLocalIPv4()
    {
        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus != OperationalStatus.Up)
                continue;

            if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                continue;

            var ipProps = ni.GetIPProperties();

            foreach (var addr in ipProps.UnicastAddresses)
            {
                if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    return addr.Address.ToString();
            }
        }

        return "0.0.0.0";
    }
    
    public static string GetMacAddress()
    {
        var nic = NetworkInterface
            .GetAllNetworkInterfaces()
            .FirstOrDefault(n =>
                n.OperationalStatus == OperationalStatus.Up &&
                n.NetworkInterfaceType != NetworkInterfaceType.Loopback);

        return nic?.GetPhysicalAddress().ToString() ?? "UNKNOWN";
    }
    
    public static long GetDirectorySize(string path)
    {
        try
        {
            long size = 0;

            foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
            {
                try
                {
                    var info = new FileInfo(file);
                    size += info.Length;
                }
                catch
                {
                    // ignora arquivos inacessíveis
                }
            }

            return size;
        }
        catch
        {
            return 0;
        }
    }
}