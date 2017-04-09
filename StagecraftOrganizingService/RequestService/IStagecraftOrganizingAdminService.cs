using StagecraftOrganizingService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StagecraftOrganizingService.RequestService
{
    [ServiceContract(CallbackContract = typeof(IStagecraftOrganizingAdminCallbackService))]
    public  interface IStagecraftOrganizingAdminService
    {
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void Connect();

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void RequestUpdatedSeatList();

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void SendUpdatedList(List<SeatDetails> updatedSeatDetailsList);

        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(ApplicationException))]
        void Disconnect();
    }
}
