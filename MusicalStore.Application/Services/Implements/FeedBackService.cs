using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.FeedBacks;

namespace MusicalStore.Application.Services.Implements
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public FeedBackService(IFeedBackRepository feedBackRepository, IUserRepository userRepository, IProductRepository productRepository, IMapper mapper)
        {
            _feedBackRepository = feedBackRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<FeedBackDto>> GetAllFeedBack()
        {
            var feedbacks = await _feedBackRepository.GetAll().ToListAsync();
            var feedbackDto = _mapper.Map<List<FeedBackDto>>(feedbacks);
            return feedbackDto;
        }

        public async Task<FeedBackDto> GetFeedBackById(Guid id)
        {
            var feedback = await _feedBackRepository.FindById(id);
            var feedbackDto = _mapper.Map<FeedBackDto>(feedback);
            return feedbackDto; 
        }

        public async Task<FeedBackDto> GetFeedBackByProductAndUser(Guid ProductId, string UserId)
        {
            var feedback = await _feedBackRepository.GetFeedBackByProductAndUser(ProductId, UserId);
            var feedbackDto = _mapper.Map<FeedBackDto>(feedback);
            return feedbackDto;
        }

        public async Task<ResponseMessage> CreateFeedback(CreateFeedback request)
        {
            ResponseMessage responseMessage = new();

            var user = await _userRepository.GetUserById(request.Id);
            var product = await _productRepository.FindById(request.ProductID);

            if (user == null || product == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not Found";
            }
            else
            {
                FeedBack feedBack = new FeedBack()
                {
                    FeedBackID = Guid.NewGuid(),
                    Id = request.Id,
                    ProductID = request.ProductID,
                    StarReview = request.StarReview,
                    ContentRated = request.ContentRated,
                    DateCreated = DateTime.Now,
                    CreateBy = request.CreateBy
                };
                var create = await _feedBackRepository.Create(feedBack);
                if (create > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Success";
                }
                else
                {
                    responseMessage.StatusCode = 400;
                    responseMessage.Message = "Fail";
                }
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> UpdateFeedBack(UpdateFeedback request)
        {
            ResponseMessage responseMessage = new();

            var feedback = await _feedBackRepository.FindById(request.FeedBackID);
            if (feedback==null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not Found";
            }
            else
            {
                feedback.StarReview = request.StarReview;
                feedback.ContentRated = request.ContentRated;
                feedback.ModifiedDate = DateTime.Now;
                feedback.UpdateBy = request.UpdateBy;

                var update = await _feedBackRepository.Update(feedback);
                if (update> 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Success";
                }
                else
                {
                    responseMessage.StatusCode = 400;
                    responseMessage.Message = "Fail";
                }
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteFeedBack(Guid id)
        {
            ResponseMessage responseMessage = new();

            var feedback = await _feedBackRepository.FindById(id);
            if (feedback == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not Found";
            }
            else
            {
                var delete = await _feedBackRepository.Delete(id);
                if (delete > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Success";
                }
                else
                {
                    responseMessage.StatusCode = 400;
                    responseMessage.Message = "Fail";
                }
            }

            return responseMessage;
        }

        public async Task<List<FeedBackDto>> GetPaginationFeedBack(int page)
        {
            var feedbacks = await _feedBackRepository.Pagination(page);
            var feedbacksDto = _mapper.Map<List<FeedBackDto>>(feedbacks);
            return feedbacksDto;
        }
    }
}
