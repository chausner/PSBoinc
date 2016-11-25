using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace PSBoinc
{
    class Utils
    {
        public static T[] FilterByName<T>(T[] elements, Func<T, string> key, string[] patterns, string errorMessage, string errorID, Cmdlet cmdlet)
        {
            List<T> filtered = new List<T>();

            foreach (string pattern in patterns)
            {
                WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);

                bool match = false;

                foreach (T element in elements)
                    if (wildcardPattern.IsMatch(key(element)))
                    {
                        if (!filtered.Contains(element))
                            filtered.Add(element);
                        match = true;
                    }

                if (!match && !WildcardPattern.ContainsWildcardCharacters(pattern))
                    cmdlet.WriteError(new ErrorRecord(new Exception(string.Format(errorMessage, pattern)), errorID, ErrorCategory.ObjectNotFound, pattern));
            }

            return filtered.ToArray();
        }
    }
}
