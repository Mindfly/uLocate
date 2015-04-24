using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Helpers
{
    using uLocate.Data;
    using uLocate.Persistance;

    using Umbraco.Core.Persistence;

    internal static class DataValuesHelper
    {
        /// <summary>
        /// Looks up the Id number for a given datatype's prevalue text string
        /// </summary>
        /// <param name="DataTypeId">
        /// The data type id.
        /// </param>
        /// <param name="PrevalueText">
        /// The prevalue text.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static int GetPreValueId(int DataTypeId, string PrevalueText)
        {
            var allDataTypePrevals = GetAllPrevaluesForDataType(DataTypeId);

            var match = allDataTypePrevals.Where(n => n.Value == PrevalueText).FirstOrDefault();
            if (match != null)
            {
                return match.Id;
            }
            else
            {
                return 0;
            }
        }

        public static string GetPreValueIds(int DataTypeId, string PrevalueText)
        {
            var returnString =",";
            var allDataTypePrevals = GetAllPrevaluesForDataType(DataTypeId);

            var parsedValues = ParseMultiSelectValues(PrevalueText);

            foreach (var val in parsedValues)
            {
                var match = allDataTypePrevals.Where(n => n.Value == val).FirstOrDefault();
                if (match != null)
                {
                    returnString = string.Concat(returnString, ",", match.Id.ToString());
                }
            }

            returnString = returnString.Replace(",,","");
            returnString = string.Concat("[", returnString, "]");

            return returnString;
        }

        internal static IEnumerable<string> ParseMultiSelectValues(string Values)
        {
            var returnValues = new List<string>();

            var valuesCleaned = Values;
            valuesCleaned = valuesCleaned.Replace("[", "");
            valuesCleaned = valuesCleaned.Replace("]", "");
            //valuesCleaned = valuesCleaned.Replace("\"", "");

            var parsedValues = valuesCleaned.Split(',');

            foreach (var val in parsedValues)
            {
                var valStripped = val.Trim();
                valStripped = valStripped.TrimStart('\"');
                valStripped = valStripped.TrimEnd('\"');
          
                returnValues.Add(valStripped);
            }

            return returnValues;
        }

        internal static List<cmsDataTypePreValuesDto> GetAllPrevaluesForDataType(int DataTypeId)
        {
            var sql = new Sql();
            sql
                .Select("*")
                .From<cmsDataTypePreValuesDto>()
                .Where<cmsDataTypePreValuesDto>(n => n.DataTypeNodeId == DataTypeId);

            return Repositories.ThisDb.Fetch<cmsDataTypePreValuesDto>(sql);
        }

        /// <summary>
        /// Trims a number to a specified length of characters 
        /// </summary>
        /// <param name="Dbl">
        /// a Double numeric
        /// </param>
        /// <param name="TotalChars">
        /// The total characters for the returned string
        /// </param>
        /// <param name="ExcludeSign">
        /// If the number has a negative sign, should it be excluded from the character count? 
        /// </param>
        /// <param name="PadWithZeros">
        /// Id the number is shorter than the specified character length, should it be padded with zeros at the end?
        /// </param>
        /// <returns>
        /// A <see cref="string"/> representing the trimmed number.
        /// </returns>
        public static string TrimToSize(this double Dbl, int TotalChars, bool ExcludeSign = true, bool PadWithZeros = true)
        {
            string trimmed = "";
            string original = Dbl.ToString();
            string eval = "";

            int testChars = 0;

            //Determine the allowed total characters for this number and options
            if (!original.Contains("-") || !ExcludeSign)
            {
                testChars = TotalChars;
            }
            else if (original.Contains("-"))
            {
                testChars = TotalChars + 1;
            }

            // Evaluate current size
            if (original.Length == testChars)
            {
                //Already perfect
                return original;
            }
            else if (original.Length < testChars)
            {
                eval = "TooShort";
            }
            else if (original.Length > testChars)
            {
                eval = "TooLong";
            }

            // Fix based on Evaluation
            if (eval == "TooShort")
            {
                if (PadWithZeros)
                {
                    trimmed = original.PadRight(testChars).Replace(" ", "0");
                }
                else
                {
                    trimmed = original;
                }
            }
            else if (eval == "TooLong")
            {
                trimmed = original.Remove(testChars);
            }
            else
            {
                // who knows what happened!
                trimmed = "";
            }

            return trimmed;
        }
    }
}
