﻿using ResumableFunction.Abstraction.InOuts;

namespace ResumableFunction.Abstraction
{
    public interface IFunctionEngine
    {
        /// <summary>
        /// Find all FunctionInstances,EventProviders and EventEventDataConverter and register them all.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        Task RegisterAssembly(string assemblyName);
        Task RegisterFunction<FunctionData>(ResumableFunction<FunctionData> FunctionInstance);
        Task RegisterEventProvider(IEventProvider eventProvider);
        Task RegisterEventEventDataConverter(IEventDataConverter eventDataConverter);

        /// <summary>
        ///pushed event  comes to the engine from event provider <br/>
        ///pushed event contains properties (ProviderName,EventType,EventData)<br/>
        ///engine search active event list with (ProviderName,EventType) and pass payload to match expression<br/>
        ///engine now know related instances list<br/>
        ///load context data and start/resume active instance Function<br/>
        ///call EventProvider.UnSubscribeEvent(pushedEvent.EventData) if no other intances waits this type for the same provider
        /// </summary>
        /// <param name="pushedEvent"></param>
        /// <returns></returns>
        Task WhenEventProviderPushEvent(PushedEvent pushedEvent);

        /// <summary>
        /// Will execueted when a Function instance run and return new EventWaiting result.<br/>
        /// * Find event provider or load it.<br/>
        /// * Start event provider if not started <br/>
        /// * Call SubscribeToEvent with current paylaod type (eventWaiting.EventData)
        /// * Save event to IActiveEventsRepository <br/>
        /// ** todo:important ?? must we send some of SingleEventWaiting props to event provider?? this will make filtering more accurate
        /// but the provider will send this data back
        /// </summary>
        /// <param name="eventWaiting"></param>
        Task FunctionRequestEvent(SingleEventWaiting eventWaiting);
        Task FunctionRequestEvent(AllEventWaiting eventWaiting);
        Task FunctionRequestEvent(AnyEventWaiting eventWaiting);
    }
}