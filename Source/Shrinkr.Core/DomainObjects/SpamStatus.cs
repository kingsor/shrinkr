namespace Shrinkr.DomainObjects
{
    using System;
    using System.ComponentModel;

    [Flags]
    public enum SpamStatus
    {
        None = 0,

        [Description("Bad Word")]
        BadWord = 1,

        Phishing = 2,

        Malware = 4,

        Clean = 8
    }
}