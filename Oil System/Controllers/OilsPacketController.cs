using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oil_System.Contract;
using Oil_System.Resource.OilPacketDtos;
using Oil_System.Service;

namespace Oil_System.Controllers
{
    [Authorize]
    [ApiController]
    public class OilsPacketController : AppControllerBase
    {
        #region Fields
        private readonly IOilPacketService _oilPacketService;
        #endregion

        #region Constructor
        public OilsPacketController(IOilPacketService oilPacketService)
        {
            _oilPacketService = oilPacketService;
        }
        #endregion

        #region Methods
        [HttpPost(ApiRoute.OilsPacket.GetAllOilsPackets)]
        public async Task<IActionResult> GetAllOilsPackets(SearchOilPacketsRequest search)
        {
            var response = await _oilPacketService.GetAllOilPacketsAsync(search);
            return GenericResult(response);
        }

        [HttpPost(ApiRoute.OilsPacket.GetOilsPacketById)]
        public async Task<IActionResult> GetOilsPacketById(Guid id)
        {
            var response = await _oilPacketService.GetOilPacketByIdAsync(id);
            return GenericResult(response);
        }

        [HttpPost(ApiRoute.OilsPacket.CreateOilsPackets)]
        public async Task<IActionResult> CreateOilsPacket(CreateOilPacketsRequest request)
        {
            var response = await _oilPacketService.CreateOilPacketAsync(request);
            return GenericResult(response);
        }

        [HttpPost(ApiRoute.OilsPacket.UpdateOilsPackets)]
        public async Task<IActionResult> UpdateOilsPacket(UpdateOilPacketsRequest request)
        {
            var response = await _oilPacketService.UpdateOilPacketAsync(request);
            return GenericResult(response);
        }

        [HttpPost(ApiRoute.OilsPacket.DeleteOilsPackets)]
        public async Task<IActionResult> DeleteOilsPacket(Guid id)
        {
            var response = await _oilPacketService.DeleteOilPacketAsync(id);
            return GenericResult(response);
        }
        #endregion
    }
}
