using System.Collections.Generic;
using System.Threading.Tasks;
using Honeycomb.Models;

namespace Honeycomb
{
    public interface IHoneycombService
    {
        /// <summary>
        /// Queue an event to be sent during the next flush
        /// </summary>
        /// <param name="ev">The full event to be pushed</param>
        void QueueEvent(HoneycombEvent ev);

        /// <summary>
        /// Flush all events
        /// </summary>
        /// <returns></returns>
        Task Flush();

        /// <summary>
        /// Send a single event directly
        /// </summary>
        /// <param name="ev">The full event to be pushed</param>
        /// <returns></returns>
        Task SendSingleAsync(HoneycombEvent ev);

        /// <summary>
        /// Send a batch of events directly
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task SendBatchAsync(IEnumerable<HoneycombEvent> items);

        /// <summary>
        /// Send a batch of events to a specific dataset
        /// </summary>
        /// <param name="items"></param>
        /// <param name="dataSetName"></param>
        /// <returns></returns>
        Task SendBatchAsync(IEnumerable<HoneycombEvent> items, string dataSetName);
    }
}