using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Categories;
using MusicalStore.Dtos.FeedBacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IFeedBackService
    {
        Task<List<FeedBackDto>> GetAllFeedBack();
        Task<FeedBackDto> GetFeedBackById(Guid id);
        Task<FeedBackDto> GetFeedBackByProductAndUser(Guid ProductId, string UserId);
        Task<ResponseMessage> CreateFeedback(CreateFeedback request);
        Task<ResponseMessage> UpdateFeedBack(UpdateFeedback request);
        Task<ResponseMessage> DeleteFeedBack(Guid id);
    }
}
