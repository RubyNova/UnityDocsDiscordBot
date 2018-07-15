using System;

namespace UnityDocsBot.Models
{
    public class ManualEntryModel
    {
        public string EntryName { get; set; }
        public string Description { get; set; }
        public Uri FullReferenceLink { get; set; }
        public string ManualVersion { get; set; }
    }
}