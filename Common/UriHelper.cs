using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class UriHelper
    {
        //Pattern library and tools:
        // http://regexlib.com/Default.aspx

        //        foo://example.com:8042/over/there?name=ferret#nose
        //        \_/   \______________/\_________/\__________/ \__/
        //         |           |             |           |        |
        //      scheme     authority       path        query   fragment
        //        s0          a0            p0          q0        f0
        public const string Pattern_Uri = @"^(?<s1>(?<s0>[^:/\?#]+):)?(?<a1>"
                                                + @"//(?<a0>[^/\?#]*))?(?<p0>[^\?#]*)"
                                                + @"(?<q1>\?(?<q0>[^#]*))?"
                                                + @"(?<f1>#(?<f0>.*))?";

        /// <summary>
        /// Checks wether passed string is valid full url.
        /// </summary>
        /// <param name="strToCheck">Uri to check where protocol part must be included. Examples:
        /// '<example>http://www.mywebsite.com:8080/mywebapp/mydir/mypage.htm?mykey1=myvalue1&mykey2=myvalue2#myanchor</example>'
        /// '<example>http://www.mywebsite.com</example>'
        /// </param>
        /// <returns></returns>
        public static bool IsUri(String strToCheck)
        {
            Regex objAplhaPattern = new Regex(Pattern_Uri);
            return objAplhaPattern.IsMatch(strToCheck);
        }

        /// <summary>
        /// Gets protocol part.
        /// </summary>
        /// <param name="uri">Uri from which part should be extracted where protocol part must be included. Examples:
        /// '<example>http://www.mywebsite.com:8080/mywebapp/mydir/mypage.htm?mykey1=myvalue1&mykey2=myvalue2#myanchor</example>'
        /// '<example>http://www.mywebsite.com</example>'
        /// </param>
        /// <returns>Requested part. Example: http, https.<value>null</value> if uri is invalid or part was not found.</returns>
        public static string GetProtocol(string uri)
        {
            string protocol = null;
            Regex regex = new Regex(Pattern_Uri);
            Match match = regex.Match(uri);
            if (match.Success)
            {
                protocol = match.Groups["s0"].Value;
            }
            return protocol;
        }

        /// <summary>
        /// Gets authority part.
        /// </summary>
        /// <param name="uri">Uri from which part should be extracted where protocol part must be included. Examples:
        /// 'http://www.mywebsite.com:8080/mywebapp/mydir/mypage.htm?mykey1=myvalue1(and sign)mykey2=myvalue2#myanchor'
        /// 'https://www.mywebsite.com'
        /// </param>
        /// <returns>Requested part. Example: www.mywebsite.com:8080. <value>null</value> if uri is invalid or part was not found.</returns>
        public static string GetAuthority(string uri)
        {
            string authority = null;
            Regex regex = new Regex(Pattern_Uri);
            Match match = regex.Match(uri);
            if (match.Success)
            {
                authority = match.Groups["a0"].Value;
            }
            return authority;
        }

        /// <summary>
        /// Gets host part.
        /// </summary>
        /// <param name="uri">
        /// Uri from which part should be extracted where protocol part must be included. Examples: 
        /// 'http://192.168.0.1:8080/mywebapp/mydir/mypage.htm?mykey1=myvalue1(and sign)mykey2=myvalue2#myanchor',
        /// 'https://www.mywebsite.com'.
        /// </param>
        /// <returns>Requested part. Examples: '192.168.0.1', 'www.mywebsite.com'. <value>null</value> if uri is invalid or part was not found.</returns>
        public static string GetHost(string uri)
        {
            string host = null;
            Regex regex = new Regex(Pattern_Uri);
            Match match = regex.Match(uri);
            if (match.Success)
            {
                string authority = match.Groups["a0"].Value;
                if (!string.IsNullOrEmpty(authority))
                {
                    string[] parts = authority.Split(':');
                    if (parts.Length > 0)
                    {
                        host = parts[0];
                    }
                }
            }
            return host;
        }

        /// <summary>
        /// Gets port part.
        /// </summary>
        /// <param name="uri">
        /// Uri from which part should be extracted where protocol part must be included. Examples: 
        /// 'http://192.168.0.1:8080/mywebapp/mydir/mypage.htm?mykey1=myvalue1(and sign)mykey2=myvalue2#myanchor',
        /// 'https://www.mywebsite.com'.
        /// </param>
        /// <returns>Requested part. Example: '8080'. <value>null</value> if uri is invalid or part was not found.</returns>
        public static string GetPort(string uri)
        {
            string port = null;
            Regex regex = new Regex(Pattern_Uri);
            Match match = regex.Match(uri);
            if (match.Success)
            {
                string authority = match.Groups["a0"].Value;
                if (!string.IsNullOrEmpty(authority))
                {
                    string[] parts = authority.Split(':');
                    if (parts.Length > 1)
                    {
                        port = parts[1];
                    }
                }
            }
            return port;
        }

        /// <summary>
        /// Gets path part.
        /// </summary>
        /// <param name="uri">
        /// Uri from which path should be extracted where protocol part must be included. Examples: 
        /// 'http://192.168.0.1:8080/mywebapp/mydir/mypage.htm?mykey1=myvalue1(and sign)mykey2=myvalue2#myanchor',
        /// 'https://www.mywebsite.com'.
        /// </param>
        /// <returns>Requested part. Example: 'mywebapp/mydir/mypage.htm'. <value>null</value> if uri is invalid or part was not found.</returns>
        public static string GetPath(string uri)
        {
            string path = null;
            Regex regex = new Regex(Pattern_Uri);
            Match match = regex.Match(uri);
            if (match.Success)
            {
                path = match.Groups["p0"].Value;
            }
            return path;
        }

        /// <summary>
        /// Gets key/value pairs list.
        /// </summary>
        /// <param name="uri">
        /// Uri from which pairs should be extracted where protocol part must be included. Examples: 
        /// 'http://192.168.0.1:8080/mywebapp/mydir/mypage.htm?mykey1=myvalue1(and sign)mykey2=myvalue2#myanchor',
        /// 'https://www.mywebsite.com'.
        /// </param>
        /// <returns>Requested part. Example: '{(mykey1,myvalue1),(mykey2,myvalue2)}'. <value>null</value> if uri is invalid or part was not found.</returns>
        public static Dictionary<string, string> GetKeyValuePairs(string uri)
        {
            Dictionary<string, string> pairs = null;
            Regex regex = new Regex(Pattern_Uri);
            Match match = regex.Match(uri);
            if (match.Success)
            {
                string query = match.Groups["q0"].Value;
                string[] keyValuePairsArray = query.Split('&');
                
                    pairs = new Dictionary<string, string>();
                    foreach (string keyValuePairString in keyValuePairsArray)
                    {
                        //This should be tested before use:
                        //Regex.Match(keyValuePairString, @"^(?<key>[^?=]+)=(?<value>[^=#]*)&").Groups["key"].Value
                        //Regex.Match(keyValuePairString, @"^(?<key>[^?=]+)=(?<value>[^=#]*)&").Groups["value"].Value
                        pairs.Add(keyValuePairString.Split('=')[0], keyValuePairString.Split('=')[1]);
                    }
                
            }
            return pairs;
        }

        /// <summary>
        /// Gets fragment part.
        /// </summary>
        /// <param name="uri">
        /// Uri from which fragment should be extracted where protocol part must be included. Examples: 
        /// 'http://192.168.0.1:8080/mywebapp/mydir/mypage.htm?mykey1=myvalue1(and sign)mykey2=myvalue2#myanchor',
        /// 'https://www.mywebsite.com'.
        /// </param>
        /// <returns>Requested part. Example: 'myanchor'. <value>null</value> if uri is invalid or part was not found.</returns>
        public static string GetFragment(string uri)
        {
            string fragment = null;
            Regex regex = new Regex(Pattern_Uri);
            Match match = regex.Match(uri);
            if (match.Success)
            {
                fragment = match.Groups["f0"].Value;
            }
            return fragment;
        }

        

    }
    
    //Other regular expression pattern examples:
    //    • Match only alphanumeric characters along with the characters -, +, ., and any whitespace, with the stipulation that there is at least one of these characters and no more than 10 of these characters:

    //    ^([\w\.+-]|\s){1,10}$

    //• Match a date in the form ##/##/#### where the day and month can be a one- or two-digit value, and year can either be a two- or four-digit value:

    //    ^\d{1,2}\/\d{1,2}\/\d{2,4}$

    //• Match a time to be entered with an optional am or pm extension (note that this regular expression also handles military time):

    //    ^\d{1,2}:\d{2}\s?([ap]m)?$

    //• Match an IP address:

    //    ^([0-2]?[0-5]?[0-5]\.){3}[0-2]?[0-5]?[0-5]$

    //• Verify that an email address is in the form name@address where address is not an IP address:

    //    ^[A-Za-z0-9_\-\.]+@(([A-Za-z0-9\-])+\.)+([A-Za-z\-])+$

    //• Verify that an email address is in the form name@address where address is an IP address:

    //    ^[A-Za-z0-9_\-\.]+@([0-2]?[0-5]?[0-5]\.){3}[0-2]?[0-5]?[0-5]$

    //• Match only a dollar amount with the optional $ and + or - preceding characters (note that any number of decimal places may be added):

    //    ^\$?[+-]?[\d,]*(\.\d*)?$

    //This is similar to the previous regular expression except that only up to two decimal places are allowed:

    //    ^\$?[+-]?[\d,]*\.?\d{0,2}$

    //• Match a credit card number to be entered as four sets of four digits separated with a space, -, or no character at all:

    //    ^((\d{4}[- ]?){3}\d{4})$

    //• Match a zip code to be entered either as five digits with an optional four-digit extension:

    //    ^\d{5}(-\d{4})?$

    //• Match a North American phone number with an optional area code and an optional - character to be used in the phone number and no extension:

    //    ^(\(?[0-9]{3}\)?)?\-?[0-9]{3}\-?[0-9]{4}$

    //• Match a phone number similar to the previous regular expression, but allow an optional five-digit extension prefixed with either ext or extension:

    //    ^(\(?[0-9]{3}\)?)?\-?[0-9]{3}\-?[0-9]{4}(\s*ext(ension)?[0-9]{5})?$

    //• Match a full path beginning with the drive letter and optionally match a filename with a three-character extension (note that no .. characters signifying to move up the directory hierarchy are allowed, nor is a directory name with a . followed by an extension):

    //    ^[a-zA-Z]:[\\/]([_a-zA-Z0-9]+[\\/]?)*([_a-zA-Z0-9]+\.[_a-zA-Z0-9]{0,3})?$
}
