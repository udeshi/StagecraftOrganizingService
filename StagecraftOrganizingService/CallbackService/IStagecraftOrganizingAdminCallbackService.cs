using StagecraftOrganizingService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StagecraftOrganizingService
{
    [ServiceContract]
    public interface IStagecraftOrganizingAdminCallbackService
    {
        [OperationContract(IsOneWay = true)]
        void SendUpdatedSeatingList(List<SeatDetails> seatingList);

        [OperationContract(IsOneWay = true)]
        void NotifyToGetUpdatedSeatingList();

        [OperationContract(IsOneWay = true)]
        void SendAvailabiltyOfSeats(List<SeatDetails> seatDetails);

    }
}
