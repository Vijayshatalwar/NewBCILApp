using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using GreenplyCommServer.Common;
using GreenplyCommServer;
using TEST;

namespace GreenplyCommServer.BI
{
    class _BClsLogin
    {
        //BcilLib.BcilLogger _obj = new BcilLib.BcilLogger();
        //LogFile _obj;


        public _BClsLogin()
        {
            //_obj = new LogFile();
           // _obj = new BcilLib.BcilLogger();
        }


       public string CheckValidUser(string UserName, string UserPass)
       {
            string _Str = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "_UpdateClientCount", "sent data =>" + UserName + "," + UserPass);
            //_obj.LogMessage(EventNotice.EventTypes.evtError , "LOGIN", "sent data =>" + UserName + "," + UserPass);
            string _s=  VariableInfo.EncryptPassword(UserPass.Trim(), "E");
            try
            {
                SqlParameter[] parma = { 
                                          new SqlParameter("@Type","D_VALIDATEUSER"),
                                          new SqlParameter("@UserID",UserName),
                                          new SqlParameter("@Password",_s),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_UserMaster", parma);
              //_obj.LogMessage(EventNotice.EventTypes.evtInfo, "LOGIN", "Response data =>" + dt.Rows[0]["ACTIVE"].ToString());
                if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0]["ACTIVE"].ToString() == "True")
                    //{
                        _Str = "LOGIN ~ SUCCESS ~ " + dt.Rows[0][5].ToString();
                    //}
                    //else
                    //{
                    //    _Str = "LOGIN ~ ERROR" + " ~ USER is DeActive";
                    //}
                    
                }
                else
                {
                    _Str = "LOGIN ~ ERROR" + " ~ INVALID USER";
                }
            }
            catch (Exception ex)
            {
                _Str = "ERROR ~ " + ex.ToString();
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "CheckValidUser", ex.Message.ToString());
                throw ex;
            }
            return _Str;
        }

       internal string GetUserRights(string UserID)
       {
           string _sResult = string.Empty;
           VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + UserID);
           try
           {
               SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETANDROIDUSERRIGHTS"),
                                        new SqlParameter("@UserID", UserID),
                                   };
               DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_UserMaster", parma);
               VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Response data =>" + dt.Rows[0][0].ToString());
               if (dt.Columns.Count > 1 && dt.Rows.Count > 0)
               {
                   _sResult = "GETANDROIDUSERRIGHTS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                   return _sResult;
               }
               else
               {
                   _sResult = "GETANDROIDUSERRIGHTS ~ ERROR ~ " + "NOT FOUND";
               }
           }
           catch (Exception ex)
           {
               VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetUserRights", ex.Message.ToString());
               throw ex;
           }
           return _sResult;
       }

      
    }
}
