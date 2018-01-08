using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MovieLibrary.Web.WebConstants;

namespace MovieLibrary.Web.Infrastructure.Extesions
{
    public static class TempDataDictionaryExtensions
    {
        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[MsgConstants.TempDataSuccessMessageKey] = message;
        }

        public static void AddErrorMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[MsgConstants.TempDataErrorMessageKey] = message;
        }
    }
}
