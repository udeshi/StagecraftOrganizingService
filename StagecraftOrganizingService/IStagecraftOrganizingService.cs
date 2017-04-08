using StagecraftOrganizingService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace StagecraftOrganizingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStagecraftOrganizingService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IStagecraftOrganizingCallbackService))]
    public interface IStagecraftOrganizingService
    {
        [OperationContract(IsOneWay = true)]
        void Connect();

        [OperationContract(IsOneWay = true)]
        void ReserveSeat(SeatDetails seatDetails);

        [OperationContract(IsOneWay = true)]
        void DeleteReservedSeat(SeatDetails seatDetails);

        [OperationContract(IsOneWay = true)]
        void BookSeat(SeatDetails seatDetails);

        [OperationContract(IsOneWay = true)]
        void DeleteBookedSeat(SeatDetails seatDetails);

        [OperationContract(IsOneWay = true)]
        void RequestMoreSeats(Int32 userId);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void Disconnect();
    }
}
