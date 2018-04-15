
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="type">The type of the message.</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs msgEventArgs = new MessageRecievedEventArgs();
            msgEventArgs.Message = message;
            msgEventArgs.Status = type;
            MessageRecieved.Invoke(this, msgEventArgs);
        }
    }
}
