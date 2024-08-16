//---------------------------------------------------------------------------
using System.Collections.Generic;
using System.Text;
using GreenplyScannerCommServer.Common;
using System.Net;
using BCILLogger;
using MOXA_CSharp_MXIO;
using System;
using GreenplyScannerCommServer;
using System.Windows.Forms;

//---------------------------------------------------------------------------
namespace MOXA_CSharp_MXIO
{
    class ioLogik
    {
        public const UInt16 Port = 502;						//Modbus TCP port
        public const UInt16 DO_SAFE_MODE_VALUE_OFF = 0;
        public const UInt16 DO_SAFE_MODE_VALUE_ON = 1;
        public const UInt16 DO_SAFE_MODE_VALUE_HOLD_LAST = 2;

        public const UInt16 DI_DIRECTION_DI_MODE = 0;
        public const UInt16 DI_DIRECTION_COUNT_MODE = 1;
        public const UInt16 DO_DIRECTION_DO_MODE = 0;
        public const UInt16 DO_DIRECTION_PULSE_MODE = 1;

        public const UInt16 TRIGGER_TYPE_LO_2_HI = 0;
        public const UInt16 TRIGGER_TYPE_HI_2_LO = 1;
        public const UInt16 TRIGGER_TYPE_BOTH = 2;
        //A-OPC Server response W5340 Device STATUS information data filed index
        public const int IP_INDEX = 0;
        public const int MAC_INDEX = 4;

        public string Obj_ItmStatus = "";


        //===================================================================
        public static void CheckErr(int iRet, string szFunctionName)
        {
            try
            {
                string szErrMsg = "MXIO_OK";

                if (iRet != MXIO_CS.MXIO_OK)
                {

                    switch (iRet)
                    {
                        case MXIO_CS.ILLEGAL_FUNCTION:
                            szErrMsg = "ILLEGAL_FUNCTION";
                            break;
                        case MXIO_CS.ILLEGAL_DATA_ADDRESS:
                            szErrMsg = "ILLEGAL_DATA_ADDRESS";
                            break;
                        case MXIO_CS.ILLEGAL_DATA_VALUE:
                            szErrMsg = "ILLEGAL_DATA_VALUE";
                            break;
                        case MXIO_CS.SLAVE_DEVICE_FAILURE:
                            szErrMsg = "SLAVE_DEVICE_FAILURE";
                            break;
                        case MXIO_CS.SLAVE_DEVICE_BUSY:
                            szErrMsg = "SLAVE_DEVICE_BUSY";
                            break;
                        case MXIO_CS.EIO_TIME_OUT:
                            szErrMsg = "EIO_TIME_OUT";
                            break;
                        case MXIO_CS.EIO_INIT_SOCKETS_FAIL:
                            szErrMsg = "EIO_INIT_SOCKETS_FAIL";
                            break;
                        case MXIO_CS.EIO_CREATING_SOCKET_ERROR:
                            szErrMsg = "EIO_CREATING_SOCKET_ERROR";
                            break;
                        case MXIO_CS.EIO_RESPONSE_BAD:
                            szErrMsg = "EIO_RESPONSE_BAD";
                            break;
                        case MXIO_CS.EIO_SOCKET_DISCONNECT:
                            szErrMsg = "EIO_SOCKET_DISCONNECT";
                            break;
                        case MXIO_CS.PROTOCOL_TYPE_ERROR:
                            szErrMsg = "PROTOCOL_TYPE_ERROR";
                            break;
                        case MXIO_CS.SIO_OPEN_FAIL:
                            szErrMsg = "SIO_OPEN_FAIL";
                            break;
                        case MXIO_CS.SIO_TIME_OUT:
                            szErrMsg = "SIO_TIME_OUT";
                            break;
                        case MXIO_CS.SIO_CLOSE_FAIL:
                            szErrMsg = "SIO_CLOSE_FAIL";
                            break;
                        case MXIO_CS.SIO_PURGE_COMM_FAIL:
                            szErrMsg = "SIO_PURGE_COMM_FAIL";
                            break;
                        case MXIO_CS.SIO_FLUSH_FILE_BUFFERS_FAIL:
                            szErrMsg = "SIO_FLUSH_FILE_BUFFERS_FAIL";
                            break;
                        case MXIO_CS.SIO_GET_COMM_STATE_FAIL:
                            szErrMsg = "SIO_GET_COMM_STATE_FAIL";
                            break;
                        case MXIO_CS.SIO_SET_COMM_STATE_FAIL:
                            szErrMsg = "SIO_SET_COMM_STATE_FAIL";
                            break;
                        case MXIO_CS.SIO_SETUP_COMM_FAIL:
                            szErrMsg = "SIO_SETUP_COMM_FAIL";
                            break;
                        case MXIO_CS.SIO_SET_COMM_TIME_OUT_FAIL:
                            szErrMsg = "SIO_SET_COMM_TIME_OUT_FAIL";
                            break;
                        case MXIO_CS.SIO_CLEAR_COMM_FAIL:
                            szErrMsg = "SIO_CLEAR_COMM_FAIL";
                            break;
                        case MXIO_CS.SIO_RESPONSE_BAD:
                            szErrMsg = "SIO_RESPONSE_BAD";
                            break;
                        case MXIO_CS.SIO_TRANSMISSION_MODE_ERROR:
                            szErrMsg = "SIO_TRANSMISSION_MODE_ERROR";
                            break;
                        case MXIO_CS.PRODUCT_NOT_SUPPORT:
                            szErrMsg = "PRODUCT_NOT_SUPPORT";
                            break;
                        case MXIO_CS.HANDLE_ERROR:
                            szErrMsg = "HANDLE_ERROR";
                            break;
                        case MXIO_CS.SLOT_OUT_OF_RANGE:
                            szErrMsg = "SLOT_OUT_OF_RANGE";
                            break;
                        case MXIO_CS.CHANNEL_OUT_OF_RANGE:
                            szErrMsg = "CHANNEL_OUT_OF_RANGE";
                            break;
                        case MXIO_CS.COIL_TYPE_ERROR:
                            szErrMsg = "COIL_TYPE_ERROR";
                            break;
                        case MXIO_CS.REGISTER_TYPE_ERROR:
                            szErrMsg = "REGISTER_TYPE_ERROR";
                            break;
                        case MXIO_CS.FUNCTION_NOT_SUPPORT:
                            szErrMsg = "FUNCTION_NOT_SUPPORT";
                            break;
                        case MXIO_CS.OUTPUT_VALUE_OUT_OF_RANGE:
                            szErrMsg = "OUTPUT_VALUE_OUT_OF_RANGE";
                            break;
                        case MXIO_CS.INPUT_VALUE_OUT_OF_RANGE:
                            szErrMsg = "INPUT_VALUE_OUT_OF_RANGE";
                            break;
                    }

                    Console.WriteLine("Function \"{0}\" execution Fail. Error Message : {1}\n", szFunctionName, szErrMsg);

                    if (iRet == MXIO_CS.EIO_TIME_OUT || iRet == MXIO_CS.HANDLE_ERROR)
                    {
                        MessageBox.Show("The connection of application with Moxa device not properly, Kindly check all the possibilities.");
                        MXIO_CS.MXEIO_Exit();
                        //Console.WriteLine("Press any key to close application\r\n");
                        Console.ReadLine();
                        Environment.Exit(1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void EthernetDIRead_Main(string MoxaIP)
        {
            try
            {
                int ret;
                Int32[] hConnection = new Int32[1];
                string IPAddr = MoxaIP;
                string Password = "";
                UInt32 Timeout = Convert.ToUInt32(GreenplyScannerCommServer.Properties.Settings.Default.Moxa_Timeout);
                UInt32 i;
                //UInt16 wIndex;
                //UInt32 uiGetInput = 0;
                {
                    ret = MXIO_CS.MXIO_GetDllVersion();
                    //Console.WriteLine("MXIO_GetDllVersion:{0}.{1}.{2}.{3}", (ret >> 12) & 0xF, (ret >> 8) & 0xF, (ret >> 4) & 0xF, (ret) & 0xF);

                    ret = MXIO_CS.MXIO_GetDllBuildDate();
                    //Console.WriteLine("MXIO_GetDllBuildDate:{0:x}/{1:x}/{2:x}", (ret >> 16), (ret >> 8) & 0xFF, (ret) & 0xFF);
                    //--------------------------------------------------------------------------
                    ret = MXIO_CS.MXEIO_Init();

                    ret = MXIO_CS.MXEIO_E1K_Connect(System.Text.Encoding.UTF8.GetBytes(IPAddr), Port, Timeout, hConnection, System.Text.Encoding.UTF8.GetBytes(Password));
                    CheckErr(ret, "MXEIO_E1K_Connect");
                    if (ret == MXIO_CS.MXIO_OK)
                    {
                        //objLog.WriteLog("Moxa connection is OK");
                    }
                    //--------------------------------------------------------------------------
                    //Check Connection
                    byte[] bytCheckStatus = new byte[1];
                    ret = MXIO_CS.MXEIO_CheckConnection(hConnection[0], Timeout, bytCheckStatus);
                    CheckErr(ret, "MXEIO_CheckConnection");
                    if (ret == MXIO_CS.MXIO_OK)
                    {
                        switch (bytCheckStatus[0])
                        {
                            case MXIO_CS.CHECK_CONNECTION_OK:
                                Console.WriteLine("MXEIO_CheckConnection: Check connection ok => {0}", bytCheckStatus[0]);
                                break;
                            case MXIO_CS.CHECK_CONNECTION_FAIL:
                                Console.WriteLine("MXEIO_CheckConnection: Check connection fail => {0}", bytCheckStatus[0]);
                                break;
                            case MXIO_CS.CHECK_CONNECTION_TIME_OUT:
                                Console.WriteLine("MXEIO_CheckConnection: Check connection time out => {0}", bytCheckStatus[0]);
                                break;
                            default:
                                Console.WriteLine("MXEIO_CheckConnection: Check connection status unknown => {0}", bytCheckStatus[0]);
                                break;
                        }
                    }
                    //--------------------------------------------------------------------------
                    //Get firmware Version
                    byte[] bytRevision = new byte[4];
                    ret = MXIO_CS.MXIO_ReadFirmwareRevision(hConnection[0], bytRevision);
                    CheckErr(ret, "MXIO_ReadFirmwareRevision");
                    if (ret == MXIO_CS.MXIO_OK)
                        Console.WriteLine("MXIO_ReadFirmwareRevision:V{0}.{1}, Release:{2}, build:{3}", bytRevision[0], bytRevision[1], bytRevision[2], bytRevision[3]);
                    //--------------------------------------------------------------------------
                    //Get firmware Release Date
                    UInt16[] wGetFirmwareDate = new UInt16[2];
                    ret = MXIO_CS.MXIO_ReadFirmwareDate(hConnection[0], wGetFirmwareDate);
                    CheckErr(ret, "MXIO_ReadFirmwareDate");
                    if (ret == MXIO_CS.MXIO_OK)
                        Console.WriteLine("MXIO_ReadFirmwareDate:{0:x}/{1:x}/{2:x}", wGetFirmwareDate[1], (wGetFirmwareDate[0] >> 8) & 0xFF, (wGetFirmwareDate[0]) & 0xFF);
                    //--------------------------------------------------------------------------
                    //Get Module Type
                    UInt16[] wModuleType = new UInt16[1];
                    ret = MXIO_CS.MXIO_GetModuleType(hConnection[0], 0, wModuleType);
                    CheckErr(ret, "MXIO_GetModuleType");
                    if (ret == MXIO_CS.MXIO_OK)
                        Console.WriteLine("MXIO_GetModuleType: Module Type = {0:x}", wModuleType[0]);
                    //--------------------------------------------------------------------------
                    byte bytCount = 0, bytStartChannel = 0;
                    //Get DIO Direction Status
                    UInt16[] wGetDOMode = new UInt16[4];        //Get DO Direction Mode
                    UInt16[] wSetDO_DOMode = new UInt16[2];     //Set DO Direction DO Mode
                    UInt16[] wSetDO_PulseMode = new UInt16[2];  //Set DO Direction Pulse Mode
                    UInt16[] wGetDIMode = new UInt16[8];        //Get DI Direction Mode
                    UInt16[] wSetDI_DIMode = new UInt16[4];     //Set DI Direction DI Mode
                    UInt16[] wSetDI_CounterMode = new UInt16[4];//Set DI Direction Counter Mode
                    UInt16 wDI_DI_MODE = 0;                     //DI Direction DI Mode Value
                    UInt16 wDO_DO_MODE = 0;                     //DO Direction DO Mode Value

                    Int32 dwShiftValue;
                    //Set Ch0~ch3 DI Direction DI Mode
                    bytCount = 4;
                    bytStartChannel = 0;
                    for (i = 0; i < bytCount; i++)
                        wSetDI_DIMode[i] = DI_DIRECTION_DI_MODE;

                    ret = MXIO_CS.E1K_DI_SetModes(hConnection[0], bytStartChannel, bytCount, wSetDI_DIMode);
                    CheckErr(ret, "E1K_DI_SetModes");
                    if (ret == MXIO_CS.MXIO_OK)
                        Console.WriteLine("E1K_DI_SetModes Set Ch{0}~ch{1} DI Direction DI Mode Succcess.", bytStartChannel, bytCount - 1);
                    //Get Ch0~ch3 DI Direction Mode
                    bytCount = 4;
                    bytStartChannel = 0;
                    ret = MXIO_CS.E1K_DI_GetModes(hConnection[0], bytStartChannel, bytCount, wGetDIMode);
                    CheckErr(ret, "E1K_DI_GetModes");
                    if (ret == MXIO_CS.MXIO_OK)
                    {
                        Console.WriteLine("E1K_DI_GetModes Get Ch{0}~ch{1} DI Direction Mode success.", bytStartChannel, bytCount + bytStartChannel - 1);
                        for (i = 0; i < bytCount; i++)
                            Console.WriteLine("ch{0}={1}", i + bytStartChannel, (wGetDIMode[i] == wDI_DI_MODE) ? "DI_MODE" : "COUNT_MODE");
                    }
                    //*******************
                    // Set/Get DI filter
                    //*******************
                    //Set Ch0~ch3 DI Direction Filter
                    bytCount = 4;
                    bytStartChannel = 0;
                    UInt16[] wFilter = new UInt16[4];
                    for (i = 0; i < bytCount; i++)
                        wFilter[i] = (UInt16)(5 + i);
                    ret = MXIO_CS.E1K_DI_SetFilters(hConnection[0], bytStartChannel, bytCount, wFilter);
                    CheckErr(ret, "E1K_DI_SetFilters");
                    if (ret == MXIO_CS.MXIO_OK)
                        Console.WriteLine("E1K_DI_SetFilters Set Ch{0}~ch{1} DI Direction Filter to 5 return {2}", bytStartChannel, bytCount + bytStartChannel - 1, ret);
                    //Get Ch0~ch3 DI Direction Filter
                    bytCount = 4;
                    bytStartChannel = 0;
                    ret = MXIO_CS.E1K_DI_GetFilters(hConnection[0], bytStartChannel, bytCount, wFilter);
                    CheckErr(ret, "E1K_DI_GetFilters");
                    if (ret == MXIO_CS.MXIO_OK)
                    {
                        Console.WriteLine("E1K_DI_GetFilters Get Ch{0}~ch{1} DI Direction Filter return {2}", bytStartChannel, bytCount + bytStartChannel - 1, ret);
                        for (i = 0, dwShiftValue = 0; i < bytCount; i++, dwShiftValue++)
                            Console.WriteLine("DI Filter Value: ch[{0}] = {1}", i, wFilter[i]);
                    }
                    //***************
                    // DI Read Value
                    //***************
                    //Get Ch0~ch3 DI Direction DI Mode DI Value
                    bytCount = 4;
                    bytStartChannel = 0;
                    UInt32[] dwGetDIValue = new UInt32[1];
                    ret = MXIO_CS.E1K_DI_Reads(hConnection[0], bytStartChannel, bytCount, dwGetDIValue);
                    CheckErr(ret, "E1K_DI_Reads");
                    if (ret == MXIO_CS.MXIO_OK)
                    {
                        Console.WriteLine("E1K_DI_Reads Get Ch0~ch3 DI Direction DI Mode DI Value success.");
                        for (i = 0, dwShiftValue = 0; i < bytCount; i++, dwShiftValue++)
                            Console.WriteLine("DI vlaue: ch[{0}] = {1}", i + bytStartChannel, ((dwGetDIValue[0] & (1 << dwShiftValue)) == 0) ? "OFF" : "ON");
                        if (dwGetDIValue[0] == 0)
                        {
                            Obj_ItmStatus = "OFF";
                            //objLog.WriteLog("OFF");
                        }
                        else
                        {
                            Obj_ItmStatus = "ON";
                            //objLog.WriteLog("ON");
                        }
                    }
                    //--------------------------------------------------------------------------
                    //End Application
                    ret = MXIO_CS.MXEIO_Disconnect(hConnection[0]);
                    CheckErr(ret, "MXEIO_Disconnect");
                    if (ret == MXIO_CS.MXIO_OK)
                        Console.WriteLine("MXEIO_Disconnect return {0}", ret);
                    //--------------------------------------------------------------------------
                    MXIO_CS.MXEIO_Exit();
                    Console.WriteLine("MXEIO_Exit, Press Enter To Exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "EthernetDIRead_Main", exDetail);
            }
        }
    }
}
