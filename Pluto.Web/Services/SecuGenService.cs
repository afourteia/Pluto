namespace Pluto;
using SecuGen.FDxSDKPro.Windows;
public class SecuGenService
{
    private readonly ILogger<NitgenService> _logger;
    private readonly SGFingerPrintManager m_FPM;

    public uint NumDevice { get; set; }
    public short[]? DeviceIDs { get; set; }

    public SecuGenService(ILogger<NitgenService> logger)
    {
        m_FPM = new SGFingerPrintManager();
        m_FPM.Init(SGFPMDeviceName.DEV_FDU03);
        _logger = logger;
        _logger.LogInformation("SecuGenService is being initialized.");
    }

    public string GetVersion()
    {
        return "strVersion";
    }

    // public NBioAPI.Type.INIT_INFO_0 GetInfo()
    // {
    //     // m_NBioAPI.GetInitInfo(out NBioAPI.Type.INIT_INFO_0 InitInfo);
    //     return InitInfo;
    // }

    // Return Value Type: NBioAPI.Error
    // public uint SetInitInfo(NBioAPI.Type.INIT_INFO_0 InitInfo)
    // {
    //     return m_NBioAPI.SetInitInfo(InitInfo);
    // }

    public uint GetDevices()
    {
        return 1;
    }

    public bool GetDeviceInfo()
    {
        SGFPMDeviceInfoParam pInfo = new SGFPMDeviceInfoParam();
        pInfo = new SGFPMDeviceInfoParam();
        Int32 iError = m_FPM.GetDeviceInfo(pInfo);
        if (iError == (Int32)SGFPMError.ERROR_NONE)
        {
            // This should be done GetDeviceInfo();
            _logger.LogInformation("Image Width: {0}", pInfo.ImageWidth);
            _logger.LogInformation("Image Height: {0}", pInfo.ImageHeight);
            _logger.LogInformation("ImageDPI: {0}", pInfo.ImageDPI);
            _logger.LogInformation("Serial Number: {0}", pInfo.DeviceSN);
            _logger.LogInformation("FW Version: {0}", pInfo.FWVersion);
            _logger.LogInformation("Device ID: {0}", pInfo.DeviceID);
            _logger.LogInformation("ComPort: {0}", pInfo.ComPort);
        }
        return true;
    }

    public uint SetDeviceInfo()
    {
        return 0;
    }

    public bool OpenDevice()
    {
        // Get DeviceID
        // EnumerateDevice function can be used to retrieve the device ID
        _logger.LogInformation("Opening device...");

        var ret = m_FPM.OpenDevice(0x255); // TODO: look for SGFPMPortAddr.USB_AUTO_DETECT
        _logger.LogInformation("Device opening ret: {0}", ret);
        if (ret == (Int32)SGFPMError.ERROR_NONE)
            return true;
        else
            return false;
    }

    public bool CloseDevice()
    {
        _logger.LogInformation("Closing device...");
        return true;
    }

    // Adjust device brightness
    public uint AdjustDevice() { return 0; }

    public short GetOpenedDeviceID()
    {
        return 1;
    }

    public uint CheckFinger()
    {
        return 1;
    }

    public string Capture()
    {
        Byte[] fp_image = new Byte[300 * 260];
        int iError;
        int img_qlty = 0;
        iError = m_FPM.GetImage(fp_image);
        if (iError == (Int32)SGFPMError.ERROR_NONE)
        {
            m_FPM.GetImageQuality(260, 300, fp_image, ref img_qlty);
            return "quality is: " + img_qlty.ToString();
        }
        else

            return "Error: " + iError;

    }

    public bool SetBrightness(int brightness)
    {
        int iError = m_FPM.SetBrightness(brightness);
        if (iError == (Int32)SGFPMError.ERROR_NONE)
            return true;
        else
            return false;
    }

    public bool Configure()
    {
        HandleEventDelegate eventDelegate = this.HandleEvent;
        int iError = m_FPM.Configure((long)eventDelegate);
        if (iError == (Int32)SGFPMError.ERROR_NONE)
            return true;
        else
            return false;
    }

    public delegate long HandleEventDelegate(long e);
    private long HandleEvent(long e)
    {
        _logger.LogInformation("Event handled.");
        return 1;
    }

    public bool EnableAutoOnEvent()
    {
        int iError = m_FPM.EnableAutoOnEvent(true, (long)this.HandleEvent);
        return true;
    }
    public uint Process() { return 0; }
    public uint CreateTemplate() { return 0; }

    public uint VerifyMatch() { return 0; }

    public uint Enroll()
    {
        return 1;
    }

    public uint Verify()
    {
        return 1;
    }

    private uint VerifyInternal(dynamic inputFIR)
    {
        return 1;
    }

    ~SecuGenService()
    {
        _logger.LogInformation("SecuGenService is destroyed.");
    }
}
