﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResumableFunction.Abstraction.InOuts;

namespace ResumableFunction.Abstraction
{

    /// <summary>
    /// Event provider will listen to events and push events to the engine.<br\>
    /// This may be a client for remote event provider or local implementation.
    /// </summary>
    public interface IEventProvider : IDisposable
    {
        /// <summary>
        /// Uniqe name to resolve this provider at runtime
        /// </summary>
        string UniqueName { get; }

        /// <summary>
        /// Start listening to the events,the engine will call this when an event that expected to be handled by this provider
        /// </summary>
        /// <returns></returns>
        Task Start();

        /// <summary>
        /// Stop listening to the all event types,the engine will call this when no active event exist that handled by this provider
        /// </summary>
        /// <returns></returns>
        Task Stop();

        /// <summary>
        /// The engine call this when Function run and ask to wait an event
        /// </summary>
        /// <param name="eventToSubscribe"></param>
        /// <returns></returns>
        Task<bool> SubscribeToEvent(IEventData eventToSubscribe);

        /// <summary>
        /// The engine will call this if no instance waiting the event type after the event pushed to the engine
        /// </summary>
        /// <param name="eventToSubscribe"></param>
        /// <returns></returns>
        Task<bool> UnSubscribeEvent(IEventData eventToSubscribe);

        ///// <summary>
        ///// Remote Call <br/>
        ///// Push and event to the engine
        ///// </summary>
        //Task PushEvent(PushedEvent pushEvent);
        ////{
        ////    var type = pushEvent.EventData?.GetType();
        ////    if (type == null) return;
        ////    //pushEvent.Type = $"{type.FullName}#{type.AssemblyQualifiedName}";
        ////    pushEvent.EventDataType = type.FullName;
        ////    pushEvent.EventDataConverterName = UniqueName;
        ////    //will use engine gRPC client to push an event
        ////}

    }
}