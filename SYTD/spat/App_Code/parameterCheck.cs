using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

/// <summary>
///parameterCheck 的摘要说明
/// </summary>
public class parameterCheck
{
    public parameterCheck()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public static bool isEmail(string emailString)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(emailString, "['\\w_-]+(\\.['\\w_-]+)*@['\\w_-]+(\\.['\\w_-]+)*\\.[a-zA-Z]{2,4}");
    }
    public static bool isInt(string intString)
    {
        if (intString == "")
        {
            return true;
        }
        return System.Text.RegularExpressions.Regex.IsMatch(intString, "^[0-9]*$");
        //return System.Text.RegularExpressions.Regex.IsMatch(intString, "^(\\d{5}-\\d{4})|(\\d{5})$");  ^[0-9]*$
    }
    public static bool isUSZip(string zipString)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(zipString, "^-[0-9]+$|^[0-9]+$");
    }

    public static bool isNumber(string numberString)
    {
        if (numberString == "")
        {
            return true;
        }
        return System.Text.RegularExpressions.Regex.IsMatch(numberString, "^-[0-9]+$|^[0-9]+$");
    }

    public static bool isType(string typeString)
    {
        if (typeString.ToUpper() != "LEAD" && typeString.ToUpper() != "UNIT")
        {
            return false;
        }
        return true;
    }

    public static bool isSubCode(string typeString)
    {
        if (isNumber(typeString) || typeString.ToLower() == "global")
        {
            return true;
        }
        return false;
    }
}
