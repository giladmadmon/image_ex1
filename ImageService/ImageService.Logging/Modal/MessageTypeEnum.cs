using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    public enum MessageTypeEnum : int
    {
        INFO = EventLogEntryType.Information,
        WARNING = EventLogEntryType.Warning,
        FAIL = EventLogEntryType.Error
    }
}
