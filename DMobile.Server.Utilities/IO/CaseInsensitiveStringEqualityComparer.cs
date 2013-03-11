using System;
using System.Collections.Generic;

namespace DMobile.Server.Utilities.IO
{
    public class CaseInsensitiveStringEqualityComparer : IEqualityComparer<string>
    {
        #region Public Function

        public bool Equals(string x, string y)
        {
            bool isEqual = String.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;

            return isEqual;
        }

        public int GetHashCode(string obj)
        {
            int hashCode = -1;

            hashCode = obj.ToUpper().GetHashCode();

            return hashCode;
        }

        #endregion
    }
}