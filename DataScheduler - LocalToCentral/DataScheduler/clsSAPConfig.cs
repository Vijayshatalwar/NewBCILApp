using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;

namespace DataScheduler
{
    class clsSAPConfig : IDestinationConfiguration
    {
        public RfcConfigParameters GetParameters(String DestinationName)
        {
            try
            {
                if ("PRD_000".Equals(DestinationName))
                {
                    RfcConfigParameters _params = new RfcConfigParameters();
                    _params.Add(RfcConfigParameters.AppServerHost, clsGlobal.PlantCode);
                    //_params.Add(RfcConfigParameters.SAPRouter, clsGlobal.mSapRtrStr);//added for remote connectivity on 20/05/2014
                    _params.Add(RfcConfigParameters.SystemNumber, clsGlobal.mSapSysNo);
                    _params.Add(RfcConfigParameters.User, clsGlobal.mServerName);
                    _params.Add(RfcConfigParameters.Password, clsGlobal.mDbPassword);
                    _params.Add(RfcConfigParameters.Client, clsGlobal.mDbPassword);
                    _params.Add(RfcConfigParameters.Language, clsGlobal.mSapLng);
                    _params.Add(RfcConfigParameters.PoolSize, "100");
                    _params.Add(RfcConfigParameters.PeakConnectionsLimit, "600");
                    _params.Add(RfcConfigParameters.IdleTimeout, "1000");
                    return _params;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("BCILComServer" + " :: GetParameters() " + ex.Message.ToString());
                return null;
            }
        }

        #region IDestinationConfiguration Members

        public bool ChangeEventsSupported()
        {
            return false;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        #endregion
    }
}
