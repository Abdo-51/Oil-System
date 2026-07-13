using AutoMapper;
using FluentValidation;
using Oil_System.BaseInterface;
using Oil_System.Contract.BaseResponse;
using Oil_System.Contract.Pagination;
using Oil_System.Models;
using Oil_System.Resource.OilPacketDtos;

namespace Oil_System.Service
{
    #region Interfaces
    public interface IOilPacketService
    {
        Task<BaseResponse<PagedResult<OilPacketDto>>> GetAllOilPacketsAsync(SearchOilPacketsRequest search);
        Task<BaseResponse<OilPacketDto>> GetOilPacketByIdAsync(Guid id);
        Task<BaseResponse<bool>> CreateOilPacketAsync(CreateOilPacketsRequest oilPacket);
        Task<BaseResponse<bool>> UpdateOilPacketAsync(UpdateOilPacketsRequest oilPacket);
        Task<BaseResponse<bool>> DeleteOilPacketAsync(Guid id);

    }
    #endregion

    #region Implementations
    public class OilPacketService : ResponseHandler, IOilPacketService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOilPacketsRequest> _createOilPacketValidator;
        private readonly IValidator<UpdateOilPacketsRequest> _updateOilPacketValidator;
        #endregion

        #region Constructor
        public OilPacketService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateOilPacketsRequest> createOilPacketValidator, IValidator<UpdateOilPacketsRequest> updateOilPacketValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createOilPacketValidator = createOilPacketValidator;
            _updateOilPacketValidator = updateOilPacketValidator;
        }

        #endregion

        #region Methods

        public async Task<BaseResponse<PagedResult<OilPacketDto>>> GetAllOilPacketsAsync(SearchOilPacketsRequest search)
        {
            search.AddOilPacketFilter();
            var oilPackets = await _unitOfWork.oilPacketsRepository.GetAllAsync(search);

            if (oilPackets == null || !oilPackets.Items.Any())
            {
                return BadRequest<PagedResult<OilPacketDto>>("No oil packets found.");
            }

            var oilPacketDtos = _mapper.Map<PagedResult<OilPacketDto>>(oilPackets);
            return Success<PagedResult<OilPacketDto>>(oilPacketDtos);
        }

        public async Task<BaseResponse<OilPacketDto>> GetOilPacketByIdAsync(Guid id)
        {
            var oilPacket = await _unitOfWork.oilPacketsRepository.GetByIdAsync(id);
            if (oilPacket == null)
            {
                return BadRequest<OilPacketDto>($"Oil packet with ID {id} not found.");
            }

            var oilPacketDto = _mapper.Map<OilPacketDto>(oilPacket);
            return Success(oilPacketDto);
        }

        public async Task<BaseResponse<bool>> CreateOilPacketAsync(CreateOilPacketsRequest request)
        {
            var validationResult = await _createOilPacketValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest<bool>(string.Join("; ", errors));
            }

            var oilPacketEntity = _mapper.Map<OilPacket>(request);
            await _unitOfWork.oilPacketsRepository.AddAsync(oilPacketEntity);

            var result = await _unitOfWork.CommitChanges();

            if (!result)
            {
                return BadRequest<bool>("Failed to create oil packet.");
            }

            return Success(true);
        }

        public async Task<BaseResponse<bool>> UpdateOilPacketAsync(UpdateOilPacketsRequest request)
        {
            var validationResult = await _updateOilPacketValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest<bool>(string.Join("; ", errors));
            }

            var oilPacketEntity = await _unitOfWork.oilPacketsRepository.GetByIdAsync(request.Id);
            if (oilPacketEntity == null)
            {
                return BadRequest<bool>("Oil packet not found.");
            }

            _mapper.Map(request, oilPacketEntity);
            await _unitOfWork.oilPacketsRepository.UpdateAsync(oilPacketEntity);

            var result = await _unitOfWork.CommitChanges();

            if (!result)
            {
                return BadRequest<bool>("Failed to update oil packet.");
            }

            return Success(true);
        }

        public async Task<BaseResponse<bool>> DeleteOilPacketAsync(Guid id)
        {
            var oilPacketEntity = await _unitOfWork.oilPacketsRepository.GetByIdAsync(id);

            if (oilPacketEntity == null)
            {
                return BadRequest<bool>("Oil packet not found.");
            }

            await _unitOfWork.oilPacketsRepository.DeleteAsync(oilPacketEntity.Id);

            var result = await _unitOfWork.CommitChanges();
            if (!result)
            {
                return BadRequest<bool>("Failed to delete oil packet.");
            }

            return Success(true);
        }

        #endregion
    }
    #endregion
}
