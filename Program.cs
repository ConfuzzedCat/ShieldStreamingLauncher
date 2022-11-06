using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ShieldStreamingLauncher
{
    internal class Program
    {
        const uint SPI_GETMOUSE = 0x0003;
        const uint SPI_SETMOUSE = 0x0004;
        const uint SPIF_SENDCHANGE = 0x02;
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(10000);
            
            DisableMouseAccel();
            string playnitePath = Path.Combine(Directory.GetCurrentDirectory(), "Playnite.DesktopApp.exe");
            try
            {

                if (File.Exists(playnitePath))
                {
                    Process p = new();
                    p.StartInfo.FileName = playnitePath;
                    p.Start();
                    p.WaitForExit();
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        private static void DisableMouseAccel()
        {
            int[] mouseParams = new int[3];
            GCHandle handle = GCHandle.Alloc(mouseParams, GCHandleType.Pinned);
            try
            {
                IntPtr pointer = handle.AddrOfPinnedObject();
                if (SystemParametersInfo(SPI_GETMOUSE, 0, pointer, 0))
                {
                    mouseParams[2] = 0; // 0 = false?
                    if (!SystemParametersInfo(SPI_SETMOUSE, 0, pointer, SPIF_SENDCHANGE))
                    {
                        Console.WriteLine();
                    }
                }
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
        }
    }
}