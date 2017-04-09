using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using StagecraftOrganizingService.DataContracts;
using StagecraftOrganizingService.RequestService.Interface;

namespace StagecraftOrganizingService.RequestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StagecraftOrganizingService" in both code and config file together.
    public class StagecraftOrganizingService : IStagecraftOrganizingService
    {
        private static List<IStagecraftOrganizingCallbackService> _callbackChannels = new List<IStagecraftOrganizingCallbackService>();
        private static List<SeatDetails> _seatingList = new List<SeatDetails>();
        private static List<SeatBookingDetails> _paymentDetialsList = new List<SeatBookingDetails>();
        private static readonly object _sycnRoot = new object();


        public void Connect()
        {
            try
            {
                IStagecraftOrganizingCallbackService callbackChannel =
                    OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingCallbackService>();

                lock (_sycnRoot)
                {
                    if (!_callbackChannels.Contains(callbackChannel))
                    {
                        _callbackChannels.Add(callbackChannel);
                        //Console.WriteLine("Added Callback Channel: {0}", callbackChannel.GetHashCode());
                        //callbackChannel.SendUpdatedSeatingList(_seatingList);
                    }
                }
            }
            catch(Exception ex)
            {
                CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                throw new FaultException<CustomExpMsg>(customMsg, new
                FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
            }
        }

        public void Disconnect()
        {
            IStagecraftOrganizingCallbackService callbackChannel =
                    OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingCallbackService>();

            try
            {
                lock (_sycnRoot)
                {
                    if (_callbackChannels.Remove(callbackChannel))
                    {
                        Console.WriteLine("Removed Callback Channel: {0}", callbackChannel.GetHashCode());
                    }
                }
            }
            catch (Exception ex)
            {
                CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                throw new FaultException<CustomExpMsg>(customMsg, new
                FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
            }
        }

        public void BookSeats(List<SeatDetails> seatDetailsList)
        {
            lock (_sycnRoot)
            {
                try
                {
                    IStagecraftOrganizingCallbackService callbackChannel =
                       OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingCallbackService>();

                    if (!_callbackChannels.Contains(callbackChannel))
                    {
                        _callbackChannels.Add(callbackChannel);
                    }

                        for (Int32 i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {

                            _callbackChannels[i].SendUpdatedSeatingList(seatDetailsList);
                        }
                        catch (Exception ex)
                        {
                            _callbackChannels.RemoveAt(i);
                            CustomExpMsg customMsg = new CustomExpMsg("Service threw exception while communicating on Callback Channel: " + _callbackChannels[i].GetHashCode());
                            throw new FaultException<CustomExpMsg>(customMsg, new
                            FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                        }
                    }
                }
                catch (ApplicationException ex)
                {
                    CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                    throw new FaultException<CustomExpMsg>(customMsg, new
                    FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                }

            }
        }

        public void DeleteBookedSeat(Int32 userId, SeatDetails seatDetails)
        {
            lock (_sycnRoot)
            {
                try
                {
                    IStagecraftOrganizingCallbackService callbackChannel =
                         OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingCallbackService>();

                    if (!_callbackChannels.Contains(callbackChannel))
                    {
                        _callbackChannels.Add(callbackChannel);
                    }

                    for (Int32 i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {
                            _callbackChannels[i].SendBookedSeatDetailsToDelete(userId,seatDetails);

                        }
                        catch (Exception ex)
                        {
                            _callbackChannels.RemoveAt(i);
                            CustomExpMsg customMsg = new CustomExpMsg("Service threw exception while communicating on Callback Channel: " + _callbackChannels[i].GetHashCode());
                            throw new FaultException<CustomExpMsg>(customMsg, new
                            FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                        }
                    }
                }
                catch (ApplicationException ex)
                {
                    CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                    throw new FaultException<CustomExpMsg>(customMsg, new
                    FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                }

            }
        }

        public void RequestMoreSeats(Int32 userId, Int32 noOfSeatsNeeded)
        {

            lock (_sycnRoot)
            {
                try
                {
                    IStagecraftOrganizingCallbackService callbackChannel =
                      OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingCallbackService>();

                    if (!_callbackChannels.Contains(callbackChannel))
                    {
                        _callbackChannels.Add(callbackChannel);
                    }
                    for (Int32 i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {
                            _callbackChannels[i].SendRequestForMoreTickets(userId, noOfSeatsNeeded);
                        }
                        catch (Exception ex)
                        {
                            _callbackChannels.RemoveAt(i);
                            CustomExpMsg customMsg = new CustomExpMsg("Service threw exception while communicating on Callback Channel: " + _callbackChannels[i].GetHashCode());
                            throw new FaultException<CustomExpMsg>(customMsg, new
                            FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                        }
                    }
                }
                catch (ApplicationException ex)
                {
                    CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                    throw new FaultException<CustomExpMsg>(customMsg, new
                    FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                }

            }
        }

        public void RequestUpdatedSeatList()
        {
            lock (_sycnRoot)
            {
                try
                {
                    IStagecraftOrganizingCallbackService callbackChannel =
                         OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingCallbackService>();

                    if (!_callbackChannels.Contains(callbackChannel))
                    {
                        _callbackChannels.Add(callbackChannel);
                    }

                    for (Int32 i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {
                            _callbackChannels[i].NotifyToGetUpdatedSeatingList();
                        }
                        catch (Exception ex)
                        {
                            _callbackChannels.RemoveAt(i);
                            CustomExpMsg customMsg = new CustomExpMsg("Service threw exception while communicating on Callback Channel: " + _callbackChannels[i].GetHashCode());
                            throw new FaultException<CustomExpMsg>(customMsg, new
                            FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                        }
                    }
                }
                catch (ApplicationException ex)
                {
                    CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                    throw new FaultException<CustomExpMsg>(customMsg, new
                    FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                }

            }

        }

        public void CheckAvailibility(List<SeatDetails> seatDetails)
        {
            lock (_sycnRoot)
            {
                try
                {
                    IStagecraftOrganizingCallbackService callbackChannel =
                         OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingCallbackService>();

                    if (!_callbackChannels.Contains(callbackChannel))
                    {
                        _callbackChannels.Add(callbackChannel);
                    }


                    //this.RequestUpdatedSeatList();

                    for (Int32 i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {
                            _callbackChannels[i].NotifyToCheckAvailabiltyOfSeats(seatDetails);
                        }
                        catch (Exception ex)
                        {
                            _callbackChannels.RemoveAt(i);
                            CustomExpMsg customMsg = new CustomExpMsg("Service threw exception while communicating on Callback Channel: " + _callbackChannels[i].GetHashCode());
                            throw new FaultException<CustomExpMsg>(customMsg, new
                            FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                        }
                    }
                }
                catch (ApplicationException ex)
                {
                    CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                    throw new FaultException<CustomExpMsg>(customMsg, new
                    FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                }

            }
        }

      

        public void SendUpdatedList(List<SeatDetails> updatedSeatDetailsList)
        {
            lock (_sycnRoot)
            {
                try
                {
                    //this.RequestUpdatedSeatList();
                    IStagecraftOrganizingCallbackService callbackChannel =
                      OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingCallbackService>();

                    if (!_callbackChannels.Contains(callbackChannel))
                    {
                        _callbackChannels.Add(callbackChannel);
                    }

                    for (Int32 i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {
                            _callbackChannels[i].SendUpdatedSeatingList(updatedSeatDetailsList);
                        }
                        catch (Exception ex)
                        {
                            _callbackChannels.RemoveAt(i);
                            CustomExpMsg customMsg = new CustomExpMsg("Service threw exception while communicating on Callback Channel: " + _callbackChannels[i].GetHashCode());
                            throw new FaultException<CustomExpMsg>(customMsg, new
                            FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                        }
                    }
                }
                catch (ApplicationException ex)
                {
                    CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                    throw new FaultException<CustomExpMsg>(customMsg, new
                    FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                }

            }
        }

        public void SendUpdatedBookingDetailsList(List<SeatBookingDetails> updatedSeatDetailsList)
        {
            lock (_sycnRoot)
            {
                try
                {
                    //this.RequestUpdatedSeatList();
                    IStagecraftOrganizingCallbackService callbackChannel =
                      OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingCallbackService>();

                    if (!_callbackChannels.Contains(callbackChannel))
                    {
                        _callbackChannels.Add(callbackChannel);
                    }

                    for (Int32 i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {
                            _callbackChannels[i].SendUpdatedBookingDetails(updatedSeatDetailsList);
                        }
                        catch (Exception ex)
                        {
                            _callbackChannels.RemoveAt(i);
                            CustomExpMsg customMsg = new CustomExpMsg("Service threw exception while communicating on Callback Channel: " + _callbackChannels[i].GetHashCode());
                            throw new FaultException<CustomExpMsg>(customMsg, new
                            FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                        }
                    }
                }
                catch (ApplicationException ex)
                {
                    CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                    throw new FaultException<CustomExpMsg>(customMsg, new
                    FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                }
            }
        }
    }
}
