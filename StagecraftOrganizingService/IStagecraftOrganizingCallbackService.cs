using StagecraftOrganizingService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace StagecraftOrganizingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStagecraftOrganizingCallbackService" in both code and config file together.
    [ServiceContract]
    public interface IStagecraftOrganizingCallbackService
    {
        [OperationContract(IsOneWay =true)]
        void SendUpdatedSeatingList(List<SeatDetails> seatingList);

        [OperationContract(IsOneWay = true)]
        void SendDeletedSeatDetails(SeatDetails seatDetails);
        
        [OperationContract(IsOneWay = true)]
        void SendBookedSeatDetailsToDelete(SeatDetails seatDetails);

        [OperationContract(IsOneWay = true)]
        void SendRequestForMoreTickets(Int32 userId);
    }
}
