using System;
using System.Collections.Generic;

namespace VisualLogin.MyUtils.MyOther
{
    public static class EvtxUtils
    {
        public static List<EventLogEntry> Read(string filePath)
        {
            List<EventLogEntry> eventLogEntries = new List<EventLogEntry>();
            System.Diagnostics.Eventing.Reader.EventLogReader reader = new System.Diagnostics.Eventing.Reader.EventLogReader(new System.Diagnostics.Eventing.Reader.EventLogQuery(filePath, System.Diagnostics.Eventing.Reader.PathType.FilePath));
            for (var eventRecord = reader.ReadEvent(); eventRecord != null; eventRecord = reader.ReadEvent())
            {
                var entry = new EventLogEntry()
                {
                    RecordId = eventRecord.RecordId ?? 0L,
                    Task = eventRecord.Task ?? 0,
                    Level = eventRecord.LevelDisplayName,
                    TimeCreated = eventRecord.TimeCreated ?? DateTime.MinValue,
                    ProcessId = eventRecord.Properties.Count == 9 ? (uint)eventRecord.Properties[3].Value : (uint)0,
                    QueryName = eventRecord.Properties.Count == 9 ? eventRecord.Properties[4].Value.ToString() : "",
                    QueryResults = eventRecord.Properties.Count == 9 ? eventRecord.Properties[6].Value.ToString() : "",
                    Image = eventRecord.Properties.Count == 9 ? eventRecord.Properties[7].Value.ToString() : "",
                };
                eventLogEntries.Add(entry);
            }
            return eventLogEntries;
        }
    }
    public class EventLogEntry
    {
        public long RecordId { get; set; }
        public int Task { get; set; }
        public string Level { get; set; }
        public DateTime TimeCreated { get; set; }
        public UInt32 ProcessId { get; set; }
        public string QueryName { get; set; }
        public string QueryResults { get; set; }
        public string Image { get; set; }
    }
}