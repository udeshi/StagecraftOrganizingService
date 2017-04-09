using StagecraftOrganizingService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace StagecraftOrganizingService.RequestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStagecraftOrganizingService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IStagecraftOrganizingCallbackService))]
    public interface IStagecraftOrganizingClientService
    {
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void Connect();

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void RequestUpdatedSeatList();

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void BookSeats(List<SeatDetails> seatDetailsList);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void DeleteBookedSeat(Int32 userId, SeatDetails seatDetails);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void CheckAvailibility(List<SeatDetails> seatDetailsList);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void RequestMoreSeats(Int32 userId, Int32 noOfSeatsNeeded);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void SendUpdatedList(List<SeatDetails> updatedSeatDetailsList);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void Disconnect();
    }
}
