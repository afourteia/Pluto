namespace Pluto;
using NITGEN.SDK.NBioBSP;

public class GlobalConfig
{
    // Bio Device 
    public static NBioAPI m_NBioAPI { get; private set; }
    public static NBioAPI.Export m_Export { get; private set; }
    public static NBioAPI.IndexSearch m_IndexSearch { get; set; }

    public static short m_OpenedDeviceID { get; set; }
    public static NBioAPI.Type.HFIR m_hNewFIR { get; set; }



    public static bool InitBioDevice()
    {

        bool result;
        short iDeviceID = NBioAPI.Type.DEVICE_ID.AUTO;
        if (!BioInitialize())
            return false;

        // Select device type
        iDeviceID = NBioAPI.Type.DEVICE_ID.AUTO;

        // Open device
        var ret = m_NBioAPI.OpenDevice(iDeviceID);
        if (ret == NBioAPI.Error.NONE)
        {

            GlobalConfig.m_OpenedDeviceID = iDeviceID;
            result = true;
        }
        else
        {
            return false;
        }
        return result;
    }


    public static bool BioInitialize()
    {
        try
        {
            m_NBioAPI = new NBioAPI();
            m_Export = new NBioAPI.Export(m_NBioAPI);
            m_OpenedDeviceID = NBioAPI.Type.DEVICE_ID.NONE;
            m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            m_hNewFIR = null;

            // initEngine
            uint ret = GlobalConfig.m_IndexSearch.InitEngine();
            if (ret != NBioAPI.Error.NONE)
            {
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


}
