using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiddleTestAuthetication.Util
{
    public class ConvertAll
    {
        public static string ConvertIntStatusToString(int intStatus)
        {
            string stringStatus;

            switch (intStatus)
            {
                case -1:
                    stringStatus = "Deleted";
                    break;
                case 0:
                    stringStatus = "Inactive";
                    break;
                case 1:
                    stringStatus = "Active";
                    break;
                default:
                    stringStatus = "Null";
                    break;
            }

            return stringStatus;
        }
    }
}