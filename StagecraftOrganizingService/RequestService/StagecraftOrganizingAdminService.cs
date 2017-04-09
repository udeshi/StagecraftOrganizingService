using StagecraftOrganizingService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StagecraftOrganizingService.RequestService
{
    public class StagecraftOrganizingAdminService:IStagecraftOrganizingAdminService
    {
        private static List<IStagecraftOrganizingAdminCallbackService> _callbackChannels = new List<IStagecraftOrganizingAdminCallbackService>();
        private static List<SeatDetails> _seatingList = new List<SeatDetails>();
        private static List<SeatBookingDetails> _paymentDetialsList = new List<SeatBookingDetails>();
        private static readonly object _sycnRoot = new object();


        public void Connect()
        {
            try
            {
                IStagecraftOrganizingAdminCallbackService callbackChannel =
                    OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingAdminCallbackService>();

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
            catch (Exception ex)
            {
                CustomExpMsg customMsg = new CustomExpMsg(ex.Message);
                throw new FaultException<CustomExpMsg>(customMsg, new
                FaultReason(customMsg.ErrorMsg), new FaultCode("Sender"));
            }
        }

        public void Disconnect()
        {
            IStagecraftOrganizingAdminCallbackService callbackChannel =
                    OperationContext.Current.GetCallbackChannel<IStagecraftOrganizingAdminCallbackService>();

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

        public void RequestUpdatedSeatList()
        {
            lock (_sycnRoot)
            {
                try
                {

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

        public void SendUpdatedList(List<SeatDetails> updatedSeatDetailsList)
        {
            lock (_sycnRoot)
            {
                try
                {
                    this.RequestUpdatedSeatList();

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
    }
}
