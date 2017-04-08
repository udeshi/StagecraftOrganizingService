using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using StagecraftOrganizingService.DataContracts;

namespace StagecraftOrganizingService
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
            catch
            {
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
            catch
            {
            }
        }

        public void ReserveSeat(SeatDetails seatDetails)
        {
            lock (_sycnRoot)
            {
                try {
                    var recordsCount = _seatingList.Where(x => x.SeatNo == seatDetails.SeatNo).Count();
                    if (recordsCount == 0)
                    {
                        _seatingList.Add(seatDetails);
                    }
                    else
                    {
                        throw new ApplicationException("Seat is already booked by another customer. Please select another seat.");
                    }

                    for (int i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {

                            _callbackChannels[i].SendUpdatedSeatingList(_seatingList);
                        }
                        catch (Exception ex)
                        {                            _callbackChannels.RemoveAt(i);
                            CustomExpMsg customMsg = new CustomExpMsg("Service threw exception while communicating on Callback Channel: " + _callbackChannels[i].GetHashCode());
                            throw new FaultException<CustomExpMsg>(customMsg, new
                            FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                        }
                    }
                }catch(ApplicationException ex){
                   CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                    throw new FaultException<CustomExpMsg>(customMsg, new
                    FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                }
              
            }
        }

        public void DeleteReservedSeat(SeatDetails seatDetails)
        {
            lock (_sycnRoot)
            {
                try
                {
                    var index = _seatingList.FindIndex(x => x.SeatNo == seatDetails.SeatNo);
                    SeatDetails deleatedSeatDetails = null;
                    if (index > -1)
                    {
                        deleatedSeatDetails = _seatingList.ElementAt(index);
                        _seatingList.RemoveAt(index);
                    }

                    //Console.WriteLine("-- Seating List --");
                    //_seatingList.ForEach(listItem => Console.WriteLine(listItem));
                    //Console.WriteLine("------------------");

                    for (int i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            //Console.WriteLine("Detected Non-Open Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {

                            _callbackChannels[i].SendUpdatedSeatingList(_seatingList);
                            _callbackChannels[i].SendDeletedSeatDetails(deleatedSeatDetails);
                            //Console.WriteLine("Pushed Updated List on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("Service threw exception while communicating on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                            //Console.WriteLine("Exception Type: {0} Description: {1}", ex.GetType(), ex.Message);
                            //_callbackChannels.RemoveAt(i);
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

        public void BookSeat(SeatDetails seatDetails)
        {
            lock (_sycnRoot)
            {
                try
                {
                    var index = _seatingList.FindIndex(x => x.SeatNo == seatDetails.SeatNo);
                    if (index > -1)
                    {
                        _seatingList[index].BgColour= "#e70f0f";
                    }
                    else
                    {
                        CustomExpMsg customMsg = new CustomExpMsg("Seat details doesn't exist");
                        throw new FaultException<CustomExpMsg>(customMsg, new
                        FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                    }


                    //Console.WriteLine("-- Seating List --");
                    //_seatingList.ForEach(listItem => Console.WriteLine(listItem));
                    //Console.WriteLine("------------------");

                    for (int i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            //Console.WriteLine("Detected Non-Open Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {
                            _callbackChannels[i].SendUpdatedSeatingList(_seatingList);
                            //Console.WriteLine("Pushed Updated List on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("Service threw exception while communicating on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                            //Console.WriteLine("Exception Type: {0} Description: {1}", ex.GetType(), ex.Message);
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

        public void DeleteBookedSeat(SeatDetails seatDetails)
        {
            lock (_sycnRoot)
            {
                try
                {
                    var index = _seatingList.FindIndex(x => x.SeatNo == seatDetails.SeatNo);
                    SeatDetails deleatedSeatDetails = null;
                    if (index > -1)
                    {
                        deleatedSeatDetails = _seatingList.ElementAt(index);
                    }
                    else
                    {
                        CustomExpMsg customMsg = new CustomExpMsg("Seat details doesn't exist");
                        throw new FaultException<CustomExpMsg>(customMsg, new
                        FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
                    }


                    //Console.WriteLine("-- Seating List --");
                    //_seatingList.ForEach(listItem => Console.WriteLine(listItem));
                    //Console.WriteLine("------------------");

                    for (int i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            //Console.WriteLine("Detected Non-Open Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {
                            _callbackChannels[i].SendBookedSeatDetailsToDelete(deleatedSeatDetails);
                            //Console.WriteLine("Pushed Updated List on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("Service threw exception while communicating on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                            //Console.WriteLine("Exception Type: {0} Description: {1}", ex.GetType(), ex.Message);
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

        public void RequestMoreSeats(int userId)
        {

            lock (_sycnRoot)
            {
                try
                {

                    for (int i = _callbackChannels.Count - 1; i >= 0; i--)
                    {
                        if (((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                        {
                            //Console.WriteLine("Detected Non-Open Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                            _callbackChannels.RemoveAt(i);
                            continue;
                        }

                        try
                        {
                            _callbackChannels[i].SendRequestForMoreTickets(userId);
                            //Console.WriteLine("Pushed Updated List on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("Service threw exception while communicating on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                            //Console.WriteLine("Exception Type: {0} Description: {1}", ex.GetType(), ex.Message);
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
